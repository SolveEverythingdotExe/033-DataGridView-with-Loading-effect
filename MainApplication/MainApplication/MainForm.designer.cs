namespace MainApplication
{
    partial class MainForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView = new MainApplication.ProgressDataGridView();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.dataGridViewColumnId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewColumnPayer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewColumnPayee = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewColumnAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewColumnId,
            this.dataGridViewColumnPayer,
            this.dataGridViewColumnPayee,
            this.dataGridViewColumnAmount});
            this.dataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView.Location = new System.Drawing.Point(12, 12);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(444, 286);
            this.dataGridView.TabIndex = 0;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(184, 314);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // dataGridViewColumnId
            // 
            this.dataGridViewColumnId.DataPropertyName = "Id";
            this.dataGridViewColumnId.HeaderText = "Id";
            this.dataGridViewColumnId.Name = "dataGridViewColumnId";
            this.dataGridViewColumnId.ReadOnly = true;
            this.dataGridViewColumnId.Visible = false;
            // 
            // dataGridViewColumnPayer
            // 
            this.dataGridViewColumnPayer.DataPropertyName = "Payer";
            this.dataGridViewColumnPayer.HeaderText = "Payer";
            this.dataGridViewColumnPayer.Name = "dataGridViewColumnPayer";
            this.dataGridViewColumnPayer.ReadOnly = true;
            this.dataGridViewColumnPayer.Width = 150;
            // 
            // dataGridViewColumnPayee
            // 
            this.dataGridViewColumnPayee.DataPropertyName = "Payee";
            this.dataGridViewColumnPayee.HeaderText = "Payee";
            this.dataGridViewColumnPayee.Name = "dataGridViewColumnPayee";
            this.dataGridViewColumnPayee.ReadOnly = true;
            this.dataGridViewColumnPayee.Width = 150;
            // 
            // dataGridViewColumnAmount
            // 
            this.dataGridViewColumnAmount.DataPropertyName = "Amount";
            dataGridViewCellStyle4.Format = "N2";
            this.dataGridViewColumnAmount.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewColumnAmount.HeaderText = "Amount";
            this.dataGridViewColumnAmount.Name = "dataGridViewColumnAmount";
            this.dataGridViewColumnAmount.ReadOnly = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 360);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.dataGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main Form";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MainApplication.ProgressDataGridView dataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewColumnId;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewColumnPayer;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewColumnPayee;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewColumnAmount;
        private System.Windows.Forms.Button btnRefresh;
    }
}

