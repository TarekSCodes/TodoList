using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TodoUI
{
    public partial class TodoItemControl : UserControl
    {
        public TodoItemControl()
        {
            InitializeComponent();
        }

        public string TodoContent
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }

        //public bool TodoDone
        //{
        //    get { return checkBox1.Checked; }
        //    set { checkBox1.Checked = value; }
        //}
    }
}
