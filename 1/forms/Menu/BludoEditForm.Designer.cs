namespace _1.forms.Menu
{
    partial class BludoEditForm
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
            textBoxName = new TextBox();
            label1 = new Label();
            label2 = new Label();
            textBoxCena = new TextBox();
            label3 = new Label();
            textBoxOpisanie = new TextBox();
            comboBoxKategoriya = new ComboBox();
            label4 = new Label();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            SuspendLayout();
            // 
            // textBoxName
            // 
            textBoxName.Location = new Point(113, 18);
            textBoxName.Name = "textBoxName";
            textBoxName.Size = new Size(151, 23);
            textBoxName.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(12, 22);
            label1.Name = "label1";
            label1.Size = new Size(81, 19);
            label1.TabIndex = 1;
            label1.Text = "Название";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F);
            label2.Location = new Point(12, 81);
            label2.Name = "label2";
            label2.Size = new Size(45, 19);
            label2.TabIndex = 2;
            label2.Text = "Цена";
            // 
            // textBoxCena
            // 
            textBoxCena.Location = new Point(113, 77);
            textBoxCena.Name = "textBoxCena";
            textBoxCena.Size = new Size(151, 23);
            textBoxCena.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F);
            label3.Location = new Point(12, 148);
            label3.Name = "label3";
            label3.Size = new Size(81, 19);
            label3.TabIndex = 4;
            label3.Text = "Описание";
            // 
            // textBoxOpisanie
            // 
            textBoxOpisanie.Font = new Font("Segoe UI Semilight", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            textBoxOpisanie.Location = new Point(113, 118);
            textBoxOpisanie.Multiline = true;
            textBoxOpisanie.Name = "textBoxOpisanie";
            textBoxOpisanie.Size = new Size(151, 67);
            textBoxOpisanie.TabIndex = 5;
            // 
            // comboBoxKategoriya
            // 
            comboBoxKategoriya.FormattingEnabled = true;
            comboBoxKategoriya.Location = new Point(113, 203);
            comboBoxKategoriya.Name = "comboBoxKategoriya";
            comboBoxKategoriya.Size = new Size(151, 22);
            comboBoxKategoriya.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F);
            label4.Location = new Point(12, 206);
            label4.Name = "label4";
            label4.Size = new Size(90, 19);
            label4.TabIndex = 7;
            label4.Text = "Категория";
            // 
            // button1
            // 
            button1.Location = new Point(71, 274);
            button1.Name = "button1";
            button1.Size = new Size(92, 37);
            button1.TabIndex = 8;
            button1.Text = "Сохранить ";
            button1.UseVisualStyleBackColor = true;
            button1.Click += buttonSave_Click;
            // 
            // button2
            // 
            button2.Location = new Point(185, 274);
            button2.Name = "button2";
            button2.Size = new Size(92, 37);
            button2.TabIndex = 9;
            button2.Text = "Отменить";
            button2.UseVisualStyleBackColor = true;
            button2.Click += buttonCancel_Click;
            // 
            // button3
            // 
            button3.Location = new Point(124, 238);
            button3.Name = "button3";
            button3.Size = new Size(106, 30);
            button3.TabIndex = 10;
            button3.Text = "Состав блюда";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // BludoEditForm
            // 
            AutoScaleDimensions = new SizeF(7F, 14F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(353, 337);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label4);
            Controls.Add(comboBoxKategoriya);
            Controls.Add(textBoxOpisanie);
            Controls.Add(label3);
            Controls.Add(textBoxCena);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBoxName);
            Name = "BludoEditForm";
            Text = "Редактирование/Добавление блюда";
            Load += BludoEditForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBoxName;
        private Label label1;
        private Label label2;
        private TextBox textBoxCena;
        private Label label3;
        private TextBox textBoxOpisanie;
        private ComboBox comboBoxKategoriya;
        private Label label4;
        private Button button1;
        private Button button2;
        private Button button3;
    }
}