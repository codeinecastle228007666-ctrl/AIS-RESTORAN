namespace _1.forms.sklad
{
    partial class ZayavkaPostavshiku
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.ComboBox comboBoxPostavshik;
        private System.Windows.Forms.DataGridView dataGridViewZayavki;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.Button buttonDelete;

        private System.Windows.Forms.TextBox textBoxNomer;
        private System.Windows.Forms.TextBox textBoxDate;
        private System.Windows.Forms.TextBox textBoxOtKogo;
        private System.Windows.Forms.TextBox textBoxAdres;
        private System.Windows.Forms.TextBox textBoxBody;
        private System.Windows.Forms.DataGridView dataGridViewItems;
        private System.Windows.Forms.Button buttonAddRow;
        private System.Windows.Forms.Button buttonRemoveRow;
        private System.Windows.Forms.Button buttonCreate;

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            comboBoxPostavshik = new ComboBox();
            dataGridViewZayavki = new DataGridView();
            buttonOpen = new Button();
            buttonDelete = new Button();
            textBoxNomer = new TextBox();
            textBoxDate = new TextBox();
            textBoxOtKogo = new TextBox();
            textBoxAdres = new TextBox();
            textBoxBody = new TextBox();
            dataGridViewItems = new DataGridView();
            buttonAddRow = new Button();
            buttonRemoveRow = new Button();
            buttonCreate = new Button();
            label1 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label8 = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridViewZayavki).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewItems).BeginInit();
            SuspendLayout();

            // Row 1 — Поставщик + Адрес
            label1.AutoSize = true;
            label1.Location = new Point(12, 15);
            label1.Name = "label1";
            label1.Size = new Size(77, 14);
            label1.TabIndex = 0;
            label1.Text = "Поставщик:";

            comboBoxPostavshik.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxPostavshik.FormattingEnabled = true;
            comboBoxPostavshik.Location = new Point(90, 12);
            comboBoxPostavshik.Name = "comboBoxPostavshik";
            comboBoxPostavshik.Size = new Size(260, 22);
            comboBoxPostavshik.TabIndex = 1;
            comboBoxPostavshik.SelectedIndexChanged += comboBoxPostavshik_SelectedIndexChanged;

            label8.AutoSize = true;
            label8.Location = new Point(370, 15);
            label8.Name = "label8";
            label8.Size = new Size(48, 14);
            label8.TabIndex = 0;
            label8.Text = "Адрес:";

            textBoxAdres.Location = new Point(424, 12);
            textBoxAdres.Name = "textBoxAdres";
            textBoxAdres.ReadOnly = true;
            textBoxAdres.Size = new Size(350, 23);
            textBoxAdres.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBoxAdres.TabIndex = 2;

            // Row 2 — От кого + № + Дата
            label6.AutoSize = true;
            label6.Location = new Point(12, 45);
            label6.Name = "label6";
            label6.Size = new Size(68, 14);
            label6.TabIndex = 0;
            label6.Text = "От кого:";

            textBoxOtKogo.Location = new Point(90, 42);
            textBoxOtKogo.Name = "textBoxOtKogo";
            textBoxOtKogo.ReadOnly = true;
            textBoxOtKogo.Size = new Size(260, 23);
            textBoxOtKogo.TabIndex = 3;

            label4.AutoSize = true;
            label4.Location = new Point(370, 45);
            label4.Name = "label4";
            label4.Size = new Size(19, 14);
            label4.TabIndex = 0;
            label4.Text = "№:";

            textBoxNomer.Location = new Point(395, 42);
            textBoxNomer.Name = "textBoxNomer";
            textBoxNomer.ReadOnly = true;
            textBoxNomer.Size = new Size(130, 23);
            textBoxNomer.TabIndex = 4;

            label5.AutoSize = true;
            label5.Location = new Point(545, 45);
            label5.Name = "label5";
            label5.Size = new Size(39, 14);
            label5.TabIndex = 0;
            label5.Text = "Дата:";

            textBoxDate.Location = new Point(590, 42);
            textBoxDate.Name = "textBoxDate";
            textBoxDate.ReadOnly = true;
            textBoxDate.Size = new Size(130, 23);
            textBoxDate.TabIndex = 5;

            // Row 3 — Текст заявки (многострочный) + кнопка справа
            textBoxBody.Location = new Point(12, 76);
            textBoxBody.Name = "textBoxBody";
            textBoxBody.Size = new Size(540, 50);
            textBoxBody.Multiline = true;
            textBoxBody.TabIndex = 6;
            textBoxBody.Text = "Прошу зарезервировать и поставить до _________________ следующие наименования товара:";

            buttonCreate.Location = new Point(570, 76);
            buttonCreate.Name = "buttonCreate";
            buttonCreate.Size = new Size(200, 50);
            buttonCreate.TabIndex = 7;
            buttonCreate.Text = "Сформировать заявку (Excel)";
            buttonCreate.UseVisualStyleBackColor = true;
            buttonCreate.Click += buttonCreate_Click;

            // Табличная часть
            dataGridViewItems.AllowUserToAddRows = false;
            dataGridViewItems.AllowUserToDeleteRows = false;
            dataGridViewItems.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewItems.Location = new Point(12, 138);
            dataGridViewItems.MultiSelect = false;
            dataGridViewItems.Name = "dataGridViewItems";
            dataGridViewItems.RowHeadersWidth = 40;
            dataGridViewItems.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dataGridViewItems.Size = new Size(760, 200);
            dataGridViewItems.TabIndex = 8;

            buttonAddRow.Location = new Point(12, 344);
            buttonAddRow.Name = "buttonAddRow";
            buttonAddRow.Size = new Size(100, 26);
            buttonAddRow.TabIndex = 9;
            buttonAddRow.Text = "+ Добавить";
            buttonAddRow.UseVisualStyleBackColor = true;
            buttonAddRow.Click += buttonAddRow_Click;

            buttonRemoveRow.Location = new Point(118, 344);
            buttonRemoveRow.Name = "buttonRemoveRow";
            buttonRemoveRow.Size = new Size(100, 26);
            buttonRemoveRow.TabIndex = 10;
            buttonRemoveRow.Text = "- Удалить";
            buttonRemoveRow.UseVisualStyleBackColor = true;
            buttonRemoveRow.Click += buttonRemoveRow_Click;

            // Архив
            dataGridViewZayavki.AllowUserToAddRows = false;
            dataGridViewZayavki.AllowUserToDeleteRows = false;
            dataGridViewZayavki.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewZayavki.Location = new Point(12, 385);
            dataGridViewZayavki.MultiSelect = false;
            dataGridViewZayavki.Name = "dataGridViewZayavki";
            dataGridViewZayavki.ReadOnly = true;
            dataGridViewZayavki.RowHeadersWidth = 51;
            dataGridViewZayavki.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewZayavki.Size = new Size(760, 150);
            dataGridViewZayavki.TabIndex = 11;

            buttonOpen.Location = new Point(12, 541);
            buttonOpen.Name = "buttonOpen";
            buttonOpen.Size = new Size(100, 26);
            buttonOpen.TabIndex = 12;
            buttonOpen.Text = "Открыть";
            buttonOpen.UseVisualStyleBackColor = true;
            buttonOpen.Click += buttonOpen_Click;

            buttonDelete.Location = new Point(118, 541);
            buttonDelete.Name = "buttonDelete";
            buttonDelete.Size = new Size(100, 26);
            buttonDelete.TabIndex = 13;
            buttonDelete.Text = "Удалить";
            buttonDelete.UseVisualStyleBackColor = false;
            buttonDelete.Click += buttonDelete_Click;

            // Form
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(790, 580);
            Controls.Add(buttonDelete);
            Controls.Add(buttonOpen);
            Controls.Add(dataGridViewZayavki);
            Controls.Add(buttonRemoveRow);
            Controls.Add(buttonAddRow);
            Controls.Add(dataGridViewItems);
            Controls.Add(buttonCreate);
            Controls.Add(textBoxBody);
            Controls.Add(textBoxDate);
            Controls.Add(label5);
            Controls.Add(textBoxNomer);
            Controls.Add(label4);
            Controls.Add(textBoxOtKogo);
            Controls.Add(label6);
            Controls.Add(textBoxAdres);
            Controls.Add(label8);
            Controls.Add(comboBoxPostavshik);
            Controls.Add(label1);
            Name = "ZayavkaPostavshiku";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Заявки поставщикам";
            Load += ZayavkaPostavshiku_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridViewZayavki).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewItems).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
