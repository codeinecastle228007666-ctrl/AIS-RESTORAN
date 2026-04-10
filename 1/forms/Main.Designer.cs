namespace _1
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            button6 = new Button();
            labelRole = new Label();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(110, 76);
            button1.Name = "button1";
            button1.Size = new Size(129, 59);
            button1.TabIndex = 0;
            button1.Text = "Заказы";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(110, 173);
            button2.Name = "button2";
            button2.Size = new Size(129, 59);
            button2.TabIndex = 1;
            button2.Text = "Клиенты";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(110, 272);
            button3.Name = "button3";
            button3.Size = new Size(129, 59);
            button3.TabIndex = 2;
            button3.Text = "Продукты/склад";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(418, 272);
            button4.Name = "button4";
            button4.Size = new Size(129, 59);
            button4.TabIndex = 3;
            button4.Text = "Отчеты/запросы";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.Location = new Point(418, 173);
            button5.Name = "button5";
            button5.Size = new Size(129, 59);
            button5.TabIndex = 4;
            button5.Text = "Бронирование";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.Location = new Point(418, 76);
            button6.Name = "button6";
            button6.Size = new Size(129, 59);
            button6.TabIndex = 5;
            button6.Text = "Меню";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // labelRole
            // 
            labelRole.AutoSize = true;
            labelRole.Font = new Font("Segoe UI", 15F);
            labelRole.Location = new Point(3, 12);
            labelRole.Name = "labelRole";
            labelRole.Size = new Size(0, 23);
            labelRole.TabIndex = 6;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 14F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.rm222batch3_mind_12;
            ClientSize = new Size(800, 450);
            Controls.Add(labelRole);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "Main";
            Text = "АИС \"Ресторан\"";
            FormClosing += Main_FormClosing;
            Load += Main_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button button6;
        private Label labelRole;
    }
}
