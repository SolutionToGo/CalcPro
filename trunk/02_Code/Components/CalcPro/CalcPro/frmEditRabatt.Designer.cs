namespace CalcPro
{
    partial class frmEditRabatt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEditRabatt));
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.txtMulti4 = new DevExpress.XtraEditors.TextEdit();
            this.txtMulti3 = new DevExpress.XtraEditors.TextEdit();
            this.txtMulti2 = new DevExpress.XtraEditors.TextEdit();
            this.txtMulti1 = new DevExpress.XtraEditors.TextEdit();
            this.dtpValidityDate = new DevExpress.XtraEditors.DateEdit();
            this.txtRabatt = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem4 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem5 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem6 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMulti4.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMulti3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMulti2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMulti1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpValidityDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpValidityDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRabatt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(158)))), ((int)(((byte)(224)))));
            this.layoutControl1.Controls.Add(this.btnCancel);
            this.layoutControl1.Controls.Add(this.btnSave);
            this.layoutControl1.Controls.Add(this.txtMulti4);
            this.layoutControl1.Controls.Add(this.txtMulti3);
            this.layoutControl1.Controls.Add(this.txtMulti2);
            this.layoutControl1.Controls.Add(this.txtMulti1);
            this.layoutControl1.Controls.Add(this.dtpValidityDate);
            this.layoutControl1.Controls.Add(this.txtRabatt);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(978, 171, 450, 400);
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(507, 258);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.ImageOptions.Image")));
            this.btnCancel.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnCancel.Location = new System.Drawing.Point(265, 195);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(91, 22);
            this.btnCancel.StyleController = this.layoutControl1;
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Abbrechen";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.ImageOptions.Image")));
            this.btnSave.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnSave.Location = new System.Drawing.Point(360, 195);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(93, 22);
            this.btnSave.StyleController = this.layoutControl1;
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Speichern";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtMulti4
            // 
            this.txtMulti4.EnterMoveNextControl = true;
            this.txtMulti4.Location = new System.Drawing.Point(164, 161);
            this.txtMulti4.Name = "txtMulti4";
            this.txtMulti4.Size = new System.Drawing.Size(289, 20);
            this.txtMulti4.StyleController = this.layoutControl1;
            this.txtMulti4.TabIndex = 9;
            this.txtMulti4.Enter += new System.EventHandler(this.txtRabatt_Enter);
            // 
            // txtMulti3
            // 
            this.txtMulti3.EnterMoveNextControl = true;
            this.txtMulti3.Location = new System.Drawing.Point(164, 137);
            this.txtMulti3.Name = "txtMulti3";
            this.txtMulti3.Size = new System.Drawing.Size(289, 20);
            this.txtMulti3.StyleController = this.layoutControl1;
            this.txtMulti3.TabIndex = 8;
            this.txtMulti3.Enter += new System.EventHandler(this.txtRabatt_Enter);
            // 
            // txtMulti2
            // 
            this.txtMulti2.EnterMoveNextControl = true;
            this.txtMulti2.Location = new System.Drawing.Point(164, 113);
            this.txtMulti2.Name = "txtMulti2";
            this.txtMulti2.Size = new System.Drawing.Size(289, 20);
            this.txtMulti2.StyleController = this.layoutControl1;
            this.txtMulti2.TabIndex = 7;
            this.txtMulti2.Enter += new System.EventHandler(this.txtRabatt_Enter);
            // 
            // txtMulti1
            // 
            this.txtMulti1.EnterMoveNextControl = true;
            this.txtMulti1.Location = new System.Drawing.Point(164, 89);
            this.txtMulti1.Name = "txtMulti1";
            this.txtMulti1.Size = new System.Drawing.Size(289, 20);
            this.txtMulti1.StyleController = this.layoutControl1;
            this.txtMulti1.TabIndex = 6;
            this.txtMulti1.Enter += new System.EventHandler(this.txtRabatt_Enter);
            // 
            // dtpValidityDate
            // 
            this.dtpValidityDate.EditValue = null;
            this.dtpValidityDate.Enabled = false;
            this.dtpValidityDate.EnterMoveNextControl = true;
            this.dtpValidityDate.Location = new System.Drawing.Point(164, 65);
            this.dtpValidityDate.Name = "dtpValidityDate";
            this.dtpValidityDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtpValidityDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtpValidityDate.Properties.CalendarView = DevExpress.XtraEditors.Repository.CalendarView.Vista;
            this.dtpValidityDate.Properties.DisplayFormat.FormatString = "y";
            this.dtpValidityDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtpValidityDate.Properties.EditFormat.FormatString = "y";
            this.dtpValidityDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtpValidityDate.Properties.Mask.EditMask = "y";
            this.dtpValidityDate.Properties.VistaCalendarViewStyle = DevExpress.XtraEditors.VistaCalendarViewStyle.YearView;
            this.dtpValidityDate.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.True;
            this.dtpValidityDate.Size = new System.Drawing.Size(289, 20);
            this.dtpValidityDate.StyleController = this.layoutControl1;
            this.dtpValidityDate.TabIndex = 5;
            // 
            // txtRabatt
            // 
            this.txtRabatt.Enabled = false;
            this.txtRabatt.EnterMoveNextControl = true;
            this.txtRabatt.Location = new System.Drawing.Point(164, 41);
            this.txtRabatt.Name = "txtRabatt";
            this.txtRabatt.Size = new System.Drawing.Size(289, 20);
            this.txtRabatt.StyleController = this.layoutControl1;
            this.txtRabatt.TabIndex = 4;
            this.txtRabatt.Enter += new System.EventHandler(this.txtRabatt_Enter);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.layoutControlGroup1.AppearanceItemCaption.ForeColor = System.Drawing.Color.White;
            this.layoutControlGroup1.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlGroup1.AppearanceItemCaption.Options.UseForeColor = true;
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.emptySpaceItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.emptySpaceItem2,
            this.emptySpaceItem3,
            this.emptySpaceItem4,
            this.layoutControlItem8,
            this.emptySpaceItem5,
            this.emptySpaceItem6,
            this.layoutControlItem7});
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Size = new System.Drawing.Size(507, 258);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.txtRabatt;
            this.layoutControlItem1.Location = new System.Drawing.Point(41, 29);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(404, 24);
            this.layoutControlItem1.Text = "Rabatt";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(108, 14);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(41, 183);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(212, 26);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.dtpValidityDate;
            this.layoutControlItem2.Location = new System.Drawing.Point(41, 53);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(404, 24);
            this.layoutControlItem2.Text = "Gültigkeit Datum ";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(108, 14);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.txtMulti1;
            this.layoutControlItem3.Location = new System.Drawing.Point(41, 77);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(404, 24);
            this.layoutControlItem3.Text = "Multi1";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(108, 14);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.txtMulti2;
            this.layoutControlItem4.Location = new System.Drawing.Point(41, 101);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(404, 24);
            this.layoutControlItem4.Text = "Multi2";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(108, 14);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.txtMulti3;
            this.layoutControlItem5.Location = new System.Drawing.Point(41, 125);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(404, 24);
            this.layoutControlItem5.Text = "Multi3";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(108, 14);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.txtMulti4;
            this.layoutControlItem6.Location = new System.Drawing.Point(41, 149);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(404, 24);
            this.layoutControlItem6.Text = "Multi4";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(108, 14);
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(41, 0);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(404, 29);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem3
            // 
            this.emptySpaceItem3.AllowHotTrack = false;
            this.emptySpaceItem3.Location = new System.Drawing.Point(445, 0);
            this.emptySpaceItem3.Name = "emptySpaceItem3";
            this.emptySpaceItem3.Size = new System.Drawing.Size(42, 238);
            this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem4
            // 
            this.emptySpaceItem4.AllowHotTrack = false;
            this.emptySpaceItem4.Location = new System.Drawing.Point(0, 0);
            this.emptySpaceItem4.Name = "emptySpaceItem4";
            this.emptySpaceItem4.Size = new System.Drawing.Size(41, 238);
            this.emptySpaceItem4.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.btnCancel;
            this.layoutControlItem8.Location = new System.Drawing.Point(253, 183);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(95, 26);
            this.layoutControlItem8.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem8.TextVisible = false;
            // 
            // emptySpaceItem5
            // 
            this.emptySpaceItem5.AllowHotTrack = false;
            this.emptySpaceItem5.Location = new System.Drawing.Point(41, 173);
            this.emptySpaceItem5.Name = "emptySpaceItem5";
            this.emptySpaceItem5.Size = new System.Drawing.Size(404, 10);
            this.emptySpaceItem5.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem6
            // 
            this.emptySpaceItem6.AllowHotTrack = false;
            this.emptySpaceItem6.Location = new System.Drawing.Point(41, 209);
            this.emptySpaceItem6.Name = "emptySpaceItem6";
            this.emptySpaceItem6.Size = new System.Drawing.Size(404, 29);
            this.emptySpaceItem6.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.btnSave;
            this.layoutControlItem7.Location = new System.Drawing.Point(348, 183);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(97, 26);
            this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem7.TextVisible = false;
            // 
            // frmEditRabatt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(507, 258);
            this.Controls.Add(this.layoutControl1);
            this.Name = "frmEditRabatt";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Bearbeitung der Rabattgruppe";
            this.Load += new System.EventHandler(this.frmEditRabatt_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtMulti4.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMulti3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMulti2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMulti1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpValidityDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpValidityDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRabatt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.TextEdit txtRabatt;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraEditors.DateEdit dtpValidityDate;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraEditors.TextEdit txtMulti4;
        private DevExpress.XtraEditors.TextEdit txtMulti3;
        private DevExpress.XtraEditors.TextEdit txtMulti2;
        private DevExpress.XtraEditors.TextEdit txtMulti1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem4;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem5;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem6;
    }
}