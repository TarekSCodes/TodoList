using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoLibrary.Models;

public class CategoryModel
{
    public int Id { get; set; }
    public string CategoryName { get; set; }
    public string CategoryColor { get; set; }
    public List<TodoModel> TodoList { get; set; }
}
