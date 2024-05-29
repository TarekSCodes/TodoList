using TodoLibrary.Models;

namespace TodoLibrary.DataAccess;

public interface IDataConnection
{
    TodoModel CreateTodoModel(TodoModel todo);

    void UpdateTodoDone(TodoModel todo);

    void UpdateSettings(bool topmost, bool hideCompletet);

    List<TodoModel> LoadTodosFromFile();

    List<bool> LoadSettingsFromFile();
}
