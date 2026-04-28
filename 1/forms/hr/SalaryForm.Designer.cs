namespace _1.forms
{
    partial class SalaryForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label labelEmployee;
        private Label labelOklad;
        private Label labelDays;
        private TextBox textBoxDays;
        private Label labelPremium;
        private TextBox textBoxPremium;
        private Label labelDeductions;
        private TextBox textBoxDeductions;
        private Button buttonCalculate;
        private Label labelResult;
        private Button buttonExport;
        private DateTimePicker dateTimePickerStart;
        private DateTimePicker dateTimePickerEnd;
        private Label labelPeriod;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.labelEmployee = new Label();
            this.labelOklad = new Label();
            this.labelDays = new Label();
            this.textBoxDays = new TextBox();
            this.labelPremium = new Label();
            this.textBoxPremium = new TextBox();
            this.labelDeductions = new Label();
            this.textBoxDeductions = new TextBox();
            this.buttonCalculate = new Button();
            this.labelResult = new Label();
            this.buttonExport = new Button();
            this.dateTimePickerStart = new DateTimePicker();
            this.dateTimePickerEnd = new DateTimePicker();
            this.labelPeriod = new Label();
            this.SuspendLayout();

            // 
            // labelEmployee
            // 
            this.labelEmployee.AutoSize = true;
            this.labelEmployee.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.labelEmployee.Location = new Point(20, 20);
            this.labelEmployee.Size = new Size(300, 42);
            this.labelEmployee.Text = "Сотрудник: \nДолжность: ";

            // 
            // labelOklad
            // 
            this.labelOklad.AutoSize = true;
            this.labelOklad.Font = new Font("Segoe UI", 12F);
            this.labelOklad.Location = new Point(20, 80);
            this.labelOklad.Size = new Size(120, 21);
            this.labelOklad.Text = "Оклад:";

            // 
            // labelPeriod
            // 
            this.labelPeriod.AutoSize = true;
            this.labelPeriod.Font = new Font("Segoe UI", 12F);
            this.labelPeriod.Location = new Point(20, 110);
            this.labelPeriod.Size = new Size(70, 21);
            this.labelPeriod.Text = "Период:";

            // 
            // dateTimePickerStart
            // 
            this.dateTimePickerStart.Format = DateTimePickerFormat.Short;
            this.dateTimePickerStart.Location = new Point(100, 110);
            this.dateTimePickerStart.Size = new Size(100, 23);

            // 
            // dateTimePickerEnd
            // 
            this.dateTimePickerEnd.Format = DateTimePickerFormat.Short;
            this.dateTimePickerEnd.Location = new Point(220, 110);
            this.dateTimePickerEnd.Size = new Size(100, 23);

            // 
            // labelDays
            // 
            this.labelDays.AutoSize = true;
            this.labelDays.Font = new Font("Segoe UI", 12F);
            this.labelDays.Location = new Point(20, 150);
            this.labelDays.Size = new Size(125, 21);
            this.labelDays.Text = "Отработано дней:";

            // 
            // textBoxDays
            // 
            this.textBoxDays.Location = new Point(180, 150);
            this.textBoxDays.Size = new Size(80, 23);

            // 
            // labelPremium
            // 
            this.labelPremium.AutoSize = true;
            this.labelPremium.Font = new Font("Segoe UI", 12F);
            this.labelPremium.Location = new Point(20, 190);
            this.labelPremium.Size = new Size(72, 21);
            this.labelPremium.Text = "Премия:";

            // 
            // textBoxPremium
            // 
            this.textBoxPremium.Location = new Point(180, 190);
            this.textBoxPremium.Size = new Size(100, 23);

            // 
            // labelDeductions
            // 
            this.labelDeductions.AutoSize = true;
            this.labelDeductions.Font = new Font("Segoe UI", 12F);
            this.labelDeductions.Location = new Point(20, 230);
            this.labelDeductions.Size = new Size(92, 21);
            this.labelDeductions.Text = "Удержания:";

            // 
            // textBoxDeductions
            // 
            this.textBoxDeductions.Location = new Point(180, 230);
            this.textBoxDeductions.Size = new Size(100, 23);

            // 
            // buttonCalculate
            // 
            this.buttonCalculate.Location = new Point(20, 280);
            this.buttonCalculate.Size = new Size(130, 35);
            this.buttonCalculate.Text = "Рассчитать";
            this.buttonCalculate.UseVisualStyleBackColor = true;
            this.buttonCalculate.Click += new EventHandler(this.buttonCalculate_Click);

            // 
            // buttonExport
            // 
            this.buttonExport.Enabled = false;
            this.buttonExport.Location = new Point(180, 280);
            this.buttonExport.Size = new Size(140, 35);
            this.buttonExport.Text = "Экспорт в Excel";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new EventHandler(this.buttonExport_Click);

            // 
            // labelResult
            // 
            this.labelResult.AutoSize = true;
            this.labelResult.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.labelResult.ForeColor = Color.DarkGreen;
            this.labelResult.Location = new Point(20, 340);
            this.labelResult.Size = new Size(180, 25);
            this.labelResult.Text = "Итого:";
            this.labelResult.Visible = false;

            // 
            // SalaryForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(380, 390);
            this.Controls.Add(this.labelResult);
            this.Controls.Add(this.buttonExport);
            this.Controls.Add(this.buttonCalculate);
            this.Controls.Add(this.textBoxDeductions);
            this.Controls.Add(this.labelDeductions);
            this.Controls.Add(this.textBoxPremium);
            this.Controls.Add(this.labelPremium);
            this.Controls.Add(this.textBoxDays);
            this.Controls.Add(this.labelDays);
            this.Controls.Add(this.dateTimePickerEnd);
            this.Controls.Add(this.dateTimePickerStart);
            this.Controls.Add(this.labelPeriod);
            this.Controls.Add(this.labelOklad);
            this.Controls.Add(this.labelEmployee);
            this.Name = "SalaryForm";
            this.Text = "Расчёт зарплаты";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}