using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Text.Json;
using TodoLibrary.Models;


namespace TodoLibrary.DataAccess;


public static class TextConnectorHelper
{
    private static readonly IConfiguration Configuration;
    private static SettingsDTO? settingsDTO;

    static TextConnectorHelper()
    {
        Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        TextConnectorHelper.EnsureSettingsFileExists("Settings.json");
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
    /// Lädt den Inhalt einer Datei und gibt ihn als String zurück.
    /// Wenn die angegebene Datei nicht existiert, wird ein leerer String zurückgegeben.
    /// Diese Methode ermöglicht es, den Dateiinhalt einfach zu lesen und für weitere Verarbeitungen zu nutzen.
    /// </summary>
    /// <param name="file">Der Pfad zur Datei, deren Inhalt geladen werden soll.</param>
    /// <returns>Den gesamten Inhalt der Datei als String. Wenn die Datei nicht existiert, wird ein leerer String zurückgegeben.</returns>
    public static string LoadFile(this string file)
    {
        if (!File.Exists(file))
        {
            return string.Empty;
        }

        return File.ReadAllText(file);
    }

    /// <summary>
    /// Legt für jede Zeile in der Datei ein TodoModel an und gibt gibt diese in einer List<TodoModel> zurück.
    /// </summary>
    /// <param name="lines"></param>
    /// <returns>List<TodoModel></returns>
    public static List<TodoModel> ConvertToTodoModels(this string jsonString)
    {
        if (String.IsNullOrEmpty(jsonString))
        {
            return new List<TodoModel>();
        }

        try
        {
            List<TodoModel>? todoList = JsonSerializer.Deserialize<List<TodoModel>>(jsonString);
            return todoList ?? new List<TodoModel>();
        }
        catch (JsonException e)
        {
            Debug.WriteLine($"JsonString konnte nicht in ein TodoModel Convertiert werden {e}");
            return new List<TodoModel>();
        }
    }

    public static SettingsDTO ConvertSettingsFromFileToSettingsDTO(this string settings)
    {
        if (string.IsNullOrEmpty(settings))
        {
            settingsDTO = new SettingsDTO(false, false);
            return settingsDTO;
        }

        try
        {
            settingsDTO = JsonSerializer.Deserialize<SettingsDTO>(settings);
            if (settingsDTO == null)
                settingsDTO = new SettingsDTO(false, false);
            return settingsDTO;
        }
        catch (JsonException e)
        {
            Debug.WriteLine($"Settings Json String konnte nicht in ein SettingsDTO Covertiert werden: {e.Message}");
            return settingsDTO = new SettingsDTO(false, false);
        }
    }

    /// <summary>
    /// Speichert eine Liste von TodoModel-Objekten in einer Json-Datei. Jedes TodoModel wird in ein Json-Objekt umgewandelt.
    /// </summary>
    /// <param name="models">Die Liste von TodoModel-Objekten, die gespeichert werden sollen.</param>
    /// <param name="fileName">Der Dateiname, unter dem die Liste gespeichert wird. Der Pfad wird durch die Methode FullFilePath erweitert.</param>
    public static void SaveToTodoFile(this List<TodoModel> models, string fileName)
    {
        JsonSerializerOptions options = new JsonSerializerOptions();
        options.WriteIndented = true;
        string jsonString = JsonSerializer.Serialize(models, options);
        File.WriteAllText(fileName.FullFilePath(), jsonString);
    }

    public static void SaveSettingsFile(SettingsDTO settings, string fileName)
    {
        string jsonString = JsonSerializer.Serialize<SettingsDTO>(settings);
        Debug.WriteLine($"Json String in SaveSettingsFile {jsonString}");
        File.WriteAllText(fileName.FullFilePath(), jsonString);
    }

    /// <summary>
    /// Stellt sicher, dass die Einstellungsdatei existiert. Wenn sie nicht existiert, wird eine neue Datei mit Standardwerten erstellt.
    /// </summary>
    /// <param name="fileName">Der Dateiname der Einstellungsdatei.</param>
    public static void EnsureSettingsFileExists(string fileName)
    {
        string fullPath = fileName.FullFilePath();
        if (!File.Exists(fullPath))
        {
            var defaultSettings = new SettingsDTO(false, false);
            string jsonString = JsonSerializer.Serialize<SettingsDTO>(defaultSettings);
            File.WriteAllText(fullPath, jsonString);
        }
    }
}
