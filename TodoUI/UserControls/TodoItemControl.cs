﻿using TodoLibrary;
using TodoLibrary.Models;

namespace TodoUI;

// TODO - Hinzufügen eines grafischen Trennzeichens im Designer zwischen TodoContent und TodoDone passend zur Kategoriefarbe
public partial class TodoItemControl : UserControl
{
    private bool isChecked = false;
    public event EventHandler? TodoStatusChanged;
    private TodoModel model;

    public TodoModel Model
    {
        get { return model; }
        set
        {
            model = value;
            TodoContent = model.TodoContent;
            TodoDone = model.TodoDone;
        }
    }

    public string TodoContent
    {
        get { return label1.Text; }
        set
        {
            label1.Text = value;
            if (model != null)
            {
                model.TodoContent = value;
            }
        }
    }

    public bool TodoDone
    {
        get { return isChecked; }
        set
        {
            isChecked = value;
            pictureBox1.Image = isChecked ? Properties.Resources.CheckBoxChecked1 : Properties.Resources.CheckBoxUnchecked1;
            label1.Font = isChecked ? new System.Drawing.Font(label1.Font, FontStyle.Strikeout) : new System.Drawing.Font(label1.Font, FontStyle.Bold);
            if (model != null)
            {
                model.TodoDone = isChecked;
            }
        }
    }

    private void pictureBox1_Click(object sender, EventArgs e)
    {
        TodoDone = !TodoDone;
        GlobalConfig.Connection.UpdateTodoDone(this.Model);
        TodoStatusChanged?.Invoke(this, EventArgs.Empty);
    }

    public TodoItemControl()
    {
        InitializeComponent();
    }
}
