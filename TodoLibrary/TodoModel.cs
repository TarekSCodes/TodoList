using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoLibrary;

public class TodoModel
{
    public int Id { get; set; }
    public string TodoContent { get; set; }
    public bool TodoDone { get; set; }
}
