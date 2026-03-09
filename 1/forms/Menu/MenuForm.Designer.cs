namespace _1.forms.Menu
{
    partial class MenuForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            textBoxSearch = new TextBox();
            buttonAdd = new Button();
            buttonRed = new Button();
            buttonDelete = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Top;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(800, 315);
            dataGridView1.TabIndex = 0;
            // 
            // textBoxSearch
            // 
            textBoxSearch.Location = new Point(12, 321);
            textBoxSearch.Name = "textBoxSearch";
            textBoxSearch.PlaceholderText = "Поиск";
            textBoxSearch.Size = new Size(137, 23);
            textBoxSearch.TabIndex = 1;
            textBoxSearch.TextChanged += textBoxSearch_TextChanged;
            // 
            // buttonAdd
            // 
            buttonAdd.Location = new Point(12, 392);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new Size(104, 46);
            buttonAdd.TabIndex = 2;
            buttonAdd.Text = "Добавить";
            buttonAdd.UseVisualStyleBackColor = true;
            buttonAdd.Click += buttonAdd_Click;
            // 
            // buttonRed
            // 
            buttonRed.Location = new Point(159, 392);
            buttonRed.Name = "buttonRed";
            buttonRed.Size = new Size(114, 46);
            buttonRed.TabIndex = 3;
            buttonRed.Text = "Редактировать";
            buttonRed.UseVisualStyleBackColor = true;
            buttonRed.Click += buttonRed_Click;
            // 
            // buttonDelete
            // 
            buttonDelete.Location = new Point(320, 392);
            buttonDelete.Name = "buttonDelete";
            buttonDelete.Size = new Size(114, 46);
            buttonDelete.TabIndex = 4;
            buttonDelete.Text = "Удалить";
            buttonDelete.UseVisualStyleBackColor = true;
            buttonDelete.Click += buttonDelete_Click;
            // 
            // MenuForm
            // 
            AutoScaleDimensions = new SizeF(7F, 14F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(buttonDelete);
            Controls.Add(buttonRed);
            Controls.Add(buttonAdd);
            Controls.Add(textBoxSearch);
            Controls.Add(dataGridView1);
            Name = "MenuForm";
            Text = "Меню";
            Load += MenuForm_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView1;
        private TextBox textBoxSearch;
        private Button buttonAdd;
        private Button buttonRed;
        private Button buttonDelete;
    }
}