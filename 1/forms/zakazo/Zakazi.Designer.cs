namespace _1
{
    partial class Zakazi
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
            labelSearch = new Label();
            button1 = new Button();
            button2 = new Button();
            button4 = new Button();
            button5 = new Button();
            comboBox1 = new ComboBox();
            label1 = new Label();
            button6 = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Top;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(800, 280);
            dataGridView1.TabIndex = 0;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            // 
            // textBoxSearch
            // 
            textBoxSearch.Location = new Point(538, 290);
            textBoxSearch.Name = "textBoxSearch";
            textBoxSearch.PlaceholderText = "Поиск по клиенту, сотруднику...";
            textBoxSearch.Size = new Size(250, 23);
            textBoxSearch.TabIndex = 9;
            textBoxSearch.TextChanged += textBoxSearch_TextChanged;
            // 
            // labelSearch
            // 
            labelSearch.AutoSize = true;
            labelSearch.Location = new Point(427, 293);
            labelSearch.Name = "labelSearch";
            labelSearch.Size = new Size(105, 14);
            labelSearch.TabIndex = 10;
            labelSearch.Text = "Поиск заказов:";
            // 
            // button1
            // 
            button1.Location = new Point(24, 370);
            button1.Name = "button1";
            button1.Size = new Size(112, 39);
            button1.TabIndex = 1;
            button1.Text = "Добавить";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(520, 369);
            button2.Name = "button2";
            button2.Size = new Size(112, 39);
            button2.TabIndex = 2;
            button2.Text = "Изменить";
            button2.UseVisualStyleBackColor = true;
            button2.Visible = false;
            button2.Click += button2_Click;
            // 
            // button4
            // 
            button4.Location = new Point(194, 286);
            button4.Name = "button4";
            button4.Size = new Size(112, 42);
            button4.TabIndex = 4;
            button4.Text = "Оплатить";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.Location = new Point(24, 286);
            button5.Name = "button5";
            button5.Size = new Size(112, 42);
            button5.TabIndex = 5;
            button5.Text = "Состав заказа";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_click;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(194, 379);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(112, 22);
            comboBox1.TabIndex = 6;
            comboBox1.SelectedIndexChanged += combobox1_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(194, 362);
            label1.Name = "label1";
            label1.Size = new Size(105, 14);
            label1.TabIndex = 7;
            label1.Text = "Выбрать статус";
            // 
            // button6
            // 
            button6.Location = new Point(312, 370);
            button6.Name = "button6";
            button6.Size = new Size(81, 36);
            button6.TabIndex = 8;
            button6.Text = "Применить статус";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // Zakazi
            // 
            AutoScaleDimensions = new SizeF(7F, 14F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 475);
            Controls.Add(labelSearch);
            Controls.Add(textBoxSearch);
            Controls.Add(button6);
            Controls.Add(label1);
            Controls.Add(comboBox1);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(dataGridView1);
            Name = "Zakazi";
            Text = "Заказы";
            Load += Zakazi_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView1;
        private Button button1;
        private Button button2;
        private Button button4;
        private Button button5;
        private ComboBox comboBox1;
        private Label label1;
        private Button button6;
        private TextBox textBoxSearch;
        private Label labelSearch;
    }
}