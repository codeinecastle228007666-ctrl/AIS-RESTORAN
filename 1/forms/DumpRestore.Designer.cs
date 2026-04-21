namespace _1.forms
{
    partial class DumpRestore
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
            button1 = new Button();
            button2 = new Button();
            progressBar1 = new ProgressBar();
            labelStatus = new Label();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(40, 12);
            button1.Name = "button1";
            button1.Size = new Size(126, 86);
            button1.TabIndex = 0;
            button1.Text = "Запустить резервное копирование БД";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_1;
            // 
            // button2
            // 
            button2.Location = new Point(233, 12);
            button2.Name = "button2";
            button2.Size = new Size(132, 86);
            button2.TabIndex = 1;
            button2.Text = "Запустить восстановление БД";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(3, 148);
            progressBar1.MarqueeAnimationSpeed = 30;
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(383, 37);
            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.TabIndex = 2;
            // 
            // labelStatus
            // 
            labelStatus.AutoSize = true;
            labelStatus.Location = new Point(12, 114);
            labelStatus.Name = "labelStatus";
            labelStatus.Size = new Size(14, 14);
            labelStatus.TabIndex = 3;
            labelStatus.Text = " ";
            // 
            // DumpRestore
            // 
            AutoScaleDimensions = new SizeF(7F, 14F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(394, 189);
            Controls.Add(labelStatus);
            Controls.Add(progressBar1);
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "DumpRestore";
            Text = "Резервное копирование и восстановление";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button button2;
        private ProgressBar progressBar1;
        private Label labelStatus;
    }
}