using Spectre.Console;
using TramTimes.Schedules.Builders;
using TramTimes.Schedules.Tools;

var random = new Random();

#region write banner

AnsiConsole.Write(renderable: new FigletText(text: "Schedules")
    .Color(color: Color.Blue)
    .LeftJustified());

#endregion

#region write description

AnsiConsole.MarkupLine(value: "[grey]Creates structured JSON files with weekly schedules for tram stops[/]");
AnsiConsole.WriteLine();

#endregion

#region create folders

Directory.CreateDirectory(path: "input");
Directory.CreateDirectory(path: "output");

#endregion

#region set path

var path = Path.Combine(
    path1: "input",
    path2: "stops.txt");

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

AnsiConsole.Write(renderable: new Panel(text: $"[bold]Target:[/] [yellow]{targetDate:yyyy-MM-dd} ({targetDate.DayOfWeek})[/]")
    .Border(border: BoxBorder.Rounded)
    .BorderColor(color: Color.Blue));

#endregion

#region write progress

await AnsiConsole
    .Progress()
    .StartAsync(action: async context =>
    {
        var task = context.AddTask(
            description: "[blue]Processing all stops[/]",
            maxValue: stops.Length);
        
        foreach (var stop in stops)
        {
            task.Description = $"[yellow]⏱️  {stop}[/]";
            
            await ScheduleBuilder.BuildWeekAsync(
                stop: stop,
                targetDate: targetDate,
                context: context);
            
            task.Increment(value: 1);
            
            await Task.Delay(delay: TimeSpanTools.GetJitteredDelay(
                baseDelay: TimeSpan.FromMilliseconds(milliseconds: 1000),
                jitterRandom: random));
        }
        
        task.Description = $"[green]✅ {stops[^1]}[/]";
        task.StopTask();
    });

#endregion

#region write completion

AnsiConsole.MarkupLine(value: "[green]🎉 All stops IDs processed successfully[/]");
AnsiConsole.WriteLine();

#endregion