namespace _1.forms
{
    partial class Sklad
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
            dataGridView1 = new DataGridView();
            button1 = new Button();
            button2 = new Button();
            buttonZayavka = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Top;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(800, 300);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellFormatting += dataGridView1_CellFormatting;
            dataGridView1.DataBindingComplete += dataGridView1_DataBindingComplete;
            // 
            // button1
            // 
            button1.Location = new Point(12, 349);
            button1.Name = "button1";
            button1.Size = new Size(166, 60);
            button1.TabIndex = 1;
            button1.Text = "Оформить приход";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(622, 349);
            button2.Name = "button2";
            button2.Size = new Size(166, 60);
            button2.TabIndex = 2;
            button2.Text = "Журнал";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // buttonZayavka
            // 
            buttonZayavka.Location = new Point(184, 352);
            buttonZayavka.Name = "buttonZayavka";
            buttonZayavka.Size = new Size(139, 55);
            buttonZayavka.TabIndex = 3;
            buttonZayavka.Text = "Оформить заявку на поставку";
            buttonZayavka.UseVisualStyleBackColor = true;
            buttonZayavka.Click += buttonZayavka_Click;
            // 
            // Sklad
            // 
            AutoScaleDimensions = new SizeF(7F, 14F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(buttonZayavka);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(dataGridView1);
            Name = "Sklad";
            Text = "Управление складом";
            Load += Sklad_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView1;
        private Button button1;
        private Button button2;
        private Button buttonZayavka;
    }
}