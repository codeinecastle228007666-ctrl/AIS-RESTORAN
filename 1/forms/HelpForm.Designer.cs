namespace _1.forms
{
    partial class HelpForm
    {
        private System.ComponentModel.IContainer components = null;
        private TabControl tabControl1;
        private TabPage tabPageAbout;
        private TabPage tabPageGuide;
        private Label labelAbout;
        private TextBox textBoxGuide;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tabControl1 = new TabControl();
            this.tabPageAbout = new TabPage();
            this.tabPageGuide = new TabPage();
            this.labelAbout = new Label();
            this.textBoxGuide = new TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPageAbout.SuspendLayout();
            this.tabPageGuide.SuspendLayout();
            this.SuspendLayout();

            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageAbout);
            this.tabControl1.Controls.Add(this.tabPageGuide);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(600, 450);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageAbout
            // 
            this.tabPageAbout.Controls.Add(this.labelAbout);
            this.tabPageAbout.Location = new Point(4, 24);
            this.tabPageAbout.Name = "tabPageAbout";
            this.tabPageAbout.Padding = new Padding(3);
            this.tabPageAbout.Size = new Size(592, 422);
            this.tabPageAbout.Text = "О программе";
            this.tabPageAbout.UseVisualStyleBackColor = true;
            // 
            // tabPageGuide
            // 
            this.tabPageGuide.Controls.Add(this.textBoxGuide);
            this.tabPageGuide.Location = new Point(4, 24);
            this.tabPageGuide.Name = "tabPageGuide";
            this.tabPageGuide.Padding = new Padding(3);
            this.tabPageGuide.Size = new Size(592, 422);
            this.tabPageGuide.Text = "Руководство";
            this.tabPageGuide.UseVisualStyleBackColor = true;
            // 
            // labelAbout
            // 
            this.labelAbout.Dock = DockStyle.Fill;
            this.labelAbout.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            this.labelAbout.Location = new Point(3, 3);
            this.labelAbout.Name = "labelAbout";
            this.labelAbout.Size = new Size(586, 416);
            this.labelAbout.Text = "АИС «Ресторан»\r\nВерсия indev v0.18\r\nРазработчик: Соловьев Максим Владимирович\r\n2026 год\r\n\r\nПрограмма предназначена для управления заказами, складом, персоналом и финансами ресторана.\r\n\r\nПоддерживаемые роли:\r\n- Официант\r\n- Повар\r\n- Шеф-повар\r\n- Руководитель";
            // 
            // textBoxGuide
            // 
            this.textBoxGuide.Dock = DockStyle.Fill;
            this.textBoxGuide.Font = new Font("Consolas", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.textBoxGuide.Location = new Point(3, 3);
            this.textBoxGuide.Multiline = true;
            this.textBoxGuide.Name = "textBoxGuide";
            this.textBoxGuide.ReadOnly = true;
            this.textBoxGuide.ScrollBars = ScrollBars.Vertical;
            this.textBoxGuide.Size = new Size(586, 416);
            this.textBoxGuide.Text = "1. АВТОРИЗАЦИЯ\r\nВведите логин и пароль. Неверные попытки записываются в журнал.\r\n\r\n2. ОСНОВНЫЕ ОПЕРАЦИИ\r\n- Заказы: создание, изменение статусов, оплата (наличные, карта, QR-код).\r\n- Меню: добавление/редактирование блюд и их состава.\r\n- Клиенты: ведение базы клиентов с телефонами.\r\n- Бронирование: выбор стола по схеме зала, проверка занятости.\r\n- Склад: приход продуктов, заявки поставщикам, просмотр остатков.\r\n- Сотрудники: учётные записи и управление.\r\n- Отчёты: финансовые сводки (выручка, прибыль, рентабельность) и др.\r\n\r\n3. СТАТУСЫ ЗАКАЗОВ\r\nНовый → Принят → Готовится → Готов к выдаче → Оплачен → Завершён.\r\nПереходы контролируются, списание продуктов со склада происходит при старте приготовления.\r\n\r\n4. ГОРЯЧИЕ КЛАВИШИ\r\n(Пока не назначены)\r\n";
            // 
            // HelpForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(600, 450);
            this.Controls.Add(this.tabControl1);
            this.Name = "HelpForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Справка";
            this.tabControl1.ResumeLayout(false);
            this.tabPageAbout.ResumeLayout(false);
            this.tabPageGuide.ResumeLayout(false);
            this.tabPageGuide.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}