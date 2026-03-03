namespace _1.forms
{
    partial class redzakazaform
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
            comboBoxClient = new ComboBox();
            comboBoxStol = new ComboBox();
            comboBoxSotrudnik = new ComboBox();
            comboBoxStatus = new ComboBox();
            dateTimePicker1 = new DateTimePicker();
            button1 = new Button();
            button2 = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            SuspendLayout();
            // 
            // comboBoxClient
            // 
            comboBoxClient.FormattingEnabled = true;
            comboBoxClient.Location = new Point(165, 39);
            comboBoxClient.Name = "comboBoxClient";
            comboBoxClient.Size = new Size(186, 22);
            comboBoxClient.TabIndex = 0;
            // 
            // comboBoxStol
            // 
            comboBoxStol.FormattingEnabled = true;
            comboBoxStol.Location = new Point(165, 112);
            comboBoxStol.Name = "comboBoxStol";
            comboBoxStol.Size = new Size(186, 22);
            comboBoxStol.TabIndex = 1;
            // 
            // comboBoxSotrudnik
            // 
            comboBoxSotrudnik.FormattingEnabled = true;
            comboBoxSotrudnik.Location = new Point(165, 198);
            comboBoxSotrudnik.Name = "comboBoxSotrudnik";
            comboBoxSotrudnik.Size = new Size(186, 22);
            comboBoxSotrudnik.TabIndex = 2;
            // 
            // comboBoxStatus
            // 
            comboBoxStatus.Enabled = false;
            comboBoxStatus.FormattingEnabled = true;
            comboBoxStatus.Location = new Point(165, 286);
            comboBoxStatus.Name = "comboBoxStatus";
            comboBoxStatus.Size = new Size(186, 22);
            comboBoxStatus.TabIndex = 3;
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Location = new Point(165, 365);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(186, 23);
            dateTimePicker1.TabIndex = 4;
            // 
            // button1
            // 
            button1.Location = new Point(466, 39);
            button1.Name = "button1";
            button1.Size = new Size(199, 73);
            button1.TabIndex = 5;
            button1.Text = "Сохранить";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(466, 172);
            button2.Name = "button2";
            button2.Size = new Size(199, 72);
            button2.TabIndex = 6;
            button2.Text = "Отменить";
            button2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semilight", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label1.Location = new Point(12, 24);
            label1.Name = "label1";
            label1.Size = new Size(102, 37);
            label1.TabIndex = 7;
            label1.Text = "Клиент";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semilight", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label2.Location = new Point(12, 97);
            label2.Name = "label2";
            label2.Size = new Size(74, 37);
            label2.TabIndex = 8;
            label2.Text = "Стол";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semilight", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label3.Location = new Point(12, 182);
            label3.Name = "label3";
            label3.Size = new Size(144, 37);
            label3.TabIndex = 9;
            label3.Text = "Сотрудник";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semilight", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label4.Location = new Point(12, 271);
            label4.Name = "label4";
            label4.Size = new Size(96, 37);
            label4.TabIndex = 10;
            label4.Text = "Статус";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI Semilight", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label5.Location = new Point(-1, 352);
            label5.Name = "label5";
            label5.Size = new Size(160, 37);
            label5.TabIndex = 11;
            label5.Text = "Дата заказа";
            // 
            // redzakazaform
            // 
            AutoScaleDimensions = new SizeF(7F, 14F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(dateTimePicker1);
            Controls.Add(comboBoxStatus);
            Controls.Add(comboBoxSotrudnik);
            Controls.Add(comboBoxStol);
            Controls.Add(comboBoxClient);
            Name = "redzakazaform";
            Text = "Создание заказа";
            Load += redzakazaform_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox comboBoxClient;
        private ComboBox comboBoxStol;
        private ComboBox comboBoxSotrudnik;
        private ComboBox comboBoxStatus;
        private DateTimePicker dateTimePicker1;
        private Button button1;
        private Button button2;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
    }
}