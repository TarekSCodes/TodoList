using Microsoft.Extensions.Configuration;
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

    /// <summary>
    /// Ergänzt den angegebenen Dateinamen um den Basispfad, der in der TextConnector Klasse gespeichert ist.
    /// Dies führt zu einem vollständigen Dateipfad, der für Dateioperationen verwendet werden kann.
    /// </summary>
    /// <param name="filename">Der Dateiname, der um den Basispfad erweitert wird.</param>
    /// <returns>Den vollständigen Dateipfad, bestehend aus Basispfad und Dateinamen.</returns>
    public static string FullFilePath(this string filename)
    {
        return $"{Configuration["filePath"]}\\{filename}";
    }

    /// <summary>
    /// Lädt den Inhalt einer Datei und gibt ihn als Liste von Zeilen zurück.
    /// Wenn die angegebene Datei nicht existiert, wird eine leere Liste zurückgegeben.
    /// Diese Methode ermöglicht es, den Dateiinhalt einfach zu lesen und für weitere Verarbeitungen zu nutzen.
    /// </summary>
    /// <param name="file">Der Pfad zur Datei, deren Inhalt geladen werden soll.</param>
    /// <returns>Eine Liste von Zeilen (Strings) aus der Datei. Wenn die Datei nicht existiert, wird eine leere Liste zurückgegeben.</returns>
    public static List<string> LoadFile(this string file)
    {
        if (!File.Exists(file))
        {
            return new List<string>();
        }


        return File.ReadLines(file).ToList();
    }

    /// <summary>
    /// Legt für jede Zeile in der Datei ein TodoModel an und gibt gibt diese in einer List<TodoModel> zurück.
    /// </summary>
    /// <param name="lines"></param>
    /// <returns>List<TodoModel></returns>
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

    public static List<bool> ConvertSettingsFromFileToBool(this List<string> lines)
    {
        List<bool> output = new List<bool>();

        foreach (string line in lines)
        {
            output.Add(Convert.ToBoolean(line));
        }
        return output;
    }

    /// <summary>
    /// Speichert eine Liste von TodoModel-Objekten in eine Datei. Jedes TodoModel wird in eine Zeile umgewandelt,
    /// wobei die Eigenschaften des Modells durch Tilden ('~') getrennt werden.
    /// </summary>
    /// <param name="models">Die Liste von TodoModel-Objekten, die gespeichert werden sollen.</param>
    /// <param name="fileName">Der Dateiname, unter dem die Liste gespeichert wird. Der Pfad wird durch die Methode FullFilePath erweitert.</param>
    public static void SaveToTodoFile(this List<TodoModel> models, string fileName)
    {
        List<string> lines = new List<string>();

        foreach (TodoModel model in models)
        {
            lines.Add($"{model.Id}~{model.TodoContent}~{model.TodoDone}");
        }

        File.WriteAllLines(fileName.FullFilePath(), lines);
    }

    public static void SaveSettingsFile(this List<string> settings, string fileName)
    {
        File.WriteAllLines(fileName.FullFilePath(), settings);
    }

    

}
