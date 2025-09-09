using Spectre.Console;

AnsiConsole.Write(renderable: new FigletText(text: "Schedules")
    .Color(color: Color.Blue)
    .LeftJustified());

AnsiConsole.MarkupLine(value: "[grey]Creates structured JSON files with weekly schedules[/]");