namespace OTTOPro
{
    partial class frmAdhocPosition
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.txtSurchargeTo = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txtSurchargeFrom = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtSurchargePer = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtMaxOZ = new DevExpress.XtraEditors.TextEdit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSurchargeTo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSurchargeFrom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSurchargePer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaxOZ.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.txtSurchargeTo, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.labelControl4, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtSurchargeFrom, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelControl3, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelControl2, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtSurchargePer, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelControl1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtMaxOZ, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18.54771F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18.54771F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.98904F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.8589F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.53988F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(310, 190);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.btnOK, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnCancel, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(115, 126);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(172, 30);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(3, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 24);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.Location = new System.Drawing.Point(89, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 24);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtSurchargeTo
            // 
            this.txtSurchargeTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSurchargeTo.Location = new System.Drawing.Point(115, 92);
            this.txtSurchargeTo.Name = "txtSurchargeTo";
            this.txtSurchargeTo.Size = new System.Drawing.Size(172, 20);
            this.txtSurchargeTo.TabIndex = 2;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(23, 92);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(74, 13);
            this.labelControl4.TabIndex = 1;
            this.labelControl4.Text = "Surcharge To : ";
            // 
            // txtSurchargeFrom
            // 
            this.txtSurchargeFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSurchargeFrom.Location = new System.Drawing.Point(115, 63);
            this.txtSurchargeFrom.Name = "txtSurchargeFrom";
            this.txtSurchargeFrom.Size = new System.Drawing.Size(172, 20);
            this.txtSurchargeFrom.TabIndex = 2;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(23, 63);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(86, 13);
            this.labelControl3.TabIndex = 1;
            this.labelControl3.Text = "Surcharge From : ";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(23, 33);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(73, 13);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "Surcharge % : ";
            // 
            // txtSurchargePer
            // 
            this.txtSurchargePer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSurchargePer.Location = new System.Drawing.Point(115, 33);
            this.txtSurchargePer.Name = "txtSurchargePer";
            this.txtSurchargePer.Size = new System.Drawing.Size(172, 20);
            this.txtSurchargePer.TabIndex = 2;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(23, 3);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(47, 13);
            this.labelControl1.TabIndex = 5;
            this.labelControl1.Text = "Max OZ : ";
            // 
            // txtMaxOZ
            // 
            this.txtMaxOZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMaxOZ.Enabled = false;
            this.txtMaxOZ.Location = new System.Drawing.Point(115, 3);
            this.txtMaxOZ.Name = "txtMaxOZ";
            this.txtMaxOZ.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMaxOZ.Properties.Appearance.ForeColor = System.Drawing.Color.Red;
            this.txtMaxOZ.Properties.Appearance.Options.UseFont = true;
            this.txtMaxOZ.Properties.Appearance.Options.UseForeColor = true;
            this.txtMaxOZ.Size = new System.Drawing.Size(172, 22);
            this.txtMaxOZ.TabIndex = 6;
            // 
            // frmAdhocPosition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 190);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmAdhocPosition";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Adhoc Position";
            this.Load += new System.EventHandler(this.frmAdhocPosition_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtSurchargeTo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSurchargeFrom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSurchargePer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaxOZ.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit txtSurchargePer;
        private DevExpress.XtraEditors.TextEdit txtSurchargeFrom;
        private DevExpress.XtraEditors.TextEdit txtSurchargeTo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtMaxOZ;
    }
}