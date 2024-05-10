using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace TodoUI;

public partial class TodoItemControl : UserControl
{
    private bool isChecked = false;

    public TodoItemControl()
    {
        InitializeComponent();
    }

    public string TodoContent
    {
        get { return label1.Text; }
        set { label1.Text = value; }
    }
    public bool TodoDone
    {
        get { return isChecked; }
        set
        {
            isChecked = value;
            pictureBox1.Image = isChecked ? Properties.Resources.CheckBoxChecked1 : Properties.Resources.CheckBoxUnchecked1;
            label1.Font = isChecked ? new System.Drawing.Font(label1.Font, FontStyle.Strikeout) : new System.Drawing.Font(label1.Font, FontStyle.Regular);
        }
    }

    private void pictureBox1_Click(object sender, EventArgs e)
    {
        TodoDone = !TodoDone;
    }
}
