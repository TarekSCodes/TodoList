using System;
using TodoLibrary;
using TodoLibrary.Models;

namespace TodoUI;

// TODO - App beim Systemstart oder anmelden starten
// TODO - Tastenkombi zum öffnen und Focusieren des Fenster implementieren
// TODO - Neuste Todos immer oben einfügen -- Erledigt
/* TODO - Kategorien
 * Erstellbar, Löschbar
 * speichern in einer csv Datei
 * Farbe wählbar
 */

/* TODO - Settings
 * Abgehakte Todos ausblenden Ja/Nein Checkbox
 * Tastenkombi zum öffnen selbst vergeben
 * Topmost Ja/Nein
*/

public partial class TodoViewerForm : Form
{

    private readonly NotifyIcon trayIcon;
    private ContextMenuStrip trayMenu;

    public TodoViewerForm()
    {
        InitializeComponent();

        // Erstellen eines ContextMenüs
        trayMenu = new ContextMenuStrip();
        trayMenu.Items.Add("Öffnen", null, OnOpen);
        trayMenu.Items.Add("Beenden", null, BtnQuit_Click);
        
        // Erstellen eines SystemTrayIcons
        trayIcon = new NotifyIcon();
        trayIcon.Text = "Simple Todos";
        trayIcon.Icon = Properties.Resources.Icon;

        trayIcon.ContextMenuStrip = trayMenu;
        trayIcon.Visible = true;

        trayIcon.DoubleClick += OnOpen;
    }

    private void OnOpen(object sender, EventArgs e)
    {
        Show();
        WindowState = FormWindowState.Normal;
        trayIcon.Visible = false;
        ShowInTaskbar = true;
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);

        if (WindowState == FormWindowState.Minimized)
        {
            Hide();
            trayIcon.Visible = true;
        }
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        trayIcon.Visible = false;
        base.OnFormClosing(e);
    }

    /// <summary>
    /// Event-Handler, der auf das Drücken der Enter-Taste in der TxtBoxTodoEntry TextBox reagiert.
    /// Erstellt ein neues TodoModel aus dem Texteingabewert, speichert es, lädt alle Todos neu und leert das Texteingabefeld.
    /// </summary>
    private void TxtBoxTodoEntry_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            // Verhindert den Ton, der beim Drücken der Enter-Taste ertönt
            e.SuppressKeyPress = true;

            if (!String.IsNullOrWhiteSpace(TxtBoxTodoEntry.Text))
            {
                // Erstellt ein neues TodoModel aus dem Texteingabewert
                TodoModel model = new TodoModel(TxtBoxTodoEntry.Text, false);
                GlobalConfig.Connection.CreateTodoModel(model);

                // Löscht alle aktuellen Einträge und lädt sie neu
                flowLayoutPanelTodos.Controls.Clear();
                LoadTodos();

                // Leert das Texteingabefeld
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
        else if (e.Control && e.KeyCode == Keys.U)
        {
            this.WindowState = FormWindowState.Normal;
        }
    }

    private void TodoViewerForm_Load(object sender, EventArgs e)
    {
        LoadTodos();
    }

    /// <summary>
    /// Lädt alle TodoModel-Einträge aus der Datei, kehrt deren Reihenfolge um und fügt sie der Benutzeroberfläche hinzu.
    /// </summary>
    private void LoadTodos()
    {
        // Lädt alle TodoModel-Einträge aus der Datei
        List<TodoModel> todos = GlobalConfig.Connection.LoadTodosFromFile();

        // Kehrt die Reihenfolge der Liste um
        todos.Reverse();

        // Fügt jeden TodoModel-Eintrag der Benutzeroberfläche hinzu
        foreach (TodoModel todo in todos)
        {
            AddTodoToUI(todo);
        }
    }

    /// <summary>
    /// Fügt ein TodoModel der Benutzeroberfläche als TodoItemControl hinzu.
    /// </summary>
    /// <param name="model">Das TodoModel, das hinzugefügt werden soll.</param>
    private void AddTodoToUI(TodoModel model)
    {
        // Erstellt ein neues TodoItemControl und weist ihm das übergebene TodoModel zu
        TodoItemControl todoitem = new TodoItemControl
        {
            Model = model
        };

        // Fügt das TodoItemControl dem flowLayoutPanelTodos hinzu
        flowLayoutPanelTodos.Controls.Add(todoitem);
    }

    /// <summary>
    /// Behandelt das Click-Ereignis des "Quit"-Buttons. Diese Methode schließt das aktuelle Formular,
    /// was typischerweise zum Beenden der Anwendung oder zum Schließen des aktuellen Fensters führt.
    /// </summary>
    private void BtnQuit_Click(object sender, EventArgs e)
    {
        trayIcon.Visible = false;
        Application.Exit();
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