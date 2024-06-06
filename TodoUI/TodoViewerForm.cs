using System;
using TodoLibrary;
using TodoLibrary.Models;

namespace TodoUI;

// TODO - App beim Systemstart oder anmelden starten
// TODO - Tastenkombi zum �ffnen und Focusieren des Fenster implementieren
/* TODO - Kategorien
 * Erstellbar, L�schbar
 * speichern in einer csv Datei
 * Farbe w�hlbar
 */

/* SETTINGS
 * TODO - Tastenkombi zum �ffnen selbst vergeben
 * Neu abgehakte Todos m�ssen auch aus der gui ausgeblendet werden wenn die einstellung so gesetzt ist
 */

public partial class TodoViewerForm : Form
{

    private NotifyIcon trayIcon;
    private ContextMenuStrip trayMenu;
    private ContextMenuStrip settingsMenu;
    private ToolStripMenuItem menuItemHideCompleted;
    private ToolStripMenuItem menuItemAlwaysOnTop;

    public TodoViewerForm()
    {
        InitializeComponent();
        InitializeTrayMenu();
        InitializesettingsMenu();
    }

    private void InitializesettingsMenu()
    {
        settingsMenu = new ContextMenuStrip();

        // Immer im Vordergrund
        menuItemAlwaysOnTop = new ToolStripMenuItem("Immer im Vordergrund");
        menuItemAlwaysOnTop.CheckOnClick = true;
        menuItemAlwaysOnTop.CheckedChanged += MenuItemAlwaysOnTop_CheckedChanged;

        // Abgehakte Todos ausblenden
        menuItemHideCompleted = new ToolStripMenuItem("Abgehakte Todos ausblenden");
        menuItemHideCompleted.CheckOnClick = true;
        menuItemHideCompleted.CheckedChanged += MenuItemHideCompleted_CheckedChanged;

        settingsMenu.Items.Add(menuItemAlwaysOnTop);
        settingsMenu.Items.Add(menuItemHideCompleted);
    }

    private void InitializeTrayMenu()
    {
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

    /// <summary>
    /// Diese Methode �ffnet die Anwendung aus dem Systemtray,
    /// zeigt das Hauptfenster an und stellt es auf den normalen Zustand zur�ck.
    /// </summary>
    private void OnOpen(object sender, EventArgs e)
    {
        Show();
        WindowState = FormWindowState.Normal;
        ShowInTaskbar = true;
    }

    /// <summary>
    /// Behandelt das Resize-Ereignis der Form.
    /// Minimiert die Form in das Systemtray, wenn sie minimiert wird.
    /// </summary>
    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);

        if (WindowState == FormWindowState.Minimized)
            Hide();
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
    /// L�dt alle To-do-Eintr�ge aus der Datei, filtert sie basierend auf den Benutzereinstellungen und aktualisiert die Benutzeroberfl�che.
    /// </summary>
    /// <remarks>
    /// Diese Methode l�dt alle To-do-Eintr�ge aus der Datei und kehrt die Reihenfolge der Liste um. Anschlie�end wird �berpr�ft, ob abgeschlossene
    /// To-dos ausgeblendet werden sollen. Basierend auf dieser Einstellung wird die Liste der To-dos gefiltert und die Benutzeroberfl�che entsprechend aktualisiert.
    /// </remarks>
    private void LoadTodos()
    {
        IEnumerable<TodoModel> filteredTodos = GlobalConfig.Connection.LoadTodosFromFile(menuItemAlwaysOnTop.Checked, menuItemHideCompleted.Checked);

        UpdateUI(filteredTodos);

        /*
        // L�dt alle TodoModel-Eintr�ge aus der Datei und kehrt die Reihenfolge der Liste um
        List<TodoModel> todos = GlobalConfig.Connection.LoadTodosFromFile();
        todos.Reverse();

        // L�dt die Einstellung, ob abgeschlossene Todos ausgeblendet werden sollen
        bool shouldHideCompletedTodos = LoadSettings();

        // Filtert die Todos basierend auf der Einstellung
        IEnumerable<TodoModel> filteredTodos = FilterTodos(todos, shouldHideCompletedTodos);

        // Aktualisiert die Benutzeroberfl�che
        UpdateUI(filteredTodos);
        */
    }

    /*
    /// <summary>
    /// Filtert die Liste der To-dos basierend auf der Einstellung, ob abgeschlossene To-dos ausgeblendet werden sollen.
    /// </summary>
    /// <param name="todos">Die urspr�ngliche Liste der To-dos.</param>
    /// <param name="shouldHideCompletedTodos">Ein boolescher Wert, der angibt, ob abgeschlossene To-dos ausgeblendet werden sollen.</param>
    /// <returns>Eine gefilterte Liste der To-dos, bei der abgeschlossene To-dos ausgeblendet werden, wenn <paramref name="shouldHideCompletedTodos"/> wahr ist.</returns>
    /// <remarks>
    /// Wenn <paramref name="shouldHideCompletedTodos"/> wahr ist, werden nur die nicht abgeschlossenen To-dos zur�ckgegeben. Andernfalls wird die urspr�ngliche Liste zur�ckgegeben.
    /// </remarks>
    private static IEnumerable<TodoModel> FilterTodos(IEnumerable<TodoModel> todos, bool shouldHideCompletedTodos)
    {
        return shouldHideCompletedTodos ? todos.Where(todo => !todo.TodoDone) : todos;
    }
    */

