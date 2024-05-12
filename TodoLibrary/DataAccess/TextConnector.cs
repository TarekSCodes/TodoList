﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoLibrary.Models;


namespace TodoLibrary.DataAccess;

public class TextConnector : IDataConnection
{
    private const string TodoFile = "TodoModels.csv";

    public TodoModel CreateTodoModel(TodoModel model)
    {
        // Load the file and convert the text to List<TodoModel>
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

        // Convert the persons to List<string>
        // Save the List<string> to the text file
        todos.SaveToTodoFile(TodoFile);

        return model;
    }

    public void UpdateTodoModel(TodoModel model)
    {
        List<TodoModel> todos = TodoFile.FullFilePath().LoadFile().ConvertToTodoModels();

        var item = todos.FirstOrDefault(x => x.Id == model.Id);
        if (item != null)
        {
            item.TodoDone = model.TodoDone;
        }

        todos.SaveToTodoFile(TodoFile);
    }

    public List<TodoModel> LoadTodosFromFile()
    {
        List<string> lines = TodoFile.FullFilePath().LoadFile();
        return lines.ConvertToTodoModels();
    }
}
