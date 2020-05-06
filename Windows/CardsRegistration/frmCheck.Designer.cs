namespace CardsRegistration
{
    partial class frmCheck
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
            this.grdData = new System.Windows.Forms.DataGridView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblBuilding = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.btnFinish = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdData
            // 
            this.grdData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdData.Location = new System.Drawing.Point(0, 65);
            this.grdData.Name = "grdData";
            this.grdData.ReadOnly = true;
            this.grdData.Size = new System.Drawing.Size(800, 363);
            this.grdData.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblMessage});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblMessage
            // 
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(0, 17);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblBuilding);
            this.groupBox1.Controls.Add(this.lblUser);
            this.groupBox1.Controls.Add(this.btnFinish);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(800, 65);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // lblBuilding
            // 
            this.lblBuilding.AutoSize = true;
            this.lblBuilding.Location = new System.Drawing.Point(12, 40);
            this.lblBuilding.Name = "lblBuilding";
            this.lblBuilding.Size = new System.Drawing.Size(44, 13);
            this.lblBuilding.TabIndex = 31;
            this.lblBuilding.Text = "Building";
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(12, 15);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(29, 13);
            this.lblUser.TabIndex = 30;
            this.lblUser.Text = "User";
            // 
            // btnFinish
            // 
            this.btnFinish.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFinish.Location = new System.Drawing.Point(673, 14);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(96, 39);
            this.btnFinish.TabIndex = 0;
            this.btnFinish.Text = "Hoàn tất";
            this.btnFinish.UseVisualStyleBackColor = true;
            this.btnFinish.Click += new System.EventHandler(this.BtnFinish_Click);
            // 
            // frmCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.grdData);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "frmCheck";
            this.Text = "Kiểm kê thẻ";
            this.Load += new System.EventHandler(this.FrmCheck_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView grdData;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblMessage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnFinish;
        private System.Windows.Forms.Label lblBuilding;
        private System.Windows.Forms.Label lblUser;
    }
}