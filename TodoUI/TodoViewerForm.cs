namespace TodoUI;

public partial class TodoViewerForm : Form
{
    public TodoViewerForm()
    {
        InitializeComponent();

        // Hiddes the titlebar
        //this.FormBorderStyle = FormBorderStyle.None;

        //SampleData();
    }

    //private void SampleData() => chkListBoxTodos.Items.Add("TestEintrag 1");

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
                    //TodoDone = false
                };

                flowLayoutPanelTodos.Controls.Add(todoitem);
                TxtBoxTodoEntry.Text = "";
            }
        }
    }

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