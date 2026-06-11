namespace _1
{
    partial class Main
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            button6 = new Button();
            button7 = new Button();
            button8 = new Button();
            button9 = new Button();
            button10 = new Button();
            sidePanel = new Panel();
            groupBoxSystem = new GroupBox();
            groupBoxReports = new GroupBox();
            groupBoxDirectories = new GroupBox();
            groupBoxService = new GroupBox();
            labelRole = new Label();
            sidePanel.SuspendLayout();
            groupBoxService.SuspendLayout();
            groupBoxDirectories.SuspendLayout();
            groupBoxReports.SuspendLayout();
            groupBoxSystem.SuspendLayout();
            SuspendLayout();
            // 
            // sidePanel
            // 
            sidePanel.BackColor = SystemColors.ControlLight;
            sidePanel.Controls.Add(groupBoxSystem);
            sidePanel.Controls.Add(groupBoxReports);
            sidePanel.Controls.Add(groupBoxDirectories);
            sidePanel.Controls.Add(groupBoxService);
            sidePanel.Dock = DockStyle.Left;
            sidePanel.Location = new Point(0, 0);
            sidePanel.Name = "sidePanel";
            sidePanel.Size = new Size(220, 520);
            sidePanel.TabIndex = 0;
            // 
            // groupBoxService
            // 
            groupBoxService.Controls.Add(button1);
            groupBoxService.Controls.Add(button5);
            groupBoxService.Controls.Add(button10);
            groupBoxService.Location = new Point(8, 8);
            groupBoxService.Name = "groupBoxService";
            groupBoxService.Size = new Size(204, 140);
            groupBoxService.TabIndex = 0;
            groupBoxService.TabStop = false;
            groupBoxService.Text = "Обслуживание";
            // 
            // button1
            // 
            button1.Location = new Point(8, 22);
            button1.Name = "button1";
            button1.Size = new Size(188, 32);
            button1.TabIndex = 0;
            button1.Text = "Заказы";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button5
            // 
            button5.Location = new Point(8, 58);
            button5.Name = "button5";
            button5.Size = new Size(188, 32);
            button5.TabIndex = 1;
            button5.Text = "Бронирование";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button10
            // 
            button10.Location = new Point(8, 94);
            button10.Name = "button10";
            button10.Size = new Size(188, 32);
            button10.TabIndex = 2;
            button10.Text = "Кухня";
            button10.UseVisualStyleBackColor = true;
            button10.Click += button10_Click;
            // 
            // groupBoxDirectories
            // 
            groupBoxDirectories.Controls.Add(button6);
            groupBoxDirectories.Controls.Add(button8);
            groupBoxDirectories.Controls.Add(button2);
            groupBoxDirectories.Controls.Add(button3);
            groupBoxDirectories.Location = new Point(8, 154);
            groupBoxDirectories.Name = "groupBoxDirectories";
            groupBoxDirectories.Size = new Size(204, 180);
            groupBoxDirectories.TabIndex = 1;
            groupBoxDirectories.TabStop = false;
            groupBoxDirectories.Text = "Справочники";
            // 
            // button6
            // 
            button6.Location = new Point(8, 22);
            button6.Name = "button6";
            button6.Size = new Size(188, 32);
            button6.TabIndex = 0;
            button6.Text = "Меню";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // button8
            // 
            button8.Location = new Point(8, 130);
            button8.Name = "button8";
            button8.Size = new Size(188, 32);
            button8.TabIndex = 3;
            button8.Text = "Сотрудники";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // button2
            // 
            button2.Location = new Point(8, 58);
            button2.Name = "button2";
            button2.Size = new Size(188, 32);
            button2.TabIndex = 1;
            button2.Text = "Клиенты";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(8, 94);
            button3.Name = "button3";
            button3.Size = new Size(188, 32);
            button3.TabIndex = 2;
            button3.Text = "Продукты/склад";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // groupBoxReports
            // 
            groupBoxReports.Controls.Add(button4);
            groupBoxReports.Location = new Point(8, 340);
            groupBoxReports.Name = "groupBoxReports";
            groupBoxReports.Size = new Size(204, 70);
            groupBoxReports.TabIndex = 2;
            groupBoxReports.TabStop = false;
            groupBoxReports.Text = "Отчёты";
            // 
            // button4
            // 
            button4.Location = new Point(8, 22);
            button4.Name = "button4";
            button4.Size = new Size(188, 32);
            button4.TabIndex = 0;
            button4.Text = "Отчеты/запросы";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // groupBoxSystem
            // 
            groupBoxSystem.Controls.Add(button7);
            groupBoxSystem.Controls.Add(button9);
            groupBoxSystem.Location = new Point(8, 416);
            groupBoxSystem.Name = "groupBoxSystem";
            groupBoxSystem.Size = new Size(204, 96);
            groupBoxSystem.TabIndex = 3;
            groupBoxSystem.TabStop = false;
            groupBoxSystem.Text = "Система";
            // 
            // button7
            // 
            button7.Location = new Point(8, 22);
            button7.Name = "button7";
            button7.Size = new Size(188, 32);
            button7.TabIndex = 0;
            button7.Text = "Резервное копирование";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // button9
            // 
            button9.Location = new Point(8, 58);
            button9.Name = "button9";
            button9.Size = new Size(188, 32);
            button9.TabIndex = 1;
            button9.Text = "Помощь";
            button9.UseVisualStyleBackColor = true;
            button9.Click += button9_Click;
            // 
            // labelRole
            // 
            labelRole.AutoSize = true;
            labelRole.Font = new Font("Segoe UI", 16F);
            labelRole.Location = new Point(240, 20);
            labelRole.Name = "labelRole";
            labelRole.Size = new Size(0, 30);
            labelRole.TabIndex = 0;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 14F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(604, 520);
            Controls.Add(labelRole);
            Controls.Add(sidePanel);
            Name = "Main";
            Text = "АИС \"Ресторан\"";
            FormClosing += Main_FormClosing;
            Load += Main_Load;
            sidePanel.ResumeLayout(false);
            groupBoxService.ResumeLayout(false);
            groupBoxDirectories.ResumeLayout(false);
            groupBoxReports.ResumeLayout(false);
            groupBoxSystem.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel sidePanel;
        private GroupBox groupBoxService;
        private GroupBox groupBoxDirectories;
        private GroupBox groupBoxReports;
        private GroupBox groupBoxSystem;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button button6;
        private Button button7;
        private Button button8;
        private Button button9;
        private Button button10;
        private Label labelRole;
    }
}
