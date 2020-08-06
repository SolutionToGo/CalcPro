namespace CalcPro
{
    partial class frmLoadProject
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
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLoadProject));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.gcProjectSearch = new DevExpress.XtraGrid.GridControl();
            this.dgProjectSearch = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ProjectID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ProjectNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ComissionNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CustomerName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PlannerName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ProjectDescription = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Created_By = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Created_Date = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.riDate = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn12 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnLoadProject = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.ribbonPage2 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcProjectSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgProjectSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riDate.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLoadProject)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.AllowCustomization = false;
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
            this.layoutControl1.Controls.Add(this.gcProjectSearch);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(759, 208, 250, 350);
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(1042, 576);
            this.layoutControl1.TabIndex = 1;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // gcProjectSearch
            // 
            this.gcProjectSearch.Location = new System.Drawing.Point(7, 7);
            this.gcProjectSearch.MainView = this.dgProjectSearch;
            this.gcProjectSearch.Name = "gcProjectSearch";
            this.gcProjectSearch.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.riDate,
            this.btnLoadProject});
            this.gcProjectSearch.Size = new System.Drawing.Size(1028, 562);
            this.gcProjectSearch.TabIndex = 3;
            this.gcProjectSearch.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgProjectSearch});
            // 
            // dgProjectSearch
            // 
            this.dgProjectSearch.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgProjectSearch.Appearance.FocusedCell.Options.UseBackColor = true;
            this.dgProjectSearch.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgProjectSearch.Appearance.FocusedRow.Options.UseBackColor = true;
            this.dgProjectSearch.Appearance.HeaderPanel.Font = new System.Drawing.Font("Bahnschrift", 10F);
            this.dgProjectSearch.Appearance.HeaderPanel.Options.UseFont = true;
            this.dgProjectSearch.Appearance.Row.Font = new System.Drawing.Font("Bahnschrift Light", 10F);
            this.dgProjectSearch.Appearance.Row.Options.UseFont = true;
            this.dgProjectSearch.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.ProjectID,
            this.ProjectNumber,
            this.ComissionNumber,
            this.CustomerName,
            this.PlannerName,
            this.ProjectDescription,
            this.Created_By,
            this.Created_Date,
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8,
            this.gridColumn9,
            this.gridColumn10,
            this.gridColumn11,
            this.gridColumn12,
            this.gridColumn13});
            this.dgProjectSearch.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.dgProjectSearch.GridControl = this.gcProjectSearch;
            this.dgProjectSearch.Name = "dgProjectSearch";
            this.dgProjectSearch.OptionsBehavior.Editable = false;
            this.dgProjectSearch.OptionsFind.AlwaysVisible = true;
            this.dgProjectSearch.OptionsFind.FindNullPrompt = "Suchtext eingeben...";
            this.dgProjectSearch.OptionsFind.ShowFindButton = false;
            this.dgProjectSearch.OptionsView.ShowGroupPanel = false;
            this.dgProjectSearch.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(this.dgProjectSearch_PopupMenuShowing);
            this.dgProjectSearch.DoubleClick += new System.EventHandler(this.dgProjectSearch_DoubleClick);
            // 
            // ProjectID
            // 
            this.ProjectID.Caption = "ProjectID";
            this.ProjectID.FieldName = "ProjectID";
            this.ProjectID.Name = "ProjectID";
            // 
            // ProjectNumber
            // 
            this.ProjectNumber.Caption = "Projekt Nummer";
            this.ProjectNumber.FieldName = "ProjectNumber";
            this.ProjectNumber.Name = "ProjectNumber";
            this.ProjectNumber.Visible = true;
            this.ProjectNumber.VisibleIndex = 0;
            this.ProjectNumber.Width = 102;
            // 
            // ComissionNumber
            // 
            this.ComissionNumber.Caption = "Kommission Nummer";
            this.ComissionNumber.FieldName = "ComissionNumber";
            this.ComissionNumber.Name = "ComissionNumber";
            this.ComissionNumber.Visible = true;
            this.ComissionNumber.VisibleIndex = 2;
            this.ComissionNumber.Width = 117;
            // 
            // CustomerName
            // 
            this.CustomerName.Caption = "Kunde Name";
            this.CustomerName.FieldName = "CustomerName";
            this.CustomerName.Name = "CustomerName";
            this.CustomerName.Visible = true;
            this.CustomerName.VisibleIndex = 3;
            this.CustomerName.Width = 208;
            // 
            // PlannerName
            // 
            this.PlannerName.Caption = "Planner";
            this.PlannerName.FieldName = "PlannerName";
            this.PlannerName.Name = "PlannerName";
            this.PlannerName.Visible = true;
            this.PlannerName.VisibleIndex = 4;
            this.PlannerName.Width = 93;
            // 
            // ProjectDescription
            // 
            this.ProjectDescription.Caption = "Project Description";
            this.ProjectDescription.FieldName = "ProjectDescription";
            this.ProjectDescription.Name = "ProjectDescription";
            this.ProjectDescription.Visible = true;
            this.ProjectDescription.VisibleIndex = 1;
            this.ProjectDescription.Width = 168;
            // 
            // Created_By
            // 
            this.Created_By.Caption = "Created_By";
            this.Created_By.FieldName = "Created_By";
            this.Created_By.Name = "Created_By";
            // 
            // Created_Date
            // 
            this.Created_Date.Caption = "Created Date";
            this.Created_Date.FieldName = "Created_Date";
            this.Created_Date.Name = "Created_Date";
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "LV Raster";
            this.gridColumn1.FieldName = "LV_Raster";
            this.gridColumn1.Name = "gridColumn1";
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "LV Sprung";
            this.gridColumn2.FieldName = "LV_Sprung";
            this.gridColumn2.Name = "gridColumn2";
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Intern S";
            this.gridColumn3.FieldName = "Intern_S";
            this.gridColumn3.Name = "gridColumn3";
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "Intern X";
            this.gridColumn4.FieldName = "Intern_X";
            this.gridColumn4.Name = "gridColumn4";
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "Vat";
            this.gridColumn5.FieldName = "Vat";
            this.gridColumn5.Name = "gridColumn5";
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "Submit Location";
            this.gridColumn6.FieldName = "Submit_Location";
            this.gridColumn6.Name = "gridColumn6";
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "Submit Date";
            this.gridColumn7.FieldName = "Submit_Date";
            this.gridColumn7.Name = "gridColumn7";
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "Submit Time";
            this.gridColumn8.FieldName = "Submit_Time";
            this.gridColumn8.Name = "gridColumn8";
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "Beginn";
            this.gridColumn9.ColumnEdit = this.riDate;
            this.gridColumn9.FieldName = "ProjectStartDate";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 5;
            this.gridColumn9.Width = 64;
            // 
            // riDate
            // 
            this.riDate.Appearance.Font = new System.Drawing.Font("Bahnschrift Light", 10F);
            this.riDate.Appearance.Options.UseFont = true;
            this.riDate.AutoHeight = false;
            this.riDate.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.riDate.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.riDate.DisplayFormat.FormatString = "y";
            this.riDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.riDate.Mask.EditMask = "y";
            this.riDate.Name = "riDate";
            this.riDate.VistaCalendarViewStyle = DevExpress.XtraEditors.VistaCalendarViewStyle.YearView;
            // 
            // gridColumn10
            // 
            this.gridColumn10.Caption = "Abschluss";
            this.gridColumn10.ColumnEdit = this.riDate;
            this.gridColumn10.FieldName = "ProjectEndDate";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 6;
            this.gridColumn10.Width = 67;
            // 
            // gridColumn11
            // 
            this.gridColumn11.Caption = "Ausführungsstand";
            this.gridColumn11.FieldName = "ShowVK";
            this.gridColumn11.Name = "gridColumn11";
            // 
            // gridColumn12
            // 
            this.gridColumn12.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn12.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.gridColumn12.Caption = "VK";
            this.gridColumn12.FieldName = "VK";
            this.gridColumn12.Name = "gridColumn12";
            this.gridColumn12.Visible = true;
            this.gridColumn12.VisibleIndex = 7;
            this.gridColumn12.Width = 104;
            // 
            // gridColumn13
            // 
            this.gridColumn13.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn13.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn13.Caption = "Load";
            this.gridColumn13.ColumnEdit = this.btnLoadProject;
            this.gridColumn13.Name = "gridColumn13";
            this.gridColumn13.Visible = true;
            this.gridColumn13.VisibleIndex = 8;
            this.gridColumn13.Width = 77;
            // 
            // btnLoadProject
            // 
            this.btnLoadProject.Appearance.Font = new System.Drawing.Font("Bahnschrift Light", 10F);
            this.btnLoadProject.Appearance.Options.UseFont = true;
            this.btnLoadProject.AutoHeight = false;
            editorButtonImageOptions1.Image = ((System.Drawing.Image)(resources.GetObject("editorButtonImageOptions1.Image")));
            this.btnLoadProject.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.btnLoadProject.Name = "btnLoadProject";
            this.btnLoadProject.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.btnLoadProject.Click += new System.EventHandler(this.btnLoadProject_Click);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.AppearanceGroup.Font = new System.Drawing.Font("Bahnschrift Light", 10F);
            this.layoutControlGroup1.AppearanceGroup.Options.UseFont = true;
            this.layoutControlGroup1.AppearanceItemCaption.Font = new System.Drawing.Font("Bahnschrift Light", 10F);
            this.layoutControlGroup1.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2});
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlGroup1.Size = new System.Drawing.Size(1042, 576);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.gcProjectSearch;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(1032, 566);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // ribbonPage2
            // 
            this.ribbonPage2.Name = "ribbonPage2";
            this.ribbonPage2.Text = "ribbonPage2";
            // 
            // frmLoadProject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1042, 576);
            this.Controls.Add(this.layoutControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmLoadProject";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Projekt laden";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmLoadProject_FormClosing);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmLoadProject_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcProjectSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgProjectSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riDate.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLoadProject)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraGrid.GridControl gcProjectSearch;
        private DevExpress.XtraGrid.Views.Grid.GridView dgProjectSearch;
        private DevExpress.XtraGrid.Columns.GridColumn ProjectID;
        private DevExpress.XtraGrid.Columns.GridColumn ProjectNumber;
        private DevExpress.XtraGrid.Columns.GridColumn ComissionNumber;
        private DevExpress.XtraGrid.Columns.GridColumn CustomerName;
        private DevExpress.XtraGrid.Columns.GridColumn PlannerName;
        private DevExpress.XtraGrid.Columns.GridColumn ProjectDescription;
        private DevExpress.XtraGrid.Columns.GridColumn Created_By;
        private DevExpress.XtraGrid.Columns.GridColumn Created_Date;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn12;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit riDate;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage2;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit btnLoadProject;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn13;
    }
}