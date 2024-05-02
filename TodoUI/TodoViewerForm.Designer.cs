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
            LstViewTodos = new ListView();
            SuspendLayout();
            // 
            // TxtBoxTodoEntry
            // 
            TxtBoxTodoEntry.BackColor = Color.White;
            TxtBoxTodoEntry.Font = new Font("Segoe UI", 14F);
            TxtBoxTodoEntry.Location = new Point(12, 12);
            TxtBoxTodoEntry.Name = "TxtBoxTodoEntry";
            TxtBoxTodoEntry.PlaceholderText = "Add Todo";
            TxtBoxTodoEntry.Size = new Size(776, 32);
            TxtBoxTodoEntry.TabIndex = 0;
            TxtBoxTodoEntry.KeyDown += TxtBoxTodoEntry_KeyDown;
            // 
            // LstViewTodos
            // 
            LstViewTodos.CheckBoxes = true;
            LstViewTodos.Location = new Point(12, 50);
            LstViewTodos.Name = "LstViewTodos";
            LstViewTodos.Size = new Size(776, 388);
            LstViewTodos.TabIndex = 1;
            LstViewTodos.UseCompatibleStateImageBehavior = false;
            // 
            // TodoViewerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(800, 450);
            Controls.Add(LstViewTodos);
            Controls.Add(TxtBoxTodoEntry);
            Name = "TodoViewerForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "TodoList";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox TxtBoxTodoEntry;
        private ListView LstViewTodos;
    }
}
