﻿using TodoLibrary.Models;


namespace TodoLibrary.DataAccess;

public class TextConnector : IDataConnection
{
    private const string TodoFile = "TodoModels.csv";
    private const string SettingsFile = "Settings.csv";

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

        // Add the new record with the new ID (max + 1)
        todos.Add(model);

        // Konvertiert das TodoModel in List<string>
        // Speichert die List<string> in die Textdatei
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
        List<string> settings = new List<string>();

        settings.Add(Convert.ToString(topmost));
        settings.Add(Convert.ToString(hideCompleted));

        settings.SaveSettingsFile(SettingsFile);
    }

    // Was brauche ich
    // - bool von Loadsettings
    public IEnumerable<TodoModel> LoadTodosFromFile(bool alwaysOnTopChecked, bool hideCompleted)
    {
        // Lädt alle TodoModel-Einträge aus der Datei und kehrt die Reihenfolge der Liste um
        List<TodoModel> todos = TodoFile.FullFilePath().LoadFile().ConvertToTodoModels();
        //return lines.ConvertToTodoModels();
        todos.Reverse();

        // Lädt die Einstellung, ob abgeschlossene Todos ausgeblendet werden sollen und ob Topmost = true ist
        List<bool> shouldHideCompletedTodos = LoadSettings(alwaysOnTopChecked, hideCompleted);

        // Filtert die Todos basierend auf der Einstellung
        IEnumerable<TodoModel> filteredTodos = FilterTodos(todos, shouldHideCompletedTodos[1]);

        return filteredTodos;
    }

    public List<bool> LoadSettingsFromFile()
    {
        List<string> lines = SettingsFile.FullFilePath().LoadFile();
        return lines.ConvertSettingsFromFileToBool();
    }

    private List<bool> LoadSettings(bool alwaysOnTopChecked, bool hideCompleted)
    {
        List<bool> output = new List<bool>();

        List<bool> settings = GlobalConfig.Connection.LoadSettingsFromFile();

        bool topmost = settings.FirstOrDefault(false);
        if (topmost.Equals(true))
            alwaysOnTopChecked = true;

        output.Add(alwaysOnTopChecked);
        // ----------------- //

        bool HideCompleted = settings.LastOrDefault(false);
        if (HideCompleted)
            hideCompleted = true;
        
        output.Add(hideCompleted);

        return output;
    }

    private static IEnumerable<TodoModel> FilterTodos(IEnumerable<TodoModel> todos, bool shouldHideCompletedTodos)
    {
        return shouldHideCompletedTodos ? todos.Where(todo => !todo.TodoDone) : todos;
    }
}
