using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

var usingStatements = new List<string>();
var filesLessUsings = new List<string>();
var filesToCombine = Directory.EnumerateFiles("../Baseline.FluentHttpExtensions/")
    .Where(x => x.EndsWith(".cs"))
    .OrderBy(x => x.Contains("HttpRequest.cs") ? 0 : 1)
    .ThenBy(x => x)
    .ToList();

foreach (var file in filesToCombine)
{
    var fileContents = await File.ReadAllLinesAsync(file);
    var fileContentsLessUselessLines = new StringBuilder();
    foreach (var line in fileContents)
    {
        if (line.StartsWith("using"))
        {
            if (!usingStatements.Contains(line))
            {
                usingStatements.Add(line);
            }
            continue;
        }

        if (string.IsNullOrWhiteSpace(line))
        {
            continue;
        }

        fileContentsLessUselessLines.AppendLine(line);
    }
    filesLessUsings.Add(fileContentsLessUselessLines.ToString());
}

var combinedFileContents = new StringBuilder();

combinedFileContents.AppendLine("// -------------------------------------------------------------------------------------------------- //");
combinedFileContents.AppendLine("// THIS IS A SUPER DUPER AUTOMAGICALLY GENERATED FILE. DO NOT EDIT DIRECTLY PLEASE.                   //");
combinedFileContents.AppendLine("// -------------------------------------------------------------------------------------------------- //");
combinedFileContents.AppendLine("// For more information and documentation, visit the Baseline.FluentHttpExtensions Github repository. //");                                        //");
combinedFileContents.AppendLine("// https://github.com/lsymonds/Baseline.FluentHttpExtensions                                          //");
combinedFileContents.AppendLine("// -------------------------------------------------------------------------------------------------- //\n");

foreach (var usingStatement in usingStatements.OrderBy(x => x))
{
    combinedFileContents.AppendLine(usingStatement);
}

combinedFileContents.AppendLine("");

foreach (var fileLessUsings in filesLessUsings)
{
    combinedFileContents.AppendLine(fileLessUsings);
}

await File.WriteAllTextAsync("../../BaselineFluentHttpExtensions.cs", combinedFileContents.ToString());
