using System;
using TodoLibrary;
using TodoLibrary.Models;

namespace TodoUI;

public partial class TodoViewerForm : Form
{
    public TodoViewerForm()
    {
        InitializeComponent();

        // Hiddes the titlebar
        this.FormBorderStyle = FormBorderStyle.None;
    }

    private void TxtBoxTodoEntry_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            // Prevents the enter tone from sounding
            e.SuppressKeyPress = true;

            if (!String.IsNullOrWhiteSpace(TxtBoxTodoEntry.Text))
            {
                // Create the TodoModel from input
                TodoModel model = new TodoModel(TxtBoxTodoEntry.Text, false);
                GlobalConfig.Connection.CreateTodoModel(model);

                var todoitem = new TodoItemControl
                {
                    Model = model,
                };

                flowLayoutPanelTodos.Controls.Add(todoitem);

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

    private void TodoViewerForm_Load(object sender, EventArgs e)
    {
        LoadTodos();
    }

    private void LoadTodos()
    {
        List<TodoModel> todos = GlobalConfig.Connection.LoadTodosFromFile();

        foreach (TodoModel todo in todos)
        {
            AddTodoToUI(todo);
        }
    }

    private void AddTodoToUI(TodoModel model)
    {
        TodoItemControl todoitem = new TodoItemControl
        {
            Model = model
        };

        flowLayoutPanelTodos.Controls.Add(todoitem);
    }

    private void BtnQuit_Click(object sender, EventArgs e)
    {
        this.Close();
    }
    
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.Tab)
        {
            Control currentControl = this.ActiveControl;

            if (currentControl.TabIndex == 2)
            {
                // Suche nach dem Steuerelement mit `TabIndex` 0
                Control firstControl = this.Controls.Cast<Control>().FirstOrDefault(c => c.TabIndex == 0 && c.CanSelect && c.TabStop);
                if (firstControl != null)
                {
                    firstControl.Focus();
                    return true;
                }
            }
        }

        // Standardverhalten für andere Fälle nutzen
        return base.ProcessCmdKey(ref msg, keyData);
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