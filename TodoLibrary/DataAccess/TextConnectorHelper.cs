using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoLibrary.Models;

namespace TodoLibrary.DataAccess;

public static class TextConnectorHelper
{
    private static readonly IConfiguration Configuration;

    static TextConnectorHelper()
    {
        Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
    }

    public static string FullFilePath(this string filename)
    {
        Console.WriteLine($"{Configuration["filePath"]}\\{filename}");
        return $"{Configuration["filePath"]}\\{filename}";
    }

    public static List<string> LoadFile(this string file)
    {
        if (!File.Exists(file))
        {
            return new List<string>();
        }

        return File.ReadLines(file).ToList();
    }

    public static List<TodoModel> ConvertToTodoModels(this List<string> lines)
    {
        List<TodoModel> output = new List<TodoModel>();

        foreach (string line in lines)
        {
            string[] cols = line.Split('~');

            TodoModel todo = new TodoModel();
            todo.Id = int.Parse(cols[0]);
            todo.TodoContent = cols[1];
            todo.TodoDone = Convert.ToBoolean(cols[2]);

            output.Add(todo);
        }
        return output;
    }

    public static void SaveToTodoFile(this List<TodoModel> models, string fileName)
    {
        List<string> lines = new List<string>();

        foreach (TodoModel model in models)
        {
            lines.Add($"{model.Id}~{model.TodoContent}~{model.TodoDone}");
        }

        File.WriteAllLines(fileName.FullFilePath(), lines);
    }
}
