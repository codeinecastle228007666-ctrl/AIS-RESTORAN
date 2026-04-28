namespace _1.forms
{
    partial class SotrudnikEditForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.labelFio = new Label();
            this.textBoxFio = new TextBox();
            this.labelDolzhnost = new Label();
            this.comboBoxDolzhnost = new ComboBox();
            this.labelPhone = new Label();
            this.textBoxPhone = new MaskedTextBox(); // заменено на MaskedTextBox
            this.labelEmail = new Label();
            this.textBoxEmail = new TextBox();
            this.labelSeriya = new Label();
            this.textBoxSeriya = new TextBox();
            this.labelNomerPasporta = new Label();
            this.textBoxNomerPasporta = new TextBox();
            this.labelAdres = new Label();
            this.textBoxAdres = new TextBox();
            this.labelBirth = new Label();
            this.dateTimePickerBirth = new DateTimePicker();
            this.buttonSave = new Button();
            this.buttonCancel = new Button();
            this.SuspendLayout();

            // 
            // labelFio
            // 
            this.labelFio.AutoSize = true;
            this.labelFio.Font = new Font("Segoe UI", 12F);
            this.labelFio.Location = new Point(20, 20);
            this.labelFio.Name = "labelFio";
            this.labelFio.Size = new Size(43, 21);
            this.labelFio.Text = "ФИО:";
            // 
            // textBoxFio
            // 
            this.textBoxFio.Location = new Point(170, 18);
            this.textBoxFio.Name = "textBoxFio";
            this.textBoxFio.Size = new Size(250, 23);
            this.textBoxFio.TabIndex = 0;
            // 
            // labelDolzhnost
            // 
            this.labelDolzhnost.AutoSize = true;
            this.labelDolzhnost.Font = new Font("Segoe UI", 12F);
            this.labelDolzhnost.Location = new Point(20, 60);
            this.labelDolzhnost.Name = "labelDolzhnost";
            this.labelDolzhnost.Size = new Size(93, 21);
            this.labelDolzhnost.Text = "Должность:";
            // 
            // comboBoxDolzhnost
            // 
            this.comboBoxDolzhnost.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxDolzhnost.FormattingEnabled = true;
            this.comboBoxDolzhnost.Location = new Point(170, 58);
            this.comboBoxDolzhnost.Name = "comboBoxDolzhnost";
            this.comboBoxDolzhnost.Size = new Size(250, 23);
            this.comboBoxDolzhnost.TabIndex = 1;
            // 
            // labelPhone
            // 
            this.labelPhone.AutoSize = true;
            this.labelPhone.Font = new Font("Segoe UI", 12F);
            this.labelPhone.Location = new Point(20, 100);
            this.labelPhone.Name = "labelPhone";
            this.labelPhone.Size = new Size(73, 21);
            this.labelPhone.Text = "Телефон:";
            // 
            // textBoxPhone (MaskedTextBox)
            // 
            this.textBoxPhone.Location = new Point(170, 98);
            this.textBoxPhone.Mask = "+7 (999) 000-00-00";  // маска телефона
            this.textBoxPhone.Name = "textBoxPhone";
            this.textBoxPhone.Size = new Size(250, 23);
            this.textBoxPhone.TabIndex = 2;
            // 
            // labelEmail
            // 
            this.labelEmail.AutoSize = true;
            this.labelEmail.Font = new Font("Segoe UI", 12F);
            this.labelEmail.Location = new Point(20, 140);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new Size(54, 21);
            this.labelEmail.Text = "Email:";
            // 
            // textBoxEmail
            // 
            this.textBoxEmail.Location = new Point(170, 138);
            this.textBoxEmail.Name = "textBoxEmail";
            this.textBoxEmail.Size = new Size(250, 23);
            this.textBoxEmail.TabIndex = 3;
            // 
            // labelSeriya
            // 
            this.labelSeriya.AutoSize = true;
            this.labelSeriya.Font = new Font("Segoe UI", 12F);
            this.labelSeriya.Location = new Point(20, 180);
            this.labelSeriya.Name = "labelSeriya";
            this.labelSeriya.Size = new Size(130, 21);
            this.labelSeriya.Text = "Серия паспорта:";
            // 
            // textBoxSeriya
            // 
            this.textBoxSeriya.Location = new Point(170, 178);
            this.textBoxSeriya.MaxLength = 4;
            this.textBoxSeriya.Name = "textBoxSeriya";
            this.textBoxSeriya.Size = new Size(80, 23);
            this.textBoxSeriya.TabIndex = 4;
            this.textBoxSeriya.TextAlign = HorizontalAlignment.Center;
            // 
            // labelNomerPasporta
            // 
            this.labelNomerPasporta.AutoSize = true;
            this.labelNomerPasporta.Font = new Font("Segoe UI", 12F);
            this.labelNomerPasporta.Location = new Point(20, 220);
            this.labelNomerPasporta.Name = "labelNomerPasporta";
            this.labelNomerPasporta.Size = new Size(143, 21);
            this.labelNomerPasporta.Text = "Номер паспорта:";
            // 
            // textBoxNomerPasporta
            // 
            this.textBoxNomerPasporta.Location = new Point(170, 218);
            this.textBoxNomerPasporta.MaxLength = 6;
            this.textBoxNomerPasporta.Name = "textBoxNomerPasporta";
            this.textBoxNomerPasporta.Size = new Size(120, 23);
            this.textBoxNomerPasporta.TabIndex = 5;
            this.textBoxNomerPasporta.TextAlign = HorizontalAlignment.Center;
            // 
            // labelAdres
            // 
            this.labelAdres.AutoSize = true;
            this.labelAdres.Font = new Font("Segoe UI", 12F);
            this.labelAdres.Location = new Point(20, 260);
            this.labelAdres.Name = "labelAdres";
            this.labelAdres.Size = new Size(56, 21);
            this.labelAdres.Text = "Адрес:";
            // 
            // textBoxAdres
            // 
            this.textBoxAdres.Location = new Point(170, 258);
            this.textBoxAdres.Name = "textBoxAdres";
            this.textBoxAdres.Size = new Size(350, 23);
            this.textBoxAdres.TabIndex = 6;
            // 
            // labelBirth
            // 
            this.labelBirth.AutoSize = true;
            this.labelBirth.Font = new Font("Segoe UI", 12F);
            this.labelBirth.Location = new Point(20, 300);
            this.labelBirth.Name = "labelBirth";
            this.labelBirth.Size = new Size(135, 21);
            this.labelBirth.Text = "Дата рождения:";
            // 
            // dateTimePickerBirth
            // 
            this.dateTimePickerBirth.Format = DateTimePickerFormat.Short;
            this.dateTimePickerBirth.Location = new Point(170, 298);
            this.dateTimePickerBirth.Name = "dateTimePickerBirth";
            this.dateTimePickerBirth.Size = new Size(130, 23);
            this.dateTimePickerBirth.TabIndex = 7;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new Point(150, 350);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new Size(100, 35);
            this.buttonSave.TabIndex = 8;
            this.buttonSave.Text = "Сохранить";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new EventHandler(this.buttonSave_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new Point(290, 350);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(100, 35);
            this.buttonCancel.TabIndex = 9;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
            // 
            // SotrudnikEditForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(550, 410);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.dateTimePickerBirth);
            this.Controls.Add(this.labelBirth);
            this.Controls.Add(this.textBoxAdres);
            this.Controls.Add(this.labelAdres);
            this.Controls.Add(this.textBoxNomerPasporta);
            this.Controls.Add(this.labelNomerPasporta);
            this.Controls.Add(this.textBoxSeriya);
            this.Controls.Add(this.labelSeriya);
            this.Controls.Add(this.textBoxEmail);
            this.Controls.Add(this.labelEmail);
            this.Controls.Add(this.textBoxPhone);
            this.Controls.Add(this.labelPhone);
            this.Controls.Add(this.comboBoxDolzhnost);
            this.Controls.Add(this.labelDolzhnost);
            this.Controls.Add(this.textBoxFio);
            this.Controls.Add(this.labelFio);
            this.Font = new Font("Segoe UI", 9F);
            this.Name = "SotrudnikEditForm";
            this.Text = "Сотрудник";
            this.Load += new EventHandler(this.SotrudnikEditForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        // Поля: заменил TextBox textBoxPhone на MaskedTextBox
        private Label labelFio, labelDolzhnost, labelPhone, labelEmail;
        private TextBox textBoxFio, textBoxEmail;
        private MaskedTextBox textBoxPhone;   // <-- изменено
        private ComboBox comboBoxDolzhnost;
        private Button buttonSave, buttonCancel;

        private Label labelSeriya, labelNomerPasporta, labelAdres, labelBirth;
        private TextBox textBoxSeriya, textBoxNomerPasporta, textBoxAdres;
        private DateTimePicker dateTimePickerBirth;
    }
}