namespace _1.forms
{
    partial class SelectRoleForm
    {
        private System.ComponentModel.IContainer components = null;
        private ComboBox comboBoxRole;
        private Button buttonOK;
        private Button buttonCancel;
        private Label labelRole;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.comboBoxRole = new ComboBox();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.labelRole = new Label();
            this.SuspendLayout();
            // 
            // comboBoxRole
            // 
            this.comboBoxRole.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxRole.FormattingEnabled = true;
            this.comboBoxRole.Location = new Point(86, 33);
            this.comboBoxRole.Name = "comboBoxRole";
            this.comboBoxRole.Size = new Size(200, 24);
            this.comboBoxRole.TabIndex = 0;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new Point(63, 85);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(100, 30);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new Point(186, 85);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(100, 30);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
            // 
            // labelRole
            // 
            this.labelRole.AutoSize = true;
            this.labelRole.Font = new Font("Segoe UI", 12F);
            this.labelRole.Location = new Point(12, 35);
            this.labelRole.Name = "labelRole";
            this.labelRole.Size = new Size(51, 21);
            this.labelRole.Text = "Роль:";
            // 
            // SelectRoleForm
            // 
            this.ClientSize = new Size(320, 140);
            this.Controls.Add(this.labelRole);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.comboBoxRole);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectRoleForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Выберите роль";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}