using Spectre.Console;
using TramTimes.Database.Stops.Builders;
using TramTimes.Database.Stops.Tools;

var random = new Random();

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
    AnsiConsole.MarkupLine(value: "[yellow]Please create a text file with stop IDs in the Data directory, one per line[/]");
    AnsiConsole.WriteLine();
    
    return;
}

#endregion

#region write prompt

var file = AnsiConsole.Prompt(prompt: new SelectionPrompt<string>()
    .Title(title: "[bold]Select Network:[/]")
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
    AnsiConsole.MarkupLine(value: $"[yellow]Please create a text file with stop IDs at {path}, one per line[/]");
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
    AnsiConsole.MarkupLine(value: $"[red]❌ Error: No valid stop IDs found in {path}[/]");
    AnsiConsole.WriteLine();
    
    return;
}

AnsiConsole.MarkupLine(value: $"[green]✅ Found {stops.Length} stop IDs to process[/]");
AnsiConsole.WriteLine();

#endregion

#region write header

var targetDate = DateTimeTools.GetTargetDate(now: DateTime.Now);

AnsiConsole.Write(renderable: new Panel(text: $"[bold]Target Date:[/] [yellow]{targetDate:yyyy-MM-dd} ({targetDate.DayOfWeek})[/]")
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
    AnsiConsole.WriteLine();
    
    return;
}

#endregion

#region write progress

await AnsiConsole
    .Progress()
    .StartAsync(action: async context =>
    {
        var task = context.AddTask(
            description: "[blue]Processing all stops[/]",
            maxValue: stops.Length);
        
        var template = await File.ReadAllTextAsync(path: Path.Combine(
            path1: "Template",
            path2: Path.GetFileName(path: path)));
        
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