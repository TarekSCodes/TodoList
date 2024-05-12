namespace TodoUI
{
    partial class TodoViewerForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            TxtBoxTodoEntry = new TextBox();
            flowLayoutPanelTodos = new FlowLayoutPanel();
            BtnQuit = new Button();
            BtnSettings = new Button();
            SuspendLayout();
            // 
            // TxtBoxTodoEntry
            // 
            TxtBoxTodoEntry.BackColor = Color.White;
            TxtBoxTodoEntry.Font = new Font("Segoe UI", 14F);
            TxtBoxTodoEntry.Location = new Point(12, 12);
            TxtBoxTodoEntry.Name = "TxtBoxTodoEntry";
            TxtBoxTodoEntry.PlaceholderText = "Add Todo";
            TxtBoxTodoEntry.Size = new Size(560, 32);
            TxtBoxTodoEntry.TabIndex = 0;
            TxtBoxTodoEntry.KeyDown += TxtBoxTodoEntry_KeyDown;
            // 
            // flowLayoutPanelTodos
            // 
            flowLayoutPanelTodos.AutoScroll = true;
            flowLayoutPanelTodos.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanelTodos.Location = new Point(12, 59);
            flowLayoutPanelTodos.Name = "flowLayoutPanelTodos";
            flowLayoutPanelTodos.Size = new Size(560, 344);
            flowLayoutPanelTodos.TabIndex = 4;
            flowLayoutPanelTodos.WrapContents = false;
            // 
            // BtnQuit
            // 
            BtnQuit.Location = new Point(523, 426);
            BtnQuit.Name = "BtnQuit";
            BtnQuit.Size = new Size(49, 23);
            BtnQuit.TabIndex = 2;
            BtnQuit.Text = "Quit";
            BtnQuit.UseVisualStyleBackColor = true;
            BtnQuit.Click += BtnQuit_Click;
            // 
            // BtnSettings
            // 
            BtnSettings.Location = new Point(442, 426);
            BtnSettings.Name = "BtnSettings";
            BtnSettings.Size = new Size(75, 23);
            BtnSettings.TabIndex = 1;
            BtnSettings.Text = "Settings";
            BtnSettings.UseVisualStyleBackColor = true;
            // 
            // TodoViewerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(584, 461);
            Controls.Add(BtnSettings);
            Controls.Add(BtnQuit);
            Controls.Add(flowLayoutPanelTodos);
            Controls.Add(TxtBoxTodoEntry);
            KeyPreview = true;
            MaximizeBox = false;
            Name = "TodoViewerForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            TopMost = true;
            Load += TodoViewerForm_Load;
            KeyDown += TodoViewerForm_KeyDown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox TxtBoxTodoEntry;
        private FlowLayoutPanel flowLayoutPanelTodos;
        private Button BtnQuit;
        private Button BtnSettings;
    }
}
