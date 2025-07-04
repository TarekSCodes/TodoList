using System.Text.Json;
using TodoLibrary.Models;


namespace TodoLibrary.DataAccess;

public class TextConnector : IDataConnection
{
    private const string TodoFile = "TodoModels.json";
    private const string SettingsFile = "Settings.json";

    /// <summary>
    /// Erstellt ein neues TodoModel, weist ihm eine eindeutige ID zu und speichert es in der Textdatei.
    /// </summary>
    /// <param name="model">Das zu erstellende TodoModel.</param>
    /// <returns>Das erstellte TodoModel mit zugewiesener ID.</returns>
    public TodoModel CreateTodoModel(TodoModel model)
    {
        // Lädt die Textdatei und konvertiert den Text in eine List<TodoModel>
        List<TodoModel> todos = TodoFile.FullFilePath().LoadFile().ConvertToTodoModels();

        // Find the max ID
        int currentId = 0;
        if (todos.Count > 0)
        {
            currentId = todos.OrderByDescending(x => x.Id).First().Id + 1;
        }
        model.Id = currentId;

        todos.Add(model);

        todos.SaveToTodoFile(TodoFile);

        return model;
    }

    /// <summary>
    /// Aktualisiert den Status des Todo-Eintrags (ob erledigt oder nicht) basierend auf dem übergebenen TodoModel.
    /// Diese Methode lädt zunächst die Liste aller Todo-Einträge aus einer Datei, sucht den spezifischen Eintrag basierend auf der ID
    /// des übergebenen Modells und aktualisiert dessen 'TodoDone'-Status. Anschließend wird die aktualisierte Liste wieder in der Datei gespeichert.
    /// </summary>
    /// <param name="model">Das TodoModel, das den zu aktualisierenden Eintrag enthält. Es sollte eine gültige ID und den neuen 'TodoDone'-Status enthalten.</param>
    public void UpdateTodoDone(TodoModel model)
    {
        List<TodoModel> todos = TodoFile.FullFilePath().LoadFile().ConvertToTodoModels();

        var item = todos.FirstOrDefault(x => x.Id == model.Id);
        if (item != null)
        {
            item.TodoDone = model.TodoDone;
        }

        todos.SaveToTodoFile(TodoFile);
    }

    public void UpdateSettings(bool topmost, bool hideCompleted)
    {
        TextConnectorHelper.SaveSettingsFile(new SettingsDTO(topmost, hideCompleted), SettingsFile);
    }

    public List<TodoModel> LoadTodosFromFile()
    {
        List<TodoModel> lines = TodoFile.FullFilePath().LoadFile().ConvertToTodoModels();
        return lines;
    }

    public SettingsDTO LoadSettingsFromFile()
    {
        SettingsDTO settings = SettingsFile.FullFilePath().LoadFile().ConvertSettingsFromFileToSettingsDTO();
        return settings;
    }
}
