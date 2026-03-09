namespace _1.forms.bronirovanie
{
    partial class BronirovanieForm
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
            comboBox1 = new ComboBox();
            dateTimePicker1 = new DateTimePicker();
            numericUpDown1 = new NumericUpDown();
            comboBox3 = new ComboBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            button1 = new Button();
            button2 = new Button();
            comboBox2 = new ComboBox();
            buttonSelectTable = new Button();
            buttonApplyStatus = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Top;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(800, 304);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellFormatting += dataGridView1_CellFormatting;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(12, 332);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(191, 22);
            comboBox1.TabIndex = 1;
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.Location = new Point(307, 332);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(152, 23);
            dateTimePicker1.TabIndex = 3;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(485, 331);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(40, 23);
            numericUpDown1.TabIndex = 4;
            // 
            // comboBox3
            // 
            comboBox3.FormattingEnabled = true;
            comboBox3.Location = new Point(625, 331);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(147, 22);
            comboBox3.TabIndex = 5;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(12, 307);
            label1.Name = "label1";
            label1.Size = new Size(63, 19);
            label1.TabIndex = 6;
            label1.Text = "Клиент";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F);
            label2.Location = new Point(233, 307);
            label2.Name = "label2";
            label2.Size = new Size(45, 19);
            label2.TabIndex = 7;
            label2.Text = "Стол";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F);
            label3.Location = new Point(307, 307);
            label3.Name = "label3";
            label3.Size = new Size(99, 19);
            label3.TabIndex = 8;
            label3.Text = "Дата брони";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F);
            label4.Location = new Point(485, 309);
            label4.Name = "label4";
            label4.Size = new Size(126, 19);
            label4.TabIndex = 9;
            label4.Text = "Кол-во гостей";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F);
            label5.Location = new Point(625, 307);
            label5.Name = "label5";
            label5.Size = new Size(63, 19);
            label5.TabIndex = 10;
            label5.Text = "Статус";
            // 
            // button1
            // 
            button1.Location = new Point(12, 402);
            button1.Name = "button1";
            button1.Size = new Size(117, 36);
            button1.TabIndex = 11;
            button1.Text = "Добавить";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(135, 402);
            button2.Name = "button2";
            button2.Size = new Size(117, 36);
            button2.TabIndex = 12;
            button2.Text = "Удалить";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(233, 332);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(45, 22);
            comboBox2.TabIndex = 2;
            // 
            // buttonSelectTable
            // 
            buttonSelectTable.Location = new Point(221, 360);
            buttonSelectTable.Name = "buttonSelectTable";
            buttonSelectTable.Size = new Size(74, 36);
            buttonSelectTable.TabIndex = 13;
            buttonSelectTable.Text = "Схема зала";
            buttonSelectTable.UseVisualStyleBackColor = true;
            buttonSelectTable.Click += buttonSelectTable_Click;
            // 
            // buttonApplyStatus
            // 
            buttonApplyStatus.Location = new Point(625, 360);
            buttonApplyStatus.Name = "buttonApplyStatus";
            buttonApplyStatus.Size = new Size(147, 23);
            buttonApplyStatus.TabIndex = 14;
            buttonApplyStatus.Text = "Применить статус";
            buttonApplyStatus.UseVisualStyleBackColor = true;
            buttonApplyStatus.Click += buttonApplyStatus_Click;
            // 
            // BronirovanieForm
            // 
            AutoScaleDimensions = new SizeF(7F, 14F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(buttonApplyStatus);
            Controls.Add(buttonSelectTable);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(comboBox3);
            Controls.Add(numericUpDown1);
            Controls.Add(dateTimePicker1);
            Controls.Add(comboBox2);
            Controls.Add(comboBox1);
            Controls.Add(dataGridView1);
            Name = "BronirovanieForm";
            Text = "Бронирование";
            Load += BronirovanieForm_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView1;
        private ComboBox comboBox1;
        private DateTimePicker dateTimePicker1;
        private NumericUpDown numericUpDown1;
        private ComboBox comboBox3;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Button button1;
        private Button button2;
        private ComboBox comboBox2;
        private Button buttonSelectTable;
        private Button buttonApplyStatus;
    }
}