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
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 14F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.rm222batch3_mind_12;
            ClientSize = new Size(800, 450);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "Main";
            Text = "АИС \"Ресторан\"";
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Button button2;
        private Button button3;
    }
}
