namespace TodoUI;

public partial class TodoViewerForm : Form
{
    public TodoViewerForm()
    {
        InitializeComponent();
        // Hiddes the titlebar
        //this.FormBorderStyle = FormBorderStyle.None;
        SampleData();
    }

    private void SampleData()
    {
        checkedListBox1.Items.Add("TestEintrag 1");
    }

    private void TxtBoxTodoEntry_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            // Prevents the enter tone from sounding
            e.SuppressKeyPress = true;

            if (TxtBoxTodoEntry.Text != "")
            {
                //MessageBox.Show("Enter Taste gedrückt!");
                checkedListBox1.Items.Add (TxtBoxTodoEntry.Text);
                TxtBoxTodoEntry.Text = "";
            }


        }
    }
}