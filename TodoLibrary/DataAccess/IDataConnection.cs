using TodoLibrary.Models;

namespace TodoLibrary.DataAccess;

public interface IDataConnection
{
    TodoModel CreateTodoModel(TodoModel todo);

    void UpdateTodoDone(TodoModel todo);

    void UpdateSettings(bool topmost, bool hideCompletet);

    public (IEnumerable<TodoModel> filteredTodos, List<bool> shouldHideCompletedTodos) LoadTodosFromFile(bool alwaysOnTopChecked, bool hideCompleted);

    List<bool> LoadSettingsFromFile();
}
