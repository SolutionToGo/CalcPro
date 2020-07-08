namespace CalcPro
{
    partial class frmOTTOMaster
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
            this.btnCancelContact = new DevExpress.XtraEditors.SimpleButton();
            this.btnSaveContact = new DevExpress.XtraEditors.SimpleButton();
            this.txtContactPerson = new DevExpress.XtraEditors.TextEdit();
            this.checkEditDefaultContact = new DevExpress.XtraEditors.CheckEdit();
            this.txtTelephone = new DevExpress.XtraEditors.TextEdit();
            this.txtTaxNo = new DevExpress.XtraEditors.TextEdit();
            this.txtFax = new DevExpress.XtraEditors.TextEdit();
            this.txtemail = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem4 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem5 = new DevExpress.XtraLayout.EmptySpaceItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtContactPerson.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditDefaultContact.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTelephone.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTaxNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFax.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtemail.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem5)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(158)))), ((int)(((byte)(224)))));
            this.layoutControl1.Controls.Add(this.btnCancelContact);
            this.layoutControl1.Controls.Add(this.btnSaveContact);
            this.layoutControl1.Controls.Add(this.txtContactPerson);
            this.layoutControl1.Controls.Add(this.checkEditDefaultContact);
            this.layoutControl1.Controls.Add(this.txtTelephone);
            this.layoutControl1.Controls.Add(this.txtTaxNo);
            this.layoutControl1.Controls.Add(this.txtFax);
            this.layoutControl1.Controls.Add(this.txtemail);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1255, 288, 450, 400);
            this.layoutControl1.OptionsFocus.EnableAutoTabOrder = false;
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(561, 257);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // btnCancelContact
            // 
            this.btnCancelContact.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelContact.ImageOptions.Image = global::CalcPro.Properties.Resources.Cancel_16x16;
            this.btnCancelContact.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnCancelContact.Location = new System.Drawing.Point(300, 188);
            this.btnCancelContact.Name = "btnCancelContact";
            this.btnCancelContact.Size = new System.Drawing.Size(98, 22);
            this.btnCancelContact.StyleController = this.layoutControl1;
            this.btnCancelContact.TabIndex = 8;
            this.btnCancelContact.Text = "Abbrechen";
            this.btnCancelContact.Click += new System.EventHandler(this.btnCancelOtto_Click);
            // 
            // btnSaveContact
            // 
            this.btnSaveContact.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSaveContact.ImageOptions.Image = global::CalcPro.Properties.Resources.Save_16x16;
            this.btnSaveContact.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnSaveContact.Location = new System.Drawing.Point(402, 188);
            this.btnSaveContact.Name = "btnSaveContact";
            this.btnSaveContact.Size = new System.Drawing.Size(103, 22);
            this.btnSaveContact.StyleController = this.layoutControl1;
            this.btnSaveContact.TabIndex = 7;
            this.btnSaveContact.Text = "Speichern";
            this.btnSaveContact.Click += new System.EventHandler(this.btnSaveContact_Click);
            // 
            // txtContactPerson
            // 
            this.txtContactPerson.EnterMoveNextControl = true;
            this.txtContactPerson.Location = new System.Drawing.Point(144, 45);
            this.txtContactPerson.Name = "txtContactPerson";
            this.txtContactPerson.Size = new System.Drawing.Size(361, 20);
            this.txtContactPerson.StyleController = this.layoutControl1;
            this.txtContactPerson.TabIndex = 1;
            this.txtContactPerson.Enter += new System.EventHandler(this.txtContactPerson_Enter);
            // 
            // checkEditDefaultContact
            // 
            this.checkEditDefaultContact.EnterMoveNextControl = true;
            this.checkEditDefaultContact.Location = new System.Drawing.Point(59, 165);
            this.checkEditDefaultContact.Name = "checkEditDefaultContact";
            this.checkEditDefaultContact.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkEditDefaultContact.Properties.Appearance.ForeColor = System.Drawing.Color.White;
            this.checkEditDefaultContact.Properties.Appearance.Options.UseFont = true;
            this.checkEditDefaultContact.Properties.Appearance.Options.UseForeColor = true;
            this.checkEditDefaultContact.Properties.Caption = "Standardkontakt";
            this.checkEditDefaultContact.Size = new System.Drawing.Size(446, 19);
            this.checkEditDefaultContact.StyleController = this.layoutControl1;
            this.checkEditDefaultContact.TabIndex = 6;
            // 
            // txtTelephone
            // 
            this.txtTelephone.EnterMoveNextControl = true;
            this.txtTelephone.Location = new System.Drawing.Point(144, 69);
            this.txtTelephone.Name = "txtTelephone";
            this.txtTelephone.Size = new System.Drawing.Size(361, 20);
            this.txtTelephone.StyleController = this.layoutControl1;
            this.txtTelephone.TabIndex = 2;
            this.txtTelephone.Enter += new System.EventHandler(this.txtContactPerson_Enter);
            // 
            // txtTaxNo
            // 
            this.txtTaxNo.EnterMoveNextControl = true;
            this.txtTaxNo.Location = new System.Drawing.Point(144, 141);
            this.txtTaxNo.Name = "txtTaxNo";
            this.txtTaxNo.Size = new System.Drawing.Size(361, 20);
            this.txtTaxNo.StyleController = this.layoutControl1;
            this.txtTaxNo.TabIndex = 5;
            this.txtTaxNo.Enter += new System.EventHandler(this.txtContactPerson_Enter);
            // 
            // txtFax
            // 
            this.txtFax.EnterMoveNextControl = true;
            this.txtFax.Location = new System.Drawing.Point(144, 93);
            this.txtFax.Name = "txtFax";
            this.txtFax.Size = new System.Drawing.Size(361, 20);
            this.txtFax.StyleController = this.layoutControl1;
            this.txtFax.TabIndex = 3;
            this.txtFax.Enter += new System.EventHandler(this.txtContactPerson_Enter);
            // 
            // txtemail
            // 
            this.txtemail.EnterMoveNextControl = true;
            this.txtemail.Location = new System.Drawing.Point(144, 117);
            this.txtemail.Name = "txtemail";
            this.txtemail.Size = new System.Drawing.Size(361, 20);
            this.txtemail.StyleController = this.layoutControl1;
            this.txtemail.TabIndex = 4;
            this.txtemail.Enter += new System.EventHandler(this.txtContactPerson_Enter);
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
            this.emptySpaceItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem7,
            this.layoutControlItem8,
            this.layoutControlItem9,
            this.emptySpaceItem2,
            this.emptySpaceItem3,
            this.emptySpaceItem4,
            this.emptySpaceItem5});
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Size = new System.Drawing.Size(561, 257);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(47, 202);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(450, 35);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.txtContactPerson;
            this.layoutControlItem2.Location = new System.Drawing.Point(47, 33);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(450, 24);
            this.layoutControlItem2.Text = "AnsprPartner";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(82, 14);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.txtTelephone;
            this.layoutControlItem3.Location = new System.Drawing.Point(47, 57);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(450, 24);
            this.layoutControlItem3.Text = "Telefon";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(82, 14);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.txtFax;
            this.layoutControlItem4.Location = new System.Drawing.Point(47, 81);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(450, 24);
            this.layoutControlItem4.Text = "Fax";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(82, 14);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.txtemail;
            this.layoutControlItem5.Location = new System.Drawing.Point(47, 105);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(450, 24);
            this.layoutControlItem5.Text = "Email";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(82, 14);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.txtTaxNo;
            this.layoutControlItem6.Location = new System.Drawing.Point(47, 129);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(450, 24);
            this.layoutControlItem6.Text = "UStIdent";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(82, 14);
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.checkEditDefaultContact;
            this.layoutControlItem7.Location = new System.Drawing.Point(47, 153);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(450, 23);
            this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem7.TextVisible = false;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.btnSaveContact;
            this.layoutControlItem8.Location = new System.Drawing.Point(390, 176);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(107, 26);
            this.layoutControlItem8.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem8.TextVisible = false;
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this.btnCancelContact;
            this.layoutControlItem9.Location = new System.Drawing.Point(288, 176);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Size = new System.Drawing.Size(102, 26);
            this.layoutControlItem9.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem9.TextVisible = false;
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(47, 176);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(241, 26);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem3
            // 
            this.emptySpaceItem3.AllowHotTrack = false;
            this.emptySpaceItem3.Location = new System.Drawing.Point(47, 0);
            this.emptySpaceItem3.Name = "emptySpaceItem3";
            this.emptySpaceItem3.Size = new System.Drawing.Size(450, 33);
            this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem4
            // 
            this.emptySpaceItem4.AllowHotTrack = false;
            this.emptySpaceItem4.Location = new System.Drawing.Point(497, 0);
            this.emptySpaceItem4.Name = "emptySpaceItem4";
            this.emptySpaceItem4.Size = new System.Drawing.Size(44, 237);
            this.emptySpaceItem4.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem5
            // 
            this.emptySpaceItem5.AllowHotTrack = false;
            this.emptySpaceItem5.Location = new System.Drawing.Point(0, 0);
            this.emptySpaceItem5.Name = "emptySpaceItem5";
            this.emptySpaceItem5.Size = new System.Drawing.Size(47, 237);
            this.emptySpaceItem5.TextSize = new System.Drawing.Size(0, 0);
            // 
            // frmOTTOMaster
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btnCancelContact;
            this.ClientSize = new System.Drawing.Size(561, 257);
            this.Controls.Add(this.layoutControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOTTOMaster";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "OTTO Firmendaten";
            this.Load += new System.EventHandler(this.frmOTTOMaster_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtContactPerson.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditDefaultContact.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTelephone.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTaxNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFax.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtemail.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem5)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.CheckEdit checkEditDefaultContact;
        private DevExpress.XtraEditors.TextEdit txtTaxNo;
        private DevExpress.XtraEditors.TextEdit txtemail;
        private DevExpress.XtraEditors.TextEdit txtFax;
        private DevExpress.XtraEditors.TextEdit txtTelephone;
        private DevExpress.XtraEditors.TextEdit txtContactPerson;
        private DevExpress.XtraEditors.SimpleButton btnSaveContact;
        private DevExpress.XtraEditors.SimpleButton btnCancelContact;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem9;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem4;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem5;

    }
}