namespace _1.forms.Menu
{
    partial class SostavBludaForm
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
            comboBoxProduct = new ComboBox();
            textBoxKolvo = new TextBox();
            button1 = new Button();
            button2 = new Button();
            label1 = new Label();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Top;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(800, 305);
            dataGridView1.TabIndex = 0;
            // 
            // comboBoxProduct
            // 
            comboBoxProduct.FormattingEnabled = true;
            comboBoxProduct.Location = new Point(12, 335);
            comboBoxProduct.Name = "comboBoxProduct";
            comboBoxProduct.Size = new Size(121, 22);
            comboBoxProduct.TabIndex = 1;
            // 
            // textBoxKolvo
            // 
            textBoxKolvo.Location = new Point(183, 334);
            textBoxKolvo.Name = "textBoxKolvo";
            textBoxKolvo.Size = new Size(100, 23);
            textBoxKolvo.TabIndex = 2;
            // 
            // button1
            // 
            button1.Location = new Point(12, 400);
            button1.Name = "button1";
            button1.Size = new Size(84, 38);
            button1.TabIndex = 3;
            button1.Text = "Добавить";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(137, 400);
            button2.Name = "button2";
            button2.Size = new Size(84, 38);
            button2.TabIndex = 4;
            button2.Text = "Удалить";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 318);
            label1.Name = "label1";
            label1.Size = new Size(56, 14);
            label1.TabIndex = 5;
            label1.Text = "Продукт";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(183, 318);
            label2.Name = "label2";
            label2.Size = new Size(77, 14);
            label2.TabIndex = 6;
            label2.Text = "Количество";
            // 
            // SostavBludaForm
            // 
            AutoScaleDimensions = new SizeF(7F, 14F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(textBoxKolvo);
            Controls.Add(comboBoxProduct);
            Controls.Add(dataGridView1);
            Name = "SostavBludaForm";
            Text = "Состав блюда";
            Load += SostavBludaForm_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView1;
        private ComboBox comboBoxProduct;
        private TextBox textBoxKolvo;
        private Button button1;
        private Button button2;
        private Label label1;
        private Label label2;
    }
}