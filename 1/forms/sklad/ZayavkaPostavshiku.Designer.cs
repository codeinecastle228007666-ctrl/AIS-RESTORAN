namespace _1.forms.sklad
{
    partial class ZayavkaPostavshiku
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ComboBox comboBoxPostavshik;
        private System.Windows.Forms.ComboBox comboBoxProduct;
        private System.Windows.Forms.TextBox textBoxKolichestvo;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.DataGridView dataGridViewZayavki;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            comboBoxPostavshik = new ComboBox();
            comboBoxProduct = new ComboBox();
            textBoxKolichestvo = new TextBox();
            buttonAdd = new Button();
            buttonOpen = new Button();
            buttonDelete = new Button();
            dataGridViewZayavki = new DataGridView();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridViewZayavki).BeginInit();
            SuspendLayout();
            // 
            // comboBoxPostavshik
            // 
            comboBoxPostavshik.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxPostavshik.FormattingEnabled = true;
            comboBoxPostavshik.Location = new Point(105, 19);
            comboBoxPostavshik.Margin = new Padding(3, 2, 3, 2);
            comboBoxPostavshik.Name = "comboBoxPostavshik";
            comboBoxPostavshik.Size = new Size(219, 22);
            comboBoxPostavshik.TabIndex = 1;
            // 
            // comboBoxProduct
            // 
            comboBoxProduct.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxProduct.FormattingEnabled = true;
            comboBoxProduct.Location = new Point(105, 47);
            comboBoxProduct.Margin = new Padding(3, 2, 3, 2);
            comboBoxProduct.Name = "comboBoxProduct";
            comboBoxProduct.Size = new Size(219, 22);
            comboBoxProduct.TabIndex = 3;
            // 
            // textBoxKolichestvo
            // 
            textBoxKolichestvo.Location = new Point(105, 75);
            textBoxKolichestvo.Margin = new Padding(3, 2, 3, 2);
            textBoxKolichestvo.Name = "textBoxKolichestvo";
            textBoxKolichestvo.Size = new Size(132, 23);
            textBoxKolichestvo.TabIndex = 5;
            // 
            // buttonAdd
            // 
            buttonAdd.BackColor = Color.Transparent;
            buttonAdd.Location = new Point(105, 105);
            buttonAdd.Margin = new Padding(3, 2, 3, 2);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new Size(131, 28);
            buttonAdd.TabIndex = 6;
            buttonAdd.Text = "Создать заявку";
            buttonAdd.UseVisualStyleBackColor = false;
            buttonAdd.Click += buttonAdd_Click;
            // 
            // buttonOpen
            // 
            buttonOpen.Location = new Point(26, 147);
            buttonOpen.Margin = new Padding(3, 2, 3, 2);
            buttonOpen.Name = "buttonOpen";
            buttonOpen.Size = new Size(88, 24);
            buttonOpen.TabIndex = 7;
            buttonOpen.Text = "Открыть";
            buttonOpen.UseVisualStyleBackColor = true;
            buttonOpen.Click += buttonOpen_Click;
            // 
            // buttonDelete
            // 
            buttonDelete.BackColor = Color.Transparent;
            buttonDelete.Location = new Point(131, 147);
            buttonDelete.Margin = new Padding(3, 2, 3, 2);
            buttonDelete.Name = "buttonDelete";
            buttonDelete.Size = new Size(88, 24);
            buttonDelete.TabIndex = 8;
            buttonDelete.Text = "Удалить";
            buttonDelete.UseVisualStyleBackColor = false;
            buttonDelete.Click += buttonDelete_Click;
            // 
            // dataGridViewZayavki
            // 
            dataGridViewZayavki.AllowUserToAddRows = false;
            dataGridViewZayavki.AllowUserToDeleteRows = false;
            dataGridViewZayavki.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewZayavki.Location = new Point(26, 182);
            dataGridViewZayavki.Margin = new Padding(3, 2, 3, 2);
            dataGridViewZayavki.MultiSelect = false;
            dataGridViewZayavki.Name = "dataGridViewZayavki";
            dataGridViewZayavki.ReadOnly = true;
            dataGridViewZayavki.RowHeadersWidth = 51;
            dataGridViewZayavki.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewZayavki.Size = new Size(472, 196);
            dataGridViewZayavki.TabIndex = 9;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(26, 21);
            label1.Name = "label1";
            label1.Size = new Size(77, 14);
            label1.TabIndex = 0;
            label1.Text = "Поставщик:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(26, 49);
            label2.Name = "label2";
            label2.Size = new Size(63, 14);
            label2.TabIndex = 2;
            label2.Text = "Продукт:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(26, 77);
            label3.Name = "label3";
            label3.Size = new Size(84, 14);
            label3.TabIndex = 4;
            label3.Text = "Количество:";
            // 
            // ZayavkaPostavshiku
            // 
            AutoScaleDimensions = new SizeF(7F, 14F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(525, 399);
            Controls.Add(dataGridViewZayavki);
            Controls.Add(buttonDelete);
            Controls.Add(buttonOpen);
            Controls.Add(buttonAdd);
            Controls.Add(textBoxKolichestvo);
            Controls.Add(label3);
            Controls.Add(comboBoxProduct);
            Controls.Add(label2);
            Controls.Add(comboBoxPostavshik);
            Controls.Add(label1);
            Margin = new Padding(3, 2, 3, 2);
            Name = "ZayavkaPostavshiku";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Заявки поставщикам";
            Load += ZayavkaPostavshiku_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridViewZayavki).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}