namespace _1.forms
{
    partial class Kuhnya
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

        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            labelTitle = new Label();
            labelOrderCount = new Label();
            buttonMarkReady = new Button();
            buttonRefresh = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(12, 112);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(600, 261);
            dataGridView1.TabIndex = 0;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            // 
            // labelTitle
            // 
            labelTitle.AutoSize = true;
            labelTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            labelTitle.Location = new Point(218, 9);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(192, 26);
            labelTitle.TabIndex = 1;
            labelTitle.Text = "Заказы на кухне";
            // 
            // labelOrderCount
            // 
            labelOrderCount.AutoSize = true;
            labelOrderCount.Font = new Font("Segoe UI", 12F);
            labelOrderCount.Location = new Point(12, 21);
            labelOrderCount.Name = "labelOrderCount";
            labelOrderCount.Size = new Size(0, 19);
            labelOrderCount.TabIndex = 2;
            // 
            // buttonMarkReady
            // 
            buttonMarkReady.Location = new Point(12, 379);
            buttonMarkReady.Name = "buttonMarkReady";
            buttonMarkReady.Size = new Size(200, 47);
            buttonMarkReady.TabIndex = 3;
            buttonMarkReady.Text = "Изменить статус";
            buttonMarkReady.UseVisualStyleBackColor = true;
            buttonMarkReady.Click += buttonMarkReady_Click;
            // 
            // buttonRefresh
            // 
            buttonRefresh.Location = new Point(218, 379);
            buttonRefresh.Name = "buttonRefresh";
            buttonRefresh.Size = new Size(120, 47);
            buttonRefresh.TabIndex = 4;
            buttonRefresh.Text = "Обновить";
            buttonRefresh.UseVisualStyleBackColor = true;
            buttonRefresh.Click += buttonRefresh_Click;
            // 
            // Kuhnya
            // 
            AutoScaleDimensions = new SizeF(7F, 14F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(624, 438);
            Controls.Add(buttonRefresh);
            Controls.Add(buttonMarkReady);
            Controls.Add(labelOrderCount);
            Controls.Add(labelTitle);
            Controls.Add(dataGridView1);
            Name = "Kuhnya";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Кухня — Заказы";
            FormClosing += Kuhnya_FormClosing;
            Load += Kuhnya_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private DataGridView dataGridView1;
        private Label labelTitle;
        private Label labelOrderCount;
        private Button buttonMarkReady;
        private Button buttonRefresh;
    }
}
