using System;
using TodoLibrary;
using TodoLibrary.Models;

namespace TodoUI;

// TODO - App beim Systemstart oder anmelden starten
// TODO - Tastenkombi zum �ffnen und Focusieren des Fenster implementieren
// TODO - Neuste Todos immer oben einf�gen -- Erledigt
/* TODO - Kategorien
 * Erstellbar, L�schbar
 * speichern in einer csv Datei
 * Farbe w�hlbar
 */

/* TODO - Settings
 * Abgehakte Todos ausblenden Ja/Nein Checkbox
 * Tastenkombi zum �ffnen selbst vergeben
 * Topmost Ja/Nein
*/

public partial class TodoViewerForm : Form
{

    private readonly NotifyIcon trayIcon;
    private ContextMenuStrip trayMenu;

    public TodoViewerForm()
    {
        InitializeComponent();

        // Erstellen eines ContextMen�s
        trayMenu = new ContextMenuStrip();
        trayMenu.Items.Add("�ffnen", null, OnOpen);
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
    /// Event-Handler, der auf das Dr�cken der Enter-Taste in der TxtBoxTodoEntry TextBox reagiert.
    /// Erstellt ein neues TodoModel aus dem Texteingabewert, speichert es, l�dt alle Todos neu und leert das Texteingabefeld.
    /// </summary>
    private void TxtBoxTodoEntry_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            // Verhindert den Ton, der beim Dr�cken der Enter-Taste ert�nt
            e.SuppressKeyPress = true;

            if (!String.IsNullOrWhiteSpace(TxtBoxTodoEntry.Text))
            {
                // Erstellt ein neues TodoModel aus dem Texteingabewert
                TodoModel model = new TodoModel(TxtBoxTodoEntry.Text, false);
                GlobalConfig.Connection.CreateTodoModel(model);

                // L�scht alle aktuellen Eintr�ge und l�dt sie neu
                flowLayoutPanelTodos.Controls.Clear();
                LoadTodos();

                // Leert das Texteingabefeld
                TxtBoxTodoEntry.Text = "";
            }
        }
    }

    /// <summary>
    /// Behandelt das KeyDown-Ereignis f�r die TodoViewerForm. Wenn die Escape-Taste gedr�ckt wird,
    /// unterdr�ckt die Methode die weitere Verarbeitung des Tastendrucks und minimiert das Fenster der Form.
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
    /// L�dt alle TodoModel-Eintr�ge aus der Datei, kehrt deren Reihenfolge um und f�gt sie der Benutzeroberfl�che hinzu.
    /// </summary>
    private void LoadTodos()
    {
        // L�dt alle TodoModel-Eintr�ge aus der Datei
        List<TodoModel> todos = GlobalConfig.Connection.LoadTodosFromFile();

        // Kehrt die Reihenfolge der Liste um
        todos.Reverse();

        // F�gt jeden TodoModel-Eintrag der Benutzeroberfl�che hinzu
        foreach (TodoModel todo in todos)
        {
            AddTodoToUI(todo);
        }
    }

    /// <summary>
    /// F�gt ein TodoModel der Benutzeroberfl�che als TodoItemControl hinzu.
    /// </summary>
    /// <param name="model">Das TodoModel, das hinzugef�gt werden soll.</param>
    private void AddTodoToUI(TodoModel model)
    {
        // Erstellt ein neues TodoItemControl und weist ihm das �bergebene TodoModel zu
        TodoItemControl todoitem = new TodoItemControl
        {
            Model = model
        };

        // F�gt das TodoItemControl dem flowLayoutPanelTodos hinzu
        flowLayoutPanelTodos.Controls.Add(todoitem);
    }

    /// <summary>
    /// Behandelt das Click-Ereignis des "Quit"-Buttons. Diese Methode schlie�t das aktuelle Formular,
    /// was typischerweise zum Beenden der Anwendung oder zum Schlie�en des aktuellen Fensters f�hrt.
    /// </summary>
    private void BtnQuit_Click(object sender, EventArgs e)
    {
        trayIcon.Visible = false;
        Application.Exit();
    }

    /// <summary>
    /// �berschreibt die Behandlung von Tastendruck-Ereignissen zur speziellen Handhabung der Tab-Taste. 
    /// Wenn die Tab-Taste gedr�ckt wird, w�hrend das Steuerelement mit dem TabIndex 2 den Fokus hat,
    /// wird der Fokus zum ersten Steuerelement (TabIndex 0) im Formular verschoben, das fokussierbar ist.
    /// F�r alle anderen Tastendr�cke wird das Standardverhalten genutzt.
    /// </summary>
    /// <param name="msg">Eine Referenz auf die Nachricht, die das Tastendruck-Ereignis repr�sentiert.</param>
    /// <param name="keyData">Die Daten der gedr�ckten Taste, einschlie�lich der Tab-Taste.</param>
    /// <returns>Gibt true zur�ck, wenn die Tastenaktion verarbeitet wurde, sonst false, um das Standardverhalten zu erm�glichen.</returns>
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

        // Standardverhalten f�r andere F�lle nutzen
        return base.ProcessCmdKey(ref msg, keyData);
    }


    // TODO - L�schen des Todos beim Rechtklicken

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