    /// <summary>
    /// Aktualisiert die Benutzeroberfl�che mit der gegebenen Liste von To-dos.
    /// </summary>
    /// <param name="todos">Die Liste der To-dos, die in der Benutzeroberfl�che angezeigt werden sollen.</param>
    /// <remarks>
    /// Diese Methode l�scht alle aktuellen To-do-Kontrollen aus dem `flowLayoutPanelTodos` und f�gt dann jede To-do-Kontrolle aus der gegebenen Liste hinzu.
    /// </remarks>
    private void UpdateUI(IEnumerable<TodoModel> todos)
    {
        flowLayoutPanelTodos.Controls.Clear();
        foreach (TodoModel todo in todos)
            AddTodoToUI(todo);
    }

    /*
    /// <summary>
    /// L�dt die Anwendungseinstellungen aus einer Datei und aktualisiert die Benutzeroberfl�che entsprechend.
    /// </summary>
    /// <returns>Ein boolescher Wert, der angibt, ob abgeschlossene To-dos ausgeblendet werden sollen.</returns>
    /// <remarks>
    /// Diese Methode l�dt die Einstellungen, ob die Anwendung immer im Vordergrund sein soll und ob abgeschlossene To-dos ausgeblendet werden sollen, aus einer Datei.
    /// Sie setzt die entsprechenden UI-Elemente basierend auf diesen Einstellungen.
    /// </remarks>
    private bool LoadSettings()
    {
        List<bool> settings = GlobalConfig.Connection.LoadSettingsFromFile();

        TopMost = settings.FirstOrDefault(false);
        if (TopMost.Equals(true))
            menuItemAlwaysOnTop.Checked = true;

        // ----------------- //

        bool HideCompleted = settings.LastOrDefault(false);
        if (HideCompleted)
            menuItemHideCompleted.Checked = true;

        return HideCompleted;
    }
    */

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
    /// Ereignishandler f�r den Klick auf den Einstellungen-Button. Berechnet die Position des Buttons und zeigt das Einstellungsmen� an.
    /// </summary>
    /// <param name="sender">Das Objekt, das das Ereignis ausl�st.</param>
    /// <param name="e">Das <see cref="EventArgs"/>-Objekt, das die Ereignisdaten enth�lt.</param>
    /// <remarks>
    /// Diese Methode berechnet die Position des Buttons relativ zum Formular und zeigt das Einstellungsmen� an einer festen Position relativ zum Button an.
    /// </remarks>
    private void BtnSettings_Click(object sender, EventArgs e)
    {
        // Berechne die Position des Buttons relativ zum Formular
        Point buttonPosition = BtnSettings.PointToScreen(Point.Empty);

        // Feste Position relativ zum Button festlegen
        int x = buttonPosition.X;
        int y = buttonPosition.Y + BtnSettings.Height;

        // Men� an der festen Position anzeigen
        this.settingsMenu.Show(x, y);
    }

    /// <summary>
    /// Ereignishandler f�r die �nderung des "Always on Top"-Men�elements.
    /// Aktualisiert die "TopMost"-Eigenschaft der Anwendung und speichert die ge�nderten Einstellungen.
    /// </summary>
    /// <remarks>
    /// Diese Methode wird aufgerufen, wenn der Status des "Always on Top"-Men�elements ge�ndert wird.
    /// Sie aktualisiert die "TopMost"-Eigenschaft der Anwendung entsprechend dem neuen Status und speichert die ge�nderten Einstellungen.
    /// </remarks>
    private void MenuItemAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
    {
        TopMost = menuItemAlwaysOnTop.Checked;
        GlobalConfig.Connection.UpdateSettings(TopMost, menuItemHideCompleted.Checked);
    }

    /// <summary>
    /// Ereignishandler f�r die �nderung des "Hide Completed"-Men�elements.
    /// Aktualisiert die Einstellungen und l�dt die To-dos basierend auf der neuen Einstellung neu.
    /// </summary>
    /// <remarks>
    /// Diese Methode wird aufgerufen, wenn der Status des "Hide Completed"-Men�elements ge�ndert wird.
    /// Sie speichert die ge�nderten Einstellungen und l�dt die To-dos basierend auf der neuen Einstellung neu.
    /// </remarks>
    private void MenuItemHideCompleted_CheckedChanged(object? sender, EventArgs e)
    {
        //sender = menuItemHideCompleted.Checked;
        GlobalConfig.Connection.UpdateSettings(TopMost, menuItemHideCompleted.Checked);
        LoadTodos();
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