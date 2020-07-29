namespace CalcPro
{
    partial class frmAddDiscount
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
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.txtShortdescription = new System.Windows.Forms.RichTextBox();
            this.btbCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.txtDiscount = new DevExpress.XtraEditors.TextEdit();
            this.txtToOZ = new DevExpress.XtraEditors.TextEdit();
            this.txtFromOZ = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDiscount.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToOZ.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromOZ.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Appearance.Control.Font = new System.Drawing.Font("Bahnschrift Light", 10F);
            this.layoutControl1.Appearance.Control.Options.UseFont = true;
            this.layoutControl1.Appearance.ControlDisabled.Font = new System.Drawing.Font("Bahnschrift Light", 10F);
            this.layoutControl1.Appearance.ControlDisabled.Options.UseFont = true;
            this.layoutControl1.Appearance.ControlDropDown.Font = new System.Drawing.Font("Bahnschrift Light", 10F);
            this.layoutControl1.Appearance.ControlDropDown.Options.UseFont = true;
            this.layoutControl1.Appearance.ControlDropDownHeader.Font = new System.Drawing.Font("Bahnschrift Light", 10F);
            this.layoutControl1.Appearance.ControlDropDownHeader.Options.UseFont = true;
            this.layoutControl1.Appearance.ControlFocused.Font = new System.Drawing.Font("Bahnschrift Light", 10F);
            this.layoutControl1.Appearance.ControlFocused.Options.UseFont = true;
            this.layoutControl1.Appearance.ControlReadOnly.Font = new System.Drawing.Font("Bahnschrift Light", 10F);
            this.layoutControl1.Appearance.ControlReadOnly.Options.UseFont = true;
            this.layoutControl1.Controls.Add(this.txtShortdescription);
            this.layoutControl1.Controls.Add(this.btbCancel);
            this.layoutControl1.Controls.Add(this.btnSave);
            this.layoutControl1.Controls.Add(this.txtDiscount);
            this.layoutControl1.Controls.Add(this.txtToOZ);
            this.layoutControl1.Controls.Add(this.txtFromOZ);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(988, 213, 450, 400);
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(582, 331);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // txtShortdescription
            // 
            this.txtShortdescription.Location = new System.Drawing.Point(97, 118);
            this.txtShortdescription.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtShortdescription.MaxLength = 80;
            this.txtShortdescription.Name = "txtShortdescription";
            this.txtShortdescription.Size = new System.Drawing.Size(463, 140);
            this.txtShortdescription.TabIndex = 9;
            this.txtShortdescription.Text = "";
            // 
            // btbCancel
            // 
            this.btbCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btbCancel.ImageOptions.Image = global::CalcPro.Properties.Resources.Cancel_16x16;
            this.btbCancel.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btbCancel.Location = new System.Drawing.Point(290, 281);
            this.btbCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btbCancel.Name = "btbCancel";
            this.btbCancel.Size = new System.Drawing.Size(123, 28);
            this.btbCancel.StyleController = this.layoutControl1;
            this.btbCancel.TabIndex = 8;
            this.btbCancel.Text = "Abbrechen";
            this.btbCancel.Click += new System.EventHandler(this.btbCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.ImageOptions.Image = global::CalcPro.Properties.Resources.Save_16x16;
            this.btnSave.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnSave.Location = new System.Drawing.Point(417, 281);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(143, 28);
            this.btnSave.StyleController = this.layoutControl1;
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Speichern";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtDiscount
            // 
            this.txtDiscount.Location = new System.Drawing.Point(97, 86);
            this.txtDiscount.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDiscount.Name = "txtDiscount";
            this.txtDiscount.Properties.Mask.EditMask = "\\d{0,2}(\\R.\\d{0,2})";
            this.txtDiscount.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtDiscount.Properties.Mask.ShowPlaceHolders = false;
            this.txtDiscount.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtDiscount.Size = new System.Drawing.Size(463, 28);
            this.txtDiscount.StyleController = this.layoutControl1;
            this.txtDiscount.TabIndex = 6;
            this.txtDiscount.Enter += new System.EventHandler(this.txtToOZ_Enter);
            // 
            // txtToOZ
            // 
            this.txtToOZ.Location = new System.Drawing.Point(97, 54);
            this.txtToOZ.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtToOZ.Name = "txtToOZ";
            this.txtToOZ.Size = new System.Drawing.Size(463, 28);
            this.txtToOZ.StyleController = this.layoutControl1;
            this.txtToOZ.TabIndex = 5;
            this.txtToOZ.Enter += new System.EventHandler(this.txtToOZ_Enter);
            // 
            // txtFromOZ
            // 
            this.txtFromOZ.Location = new System.Drawing.Point(97, 22);
            this.txtFromOZ.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtFromOZ.Name = "txtFromOZ";
            this.txtFromOZ.Size = new System.Drawing.Size(463, 28);
            this.txtFromOZ.StyleController = this.layoutControl1;
            this.txtFromOZ.TabIndex = 4;
            this.txtFromOZ.Enter += new System.EventHandler(this.txtFromOZ_Enter);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.AppearanceGroup.Font = new System.Drawing.Font("Bahnschrift Light", 10F);
            this.layoutControlGroup1.AppearanceGroup.Options.UseFont = true;
            this.layoutControlGroup1.AppearanceItemCaption.Font = new System.Drawing.Font("Bahnschrift Light", 10F);
            this.layoutControlGroup1.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem5,
            this.emptySpaceItem1,
            this.layoutControlItem6,
            this.layoutControlItem4,
            this.emptySpaceItem2});
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(20, 20, 20, 20);
            this.layoutControlGroup1.Size = new System.Drawing.Size(582, 331);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.txtFromOZ;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(542, 32);
            this.layoutControlItem1.Text = "Von OZ";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(72, 21);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.txtToOZ;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 32);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(542, 32);
            this.layoutControlItem2.Text = "Bis OZ";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(72, 21);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.txtDiscount;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 64);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(542, 32);
            this.layoutControlItem3.Text = "Nachlass";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(72, 21);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.btbCancel;
            this.layoutControlItem5.Location = new System.Drawing.Point(268, 259);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(127, 32);
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 259);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(268, 32);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.txtShortdescription;
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 96);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(542, 144);
            this.layoutControlItem6.Text = "Kurz Text";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(72, 21);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.btnSave;
            this.layoutControlItem4.Location = new System.Drawing.Point(395, 259);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(147, 32);
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(0, 240);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(542, 19);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // frmAddDiscount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btbCancel;
            this.ClientSize = new System.Drawing.Size(582, 331);
            this.Controls.Add(this.layoutControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddDiscount";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Nachlass";
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtDiscount.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToOZ.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromOZ.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.SimpleButton btbCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.TextEdit txtDiscount;
        private DevExpress.XtraEditors.TextEdit txtToOZ;
        private DevExpress.XtraEditors.TextEdit txtFromOZ;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private System.Windows.Forms.RichTextBox txtShortdescription;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
    }
}