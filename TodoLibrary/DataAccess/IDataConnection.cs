﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoLibrary.Models;

namespace TodoLibrary.DataAccess;

public interface IDataConnection
{
    TodoModel CreateTodoModel(TodoModel todo);

    void UpdateTodoDone(TodoModel todo);

    List<TodoModel> LoadTodosFromFile();

    List<bool> LoadSettingsFromFile();
}
