using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoLibrary.Models;

public class TodoModel
{
    public int Id { get; set; }
    public string TodoContent { get; set; }
    public bool TodoDone { get; set; }

    public TodoModel() { }

    public TodoModel(string todoContent, bool todoDone)
    {
        TodoContent = todoContent;
        TodoDone = todoDone;
    }
}
