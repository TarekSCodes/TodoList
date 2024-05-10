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
            flowLayoutPanelTodos.Size = new Size(560, 379);
            flowLayoutPanelTodos.TabIndex = 1;
            flowLayoutPanelTodos.WrapContents = false;
            // 
            // TodoViewerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(584, 461);
            Controls.Add(flowLayoutPanelTodos);
            Controls.Add(TxtBoxTodoEntry);
            KeyPreview = true;
            MaximizeBox = false;
            Name = "TodoViewerForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            KeyDown += TodoViewerForm_KeyDown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox TxtBoxTodoEntry;
        private FlowLayoutPanel flowLayoutPanelTodos;
    }
}
