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
}

#endregion