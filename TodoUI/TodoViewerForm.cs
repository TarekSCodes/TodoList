using System;
using TodoLibrary;
using TodoLibrary.Models;

namespace TodoUI;

public partial class TodoViewerForm : Form
{
    public TodoViewerForm()
    {
        InitializeComponent();

        // Versteckt die Titelleiste
        this.FormBorderStyle = FormBorderStyle.None;
    }

    /// <summary>
    /// Reagiert auf das KeyDown-Ereignis der Textbox für Todo-Einträge. Wenn die Enter-Taste gedrückt wird,
    /// unterdrückt diese Methode das akustische Signal des Tastendrucks und prüft den Text der Textbox auf Inhalte.
    /// Ist der Text nicht leer oder nur Leerzeichen, wird ein neues TodoModel-Objekt erstellt und dem Datenmodell hinzugefügt.
    /// Das zugehörige TodoItemControl wird dann im FlowLayoutPanel hinzugefügt und das Textfeld geleert.
    /// </summary>
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

    /// <summary>
    /// Behandelt das KeyDown-Ereignis für die TodoViewerForm. Wenn die Escape-Taste gedrückt wird,
    /// unterdrückt die Methode die weitere Verarbeitung des Tastendrucks und minimiert das Fenster der Form.
    /// </summary>
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

    /// <summary>
    /// Fügt ein TodoItemControl, das das übergebene TodoModel repräsentiert, zum FlowLayoutPanel hinzu.
    /// Diese Methode erstellt ein neues TodoItemControl-Objekt, setzt dessen Model-Eigenschaft auf das übergebene
    /// TodoModel und fügt es dann zur Steuerelementsammlung des FlowLayoutPanels hinzu. Dies dient der visuellen
    /// Darstellung des Todo-Modells in der Benutzeroberfläche.
    /// </summary>
    /// <param name="model">Das TodoModel, das in der Benutzeroberfläche angezeigt werden soll. Das Modell enthält
    /// die Daten, die im TodoItemControl visualisiert werden.</param>
    private void AddTodoToUI(TodoModel model)
    {
        TodoItemControl todoitem = new TodoItemControl
        {
            Model = model
        };

        flowLayoutPanelTodos.Controls.Add(todoitem);
    }

    /// <summary>
    /// Behandelt das Click-Ereignis des "Quit"-Buttons. Diese Methode schließt das aktuelle Formular,
    /// was typischerweise zum Beenden der Anwendung oder zum Schließen des aktuellen Fensters führt.
    /// </summary>
    private void BtnQuit_Click(object sender, EventArgs e)
    {
        this.Close();
    }

    /// <summary>
    /// Überschreibt die Behandlung von Tastendruck-Ereignissen zur speziellen Handhabung der Tab-Taste. 
    /// Wenn die Tab-Taste gedrückt wird, während das Steuerelement mit dem TabIndex 2 den Fokus hat,
    /// wird der Fokus zum ersten Steuerelement (TabIndex 0) im Formular verschoben, das fokussierbar ist.
    /// Für alle anderen Tastendrücke wird das Standardverhalten genutzt.
    /// </summary>
    /// <param name="msg">Eine Referenz auf die Nachricht, die das Tastendruck-Ereignis repräsentiert.</param>
    /// <param name="keyData">Die Daten der gedrückten Taste, einschließlich der Tab-Taste.</param>
    /// <returns>Gibt true zurück, wenn die Tastenaktion verarbeitet wurde, sonst false, um das Standardverhalten zu ermöglichen.</returns>
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