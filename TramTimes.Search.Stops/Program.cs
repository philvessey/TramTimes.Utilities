using Spectre.Console;
using TramTimes.Search.Stops.Builders;
using TramTimes.Search.Stops.Extensions;
using TramTimes.Search.Stops.Tools;

var random = new Random();

#region listen cancel

Console.CancelKeyPress += (_, _) =>
{
    Console.Clear();
    Console.CursorVisible = true;
};

#endregion

#region write banner

AnsiConsole.Write(renderable: new FigletText(text: "TramTimes")
    .Color(color: Color.Blue)
    .LeftJustified());

#endregion

#region write description

AnsiConsole.MarkupLine(value: "[grey]Generates C# quartz jobs from a template for tram stops[/]");
AnsiConsole.WriteLine();

#endregion

#region build data

var data = Directory.GetFiles(
    searchPattern: "*.txt",
    path: "Data");

#endregion

#region check data

if (data.Length is 0)
{
    AnsiConsole.MarkupLine(value: "[red]❌ Error: No data files found in Data directory[/]");
    AnsiConsole.MarkupLine(value: "[yellow]Please create a text file with stop ids in the Data directory, one per line[/]");
    AnsiConsole.WriteLine();
    
    return;
}

#endregion

#region write prompt

var file = AnsiConsole.Prompt(prompt: new SelectionPrompt<string>()
    .Title(title: "[bold]Select network:[/]")
    .AddChoices(choices: data
        .Select(selector: Path.GetFileName)
        .OrderBy(keySelector: name => name)
        .Cast<string>()
        .ToArray())
);

#endregion

#region build path

var path = Path.Combine(
    path1: "Data",
    path2: file);

#endregion

#region check path

if (!File.Exists(path: path))
{
    AnsiConsole.MarkupLine(value: $"[red]❌ Error: {path} not found[/]");
    AnsiConsole.MarkupLine(value: $"[yellow]Please create a text file with stop ids at {path}, one per line[/]");
    AnsiConsole.WriteLine();
    
    return;
}

#endregion

#region build stops

var stops = await StopBuilder.BuildAsync(path: path);

#endregion

#region check stops

if (stops.Length is 0)
{
    AnsiConsole.MarkupLine(value: $"[red]❌ Error: No valid stop ids found in {path}[/]");
    AnsiConsole.MarkupLine(value: "[yellow]Please ensure the file contains valid stop ids, one per line[/]");
    AnsiConsole.WriteLine();
    
    return;
}

#endregion

#region write prompt

var term = AnsiConsole.Prompt(prompt: new TextPrompt<string>(prompt: "[bold]Enter search term (or leave blank for all):[/]")
    .ValidationErrorMessage(message: "[red]❌ Error: Invalid search term[/]")
    .AllowEmpty());

AnsiConsole.Cursor.MoveUp(steps: 1);
AnsiConsole.WriteLine(value: new string(c: ' ', count: Console.WindowWidth));
AnsiConsole.Cursor.MoveUp(steps: 1);

#endregion

#region build choices

var filtered = !string.IsNullOrEmpty(value: term)
    ? stops.Where(predicate: id => id.ContainsIgnoreCase(value: term))
    : stops;

var choices = filtered as string[] ?? filtered.ToArray();

#endregion

#region check choices

if (choices.Length is 0)
{
    AnsiConsole.MarkupLine(value: $"[red]❌ Error: No stop ids found matching '{term}'[/]");
    AnsiConsole.MarkupLine(value: "[yellow]Please try a different search term or leave blank to select all[/]");
    AnsiConsole.WriteLine();
    
    return;
}

#endregion

#region write prompt

var prompt = new MultiSelectionPrompt<string>()
    .Title(title: "[bold]Select stop ids to process:[/]")
    .AddChoices(choices: choices)
    .WrapAround();

if (string.IsNullOrEmpty(value: term))
    foreach (var item in choices.Reverse())
        prompt.Select(item: item);

#endregion

#region build stops

stops = AnsiConsole.Prompt(prompt: prompt)
    .OrderBy(keySelector: id => id)
    .ToArray();

AnsiConsole.MarkupLine(value: $"[green]✅ Found {stops.Length} stop ids to process[/]");
AnsiConsole.WriteLine();

#endregion

#region check stops

if (stops.Length is 0)
{
    AnsiConsole.MarkupLine(value: "[red]❌ Error: No stop ids selected to process[/]");
    AnsiConsole.MarkupLine(value: "[yellow]Please select at least one stop id to process[/]");
    AnsiConsole.WriteLine();
    
    return;
}

#endregion

#region write header

var targetDate = DateTimeTools.GetTargetDate(now: DateTime.Now);

AnsiConsole.Write(renderable: new Panel(text: $"[bold]Target date:[/] [yellow]{targetDate:yyyy-MM-dd} ({targetDate.DayOfWeek})[/]")
    .Border(border: BoxBorder.Rounded)
    .BorderColor(color: Color.Blue));

#endregion

#region build output

Directory.CreateDirectory(path: "output");

#endregion

#region check output

if (!Directory.Exists(path: "output"))
{
    AnsiConsole.MarkupLine(value: "[red]❌ Error: Could not create output directory[/]");
    AnsiConsole.MarkupLine(value: "[yellow]Please ensure you have permission to create directories in the application folder[/]");
    AnsiConsole.WriteLine();
    
    return;
}

#endregion

#region clean output

foreach (var item in Directory.GetFiles(path: "output"))
    File.Delete(path: item);

#endregion

#region write progress

await AnsiConsole
    .Progress()
    .StartAsync(action: async context =>
    {
        var task = context.AddTask(
            description: "[blue]Processing selected stop ids[/]",
            maxValue: stops.Length);
        
        var template = await File.ReadAllTextAsync(path: Path.Combine(
            path1: "Template",
            path2: Path.GetFileName(path: path)));
        
        template = template.Replace(
            oldValue: "{{NETWORK}}",
            newValue: Path.GetFileNameWithoutExtension(path: path));
        
        foreach (var stop in stops)
        {
            task.Description = $"[yellow]⏱️  {stop}[/]";
            
            await File.WriteAllTextAsync(
                path: Path.Combine(
                    path1: "output",
                    path2: $"_{stop}.cs"),
                contents: template.Replace(
                    oldValue: "{{STOP}}",
                    newValue: stop));
            
            task.Increment(value: 1);
            
            await Task.Delay(delay: TimeSpanTools.GetJitteredDelay(
                baseDelay: TimeSpan.FromMilliseconds(milliseconds: 1000),
                jitterRandom: random));
        }
        
        task.Description = $"[green]✅ {stops[^1]}[/]";
        task.StopTask();
    });

#endregion