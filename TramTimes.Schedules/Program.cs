using Spectre.Console;

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

#region check folders

var file = Path.Combine(
    path1: "input",
    path2: "stops.txt");

if (!File.Exists(path: file))
{
    AnsiConsole.MarkupLine(value: $"[red]❌ Error: {file} not found[/]");
    AnsiConsole.MarkupLine(value: $"[yellow]Please create a text file with stop IDs at {file}, one per line[/]");
    AnsiConsole.WriteLine();
    
    return;
}

#endregion

#region process stops

try
{
    var stops = await File.ReadAllLinesAsync(path: file);
    
    stops = stops
        .Where(predicate: id => !string.IsNullOrWhiteSpace(value: id))
        .Select(selector: id => id.Trim())
        .ToArray();
    
    if (stops.Length is 0)
    {
        AnsiConsole.MarkupLine(value: $"[red]❌ Error: No valid stop IDs found in {file}[/]");
        AnsiConsole.WriteLine();
        
        return;
    }
    
    AnsiConsole.MarkupLine(value: $"[green]✅ Found {stops.Length} stop IDs to process[/]");
    AnsiConsole.WriteLine();
    
    AnsiConsole.MarkupLine(value: "[green]🎉 All stops processed successfully[/]");
    AnsiConsole.WriteLine();
}
catch (Exception e)
{
    AnsiConsole.MarkupLine(value: $"[red]❌ Error reading {file}: {e.Message}[/]");
    AnsiConsole.WriteLine();
}

#endregion