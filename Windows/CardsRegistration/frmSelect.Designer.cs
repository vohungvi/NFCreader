namespace CardsRegistration
{
    partial class frmSelect
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
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnCheck = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.lblBuilding = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.Enabled = false;
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(80, 76);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(210, 54);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Thêm thẻ mới";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // btnCheck
            // 
            this.btnCheck.Enabled = false;
            this.btnCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheck.Location = new System.Drawing.Point(339, 76);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(210, 54);
            this.btnCheck.TabIndex = 1;
            this.btnCheck.Text = "Kiểm kê thẻ";
            this.btnCheck.UseVisualStyleBackColor = true;
            this.btnCheck.Click += new System.EventHandler(this.BtnCheck_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(590, 12);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(75, 23);
            this.btnLogout.TabIndex = 28;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.BtnLogout_Click);
            // 
            // lblBuilding
            // 
            this.lblBuilding.AutoSize = true;
            this.lblBuilding.Location = new System.Drawing.Point(401, 34);
            this.lblBuilding.Name = "lblBuilding";
            this.lblBuilding.Size = new System.Drawing.Size(44, 13);
            this.lblBuilding.TabIndex = 31;
            this.lblBuilding.Text = "Building";
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(401, 9);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(29, 13);
            this.lblUser.TabIndex = 30;
            this.lblUser.Text = "User";
            // 
            // frmSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(677, 208);
            this.Controls.Add(this.lblBuilding);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnCheck);
            this.Controls.Add(this.btnAdd);
            this.MaximizeBox = false;
            this.Name = "frmSelect";
            this.Text = "Lựa chọn công việc";            
            this.Load += new System.EventHandler(this.FrmSelect_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnCheck;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Label lblBuilding;
        private System.Windows.Forms.Label lblUser;
    }
}