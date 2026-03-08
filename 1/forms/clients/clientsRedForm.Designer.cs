namespace _1.forms.clients
{
    partial class clientsRedForm
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
            textBoxFIO = new TextBox();
            buttonSave = new Button();
            buttonCancel = new Button();
            label1 = new Label();
            label2 = new Label();
            maskedTextBox1 = new MaskedTextBox();
            SuspendLayout();
            // 
            // textBoxFIO
            // 
            textBoxFIO.Location = new Point(151, 26);
            textBoxFIO.Name = "textBoxFIO";
            textBoxFIO.PlaceholderText = "Введите ФИО";
            textBoxFIO.Size = new Size(176, 23);
            textBoxFIO.TabIndex = 0;
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(23, 130);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(87, 41);
            buttonSave.TabIndex = 2;
            buttonSave.Text = "Сохранить запись";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(182, 130);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(87, 41);
            buttonCancel.TabIndex = 3;
            buttonCancel.Text = "Отменить";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15F);
            label1.Location = new Point(40, 26);
            label1.Name = "label1";
            label1.Size = new Size(43, 23);
            label1.TabIndex = 4;
            label1.Text = "ФИО";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F);
            label2.Location = new Point(10, 77);
            label2.Name = "label2";
            label2.Size = new Size(135, 19);
            label2.TabIndex = 5;
            label2.Text = "Номер телефона";
            // 
            // maskedTextBox1
            // 
            maskedTextBox1.Location = new Point(151, 77);
            maskedTextBox1.Mask = "+7 (999) 000-00-00";
            maskedTextBox1.Name = "maskedTextBox1";
            maskedTextBox1.Size = new Size(176, 23);
            maskedTextBox1.TabIndex = 6;
            // 
            // clientsRedForm
            // 
            AutoScaleDimensions = new SizeF(7F, 14F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(370, 183);
            Controls.Add(maskedTextBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(buttonCancel);
            Controls.Add(buttonSave);
            Controls.Add(textBoxFIO);
            Name = "clientsRedForm";
            Text = "Добавление/Редактирование клиента";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBoxFIO;
        private Button buttonSave;
        private Button buttonCancel;
        private Label label1;
        private Label label2;
        private MaskedTextBox maskedTextBox1;
    }
}