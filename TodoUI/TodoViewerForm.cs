using TodoLibrary;
using TodoLibrary.Models;

namespace TodoUI;

public partial class TodoViewerForm : Form
{
    public TodoViewerForm()
    {
        InitializeComponent();

        // Hiddes the titlebar
        //this.FormBorderStyle = FormBorderStyle.None;
    }

    private void TxtBoxTodoEntry_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            // Prevents the enter tone from sounding
            e.SuppressKeyPress = true;

            if (TxtBoxTodoEntry.Text != "")
            {
                var todoitem = new TodoItemControl
                {
                    TodoContent = TxtBoxTodoEntry.Text,
                    TodoDone = false
                };

                flowLayoutPanelTodos.Controls.Add(todoitem);

                TodoModel testmodel = new TodoModel(
                    TxtBoxTodoEntry.Text,
                    false);

                GlobalConfig.Connection.CreateTodoModel(testmodel);

                TxtBoxTodoEntry.Text = "";
            }
        }
    }

    private void TodoViewerForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape)
        {
            e.SuppressKeyPress = true;
            this.WindowState = FormWindowState.Minimized;
        }
    }

    // TODO - Löschen des Todos beim Rechtklicken

    //private void chkListBoxTodos_MouseDown(object sender, MouseEventArgs e)
    //{
    //    if (e.Button == MouseButtons.Right)
    //    {
    //        int index = chkListBoxTodos.IndexFromPoint(e.Location);
    //
    //        if (index != ListBox.NoMatches)
    //        {
    //            chkListBoxTodos.Items.RemoveAt(index);
    //        }
    //    }
    //}
}