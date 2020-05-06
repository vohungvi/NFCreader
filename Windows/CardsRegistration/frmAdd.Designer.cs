namespace CardsRegistration
{
    partial class frmAdd
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnUploadCard = new System.Windows.Forms.Button();
            this.lblBuilding = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.grdData = new System.Windows.Forms.DataGridView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnUploadCard);
            this.panel1.Controls.Add(this.lblBuilding);
            this.panel1.Controls.Add(this.lblUser);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(611, 67);
            this.panel1.TabIndex = 24;
            // 
            // btnUploadCard
            // 
            this.btnUploadCard.Location = new System.Drawing.Point(194, 12);
            this.btnUploadCard.Margin = new System.Windows.Forms.Padding(2);
            this.btnUploadCard.Name = "btnUploadCard";
            this.btnUploadCard.Size = new System.Drawing.Size(124, 25);
            this.btnUploadCard.TabIndex = 31;
            this.btnUploadCard.Text = "Upload card";
            this.btnUploadCard.UseVisualStyleBackColor = true;
            this.btnUploadCard.Click += new System.EventHandler(this.btnUploadCard_ClickAsync);
            // 
            // lblBuilding
            // 
            this.lblBuilding.AutoSize = true;
            this.lblBuilding.Location = new System.Drawing.Point(370, 34);
            this.lblBuilding.Name = "lblBuilding";
            this.lblBuilding.Size = new System.Drawing.Size(44, 13);
            this.lblBuilding.TabIndex = 29;
            this.lblBuilding.Text = "Building";
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(370, 9);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(29, 13);
            this.lblUser.TabIndex = 28;
            this.lblUser.Text = "User";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(11, 11);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(124, 25);
            this.btnRefresh.TabIndex = 26;
            this.btnRefresh.Text = "Connect Card reader";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // grdData
            // 
            this.grdData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdData.Location = new System.Drawing.Point(0, 67);
            this.grdData.Margin = new System.Windows.Forms.Padding(2);
            this.grdData.Name = "grdData";
            this.grdData.RowTemplate.Height = 28;
            this.grdData.Size = new System.Drawing.Size(611, 344);
            this.grdData.TabIndex = 25;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblMessage});
            this.statusStrip1.Location = new System.Drawing.Point(0, 411);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(611, 22);
            this.statusStrip1.TabIndex = 26;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblMessage
            // 
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(0, 17);
            // 
            // frmAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(611, 433);
            this.Controls.Add(this.grdData);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmAdd";
            this.Text = "Thêm thẻ mới";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridView grdData;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblMessage;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label lblBuilding;
        private System.Windows.Forms.Button btnUploadCard;
    }
}

