using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using DevExpress.XtraTab.ViewInfo;
using BL;
using DevExpress.Utils;
using EL;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes.Operations;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraTreeList.Columns;
using System.Drawing.Drawing2D;
using System.Collections;
using DevExpress.XtraLayout;
using System.Diagnostics;
using System.IO;
using System.Configuration;
using DevExpress.XtraTreeList.Menu;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using DevExpress.XtraReports.UI;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using CalcPro.Report_Design;
using DevExpress.XtraBars;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid.Views.Layout.Modes;
using DevExpress.XtraGrid.Views.Base;
using System.Globalization;
using DevExpress.XtraPrinting;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraEditors.Repository;
using System.Xml;
using System.Threading;
using DataAccess;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using System.Text.RegularExpressions;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraTreeList.StyleFormatConditions;
using log4net;
using DevExpress.XtraEditors.Popup;

namespace CalcPro
{
    public partial class frmProject : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        #region Local Variables

        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private List<Control> RequiredPositionFields = new List<Control>();
        private List<Control> RequiredPositionFieldsforTitle = new List<Control>();
        private List<Control> RequiredFieldsFormBlatt = new List<Control>();
        private int _ProjectID = -1;
        private bool _IsCopy = false;
        private string LongDescription = string.Empty;
        private bool _IsEditMode = false;
        public bool _IsNewMode = false;
        public int iSNO = -1;
        public bool _IsAddhoc = false;
        public int iRasterCount = 0;
        bool _ISChange = true;
        bool _IsTabPressed = false;
        private string _DocuwareLink1;
        private string _DocuwareLink2;
        private string _DocuwareLink3;
        private EDeliveryNotes ObjEDeliveryNotes = null;
        private BDeliveryNotes ObjBDeliveryNotes = null;
        GridHitInfo downHitInfo = null;
        private int SNO = 1;
        private bool _IsValueChanged = true;
        private bool _IsSave = false;
        EProject ObjEProject = new EProject();
        EPosition ObjEPosition = new EPosition();
        BProject ObjBProject = new BProject();
        BPosition ObjBPosition = new BPosition();
        BGAEB objBGAEB = new BGAEB();
        EGAEB objEGAEB = new EGAEB();
        EMulti ObjEMulti = null;
        BMulti ObjBMulti = null;
        EUmlage ObjEUmlage = null;
        BUmlage ObjBUmlage = null;
        EInvoice ObjEInvoice = null;
        BInvoice oBJBInvoice = null;
        BFormBlatt ObjBFormBlatt = null;
        EFormBlatt ObjEFormBlatt = null;

        ESupplier ObjESupplier = new ESupplier();
        BSupplier ObjBSupplier = new BSupplier();
        private int _ProposalID = 0;
        string _WGforSupplier = string.Empty;
        string _WAforSupplier = string.Empty;
        string _LVSection = string.Empty;
        string _pdfpath = null;
        DataTable _dtSuppliermail = new DataTable();
        DataTable _dtCopyLVAndDetailKZ;
        bool _ISCopiedLV = false;
        DataTable _dtCopyDetailKZ;
        bool _ISCopied = false;
        bool _IsKeyypressEvent = false;
        bool _IsTypKeyypressEvent = false;
        bool IsRefresh = false;
        bool _WGWAChanged = false;
        bool WGChanged = false;
        bool WAChanged = false;
        bool _FiredFromEvent = false;
        XtraTabPage ObjTabDetails = null;
        string _txtDimensions = string.Empty;
        string tType = null;
        private bool _Usertrigger = true;
        double sum = 0;
        DataSet ds_oldPrj;
        bool IsFire = false;
        #endregion

        #region Global Variables

        public TreeList tl
        {
            get { return tlPositions; }
        }

        public int ProjectID
        { get { return _ProjectID; } set { _ProjectID = value; } }

        public bool IsCopy
        {
            get { return _IsCopy; }
            set { _IsCopy = value; }
        }

        public frmProject()
        {
            InitializeComponent();
            chkCreateNew.EditValue = false;
            ChkManualMontageentry.EditValue = false;
            this.tlPositions.GetNodeDisplayValue += new DevExpress.XtraTreeList.GetNodeDisplayValueEventHandler(this.treeList1_GetNodeDisplayValue);
            this.rpLVDropMode.SelectedIndexChanged += rpLVDropMode_SelectedIndexChanged;
        }

        #endregion

        #region Project Form Events

        #region Events
        private void txProjectDetails_CloseButtonClick(object sender, EventArgs e)
        {
            try
            {
                ClosePageButtonEventArgs arg = e as ClosePageButtonEventArgs;
                (arg.Page as XtraTabPage).PageVisible = false;
            }
            catch (Exception ex) { }
        }

        private void frmProject_Load(object sender, EventArgs e)
        {
            try
            {
                LoadExistingRasters();
                ECustomer ObjECustomer = new ECustomer();
                BCustomer ObjBCustomer = new BCustomer();
                ObjBCustomer.GetCustomers(ObjECustomer);
                if (ObjECustomer.dsCustomer != null)
                {
                    cmbKundeNo.Properties.DataSource = ObjECustomer.dsCustomer.Tables[0];
                    cmbKundeNo.Properties.DisplayMember = "CustomerFullName";
                    cmbKundeNo.Properties.ValueMember = "CustomerID";
                }
                splitContainerControl1.CollapsePanel = SplitCollapsePanel.Panel1;
                splitContainerControl2.CollapsePanel = SplitCollapsePanel.Panel1;
                Utility.SetLookupEditValue(cmbPositionKZ, "N-Normalposition");
                chkCumulated.Checked = true;
                SetRoundingPriceforColumn();

                if (ProjectID > 0)
                    ChkRaster.Enabled = false;
                if (txtkommissionNumber.Text != string.Empty)
                {
                    DisalbeProjectControls();
                }

                if (Utility.LVDetailsAccess == "9" || Utility.CalcAccess == "9")
                {
                    tbLVDetails.Visible = false;
                    if (Utility.LVDetailsAccess == "9")
                    {
                        btnProjectImport.Visibility = BarItemVisibility.Never;
                        btnProjectExport.Visibility = BarItemVisibility.Never;
                        tbCopyLVs.Visible = false;
                    }
                    if (Utility.CalcAccess == "9")
                    {
                        tbBulkProcess.Visible = false;
                        tbMulti5.Visible = false;
                        tbMulti6.Visible = false;
                        tbSupplierProposal.Visible = false;
                        tbUpdateSupplier.Visible = false;
                        tbOmlage.Visible = false;
                    }
                    if (Utility.LVsectionAddAccess == "8" || Utility.LVSectionEditAccess == "8")
                    {
                        tbLVDetails.PageVisible = true;
                    }
                }
                if (Utility.ProjectDataAccess == "9")
                    tbProjectDetails.PageVisible = false;
                if (Utility.DeliveryAccess == "9")
                    tbDeliveryNotes.PageVisible = false;
                if (Utility.InvoiceAccess == "9")
                    tbInvoices.PageVisible = false;

                if (Utility.FormBlattArticleMappingAccess == "9" || Utility.FormBlattArticleMappingAccess == "7")
                    btnArtSettings.Visibility = BarItemVisibility.Never;

                if (Utility.KomissionDataAccess == "9" || Utility.KomissionDataAccess == "7")
                    txtkommissionNumber.Enabled = false;

                if (Utility.RoleID != 14)
                {
                    chkLockHierarchy.Enabled = false;
                    chkLockHierarchy.Enabled = false;
                }
                chkLockHierarchy_CheckedChanged(null, null);
                panelControldoc.Visible = false;
                toggleSwitchType.Visible = false;
                dockPanelArticles.Hide();

                cmbLVStatus.Properties.DataSource = Utility.dtLVStatus;
                cmbLVStatus.Properties.DisplayMember = "LVStatus";
                cmbLVStatus.Properties.ValueMember = "LVStatusID";

                cmbPositionKZ.Properties.DataSource = Utility.dtPositionKZ;
                cmbPositionKZ.Properties.DisplayMember = "KZDescription";
                cmbPositionKZ.Properties.ValueMember = "PositionKZListID";

                cmbPositionKZCD.Properties.DataSource = Utility.dtPositionKZ;
                cmbPositionKZCD.Properties.DisplayMember = "KZDescription";
                cmbPositionKZCD.Properties.ValueMember = "PositionKZListID";
                tcProjectDetails_SelectedPageChanged(null, null);

                DataTable dt = new DataTable();
                dt.Columns.Add("Von", typeof(string));
                dt.Columns.Add("Bis", typeof(string));
                dt.Columns[0].ReadOnly = false;
                dt.Columns[1].ReadOnly = false;
                gvAddRemovePositions.DataSource = dt;
                        }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }

        }

        private void frmProject_FormClosing(object sender, FormClosingEventArgs e)
        {
            BaseTabHeaderViewInfo headerInfo = (tcProjectDetails as IXtraTab).ViewInfo.HeaderInfo;
            try
            {
                if (!Utility.Isclose)
                {
                    if (headerInfo.VisiblePages.Count >= 1)
                    {
                        var dlgresult = "";
                        if (Utility._IsGermany == true)
                        {
                            dlgresult = XtraMessageBox.Show("Möchten Sie diese Seite wirklich schließen.?", " Bestätigung …!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning).ToString();
                        }
                        else
                        {
                            dlgresult = XtraMessageBox.Show("Are you sure you want to close the page.?", "confirmation..!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning).ToString();

                        }
                        if (dlgresult.ToString().ToLower() == "cancel")
                        {
                            e.Cancel = true;
                        }
                        btnCancel_Click(null, null);
                    }
                    if (frmCalcPro.Instance.MdiChildren.Count() == 1)
                    {
                        frmCalcPro.Instance.SetPictureBoxVisible(true);
                    }
                }
                else
                {
                    Utility.Isclose = false;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void tcProjectDetails_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            try
            {
                if (tcProjectDetails.SelectedTabPage == null)
                {
                    this.Close();
                    return;
                }

                switch (tcProjectDetails.SelectedTabPage.Name.ToString())
                {
                    case "tbProjectDetails":
                        rpProjectMetaData.Visible = true;
                        this.ribbonControl1.SelectPage(rpProjectMetaData);
                        break;
                    case "tbLVDetails":
                        rpPosition.Visible = true;
                        this.ribbonControl1.SelectPage(rpPosition);
                        break;
                    case "tbMulti5":
                        rpCalculations.Visible = true;
                        rpgSelb.Visible = true;
                        rpgVerka.Visible = false;
                        rpgUmlage.Visible = false;
                        this.ribbonControl1.SelectPage(rpCalculations);
                        break;
                    case "tbMulti6":
                        rpCalculations.Visible = true;
                        rpgSelb.Visible = false;
                        rpgVerka.Visible = true;
                        rpgUmlage.Visible = false;
                        this.ribbonControl1.SelectPage(rpCalculations);
                        break;
                    case "tbOmlage":
                        rpCalculations.Visible = true;
                        rpgSelb.Visible = false;
                        rpgVerka.Visible = false;
                        rpgUmlage.Visible = true;
                        this.ribbonControl1.SelectPage(rpCalculations);
                        break;
                    case "tbBulkProcess":
                        rpBulkProcess.Visible = true;
                        rpgBuilkProccess.Visible = true;
                        rpgLVCopy.Visible = false;
                        this.ribbonControl1.SelectPage(rpBulkProcess);
                        break;
                    case "tbInvoices":
                        this.ribbonControl1.SelectPage(rpDelInv);
                        break;
                    case "tbSupplierProposal":
                        rpProposal.Visible = true;
                        rpgSP.Visible = true;
                        rpgUSP.Visible = false;
                        rpgFilter.Visible = false;
                        rpgProposalViews.Visible = false;
                        this.ribbonControl1.SelectPage(rpProposal);
                        break;
                    case "tbUpdateSupplier":
                        rpProposal.Visible = true;
                        rpgSP.Visible = false;
                        rpgUSP.Visible = true;
                        rpgFilter.Visible = true;
                        rpgProposalViews.Visible = true;
                        this.ribbonControl1.SelectPage(rpProposal);
                        break;
                    case "tbAufmassReport":
                        this.ribbonControl1.SelectPage(rpDelInv);
                        break;
                    case "tbCopyLVs":
                        rpBulkProcess.Visible = true;
                        rpgBuilkProccess.Visible = false;
                        rpgLVCopy.Visible = true;
                        this.ribbonControl1.SelectPage(rpBulkProcess);
                        break;
                }

                if (tcProjectDetails.SelectedTabPage == tbProjectDetails)
                {
                    if (ObjEProject.IsFinalInvoice && Utility.ProjectDataAccess == "7")
                        btnSaveProject.Enabled = false;
                    if (tlPositions.Nodes.Count == 0)
                        ddlRaster.Enabled = true;
                    LoadExistingProject();
                    FormatLVFields();
                }
                else if (tcProjectDetails.SelectedTabPage == tbLVDetails)
                {
                    if (ObjEProject.ProjectID > 0)
                    {
                        FormatLVFields();
                        IntializeLVPositions();
                        SetOhnestuffeMask();
                        BindPositionData();
                        SetMaskForMaulties();
                        SetRoundingPriceforColumn();
                        if (tlPositions.Nodes != null && tlPositions.Nodes.Count > 0)
                            tlPositions.SetFocusedNode(tlPositions.MoveLastVisible());
                        else
                        {
                            btnNext.Enabled = false;
                            btnPrevious.Enabled = false;
                        }
                    }

                    if (string.IsNullOrEmpty(ObjEProject.CommissionNumber))
                    {
                        cmbLVSection.Enabled = false;
                        btnAddLVSection.Enabled = false;
                    }
                    else
                    {
                        cmbLVSection.Enabled = true;
                        btnAddLVSection.Enabled = true;
                    }

                    //LV Details Access
                    if (Utility.LVDetailsAccess == "9")
                    {
                        splitContainerControl2.PanelVisibility = SplitPanelVisibility.Panel2;
                        btnNew.Enabled = false;
                        chkCreateNew.Enabled = false;
                        LVDetailsColumnReadOnly(true);
                    }
                    else if (Utility.LVDetailsAccess == "7")
                    {
                        splitContainerControl2.Panel1.Enabled = false;
                        btnNew.Enabled = false;
                        chkCreateNew.Enabled = false;
                        LVDetailsColumnReadOnly(true);
                    }
                    else if (Utility.LVDetailsAccess == "8")
                    {
                        splitContainerControl2.Panel1.Enabled = true;
                        btnNew.Enabled = true;
                        chkCreateNew.Enabled = true;
                        LVDetailsColumnReadOnly(false);
                    }

                    //Cost Details Access
                    if (Utility.CalcAccess == "9")
                    {
                        splitContainerControl2.PanelVisibility = SplitPanelVisibility.Panel1;
                        LVCalculationColumnReadOnly(true);
                    }
                    else if (Utility.CalcAccess == "7")
                    {
                        splitContainerControl2.Panel2.Enabled = false;
                        LVCalculationColumnReadOnly(true);
                    }
                    else if (Utility.CalcAccess == "8")
                    {
                        splitContainerControl2.Panel2.Enabled = true;
                        LVCalculationColumnReadOnly(false);
                    }

                    if (Utility.LVsectionAddAccess == "9" || Utility.LVsectionAddAccess == "7")
                    {
                        btnAddLVSection.Enabled = false;
                        cmbLVSection.Enabled = false;
                    }
                    else
                    {
                        if (Utility.LVDetailsAccess != "8" && !string.IsNullOrEmpty(ObjEProject.CommissionNumber))
                        {
                            splitContainerControl2.PanelVisibility = SplitPanelVisibility.Panel1;
                            splitContainerControl2.Panel1.Enabled = true;
                            DataView dvLVSection = ObjEProject.dtLVSection.DefaultView;
                            dvLVSection.RowFilter = "LVSectionName <> 'HA'";
                            cmbLVSection.Properties.DataSource = ObjEProject.dtLVSection;
                            cmbLVSection.Properties.DisplayMember = "LVSectionName";
                            cmbLVSection.Properties.ValueMember = "LVSectionID";
                            btnNew.Enabled = true;
                            chkCreateNew.Enabled = true;
                            LVDetailsColumnReadOnly(true);
                        }
                    }

                    if (Utility.LVDetailsAccess == "9" && Utility.CalcAccess == "9" && (Utility.LVsectionAddAccess != "8" || string.IsNullOrEmpty(ObjEProject.CommissionNumber)))
                        splitContainerControl1.PanelVisibility = SplitPanelVisibility.Panel2;
                    else
                    {
                        //if(splitContainerControl1.PanelVisibility == SplitPanelVisibility.Panel2)
                    }
                    if (ObjEProject.IsFinalInvoice)
                    {
                        btnNew.Enabled = false;
                        btnSaveLVDetails.Enabled = false;
                        btnCancel.Enabled = false;
                        chkCreateNew.Enabled = false;
                        tlPositions.OptionsBehavior.Editable = false;
                    }

                    if (splitContainerControl2.PanelVisibility == SplitPanelVisibility.Panel1)
                        splitContainerControl1.SplitterPosition = 350;
                    else if (splitContainerControl2.PanelVisibility == SplitPanelVisibility.Panel2)
                        splitContainerControl1.SplitterPosition = 560;
                }
                else if (tcProjectDetails.SelectedTabPage == tbBulkProcess)
                {
                    ObjBProject.GetProjectDetails(ObjEProject);
                    if (ObjEProject.ActualLvs == 0)
                        return;
                    if (ObjEProject.ProjectID > 0)
                    {
                        if (txtkommissionNumber.Text != "")
                        {
                            checkEditSectionAMulti5MA.Enabled = false;
                            checkEditSectionAMulti5MO.Enabled = false;
                            checkEditSectionAMulti6MA.Enabled = false;
                            checkEditSectionAMulti6MO.Enabled = false;
                            btnSaveActionA.Enabled = false;

                            checkEditPositionMenge.Enabled = false;
                            checkEditMaterialKz.Enabled = false;
                            checkEditMontageKZ.Enabled = false;
                            checkEditPreisErstaztext.Enabled = false;
                            checkEditArtikelnummerWG.Enabled = false;
                            checkEditFabrikat.Enabled = false;
                            checkEditTyp.Enabled = false;
                            checkEditLieferantMA.Enabled = false;
                            btnSavesectionB.Enabled = false;
                            checkEditNachtragsnummer.Enabled = true;
                        }
                        else
                        {
                            checkEditSectionAMulti5MA.Enabled = true;
                            checkEditSectionAMulti5MO.Enabled = true;
                            checkEditSectionAMulti6MA.Enabled = true;
                            checkEditSectionAMulti6MO.Enabled = true;
                            btnSaveActionA.Enabled = true;

                            checkEditPositionMenge.Enabled = true;
                            checkEditMaterialKz.Enabled = true;
                            checkEditMontageKZ.Enabled = true;
                            checkEditPreisErstaztext.Enabled = true;
                            checkEditArtikelnummerWG.Enabled = true;
                            checkEditFabrikat.Enabled = true;
                            checkEditTyp.Enabled = true;
                            checkEditLieferantMA.Enabled = true;
                            btnSavesectionB.Enabled = true;
                            checkEditNachtragsnummer.Enabled = false;

                        }

                    }
                    if (Utility.CalcAccess == "7" || ObjEProject.IsFinalInvoice)
                        btnApply.Enabled = false;
                    if (!string.IsNullOrEmpty(ObjEProject.CommissionNumber))
                    {
                        if (Utility.LVsectionAddAccess == "9" || Utility.LVsectionAddAccess == "7"
                             || Utility.LVSectionEditAccess == "9" || Utility.LVSectionEditAccess == "7")
                            btnApply.Enabled = false;
                    }
                }
                else if (tcProjectDetails.SelectedTabPage == tbMulti5)
                {
                    if (ObjEProject.ProjectID > 0)
                    {
                        if (objBGAEB == null)
                            objBGAEB = new BGAEB();
                        DataTable dtLVSection = new DataTable();
                        dtLVSection = objBGAEB.GetLVSection(ObjEProject.ProjectID);

                        if (Utility.LVSectionEditAccess == "7")
                        {
                            DataTable dttemp = dtLVSection.Copy();
                            DataView dv = dttemp.DefaultView;
                            dv.RowFilter = "LVSectionName = 'HA'";
                            dtLVSection = new DataTable();
                            dtLVSection = dv.ToTable();
                        }

                        cmbLVSectionFilter.Properties.DataSource = dtLVSection;
                        cmbLVSectionFilter.Properties.DisplayMember = "LVSectionName";
                        cmbLVSectionFilter.Properties.ValueMember = "LVSectionID";

                        cmbLVSectionMulti5.Properties.DataSource = dtLVSection;
                        cmbLVSectionMulti5.Properties.DisplayMember = "LVSectionName";
                        cmbLVSectionMulti5.Properties.ValueMember = "LVSectionID";

                        Utility.SetCheckedComboexitValue(cmbLVSectionFilter, "HA");
                        btnMulti5LoadArticles_Click(null, null);
                        gvMulti5.BestFitColumns();
                        if (Utility.CalcAccess == "7" || ObjEProject.IsFinalInvoice)
                            btnMulti5UpdateSelbekosten.Enabled = false;
                    }
                }
                else if (tcProjectDetails.SelectedTabPage == tbMulti6)
                {
                    if (ObjEProject.ProjectID > 0)
                    {
                        if (objBGAEB == null)
                            objBGAEB = new BGAEB();
                        DataTable dtLVSection = new DataTable();
                        gcMulti6.DataSource = null;
                        dtLVSection = objBGAEB.GetLVSection(ObjEProject.ProjectID);

                        if (Utility.LVSectionEditAccess == "7")
                        {
                            DataTable dttemp = dtLVSection.Copy();
                            DataView dv = dttemp.DefaultView;
                            dv.RowFilter = "LVSectionName = 'HA'";
                            dtLVSection = new DataTable();
                            dtLVSection = dv.ToTable();
                        }

                        cmbMulti6LVFilter.Properties.DataSource = dtLVSection;
                        cmbMulti6LVFilter.Properties.DisplayMember = "LVSectionName";
                        cmbMulti6LVFilter.Properties.ValueMember = "LVSectionID";

                        cmbLvSectionMulti6.Properties.DataSource = dtLVSection;
                        cmbLvSectionMulti6.Properties.DisplayMember = "LVSectionName";
                        cmbLvSectionMulti6.Properties.ValueMember = "LVSectionID";

                        Utility.SetCheckedComboexitValue(cmbMulti6LVFilter, "HA");
                        cmbType.SelectedIndex = 0;

                        btnMulti6LoadArticles_Click(null, null);
                        gvMulti6.BestFitColumns();
                        if (Utility.CalcAccess == "7" || ObjEProject.IsFinalInvoice)
                            btnMulti6UpdateSelbekosten.Enabled = false;
                    }
                }
                else if (tcProjectDetails.SelectedTabPage == tbOmlage)
                {
                    ObjBProject.GetProjectDetails(ObjEProject);
                    if (ObjEProject.ProjectID > 0 && ObjEProject.CommissionNumber == string.Empty && ObjEProject.ActualLvs > 0)
                    {
                        if (ObjEUmlage == null)
                            ObjEUmlage = new EUmlage();
                        if (ObjBUmlage == null)
                            ObjBUmlage = new BUmlage();
                        ObjEUmlage.ProjectID = ObjEProject.ProjectID;
                        ObjEUmlage = ObjBUmlage.GetSpecialCost(ObjEUmlage);
                        rgUmlageMode.SelectedIndex = ObjEUmlage.UmlageMode;
                        gcOmlage.DataSource = ObjEUmlage.dtSpecialCost;
                        rgUmlageMode_SelectedIndexChanged(null, null);
                        if (Utility.CalcAccess == "7" || ObjEProject.IsFinalInvoice)
                        {
                            btnUmlageSave.Enabled = false;
                            btnAddedCost.Enabled = false;
                        }
                    }
                }
                else if (tcProjectDetails.SelectedTabPage == tbSupplierProposal)
                {
                    if (ObjEProject.ProjectID > 0)
                    {
                        ObjESupplier.ProjectID = ObjEProject.ProjectID;
                        FillLVSection();
                        gcDeletedDetails.DataSource = null;
                        gcProposedDetails.DataSource = null;
                        if (Utility.CalcAccess == "7" || ObjEProject.IsFinalInvoice)
                            btnSaveSupplierProposal.Enabled = false;
                        cmbLVSectionProposal_Closed(null, null);
                    }
                }
                else if (tcProjectDetails.SelectedTabPage == tbUpdateSupplier)
                {
                    if (ObjEProject.ProjectID > 0)
                    {
                        rgProposalViews.EditValue = 0;
                        FillProposalNumbers();
                        gcDeletedDetails.DataSource = null;
                        gcProposedDetails.DataSource = null;
                        if (Utility.CalcAccess == "7" || ObjEProject.IsFinalInvoice)
                        {
                            //layoutControl16.Enabled = false;
                            btnSubmit.Enabled = false;
                            gvSupplier.OptionsBehavior.Editable = false;
                        }
                        gvProposal_RowClick(null, null);
                    }
                }
                else if (tcProjectDetails.SelectedTabPage == tbDeliveryNotes)
                {
                    if (ObjEDeliveryNotes == null)
                        ObjEDeliveryNotes = new EDeliveryNotes();
                    if (ObjBDeliveryNotes == null)
                        ObjBDeliveryNotes = new BDeliveryNotes();
                    chkActiveDelivery.Checked = true;
                    ObjEDeliveryNotes.ProjectID = ObjEProject.ProjectID;
                    ObjEDeliveryNotes = ObjBDeliveryNotes.GetPositions(ObjEDeliveryNotes);
                    gcPositions.DataSource = ObjEDeliveryNotes.dtPositions;
                    NewBlattnumber();
                    LoadNonActiveDelivery();
                    ObjEDeliveryNotes = ObjBDeliveryNotes.GetBlattNumbers(ObjEDeliveryNotes);
                    gcDeliveryNumbers.DataSource = ObjEDeliveryNotes.dtBlattNumbers;
                    if (Utility.DeliveryAccess == "7" || ObjEProject.IsFinalInvoice)
                        btnSave.Enabled = false;
                }
                else if (tcProjectDetails.SelectedTabPage == tbInvoices)
                {
                    if (ObjEInvoice == null)
                        ObjEInvoice = new EInvoice();
                    if (oBJBInvoice == null)
                        oBJBInvoice = new BInvoice();
                    ObjEInvoice.ProjectID = ObjEProject.ProjectID;
                    ObjEInvoice = oBJBInvoice.GeTBlattNumbers(ObjEInvoice);
                    gcDeliveryNotes.DataSource = ObjEInvoice.dtBlattNumbers;
                    ObjEInvoice = oBJBInvoice.GetInvoices(ObjEInvoice);
                    gcInvoices.DataSource = ObjEInvoice.dtInvoices;
                    if (Utility.InvoiceAccess == "7" || ObjEProject.IsFinalInvoice)
                        btnGenerate.Enabled = false;
                    if (ObjEProject.IsFinalInvoice)
                    {
                        chkFinalInvoice.Checked = true;
                        if (Utility.RoleID != 14)
                        {
                            chkFinalInvoice.Enabled = false;
                            btnFinalBill.Enabled = false;
                        }
                    }
                }
                else if (tcProjectDetails.SelectedTabPage == tbCopyLVs)
                {
                    if (ObjEProject.ProjectID > 0)
                    {
                        BindPositionData();
                        if (tlPositions.AllNodesCount >= 2)
                        {
                            FillProjectNumber();
                            rgDropMode1.EditValue = 2;
                        }
                        else
                        {
                            if (!Utility._IsGermany)
                                throw new Exception("Bitte legen Sie mindestens einen Titel und einen Untertitel an");
                            else
                                throw new Exception("Atleast one title and subtitle should be there in the project.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void frmProject_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    btnCancel_Click(null, null);
                }
                else if (e.KeyCode == Keys.F6)
                {
                    if (tcProjectDetails.SelectedTabPage.Name == "tbLVDetails")
                    {
                        if (splitContainerControl2.PanelVisibility == SplitPanelVisibility.Both)
                        {

                            if (splitContainerControl2.SplitterPosition > 0)
                            {
                                splitContainerControl2.SplitterPosition = 0;
                                splitContainerControl1.SplitterPosition = 420;
                            }
                            else if (splitContainerControl2.SplitterPosition == 0 && splitContainerControl1.PanelVisibility == SplitPanelVisibility.Both)
                            {
                                splitContainerControl1.PanelVisibility = SplitPanelVisibility.Panel1;
                            }
                            else if (splitContainerControl2.SplitterPosition == 0 && splitContainerControl1.PanelVisibility == SplitPanelVisibility.Panel1)
                            {
                                splitContainerControl1.PanelVisibility = SplitPanelVisibility.Both;
                                splitContainerControl2.SplitterPosition = 300;
                                splitContainerControl1.SplitterPosition = 300;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = @"D:\";
                saveFileDialog1.Title = "Save Positons Table";
                saveFileDialog1.CheckPathExists = true;
                saveFileDialog1.DefaultExt = "xlsx";
                saveFileDialog1.Filter = "Excel files (*.xlsx)|*.xlsx";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog1.FileName;
                    XlsxExportOptionsEx opt = new XlsxExportOptionsEx();
                    opt.CustomizeCell += op_CustomizeCell;
                    tlPositions.ExportToXlsx(filePath, opt);
                    Process.Start(filePath);
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }
        #endregion

        #region Functions

        /// <summary>
        /// Code to Show and activate tab
        /// </summary>
        /// <param name="ObjTabDetails"></param>
        private void TabChange(XtraTabPage ObjTabDetails)
        {
            try
            {
                FormatLVFields();
                if (ObjTabDetails != null)
                {
                    if (ObjTabDetails.PageVisible == true)
                        tcProjectDetails.SelectedTabPage = ObjTabDetails;
                    else
                    {
                        ObjTabDetails.PageVisible = true;
                        tcProjectDetails.SelectedTabPage = ObjTabDetails;
                    }
                }
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// Overrided method to handle the keys on form
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                if (keyData == (Keys.F9))
                {
                    if (tcProjectDetails.SelectedTabPage.Name == "tbLVDetails")
                    {
                        string stPosKZ = Utility.GetPosKZ(cmbPositionKZ.Text);
                        if (stPosKZ == "ZZ")
                            return false;
                        if (ObjEProject.IsFinalInvoice)
                            return false;
                        btnSaveLVDetails_Click(null, null);
                        return true;
                    }
                    if (tcProjectDetails.SelectedTabPage.Name == "tbProjectDetails" && Utility.ProjectDataAccess != "7")
                    {
                        if (ObjEProject.IsFinalInvoice)
                            return false;
                        btnSaveProject_ItemClick(null, null);
                        return true;
                    }
                }
                else if (tcProjectDetails.SelectedTabPage == tbLVDetails && keyData == (Keys.PageDown))
                {
                    _IsTabPressed = true;
                    btnNext_Click(null, null);
                    return true;
                }
                else if (tcProjectDetails.SelectedTabPage == tbLVDetails && keyData == (Keys.PageUp))
                {
                    _IsTabPressed = true;
                    btnPrevious_Click(null, null);
                    return true;
                }
                else if (tcProjectDetails.SelectedTabPage == tbUpdateSupplier && keyData == (Keys.F6))
                {
                    int IValue = 0;
                    if (gvProposal.FocusedRowHandle >= 0 &&
                        int.TryParse(Convert.ToString(gvProposal.GetFocusedRowCellValue("SupplierProposalID")), out IValue))
                    {
                        if (ObjESupplier == null)
                            ObjESupplier = new ESupplier();
                        ObjESupplier.SupplierProposalID = IValue;
                        ObjESupplier.ProjectID = ObjEProject.ProjectID;
                        frmSupplierList Obj = new frmSupplierList();
                        Obj.ObjESupplier = ObjESupplier;
                        Obj.ShowDialog();
                        if (Obj._IsSave && ObjESupplier.SupplierID > 0)
                        {
                            Obj._IsSave = false;
                            ObjESupplier.ProjectID = ObjEProject.ProjectID;
                            if (ObjBSupplier == null)
                                ObjBSupplier = new BSupplier();
                            ObjESupplier = ObjBSupplier.UpdateSupplierProposal(ObjESupplier);
                            DataColumn keyColumn = ObjESupplier.dsProposal.Tables[0].Columns["SupplierProposalID"];
                            DataColumn foreignKeyColumn = ObjESupplier.dsProposal.Tables[1].Columns["SupplierProposalID"];
                            ObjESupplier.dsProposal.Relations.Add("ProposalArticles", keyColumn, foreignKeyColumn);
                            gcProposal.DataSource = ObjESupplier.dsProposal.Tables[0];
                            gcProposal.LevelTree.Nodes.Add("ProposalArticles", gvProposalArt);
                            gvProposalArt.ViewCaption = "Proposal Articles";

                            ExpandAllRows(gvProposal);

                            Utility.Setfocus(gvProposal, "SupplierProposalID", IValue);
                            gvProposal_RowClick(null, null);
                        }
                    }
                    return true;
                }
                else if (tcProjectDetails.SelectedTabPage == tbLVDetails && keyData == (Keys.F1))
                {
                    btnLongDescription_Click(null, null);
                }
                else if (keyData == (Keys.F2))
                {
                    TreeListNode node = tlPositions.FindNodeByFieldValue("Position_OZ", txtLVPositionCD.Text);
                    if (node != null)
                        tlPositions.FocusedNode = node;
                }
            }
            catch (Exception ex) { Utility.ShowError(ex); }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Code to set styles to treelist cells
        /// </summary>
        /// <param name="e"></param>
        void op_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            if (e.ColumnFieldName == "MA_verkaufspreis" ||
                e.ColumnFieldName == "MO_verkaufspreis" ||
                e.ColumnFieldName == "EP" ||
                e.ColumnFieldName == "GB")
            {
                e.Handled = true;
                decimal d = 0;
                if (decimal.TryParse(Convert.ToString(e.Value), out d))
                    e.Value = Math.Round(d, 2, MidpointRounding.AwayFromZero);
            }
        }

        /// <summary>
        /// Code to refresh active project
        /// </summary>
        public void RefreshProject()
        {
            try
            {
                if (tcProjectDetails.SelectedTabPage == tbProjectDetails)
                    LoadExistingProject();
                else if (tcProjectDetails.SelectedTabPage == tbLVDetails)
                {
                    IsRefresh = true;
                    LoadExistingProject();
                    BindPositionData();
                    tlPositions_FocusedNodeChanged(null, null);
                    IsRefresh = false;
                }
                else if (tcProjectDetails.SelectedTabPage == tbBulkProcess)
                    btnApply_Click(null, null);
                else if (tcProjectDetails.SelectedTabPage == tbMulti5)
                {
                    cmbLVSectionFilter_Closed(null, null);
                    if (cmbLVSectionMulti5.EditValue != null)
                        cmbLVSectionMulti5_Closed(null, null);
                }
                else if (tcProjectDetails.SelectedTabPage == tbMulti6)
                {
                    cmbMulti6LVFilter_Closed(null, null);
                    if (cmbLvSectionMulti6.EditValue != null)
                        cmbLvSectionMulti6_Closed(null, null);
                }
                else if (tcProjectDetails.SelectedTabPage == tbOmlage)
                {
                    ObjBProject.GetProjectDetails(ObjEProject);
                    if (ObjEProject.ProjectID > 0 && ObjEProject.CommissionNumber == string.Empty && ObjEProject.ActualLvs > 0)
                    {
                        if (ObjEUmlage == null)
                            ObjEUmlage = new EUmlage();
                        if (ObjBUmlage == null)
                            ObjBUmlage = new BUmlage();
                        ObjEUmlage.ProjectID = ObjEProject.ProjectID;
                        ObjEUmlage = ObjBUmlage.GetSpecialCost(ObjEUmlage);
                        rgUmlageMode.SelectedIndex = ObjEUmlage.UmlageMode;
                        gcOmlage.DataSource = ObjEUmlage.dtSpecialCost;
                        rgUmlageMode_SelectedIndexChanged(null, null);
                        if (Utility.CalcAccess == "7" || ObjEProject.IsFinalInvoice)
                        {
                            btnUmlageSave.Enabled = false;
                            btnAddedCost.Enabled = false;
                        }
                    }
                }
                else if (tcProjectDetails.SelectedTabPage == tbSupplierProposal)
                {
                    if (ObjEProject.ProjectID > 0)
                    {
                        ObjESupplier.ProjectID = ObjEProject.ProjectID;
                        FillLVSection();
                        gcDeletedDetails.DataSource = null;
                        gcProposedDetails.DataSource = null;
                        if (Utility.CalcAccess == "7" || ObjEProject.IsFinalInvoice)
                            btnSaveSupplierProposal.Enabled = false;
                        cmbLVSectionProposal_Closed(null, null);
                        gvProposedSupplier_RowClick(null, null);
                    }
                }
                else if (tcProjectDetails.SelectedTabPage == tbUpdateSupplier)
                {
                    if (ObjEProject.ProjectID > 0)
                    {
                        rgProposalViews.EditValue = 0;
                        FillProposalNumbers();
                        gcDeletedDetails.DataSource = null;
                        gcProposedDetails.DataSource = null;
                        if (Utility.CalcAccess == "7" || ObjEProject.IsFinalInvoice)
                        {
                            //layoutControl16.Enabled = false;
                            btnSubmit.Enabled = false;
                            gvSupplier.OptionsBehavior.Editable = false;
                        }
                        gvProposal_RowClick(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        /// <summary>
        /// Code to expand rows in master detail gridview
        /// </summary>
        /// <param name="View"></param>
        public void ExpandAllRows(GridView View)
        {
            View.BeginUpdate();
            try
            {
                int dataRowCount = View.DataRowCount;
                for (int rHandle = 0; rHandle < dataRowCount; rHandle++)
                    View.SetMasterRowExpanded(rHandle, true);
            }
            finally
            {
                View.EndUpdate();
            }
        }

        #endregion

        #endregion

        #region Project Meta data

        #region Events

        private void btnSaveProject_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (!dxVPProject.Validate())
                    return;
                ParseProjectDetails();
                string strConfirmation = "";
                // Confirmation incase of project convert into order
                if (txtkommissionNumber.Text != string.Empty && txtkommissionNumber.Enabled == true)
                {
                    if (ObjEProject.ActualLvs > 0)
                    {
                        if (Utility._IsGermany == true)
                            strConfirmation = XtraMessageBox.Show("Möchten Sie das Projekt in eine Kommission wandeln?", "Bestätigung …!", MessageBoxButtons.YesNo, MessageBoxIcon.Question).ToString();
                        else
                            strConfirmation = XtraMessageBox.Show("Are you sure you want to convert this Project into Kommission?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question).ToString();
                    }
                    else
                    {
                        if (Utility._IsGermany == true)
                            throw new Exception("Keine LV Positionen.");
                        else
                            throw new Exception("No LV Positions to Order.");
                    }
                }
                else
                {
                    strConfirmation = "Yes";
                }

                if (strConfirmation.ToLower() == "yes")
                {
                    ObjEProject.dtQuerCalc = GetQuerCalcValues();
                    ObjBProject.SaveProjectDetails(ObjEProject);
                    if (!string.IsNullOrEmpty(ObjEProject.CommissionNumber))
                    {
                        DisalbeProjectControls();
                    }
                    if (Utility._IsGermany == true)
                    {
                        this.Text = ObjEProject.ProjectDescription + " - " + ObjEProject.ProjectNumber;
                        XtraMessageBox.Show("'" + ObjEProject.ProjectNumber + "'" + "Die Projektangaben wurden erfolgreich gespeichert");
                    }
                    else
                    {
                        this.Text = ObjEProject.ProjectDescription + " - " + ObjEProject.ProjectNumber;
                        XtraMessageBox.Show("'" + ObjEProject.ProjectNumber + "'" + " Project Details Saved Successfully");
                    }
                    ObjBProject.GetProjectDetails(ObjEProject);
                    cmbLVSection.Properties.DataSource = ObjEProject.dtLVSection;
                    cmbLVSection.Properties.DisplayMember = "LVSectionName";
                    cmbLVSection.Properties.ValueMember = "LVSectionID";
                    BindPositionData();
                }
                else
                {
                    txtkommissionNumber.Text = string.Empty;
                }
                SetMaskForMaulties();
                SetRoundingPriceforColumn();
                txtProjectNumber.Enabled = false;
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnAngebotCommentary_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                string st = ObjBProject.GetAngebotCommentary(ObjEProject.ProjectID);
                frmViewcommentory Obj = new frmViewcommentory();
                Obj.LongDescription = st;
                Obj.ShowDialog();
                if (Obj._IsSave)
                {
                    ObjBProject.SaveAngebotCommentary(ObjEProject.ProjectID, Obj.LongDescription);
                }
            }
            catch (Exception ex) { }
        }

        private void btnCaluclatorCommentary_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                string st = ObjBProject.GetProjectCommentary(ObjEProject.ProjectID);
                frmViewcommentory Obj = new frmViewcommentory();
                Obj.LongDescription = st;
                Obj.ShowDialog();
                if (Obj._IsSave)
                {
                    ObjBProject.SaveProjectCommentary(ObjEProject.ProjectID, Obj.LongDescription);
                }
            }
            catch (Exception ex) { }
        }

        private void btnDataTransfer_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                string strFilePath = string.Empty;
                XtraOpenFileDialog dlg = new XtraOpenFileDialog();

                dlg.InitialDirectory = @"C:\";
                dlg.Title = "Dateiauswahl für Data File Import";

                dlg.CheckFileExists = true;
                dlg.CheckPathExists = true;

                dlg.Filter = "All files (*.*)|*.*";
                dlg.RestoreDirectory = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    SplashScreenManager.Default.SetWaitFormDescription("Datentransfer läuft … ");
                    strFilePath = dlg.FileName;
                    string fileExt = Path.GetExtension(strFilePath);
                    if (fileExt.CompareTo(".xls") == 0 || fileExt.CompareTo(".xlsx") == 0)
                    {
                        DataTable dtExcel = new DataTable();
                        dtExcel = Utility.ReadExcel(strFilePath, fileExt); //read excel file  
                        dtExcel.Rows.RemoveAt(0);
                        dtExcel.Columns.RemoveAt(0);
                        dtExcel.Columns[0].ColumnName = "WG";
                        dtExcel.Columns[1].ColumnName = "WI";
                        dtExcel.Columns[2].ColumnName = "KG";
                        if (ObjEProject == null)
                            ObjEProject = new EProject();
                        if (ObjBProject == null)
                            ObjBProject = new BProject();
                        ObjEProject.dtTemplateData = new DataTable();
                        ObjEProject.dtTemplateData = dtExcel.Copy();
                        ObjEProject.UserName = Utility.UserName;
                        ObjEProject = ObjBProject.GetCockpitData(ObjEProject);
                        if (ObjBProject == null)
                            ObjBProject = new BProject();
                        string strTemp = ObjBProject.InssertCockpitData(ObjEProject);
                        if (!string.IsNullOrEmpty(strTemp))
                            throw new Exception(strTemp);
                        SplashScreenManager.CloseForm(false);
                        frmCalcPro.UpdateStatus("Data tranfer done successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Utility.ShowError(ex);
            }
        }

        private void btnProjectExport_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                ObjBProject.GetProjectDetails(ObjEProject);
                if (ObjEProject.IsFinalInvoice)
                    return;
                BindPositionData();
                if (ObjEProject.ActualLvs == 0)
                {
                    if (Utility._IsGermany == true)
                        XtraMessageBox.Show("Es liegen keine LV Positionen für den Export vor!");
                    else
                        XtraMessageBox.Show("No LV Positions to Export.!");
                    return;
                }
                if (ObjEProject.ProjectID > 0)
                {
                    if (objBGAEB == null)
                        objBGAEB = new BGAEB();
                    if (objEGAEB == null)
                        objEGAEB = new EGAEB();
                    string SelectedRaster = ObjEProject.LVRaster;
                    bool _rtnOldRaster = false;
                    objEGAEB.OldRaster = objBGAEB.GetOld_Raster(ObjEProject.ProjectID);
                    if (objEGAEB.OldRaster != "")
                    {
                        objEGAEB.NewRaster = ObjEProject.LVRaster;
                        frmSelectRaster frm = new frmSelectRaster(objEGAEB);
                        frm.ShowDialog();
                        if (frm.DialogResult == DialogResult.OK)
                        {
                            SelectedRaster = frm.LVRaster;
                            if (ObjEProject.LVRaster != SelectedRaster)
                                _rtnOldRaster = true;
                        }
                        else
                            return;
                    }
                    int raster_count = SelectedRaster.Replace(".", string.Empty).Length;
                    frmGAEBExport Obj = new frmGAEBExport(ObjEProject.ProjectNumber, ObjEProject.ProjectID, raster_count, SelectedRaster, _rtnOldRaster);
                    Obj.KNr = ObjEProject.CommissionNumber;
                    Obj.ShowDialog();
                    if (File.Exists(Obj.OutputFilePath))
                        Process.Start("explorer.exe", "/select, \"" + Obj.OutputFilePath + "\"");
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnProjectImport_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (ObjEProject.ProjectID > 0)
                {
                    if (ObjEProject.IsFinalInvoice)
                        return;
                    frmGAEBImport Obj = new frmGAEBImport();
                    Obj.ProjectID = ObjEProject.ProjectID;
                    Obj.KNr = ObjEProject.CommissionNumber;
                    Obj.ShowDialog();
                    ProjectID = Obj.ProjectID;
                    if (ProjectID > 0 && Obj.isbuild)
                    {
                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                        SplashScreenManager.Default.SetWaitFormDescription("Laden Projekt...");
                        LoadExistingRasters();
                        LoadExistingProject();
                        BindPositionData();
                        IntializeLVPositions();
                        if (ObjEProject.ActualLvs == 0)
                            ChkRaster.Enabled = true;
                        else
                            ChkRaster.Enabled = false;
                        SplashScreenManager.CloseForm(false);
                        frmCalcPro.UpdateStatus("Projektdatenimport mit Erfolg abgeschlossen");
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                if (Utility._IsGermany == true)
                {
                    throw new Exception("Das ausgewählte Datei-Raster ist nicht mit dem ausgewählten Projektraster kompatibel!");
                }
                else
                {
                    Utility.ShowError(ex);
                }
            }
        }

        private void dbtnPriceComp_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (tcProjectDetails.SelectedTabPage == tbLVDetails)
                {
                    if (tlPositions.Nodes.Count > 0)
                    {
                        panelControldoc.Visible = true;
                        toggleSwitchType.Visible = true;
                        dockPanelArticles.Show();
                        dockPanelArticles_Click(null, null);
                        lblArticles.Text = "Artikels :" + txtWG.Text + "/" + txtWA.Text + "/" + txtWI.Text;
                        lblDimensions.Text = "Maße :" + txtDim1.Text + "/" + txtDim2.Text + "/" + txtDim3.Text;
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnFormBlattArt_ItemClick(object sender, ItemClickEventArgs e)
        {
            ObjEPosition.ProjectID = ObjEProject.ProjectID;
            frmFormBlattarticles Obj = new frmFormBlattarticles(ObjEPosition);
            Obj.ShowDialog();
        }

        private void btnProjectArt_ItemClick(object sender, ItemClickEventArgs e)
        {
            ObjEPosition.ProjectID = ObjEProject.ProjectID;
            frmProjectArticles Obj = new frmProjectArticles(ObjEPosition);
            Obj.ShowDialog();
        }

        private void btnArtSettings_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmArticleSettings Obj = new frmArticleSettings(ObjEProject);
            Obj.ShowDialog();
        }

        private void txtLVSprunge_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    e.Handled = true;
                else
                    e.Handled = false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void txtLVSprunge_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtLVSprunge.Text))
                {
                    if (Convert.ToInt32(txtLVSprunge.Text) == 0)
                    {
                        txtLVSprunge.Text = "10";
                    }
                }
                else
                {
                    txtLVSprunge.Text = "10";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void dtpProjectEndDate_QueryPopUp(object sender, CancelEventArgs e)
        {
            dtpProjectEndDate.Properties.MinValue = Convert.ToDateTime(dtpProjectStartDate.EditValue);
        }

        private void dtpProjectStartDate_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                dtpProjectEndDate.EditValue = dtpProjectStartDate.EditValue;
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnFinalBill_Click(object sender, EventArgs e)
        {
            try
            {
                bool _status = chkFinalInvoice.Checked;
                if (_status == true)
                {
                    ObjEProject.Status = "Completed";
                }
                else
                {
                    ObjEProject.Status = "";
                }
                ObjBProject.UpdateStatus(ObjEProject);
                if (Utility.RoleID != 14 && chkFinalInvoice.Checked == true)
                {
                    btnFinalBill.Enabled = false;
                    chkFinalInvoice.Enabled = false;
                    ObjBProject.GetProjectDetails(ObjEProject);
                }
                if (Utility._IsGermany == true)
                {
                    frmCalcPro.UpdateStatus("'" + ObjEProject.ProjectNumber + "'" + " Projektstatus erfolgreich gespeichert");
                }
                else
                {
                    frmCalcPro.UpdateStatus("'" + ObjEProject.ProjectNumber + "'" + " Project Status Saved Successfully");
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void chkLockHierarchy_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkLockHierarchy.Checked == false)
                {
                    if (ddlRaster.Text != "")
                    {
                        frmAddRaster frm = new frmAddRaster(ddlRaster.Text);
                        frm._ProjectID = ObjEProject.ProjectID;
                        frm.ShowDialog();
                        if (frm.DialogResult == DialogResult.OK)
                        {
                            if (ObjEProject == null)
                                ObjEProject = new EProject();
                            ObjEProject.IsRasterChange = true;
                            ddlRaster.Text = frm.NewRaster;
                        }
                    }
                }
                chkLockHierarchy.Checked = true;
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        #endregion

        #region Functions

        /// <summary>
        /// Code to parse project details to its entities while adding or editing a porject
        /// </summary>
        private void ParseProjectDetails()
        {
            try
            {
                int iValue = 0;
                decimal dValue = 0;
                ObjEProject.ProjectNumber = txtProjectNumber.Text;
                ObjEProject.ProjectName = string.Empty;
                ObjEProject.CommissionNumber = txtkommissionNumber.Text;
                ObjEProject.LVRaster = ddlRaster.Text;

                if (int.TryParse(txtLVSprunge.Text, out iValue))
                    ObjEProject.LVSprunge = iValue;
                else
                {
                    if (Utility._IsGermany == true)
                        throw new Exception("Ungültiger LV Sprung");
                    else
                    {
                        throw new Exception("Invalid LV Sprunge");
                    }
                }
                if (decimal.TryParse(txtMWST.Text, out dValue))
                    ObjEProject.Vat = dValue;
                else
                {
                    if (Utility._IsGermany == true)
                        throw new Exception("Ungültige Angabe zur Umsatzsteuer");
                    else
                    {
                        throw new Exception("Invalid Vat");
                    }
                }

                ObjEProject.ProjectDescription = txtBauvorhaben.Text;
                ObjEProject.KundeID = 1;
                if (int.TryParse(Convert.ToString(cmbKundeNo.EditValue), out iValue))
                    ObjEProject.KundeNr = iValue;
                else
                    return;
                ObjEProject.KundeName = cmbKundeNo.Text;
                ObjEProject.PlannedID = 1;
                ObjEProject.PlannerName = txtPlanner.Text;

                if (decimal.TryParse(txtInternX.Text, out dValue))
                    ObjEProject.InternX = dValue;
                else
                {
                    if (Utility._IsGermany == true)
                        throw new Exception("Ungültige Angabe zum internen Verrechnungssatz");
                    else
                    {
                        throw new Exception("Invalid Intern (X)");
                    }
                }

                if (decimal.TryParse(txtInternS.Text, out dValue))
                    ObjEProject.InternS = dValue;
                else
                {
                    if (Utility._IsGermany == true)
                        throw new Exception("Ungültige Angabe zum externen Verrechnungssatz");
                    else
                    {
                        throw new Exception("Invalid Intern (S)");
                    }

                }

                ObjEProject.Submitlocation = txtSubmitLocation.Text;
                ObjEProject.SubmitDate = dtpSubmitDate.DateTime;
                ObjEProject.SubmitTime = dtpSubmitDate.DateTime;

                if (int.TryParse(txtEstimatedLVs.Text, out iValue))
                    ObjEProject.EstimatedLvs = iValue;

                ObjEProject.RoundingPriceID = 1;
                ObjEProject.Remarks = txtRemarks.Text;
                ObjEProject.LockHierarchy = chkLockHierarchy.Checked == true ? true : false;
                ObjEProject.IsCumulated = chkCumulated.Checked == true ? true : false;

                if (int.TryParse(ddlRounding.Text, out iValue))
                    ObjEProject.RoundingPrice = iValue;
                else
                {
                    if (Utility._IsGermany == true)
                        throw new Exception("Ungültige Angabe zur Rundungseinstellung");
                    else
                    {
                        throw new Exception("Invalid Rounding Price");
                    }
                }

                ObjEProject.ProjectStartDate = dtpProjectStartDate.DateTime;
                ObjEProject.ProjectEndDate = dtpProjectEndDate.DateTime;
                ObjEProject.ShowVK = Convert.ToBoolean(chkShowVK.CheckState);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Code to load an existsing project from project load screen
        /// </summary>
        private void LoadExistingProject()
        {
            try
            {
                ObjEProject.ProjectID = ProjectID;
                ObjBProject.GetProjectDetails(ObjEProject);
                if (ObjEProject.dtLVSection != null && ObjEProject.dtLVSection.Rows.Count > 0)
                {
                    cmbLVSection.Properties.DataSource = ObjEProject.dtLVSection;
                    cmbLVSection.Properties.DisplayMember = "LVSectionName";
                    cmbLVSection.Properties.ValueMember = "LVSectionID";
                }
                if (ObjEProject.dtArticleSettings != null && ObjEProject.dtArticleSettings.Rows.Count > 0)
                {
                    foreach (DataRow dr in ObjEProject.dtArticleSettings.Rows)
                    {
                        if (Convert.ToString(dr["SettingName"]) == "Listpreis")
                            ObjEProject.lPVisible = Convert.ToBoolean(dr["IsVisible"]);
                        else if (Convert.ToString(dr["SettingName"]) == "Multi1")
                            ObjEProject.M1Visible = Convert.ToBoolean(dr["IsVisible"]);
                        else if (Convert.ToString(dr["SettingName"]) == "Multi2")
                            ObjEProject.M2Visible = Convert.ToBoolean(dr["IsVisible"]);
                        else if (Convert.ToString(dr["SettingName"]) == "Multi3")
                            ObjEProject.M3Visible = Convert.ToBoolean(dr["IsVisible"]);
                        else if (Convert.ToString(dr["SettingName"]) == "Multi4")
                            ObjEProject.M4Visible = Convert.ToBoolean(dr["IsVisible"]);
                        else if (Convert.ToString(dr["SettingName"]) == "Dim")
                            ObjEProject.DimVisible = Convert.ToBoolean(dr["IsVisible"]);
                        else if (Convert.ToString(dr["SettingName"]) == "Minutes")
                            ObjEProject.MinVisible = Convert.ToBoolean(dr["IsVisible"]);
                        else if (Convert.ToString(dr["SettingName"]) == "Fabrikat")
                            ObjEProject.FabVisible = Convert.ToBoolean(dr["IsVisible"]);
                        else if (Convert.ToString(dr["SettingName"]) == "ME")
                            ObjEProject.MeVisible = Convert.ToBoolean(dr["IsVisible"]);
                    }
                }
                gcQuerCalc.DataSource = ObjEProject.dtQuerCalc;
                if (ProjectID > 0)
                {
                    if (_IsCopy)
                    {
                        txtProjectNumber.Text = string.Empty;
                        txtkommissionNumber.Text = string.Empty;
                        ObjEProject.ProjectID = -1;
                    }
                    else
                    {
                        txtProjectNumber.Text = ObjEProject.ProjectNumber;
                        txtkommissionNumber.Text = ObjEProject.CommissionNumber;
                        txtProjectNumber.Enabled = false;
                        this.Text = ObjEProject.ProjectDescription + " - " + ObjEProject.ProjectNumber;
                    }
                    ddlRaster.SelectedIndex = ddlRaster.Properties.Items.IndexOf(ObjEProject.LVRaster);
                    txtLVSprunge.Text = ObjEProject.LVSprunge.ToString();
                    txtMWST.Text = ObjEProject.Vat.ToString();
                    txtBauvorhaben.Text = ObjEProject.ProjectDescription;
                    cmbKundeNo.EditValue = ObjEProject.KundeNr;
                    txtPlanner.Text = ObjEProject.PlannerName;
                    txtInternX.Text = ObjEProject.InternX.ToString();
                    txtInternS.Text = ObjEProject.InternS.ToString();
                    txtSubmitLocation.Text = ObjEProject.Submitlocation;
                    dtpSubmitDate.DateTime = ObjEProject.SubmitDate;
                    txtEstimatedLVs.Text = ObjEProject.EstimatedLvs.ToString();
                    txtActualLVs.Text = ObjEProject.ActualLvs.ToString();
                    txtRemarks.Text = ObjEProject.Remarks;
                    chkLockHierarchy.Checked = ObjEProject.LockHierarchy;
                    chkCumulated.Checked = ObjEProject.IsCumulated;
                    ddlRounding.SelectedIndex = ddlRounding.Properties.Items.IndexOf(ObjEProject.RoundingPrice.ToString());
                    dtpProjectStartDate.Properties.MinValue = ObjEProject.ProjectStartDate;
                    dtpProjectEndDate.Properties.MinValue = ObjEProject.ProjectStartDate;
                    dtpProjectStartDate.DateTime = ObjEProject.ProjectStartDate;
                    dtpProjectEndDate.DateTime = ObjEProject.ProjectEndDate;
                    chkShowVK.Checked = ObjEProject.ShowVK;
                    if (ObjEProject.IsDisable && !_IsCopy)
                    {
                        ddlRaster.Enabled = false;
                        txtLVSprunge.Enabled = false;
                    }
                    if (!string.IsNullOrEmpty(txtkommissionNumber.Text) && Utility.UserName.ToLower() != "poweradmin")
                        btnSaveProject.Enabled = false;
                    if (ObjEProject.dtDiscount != null)
                        gcDiscount.DataSource = ObjEProject.dtDiscount;
                }
                else
                {
                    txtkommissionNumber.ReadOnly = true;
                    txtLVSprunge.Text = "10";
                    txtInternS.Text = "1";
                    txtInternX.Text = "1";
                    txtMWST.Text = "19.00";
                    ddlRounding.SelectedIndex = ddlRounding.Properties.Items.IndexOf("2");
                    ddlRaster.SelectedIndex = ddlRaster.Properties.Items.IndexOf("99.99.1111.9");
                    dtpSubmitDate.Properties.VistaEditTime = DefaultBoolean.True;
                    dtpSubmitDate.Properties.MinValue = DateTime.Now;
                    dtpProjectStartDate.Properties.MinValue = DateTime.Today;
                    dtpProjectEndDate.Properties.MinValue = DateTime.Today;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Code to get Quer calc values from database and bind to gridcontrol
        /// </summary>
        /// <returns></returns>
        private DataTable GetQuerCalcValues()
        {
            DataTable dt = new DataTable();
            try
            {
                dt = ObjEProject.dtQuerCalc.Clone();
                DataRow dr = null;
                int rowHandle = 0;
                while (gvQuerCalc.IsValidRowHandle(rowHandle))
                {
                    dr = dt.NewRow();
                    dr["KalkDescription"] = gvQuerCalc.GetRowCellValue(rowHandle, "KalkDescription");
                    dr["Value1"] = gvQuerCalc.GetRowCellValue(rowHandle, "Value1");
                    dr["Value2"] = gvQuerCalc.GetRowCellValue(rowHandle, "Value2");
                    dt.Rows.Add(dr);
                    rowHandle++;
                }
            }
            catch (Exception ex) { }
            return dt;
        }

        /// <summary>
        /// Code to disable project controls after saving a project
        /// </summary>
        private void DisalbeProjectControls()
        {
            if (Utility.UserName.ToLower() != "poweradmin")
            {
                txtMWST.Enabled = false;
                btnSaveProject.Enabled = false;
            }
            txtBauvorhaben.Enabled = false;
            cmbKundeNo.Enabled = false;
            txtPlanner.Enabled = false;
            dtpProjectStartDate.Enabled = false;
            dtpProjectEndDate.Enabled = false;
            txtInternX.Enabled = false;
            txtInternS.Enabled = false;
            txtSubmitLocation.Enabled = false;
            dtpSubmitDate.Enabled = false;
            txtEstimatedLVs.Enabled = false;
            ddlRounding.Enabled = false;
            txtActualLVs.Enabled = false;
            chkLockHierarchy.Enabled = false;
            txtRemarks.Enabled = false;
            txtLVSprunge.Enabled = false;
            chkCumulated.Enabled = false;
            gcDiscount.Enabled = false;
            gcQuerCalc.Enabled = false;
            chkShowVK.Enabled = false;
            txtkommissionNumber.Enabled = false;
        }

        #endregion

        #endregion

        #region lV Detials

        #region Internal Class
        /// <summary>
        /// Code to role up the values to parents in a treelist
        /// </summary>
        public class ChildrenSumOperation : TreeListOperation
        {
            private decimal _result = 0;
            public ChildrenSumOperation() : base() { }

            public override void Execute(DevExpress.XtraTreeList.Nodes.TreeListNode node)
            {
                try
                {
                    string stPKZ = Convert.ToString(node["PositionKZ"]);
                    if (stPKZ != "ZS" && stPKZ != "E" && stPKZ != "A")
                    {
                        decimal d = 0;
                        if (Convert.ToString(node["DetailKZ"]) == "0" && decimal.TryParse(Convert.ToString(node["GB"]), out d))
                        {
                            _result += d;
                        }
                    }
                }
                catch (Exception ex) { }
            }

            public decimal Result { get { return _result; } }
        }

        #endregion

        #region Events
        private void tlPositions_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            try
            {
                if (!_IsNewMode && tlPositions.FocusedNode != null && tlPositions.FocusedNode["PositionID"] != null)
                {

                    if (!ObjEProject.IsFinalInvoice)
                    {
                        string strLVSection = tlPositions.FocusedNode["LVSection"].ToString();
                        if (strLVSection.ToLower() != "ha")
                        {
                            if (Utility.LVSectionEditAccess == "9" || Utility.LVSectionEditAccess == "7")
                            {
                                LCGLVDetails.Enabled = false;
                                lcCostDetails.Enabled = false;
                                tlPositions.OptionsBehavior.Editable = false;
                                btnSaveLVDetails.Enabled = false;
                            }
                            else
                            {
                                LCGLVDetails.Enabled = true;
                                lcCostDetails.Enabled = true;
                                tlPositions.OptionsBehavior.Editable = true;
                                btnSaveLVDetails.Enabled = true;
                            }
                        }
                        else
                        {
                            if (Utility.LVDetailsAccess == "8")
                            {
                                LCGLVDetails.Enabled = true;
                                btnNew.Enabled = true;
                                chkCreateNew.Enabled = true;
                            }
                            else
                            {
                                LCGLVDetails.Enabled = false;
                            }
                            if (Utility.CalcAccess == "8")
                            {
                                lcCostDetails.Enabled = true;
                                tlPositions.OptionsBehavior.Editable = true;
                            }
                            else
                            {
                                lcCostDetails.Enabled = false;
                            }
                            btnSaveLVDetails.Enabled = true;
                            btnCancel.Enabled = true;
                        }
                    }
                    LongDescription = string.Empty;
                    string strPositionID = tlPositions.FocusedNode["PositionID"].ToString();
                    ObjEPosition.PositionID = Convert.ToInt32(strPositionID);
                    string strPositionOZ = tlPositions.FocusedNode["Position_OZ"] == DBNull.Value ? "" : Convert.ToString(tlPositions.FocusedNode["Position_OZ"]);

                    if (!string.IsNullOrEmpty(strPositionOZ) && !string.IsNullOrEmpty(ObjEProject.LVRaster))
                    {
                        string[] Titles = strPositionOZ.Split('.');
                        string[] Raster = ObjEProject.LVRaster.Split('.');
                        int TitleCount = Titles.Count();

                        if (Titles != null && Raster != null && TitleCount == Raster.Count())
                        {
                            //if postion 1.1.1.1.10.1
                            TitleCount -= 2;
                            if (TitleCount > 0)
                            {
                                txtStufe1Short.Text = Titles[0];
                                txtStufe1Short_TextChanged(null, null);
                                if (TitleCount > 1)
                                {
                                    txtStufe2Short.Text = Titles[1];
                                    txtStufe2Short_TextChanged(null, null);
                                    if (TitleCount > 2)
                                    {
                                        txtStufe3Short.Text = Titles[2];
                                        txtStufe3Short_TextChanged(null, null);
                                        if (TitleCount > 3)
                                        {
                                            txtStufe4Short.Text = Titles[3];
                                            txtStufe4Short_TextChanged(null, null);
                                        }
                                        else
                                        {
                                            txtStufe4Short.Text = string.Empty;
                                        }
                                    }
                                    else
                                    {
                                        txtStufe3Short.Text = string.Empty;
                                        txtStufe4Short.Text = string.Empty;
                                    }
                                }
                                else
                                {
                                    txtStufe2Short.Text = string.Empty;
                                    txtStufe3Short.Text = string.Empty;
                                    txtStufe4Short.Text = string.Empty;
                                }
                            }
                            else
                            {
                                txtStufe1Short.Text = string.Empty;
                                txtStufe2Short.Text = string.Empty;
                                txtStufe3Short.Text = string.Empty;
                                txtStufe4Short.Text = string.Empty;
                            }
                            txtPosition.Text = Titles[TitleCount] + "." + Titles[TitleCount + 1];
                        }
                        else if (TitleCount <= Raster.Count() - 1)
                        {
                            //if position 1.1.1.1.10
                            TitleCount -= 1;
                            if (TitleCount > 0)
                            {
                                txtStufe1Short.Text = Titles[0];
                                if (TitleCount > 1)
                                {
                                    txtStufe2Short.Text = Titles[1];
                                    if (TitleCount > 2)
                                    {
                                        txtStufe3Short.Text = Titles[2];
                                        if (TitleCount > 3)
                                            txtStufe4Short.Text = Titles[3];
                                        else
                                            txtStufe4Short.Text = string.Empty;
                                    }
                                    else
                                    {
                                        txtStufe3Short.Text = string.Empty;
                                        txtStufe4Short.Text = string.Empty;
                                    }
                                }
                                else
                                {
                                    txtStufe2Short.Text = string.Empty;
                                    txtStufe3Short.Text = string.Empty;
                                    txtStufe4Short.Text = string.Empty;
                                }
                            }
                            else
                            {
                                txtStufe1Short.Text = string.Empty;
                                txtStufe2Short.Text = string.Empty;
                                txtStufe3Short.Text = string.Empty;
                                txtStufe4Short.Text = string.Empty;
                            }
                            txtPosition.Text = Titles[TitleCount];
                        }
                        _FiredFromEvent = true;
                        txtLVPositionCD.Text = txtLVPosition.Text = strPositionOZ;
                    }
                    else
                    {
                        _FiredFromEvent = true;
                        txtLVPositionCD.Text = txtLVPosition.Text = string.Empty;
                        txtStufe1Short.Text = string.Empty;
                        txtStufe2Short.Text = string.Empty;
                        txtStufe3Short.Text = string.Empty;
                        txtStufe4Short.Text = string.Empty;
                        txtPosition.Text = string.Empty;
                    }

                    string stcheckdkz = Convert.ToString(tlPositions.FocusedNode["HaveDetailkz"]);
                    bool _rtn = false;
                    if (bool.TryParse(stcheckdkz, out _rtn))
                    {
                        Position.HaveDetailsKz = _rtn;
                    }

                    bool MontageEntry = false;
                    if (bool.TryParse(Convert.ToString(tlPositions.FocusedNode["MontageEntry"]), out MontageEntry))
                        ChkManualMontageentry.EditValue = MontageEntry;
                    else
                        ChkManualMontageentry.EditValue = MontageEntry;

                    txtWG.Text = txtWGCD.Text = Position.OldWG = tlPositions.FocusedNode["WG"] == DBNull.Value ? "" : tlPositions.FocusedNode["WG"].ToString();
                    txtWI.Text = txtWICD.Text = tlPositions.FocusedNode["WI"] == DBNull.Value ? "" : tlPositions.FocusedNode["WI"].ToString();
                    txtWA.Text = txtWACD.Text = Position.OldWA = tlPositions.FocusedNode["WA"] == DBNull.Value ? "" : tlPositions.FocusedNode["WA"].ToString();
                    txtType.Text = txtTypeCD.Text = tlPositions.FocusedNode["Type"] == DBNull.Value ? "" : tlPositions.FocusedNode["Type"].ToString();
                    txtFabrikate.Text = tlPositions.FocusedNode["Fabricate"] == DBNull.Value ? "" : tlPositions.FocusedNode["Fabricate"].ToString();
                    txtLiefrantMA.Text = tlPositions.FocusedNode["LiefrantMA"] == DBNull.Value ? "" : tlPositions.FocusedNode["LiefrantMA"].ToString();
                    txtMenge.Text = txtMengeCD.Text = tlPositions.FocusedNode["Menge"] == DBNull.Value ? "0" : tlPositions.FocusedNode["Menge"].ToString();
                    cmbPositionKZ.EditValue = cmbPositionKZCD.EditValue = tlPositions.FocusedNode["PositionKZID"];
                    txtDetailKZ.Text = txtDetailKZCD.Text = tlPositions.FocusedNode["DetailKZ"] == DBNull.Value ? "" : tlPositions.FocusedNode["DetailKZ"].ToString();
                    int _DetailKZ = 0;
                    if (int.TryParse(txtDetailKZ.Text, out _DetailKZ))
                    {
                        if (_DetailKZ > 0 || splitContainerControl2.SplitterPosition != 0)
                            btnAddAccessories.Enabled = false;
                        else
                            btnAddAccessories.Enabled = true;
                    }
                    decimal DValue = 0;
                    txtShortDescription.Text = txtShortDescriptionCD.Text =
                        Utility.GetPlaintext(Convert.ToString(tlPositions.FocusedNode["ShortDescription"])).Replace("\n", "\r\n");
                    cmbCDME.Text = cmbME.Text = tlPositions.FocusedNode["ME"] == DBNull.Value ? "" : tlPositions.FocusedNode["ME"].ToString();
                    cmbLVSection.EditValue = tlPositions.FocusedNode["LVSectionID"];
                    cmbLVStatus.EditValue = tlPositions.FocusedNode["LVStatusID"];
                    txtSurchargeFrom.Text = tlPositions.FocusedNode["surchargefrom"] == DBNull.Value ? "" : tlPositions.FocusedNode["surchargefrom"].ToString();
                    txtSurchargeTo.Text = tlPositions.FocusedNode["surchargeto"] == DBNull.Value ? "" : tlPositions.FocusedNode["surchargeto"].ToString();
                    txtSurchargePerME.Text = tlPositions.FocusedNode["surchargePercentage"] == DBNull.Value ? "" : tlPositions.FocusedNode["surchargePercentage"].ToString();
                    txtSurchargePerMO.Text = tlPositions.FocusedNode["surchargePercentage_MO"] == DBNull.Value ? "" : tlPositions.FocusedNode["surchargePercentage_MO"].ToString();
                    dtpValidityDate.EditValue = tlPositions.FocusedNode["validitydate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(tlPositions.FocusedNode["validitydate"]);
                    txtMa.Text = tlPositions.FocusedNode["MA"] == DBNull.Value ? "" : tlPositions.FocusedNode["MA"].ToString();
                    txtMo.Text = tlPositions.FocusedNode["MO"] == DBNull.Value ? "" : tlPositions.FocusedNode["MO"].ToString();
                    decimal dval = 0;
                    if (decimal.TryParse(Convert.ToString(tlPositions.FocusedNode["MINUTES"]), out dval))
                        txtMin.EditValue = dval;
                    txtStdSatz.Text = tlPositions.FocusedNode["std_satz"] == DBNull.Value ? "0" : tlPositions.FocusedNode["std_satz"].ToString();
                    txtFaktor.Text = tlPositions.FocusedNode["Faktor"] == DBNull.Value ? "" : tlPositions.FocusedNode["Faktor"].ToString();
                    if (decimal.TryParse(Convert.ToString(tlPositions.FocusedNode["MA_listprice"]), out DValue) && DValue != 0)
                    {
                        txtLPMe.Properties.Mask.UseMaskAsDisplayFormat = true;
                        Position.MA_ListPrice = DValue;
                        txtLPMe.EditValue = Math.Round(DValue, 2);
                    }
                    else
                    {
                        Position.MA_ListPrice = 0;
                        txtLPMe.Properties.Mask.UseMaskAsDisplayFormat = false;
                        txtLPMe.Text = string.Empty;
                    }

                    if (decimal.TryParse(Convert.ToString(tlPositions.FocusedNode["MO_listprice"]), out DValue) && DValue != 0)
                    {
                        txtLPMO.Properties.Mask.UseMaskAsDisplayFormat = true;
                        Position.MO_ListPrice = DValue;
                        txtLPMO.EditValue = Math.Round(DValue, 2);
                    }
                    else
                    {
                        Position.MO_ListPrice = 0;
                        txtLPMO.Properties.Mask.UseMaskAsDisplayFormat = false;
                        txtLPMO.Text = string.Empty;
                    }
                    txtMulti1ME.Text = tlPositions.FocusedNode["MA_Multi1"] == DBNull.Value ? "1" : tlPositions.FocusedNode["MA_Multi1"].ToString();
                    txtMulti2ME.Text = tlPositions.FocusedNode["MA_multi2"] == DBNull.Value ? "1" : tlPositions.FocusedNode["MA_multi2"].ToString();
                    txtMulti3ME.Text = tlPositions.FocusedNode["MA_multi3"] == DBNull.Value ? "1" : tlPositions.FocusedNode["MA_multi3"].ToString();
                    txtMulti4ME.Text = tlPositions.FocusedNode["MA_multi4"] == DBNull.Value ? "1" : tlPositions.FocusedNode["MA_multi4"].ToString();
                    if (decimal.TryParse(Convert.ToString(tlPositions.FocusedNode["MA_einkaufspreis"]), out DValue) && DValue != 0)
                    {
                        txtEinkaufspreisME.Properties.Mask.UseMaskAsDisplayFormat = true;
                        Position.MA_EP = DValue;
                        txtEinkaufspreisME.EditValue = Math.Round(DValue, 2);
                    }
                    else
                    {
                        Position.MA_EP = 0;
                        txtEinkaufspreisME.Properties.Mask.UseMaskAsDisplayFormat = false;
                        txtEinkaufspreisME.Text = string.Empty;
                    }

                    txtSelbstkostenMultiME.Text = tlPositions.FocusedNode["MA_selbstkostenMulti"] == DBNull.Value ? "1" : tlPositions.FocusedNode["MA_selbstkostenMulti"].ToString();
                    if (decimal.TryParse(Convert.ToString(tlPositions.FocusedNode["MA_selbstkosten"]), out DValue) && DValue != 0)
                    {
                        txtSelbstkostenValueME.Properties.Mask.UseMaskAsDisplayFormat = true;
                        Position.MA_SK = DValue;
                        txtSelbstkostenValueME.EditValue = Math.Round(DValue, 2);
                    }
                    else
                    {
                        Position.MA_SK = 0;
                        txtSelbstkostenValueME.Properties.Mask.UseMaskAsDisplayFormat = false;
                        txtSelbstkostenValueME.Text = string.Empty;
                    }
                    txtVerkaufspreisMultiME.Text = tlPositions.FocusedNode["MA_verkaufspreis_Multi"] == DBNull.Value ? "1" : tlPositions.FocusedNode["MA_verkaufspreis_Multi"].ToString();
                    if (decimal.TryParse(Convert.ToString(tlPositions.FocusedNode["MA_verkaufspreis"]), out DValue) && DValue != 0)
                    {
                        txtVerkaufspreisValueME.Properties.Mask.UseMaskAsDisplayFormat = true;
                        Position.MA_VK = DValue;
                        txtVerkaufspreisValueME.EditValue = Math.Round(DValue, 2);
                    }
                    else
                    {
                        Position.MA_VK = 0;
                        txtVerkaufspreisValueME.Properties.Mask.UseMaskAsDisplayFormat = false;
                        txtVerkaufspreisValueME.Text = string.Empty;
                    }
                    txtMulti1MO.Text = tlPositions.FocusedNode["MO_multi1"] == DBNull.Value ? "1" : tlPositions.FocusedNode["MO_multi1"].ToString();
                    txtMulti2MO.Text = tlPositions.FocusedNode["MO_multi2"] == DBNull.Value ? "1" : tlPositions.FocusedNode["MO_multi2"].ToString();
                    txtMulti3MO.Text = tlPositions.FocusedNode["MO_multi3"] == DBNull.Value ? "1" : tlPositions.FocusedNode["MO_multi3"].ToString();
                    txtMulti4MO.Text = tlPositions.FocusedNode["MO_multi4"] == DBNull.Value ? "1" : tlPositions.FocusedNode["MO_multi4"].ToString();
                    if (decimal.TryParse(Convert.ToString(tlPositions.FocusedNode["MO_Einkaufspreis"]), out DValue) && DValue != 0)
                    {
                        txtEinkaufspreisMO.Properties.Mask.UseMaskAsDisplayFormat = true;
                        Position.MO_EP = DValue;
                        txtEinkaufspreisMO.Text = Math.Round(DValue, 2).ToString("F2");
                    }
                    else
                    {
                        Position.MO_EP = 0;
                        txtEinkaufspreisMO.Properties.Mask.UseMaskAsDisplayFormat = false;
                        txtEinkaufspreisMO.Text = string.Empty;
                    }

                    txtSelbstkostenMultiMO.Text = tlPositions.FocusedNode["MO_selbstkostenMulti"] == DBNull.Value ? "1" : tlPositions.FocusedNode["MO_selbstkostenMulti"].ToString();
                    if (decimal.TryParse(Convert.ToString(tlPositions.FocusedNode["MO_selbstkosten"]), out DValue) && DValue != 0)
                    {
                        txtSelbstkostenValueMO.Properties.Mask.UseMaskAsDisplayFormat = true;
                        Position.MO_SK = DValue;
                        txtSelbstkostenValueMO.EditValue = Math.Round(DValue, 2);
                    }
                    else
                    {
                        Position.MO_SK = 0;
                        txtSelbstkostenValueMO.Properties.Mask.UseMaskAsDisplayFormat = false;
                        txtSelbstkostenValueMO.Text = string.Empty;
                    }
                    txtVerkaufspreisMultiMO.Text = tlPositions.FocusedNode["MO_verkaufspreisMulti"] == DBNull.Value ? "1" : tlPositions.FocusedNode["MO_verkaufspreisMulti"].ToString();
                    if (decimal.TryParse(Convert.ToString(tlPositions.FocusedNode["MO_verkaufspreis"]), out DValue) && DValue != 0)
                    {
                        txtVerkaufspreisValueMO.Properties.Mask.UseMaskAsDisplayFormat = true;
                        Position.MO_VK = DValue;
                        txtVerkaufspreisValueMO.EditValue = Math.Round(DValue, 2);
                    }
                    else
                    {
                        Position.MO_VK = 0;
                        txtVerkaufspreisValueMO.Properties.Mask.UseMaskAsDisplayFormat = false;
                        txtVerkaufspreisValueMO.Text = string.Empty;
                    }
                    txtPreisText.Text = tlPositions.FocusedNode["PreisText"] == DBNull.Value ? "" : tlPositions.FocusedNode["PreisText"].ToString();
                    chkEinkaufspreisME.EditValue = tlPositions.FocusedNode["MA_einkaufspreis_lck"] == DBNull.Value ? true : Convert.ToBoolean(tlPositions.FocusedNode["MA_einkaufspreis_lck"]);
                    chkSelbstkostenME.EditValue = tlPositions.FocusedNode["MA_selbstkosten_lck"] == DBNull.Value ? true : Convert.ToBoolean(tlPositions.FocusedNode["MA_selbstkosten_lck"]);
                    chkVerkaufspreisME.EditValue = tlPositions.FocusedNode["MA_verkaufspreis_lck"] == DBNull.Value ? true : Convert.ToBoolean(tlPositions.FocusedNode["MA_verkaufspreis_lck"]);
                    chkEinkaufspreisMO.EditValue = tlPositions.FocusedNode["MO_Einkaufspreis_lck"] == DBNull.Value ? true : Convert.ToBoolean(tlPositions.FocusedNode["MO_Einkaufspreis_lck"]);
                    chkSelbstkostenMO.EditValue = tlPositions.FocusedNode["MO_selbstkosten_lck"] == DBNull.Value ? true : Convert.ToBoolean(tlPositions.FocusedNode["MO_selbstkosten_lck"]);
                    chkVerkaufspreisMO.EditValue = tlPositions.FocusedNode["MO_verkaufspreis_lck"] == DBNull.Value ? true : Convert.ToBoolean(tlPositions.FocusedNode["MO_verkaufspreis_lck"]);
                    txtDim1.Text = tlPositions.FocusedNode["A"] == DBNull.Value ? "" : tlPositions.FocusedNode["A"].ToString();
                    txtDim2.Text = tlPositions.FocusedNode["B"] == DBNull.Value ? "" : tlPositions.FocusedNode["B"].ToString();
                    txtDim3.Text = tlPositions.FocusedNode["L"] == DBNull.Value ? "" : tlPositions.FocusedNode["L"].ToString();
                    txtDim.Text = tlPositions.FocusedNode["dim"] == DBNull.Value ? "" : tlPositions.FocusedNode["dim"].ToString();
                    _DocuwareLink1 = tlPositions.FocusedNode["DocuwareLink1"] == DBNull.Value ? "" : tlPositions.FocusedNode["DocuwareLink1"].ToString();
                    _DocuwareLink2 = tlPositions.FocusedNode["DocuwareLink2"] == DBNull.Value ? "" : tlPositions.FocusedNode["DocuwareLink2"].ToString();
                    _DocuwareLink3 = tlPositions.FocusedNode["DocuwareLink3"] == DBNull.Value ? "" : tlPositions.FocusedNode["DocuwareLink3"].ToString();
                    decimal dValue = 0;
                    if (decimal.TryParse(Convert.ToString(tlPositions.FocusedNode["GrandTotalME"]), out dValue) && dValue != 0)
                    {
                        txtGrandTotalME.Properties.Mask.UseMaskAsDisplayFormat = true;
                        txtGrandTotalME.EditValue = dValue;
                    }
                    else
                    {
                        txtGrandTotalME.Properties.Mask.UseMaskAsDisplayFormat = false;
                        txtGrandTotalME.Text = string.Empty;
                    }

                    if (decimal.TryParse(Convert.ToString(tlPositions.FocusedNode["GrandTotalMO"]), out dValue) && dValue != 0)
                    {
                        txtGrandTotalMO.Properties.Mask.UseMaskAsDisplayFormat = true;
                        txtGrandTotalMO.EditValue = dValue;
                    }
                    else
                    {
                        txtGrandTotalMO.Properties.Mask.UseMaskAsDisplayFormat = false;
                        txtGrandTotalMO.Text = string.Empty;
                    }

                    if (decimal.TryParse(Convert.ToString(tlPositions.FocusedNode["GB"]), out dValue) && dValue != 0)
                    {
                        txtFinalGB.Properties.Mask.UseMaskAsDisplayFormat = true;
                        txtFinalGB.EditValue = dValue;
                    }
                    else
                    {
                        txtFinalGB.Properties.Mask.UseMaskAsDisplayFormat = false;
                        txtFinalGB.Text = string.Empty;
                    }

                    if (decimal.TryParse(Convert.ToString(tlPositions.FocusedNode["EP"]), out dValue) && dValue != 0)
                    {
                        txtEP.Properties.Mask.UseMaskAsDisplayFormat = true;
                        txtEP.EditValue = dValue;
                    }
                    else
                    {
                        txtEP.Properties.Mask.UseMaskAsDisplayFormat = false;
                        txtEP.Text = string.Empty;
                    }

                    txtDiscount.Text = tlPositions.FocusedNode["Discount"] == DBNull.Value ? "0" : tlPositions.FocusedNode["Discount"].ToString();


                    if (Utility.CalcAccess != "7")
                    {
                        tlPositions.Columns["MA_Multi1"].OptionsColumn.AllowEdit
                            = tlPositions.Columns["MA_multi2"].OptionsColumn.AllowEdit
                            = tlPositions.Columns["MA_multi3"].OptionsColumn.AllowEdit
                            = tlPositions.Columns["MA_multi4"].OptionsColumn.AllowEdit = Convert.ToBoolean(!chkEinkaufspreisME.Checked);

                        tlPositions.Columns["MO_multi1"].OptionsColumn.AllowEdit
                            = tlPositions.Columns["MO_multi2"].OptionsColumn.AllowEdit
                            = tlPositions.Columns["MO_multi3"].OptionsColumn.AllowEdit
                            = tlPositions.Columns["MO_multi4"].OptionsColumn.AllowEdit = Convert.ToBoolean(!chkEinkaufspreisMO.Checked);

                        tlPositions.Columns["MA_selbstkostenMulti"].OptionsColumn.AllowEdit = Convert.ToBoolean(!chkSelbstkostenME.Checked);
                        tlPositions.Columns["MO_selbstkostenMulti"].OptionsColumn.AllowEdit = Convert.ToBoolean(!chkSelbstkostenMO.Checked);
                        tlPositions.Columns["MA_verkaufspreis_Multi"].OptionsColumn.AllowEdit = Convert.ToBoolean(!chkVerkaufspreisME.Checked);
                        tlPositions.Columns["MO_verkaufspreisMulti"].OptionsColumn.AllowEdit = Convert.ToBoolean(!chkVerkaufspreisMO.Checked);
                    }

                    if (tlPositions.Nodes.Count > 0)
                    {
                        if (e != null)
                        {
                            if (e.Node.HasChildren)
                                btnLongDescription.Enabled = false;
                            else
                                btnLongDescription.Enabled = true;
                        }
                    }
                    txtMo_TextChanged(null, null);
                    txtMin_TextChanged(null, null);
                    if (txtkommissionNumber.Text != string.Empty)
                    {
                        chkEinkaufspreisME_CheckedChanged(null, null);
                        chkEinkaufspreisMO_CheckedChanged(null, null);
                        chkVerkaufspreisME_CheckedChanged(null, null);
                        chkVerkaufspreisMO_CheckedChanged(null, null);
                    }
                    if (Position.HaveDetailsKz)
                    {
                        if (Position.MA_SK != 0)
                            txtVerkaufspreisMultiME.EditValue = Math.Round(Position.MA_VK / Position.MA_SK, 3);
                        if (Position.MO_SK != 0)
                            txtVerkaufspreisMultiMO.EditValue = Math.Round(Position.MO_VK / Position.MO_SK, 3);
                        if (Position.MA_EP != 0)
                            txtSelbstkostenMultiME.EditValue = Math.Round(Position.MA_SK / Position.MA_EP, 3);
                        if (Position.MO_EP != 0)
                            txtSelbstkostenMultiMO.EditValue = Math.Round(Position.MO_SK / Position.MO_EP, 3);
                        if (Position.MA_ListPrice != 0)
                            txtGrundMultiME1.EditValue = Math.Round(Position.MA_EP / Position.MA_ListPrice, 3);
                        if (Position.MO_ListPrice != 0)
                            txtGrundMultiMO1.EditValue = Math.Round(Position.MO_EP / Position.MO_ListPrice, 3);
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
            finally
            {
                if (_FiredFromEvent)
                    _FiredFromEvent = false;
            }
        }

        private void btnLongDescription_Click(object sender, EventArgs e)
        {
            try
            {
                int ivpos = 0;
                if (tlPositions.FocusedNode != null && int.TryParse(Convert.ToString(tlPositions.FocusedNode["PositionID"]), out ivpos))
                {
                    if (Convert.ToString(tlPositions.FocusedNode["PositionKZ"]) != "NG")
                    {
                        frmViewdescription Obj = new frmViewdescription(_IsNewMode, this, ivpos);
                        if (LongDescription == string.Empty && !_IsNewMode)
                            Obj.LongDescription = ObjBPosition.GetLongDescription(ivpos);
                        else
                            Obj.LongDescription = LongDescription;
                        Obj.ShowDialog();
                        LongDescription = Obj.LongDescription;
                        if (Obj._IsSave && !_IsNewMode)
                        {
                            int ivalue = 0;
                            if (int.TryParse(Convert.ToString(tlPositions.FocusedNode["PositionID"]), out ivalue))
                            {
                                ObjBPosition.UpdateLongDescription(ivalue, LongDescription);
                                LongDescription = string.Empty;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void treeList1_GetNodeDisplayValue(object sender, GetNodeDisplayValueEventArgs e)
        {
            try
            {
                TreeList tl = sender as TreeList;
                if (e.Node.HasChildren)
                {
                    ChildrenSumOperation op = new ChildrenSumOperation();
                    tl.NodesIterator.DoLocalOperation(op, e.Node.Nodes);
                    if (e.Column.FieldName == "GB")
                        e.Value = op.Result;
                }
            }
            catch (Exception ex) { }
        }

        private void cmbPositionKZ_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string stPosKZ = Utility.GetPosKZ(cmbPositionKZ.Text).ToLower();
                if (!string.IsNullOrEmpty(stPosKZ) && (stPosKZ == "zs"
                    || stPosKZ == "z") || stPosKZ == "zz")
                {
                    FormatFieldsForSum();
                    if (stPosKZ == "zs")
                    {
                        txtSurchargePerME.Enabled = false;
                        txtSurchargePerMO.Enabled = false;
                        txtDiscount.Enabled = false;
                        txtSurchargeFrom.Enabled = true;
                        txtSurchargeTo.Enabled = true;
                        if (_IsNewMode)
                        {
                            string strParentOZ = PrepareOZ();
                            txtSurchargeFrom.Text = FromOZ(strParentOZ, "ZS");
                            txtSurchargeTo.Text = ToOZ(strParentOZ, "ZS");
                        }
                        lcCostDetails.Enabled = false;
                    }
                    else if (stPosKZ == "z")
                    {
                        if (tlPositions.FocusedNode != null)
                        {
                            txtSurchargePerME.Enabled = true;
                            txtSurchargePerMO.Enabled = true;
                            txtDiscount.Enabled = false;
                            txtSurchargeFrom.Enabled = true;
                            txtSurchargeTo.Enabled = true;
                            txtPosition.Enabled = true;
                            if (_IsNewMode)
                            {
                                string strParentOZ = PrepareOZ();
                                txtSurchargeFrom.Text = FromOZ(strParentOZ, "Z");
                                txtSurchargeTo.Text = ToOZ(strParentOZ, "Z");
                                txtSurchargePerME.Text = "1";
                                txtSurchargePerMO.Text = "1";
                            }
                        }
                        lcCostDetails.Enabled = false;
                    }
                    else if (stPosKZ == "zz")
                    {
                        txtSurchargePerME.Enabled = false;
                        txtSurchargePerMO.Enabled = false;
                        txtDiscount.Enabled = true;
                        txtSurchargeFrom.Enabled = true;
                        txtSurchargeTo.Enabled = true;
                    }
                }
                else
                {
                    FormatFieldsForNormal();
                    ClearingValues();
                    lcCostDetails.Enabled = true;
                }

                if (stPosKZ == "h" || stPosKZ == "vr" || stPosKZ == "ub" || stPosKZ == "ab" || stPosKZ == "ba")
                {
                    EnableAndDisableAllControls(false);
                }
                else
                {
                    EnableAndDisableAllControls(true);
                }
                if (stPosKZ == "p")
                {
                    cmbCDME.Text = "psch";
                }
            }
            catch (Exception ex) { }
        }

        private void btnSaveLVDetails_Click(object sender, EventArgs e)
        {
            try
            {
                string stPosKZ = Utility.GetPosKZ(cmbPositionKZ.Text);
                if (stPosKZ == "ZZ" || stPosKZ == "VR" || stPosKZ == "AB" || stPosKZ == "BA" || stPosKZ == "UB" || stPosKZ == "LO")
                    return;

                if (ObjBPosition == null)
                    ObjBPosition = new BPosition();

                Log.Info("Save Proccess Started");
                if (!string.IsNullOrEmpty(txtWG.Text) && !string.IsNullOrEmpty(txtWA.Text) && (_IsNewMode || (txtWG.Text != Position.OldWG || txtWA.Text != Position.OldWA)))
                {
                    ObjEPosition.dtDimensions = new DataTable();
                    ObjEPosition.dtArticle = new DataTable();
                    ObjEPosition.WG = txtWG.Text;
                    ObjEPosition.WA = txtWA.Text;
                    ObjEPosition.WI = txtWI.Text;
                    ObjEPosition.ValidityDate = ObjEProject.SubmitDate;
                    ObjEPosition.ProjectID = ObjEProject.ProjectID;
                    ObjEPosition = ObjBPosition.GetArticleByWGWA(ObjEPosition);
                    if (ObjEPosition.dtArticle.Rows.Count == 0)
                    {
                        var dlgResult = XtraMessageBox.Show("Do you want to save the new article?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (Convert.ToString(dlgResult).ToLower() == "yes")
                        {
                            frmAddProjectArticles Obj = new frmAddProjectArticles(ObjEPosition);
                            Obj.ShowDialog();
                            if (Obj.IsContinue)
                            {
                                txtWI.Text = string.Empty;
                                txtWICD.Text = string.Empty;
                            }
                            else
                            {
                                txtWI.Text = string.Empty;
                                txtWICD.Text = string.Empty;
                                SavePositionArticle();
                            }
                        }
                        else
                        {
                            txtWI.Text = string.Empty;
                            txtWICD.Text = string.Empty;
                            SavePositionArticle();
                        }
                    }
                }
                Log.Info("Checking article is completed..");
                if (!dxValidationProvider1.Validate())
                    return;
                if (_IsNewMode == false)
                    btnModify_Click(null, null);
                if (cmbLVSection.Text == "NT" || cmbLVSection.Text == "NTM")
                {
                    if (Utility._IsGermany == true)
                        throw new Exception("Positionen mit NT oder NTM können nicht gespeichert werden");
                    else
                        throw new Exception("Cannot Save Positions With NT Or NTM");
                }
                if (stPosKZ != "H")
                {
                    if (!string.IsNullOrEmpty(txtPosition.Text))
                    {
                        if (Utility.ValidateRequiredFields(RequiredPositionFields) == false)
                            return;
                    }
                    if (!string.IsNullOrEmpty(txtPosition.Text))
                    {
                        if (Utility.ValidateRequiredFields(RequiredPositionFieldsforTitle) == false)
                            return;
                    }
                    if (string.IsNullOrEmpty(txtStufe1Short.Text))
                    {
                        if (Utility._IsGermany == true)
                            throw new Exception("Bitte geben Sie mindestens eine Stufe ein");
                        else
                            throw new Exception("Plesae Enter Atleast One Stufe");
                    }
                    if (stPosKZ == "Z" || stPosKZ == "ZS")
                    {
                        if (string.IsNullOrEmpty(txtSurchargeFrom.Text))
                            if (Utility._IsGermany == true)
                                throw new Exception("Diese Position kann nicht angelegt werden ohne folgende Angaben: VON und ZU.");
                            else
                                throw new Exception("Position Cannot be created with Empty From and To Fields");
                    }
                }
                if (string.IsNullOrEmpty(txtPosition.Text))
                    txtDetailKZ.Text = "0";
                if (string.IsNullOrEmpty(stPosKZ))
                    Utility.SetLookupEditValue(cmbPositionKZ, "N-Normalposition");
                if (ObjEPosition == null)
                    ObjEPosition = new EPosition();
                Log.Info("Position Values Parsing Started");
                ParsePositionDetails();
                if (ObjEPosition.PositionKZ.ToLower() == "h")
                {
                    if (tlPositions != null && tlPositions.Nodes.Count > 0 && tlPositions.FocusedNode != null && tlPositions.FocusedNode.ParentNode != null)
                        ObjEPosition.Parent_OZ = Convert.ToString(tlPositions.FocusedNode.ParentNode["Position_OZ"]);
                    else
                        ObjEPosition.Parent_OZ = "";
                }
                Log.Info("Save database transaction Started");
                ObjEPosition.RoundOffValue = ObjEProject.RoundingPrice;
                ObjBPosition.SavePositionDetails(ObjEPosition, ObjEProject.LVRaster, ObjEProject.LVSprunge, false);
                RefreshTreelist(ObjEPosition);
                if (Utility._IsGermany == true)
                    frmCalcPro.UpdateStatus("'" + ObjEPosition.Position_OZ + "'" + " Vorgang abgeschlossen: Speichern der OZ");
                else
                    frmCalcPro.UpdateStatus("'" + ObjEPosition.Position_OZ + "'" + " OZ Saved Successfully");
                if (Convert.ToBoolean(chkCreateNew.EditValue) == true)
                    btnNew_Click(null, null);
                else
                {
                    EnableDisableLVAndCostDetails(false, false, false, false);
                    EnableDisableButtons(true, true, false, true, true);
                    tlPositions.OptionsBehavior.ReadOnly = false;
                    Color _Color = Color.FromArgb(0, 158, 224);
                    tlPositions.Appearance.HeaderPanel.BackColor = _Color;
                    LCGLVDetails.Root.AppearanceGroup.BackColor = _Color;
                }
                if (!_IsTabPressed)
                {
                    if (stPosKZ == "H")
                    {
                        tlPositions.MoveNext();
                    }
                }
                Log.Info("Save Proccess Completed");
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnRefreshTreeView_Click(object sender, EventArgs e)
        {
            try
            {
                BindPositionData();
            }
            catch (Exception ex) { Utility.ShowError(ex); }
        }

        #region 'Textchanged events for cost details calculations'

        private void txtLPMe_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (_IsNewMode || !Position.HaveDetailsKz)
                {
                    Position.MA_ListPrice = getDValue(txtLPMe.Text);
                    Position.MA_Multi1 = Math.Round(GetValue(Position.MA_ListPrice, getDValue(txtMulti1ME.Text)), 8);
                    if (Position.MA_Multi1 == 0)
                    {
                        txtValue1ME.Properties.Mask.UseMaskAsDisplayFormat = false;
                        txtValue1ME.Text = string.Empty;
                    }
                    else
                    {
                        txtValue1ME.Properties.Mask.UseMaskAsDisplayFormat = true;
                        txtValue1ME.EditValue = Math.Round(Position.MA_Multi1, 2, MidpointRounding.AwayFromZero);
                    }
                    CalculateEinkuafpreisME();
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtMulti1ME_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (_IsNewMode || !Position.HaveDetailsKz)
                {
                    Position.MA_Multi1 = Math.Round(GetValue(Position.MA_ListPrice,
                    getDValue(txtMulti1ME.Text)), 8);
                    if (Position.MA_Multi1 == 0)
                    {
                        txtValue1ME.Properties.Mask.UseMaskAsDisplayFormat = false;
                        txtValue1ME.Text = string.Empty;
                    }
                    else
                    {
                        txtValue1ME.Properties.Mask.UseMaskAsDisplayFormat = true;
                        txtValue1ME.EditValue = Math.Round(Position.MA_Multi1, 2, MidpointRounding.AwayFromZero);
                    }
                    CalculateGrundMultiME();
                    CalculateEinkuafpreisME();
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtValue1ME_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (_IsNewMode || !Position.HaveDetailsKz)
                {
                    Position.MA_Multi2 = Math.Round(GetValue(Position.MA_ListPrice +
                    Position.MA_Multi1, getDValue(txtMulti2ME.Text)), 8);

                    if (Position.MA_Multi2 == 0)
                    {
                        txtValue2ME.Properties.Mask.UseMaskAsDisplayFormat = false;
                        txtValue2ME.Text = string.Empty;
                    }
                    else
                    {
                        txtValue2ME.Properties.Mask.UseMaskAsDisplayFormat = true;
                        txtValue2ME.EditValue = Math.Round(Position.MA_Multi2, 2, MidpointRounding.AwayFromZero);
                    }
                    CalculateGrundMultiME();
                    CalculateGrundValueME();
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtValue2ME_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (_IsNewMode || !Position.HaveDetailsKz)
                {
                    Position.MA_Multi3 = Math.Round(GetValue(Position.MA_ListPrice + Position.MA_Multi1
                    + Position.MA_Multi2, getDValue(txtMulti3ME.Text)), 8);

                    if (Position.MA_Multi3 == 0)
                    {
                        txtValue3ME.Properties.Mask.UseMaskAsDisplayFormat = false;
                        txtValue3ME.Text = string.Empty;
                    }
                    else
                    {
                        txtValue3ME.Properties.Mask.UseMaskAsDisplayFormat = true;
                        txtValue3ME.EditValue = Math.Round(Position.MA_Multi3, 2, MidpointRounding.AwayFromZero);
                    }
                    CalculateGrundMultiME();
                    CalculateGrundValueME();
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtValue3ME_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (_IsNewMode || !Position.HaveDetailsKz)
                {
                    Position.MA_Multi4 = Math.Round(GetValue(Position.MA_ListPrice + Position.MA_Multi1 +
                    Position.MA_Multi2 + Position.MA_Multi3, getDValue(txtMulti4ME.Text)), 8);
                    if (Position.MA_Multi4 == 0)
                    {
                        txtValue4ME.Properties.Mask.UseMaskAsDisplayFormat = false;
                        txtValue4ME.Text = string.Empty;
                    }
                    else
                    {
                        txtValue4ME.Properties.Mask.UseMaskAsDisplayFormat = true;
                        txtValue4ME.EditValue = Math.Round(Position.MA_Multi4, 2, MidpointRounding.AwayFromZero);
                    }
                    CalculateGrundMultiME();
                    CalculateGrundValueME();
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtValue4ME_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (_IsNewMode || !Position.HaveDetailsKz)
                {
                    CalculateGrundMultiME();
                    CalculateGrundValueME();
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtEinkaufspreisME_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (_IsNewMode || !Position.HaveDetailsKz)
                {
                    Position.MA_SK = Math.Round(Position.MA_EP + GetValue(Position.MA_EP,
                        getDValue(txtSelbstkostenMultiME.Text)), 8);
                    if (Position.MA_SK != 0)
                    {
                        txtSelbstkostenValueME.Properties.Mask.UseMaskAsDisplayFormat = true;
                        txtSelbstkostenValueME.EditValue = Math.Round(Position.MA_SK, 2, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        txtSelbstkostenValueME.Properties.Mask.UseMaskAsDisplayFormat = false;
                        txtSelbstkostenValueME.Text = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtSelbstkostenValueME_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (_IsNewMode || !Position.HaveDetailsKz)
                {
                    if (IsFire)
                    {
                        Position.MA_VK = Math.Round(Position.MA_SK + GetValue(Position.MA_SK,
                            getDValue(txtVerkaufspreisMultiME.Text)), 8);
                        if (Position.MA_VK != 0)
                        {
                            txtVerkaufspreisValueME.Properties.Mask.UseMaskAsDisplayFormat = true;
                            txtVerkaufspreisValueME.EditValue = Math.Round(Position.MA_VK, 2, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            txtVerkaufspreisValueME.Properties.Mask.UseMaskAsDisplayFormat = false;
                            txtVerkaufspreisValueME.Text = string.Empty;
                        }
                    }
                    else
                    {
                        IsFire = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtLPMO_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (_IsNewMode || !Position.HaveDetailsKz)
                {
                    Position.MO_ListPrice = Math.Round(getDValue(txtLPMO.Text), 8);
                    Position.MO_Multi1 = Math.Round(GetValue(Position.MO_ListPrice,
                        getDValue(txtMulti1MO.Text)), 8);

                    if (Position.MO_Multi1 == 0)
                    {
                        txtValue1MO.Properties.Mask.UseMaskAsDisplayFormat = false;
                        txtValue1MO.Text = string.Empty;
                    }
                    else
                    {
                        txtValue1MO.Properties.Mask.UseMaskAsDisplayFormat = true;
                        txtValue1MO.EditValue = Math.Round(Position.MO_Multi1, 2, MidpointRounding.AwayFromZero);
                    }

                    CalculateGrundMultiMO();
                    CalculateEinkuafpreisMO();

                    if (Convert.ToBoolean(ChkManualMontageentry.EditValue) == true)
                    {
                        decimal HourlyRate = getDValue(txtStdSatz.Text);
                        decimal factor = getDValue(txtFaktor.Text);
                        if (HourlyRate != 0 && factor != 0)
                        {
                            txtHours.Text = Convert.ToString(Math.Round((Position.MO_ListPrice / HourlyRate) / factor, 2, MidpointRounding.AwayFromZero));
                            txtMin.Text = Convert.ToString(Math.Round(getDValue(txtHours.Text) * 60, 2));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtValue1MO_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (_IsNewMode || !Position.HaveDetailsKz)
                {
                    Position.MO_Multi2 = Math.Round(GetValue(Position.MO_ListPrice +
                    Position.MO_Multi1, getDValue(txtMulti2MO.Text)), 8);
                    if (Position.MO_Multi2 == 0)
                    {
                        txtValue2MO.Properties.Mask.UseMaskAsDisplayFormat = false;
                        txtValue2MO.Text = string.Empty;
                    }
                    else
                    {
                        txtValue2MO.Properties.Mask.UseMaskAsDisplayFormat = true;
                        txtValue2MO.EditValue = Math.Round(Position.MO_Multi2, 2, MidpointRounding.AwayFromZero);
                    }
                    CalculateGrundMultiMO();
                    CalculateGrundValueMO();
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtValue2MO_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (_IsNewMode || !Position.HaveDetailsKz)
                {
                    Position.MO_Multi3 = Math.Round(GetValue(Position.MO_ListPrice + Position.MO_Multi1 +
                    Position.MO_Multi2, getDValue(txtMulti3MO.Text)), 8);
                    if (Position.MO_Multi3 == 0)
                    {
                        txtValue3MO.Properties.Mask.UseMaskAsDisplayFormat = false;
                        txtValue3MO.Text = string.Empty;
                    }
                    else
                    {
                        txtValue3MO.Properties.Mask.UseMaskAsDisplayFormat = true;
                        txtValue3MO.EditValue = Math.Round(Position.MO_Multi3, 2, MidpointRounding.AwayFromZero);
                    }
                    CalculateGrundMultiMO();
                    CalculateGrundValueMO();
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtValue3MO_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (_IsNewMode || !Position.HaveDetailsKz)
                {
                    Position.MO_Multi4 = Math.Round(GetValue(Position.MO_ListPrice +
                    Position.MO_Multi1 + Position.MO_Multi2 + Position.MO_Multi3, getDValue(txtMulti4MO.Text)), 8);
                    if (Position.MO_Multi4 == 0)
                    {
                        txtValue4MO.Properties.Mask.UseMaskAsDisplayFormat = false;
                        txtValue4MO.Text = string.Empty;
                    }
                    else
                    {
                        txtValue4MO.Properties.Mask.UseMaskAsDisplayFormat = true;
                        txtValue4MO.EditValue = Math.Round(Position.MO_Multi4, 2, MidpointRounding.AwayFromZero);
                    }
                    CalculateGrundMultiMO();
                    CalculateGrundValueMO();
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtValue4MO_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (_IsNewMode || !Position.HaveDetailsKz)
                {
                    CalculateGrundMultiMO();
                    CalculateGrundValueMO();
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtEinkaufspreisMO_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (_IsNewMode || !Position.HaveDetailsKz)
                {
                    Position.MO_SK = Math.Round(Position.MO_EP + GetValue(Position.MO_EP,
                        getDValue(txtSelbstkostenMultiMO.Text)), 8);
                    if (Position.MO_SK != 0)
                    {
                        txtSelbstkostenValueMO.Properties.Mask.UseMaskAsDisplayFormat = true;
                        txtSelbstkostenValueMO.EditValue = Math.Round(Position.MO_SK, 2, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        txtSelbstkostenValueMO.Properties.Mask.UseMaskAsDisplayFormat = false;
                        txtSelbstkostenValueMO.Text = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtSelbstkostenValueMO_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (_IsNewMode || !Position.HaveDetailsKz)
                {
                    if (IsFire)
                    {
                        Position.MO_VK = Math.Round(Position.MO_SK + GetValue(Position.MO_SK,
                            getDValue(txtVerkaufspreisMultiMO.Text)), 8);
                        if (Position.MO_VK != 0)
                        {
                            txtVerkaufspreisValueMO.Properties.Mask.UseMaskAsDisplayFormat = true;
                            txtVerkaufspreisValueMO.EditValue = Math.Round(Position.MO_VK, 2, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            txtVerkaufspreisValueMO.Properties.Mask.UseMaskAsDisplayFormat = false;
                            txtVerkaufspreisValueMO.Text = string.Empty;
                        }
                    }
                    else
                    {
                        IsFire = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtGrandTotalME_TextChanged(object sender, EventArgs e)
        {
            try
            {

                CalculateEP();
                decimal GB = RoundValue((getDValue(txtGrandTotalME.Text) * getDValue(txtMenge.Text)))
                    + RoundValue(getDValue(txtGrandTotalMO.Text) * getDValue(txtMenge.Text));
                if (GB != 0)
                {
                    txtFinalGB.Properties.Mask.UseMaskAsDisplayFormat = true;
                    txtFinalGB.EditValue = GB;
                }
                else
                {
                    txtFinalGB.Properties.Mask.UseMaskAsDisplayFormat = false;
                    txtFinalGB.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtMin_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Position.HaveDetailsKz)
                    txtHours.EditValue = 0;
                if (_IsNewMode || !Position.HaveDetailsKz)
                {
                    if (Convert.ToBoolean(ChkManualMontageentry.EditValue) == false)
                    {
                        txtHours.Text = Convert.ToString(Math.Round(getDValue(txtMin.Text)
                            / Convert.ToDecimal(60), 8));
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtStdSatz_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (_IsNewMode || !Position.HaveDetailsKz)
                {
                    if (Convert.ToBoolean(ChkManualMontageentry.EditValue) == false)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(txtHours.EditValue))
                            && !string.IsNullOrEmpty(Convert.ToString(txtFaktor.EditValue))
                            && !string.IsNullOrEmpty(Convert.ToString(txtStdSatz.EditValue)) && tlPositions.FocusedNode != null)
                        {
                            Position.MO_ListPrice = Math.Round(
                                getDValue(Convert.ToString(txtHours.EditValue)) *
                                getDValue(txtFaktor.Text) *
                                getDValue(txtStdSatz.Text), 8);
                            if (Position.MO_ListPrice != 0)
                            {
                                txtLPMO.Properties.Mask.UseMaskAsDisplayFormat = true;
                                txtLPMO.EditValue = Math.Round(Position.MO_ListPrice, 2, MidpointRounding.AwayFromZero);
                            }
                            else
                            {
                                txtLPMO.Properties.Mask.UseMaskAsDisplayFormat = false;
                                txtLPMO.Text = string.Empty;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtGrundMultiME1_TextChanged(object sender, EventArgs e)
        {
            CalculateEinkuafpreisME();
        }

        private void txtGrundMultiMO1_TextChanged(object sender, EventArgs e)
        {
            CalculateEinkuafpreisMO();
        }

        private void CalculateGrundMultiME()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtMulti1ME.Text) &&
                                                !string.IsNullOrEmpty(txtMulti2ME.Text) &&
                                                !string.IsNullOrEmpty(txtMulti3ME.Text) &&
                                                !string.IsNullOrEmpty(txtMulti4ME.Text)
                                                )
                {
                    decimal GrundMulti = Math.Round(
                        getDValue(txtMulti1ME.Text) *
                        getDValue(txtMulti2ME.Text) *
                        getDValue(txtMulti3ME.Text) *
                        getDValue(txtMulti4ME.Text), 3, MidpointRounding.AwayFromZero);
                    txtGrundMultiME1.Text = txtGrundMultiME.Text = GrundMulti.ToString();
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void CalculateGrundValueME()
        {
            try
            {
                Position.MA_GrundMulti = Math.Round(Position.MA_Multi1 +
                    Position.MA_Multi2 +
                    Position.MA_Multi3 +
                    Position.MA_Multi4, 8);
                if (Position.MA_GrundMulti == 0)
                {
                    txtGrundValueME.Properties.Mask.UseMaskAsDisplayFormat = false;
                    txtGrundValueME.Text = string.Empty;
                }
                else
                {
                    txtGrundValueME.Properties.Mask.UseMaskAsDisplayFormat = true;
                    txtGrundValueME.EditValue = Math.Round(Position.MA_GrundMulti, 2, MidpointRounding.AwayFromZero);
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void CalculateGrundMultiMO()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtMulti1MO.Text) &&
                                !string.IsNullOrEmpty(txtMulti2MO.Text) &&
                                !string.IsNullOrEmpty(txtMulti3MO.Text) &&
                                !string.IsNullOrEmpty(txtMulti4MO.Text)
                                )
                {
                    decimal GrundMulti = Math.Round(
                        getDValue(txtMulti1MO.Text) *
                        getDValue(txtMulti2MO.Text) *
                        getDValue(txtMulti3MO.Text) *
                        getDValue(txtMulti4MO.Text), 3, MidpointRounding.AwayFromZero);
                    txtGrundMultiMO1.Text = txtGrundMultiMO.Text = GrundMulti.ToString();
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void CalculateGrundValueMO()
        {
            try
            {
                Position.MO_GrundMulti = Math.Round(
                    Position.MO_Multi1 +
                    Position.MO_Multi2 +
                    Position.MO_Multi3 +
                    Position.MO_Multi4, 8);
                if (Position.MO_GrundMulti == 0)
                {
                    txtGrundValueMO.Properties.Mask.UseMaskAsDisplayFormat = false;
                    txtGrundValueMO.Text = string.Empty;
                }
                else
                {
                    txtGrundValueMO.Properties.Mask.UseMaskAsDisplayFormat = true;
                    txtGrundValueMO.EditValue = Math.Round(Position.MO_GrundMulti, 2, MidpointRounding.AwayFromZero);
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void CalculateEinkuafpreisME()
        {
            try
            {
                if (_IsNewMode || !Position.HaveDetailsKz)
                {
                    decimal dValue = 0;
                    decimal M1 = 0;
                    decimal M2 = 0;
                    decimal M3 = 0;
                    decimal M4 = 0;

                    decimal.TryParse(txtLPMe.Text, out dValue);

                    if (decimal.TryParse(txtMulti1ME.Text, out M1))
                        dValue = dValue * M1;
                    if (decimal.TryParse(txtMulti2ME.Text, out M2))
                        dValue = dValue * M2;
                    if (decimal.TryParse(txtMulti3ME.Text, out M3))
                        dValue = dValue * M3;
                    if (decimal.TryParse(txtMulti4ME.Text, out M4))
                        dValue = dValue * M4;

                    Position.MA_EP = Math.Round(dValue, 8);
                    if (Position.MA_EP != 0)
                    {
                        txtEinkaufspreisME.Properties.Mask.UseMaskAsDisplayFormat = true;
                        txtEinkaufspreisME.EditValue = Math.Round(Position.MA_EP, 2, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        txtEinkaufspreisME.Properties.Mask.UseMaskAsDisplayFormat = false;
                        txtEinkaufspreisME.Text = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void CalculateEinkuafpreisMO()
        {
            try
            {
                if (_IsNewMode || !Position.HaveDetailsKz)
                {

                    decimal dValue = 0;
                    decimal M1 = 0;
                    decimal M2 = 0;
                    decimal M3 = 0;
                    decimal M4 = 0;

                    decimal.TryParse(txtLPMO.Text, out dValue);

                    if (decimal.TryParse(txtMulti1MO.Text, out M1))
                        dValue = dValue * M1;
                    if (decimal.TryParse(txtMulti2MO.Text, out M2))
                        dValue = dValue * M2;
                    if (decimal.TryParse(txtMulti3MO.Text, out M3))
                        dValue = dValue * M3;
                    if (decimal.TryParse(txtMulti4MO.Text, out M4))
                        dValue = dValue * M4;

                    Position.MO_EP = Math.Round(dValue, 8);
                    if (Position.MO_EP != 0)
                    {
                        txtEinkaufspreisMO.Properties.Mask.UseMaskAsDisplayFormat = true;
                        txtEinkaufspreisMO.EditValue = Math.Round(Position.MO_EP, 2, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        txtEinkaufspreisMO.Properties.Mask.UseMaskAsDisplayFormat = false;
                        txtEinkaufspreisMO.Text = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private decimal GetValue(decimal dValue, decimal dMulti)
        {
            decimal dreturnvalue = 0;
            try
            {
                dreturnvalue = (dMulti - 1) * dValue;

            }
            catch (Exception ex)
            {
                throw;
            }
            return dreturnvalue;
        }

        private decimal getDValue(string strValue)
        {
            decimal dValue = 0;
            try
            {
                if (!decimal.TryParse(strValue, out dValue)) { }
            }
            catch (Exception ex) { }
            return dValue;
        }

        private decimal RoundValue(decimal dValue)
        {
            try
            {
                return Math.Round(dValue, ObjEProject.RoundingPrice, MidpointRounding.AwayFromZero);
            }
            catch (Exception ex)
            {
                if (Utility._IsGermany == true)
                {
                    throw new Exception("Fehler bei der Rundungsoperation");
                }
                else
                {
                    throw new Exception("Error in Rounding the Value");
                }
            }
        }

        private void CalculateEP()
        {
            try
            {
                decimal EP = getDValue(txtGrandTotalME.Text) +
                   getDValue(txtGrandTotalMO.Text);
                if (EP != 0)
                {
                    txtEP.Properties.Mask.UseMaskAsDisplayFormat = true;
                    txtEP.EditValue = EP;
                }
                else
                {
                    txtEP.Properties.Mask.UseMaskAsDisplayFormat = false;
                    txtEP.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToBoolean(chkCreateNew.EditValue) == false)
                {
                    Color _Color = Color.FromArgb(255, 135, 0);
                    tlPositions.Appearance.HeaderPanel.BackColor = _Color;
                    LCGLVDetails.Root.AppearanceGroup.BackColor = _Color;
                    btnAddAccessories.Enabled = true;
                }
                if (Utility.LVSectionEditAccess == "7")
                {
                    if (Utility.LVDetailsAccess != "7")
                    {
                        LCGLVDetails.Enabled = true;
                        btnNew.Enabled = true;
                        chkCreateNew.Enabled = true;
                    }
                    if (Utility.CalcAccess != "7")
                    {
                        lcCostDetails.Enabled = true;
                        tlPositions.OptionsBehavior.Editable = true;
                    }
                    btnSaveLVDetails.Enabled = true;
                    btnCancel.Enabled = true;
                }
                _IsAddhoc = false;
                CreateNewPosition();
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {

            try
            {
                int iValue = 0;
                if (tlPositions.FocusedNode != null
                    && tlPositions.FocusedNode["PositionID"] != null
                    && Int32.TryParse(tlPositions.FocusedNode["PositionID"].ToString(), out iValue)
                    )
                {
                    ObjEPosition.PositionID = iValue;
                    if (string.IsNullOrEmpty(ObjEProject.CommissionNumber))
                        cmbLVSection.Enabled = false;
                }
                else if (_IsEditMode)
                {
                    if (Utility._IsGermany == true)
                        XtraMessageBox.Show("Bitte speichern Sie die Änderungen", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                    {
                        XtraMessageBox.Show("Please save the changes", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            EnableDisableLVAndCostDetails(true, true, false, true);
            EnableDisableButtons(false, true, true, true, true);
        }

        private void txtLPMe_Leave(object sender, EventArgs e)
        {
            try
            {
                TextEdit textbox = (TextEdit)sender;
                decimal dValue = 0;
                if (textbox.Text == string.Empty || decimal.TryParse(textbox.Text, out dValue))
                {
                    if (dValue == 0)
                    {
                        textbox.Text = RoundValue(1).ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtMo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtMo.Text.ToLower() == "s")
                    txtStdSatz.Text = RoundValue(ObjEProject.InternS).ToString();
                else if (txtMo.Text.ToLower() == "x")
                    txtStdSatz.Text = RoundValue(ObjEProject.InternX).ToString();
                else
                    txtStdSatz.Text = string.Empty;
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtMa_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar.ToString().ToLower() == "s" || e.KeyChar.ToString().ToLower() == "x" || e.KeyChar == '\b')
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (tcProjectDetails.SelectedTabPage == tbLVDetails)
                {
                    EnableDisableLVAndCostDetails(false, false, false, false);
                    EnableDisableButtons(true, true, false, true, true);
                    _IsEditMode = false;
                    chkCreateNew.EditValue = false;
                    tlPositions_FocusedNodeChanged(null, null);
                    tlPositions.OptionsBehavior.ReadOnly = false;
                    Color _Color = Color.FromArgb(0, 158, 224);
                    tlPositions.Appearance.HeaderPanel.BackColor = _Color;
                    LCGLVDetails.Root.AppearanceGroup.BackColor = _Color;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtSurchargeFrom_Leave(object sender, EventArgs e)
        {
            TextEdit textbox = (TextEdit)sender;
            try
            {
                string stPosKZ = Utility.GetPosKZ(cmbPositionKZ.Text);
                DataView dvPosition = ObjEPosition.dsPositionList.Tables[0].DefaultView;
                if (stPosKZ == "Z")
                    dvPosition.RowFilter = "Position_OZ = '" + textbox.Text + "' AND (PositionKZ IN ('N','M','P','K','W','S','B','G','Z'))";
                else if (stPosKZ == "ZS")
                    dvPosition.RowFilter = "Position_OZ = '" + textbox.Text + "' AND (PositionKZ IN ('N','M','P','K','W','S','B','G','Z','E','A'))";
                else if (stPosKZ == "ZZ")
                {
                    if (textbox == txtSurchargeFrom)
                    {
                        txtSurchargeFrom.Text = textbox.Text.Replace(',', '.');
                    }
                    else
                    {
                        txtSurchargeTo.Text = textbox.Text.Replace(',', '.');
                    }
                }


                DataTable dtTemp = dvPosition.ToTable();
                if (dtTemp != null && dtTemp.Rows.Count < 1)
                {
                    if (textbox.Text == string.Empty)
                    {
                        if (Utility._IsGermany == true)
                            XtraMessageBox.Show(textbox.Tag.ToString() + " Fehlende Angabe", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else
                            XtraMessageBox.Show(textbox.Tag.ToString() + " Should not be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        if (Utility._IsGermany == true)
                            XtraMessageBox.Show("Die gewählte " + textbox.Tag.ToString() + " existiert nicht", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else
                            XtraMessageBox.Show("Selected " + textbox.Tag.ToString() + " does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    if (_IsNewMode)
                    {

                        if (textbox == txtSurchargeFrom)
                            textbox.Text = FromOZ(PrepareOZ(), stPosKZ);
                        else
                            textbox.Text = ToOZ(PrepareOZ(), stPosKZ);
                    }
                    else
                    {
                        tlPositions_FocusedNodeChanged(null, null);
                    }
                    textbox.Select(0, 0);
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnDocuwareLink_Click(object sender, EventArgs e)
        {
            try
            {
                frmDocuwareLink Obj = new frmDocuwareLink();
                Obj.DocuwareLink1 = _DocuwareLink1;
                Obj.DocuwareLink2 = _DocuwareLink2;
                Obj.DocuwareLink3 = _DocuwareLink3;
                Obj.ShowDialog();
                _DocuwareLink1 = Obj.DocuwareLink1;
                _DocuwareLink2 = Obj.DocuwareLink2;
                _DocuwareLink3 = Obj.DocuwareLink3;
            }
            catch (Exception ex) { }
        }

        private void btnAddPositionKZ_Click(object sender, EventArgs e)
        {
            try
            {
                string strLVSecction = cmbLVSection.Text;
                if (strLVSecction.ToLower() == "nt" || strLVSecction.ToLower() == "ntm")
                {
                    frmTextDialog Obj = new frmTextDialog();
                    Obj.ShowInTaskbar = false;
                    Obj.NewLVSection = strLVSecction + ObjBPosition.GetNewLVSection(cmbLVSection.Text, ObjEProject.ProjectID);
                    Obj.ShowDialog();
                    DataRow[] tPosition_Id = ObjEProject.dtLVSection.Select("LVSectionName='" + Obj.NewLVSection + "'");
                    if (tPosition_Id.Count() > 0)
                    {
                        if (Utility._IsGermany == true)
                        {
                            throw new Exception("Die gewählte LV Sektion existiert bereits");
                        }
                        else
                        {
                            throw new Exception("Selected LV Section Already Exist");
                        }

                    }
                    else if (!string.IsNullOrEmpty(Obj.NewLVSection.Trim()) && Obj.IsSave)
                    {
                        ObjBPosition.InsertNewLVSection(Obj.NewLVSection, ObjEProject.ProjectID, ObjEProject);
                        if (ObjEProject.dtLVSection != null && ObjEProject.dtLVSection.Rows.Count > 0)
                        {
                            cmbLVSection.Properties.DataSource = ObjEProject.dtLVSection;
                            cmbLVSection.Properties.DisplayMember = "LVSectionName";
                            cmbLVSection.Properties.ValueMember = "LVSectionID";
                        }
                        cmbLVSection.EditValue = ObjEProject.LVSectionID;
                    }
                }
                else
                {
                    if (Utility._IsGermany == true)
                        throw new Exception("Bitte wählen Sie NT oder NTM");
                    else
                    {
                        throw new Exception("Please Select NT or NTM");
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtVerkaufspreisValueME_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Position.MA_VK != 0)
                {
                    txtGrandTotalME.Properties.Mask.UseMaskAsDisplayFormat = true;
                    txtGrandTotalME.EditValue = RoundValue(Position.MA_VK);
                }
                else
                {
                    txtGrandTotalME.Properties.Mask.UseMaskAsDisplayFormat = false;
                    txtGrandTotalME.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtVerkaufspreisValueMO_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Position.MO_VK != 0)
                {
                    txtGrandTotalMO.Properties.Mask.UseMaskAsDisplayFormat = true;
                    txtGrandTotalMO.EditValue = RoundValue(Position.MO_VK);
                }
                else
                {
                    txtGrandTotalMO.Properties.Mask.UseMaskAsDisplayFormat = false;
                    txtGrandTotalMO.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);

            }
        }

        private void txtMenge_TextChanged(object sender, EventArgs e)
        {
            txtGrandTotalME_TextChanged(null, null);
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                if (Utility.AutoSave)
                {
                    string stPosKZ = Utility.GetPosKZ(cmbPositionKZ.Text);
                    if (stPosKZ == "ZZ")
                    {
                        tlPositions.MovePrev();
                        return;
                    }
                    btnModify_Click(null, null);
                    if (_IsEditMode)
                    {
                        if (!ObjEProject.IsFinalInvoice)
                            btnSaveLVDetails_Click(null, null);
                        tlPositions.MovePrev();
                        btnModify_Click(null, null);
                    }
                    else if (!_IsNewMode)
                    {
                        tlPositions.MovePrev();
                    }
                }
                else
                {
                    if (!_IsNewMode)
                    {
                        tlPositions.MovePrev();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (Utility.AutoSave)
                {
                    string stPosKZ = Utility.GetPosKZ(cmbPositionKZ.Text);
                    if (stPosKZ == "ZZ")
                    {
                        tlPositions.MoveNext();
                        return;
                    }
                    btnModify_Click(null, null);
                    if (_IsEditMode)
                    {
                        if (!ObjEProject.IsFinalInvoice)
                            btnSaveLVDetails_Click(null, null);
                        tlPositions.MoveNext();
                        btnModify_Click(null, null);
                    }
                    else if (!_IsNewMode)
                    {
                        tlPositions.MoveNext();
                    }
                }
                else
                {
                    if (!_IsNewMode)
                    {
                        tlPositions.MoveNext();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void txtMulti1ME_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != ','))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void cmbLVSection_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtValue1ME_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                TextEdit textbox = (TextEdit)sender;
                decimal dValue = 0;
                if (decimal.TryParse(Convert.ToString(textbox.Text), out dValue))
                {
                    if (dValue < 0)
                        textbox.ForeColor = Color.Red;
                    else
                        textbox.ForeColor = Color.Black;
                }
            }
            catch (Exception) { }
        }

        private void txtMa_TextChanged(object sender, EventArgs e)
        {
            if (txtMa.Text.ToLower() == "s")
            {
                txtMo.Text = "S";
                txtMo.Enabled = false;
            }
            else
            {
                txtMo.Enabled = true;
            }
        }

        private void txtStufe1Short_Leave(object sender, EventArgs e)
        {
            try
            {
                TextEdit textbox = (TextEdit)sender;
                if (textbox.Text == "0")
                {
                    textbox.Text = "";
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void tlPositions_CellValueChanged(object sender, DevExpress.XtraTreeList.CellValueChangedEventArgs e)
        {
            try
            {
                if (!_ISChange)
                    return;
                if (tlPositions.FocusedNode != null && tlPositions.FocusedNode["PositionID"] != null)
                {
                    string strColumnName = e.Column.FieldName;
                    switch (strColumnName)
                    {
                        case "MA_Multi1":
                            txtMulti1ME.Text = Convert.ToString(tlPositions.FocusedNode.GetValue("MA_Multi1"));
                            break;
                        case "MA_multi2":
                            txtMulti2ME.Text = Convert.ToString(tlPositions.FocusedNode.GetValue("MA_multi2"));
                            break;
                        case "MA_multi3":
                            txtMulti3ME.Text = Convert.ToString(tlPositions.FocusedNode.GetValue("MA_multi3"));
                            break;
                        case "MA_multi4":
                            txtMulti4ME.Text = Convert.ToString(tlPositions.FocusedNode.GetValue("MA_multi4"));
                            break;
                        case "MA_selbstkostenMulti":
                            txtSelbstkostenMultiME.Text = Convert.ToString(tlPositions.FocusedNode.GetValue("MA_selbstkostenMulti"));
                            break;
                        case "MA_verkaufspreis_Multi":
                            txtVerkaufspreisMultiME.Text = Convert.ToString(tlPositions.FocusedNode.GetValue("MA_verkaufspreis_Multi"));
                            break;
                        case "MO_multi1":
                            txtMulti1MO.Text = Convert.ToString(tlPositions.FocusedNode.GetValue("MO_multi1"));
                            break;
                        case "MO_multi2":
                            txtMulti2MO.Text = Convert.ToString(tlPositions.FocusedNode.GetValue("MO_multi2"));
                            break;
                        case "MO_multi3":
                            txtMulti3MO.Text = Convert.ToString(tlPositions.FocusedNode.GetValue("MO_multi3"));
                            break;
                        case "MO_multi4":
                            txtMulti4MO.Text = Convert.ToString(tlPositions.FocusedNode.GetValue("MO_multi4"));
                            break;
                        case "MO_selbstkostenMulti":
                            txtSelbstkostenMultiMO.Text = Convert.ToString(tlPositions.FocusedNode.GetValue("MO_selbstkostenMulti"));
                            break;
                        case "MO_verkaufspreisMulti":
                            txtVerkaufspreisMultiMO.Text = Convert.ToString(tlPositions.FocusedNode.GetValue("MO_verkaufspreisMulti"));
                            break;
                        case "WG":
                            _WGWAChanged = true;
                            txtWG.Text = Convert.ToString(tlPositions.FocusedNode.GetValue("WG"));
                            WGChanged = true;
                            txtWI_Leave(null, null);
                            break;
                        case "WA":
                            _WGWAChanged = true;
                            txtWA.Text = Convert.ToString(tlPositions.FocusedNode.GetValue("WA"));
                            WAChanged = true;
                            txtWI_Leave(null, null);
                            break;
                        case "WI":
                            txtWI.Text = Convert.ToString(tlPositions.FocusedNode.GetValue("WI"));
                            txtWI_Leave(null, null);
                            break;
                        case "Menge":
                            txtMenge.Text = Convert.ToString(tlPositions.FocusedNode.GetValue("Menge"));
                            break;
                        case "ME":
                            cmbCDME.Text = Convert.ToString(tlPositions.FocusedNode.GetValue("ME"));
                            break;
                        case "ShortDescription":
                            txtShortDescriptionCD.Text = Utility.GetPlaintext(Convert.ToString(tlPositions.FocusedNode.GetValue("ShortDescription")));
                            break;
                        case "MINUTES":
                            txtMin.EditValue = Convert.ToString(tlPositions.FocusedNode.GetValue("MINUTES"));
                            break;
                        default:
                            break;
                    }
                    if (WGChanged)
                    {
                        WGChanged = false;
                        string stwa = Convert.ToString(tlPositions.FocusedNode.GetValue("WA"));
                        if (string.IsNullOrEmpty(stwa))
                            return;
                    }
                    if (WAChanged)
                    {
                        WAChanged = false;
                        string stwG = Convert.ToString(tlPositions.FocusedNode.GetValue("WG"));
                        if (string.IsNullOrEmpty(stwG))
                            return;
                    }
                    string strcolumnName = tlPositions.FocusedColumn.FieldName;
                    btnSaveLVDetails_Click(null, null);
                    tlPositions.FocusedColumn = tlPositions.Columns[strcolumnName];
                }
            }
            catch (Exception ex) { Utility.ShowError(ex); }
        }

        private void chkCreateNew_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToBoolean(chkCreateNew.EditValue) == true)
                {
                    if (!_IsNewMode)
                        btnNew_Click(null, null);
                    tlPositions.OptionsBehavior.ReadOnly = true;
                    Color _Color = Color.FromArgb(255, 183, 0);
                    LCGLVDetails.Root.AppearanceGroup.BackColor = _Color;
                    tlPositions.Appearance.HeaderPanel.BackColor = _Color;
                    btnAddAccessories.Enabled = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void chkRaster_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ChkRaster.Checked == true)
                {
                    ddlRaster.SelectedIndex = ddlRaster.Properties.Items.IndexOf("");
                    ddlRaster.Enabled = false;
                }
                else
                {
                    ddlRaster.Enabled = true;
                    ddlRaster.SelectedIndex = ddlRaster.Properties.Items.IndexOf("99.99.1111.9");
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void tlPositions_PopupMenuShowing(object sender, DevExpress.XtraTreeList.PopupMenuShowingEventArgs e)
        {
            try
            {
                if (ObjEProject.IsFinalInvoice && Utility.LVDetailsAccess == "7")
                    return;

                TreeListHitInfo hitInfo = (sender as TreeList).CalcHitInfo(e.Point);
                if (hitInfo.HitInfoType == HitInfoType.Column || hitInfo.HitInfoType == HitInfoType.BehindColumn)
                    return;

                tlPositions.FocusedNode = ((TreeListNodeMenu)e.Menu).Node;
                string P_value = tlPositions.FocusedNode["PositionKZ"].ToString();
                int _DetailKZ = Convert.ToInt32(tlPositions.FocusedNode["DetailKZ"]);
                if (P_value == "NG" || P_value == "Z" || P_value == "ZS")
                {
                    if (e.Menu is TreeListNodeMenu)
                    {
                        if (P_value == "Z" || P_value == "ZS")
                            e.Menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("Löschen", bbDelete_ItemClick));
                        else if (tlPositions.FocusedNode.Nodes.Count <= 0)
                            e.Menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("Löschen", bbDelete_ItemClick));

                        e.Menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("Textposition hinzufügen", bbAddTextPosition_Click));
                    }
                }
                else if (P_value == "H" || P_value == "VR" || P_value == "AB" || P_value == "BA" || P_value == "UB")
                    e.Menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("Löschen", bbDelete_ItemClick));
                else if (P_value != "ZZ")
                {
                    if (e.Menu is TreeListNodeMenu)
                    {
                        e.Menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("Löschen", bbDelete_ItemClick));
                        e.Menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("Textposition hinzufügen", bbAddTextPosition_Click));
                        e.Menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("Detail KZ hinzufügen", bbAddDetailKZ_Click));
                        e.Menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("Kopieren LV Position", bbCopyLVAndDetailKZ_Click));
                    }
                }
                if (_DetailKZ > 0)
                {
                    e.Menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("Kopieren Detail KZ", bbCopyDetailKZ_Click));
                }
                if (_ISCopied == true && P_value != "NG" && P_value != "Z" && P_value != "ZS" && P_value != "H" && P_value != "AB" && P_value != "BA" && P_value != "UB" && P_value != "VR")
                {
                    e.Menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("Einfügen Detail KZ", bbPasteyDetailKZ_Click));
                }
                if (_ISCopiedLV == true && P_value != "NG" && P_value != "Z" && P_value != "ZS" && P_value != "H" && P_value != "AB" && P_value != "BA" && P_value != "UB" && P_value != "VR")
                {
                    e.Menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("Einfügen LV Position(Unter der ausgewählten Zeile)", bbPasteLVAndDetailKZ_Click));
                }
            }
            catch (Exception ex) { }
        }

        private void bbCopyLVAndDetailKZ_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow dataRow = (tlPositions.GetDataRecordByNode(tlPositions.FocusedNode) as DataRowView).Row;

                string strPositionOZ = Convert.ToString(dataRow["Position_OZ"]);
                int _PID = Convert.ToInt32(dataRow["PositionID"]);

                _dtCopyLVAndDetailKZ = ObjEPosition.dsPositionList.Tables[0].Copy();
                DataView dvTemp = _dtCopyLVAndDetailKZ.DefaultView;
                dvTemp.RowFilter = "Position_OZ = '" + strPositionOZ + "'";
                ObjEPosition.dtCopyPosition = dvTemp.ToTable();
                _ISCopiedLV = true;
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void bbCopyDetailKZ_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow dataRow = (tlPositions.GetDataRecordByNode(tlPositions.FocusedNode) as DataRowView).Row;

                string strPositionOZ = Convert.ToString(dataRow["Position_OZ"]);
                int _PID = Convert.ToInt32(dataRow["PositionID"]);

                _dtCopyDetailKZ = ObjEPosition.dsPositionList.Tables[0].Copy();
                DataView dvTemp = _dtCopyDetailKZ.DefaultView;
                dvTemp.RowFilter = "Position_OZ = '" + strPositionOZ + "' AND PositionID ='" + _PID + "'";
                ObjEPosition.dtCopyPosition = dvTemp.ToTable();
                _ISCopied = true;
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void bbPasteyDetailKZ_Click(object sender, EventArgs e)
        {
            try
            {
                if (tlPositions.FocusedNode["Position_OZ"] != null)
                {
                    string strPositionOZ = Convert.ToString(tlPositions.FocusedNode["Position_OZ"]);
                    string strParentOZ = Convert.ToString(tlPositions.FocusedNode.ParentNode["Position_OZ"]);
                    if (ObjEPosition.dsPositionList != null && ObjEPosition.dsPositionList.Tables.Count > 0)
                    {
                        object objDetailKZ = ObjEPosition.dsPositionList.Tables[0].Compute("MAX(DetailKZ)", "Position_OZ ='" + strPositionOZ + "'");
                        object ObjSNO = ObjEPosition.dsPositionList.Tables[0].Compute("MAX(SNO)", "Position_OZ ='" + strPositionOZ + "'");
                        int iValue = 0;
                        int iSNOValue = 0;
                        ObjEPosition.RoundOffValue = ObjEProject.RoundingPrice;
                        if (objDetailKZ != null && ObjSNO != null && int.TryParse(Convert.ToString(objDetailKZ), out iValue) && int.TryParse(Convert.ToString(ObjSNO), out iSNOValue))
                        {
                            _ISChange = false;
                            if (ObjEPosition.dtCopyPosition.Rows.Count > 1)
                            {
                                foreach (DataRow dr in ObjEPosition.dtCopyPosition.Rows)
                                {
                                    ParseLVAndDetailKZCopyLV(dr, strPositionOZ, strParentOZ, iSNOValue, string.Empty, true, iValue + 1);
                                    ObjBPosition.SavePositionDetails(ObjEPosition, ObjEProject.LVRaster, ObjEProject.LVSprunge, true);
                                }
                                BindPositionData();
                                SetFocus(ObjEPosition.PositionID, tlPositions);
                            }
                            else
                            {
                                ParseLVAndDetailKZCopyLV(ObjEPosition.dtCopyPosition.Rows[0], strPositionOZ, strParentOZ, iSNOValue, string.Empty, true, iValue + 1);
                                ObjBPosition.SavePositionDetails(ObjEPosition, ObjEProject.LVRaster, ObjEProject.LVSprunge, true);
                                _IsNewMode = true;
                                RefreshTreelist(ObjEPosition);
                                _IsNewMode = false;
                            }
                            _ISChange = true;
                        }
                    }
                    _ISCopied = false;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void bbPasteLVAndDetailKZ_Click(object sender, EventArgs e)
        {
            try
            {
                if (tlPositions.FocusedNode["Position_OZ"] != null)
                {
                    string strPositionOZ = Convert.ToString(tlPositions.FocusedNode["Position_OZ"]);
                    string strParentOZ = Convert.ToString(tlPositions.FocusedNode.ParentNode["Position_OZ"]);
                    if (ObjEPosition.dsPositionList != null && ObjEPosition.dsPositionList.Tables.Count > 0)
                    {
                        object ObjSNO = ObjEPosition.dsPositionList.Tables[0].Compute("MAX(SNO)", "Position_OZ ='" + strPositionOZ + "'");
                        int iSNOValue = 0;
                        if (ObjSNO != null && int.TryParse(Convert.ToString(ObjSNO), out iSNOValue))
                        {
                            _ISChange = false;
                            ObjEPosition.RoundOffValue = ObjEProject.RoundingPrice;
                            if (ObjEPosition.dtCopyPosition.Rows.Count > 1)
                            {
                                foreach (DataRow dr in ObjEPosition.dtCopyPosition.Rows)
                                {
                                    ParseLVAndDetailKZCopyLV(dr, strPositionOZ, strParentOZ, iSNOValue, string.Empty, false);
                                    if (string.IsNullOrEmpty(ObjEPosition.Position_OZ))
                                        return;
                                    ObjBPosition.SavePositionDetails(ObjEPosition, ObjEProject.LVRaster, ObjEProject.LVSprunge, true);
                                    if (iSNOValue != -1)
                                        iSNOValue++;
                                }
                                BindPositionData();
                                SetFocus(ObjEPosition.PositionID, tlPositions);
                            }
                            else
                            {
                                ParseLVAndDetailKZCopyLV(ObjEPosition.dtCopyPosition.Rows[0], strPositionOZ,
                                    strParentOZ, iSNOValue, string.Empty, false);
                                if (string.IsNullOrEmpty(ObjEPosition.Position_OZ))
                                    return;
                                ObjBPosition.SavePositionDetails(ObjEPosition, ObjEProject.LVRaster, ObjEProject.LVSprunge, true);
                                _IsNewMode = true;
                                RefreshTreelist(ObjEPosition);
                                _IsNewMode = false;
                            }
                            _ISChange = true;
                        }
                    }
                    _ISCopiedLV = false;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void bbDelete_ItemClick(object sender, EventArgs e)
        {
            try
            {
                DeletePosition();
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void bbAddTextPosition_Click(object sender, EventArgs e)
        {
            try
            {
                CreateNewPosition("h");
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void bbAddDetailKZ_Click(object sender, EventArgs e)
        {
            try
            {
                if (tlPositions.FocusedNode["Position_OZ"] != null)
                {
                    string strPositionOZ = Convert.ToString(tlPositions.FocusedNode["Position_OZ"]);
                    string strParentOZ = Convert.ToString(tlPositions.FocusedNode.ParentNode["Position_OZ"]);
                    if (ObjEPosition.dsPositionList != null && ObjEPosition.dsPositionList.Tables.Count > 0)
                    {
                        _IsNewMode = true;
                        object objDetailKZ = ObjEPosition.dsPositionList.Tables[0].Compute("MAX(DetailKZ)", "Position_OZ ='" + strPositionOZ + "'");
                        object ObjSNO = ObjEPosition.dsPositionList.Tables[0].Compute("MAX(SNO)", "Position_OZ ='" + strPositionOZ + "'");
                        int iValue = 0;
                        int iSNOValue = 0;
                        if (objDetailKZ != null && ObjSNO != null && int.TryParse(Convert.ToString(objDetailKZ), out iValue) && int.TryParse(Convert.ToString(ObjSNO), out iSNOValue))
                        {
                            DataRow dataRow = (tlPositions.GetDataRecordByNode(tlPositions.FocusedNode) as DataRowView).Row;
                            if (dataRow == null) return;
                            ParsePositionDetailsfoCopyLV(dataRow, strPositionOZ, strParentOZ, iSNOValue, string.Empty, iValue + 1);
                            ObjEPosition.RoundOffValue = ObjEProject.RoundingPrice;
                            ObjBPosition.SavePositionDetails(ObjEPosition, ObjEProject.LVRaster, ObjEProject.LVSprunge, true);
                            RefreshTreelist(ObjEPosition);
                        }
                        _IsNewMode = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void chkEinkaufspreisME_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtMulti1ME.Enabled = !chkEinkaufspreisME.Checked;
                txtMulti2ME.Enabled = !chkEinkaufspreisME.Checked;
                txtMulti3ME.Enabled = !chkEinkaufspreisME.Checked;
                txtMulti4ME.Enabled = !chkEinkaufspreisME.Checked;
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void chkEinkaufspreisMO_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtMulti1MO.Enabled = !chkEinkaufspreisMO.Checked;
                txtMulti2MO.Enabled = !chkEinkaufspreisMO.Checked;
                txtMulti3MO.Enabled = !chkEinkaufspreisMO.Checked;
                txtMulti4MO.Enabled = !chkEinkaufspreisMO.Checked;
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void chkSelbstkostenME_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkSelbstkostenME.Checked == true)
                {
                    chkEinkaufspreisME.Checked = true;
                    chkEinkaufspreisME.Enabled = false;
                    txtSelbstkostenMultiME.Enabled = false;
                }
                else
                {
                    chkEinkaufspreisME.Enabled = true;
                    txtSelbstkostenMultiME.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void chkSelbstkostenMO_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkSelbstkostenMO.Checked == true)
                {
                    chkEinkaufspreisMO.Checked = true;
                    chkEinkaufspreisMO.Enabled = false;
                    txtSelbstkostenMultiMO.Enabled = false;
                }
                else
                {
                    chkEinkaufspreisMO.Enabled = true;
                    txtSelbstkostenMultiMO.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void chkVerkaufspreisME_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkVerkaufspreisME.Checked == true)
                {
                    chkEinkaufspreisME.Checked = true;
                    chkSelbstkostenME.Checked = true;
                    chkEinkaufspreisME.Enabled = false;
                    chkSelbstkostenME.Enabled = false;
                    txtSelbstkostenMultiME.Enabled = false;
                    txtVerkaufspreisMultiME.Enabled = false;
                    txtVerkaufspreisValueME.Enabled = false;
                }
                else
                {
                    chkSelbstkostenME.Enabled = true;
                    txtSelbstkostenMultiME.Enabled = true;
                    txtVerkaufspreisMultiME.Enabled = true;
                    txtVerkaufspreisValueME.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void chkVerkaufspreisMO_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkVerkaufspreisMO.Checked == true)
                {
                    chkEinkaufspreisMO.Checked = true;
                    chkSelbstkostenMO.Checked = true;
                    chkEinkaufspreisMO.Enabled = false;
                    chkSelbstkostenMO.Enabled = false;
                    txtSelbstkostenMultiMO.Enabled = false;
                    txtVerkaufspreisMultiMO.Enabled = false;
                    txtVerkaufspreisValueMO.Enabled = false;
                }
                else
                {
                    chkSelbstkostenMO.Enabled = true;
                    txtSelbstkostenMultiMO.Enabled = true;
                    txtVerkaufspreisMultiMO.Enabled = true;
                    txtVerkaufspreisValueMO.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtSurchargeFrom_Validating(object sender, CancelEventArgs e)
        {
            if (txtSurchargeTo.Text != "")
            {
                dxValidationProvider1.Validate();
            }
        }

        private void dtpSubmitDate_EditValueChanged(object sender, EventArgs e)
        {
            dtpValidityDate.EditValue = dtpSubmitDate.EditValue;
        }

        private void txtLPMe_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                    e.Handled = true;
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtStufe1Short_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtStufe1Title.Text = GetTitle(txtStufe1Short.Text + ".");
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtStufe2Short_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtStufe2Title.Text = GetTitle(txtStufe1Short.Text + "." +
                txtStufe2Short.Text + ".");
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtStufe3Short_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtStufe3Title.Text = GetTitle(txtStufe1Short.Text + "." +
                                txtStufe2Short.Text + "." +
                                txtStufe3Short.Text + ".");
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtStufe4Short_TextChanged(object sender, EventArgs e)
        {
            txtStufe4Title.Text = GetTitle(txtStufe1Short.Text + "." +
                                txtStufe2Short.Text + "." +
                                txtStufe3Short.Text + "." +
                                txtStufe4Short.Text + ".");
        }

        private void cmbSelectGridviewOptions_Closed(object sender, ClosedEventArgs e)
        {
            
        }

        private void txtType_Leave(object sender, EventArgs e)
        {
            try
            {
                if (ObjBPosition == null)
                    ObjBPosition = new BPosition();
                if (ObjEPosition == null)
                    ObjEPosition = new EPosition();
                ObjEPosition.dtArticle = new DataTable();
                ObjEPosition.dtDimensions = new DataTable();
                txtTypeCD.Text = ObjEPosition.Type = txtType.Text;
                ObjEPosition.ValidityDate = ObjEProject.SubmitDate;
                ObjEPosition = ObjBPosition.GetArticleByTyp(ObjEPosition);
                if (ObjEPosition.dtArticle != null && ObjEPosition.dtArticle.Rows.Count > 0)
                {
                    if (ObjEPosition.dtDimensions != null && ObjEPosition.dtDimensions.Rows.Count > 0)
                    {
                        if (ObjEPosition.dtDimensions.Rows.Count > 1)
                        {
                            frmSelectDimension Obj = new frmSelectDimension();
                            Obj.ObjEPosition = ObjEPosition;
                            Obj.ShowInTaskbar = false;
                            Obj.ShowDialog();
                        }
                        else
                        {
                            ObjEPosition.Dim1 = ObjEPosition.dtDimensions.Rows[0]["A"] == DBNull.Value ? "" : ObjEPosition.dtDimensions.Rows[0]["A"].ToString();
                            ObjEPosition.Dim2 = ObjEPosition.dtDimensions.Rows[0]["B"] == DBNull.Value ? "" : ObjEPosition.dtDimensions.Rows[0]["B"].ToString();
                            ObjEPosition.Dim3 = ObjEPosition.dtDimensions.Rows[0]["L"] == DBNull.Value ? "" : ObjEPosition.dtDimensions.Rows[0]["L"].ToString();
                            ObjEPosition.LPMA = ObjEPosition.dtDimensions.Rows[0]["ListPrice"] == DBNull.Value ? 0 : Convert.ToDecimal(ObjEPosition.dtDimensions.Rows[0]["ListPrice"]);
                            ObjEPosition.Mins = ObjEPosition.dtDimensions.Rows[0]["Minuten"] == DBNull.Value ? 0 : Convert.ToDecimal(ObjEPosition.dtDimensions.Rows[0]["Minuten"]);
                            DateTime dt = DateTime.Now;
                            if (DateTime.TryParse(Convert.ToString(ObjEPosition.dtDimensions.Rows[0]["ValidityDate"]), out dt))
                                ObjEPosition.ValidityDate = dt;
                            else
                                ObjEPosition.ValidityDate = ObjEProject.SubmitDate;
                        }
                    }
                    txtWGCD.Text = ObjEPosition.WG;
                    txtWACD.Text = ObjEPosition.WA;
                    txtWICD.Text = ObjEPosition.WI;
                    if (ObjEProject.FabVisible && !string.IsNullOrEmpty(ObjEPosition.Fabricate))
                        txtFabrikate.Text = ObjEPosition.Fabricate;
                    if (!string.IsNullOrEmpty(ObjEPosition.LiefrantMA))
                        txtLiefrantMA.Text = ObjEPosition.LiefrantMA;
                    if (ObjEProject.MeVisible && !string.IsNullOrEmpty(ObjEPosition.ME))
                        cmbCDME.Text = ObjEPosition.ME;
                    txtDim1.Text = ObjEPosition.Dim1;
                    txtDim2.Text = ObjEPosition.Dim2;
                    txtDim3.Text = ObjEPosition.Dim3;
                    if (ObjEProject.MinVisible && ObjEPosition.Mins != 0)
                        txtMin.Text = Convert.ToString(ObjEPosition.Mins);
                    txtFaktor.Text = Convert.ToString(ObjEPosition.Faktor);
                    if (ObjEProject.lPVisible && ObjEPosition.LPMA != 0)
                        txtLPMe.Text = Convert.ToString(ObjEPosition.LPMA);
                    else
                        txtLPMe.Text = string.Empty;

                    decimal dv = 0;
                    if (decimal.TryParse(Convert.ToString(ObjEPosition.dtArticle.Rows[0]["Multi1"]), out dv))
                    {
                        if (ObjEProject.M1Visible)
                            txtMulti1ME.EditValue = dv;
                    }

                    if (decimal.TryParse(Convert.ToString(ObjEPosition.dtArticle.Rows[0]["Multi2"]), out dv))
                    {
                        if (ObjEProject.M2Visible)
                            txtMulti2ME.EditValue = dv;
                    }
                    if (decimal.TryParse(Convert.ToString(ObjEPosition.dtArticle.Rows[0]["Multi3"]), out dv))
                    {
                        if (ObjEProject.M3Visible)
                            txtMulti3ME.EditValue = dv;
                    }
                    if (decimal.TryParse(Convert.ToString(ObjEPosition.dtArticle.Rows[0]["Multi4"]), out dv))
                    {
                        if (ObjEProject.M4Visible)
                            txtMulti4ME.EditValue = dv;
                    }

                    if (ObjEProject.DimVisible && !string.IsNullOrEmpty(ObjEPosition.Dim))
                        txtDim.Text = ObjEPosition.Dim;
                    dtpValidityDate.EditValue = ObjEPosition.ValidityDate;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtWI_Leave(object sender, EventArgs e)
        {
            try
            {
                if (ObjBPosition == null)
                    ObjBPosition = new BPosition();
                if (ObjEPosition == null)
                    ObjEPosition = new EPosition();
                ObjEPosition.dtDimensions = new DataTable();
                ObjEPosition.dtArticle = new DataTable();
                ObjEPosition.WG = txtWG.Text;
                ObjEPosition.WA = txtWA.Text;
                ObjEPosition.WI = txtWI.Text;
                ObjEPosition.ValidityDate = ObjEProject.SubmitDate;
                ObjEPosition = ObjBPosition.GetArticleByWI(ObjEPosition);
                if (ObjEPosition.dtArticle != null && ObjEPosition.dtArticle.Rows.Count > 0)
                {
                    if (ObjEPosition.dtDimensions != null && ObjEPosition.dtDimensions.Rows.Count > 0)
                    {
                        if (ObjEPosition.dtDimensions.Rows.Count > 1)
                        {
                            frmSelectDimension Obj = new frmSelectDimension();
                            Obj.ObjEPosition = ObjEPosition;
                            Obj.ShowInTaskbar = false;
                            Obj.ShowDialog();
                        }
                        else
                        {
                            ObjEPosition.Dim1 = Convert.ToString(ObjEPosition.dtDimensions.Rows[0]["A"]);
                            ObjEPosition.Dim2 = Convert.ToString(ObjEPosition.dtDimensions.Rows[0]["B"]);
                            ObjEPosition.Dim3 = Convert.ToString(ObjEPosition.dtDimensions.Rows[0]["L"]);
                            ObjEPosition.LPMA = ObjEPosition.dtDimensions.Rows[0]["ListPrice"] == DBNull.Value ? 0 : Convert.ToDecimal(ObjEPosition.dtDimensions.Rows[0]["ListPrice"]);
                            ObjEPosition.Mins = ObjEPosition.dtDimensions.Rows[0]["Minuten"] == DBNull.Value ? 0 : Convert.ToDecimal(ObjEPosition.dtDimensions.Rows[0]["Minuten"]);
                            DateTime dt = DateTime.Now;
                            if (DateTime.TryParse(Convert.ToString(ObjEPosition.dtDimensions.Rows[0]["ValidityDate"]), out dt))
                                ObjEPosition.ValidityDate = dt;
                            else
                                ObjEPosition.ValidityDate = ObjEProject.SubmitDate;
                        }
                    }
                    else
                    {
                        ObjEPosition.LPMA = 0;
                        ObjEPosition.Mins = 0;
                        ObjEPosition.ValidityDate = ObjEProject.SubmitDate;
                    }
                    txtTypeCD.Text = txtType.Text = ObjEPosition.Type;
                    if (ObjEProject.FabVisible && !string.IsNullOrEmpty(ObjEPosition.Fabricate))
                        txtFabrikate.Text = ObjEPosition.Fabricate;
                    if (!string.IsNullOrEmpty(ObjEPosition.LiefrantMA))
                        txtLiefrantMA.Text = ObjEPosition.LiefrantMA;
                    if (ObjEProject.MeVisible && !string.IsNullOrEmpty(ObjEPosition.ME))
                        cmbCDME.SelectedIndex = cmbCDME.Properties.Items.IndexOf(ObjEPosition.ME);

                    decimal dv = 0;
                    if (decimal.TryParse(Convert.ToString(ObjEPosition.dtArticle.Rows[0]["Multi1"]), out dv))
                    {
                        if (ObjEProject.M1Visible)
                            txtMulti1ME.EditValue = dv;
                    }

                    if (decimal.TryParse(Convert.ToString(ObjEPosition.dtArticle.Rows[0]["Multi2"]), out dv))
                    {
                        if (ObjEProject.M2Visible)
                            txtMulti2ME.EditValue = dv;
                    }
                    if (decimal.TryParse(Convert.ToString(ObjEPosition.dtArticle.Rows[0]["Multi3"]), out dv))
                    {
                        if (ObjEProject.M3Visible)
                            txtMulti3ME.EditValue = dv;
                    }
                    if (decimal.TryParse(Convert.ToString(ObjEPosition.dtArticle.Rows[0]["Multi4"]), out dv))
                    {
                        if (ObjEProject.M4Visible)
                            txtMulti4ME.EditValue = dv;
                    }

                    txtFaktor.Text = ObjEPosition.Faktor.ToString();
                    txtDim1.Text = ObjEPosition.Dim1;
                    txtDim2.Text = ObjEPosition.Dim2;
                    txtDim3.Text = ObjEPosition.Dim3;
                    if (ObjEProject.MinVisible && ObjEPosition.Mins != 0)
                        txtMin.Text = ObjEPosition.Mins.ToString();
                    if (ObjEProject.lPVisible && ObjEPosition.LPMA != 0)
                        txtLPMe.Text = ObjEPosition.LPMA.ToString();
                    if (ObjEProject.DimVisible && !string.IsNullOrEmpty(ObjEPosition.Dim))
                        txtDim.Text = ObjEPosition.Dim;
                    dtpValidityDate.EditValue = ObjEPosition.ValidityDate;
                }
                else
                {
                    ObjEPosition.LPMA = 0;
                    ObjEPosition.Mins = 0;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtWI_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    txtWI_Leave(null, null);
                    _IsKeyypressEvent = true;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtDim1_Leave(object sender, EventArgs e)
        {
            try
            {
                string _DimType = string.Empty;
                if (ObjBPosition == null)
                    ObjBPosition = new BPosition();
                if (ObjEPosition == null)
                    ObjEPosition = new EPosition();
                ObjEPosition.dtDimensions = new DataTable();
                ObjEPosition.WG = txtWG.Text;
                ObjEPosition.WA = txtWA.Text;
                ObjEPosition.WI = txtWI.Text;
                ObjEPosition.Dim1 = txtDim1.Text;
                ObjEPosition.Dim2 = txtDim2.Text;
                ObjEPosition.Dim3 = txtDim3.Text;
                ObjEPosition.ValidityDate = ObjEProject.SubmitDate;
                if (!string.IsNullOrEmpty(_txtDimensions))
                {
                    if (_txtDimensions == "txtDim1")
                    {
                        _DimType = "A";
                        ObjEPosition = ObjBPosition.GetArticleByA(ObjEPosition, _DimType);
                        txtDim1.Text = ObjEPosition.Dim1;
                        txtDim2.Text = ObjEPosition.Dim2;
                        txtDim3.Text = ObjEPosition.Dim3;
                    }
                    if (_txtDimensions == "txtDim2")
                    {
                        _DimType = "B";
                        ObjEPosition = ObjBPosition.GetArticleByB(ObjEPosition, _DimType);
                        txtDim2.Text = ObjEPosition.Dim2;
                        txtDim3.Text = ObjEPosition.Dim3;
                    }
                }
                ObjEPosition = ObjBPosition.GetArticleByDimension(ObjEPosition);
                if (ObjEPosition.dtDimensions.Rows.Count > 0)
                {
                    txtMin.EditValue = ObjEPosition.Mins;
                    txtFaktor.EditValue = ObjEPosition.Faktor;
                    if (ObjEPosition.LPMA != 0)
                        txtLPMe.EditValue = ObjEPosition.LPMA;
                    else
                        txtLPMe.Text = string.Empty;
                    dtpValidityDate.EditValue = ObjEPosition.ValidityDate;
                }

            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtDim1_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                TextEdit txtDimens = (TextEdit)sender;
                _txtDimensions = txtDimens.Name;
                if (!Char.IsDigit(e.KeyChar) && (e.KeyChar) != '\b')
                    e.Handled = true;
                if (e.KeyChar == (char)Keys.Enter)
                {
                    txtDim1_Leave(null, null);
                }

            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnAddAccessories_Click(object sender, EventArgs e)
        {

            try
            {
                string strPositionKZ = Utility.GetPosKZ(cmbPositionKZ.Text);
                if (strPositionKZ == "N" || strPositionKZ == "E" || strPositionKZ == "A" || strPositionKZ == "M" || strPositionKZ == "P")
                {
                    BArticles ObjBArticles = new BArticles();
                    EArticles ObjEArticles = new EArticles();
                    int SNo = -1;
                    if (ObjEPosition.PositionID > 0)
                    {
                        if (tlPositions.FocusedNode != null && tlPositions.FocusedNode["SNO"] != null)
                        {
                            int IValue = 0;
                            bool _HaveDetailKZ = false;
                            if (int.TryParse(Convert.ToString(tlPositions.FocusedNode["DetailKZ"]), out IValue))
                            {
                                if (IValue > 0)
                                    return;
                            }
                            if (bool.TryParse(Convert.ToString(tlPositions.FocusedNode["HaveDetailkz"]), out _HaveDetailKZ))
                            {
                                if (_HaveDetailKZ)
                                    return;
                            }

                            if (int.TryParse(Convert.ToString(tlPositions.FocusedNode["SNO"]), out IValue))
                                SNo = IValue;
                            else
                                SNo = -1;
                        }
                    }

                    ObjEArticles.WG = txtWGCD.Text;
                    ObjEArticles.WA = txtWACD.Text;
                    ObjEArticles.WI = txtWICD.Text;
                    ObjEArticles.A = txtDim1.Text;
                    ObjEArticles.B = txtDim2.Text;
                    ObjEArticles.L = txtDim3.Text;
                    ObjEArticles = ObjBArticles.GetAccessoriesForLVs(ObjEArticles);
                    if (ObjEArticles.dtAccessories == null || ObjEArticles.dtAccessories.Rows.Count <= 0)
                        throw new Exception("Keine Zubehörangaben für ausgewählten Artikel");
                    DataTable dt = ObjEArticles.dtAccessories.Copy();
                    ObjEArticles.dtAccessories = new DataTable();
                    ObjEArticles.dtAccessories = dt.Clone();
                    DataRow drnew = ObjEArticles.dtAccessories.NewRow();
                    drnew["WG"] = ObjEArticles.WG;
                    drnew["WA"] = ObjEArticles.WA;
                    drnew["WI"] = ObjEArticles.WI;
                    drnew["A"] = ObjEArticles.A;
                    drnew["B"] = ObjEArticles.B;
                    drnew["L"] = ObjEArticles.L;
                    drnew["MENGE"] = 0;
                    ObjEArticles.dtAccessories.Rows.Add(drnew);
                    foreach (DataRow dr in dt.Rows)
                        ObjEArticles.dtAccessories.ImportRow(dr);
                    ObjEArticles.ValidityDate = ObjEProject.SubmitDate;
                    frmSelectAccessories Obj = new frmSelectAccessories();
                    Obj.ObjEArticle = ObjEArticles;
                    Obj.ShowInTaskbar = false;
                    Obj.ShowDialog();
                    if (Obj._ISSave)
                    {
                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                        SplashScreenManager.Default.SetWaitFormDescription("Zubehör hinzufügen...");
                        btnSaveLVDetails_Click(null, null);
                        btnCancel_Click(null, null);
                        int NewPositionID = 0;
                        int IDetailKz = 0;

                        DataView dvAccessories = ObjEArticles.dtAccessories.DefaultView;
                        dvAccessories.RowFilter = "MENGE > 0";
                        DataTable dtTemp = dvAccessories.ToTable();
                        ObjEPosition.RoundOffValue = ObjEProject.RoundingPrice;
                        foreach (DataRow dr in dtTemp.Rows)
                        {
                            if (ObjBPosition == null)
                                ObjBPosition = new BPosition();
                            if (ObjEPosition == null)
                                ObjEPosition = new EPosition();
                            IDetailKz++;
                            ParseAccessories(dr, SNo, IDetailKz, cmbLVSection.Text, txtFaktor.Text, ObjEArticles, ObjBArticles);
                            ObjBPosition.SavePositionDetails(ObjEPosition, ObjEProject.LVRaster, ObjEProject.LVSprunge, false);
                            NewPositionID = ObjEPosition.PositionID;
                            if (SNo != -1)
                                SNo++;
                        }
                        BindPositionData();
                        SetFocus(NewPositionID, tlPositions);
                    }
                }
                SplashScreenManager.CloseForm(false);
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void tlPositions_DragEnter(object sender, DragEventArgs e)
        {
            try
            {
                e.Effect = DragDropEffects.Move;
            }
            catch (Exception ex) { }
        }

        private void tlPositions_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                e.Effect = DragDropEffects.None;
                DXDragEventArgs args = tlPositions.GetDXDragEventArgs(e);
                DataRow dataRow = (tlPositions.GetDataRecordByNode(tlPositions.FocusedNode) as DataRowView).Row;
                if (dataRow == null) return;
                string _OldPosKZ = dataRow["PositionKZ"] == DBNull.Value ? "" : dataRow["PositionKZ"].ToString();
                if (_OldPosKZ == "NG" || _OldPosKZ == "ZZ" || _OldPosKZ == "Z")
                    return;
                TreeListNode Tnode = args.TargetNode;
                if (Tnode == null)
                    return;
                int IValue = 0;
                if (int.TryParse(Convert.ToString(Tnode["DetailKZ"]), out IValue))
                {
                    if (IValue > 0)
                        return;
                }
                else
                    return;

                bool _HaveDetailKZ = false;
                if (bool.TryParse(Convert.ToString(Tnode["HaveDetailkz"]), out _HaveDetailKZ))
                {
                    if (_HaveDetailKZ)
                        return;
                }
                else
                    return;

                string TargetPositionKZ = Convert.ToString(Tnode["PositionKZ"]);
                string strOZ = Convert.ToString(Tnode["Position_OZ"]);
                string SNO = string.Empty;
                string ParentID = string.Empty;
                string ParentOZ = string.Empty;
                string PositionOZ = string.Empty;
                string strnextLV = string.Empty;
                int IIndex = 0;
                if (string.IsNullOrEmpty(strOZ))
                    return;
                if (TargetPositionKZ == "NG")
                {
                    string[] _Raster = ObjEProject.LVRaster.Split('.');
                    int _Rastercount = _Raster.Count();
                    ParentOZ = Tnode["Position_OZ"].ToString();
                    string[] _OZ = ParentOZ.Split('.');
                    int _OZCount = _OZ.Count();
                    if (_OZCount != _Rastercount - 1)
                        return;
                    IIndex = 0;
                    ParentID = Convert.ToString(Tnode["PositionID"]);
                    if (Tnode.FirstNode != null)
                        PositionOZ = Convert.ToString(Tnode.FirstNode["Position_OZ"]);
                    SNO = "0";
                }
                else
                {
                    SNO = Convert.ToString(Tnode["SNO"]);
                    ParentID = Convert.ToString(Tnode["Parent_OZ"]);
                    ParentOZ = Convert.ToString(Tnode.ParentNode["Position_OZ"]);
                    PositionOZ = Convert.ToString(Tnode["Position_OZ"]);
                    IIndex = 2;
                    int INodeIndex = tlPositions.GetNodeIndex(Tnode);
                    if (INodeIndex != null)
                    {
                        bool _Continue = true;
                        int iValue = 0;
                        while (_Continue)
                        {
                            iValue++;
                            if (Tnode.ParentNode.Nodes.Count > INodeIndex + iValue)
                            {
                                strnextLV = Convert.ToString(Tnode.ParentNode.Nodes[INodeIndex + iValue]["Position_OZ"]);
                                if (!string.IsNullOrEmpty(strnextLV))
                                    _Continue = false;

                            }
                            else
                                _Continue = false;
                        }
                    }
                }

                int IPositionID = dataRow["PositionID"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PositionID"]);
                string strPositionOZ = Convert.ToString(dataRow["Position_OZ"]);

                if (ObjBPosition == null)
                    ObjBPosition = new BPosition();
                if (ObjEPosition == null)
                    ObjEPosition = new EPosition();

                DataTable dtTemp = ObjEPosition.dsPositionList.Tables[0].Copy();
                DataView dvTemp = dtTemp.DefaultView;
                if (_OldPosKZ == "H" || _OldPosKZ == "VR" || _OldPosKZ == "AB" || _OldPosKZ == "BA" || _OldPosKZ == "UB")
                {
                    dvTemp.RowFilter = "PositionID = " + IPositionID;
                }
                else
                {
                    dvTemp.RowFilter = "Position_OZ = '" + strPositionOZ + "'";
                }
                ObjEPosition.dtCopyPosition = dvTemp.ToTable();
                foreach (DataColumn dc in dtTemp.Columns)
                {
                    if (dc.ColumnName != "PositionID")
                    {
                        if (dc.ColumnName != "SNO")
                            ObjEPosition.dtCopyPosition.Columns.Remove(dc.ColumnName);
                    }
                }
                string _Suggested_OZ = string.Empty;
                if (_OldPosKZ == "H" || _OldPosKZ == "VR" || _OldPosKZ == "AB" || _OldPosKZ == "BA" || _OldPosKZ == "UB")
                { ObjEPosition.Position_OZ = string.Empty; }
                else
                {
                    _Suggested_OZ = SuggestOZForCopy(PositionOZ, strnextLV, IIndex);
                    frmNewOZ Obj = new frmNewOZ();
                    Obj.strNewOZ = _Suggested_OZ;
                    Obj.LVRaster = ObjEProject.LVRaster;
                    Obj.ShowDialog();
                    if (!Obj.IsSave)
                        return;
                    _Suggested_OZ = Obj.strNewOZ;
                    string str = string.Empty;
                    string strRaster = ObjEProject.LVRaster;
                    string[] strPOZ = PositionOZ.Split('.');
                    string[] strPRaster = strRaster.Split('.');
                    int Count = -1;
                    int i = -1;
                    Count = strPOZ.Count();
                    while (Count > 0)
                    {
                        i = i + 1;
                        Count = Count - 1;
                        int OZLength = 0;
                        int RasterLength = 0;
                        string OZ = string.Empty;
                        OZ = strPOZ[i].Trim();
                        RasterLength = strPRaster[i].Length;
                        OZLength = OZ.Trim().Length;
                        if (Count > 0)
                        {
                            if (OZ == "")
                            {
                                str = str + string.Concat(Enumerable.Repeat(" ", RasterLength - OZLength)) + OZ + ".";
                            }
                        }
                    }
                    if (str.Length > 0)
                    {
                        string strBlankNewOz = ParentOZ + str + _Suggested_OZ;
                        ObjEPosition.Position_OZ = strBlankNewOz;
                    }
                    else
                    {
                        string strNewOz = ParentOZ + _Suggested_OZ;
                        ObjEPosition.Position_OZ = strNewOz;
                    }
                }

                ObjEPosition.ProjectID = ObjEProject.ProjectID;
                ObjEPosition.PositionID = IPositionID;

                int IParnetValue = 0;
                if (int.TryParse(ParentID, out IParnetValue))
                    ObjEPosition.ParentID = IParnetValue;
                else
                    throw new Exception("Fehler beim Verschieben der Position");

                if (int.TryParse(SNO, out IValue))
                    ObjEPosition.SNO = IValue;
                else
                    throw new Exception("Fehler beim Verschieben der Position");
                int ITemp = ObjEPosition.SNO;


                string stOZChar = string.Empty;
                if (!double.TryParse(_Suggested_OZ, NumberStyles.Float, CultureInfo.GetCultureInfo("en"), out ObjEPosition.OZID))
                    ObjEPosition.OZID = 0;


                string OZ1 = string.Empty, OZ2 = string.Empty, OZ3 = string.Empty, OZ4 = string.Empty, OZ5 = string.Empty, OZ6 = string.Empty;
                if (!string.IsNullOrEmpty(ObjEPosition.Position_OZ))
                {
                    string[] strOZList = Utility.PrepareOZ(ObjEPosition.Position_OZ, ObjEProject.LVRaster).Split('.');
                    if (strOZList.Count() > 1)
                    {
                        string strOZID = strOZList[strOZList.Count() - 2];
                        string strIndex = strOZList[strOZList.Count() - 1];
                        char[] OZcharList = strOZID.ToCharArray();
                        int CharCount = OZcharList.Count();
                        if (CharCount > 0)
                        {
                            OZ1 = Convert.ToString(OZcharList[0]);
                            if (CharCount > 1)
                            {
                                OZ2 = Convert.ToString(OZcharList[1]);
                                if (CharCount > 2)
                                {
                                    OZ3 = Convert.ToString(OZcharList[2]);
                                    if (CharCount > 3)
                                    {
                                        OZ4 = Convert.ToString(OZcharList[3]);
                                        if (CharCount > 4)
                                        {
                                            OZ5 = Convert.ToString(OZcharList[4]);
                                            OZ6 = strIndex;
                                        }
                                        else
                                            OZ5 = strIndex;
                                    }
                                    else
                                        OZ4 = strIndex;
                                }
                                else
                                    OZ3 = strIndex;
                            }
                            else
                                OZ2 = strIndex;
                        }
                    }
                }


                ObjEPosition.dtCopyPosition.Columns.Add("O1", typeof(string));
                ObjEPosition.dtCopyPosition.Columns.Add("O2", typeof(string));
                ObjEPosition.dtCopyPosition.Columns.Add("O3", typeof(string));
                ObjEPosition.dtCopyPosition.Columns.Add("O4", typeof(string));
                ObjEPosition.dtCopyPosition.Columns.Add("O5", typeof(string));
                ObjEPosition.dtCopyPosition.Columns.Add("O6", typeof(string));
                foreach (DataRow dr in ObjEPosition.dtCopyPosition.Rows)
                {
                    ITemp++;
                    dr["SNO"] = ITemp;
                    dr["O1"] = OZ1;
                    dr["O2"] = OZ2;
                    dr["O3"] = OZ3;
                    dr["O4"] = OZ4;
                    dr["O5"] = OZ5;
                    dr["O6"] = OZ6;
                }
                int NewPositionID = ObjBPosition.CopyPosition(ObjEPosition, stOZChar);
                BindPositionData();
                SetFocus(NewPositionID, tlPositions);
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtWG_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                TextEdit textBox = (TextEdit)sender;
                if (e.KeyData == Keys.F5)
                {
                    panelControldoc.Visible = true;
                    toggleSwitchType.Visible = true;
                    dockPanelArticles.Show();
                    lblArticles.Text = "Artikels :" + txtWG.Text + "/" + txtWA.Text + "/" + txtWI.Text;
                    lblDimensions.Text = "Maße :" + txtDim1.Text + "/" + txtDim2.Text + "/" + txtDim3.Text;
                    dockPanelArticles_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void dockPanelArticles_Click(object sender, EventArgs e)
        {
            try
            {
                toggleSwitchType.IsOn = true;
                BindCoaprePrice("Project");
                BgvComparePrice.Columns["ProjectNumber"].Visible = false;
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }

        }

        private void toggleSwitchType_Toggled(object sender, EventArgs e)
        {
            try
            {
                if (toggleSwitchType.IsOn)
                {
                    BindCoaprePrice("Project");
                    BgvComparePrice.Columns["ProjectNumber"].Visible = false;
                }
                else
                {
                    BindCoaprePrice("Customer");
                    BgvComparePrice.Columns["ProjectNumber"].Visible = true;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void tlOldProject_DragDrop(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
        }

        private void txtType_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                _IsTypKeyypressEvent = true;
                txtType_Leave(null, null);
            }
        }

        private void txtType_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
        }

        private void txtDetailKZCD_TextChanged(object sender, EventArgs e)
        {
            txtDetailKZ.Text = txtDetailKZCD.Text;
        }

        private void txtWGCD_TextChanged(object sender, EventArgs e)
        {
            txtWG.Text = txtWGCD.Text;
        }

        private void txtWACD_TextChanged(object sender, EventArgs e)
        {
            txtWA.Text = txtWACD.Text;
        }

        private void txtWICD_TextChanged(object sender, EventArgs e)
        {
            txtWI.Text = txtWICD.Text;
        }

        private void txtTypeCD_TextChanged(object sender, EventArgs e)
        {
            txtType.Text = txtTypeCD.Text;
        }

        private void txtShortDescriptionCD_TextChanged(object sender, EventArgs e)
        {
            txtShortDescription.Text = txtShortDescriptionCD.Text.Trim();
        }

        private void txtMengeCD_EditValueChanged(object sender, EventArgs e)
        {
            txtMenge.Text = txtMengeCD.Text;
        }

        private void cmbPositionKZCD_EditValueChanged(object sender, EventArgs e)
        {
            cmbPositionKZ.EditValue = cmbPositionKZCD.EditValue;
        }

        private void cmbME_TextChanged(object sender, EventArgs e)
        {
            cmbME.Text = cmbCDME.Text;
        }

        private void txtShortDescription_KeyDown(object sender, KeyEventArgs e)
        {
            //try
            //{
            //    RichTextBox txt = (RichTextBox)sender;
            //    if (e.KeyCode == Keys.Enter && txt.Lines.Length >= 2)
            //    {
            //        e.Handled = true;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Utility.ShowError(ex);
            //}
        }

        private void txtVerkaufspreisValueME_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                TextEdit textedit = (TextEdit)sender;
                ShowTooltip(textedit);
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtVerkaufspreisValueME_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    decimal VerkValue = 0;
                    if (decimal.TryParse(txtVerkaufspreisValueME.Text, out VerkValue) && VerkValue != 0 && Position.MA_SK != 0)
                    {
                        Position.MA_VK = VerkValue;
                        decimal VerkMulti = 0;
                        VerkMulti = Math.Round(Position.MA_VK / Position.MA_SK, 3);
                        IsFire = false;
                        txtVerkaufspreisMultiME.EditValue = VerkMulti;
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtVerkaufspreisValueMO_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    decimal VerkValue = 0;
                    if (decimal.TryParse(txtVerkaufspreisValueMO.Text, out VerkValue) && VerkValue != 0 && Position.MO_SK != 0)
                    {
                        Position.MO_VK = VerkValue;
                        decimal VerkMulti = 0;
                        VerkMulti = Math.Round(Position.MO_VK / Position.MO_SK, 3);
                        IsFire = false;
                        txtVerkaufspreisMultiMO.EditValue = VerkMulti;
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtVerkaufspreisValueME_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.F8)
                {
                    string strPositionKZ = Utility.GetPosKZ(cmbPositionKZ.Text);
                    if (strPositionKZ == "ZZ")
                        return;
                    if (ObjEProject.IsFinalInvoice)
                        return;
                    decimal VerkValue = 0;
                    if (decimal.TryParse(txtVerkaufspreisValueME.Text, out VerkValue) && VerkValue != 0 && Position.MA_SK != 0)
                    {
                        Position.MA_VK = VerkValue;
                        decimal VerkMulti = 0;
                        VerkMulti = Math.Round(Position.MA_VK / Position.MA_SK, 3);
                        IsFire = false;
                        txtVerkaufspreisMultiME.EditValue = VerkMulti;
                        txtVerkaufspreisValueME_TextChanged(null, null);
                        btnSaveLVDetails_Click(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtVerkaufspreisValueMO_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.F8)
                {
                    string strPositionKZ = Utility.GetPosKZ(cmbPositionKZ.Text);
                    if (strPositionKZ == "ZZ")
                        return;
                    if (ObjEProject.IsFinalInvoice)
                        return;
                    decimal VerkValue = 0;
                    if (decimal.TryParse(txtVerkaufspreisValueMO.Text, out VerkValue) && VerkValue != 0 && Position.MO_SK != 0)
                    {
                        Position.MO_VK = VerkValue;
                        decimal VerkMulti = 0;
                        VerkMulti = Math.Round(Position.MO_VK / Position.MO_SK, 3);
                        IsFire = false;
                        txtVerkaufspreisMultiMO.EditValue = VerkMulti;
                        txtVerkaufspreisValueMO_TextChanged(null, null);
                        btnSaveLVDetails_Click(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtValue1MO_Properties_Enter(object sender, EventArgs e)
        {
            try
            {
                var edit = ((DevExpress.XtraEditors.TextEdit)sender);
                BeginInvoke(new MethodInvoker(() =>
                {
                    edit.SelectionStart = 0;
                    edit.SelectionLength = edit.Text.Length;
                }));
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtValue1MO_Properties_Leave(object sender, EventArgs e)
        {
            try
            {
                TextEdit textBox = (TextEdit)sender;
                if (textBox == txtLPMe && Position.MA_ListPrice != 0)
                    textBox.EditValue = Math.Round(Position.MA_ListPrice, 2);
                else if (textBox == txtValue1ME && Position.MA_Multi1 != 0)
                    textBox.EditValue = Math.Round(Position.MA_Multi1, 2);
                else if (textBox == txtValue2ME && Position.MA_Multi2 != 0)
                    textBox.EditValue = Math.Round(Position.MA_Multi2, 2);
                else if (textBox == txtValue3ME && Position.MA_Multi3 != 0)
                    textBox.EditValue = Math.Round(Position.MA_Multi3, 2);
                else if (textBox == txtValue4ME && Position.MA_Multi4 != 0)
                    textBox.EditValue = Math.Round(Position.MA_Multi4, 2);
                else if (textBox == txtEinkaufspreisME && Position.MA_EP != 0)
                    textBox.EditValue = Math.Round(Position.MA_EP, 2);
                else if (textBox == txtSelbstkostenValueME && Position.MA_SK != 0)
                    textBox.EditValue = Math.Round(Position.MA_SK, 2);
                else if (textBox == txtVerkaufspreisValueME && Position.MA_VK != 0)
                    textBox.EditValue = Math.Round(Position.MA_VK, 2);
                else if (textBox == txtLPMO && Position.MO_ListPrice != 0)
                    textBox.EditValue = Math.Round(Position.MO_ListPrice, 2);
                else if (textBox == txtValue1MO && Position.MO_Multi1 != 0)
                    textBox.EditValue = Math.Round(Position.MO_Multi1, 2);
                else if (textBox == txtValue2MO && Position.MO_Multi2 != 0)
                    textBox.EditValue = Math.Round(Position.MO_Multi2, 2);
                else if (textBox == txtValue3MO && Position.MO_Multi3 != 0)
                    textBox.EditValue = Math.Round(Position.MO_Multi3, 2);
                else if (textBox == txtValue4MO && Position.MO_Multi4 != 0)
                    textBox.EditValue = Math.Round(Position.MO_Multi4, 2);
                else if (textBox == txtEinkaufspreisMO && Position.MO_EP != 0)
                    textBox.EditValue = Math.Round(Position.MO_EP, 2);
                else if (textBox == txtSelbstkostenValueMO && Position.MO_SK != 0)
                    textBox.EditValue = Math.Round(Position.MO_SK, 2);
                else if (textBox == txtVerkaufspreisValueMO && Position.MO_VK != 0)
                    textBox.EditValue = Math.Round(Position.MO_VK, 2);
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtWG_Enter(object sender, EventArgs e)
        {
            try
            {
                var edit = ((DevExpress.XtraEditors.TextEdit)sender);
                BeginInvoke(new MethodInvoker(() =>
                {
                    edit.SelectionStart = 0;
                    edit.SelectionLength = edit.Text.Length;
                }));
            }
            catch (Exception ex) { }
        }

        private void txtWG_Leave(object sender, EventArgs e)
        {
            try
            {
                TextEdit txt = sender as TextEdit;
                string OldWG = string.Empty;
                string OldWA = string.Empty;

                OldWG = Convert.ToString(tlPositions.FocusedNode["WG"]);
                OldWA = Convert.ToString(tlPositions.FocusedNode["WA"]);

                if (OldWG != txtWG.Text || OldWA != txtWA.Text && !_IsKeyypressEvent)
                {
                    if (ObjBPosition == null)
                        ObjBPosition = new BPosition();
                    if (ObjEPosition == null)
                        ObjEPosition = new EPosition();
                    ObjEPosition.dtDimensions = new DataTable();
                    ObjEPosition.dtArticle = new DataTable();
                    ObjEPosition.WG = txtWG.Text;
                    ObjEPosition.WA = txtWA.Text;
                    ObjEPosition.WI = txtWI.Text;
                    ObjEPosition.ValidityDate = ObjEProject.SubmitDate;
                    ObjEPosition.ProjectID = ObjEProject.ProjectID;
                    ObjEPosition = ObjBPosition.GetArticleByWI(ObjEPosition);
                    if (ObjEPosition.dtArticle != null && ObjEPosition.dtArticle.Rows.Count > 0)
                    {
                        txtTypeCD.Text = txtType.Text = ObjEPosition.Type;
                        if (ObjEProject.FabVisible && !string.IsNullOrEmpty(ObjEPosition.Fabricate))
                            txtFabrikate.Text = ObjEPosition.Fabricate;
                        if (!string.IsNullOrEmpty(ObjEPosition.LiefrantMA))
                            txtLiefrantMA.Text = ObjEPosition.LiefrantMA;
                        if (ObjEProject.MeVisible && !string.IsNullOrEmpty(ObjEPosition.ME))
                            cmbCDME.Text = ObjEPosition.ME;

                        decimal dv = 0;
                        if (decimal.TryParse(Convert.ToString(ObjEPosition.dtArticle.Rows[0]["Multi1"]), out dv))
                        {
                            if (ObjEProject.M1Visible)
                                txtMulti1ME.EditValue = dv;
                        }

                        if (decimal.TryParse(Convert.ToString(ObjEPosition.dtArticle.Rows[0]["Multi2"]), out dv))
                        {
                            if (ObjEProject.M2Visible)
                                txtMulti2ME.EditValue = dv;
                        }
                        if (decimal.TryParse(Convert.ToString(ObjEPosition.dtArticle.Rows[0]["Multi3"]), out dv))
                        {
                            if (ObjEProject.M3Visible)
                                txtMulti3ME.EditValue = dv;
                        }
                        if (decimal.TryParse(Convert.ToString(ObjEPosition.dtArticle.Rows[0]["Multi4"]), out dv))
                        {
                            if (ObjEProject.M4Visible)
                                txtMulti4ME.EditValue = dv;
                        }

                        txtFaktor.Text = ObjEPosition.Faktor.ToString();
                        if (ObjEProject.DimVisible && !string.IsNullOrEmpty(ObjEPosition.Dim))
                            txtDim.Text = ObjEPosition.Dim;
                    }
                }
                if (_IsKeyypressEvent)
                    _IsKeyypressEvent = false;
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void ChkManualMontageentry_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void txtType_Leave_1(object sender, EventArgs e)
        {
            try
            {
                if (tlPositions.FocusedNode != null)
                {
                    string OldTyp = Convert.ToString(tlPositions.FocusedNode["Type"]);
                    if (OldTyp != txtType.Text && !_IsTypKeyypressEvent)
                        FillDimension();
                    if (_IsTypKeyypressEvent)
                        _IsTypKeyypressEvent = false;
                }
            }
            catch (Exception ex) { }
        }

        private void txtLVPositionCD_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!_FiredFromEvent)
                {
                    if (!string.IsNullOrEmpty(txtLVPositionCD.Text) && !string.IsNullOrEmpty(ObjEProject.LVRaster))
                    {
                        string[] Titles = txtLVPositionCD.Text.Split('.');
                        string[] Raster = ObjEProject.LVRaster.Split('.');
                        int TitleCount = Titles.Count();

                        if (Titles != null && Raster != null && TitleCount == Raster.Count())
                        {
                            //if postion 1.1.1.1.10.1
                            TitleCount -= 2;
                            if (TitleCount > 0)
                            {
                                txtStufe1Short.Text = Titles[0];
                                txtStufe1Short_TextChanged(null, null);
                                if (TitleCount > 1)
                                {
                                    txtStufe2Short.Text = Titles[1];
                                    txtStufe2Short_TextChanged(null, null);
                                    if (TitleCount > 2)
                                    {
                                        txtStufe3Short.Text = Titles[2];
                                        txtStufe3Short_TextChanged(null, null);
                                        if (TitleCount > 3)
                                        {
                                            txtStufe4Short.Text = Titles[3];
                                            txtStufe4Short_TextChanged(null, null);
                                        }
                                        else
                                        {
                                            txtStufe4Short.Text = string.Empty;
                                        }
                                    }
                                    else
                                    {
                                        txtStufe3Short.Text = string.Empty;
                                        txtStufe4Short.Text = string.Empty;
                                    }
                                }
                                else
                                {
                                    txtStufe2Short.Text = string.Empty;
                                    txtStufe3Short.Text = string.Empty;
                                    txtStufe4Short.Text = string.Empty;
                                }
                            }
                            else
                            {
                                txtStufe1Short.Text = string.Empty;
                                txtStufe2Short.Text = string.Empty;
                                txtStufe3Short.Text = string.Empty;
                                txtStufe4Short.Text = string.Empty;
                            }
                            txtPosition.Text = Titles[TitleCount] + "." + Titles[TitleCount + 1];
                        }
                        else if (TitleCount <= Raster.Count() - 1)
                        {
                            //if position 1.1.1.1.10
                            TitleCount -= 1;
                            if (TitleCount > 0)
                            {
                                txtStufe1Short.Text = Titles[0];
                                if (TitleCount > 1)
                                {
                                    txtStufe2Short.Text = Titles[1];
                                    if (TitleCount > 2)
                                    {
                                        txtStufe3Short.Text = Titles[2];
                                        if (TitleCount > 3)
                                            txtStufe4Short.Text = Titles[3];
                                        else
                                            txtStufe4Short.Text = string.Empty;
                                    }
                                    else
                                    {
                                        txtStufe3Short.Text = string.Empty;
                                        txtStufe4Short.Text = string.Empty;
                                    }
                                }
                                else
                                {
                                    txtStufe2Short.Text = string.Empty;
                                    txtStufe3Short.Text = string.Empty;
                                    txtStufe4Short.Text = string.Empty;
                                }
                            }
                            else
                            {
                                txtStufe1Short.Text = string.Empty;
                                txtStufe2Short.Text = string.Empty;
                                txtStufe3Short.Text = string.Empty;
                                txtStufe4Short.Text = string.Empty;
                            }
                            if (TitleCount == Raster.Count() - 1)
                                txtPosition.Text = Titles[TitleCount];
                        }
                        txtLVPosition.Text = txtLVPositionCD.Text;
                    }
                    else
                    {
                        txtLVPosition.Text = string.Empty;
                        txtStufe1Short.Text = string.Empty;
                        txtStufe2Short.Text = string.Empty;
                        txtStufe3Short.Text = string.Empty;
                        txtStufe4Short.Text = string.Empty;
                        txtPosition.Text = string.Empty;
                    }
                }
                else
                    _FiredFromEvent = false;
            }
            catch (Exception ex) { }
        }

        private void txtDetailKZCD_EditValueChanged(object sender, EventArgs e)
        {
            txtDetailKZ.EditValue = txtDetailKZCD.EditValue;
        }

        private void btnValiditydates_Click(object sender, EventArgs e)
        {
            try
            {
                EArticles ObjEArticles = new EArticles();
                DArticles ObjDArticles = new DArticles();
                ObjEArticles.WG = txtWG.Text;
                ObjEArticles.WA = txtWA.Text;
                ObjEArticles.WI = txtWI.Text;
                ObjEArticles.A = txtDim1.Text;
                ObjEArticles.B = txtDim2.Text;
                ObjEArticles.L = txtDim3.Text;
                ObjDArticles.GetDimensionValidityDates(ObjEArticles);
                frmAddType Obj = new frmAddType();
                Obj.Typ = "DM";
                Obj.dtDates = ObjEArticles.dtDimensionValidityDates;
                Obj.ShowDialog();
                if (Obj._IsSave)
                {
                    ObjEArticles.DimensionID = Obj.DimensionID;
                    ObjDArticles.GetValuesByDimension(ObjEArticles);
                    decimal dValue = 0;
                    if (ObjEArticles.dtMultiValues != null && ObjEArticles.dtMultiValues.Rows.Count > 0)
                    {
                        if (decimal.TryParse(Convert.ToString(ObjEArticles.dtMultiValues.Rows[0]["Multi1"]), out dValue))
                            txtMulti1ME.EditValue = dValue;
                        else
                            txtMulti1ME.EditValue = 1;
                        if (decimal.TryParse(Convert.ToString(ObjEArticles.dtMultiValues.Rows[0]["Multi2"]), out dValue))
                            txtMulti2ME.EditValue = dValue;
                        else
                            txtMulti2ME.EditValue = 1;
                        if (decimal.TryParse(Convert.ToString(ObjEArticles.dtMultiValues.Rows[0]["Multi3"]), out dValue))
                            txtMulti3ME.EditValue = dValue;
                        else
                            txtMulti3ME.EditValue = 1;
                        if (decimal.TryParse(Convert.ToString(ObjEArticles.dtMultiValues.Rows[0]["Multi4"]), out dValue))
                            txtMulti4ME.EditValue = dValue;
                        else
                            txtMulti4ME.EditValue = 1;
                    }
                    if (ObjEArticles.dtDimensionValues != null && ObjEArticles.dtDimensionValues.Rows.Count > 0)
                    {
                        if (decimal.TryParse(Convert.ToString(ObjEArticles.dtDimensionValues.Rows[0]["listPrice"]), out dValue) && dValue != 0)
                        {
                            txtLPMe.Properties.Mask.UseMaskAsDisplayFormat = true;
                            Position.MA_ListPrice = dValue;
                            txtLPMe.EditValue = Math.Round(dValue, 2);
                        }
                        else
                        {
                            Position.MA_ListPrice = 0;
                            txtLPMe.Properties.Mask.UseMaskAsDisplayFormat = false;
                            txtLPMe.Text = string.Empty;
                        }
                        if (decimal.TryParse(Convert.ToString(ObjEArticles.dtDimensionValues.Rows[0]["Minuten"]), out dValue))
                            txtMin.EditValue = dValue;
                        DateTime dt = DateTime.Now;
                        if (DateTime.TryParse(Convert.ToString(ObjEArticles.dtDimensionValues.Rows[0]["ValidityDate"]), out dt))
                            dtpValidityDate.EditValue = dt;
                        else
                            dtpValidityDate.EditValue = ObjEProject.SubmitDate;
                    }
                }
            }
            catch (Exception ex) { }
        }

        #endregion

        #region Functions

        /// <summary>
        ///  Code to configure stufe and descriptions based on LV raster
        /// </summary>
        private void IntializeLVPositions()
        {
            try
            {
                if (!string.IsNullOrEmpty(ObjEProject.LVRaster))
                {
                    string[] Levels = ObjEProject.LVRaster.Split('.');
                    int Count = Levels.Length;
                    if (Levels[Count - 1].Length == 1)
                        Count -= 1;
                    iRasterCount = Count;
                    if (Count < 3)
                    {
                        txtStufe1Short.Properties.MaxLength = Levels[0].Length;
                        txtPosition.Properties.MaxLength = (Levels[1] + Levels[2]).Length + 1;
                        lciStufe2Short.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        lciStufe2Title.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        lciStufe3Short.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        lciStufe3Title.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        lciStufe4Short.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        lciStufe4Title.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    }
                    else if (Count < 4)
                    {
                        txtStufe1Short.Properties.MaxLength = Levels[0].Length;
                        txtStufe2Short.Properties.MaxLength = Levels[1].Length;
                        txtPosition.Properties.MaxLength = (Levels[2] + Levels[3]).Length + 1;
                        lciStufe2Short.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        lciStufe2Title.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        lciStufe3Short.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        lciStufe3Title.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        lciStufe4Short.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        lciStufe4Title.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    }
                    else if (Count < 5)
                    {
                        txtStufe1Short.Properties.MaxLength = Levels[0].Length;
                        txtStufe2Short.Properties.MaxLength = Levels[1].Length;
                        txtStufe3Short.Properties.MaxLength = Levels[2].Length;
                        txtPosition.Properties.MaxLength = (Levels[3] + Levels[4]).Length + 1;
                        lciStufe2Short.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        lciStufe2Title.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        lciStufe3Short.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        lciStufe3Title.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        lciStufe4Short.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        lciStufe4Title.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    }
                    else
                    {
                        txtStufe1Short.Properties.MaxLength = Levels[0].Length;
                        txtStufe2Short.Properties.MaxLength = Levels[1].Length;
                        txtStufe3Short.Properties.MaxLength = Levels[2].Length;
                        txtStufe4Short.Properties.MaxLength = Levels[3].Length;
                        txtPosition.Properties.MaxLength = (Levels[4] + Levels[5]).Length + 1;
                        lciStufe2Short.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        lciStufe2Title.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        lciStufe3Short.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        lciStufe3Title.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        lciStufe4Short.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        lciStufe4Title.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Code to Parse position details from text boxes to entities while adding or editing a position
        /// </summary>
        private void ParsePositionDetails()
        {
            try
            {
                int iValue = 0;
                decimal dValue = 0;
                DateTime dt = DateTime.Now;
                ObjEPosition.RasterCount = iRasterCount;
                ObjEPosition.ProjectID = ObjEProject.ProjectID;
                ObjEPosition.Stufe1 = txtStufe1Short.Text;
                ObjEPosition.Stufe2 = txtStufe2Short.Text;
                ObjEPosition.Stufe3 = txtStufe3Short.Text;
                ObjEPosition.Stufe4 = txtStufe4Short.Text;
                ObjEPosition.Position = txtPosition.Text;


                if (string.IsNullOrEmpty(txtShortDescription.Text.Trim()) || txtShortDescription.Text.Trim() == "\n")
                {
                    txtShortDescription.Text = string.Empty;
                    if (ObjEPosition.PositionID > 0)
                    {
                        LongDescription = ObjBPosition.GetLongDescription(Convert.ToInt32(tlPositions.FocusedNode["PositionID"]));
                    }
                    if (!string.IsNullOrEmpty(LongDescription))
                    {
                        string plaintext = Utility.GetPlaintext(LongDescription);
                        if (plaintext.Length >= 80)
                        {
                            string _substring = plaintext.Substring(0, 80);
                            txtShortDescription.Text = _substring.Replace("\n", "");
                        }
                        if (plaintext.Length < 80)
                        {
                            string _substring = plaintext;
                            txtShortDescription.Text = _substring.Replace("\n", "");
                        }
                    }
                }

                ObjEPosition.ShortDescription = Utility.GetRTFFormat(txtShortDescription.Text.Trim());

                if (txtPosition.Text == string.Empty)
                    ObjEPosition.Title = txtShortDescription.Text.Trim();

                string stPosKZ = Utility.GetPosKZ(cmbPositionKZ.Text);

                if (!string.IsNullOrEmpty(txtPosition.Text))
                    ObjEPosition.PositionKZ = stPosKZ;
                else if (stPosKZ == "H")
                    ObjEPosition.PositionKZ = stPosKZ;
                else
                {
                    Utility.SetLookupEditValue(cmbPositionKZ, "NG-LV-Normalgruppe");
                    Utility.SetLookupEditValue(cmbPositionKZCD, "NG-LV-Normalgruppe");
                    ObjEPosition.PositionKZ = "NG";
                }
                ObjEPosition.PositionKZID = cmbPositionKZ.EditValue;
                if (int.TryParse(txtDetailKZ.Text, out iValue))
                    ObjEPosition.DetailKZ = iValue;

                if (cmbLVSection.Text == string.Empty)
                    ObjEPosition.LVSection = "HA";
                else
                    ObjEPosition.LVSection = cmbLVSection.Text;

                ObjEPosition.LVSectionID = cmbLVSection.EditValue;


                ObjEPosition.WG = txtWG.Text;
                if (!string.IsNullOrEmpty(ObjEPosition.WG) && string.IsNullOrEmpty(txtWA.Text))
                    ObjEPosition.WA = "0";
                else
                    ObjEPosition.WA = txtWA.Text;
                ObjEPosition.WI = txtWI.Text;

                if (decimal.TryParse(txtMenge.Text, out dValue))
                    ObjEPosition.Menge = dValue;

                if (stPosKZ == "P")
                    ObjEPosition.ME = "Psch";
                else
                    ObjEPosition.ME = cmbME.Text;
                ObjEPosition.Fabricate = txtFabrikate.Text;
                ObjEPosition.LiefrantMA = txtLiefrantMA.Text;
                ObjEPosition.Type = txtType.Text;
                ObjEPosition.LongDescription = LongDescription;

                ObjEPosition.Surcharge_From = txtSurchargeFrom.Text;
                ObjEPosition.Surcharge_To = txtSurchargeTo.Text;

                if (decimal.TryParse(txtSurchargePerME.Text, out dValue))
                    ObjEPosition.Surcharge_Per = dValue;

                if (decimal.TryParse(txtSurchargePerMO.Text, out dValue))
                    ObjEPosition.surchargePercentage_MO = dValue;

                ObjEPosition.ValidityDate = dtpValidityDate.DateTime;

                ObjEPosition.MA = txtMa.Text;
                ObjEPosition.MO = txtMo.Text;

                if (decimal.TryParse(txtMin.Text, out dValue))
                    ObjEPosition.Mins = dValue;

                if (decimal.TryParse(txtFaktor.Text, out dValue))
                    ObjEPosition.Faktor = dValue;

                ObjEPosition.LPMA = Position.MA_ListPrice;
                ObjEPosition.LPMO = Position.MO_ListPrice;

                if (decimal.TryParse(txtMulti1ME.Text, out dValue))
                    ObjEPosition.Multi1MA = dValue;

                if (decimal.TryParse(txtMulti2ME.Text, out dValue))
                    ObjEPosition.Multi2MA = dValue;

                if (decimal.TryParse(txtMulti3ME.Text, out dValue))
                    ObjEPosition.Multi3MA = dValue;

                if (decimal.TryParse(txtMulti4ME.Text, out dValue))
                    ObjEPosition.Multi4MA = dValue;

                if (decimal.TryParse(txtMulti1MO.Text, out dValue))
                    ObjEPosition.Multi1MO = dValue;

                if (decimal.TryParse(txtMulti2MO.Text, out dValue))
                    ObjEPosition.Multi2MO = dValue;

                if (decimal.TryParse(txtMulti3MO.Text, out dValue))
                    ObjEPosition.Multi3MO = dValue;

                if (decimal.TryParse(txtMulti4MO.Text, out dValue))
                    ObjEPosition.Multi4MO = dValue;

                ObjEPosition.EinkaufspreisMA = Position.MA_EP;
                ObjEPosition.EinkaufspreisMO = Position.MO_EP;

                if (decimal.TryParse(txtSelbstkostenMultiME.Text, out dValue))
                    ObjEPosition.SelbstkostenMultiMA = dValue;

                ObjEPosition.SelbstkostenValueMA = Position.MA_SK;

                if (decimal.TryParse(txtSelbstkostenMultiMO.Text, out dValue))
                    ObjEPosition.SelbstkostenMultiMO = dValue;

                ObjEPosition.SelbstkostenValueMO = Position.MO_SK;

                if (decimal.TryParse(txtVerkaufspreisMultiME.Text, out dValue))
                    ObjEPosition.VerkaufspreisMultiMA = dValue;

                ObjEPosition.VerkaufspreisValueMA = Position.MA_VK;

                if (decimal.TryParse(txtVerkaufspreisMultiMO.Text, out dValue))
                    ObjEPosition.VerkaufspreisMultiMO = dValue;

                ObjEPosition.VerkaufspreisValueMO = Position.MO_VK;

                if (decimal.TryParse(txtStdSatz.Text, out dValue))
                    ObjEPosition.StdSatz = dValue;

                ObjEPosition.PreisText = txtPreisText.Text;

                ObjEPosition.EinkaufspreisLockMA = Convert.ToBoolean(chkEinkaufspreisME.CheckState);
                ObjEPosition.EinkaufspreisLockMO = Convert.ToBoolean(chkEinkaufspreisMO.CheckState);
                ObjEPosition.SelbstkostenLockMA = Convert.ToBoolean(chkSelbstkostenME.CheckState);
                ObjEPosition.SelbstkostenLockMO = Convert.ToBoolean(chkSelbstkostenMO.CheckState);
                ObjEPosition.VerkaufspreisLockMA = Convert.ToBoolean(chkVerkaufspreisME.CheckState);
                ObjEPosition.VerkaufspreisLockMO = Convert.ToBoolean(chkVerkaufspreisMO.CheckState);

                ObjEPosition.Dim1 = txtDim1.Text;
                ObjEPosition.Dim2 = txtDim2.Text;
                ObjEPosition.Dim3 = txtDim3.Text;
                ObjEPosition.Dim = txtDim.Text;

                ObjEPosition.DocuwareLink1 = _DocuwareLink1;
                ObjEPosition.DocuwareLink2 = _DocuwareLink2;
                ObjEPosition.DocuwareLink3 = _DocuwareLink3;

                ObjEPosition.LVStatus = cmbLVStatus.Text;
                ObjEPosition.LVStatusID = cmbLVStatus.EditValue;

                if (decimal.TryParse(txtGrandTotalME.Text, out dValue))
                    ObjEPosition.GrandTotalME = dValue;
                else
                    ObjEPosition.GrandTotalME = 0;



                if (decimal.TryParse(txtGrandTotalMO.Text, out dValue) && dValue != 0)
                    ObjEPosition.GrandTotalMO = dValue;
                else
                    ObjEPosition.GrandTotalMO = 0;

                if (decimal.TryParse(txtFinalGB.Text, out dValue) && dValue != 0)
                    ObjEPosition.FinalGB = dValue;
                else
                    ObjEPosition.FinalGB = 0;
                if (decimal.TryParse(txtEP.Text, out dValue) && dValue != 0)
                    ObjEPosition.EP = dValue;
                else
                    ObjEPosition.EP = 0;
                ObjEPosition.SNO = iSNO;
                if (decimal.TryParse(txtDiscount.Text, out dValue))
                    ObjEPosition.Discount = dValue;
                ObjEPosition.MontageEntry = Convert.ToBoolean(ChkManualMontageentry.EditValue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Code to fetch position list from database and bind to treelist
        /// </summary>
        private void BindPositionData()
        {
            try
            {
                if (ObjEProject.ActualLvs < 1)
                {
                    btnModify.Enabled = false;
                }
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                SplashScreenManager.Default.SetWaitFormDescription("Loading...");
                ObjBPosition.GetPositionList(ObjEPosition, ObjEProject.ProjectID);
                if (ObjEPosition.dsPositionList != null)
                {
                    int OldPosID = 0;
                    if (tlPositions.DataSource != null && tlPositions.FocusedNode != null)
                        int.TryParse(Convert.ToString(tlPositions.FocusedNode["PositionID"]), out OldPosID);
                    CalculatePositions(ObjEPosition.dsPositionList.Tables[0], "GB");
                    tlPositions.DataSource = ObjEPosition.dsPositionList;
                    tlPositions.DataMember = "Positions";
                    tlPositions.ParentFieldName = "Parent_OZ";
                    tlPositions.KeyFieldName = "PositionID";
                    tlPositions.ForceInitialize();
                    tlPositions.ExpandAll();
                    if (OldPosID != 0)
                        SetFocus(OldPosID, tlPositions);
                }
                SplashScreenManager.CloseForm(false);
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Utility.ShowError(ex);
            }
        }

        /// <summary>
        /// Code to focus a node on treelist using column name and key value
        /// </summary>
        /// <param name="PositionID"></param>
        /// <param name="tl"></param>
        private void SetFocus(object PositionID, TreeList tl)
        {
            try
            {
                TreeListNode Nodetofocus = null;
                Nodetofocus = tl.FindNodeByKeyID(PositionID);
                tl.SetFocusedNode(Nodetofocus);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Code to clear LV details controls values
        /// </summary>
        public void ClearingValues()
        {
            txtSurchargePerME.Enabled = false;
            txtSurchargePerMO.Enabled = false;
            txtDiscount.Enabled = false;
            txtSurchargeFrom.Enabled = false;
            txtSurchargeTo.Enabled = false;

            txtSurchargePerME.Text = "";
            txtSurchargePerMO.Text = "";
            txtSurchargeFrom.Text = "";
            txtSurchargeTo.Text = "";
            txtDiscount.Text = "";
        }

        /// <summary>
        /// Code to calculate Sum positions value in a treelist
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="strField"></param>
        private void CalculatePositions(DataTable dt, string strField)
        {
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string strPosition = Convert.ToString(dr["PositionKZ"]).ToLower();
                    if (strPosition == "zs")
                    {
                        string Value = GetTotalValue(dt, dr, strField, strPosition);
                        dr[strField] = Value;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        ///  Internal function to calculate Sum position values
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dr"></param>
        /// <param name="strField"></param>
        /// <param name="strPosition"></param>
        /// <returns></returns>
        private string GetTotalValue(DataTable dt, DataRow dr, string strField, string strPosition)
        {
            decimal TotalValue = 0;
            try
            {

                int IParent = 0;
                if (int.TryParse(Convert.ToString(dr["Parent_OZ"]), out IParent))
                {

                    string strFromOZ = dr["surchargefrom"] == DBNull.Value ? "" : dr["surchargefrom"].ToString();
                    string strToOZ = dr["surchargeto"] == DBNull.Value ? "" : dr["surchargeto"].ToString();

                    DataRow[] FilteredFromRow = dt.Select("Position_OZ = '" + strFromOZ + "'");
                    DataRow[] FilteredToRow = dt.Select("Position_OZ = '" + strToOZ + "'");

                    int ifromValue = 0;
                    int itoValue = 0;

                    if (FilteredFromRow != null && FilteredFromRow.Count() > 0)
                        ifromValue = Convert.ToInt32(FilteredFromRow[0]["SNO"]);

                    if (FilteredToRow != null && FilteredToRow.Count() > 0)
                        itoValue = Convert.ToInt32(FilteredToRow[0]["SNO"]);

                    object Obj = null;
                    Obj = dt.Compute("SUM(" + strField + ")", "SNO >=" + ifromValue +
                          "And SNO <=" + itoValue + "And DetailKZ = 0 AND Parent_OZ =" + IParent +
                          "AND PositionKZ <> 'A' AND PositionKZ <> 'E' AND PositionKZ <> 'ZS'");
                    if (!decimal.TryParse(Convert.ToString(Obj), out TotalValue))
                        TotalValue = 0;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return TotalValue.ToString("n2");
        }

        /// <summary>
        ///  Code to set rouning rule on treelist column
        /// </summary>
        private void SetRoundingPriceforColumn()
        {
            string _Mask = "n" + ObjEProject.RoundingPrice.ToString();
            tlPositions.Columns["MA_einkaufspreis"].Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            tlPositions.Columns["MA_einkaufspreis"].Format.FormatString = _Mask;

            tlPositions.Columns["MA_selbstkosten"].Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            tlPositions.Columns["MA_selbstkosten"].Format.FormatString = _Mask;

            tlPositions.Columns["MA_verkaufspreis"].Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            tlPositions.Columns["MA_verkaufspreis"].Format.FormatString = _Mask;

            tlPositions.Columns["MO_Einkaufspreis"].Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            tlPositions.Columns["MO_Einkaufspreis"].Format.FormatString = _Mask;

            tlPositions.Columns["MO_selbstkosten"].Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            tlPositions.Columns["MO_selbstkosten"].Format.FormatString = _Mask;

            tlPositions.Columns["MO_verkaufspreis"].Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            tlPositions.Columns["MO_verkaufspreis"].Format.FormatString = _Mask;

            tlPositions.Columns["EP"].Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            tlPositions.Columns["EP"].Format.FormatString = _Mask;

            tlPositions.Columns["GB"].Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            tlPositions.Columns["GB"].Format.FormatString = _Mask;
        }

        /// <summary>
        /// Code to save Position or porject specific article
        /// </summary>
        private void SavePositionArticle()
        {
            try
            {
                ObjEPosition.ProjectID = ObjEProject.ProjectID;
                ObjEPosition.WG = txtWG.Text;
                ObjEPosition.WA = txtWA.Text;
                ObjEPosition.WI = txtWI.Text;
                ObjEPosition.UserID = Utility.UserID;
                DPosition ObjDPosition = new DPosition();
                ObjDPosition.SavePositionArticle(ObjEPosition);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        ///  Code to Refresh treelist with Database values
        /// </summary>
        /// <param name="OEPosition"></param>
        private void RefreshTreelist(EPosition OEPosition)
        {
            try
            {
                Log.Info("Refresh Started");
                _ISChange = false;
                TreeListNode parentnode = tlPositions.FindNodeByKeyID(ObjEPosition.drnewrow["Parent_OZ"]);
                if (_IsNewMode)
                {
                    tlPositions.AppendNode(ObjEPosition.drnewrow, parentnode);
                    SetFocus(OEPosition.PositionID, tlPositions);
                }
                else
                {
                    if (parentnode == tlPositions.FocusedNode.ParentNode)
                    {
                        tlPositions.FocusedNode.SetValue("PositionID", ObjEPosition.drnewrow["PositionID"]);
                        tlPositions.FocusedNode.SetValue("Position_OZ", ObjEPosition.drnewrow["Position_OZ"]);
                        tlPositions.FocusedNode.SetValue("Parent_OZ", ObjEPosition.drnewrow["Parent_OZ"]);
                        tlPositions.FocusedNode.SetValue("Title", ObjEPosition.drnewrow["Title"]);
                        tlPositions.FocusedNode.SetValue("ShortDescription", ObjEPosition.drnewrow["ShortDescription"]);
                        tlPositions.FocusedNode.SetValue("PositionKZ", ObjEPosition.drnewrow["PositionKZ"]);
                        tlPositions.FocusedNode.SetValue("LVSection", ObjEPosition.drnewrow["LVSection"]);
                        tlPositions.FocusedNode.SetValue("WG", ObjEPosition.drnewrow["WG"]);
                        tlPositions.FocusedNode.SetValue("WA", ObjEPosition.drnewrow["WA"]);
                        tlPositions.FocusedNode.SetValue("WI", ObjEPosition.drnewrow["WI"]);
                        tlPositions.FocusedNode.SetValue("Menge", ObjEPosition.drnewrow["Menge"]);
                        tlPositions.FocusedNode.SetValue("ME", ObjEPosition.drnewrow["ME"]);
                        tlPositions.FocusedNode.SetValue("Fabricate", ObjEPosition.drnewrow["Fabricate"]);
                        tlPositions.FocusedNode.SetValue("LiefrantMA", ObjEPosition.drnewrow["LiefrantMA"]);
                        tlPositions.FocusedNode.SetValue("Type", ObjEPosition.drnewrow["Type"]);
                        tlPositions.FocusedNode.SetValue("LVStatus", ObjEPosition.drnewrow["LVStatus"]);
                        tlPositions.FocusedNode.SetValue("surchargefrom", ObjEPosition.drnewrow["surchargefrom"]);
                        tlPositions.FocusedNode.SetValue("surchargeto", ObjEPosition.drnewrow["surchargeto"]);
                        tlPositions.FocusedNode.SetValue("surchargePercentage", ObjEPosition.drnewrow["surchargePercentage"]);
                        tlPositions.FocusedNode.SetValue("surchargePercentage_MO", ObjEPosition.drnewrow["surchargePercentage_MO"]);
                        tlPositions.FocusedNode.SetValue("DetailKZ", ObjEPosition.drnewrow["DetailKZ"]);
                        tlPositions.FocusedNode.SetValue("EP", ObjEPosition.drnewrow["EP"]);
                        tlPositions.FocusedNode.SetValue("GB", ObjEPosition.drnewrow["GB"]);
                        tlPositions.FocusedNode.SetValue("validitydate", ObjEPosition.drnewrow["validitydate"]);
                        tlPositions.FocusedNode.SetValue("MA", ObjEPosition.drnewrow["MA"]);
                        tlPositions.FocusedNode.SetValue("MO", ObjEPosition.drnewrow["MO"]);
                        tlPositions.FocusedNode.SetValue("MINUTES", ObjEPosition.drnewrow["MINUTES"]);
                        tlPositions.FocusedNode.SetValue("Faktor", ObjEPosition.drnewrow["Faktor"]);
                        tlPositions.FocusedNode.SetValue("MA_listprice", ObjEPosition.drnewrow["MA_listprice"]);
                        tlPositions.FocusedNode.SetValue("MO_listprice", ObjEPosition.drnewrow["MO_listprice"]);
                        tlPositions.FocusedNode.SetValue("MA_Multi1", ObjEPosition.drnewrow["MA_Multi1"]);
                        tlPositions.FocusedNode.SetValue("MA_multi2", ObjEPosition.drnewrow["MA_multi2"]);
                        tlPositions.FocusedNode.SetValue("MA_multi3", ObjEPosition.drnewrow["MA_multi3"]);
                        tlPositions.FocusedNode.SetValue("MA_multi4", ObjEPosition.drnewrow["MA_multi4"]);
                        tlPositions.FocusedNode.SetValue("MA_einkaufspreis", ObjEPosition.drnewrow["MA_einkaufspreis"]);
                        tlPositions.FocusedNode.SetValue("MA_selbstkosten", ObjEPosition.drnewrow["MA_selbstkosten"]);
                        tlPositions.FocusedNode.SetValue("MA_selbstkostenMulti", ObjEPosition.drnewrow["MA_selbstkostenMulti"]);
                        tlPositions.FocusedNode.SetValue("MA_verkaufspreis", ObjEPosition.drnewrow["MA_verkaufspreis"]);
                        tlPositions.FocusedNode.SetValue("MA_verkaufspreis_Multi", ObjEPosition.drnewrow["MA_verkaufspreis_Multi"]);
                        tlPositions.FocusedNode.SetValue("MO_multi1", ObjEPosition.drnewrow["MO_multi1"]);
                        tlPositions.FocusedNode.SetValue("MO_multi2", ObjEPosition.drnewrow["MO_multi2"]);
                        tlPositions.FocusedNode.SetValue("MO_multi3", ObjEPosition.drnewrow["MO_multi3"]);
                        tlPositions.FocusedNode.SetValue("MO_multi4", ObjEPosition.drnewrow["MO_multi4"]);
                        tlPositions.FocusedNode.SetValue("MO_Einkaufspreis", ObjEPosition.drnewrow["MO_Einkaufspreis"]);
                        tlPositions.FocusedNode.SetValue("MO_selbstkostenMulti", ObjEPosition.drnewrow["MO_selbstkostenMulti"]);
                        tlPositions.FocusedNode.SetValue("MO_selbstkosten", ObjEPosition.drnewrow["MO_selbstkosten"]);
                        tlPositions.FocusedNode.SetValue("MO_verkaufspreisMulti", ObjEPosition.drnewrow["MO_verkaufspreisMulti"]);
                        tlPositions.FocusedNode.SetValue("MO_verkaufspreis", ObjEPosition.drnewrow["MO_verkaufspreis"]);
                        tlPositions.FocusedNode.SetValue("std_satz", ObjEPosition.drnewrow["std_satz"]);
                        tlPositions.FocusedNode.SetValue("PreisText", ObjEPosition.drnewrow["PreisText"]);
                        tlPositions.FocusedNode.SetValue("MA_einkaufspreis_lck", ObjEPosition.drnewrow["MA_einkaufspreis_lck"]);
                        tlPositions.FocusedNode.SetValue("MA_selbstkosten_lck", ObjEPosition.drnewrow["MA_selbstkosten_lck"]);
                        tlPositions.FocusedNode.SetValue("MA_verkaufspreis_lck", ObjEPosition.drnewrow["MA_verkaufspreis_lck"]);
                        tlPositions.FocusedNode.SetValue("MO_Einkaufspreis_lck", ObjEPosition.drnewrow["MO_Einkaufspreis_lck"]);
                        tlPositions.FocusedNode.SetValue("MO_selbstkosten_lck", ObjEPosition.drnewrow["MO_selbstkosten_lck"]);
                        tlPositions.FocusedNode.SetValue("MO_verkaufspreis_lck", ObjEPosition.drnewrow["MO_verkaufspreis_lck"]);
                        tlPositions.FocusedNode.SetValue("A", ObjEPosition.drnewrow["A"]);
                        tlPositions.FocusedNode.SetValue("B", ObjEPosition.drnewrow["B"]);
                        tlPositions.FocusedNode.SetValue("L", ObjEPosition.drnewrow["L"]);
                        tlPositions.FocusedNode.SetValue("DocuwareLink1", ObjEPosition.drnewrow["DocuwareLink1"]);
                        tlPositions.FocusedNode.SetValue("DocuwareLink2", ObjEPosition.drnewrow["DocuwareLink2"]);
                        tlPositions.FocusedNode.SetValue("DocuwareLink3", ObjEPosition.drnewrow["DocuwareLink3"]);
                        tlPositions.FocusedNode.SetValue("GrandTotalME", ObjEPosition.drnewrow["GrandTotalME"]);
                        tlPositions.FocusedNode.SetValue("GrandTotalMO", ObjEPosition.drnewrow["GrandTotalMO"]);
                        tlPositions.FocusedNode.SetValue("FinalGB", ObjEPosition.drnewrow["FinalGB"]);
                        tlPositions.FocusedNode.SetValue("SNO", ObjEPosition.drnewrow["SNO"]);
                        tlPositions.FocusedNode.SetValue("HaveDetailkz", ObjEPosition.drnewrow["HaveDetailkz"]);
                        tlPositions.FocusedNode.SetValue("Discount", ObjEPosition.drnewrow["Discount"]);
                        tlPositions.FocusedNode.SetValue("DiscountPositionID", ObjEPosition.drnewrow["DiscountPositionID"]);
                        tlPositions.FocusedNode.SetValue("SurchargePosID", ObjEPosition.drnewrow["SurchargePosID"]);
                        tlPositions.FocusedNode.SetValue("dim", ObjEPosition.drnewrow["dim"]);
                        tlPositions.FocusedNode.SetValue("OZID", ObjEPosition.drnewrow["OZID"]);
                        tlPositions.FocusedNode.SetValue("MontageEntry", ObjEPosition.drnewrow["MontageEntry"]);
                        tlPositions.FocusedNode.SetValue("WGID", ObjEPosition.drnewrow["WGID"]);
                        tlPositions.FocusedNode.SetValue("LVSectionID", ObjEPosition.drnewrow["LVSectionID"]);
                        tlPositions.FocusedNode.SetValue("LVStatusID", ObjEPosition.drnewrow["LVStatusID"]);
                        tlPositions.FocusedNode.SetValue("PositionKZID", ObjEPosition.drnewrow["PositionKZID"]);
                    }
                }
                _ISChange = true;
                Log.Info("Refresh Completed");
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Code to set defualt values to cost details controls
        /// </summary>
        public void CostDetailsDefaultValues()
        {
            txtLPMe.Text = string.Empty;
            txtMulti1ME.EditValue = 1;
            txtMulti1MO.EditValue = 1;
            txtMulti2ME.EditValue = 1;
            txtMulti2MO.EditValue = 1;
            txtMulti3ME.EditValue = 1;
            txtMulti3MO.EditValue = 1;
            txtMulti4ME.EditValue = 1;
            txtMulti4MO.EditValue = 1;
            txtMin.EditValue = 0;
            txtFaktor.EditValue = 1;
            txtSelbstkostenMultiME.EditValue = 1;
            txtSelbstkostenMultiMO.EditValue = 1;
            txtVerkaufspreisMultiME.EditValue = 1;
            txtVerkaufspreisMultiMO.EditValue = 1;
            txtStdSatz.EditValue = ObjEProject.InternX;
        }

        /// <summary>
        /// Code to  Set LV details tab in new mode 
        /// </summary>
        /// <param name="strPositiontype"></param>
        private void CreateNewPosition(string strPositiontype = "")
        {
            try
            {
                ChkManualMontageentry.EditValue = false;
                txtPosition.Enabled = true;
                ObjEPosition.PositionID = -1;
                if (strPositiontype == string.Empty)
                {
                    chkCreateNew.Enabled = true;
                    txtMo.Text = "X";
                    txtMa.Text = "X";
                    _IsNewMode = true;
                    EnableDisableLVAndCostDetails(true, true, true, false);
                    CostDetailsDefaultValues();
                    EnableDisableButtons(true, false, true, false, false);
                    bool _isSuggest = true;
                    if (RequiredPositionFieldsforTitle != null && RequiredPositionFieldsforTitle.Count > 0)
                    {
                        foreach (Control c in RequiredPositionFieldsforTitle)
                        {
                            if (c is TextEdit && string.IsNullOrEmpty(c.Text))
                            {
                                _isSuggest = false;
                                continue;
                            }
                        }
                    }
                    if (_isSuggest)
                        txtPosition.Text = SuggestOZ(PrepareOZ());
                    _DocuwareLink1 = string.Empty;
                    _DocuwareLink2 = string.Empty;
                    _DocuwareLink3 = string.Empty;
                    txtShortDescription.Text = "";
                    txtShortDescriptionCD.Text = "";
                    txtPreisText.Text = "";
                    txtFabrikate.Text = "";
                    txtLiefrantMA.Text = "";
                    txtType.Text = "";
                    txtTypeCD.Text = "";
                    cmbCDME.Text = "";
                    Utility.SetLookupEditValue(cmbPositionKZ, "N-Normalposition");
                    txtMenge.Text = "1";
                    txtMengeCD.Text = "1";
                    _FiredFromEvent = true;
                    txtLVPosition.Text = string.Empty;
                    txtLVPositionCD.Text = string.Empty;
                    FormatLVFields();
                    btnLongDescription.Enabled = true;
                    tlPositions.OptionsBehavior.ReadOnly = true;
                    LongDescription = string.Empty;
                    cmbPositionKZ.Enabled = true;
                    cmbPositionKZ_TextChanged(null, null);
                    if (!_IsAddhoc)
                    {
                        iSNO = -1;
                    }
                    if (!string.IsNullOrEmpty(ObjEProject.CommissionNumber))
                    {
                        Utility.SetLookupEditValue(cmbLVStatus, "B");
                    }
                }
                else if (strPositiontype.ToLower() == "h")
                {
                    chkCreateNew.EditValue = false;
                    chkCreateNew.Enabled = false;
                    txtMo.Text = "";
                    txtMa.Text = "";
                    _IsNewMode = true;
                    EnableDisableLVAndCostDetails(true, true, true, false);
                    CostDetailsDefaultValues();
                    EnableDisableButtons(true, false, true, false, false);
                    _DocuwareLink1 = string.Empty;
                    _DocuwareLink2 = string.Empty;
                    _DocuwareLink3 = string.Empty;
                    txtShortDescription.Text = "";
                    txtShortDescriptionCD.Text = "";
                    txtPreisText.Text = "";
                    txtFabrikate.Text = "";
                    txtLiefrantMA.Text = "";
                    txtType.Text = "";
                    txtTypeCD.Text = "";
                    cmbCDME.Text = "";
                    Utility.SetLookupEditValue(cmbPositionKZ, "H-Hinweistext");
                    txtMenge.Text = "";
                    txtMengeCD.Text = "";
                    txtLVPosition.Text = string.Empty;
                    _FiredFromEvent = true;
                    txtLVPositionCD.Text = string.Empty;
                    txtStufe1Short.Text = "";
                    txtStufe2Short.Text = "";
                    txtStufe3Short.Text = "";
                    txtStufe4Short.Text = "";
                    txtPosition.Text = "";
                    FormatLVFields();
                    btnLongDescription.Enabled = true;
                    tlPositions.OptionsBehavior.ReadOnly = true;
                    LongDescription = string.Empty;
                    cmbPositionKZ.Enabled = true;
                    cmbPositionKZ_TextChanged(null, null);
                    if (tlPositions != null && tlPositions.FocusedNode != null)
                    {
                        string strSNO = tlPositions.FocusedNode["SNO"].ToString();
                        if (!int.TryParse(strSNO, out iSNO))
                            iSNO = 1;
                    }
                }
                else if (strPositiontype.ToLower() == "n")
                {
                    chkCreateNew.Enabled = true;
                    txtMo.Text = "X";
                    txtMa.Text = "X";
                    _IsNewMode = true;
                    EnableDisableLVAndCostDetails(true, true, true, false);
                    CostDetailsDefaultValues();
                    EnableDisableButtons(true, false, true, false, false);
                    bool _isSuggest = true;
                    if (RequiredPositionFieldsforTitle != null && RequiredPositionFieldsforTitle.Count > 0)
                    {
                        foreach (Control c in RequiredPositionFieldsforTitle)
                        {
                            if (c is TextEdit && string.IsNullOrEmpty(c.Text))
                            {
                                _isSuggest = false;
                                continue;
                            }
                        }
                    }
                    if (_isSuggest)
                        txtPosition.Text = SuggestOZ(PrepareOZ());
                    _DocuwareLink1 = string.Empty;
                    _DocuwareLink2 = string.Empty;
                    _DocuwareLink3 = string.Empty;
                    txtShortDescription.Text = "";
                    txtShortDescriptionCD.Text = "";
                    txtPreisText.Text = "";
                    txtFabrikate.Text = "";
                    txtLiefrantMA.Text = "";
                    txtType.Text = "";
                    txtTypeCD.Text = "";
                    cmbCDME.Text = "";
                    Utility.SetLookupEditValue(cmbLVSection, "N-Normalposition");
                    txtMenge.Text = "1";
                    txtMengeCD.Text = "1";
                    _FiredFromEvent = true;
                    txtLVPosition.Text = string.Empty;
                    txtLVPositionCD.Text = string.Empty;
                    FormatLVFields();
                    btnLongDescription.Enabled = true;
                    tlPositions.OptionsBehavior.ReadOnly = true;
                    LongDescription = string.Empty;
                    cmbPositionKZ.Enabled = true;
                    cmbPositionKZ_TextChanged(null, null);
                    if (tlPositions != null && tlPositions.FocusedNode != null)
                    {
                        string strSNO = tlPositions.FocusedNode["SNO"].ToString();
                        if (!int.TryParse(strSNO, out iSNO))
                        {
                            iSNO = -1;
                        }
                        else
                        {
                            iSNO = iSNO - 1;
                        }
                    }
                }
                chkVerkaufspreisME.Checked = false;
                chkVerkaufspreisMO.Checked = false;
                chkSelbstkostenME.Checked = false;
                chkSelbstkostenMO.Checked = false;
                chkEinkaufspreisME.Checked = false;
                chkEinkaufspreisMO.Checked = false;
                txtWG.Text = string.Empty;
                txtWA.Text = string.Empty;
                txtWI.Text = string.Empty;
                txtWGCD.Text = string.Empty;
                txtWACD.Text = string.Empty;
                txtWICD.Text = string.Empty;
                txtDim1.Text = string.Empty;
                txtDim2.Text = string.Empty;
                txtDim3.Text = string.Empty;
                txtDim.Text = string.Empty;
                txtMin.Text = "0";
                txtLPMe.Text = string.Empty;
                txtDetailKZ.Text = "0";
                txtEP.Text = string.Empty;
                txtFinalGB.Text = string.Empty;
                dtpValidityDate.EditValue = ObjEProject.SubmitDate;
                if (Utility.LVsectionAddAccess == "7" || Utility.LVsectionAddAccess == "9")
                    Utility.SetLookupEditValue(cmbLVSection, "HA");
                else
                {
                    if (Utility.LVDetailsAccess != "8" && !string.IsNullOrEmpty(ObjEProject.CommissionNumber))
                        Utility.SetLookupEditValue(cmbLVSection, "NT");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        ///  Code to set Mask for Multi textboxes
        /// </summary>
        private void SetMaskForMaulties()
        {
            try
            {
                rpiMulti1.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
                rpiMulti1.Mask.EditMask = @"\d{1,2}(\R.\d{0,3})";
                rpiMulti1.Mask.UseMaskAsDisplayFormat = true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Code to set enable property to Cost details text boxes based on position KZ
        /// </summary>
        /// <param name="Lcvalue"></param>
        /// <param name="LcCostvalue"></param>
        /// <param name="_IsNew"></param>
        /// <param name="_IsEdit"></param>
        private void EnableDisableLVAndCostDetails(bool Lcvalue, bool LcCostvalue, bool _IsNew, bool _IsEdit)
        {
            _IsNewMode = _IsNew;
            _IsEditMode = _IsEdit;
        }

        /// <summary>
        /// Code to set Enable proerty to cost details button based on Positon KZ and Mode
        /// </summary>
        /// <param name="tnew"></param>
        /// <param name="tmodify"></param>
        /// <param name="tsave"></param>
        /// <param name="tnext"></param>
        /// <param name="tprev"></param>
        private void EnableDisableButtons(bool tnew, bool tmodify, bool tsave, bool tnext, bool tprev)
        {
            btnModify.Enabled = tmodify;
            btnNext.Enabled = tnext;
            btnPrevious.Enabled = tprev;
        }

        /// <summary>
        /// Formatting fields for Sum Position
        /// </summary>
        private void FormatFieldsForSum()
        {
            txtWG.Enabled = false;
            txtWA.Enabled = false;
            txtWI.Enabled = false;
            txtMenge.Enabled = false;
            txtFabrikate.Enabled = false;
            txtLiefrantMA.Enabled = false;
            txtType.Enabled = false;
            txtDetailKZ.Enabled = false;
            cmbCDME.Enabled = false;
            txtWG.Text = string.Empty;
            txtWA.Text = string.Empty;
            txtWI.Text = string.Empty;
            txtMenge.Text = string.Empty;
            txtFabrikate.Text = string.Empty;
            txtLiefrantMA.Text = "";
            txtType.Text = string.Empty;
            txtDetailKZ.Text = "0";
            cmbCDME.Text = string.Empty;
        }

        /// <summary>
        /// Formatting fields for Normal Position
        /// </summary>
        private void FormatFieldsForNormal()
        {
            txtWG.Enabled = true;
            txtWA.Enabled = true;
            txtWI.Enabled = true;
            txtMenge.Enabled = true;
            txtFabrikate.Enabled = true;
            txtLiefrantMA.Enabled = true;
            txtType.Enabled = true;
            txtDetailKZ.Enabled = true;
            cmbCDME.Enabled = true;
        }

        /// <summary>
        /// Code to extract Parent OZ from Position OZ string
        /// </summary>
        /// <returns></returns>
        private string PrepareOZ()
        {
            try
            {
                StringBuilder strParentOZ = new StringBuilder();
                //Checking stufe existence
                if (!string.IsNullOrEmpty(txtStufe1Short.Text.Trim())) //1
                {
                    strParentOZ.Append(txtStufe1Short.Text + ".");//1
                    if (!string.IsNullOrEmpty(txtStufe2Short.Text.Trim()))//1
                    {
                        strParentOZ.Append(txtStufe2Short.Text + ".");//1.1
                        if (!string.IsNullOrEmpty(txtStufe3Short.Text.Trim()))//1
                        {
                            strParentOZ.Append(txtStufe3Short.Text + ".");//1.1.1
                            if (!string.IsNullOrEmpty(txtStufe4Short.Text.Trim()))//1
                                strParentOZ.Append(txtStufe4Short.Text + ".");//1.1.1.1
                        }
                    }
                }
                return Utility.PrepareOZ(strParentOZ.ToString(), ObjEProject.LVRaster);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        ///  Code find the next OZ number based on other OZs under that parent
        /// </summary>
        /// <param name="strParent"></param>
        /// <returns></returns>
        private string SuggestOZ(string strParent)
        {
            string strNewOZ = string.Empty;
            try
            {
                if (ObjEPosition.dsPositionList != null)
                {
                    DataTable dt = ObjEPosition.dsPositionList.Tables[0];
                    DataRow[] tPosition_Id = dt.Select("Position_OZ='" + strParent + "'");
                    if (tPosition_Id != null && tPosition_Id.Count() > 0)
                    {
                        string tResult = tPosition_Id[0]["PositionID"] == DBNull.Value ? "" : tPosition_Id[0]["PositionID"].ToString();
                        if (!string.IsNullOrEmpty(tResult))
                        {
                            strNewOZ = Convert.ToString(dt.Compute("MAX(OZID)", "Parent_OZ =" + tResult));
                            int iValue = 0;
                            if (int.TryParse(strNewOZ, out iValue))
                            {
                                strNewOZ = Convert.ToString((((iValue / ObjEProject.LVSprunge) * ObjEProject.LVSprunge) + ObjEProject.LVSprunge));
                            }
                            else
                                strNewOZ = Convert.ToString(ObjEProject.LVSprunge);
                        }
                        else
                            strNewOZ = Convert.ToString(ObjEProject.LVSprunge);
                    }
                    else
                    {
                        string[] strArr = ObjEProject.LVRaster.Split('.');
                        int Count = strArr.Count();
                        if (strArr != null)
                        {
                            if (Count > 3 && string.IsNullOrEmpty(txtStufe2Short.Text))//99.99.1111.9
                                strNewOZ = "";
                            else if (Count > 4 && string.IsNullOrEmpty(txtStufe3Short.Text))//99.99.99.1111.9
                                strNewOZ = "";
                            else if (Count > 5 && string.IsNullOrEmpty(txtStufe4Short.Text))//99.99.99.99.1111.9
                                strNewOZ = "";
                            else
                                strNewOZ = ObjEProject.LVSprunge.ToString();
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return strNewOZ;
        }

        /// <summary>
        /// Code to find the FromOZ under selected parent while saving Surcharge or sum positions
        /// </summary>
        /// <param name="strParentOZ"></param>
        /// <param name="strPositionKZ"></param>
        /// <returns></returns>
        private string FromOZ(string strParentOZ, string strPositionKZ)
        {
            string strFromOZ = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                dt = ObjEPosition.dsPositionList.Tables[0].Copy();
                DataRow[] tPosition_Id = dt.Select("Position_OZ='" + strParentOZ + "'");
                string tResult = tPosition_Id[0]["PositionID"] == DBNull.Value ? "" : tPosition_Id[0]["PositionID"].ToString();
                object Min_Identity = null;
                string MinValue = string.Empty;

                if (strPositionKZ.ToLower() == "zs")
                    Min_Identity = dt.Compute("MIN(SNO)", "Parent_OZ =" + tResult + " AND (PositionKZ IN ('N','M','P','K','W','S','B','G','Z'))");
                else if (strPositionKZ.ToLower() == "z")
                    Min_Identity = dt.Compute("MIN(SNO)", "Parent_OZ =" + tResult + " AND (PositionKZ IN ('N','M','P','K','W','S','B','G'))");

                if (Min_Identity != DBNull.Value)
                {
                    strFromOZ = Convert.ToString(dt.Compute("MIN(Position_OZ)", "SNO =" + Min_Identity + " AND Parent_OZ =" + tResult));
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return strFromOZ;
        }

        /// <summary>
        /// Code to find TOOZ under selected parent while saving surcharge or sum positions
        /// </summary>
        /// <param name="strParentOZ"></param>
        /// <param name="strPositionKZ"></param>
        /// <returns></returns>
        private string ToOZ(string strParentOZ, string strPositionKZ)
        {
            string strToOZ = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                dt = ObjEPosition.dsPositionList.Tables[0];
                DataRow[] tPosition_Id = dt.Select("Position_OZ='" + strParentOZ + "'");
                string tResult = tPosition_Id[0]["PositionID"] == DBNull.Value ? "" : tPosition_Id[0]["PositionID"].ToString();
                object Max_Identity = null;

                if (strPositionKZ.ToLower() == "zs")
                    Max_Identity = dt.Compute("MAX(SNO)", "Parent_OZ =" + tResult + " AND (PositionKZ IN ('N','M','P','K','W','S','B','G','Z'))");
                else if (strPositionKZ.ToLower() == "z")
                    Max_Identity = dt.Compute("MAX(SNO)", "Parent_OZ =" + tResult + " AND (PositionKZ IN ('N','M','P','K','W','S','B','G'))");

                if (Max_Identity != DBNull.Value)
                {
                    strToOZ = Convert.ToString(dt.Compute("MIN(Position_OZ)", "SNO =" + Max_Identity + " AND Parent_OZ =" + tResult));
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return strToOZ;
        }

        /// <summary>
        ///  Code format LV fields
        /// </summary>
        private void FormatLVFields()
        {
            try
            {
                if (string.IsNullOrEmpty(ObjEProject.CommissionNumber))
                {
                    cmbLVStatus.Enabled = false;
                    Utility.SetLookupEditValue(cmbLVSection, "HA");
                }
                else
                {
                    if (RequiredPositionFields != null && RequiredPositionFields.Count > 0)
                        RequiredPositionFields.Clear();
                    cmbLVStatus.Enabled = true;
                    RequiredPositionFields.Add(cmbLVSection);
                    RequiredPositionFields.Add(cmbLVStatus);
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        /// <summary>
        /// Code to delete selected position
        /// </summary>
        public void DeletePosition()
        {
            string strConfirmation = "";
            try
            {
                if (tcProjectDetails.SelectedTabPage != tbLVDetails)
                    return;
                if (tlPositions.FocusedNode != null && tlPositions.FocusedNode["PositionID"] != null)
                {
                    if (tlPositions.FocusedNode.HasChildren)
                        return;
                    int iValue = 0;
                    string Pos = Convert.ToString(tlPositions.FocusedNode.GetDisplayText("Position_OZ"));
                    string _PosKZ = Convert.ToString(tlPositions.FocusedNode.GetDisplayText("PositionKZ"));
                    if (int.TryParse(tlPositions.FocusedNode["PositionID"].ToString(), out iValue))
                    {
                        if (Utility._IsGermany == true)
                            strConfirmation = XtraMessageBox.Show("Wollen Sie die LV Position" + Pos + "wirklich löschen?", "Bestätigung!", MessageBoxButtons.YesNo, MessageBoxIcon.Question).ToString();
                        else
                            strConfirmation = XtraMessageBox.Show("Do you really want to delete the Position " + Pos + ".?", "Confirmation!", MessageBoxButtons.YesNo, MessageBoxIcon.Question).ToString();
                        if (strConfirmation.ToLower() == "yes")
                        {
                            ObjBPosition.Deleteposition(iValue, _PosKZ);
                            tlPositions.DeleteSelectedNodes();
                            //if (tlPositions.FocusedNode != null)
                            //{
                            //    string strTempID = tlPositions.FocusedNode["PositionID"] == null ? "" : Convert.ToString(tlPositions.FocusedNode["PositionID"]);
                            //    BindPositionData();
                            //    int ID = 0;
                            //    if (int.TryParse(strTempID, out ID))
                            //    {
                            //        SetFocus(ID, tlPositions);
                            //    }
                            //}
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        /// <summary>
        /// Code to parse Position details while copying LV Position or Detail KZ Position
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="strPositionsOZ"></param>
        /// <param name="strParetntOZ"></param>
        /// <param name="iTempSNO"></param>
        /// <param name="strLongDescription"></param>
        /// <param name="isDetailKZ"></param>
        /// <param name="iDetailKZ"></param>
        private void ParseLVAndDetailKZCopyLV(DataRow dr, string strPositionsOZ, string strParetntOZ, int iTempSNO, string strLongDescription, bool isDetailKZ, int iDetailKZ = -1)
        {
            try
            {
                string strnextLV = string.Empty;
                int iValue = 0;
                decimal dValue = 0;
                int INodeIndex = 0;
                int IIndex = 0;
                DateTime dt = DateTime.Now;
                ObjEPosition.PositionKZ = Convert.ToString(dr["PositionKZ"]);
                ObjEPosition.PositionKZID = Convert.ToString(dr["PositionKZID"]);

                int PID = 0;
                if (int.TryParse(Convert.ToString(dr["PositionID"]), out PID))
                    ObjEPosition.LongDescription = ObjBPosition.GetLongDescription(PID);
                if (iDetailKZ > 0)
                    ObjEPosition.DetailKZ = iDetailKZ;
                else
                {
                    if (int.TryParse(Convert.ToString(dr["DetailKZ"]), out iValue))
                        ObjEPosition.DetailKZ = iValue;
                    else
                        ObjEPosition.DetailKZ = 0;
                }

                if (ObjEPosition.DetailKZ == 0)
                    ObjEPosition.Position_OZ = strPositionsOZ;
                if (isDetailKZ)
                    ObjEPosition.Position_OZ = strPositionsOZ;

                ObjEPosition.Parent_OZ = strParetntOZ;
                ObjEPosition.ProjectID = ObjEProject.ProjectID;
                ObjEPosition.PositionID = -1;
                if (!isDetailKZ && ObjEPosition.DetailKZ == 0)
                {
                    TreeListNode Nodetofocus = null;
                    Nodetofocus = this.tlPositions.FindNodeByKeyID(tlPositions.FocusedNode["PositionID"]);
                    INodeIndex = tlPositions.GetNodeIndex(Nodetofocus);
                    IIndex = 2;
                    if (INodeIndex != null)
                    {
                        bool _Continue = true;
                        int IValue = 0;
                        while (_Continue)
                        {
                            IValue++;
                            if (Nodetofocus.ParentNode.Nodes.Count > INodeIndex + IValue)
                            {
                                strnextLV = Convert.ToString(Nodetofocus.ParentNode.Nodes[INodeIndex + IValue]["Position_OZ"]);
                                if (!string.IsNullOrEmpty(strnextLV))
                                    _Continue = false;
                            }
                            else
                                _Continue = false;
                        }
                    }
                    string _Suggested_OZ = SuggestOZForCopy(strPositionsOZ, strnextLV, IIndex);

                    frmNewOZ Obj = new frmNewOZ();
                    Obj.strNewOZ = _Suggested_OZ;
                    Obj.LVRaster = ObjEProject.LVRaster;
                    Obj.ShowDialog();
                    if (!Obj.IsSave)
                    {
                        ObjEPosition.Position_OZ = string.Empty;
                        return;
                    }
                    _Suggested_OZ = Obj.strNewOZ;

                    string str = string.Empty;
                    string strRaster = ObjEProject.LVRaster;
                    string[] strPOZ = strPositionsOZ.Split('.');
                    string[] strPRaster = strRaster.Split('.');
                    int Count = -1;
                    int i = -1;
                    Count = strPOZ.Count();
                    while (Count > 0)
                    {
                        i = i + 1;
                        Count = Count - 1;
                        int OZLength = 0;
                        int RasterLength = 0;
                        string OZ = string.Empty;
                        OZ = strPOZ[i].Trim();
                        RasterLength = strPRaster[i].Length;
                        OZLength = OZ.Trim().Length;
                        if (Count > 0)
                        {
                            if (OZ == "")
                            {
                                str = str + string.Concat(Enumerable.Repeat(" ", RasterLength - OZLength)) + OZ + ".";
                            }
                        }
                    }
                    if (str.Length > 0)
                    {
                        string strBlankNewOz = strParetntOZ + str + _Suggested_OZ;
                        ObjEPosition.Position_OZ = Utility.PrepareOZ(strBlankNewOz, ObjEProject.LVRaster);
                    }
                    else
                    {
                        string strNewOz = strParetntOZ + _Suggested_OZ;
                        ObjEPosition.Position_OZ = Utility.PrepareOZ(strNewOz, ObjEProject.LVRaster);
                    }

                }
                if (cmbLVSection.Text == string.Empty)
                    ObjEPosition.LVSection = "HA";
                else
                    ObjEPosition.LVSection = cmbLVSection.Text;

                ObjEPosition.LVSectionID = cmbLVSection.EditValue;

                ObjEPosition.WG = dr["WG"] == DBNull.Value ? "" : dr["WG"].ToString();
                ObjEPosition.WA = dr["WA"] == DBNull.Value ? "" : dr["WA"].ToString();
                ObjEPosition.WI = dr["WI"] == DBNull.Value ? "" : dr["WI"].ToString();

                if (decimal.TryParse(Convert.ToString(dr["Menge"]), out dValue))
                    ObjEPosition.Menge = dValue;
                else
                    ObjEPosition.Menge = 1;

                ObjEPosition.ME = dr["ME"] == DBNull.Value ? "" : dr["ME"].ToString();

                ObjEPosition.Fabricate = dr["Fabricate"] == DBNull.Value ? "" : dr["Fabricate"].ToString();
                ObjEPosition.LiefrantMA = dr["LiefrantMA"] == DBNull.Value ? "" : dr["LiefrantMA"].ToString();
                ObjEPosition.Type = dr["Type"] == DBNull.Value ? "" : dr["Type"].ToString();
                ObjEPosition.ShortDescription = dr["ShortDescription"] == DBNull.Value ? "" : dr["ShortDescription"].ToString();
                ObjEPosition.Surcharge_From = dr["surchargefrom"] == DBNull.Value ? "" : dr["surchargefrom"].ToString();
                ObjEPosition.Surcharge_To = dr["surchargeto"] == DBNull.Value ? "" : dr["surchargeto"].ToString();

                if (decimal.TryParse(Convert.ToString(dr["surchargePercentage"]), out dValue))
                    ObjEPosition.Surcharge_Per = dValue;
                else
                    ObjEPosition.Surcharge_Per = 0;

                if (decimal.TryParse(Convert.ToString(dr["surchargePercentage_MO"]), out dValue))
                    ObjEPosition.Surcharge_Per = dValue;
                else
                    ObjEPosition.surchargePercentage_MO = 0;

                DateTime dtv = ObjEProject.SubmitDate;
                if (DateTime.TryParse(Convert.ToString(dr["validitydate"]), out dtv))
                    ObjEPosition.ValidityDate = dtv;
                else
                    ObjEPosition.ValidityDate = ObjEProject.SubmitDate;

                ObjEPosition.MA = dr["MA"] == DBNull.Value ? "" : dr["MA"].ToString();
                ObjEPosition.MO = dr["MO"] == DBNull.Value ? "" : dr["MO"].ToString();

                if (decimal.TryParse(Convert.ToString(dr["minutes"]), out dValue))
                    ObjEPosition.Mins = dValue;
                else
                    ObjEPosition.Mins = 0;

                if (decimal.TryParse(Convert.ToString(dr["Faktor"]), out dValue))
                    ObjEPosition.Faktor = dValue;
                else
                    ObjEPosition.Faktor = 1;

                if (decimal.TryParse(Convert.ToString(dr["MA_listprice"]), out dValue))
                    ObjEPosition.LPMA = dValue;
                else
                    ObjEPosition.LPMA = 0;

                if (decimal.TryParse(Convert.ToString(dr["MO_listprice"]), out dValue))
                    ObjEPosition.LPMO = dValue;
                else
                    ObjEPosition.LPMO = 0;

                if (decimal.TryParse(Convert.ToString(dr["MA_Multi1"]), out dValue))
                    ObjEPosition.Multi1MA = dValue;
                else
                    ObjEPosition.Multi1MA = 1;

                if (decimal.TryParse(Convert.ToString(dr["MA_multi2"]), out dValue))
                    ObjEPosition.Multi2MA = dValue;
                else
                    ObjEPosition.Multi2MA = 1;

                if (decimal.TryParse(Convert.ToString(dr["MA_multi3"]), out dValue))
                    ObjEPosition.Multi3MA = dValue;
                else
                    ObjEPosition.Multi3MA = 1;

                if (decimal.TryParse(Convert.ToString(dr["MA_multi4"]), out dValue))
                    ObjEPosition.Multi4MA = dValue;
                else
                    ObjEPosition.Multi4MA = 1;

                if (decimal.TryParse(Convert.ToString(dr["MO_multi1"]), out dValue))
                    ObjEPosition.Multi1MO = dValue;
                else
                    ObjEPosition.Multi1MO = 1;

                if (decimal.TryParse(Convert.ToString(dr["MO_multi2"]), out dValue))
                    ObjEPosition.Multi2MO = dValue;
                else
                    ObjEPosition.Multi2MO = 1;

                if (decimal.TryParse(Convert.ToString(dr["MO_multi3"]), out dValue))
                    ObjEPosition.Multi3MO = dValue;
                else
                    ObjEPosition.Multi3MO = 1;

                if (decimal.TryParse(Convert.ToString(dr["MO_multi4"]), out dValue))
                    ObjEPosition.Multi4MO = dValue;
                else
                    ObjEPosition.Multi4MO = 1;

                if (decimal.TryParse(Convert.ToString(dr["MA_einkaufspreis"]), out dValue))
                    ObjEPosition.EinkaufspreisMA = dValue;
                else
                    ObjEPosition.EinkaufspreisMA = 0;

                if (decimal.TryParse(Convert.ToString(dr["MO_Einkaufspreis"]), out dValue))
                    ObjEPosition.EinkaufspreisMO = dValue;
                else
                    ObjEPosition.EinkaufspreisMO = 0;

                if (decimal.TryParse(Convert.ToString(dr["MA_selbstkostenMulti"]), out dValue))
                    ObjEPosition.SelbstkostenMultiMA = dValue;
                else
                    ObjEPosition.SelbstkostenMultiMA = 1;

                if (decimal.TryParse(Convert.ToString(dr["MA_selbstkosten"]), out dValue))
                    ObjEPosition.SelbstkostenValueMA = dValue;
                else
                    ObjEPosition.SelbstkostenValueMA = 0;

                if (decimal.TryParse(Convert.ToString(dr["MO_selbstkostenMulti"]), out dValue))
                    ObjEPosition.SelbstkostenMultiMO = dValue;
                else
                    ObjEPosition.SelbstkostenMultiMO = 1;

                if (decimal.TryParse(Convert.ToString(dr["MO_selbstkosten"]), out dValue))
                    ObjEPosition.SelbstkostenValueMO = dValue;
                else
                    ObjEPosition.SelbstkostenValueMO = 0;

                if (decimal.TryParse(Convert.ToString(dr["MA_verkaufspreis_Multi"]), out dValue))
                    ObjEPosition.VerkaufspreisMultiMA = dValue;
                else
                    ObjEPosition.VerkaufspreisMultiMA = 1;

                if (decimal.TryParse(Convert.ToString(dr["MA_verkaufspreis"]), out dValue))
                    ObjEPosition.VerkaufspreisValueMA = dValue;
                else
                    ObjEPosition.VerkaufspreisValueMA = 0;

                if (decimal.TryParse(Convert.ToString(dr["MO_verkaufspreisMulti"]), out dValue))
                    ObjEPosition.VerkaufspreisMultiMO = dValue;
                else
                    ObjEPosition.VerkaufspreisMultiMO = 1;

                if (decimal.TryParse(Convert.ToString(dr["MO_verkaufspreis"]), out dValue))
                    ObjEPosition.VerkaufspreisValueMO = dValue;
                else
                    ObjEPosition.VerkaufspreisValueMO = 0;

                if (decimal.TryParse(Convert.ToString(dr["std_satz"]), out dValue))
                    ObjEPosition.StdSatz = dValue;
                else
                    ObjEPosition.StdSatz = 0;

                ObjEPosition.PreisText = Convert.ToString(dr["PreisText"]);
                ObjEPosition.EinkaufspreisLockMA = dr["MA_einkaufspreis_lck"] == DBNull.Value ? true : Convert.ToBoolean(dr["MA_einkaufspreis_lck"]);
                ObjEPosition.EinkaufspreisLockMO = dr["MO_Einkaufspreis_lck"] == DBNull.Value ? true : Convert.ToBoolean(dr["MO_Einkaufspreis_lck"]);
                ObjEPosition.SelbstkostenLockMA = dr["MA_selbstkosten_lck"] == DBNull.Value ? true : Convert.ToBoolean(dr["MA_selbstkosten_lck"]);
                ObjEPosition.SelbstkostenLockMO = dr["MO_selbstkosten_lck"] == DBNull.Value ? true : Convert.ToBoolean(dr["MO_selbstkosten_lck"]);
                ObjEPosition.VerkaufspreisLockMA = dr["MA_verkaufspreis_lck"] == DBNull.Value ? true : Convert.ToBoolean(dr["MA_verkaufspreis_lck"]);
                ObjEPosition.VerkaufspreisLockMO = dr["MO_verkaufspreis_lck"] == DBNull.Value ? true : Convert.ToBoolean(dr["MO_verkaufspreis_lck"]);
                ObjEPosition.Dim1 = dr["A"] == DBNull.Value ? "" : dr["A"].ToString();
                ObjEPosition.Dim2 = dr["B"] == DBNull.Value ? "" : dr["B"].ToString();
                ObjEPosition.Dim3 = dr["L"] == DBNull.Value ? "" : dr["L"].ToString();
                ObjEPosition.LVStatus = Convert.ToString(dr["LVStatus"]);
                ObjEPosition.LVStatusID = dr["LVStatusID"];

                if (decimal.TryParse(Convert.ToString(dr["GrandTotalME"]), out dValue))
                    ObjEPosition.GrandTotalME = dValue;
                else
                    ObjEPosition.GrandTotalME = 0;

                if (decimal.TryParse(Convert.ToString(dr["GrandTotalMO"]), out dValue))
                    ObjEPosition.GrandTotalMO = dValue;
                else
                    ObjEPosition.GrandTotalMO = 0;

                if (decimal.TryParse(Convert.ToString(dr["FinalGB"]), out dValue))
                    ObjEPosition.FinalGB = dValue;
                else
                    ObjEPosition.FinalGB = 0;

                if (decimal.TryParse(Convert.ToString(dr["EP"]), out dValue))
                    ObjEPosition.EP = dValue;
                else
                    ObjEPosition.EP = 0;

                if (decimal.TryParse(Convert.ToString(dr["Discount"]), out dValue))
                    ObjEPosition.Discount = dValue;
                else
                    ObjEPosition.Discount = 0;

                ObjEPosition.SNO = iTempSNO;

                bool MontageCheck = false;
                if (bool.TryParse(Convert.ToString(dr["MontageEntry"]), out MontageCheck))
                    ObjEPosition.MontageEntry = MontageCheck;
                else
                    ObjEPosition.MontageEntry = MontageCheck;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Code to fetch LV Rasters from database and vind to Combobox
        /// </summary>
        private void LoadExistingRasters()
        {
            try
            {
                objEGAEB.dtLVRaster = objBGAEB.Get_LVRasters();
                if (objEGAEB.dtLVRaster.Rows != null)
                {
                    foreach (DataRow Row in objEGAEB.dtLVRaster.Rows)
                    {
                        ddlRaster.Properties.Items.Add(Row["LVRasterName"]);
                    }
                    ddlRaster.Properties.Sorted = true;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }

        }

        /// <summary>
        /// Code to Get Title description from Position table
        /// </summary>
        /// <param name="strTitle"></param>
        /// <returns></returns>
        private string GetTitle(string strTitle)
        {
            string strTitleDesc = string.Empty;
            try
            {
                DataRow[] drPosition = ObjEPosition.dsPositionList.Tables[0].Select("Position_OZ='" + strTitle + "'");
                if (drPosition != null && drPosition.Count() > 0)
                {
                    strTitleDesc = drPosition[0]["Title"] == DBNull.Value ? "" : drPosition[0]["Title"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return strTitleDesc;
        }

        /// <summary>
        /// Code to set enabled property to LV  and Cost details controls
        /// </summary>
        /// <param name="result"></param>
        private void EnableAndDisableAllControls(bool result)
        {
            txtStufe1Short.Enabled = result;
            txtStufe2Short.Enabled = result;
            txtStufe3Short.Enabled = result;
            txtStufe4Short.Enabled = result;
            txtWG.Enabled = result;
            txtWA.Enabled = result;
            txtWI.Enabled = result;
            cmbCDME.Enabled = result;
            txtMenge.Enabled = result;
            txtFabrikate.Enabled = result;
            txtLiefrantMA.Enabled = result;
            cmbPositionKZ.Enabled = result;
            txtType.Enabled = result;
            txtDetailKZ.Enabled = result;
            txtDim1.Enabled = result;
            txtDim2.Enabled = result;
            txtDim3.Enabled = result;
            txtMin.Enabled = result;
            txtFaktor.Enabled = result;
            txtMa.Enabled = result;
            txtMo.Enabled = result;
            txtPreisText.Enabled = result;
            btnDocuwareLink.Enabled = result;
            txtLPMe.Enabled = result;
            txtMulti1ME.Enabled = result;
            txtMulti2ME.Enabled = result;
            txtMulti3ME.Enabled = result;
            txtMulti4ME.Enabled = result;
            txtSelbstkostenMultiME.Enabled = result;
            txtVerkaufspreisMultiME.Enabled = result;
            txtMulti1MO.Enabled = result;
            txtMulti2MO.Enabled = result;
            txtMulti3MO.Enabled = result;
            txtMulti4MO.Enabled = result;
            txtSelbstkostenMultiMO.Enabled = result;
            txtVerkaufspreisMultiMO.Enabled = result;
            chkEinkaufspreisME.Enabled = result;
            chkSelbstkostenME.Enabled = result;
            chkVerkaufspreisME.Enabled = result;
            chkEinkaufspreisMO.Enabled = result;
            chkSelbstkostenMO.Enabled = result;
            chkVerkaufspreisMO.Enabled = result;
        }

        /// <summary>
        /// Code to Parse position details to entities while adding accessories
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="Sno"></param>
        /// <param name="DetailKZ"></param>
        /// <param name="strLVSection"></param>
        /// <param name="FActor"></param>
        /// <param name="ObjEArticles"></param>
        /// <param name="ObjBArticles"></param>
        private void ParseAccessories(DataRow dr, int Sno, int DetailKZ, string strLVSection, string FActor, EArticles ObjEArticles, BArticles ObjBArticles)
        {
            try
            {
                decimal dValue = 0;
                DateTime dt = DateTime.Now;
                ObjEPosition.ProjectID = ObjEProject.ProjectID;
                ObjEPosition.RasterCount = iRasterCount;
                ObjEPosition.Stufe1 = txtStufe1Short.Text;
                ObjEPosition.Stufe2 = txtStufe2Short.Text;
                ObjEPosition.Stufe3 = txtStufe3Short.Text;
                ObjEPosition.Stufe4 = txtStufe4Short.Text;
                ObjEPosition.Position = txtPosition.Text;
                ObjEPosition.PositionID = -1;
                ObjEPosition.PositionKZ = Utility.GetPosKZ(cmbPositionKZ.Text);
                ObjEPosition.PositionKZID = cmbPositionKZ.EditValue;
                ObjEPosition.DetailKZ = DetailKZ;
                ObjEPosition.LVSection = strLVSection;
                ObjEPosition.LVSectionID = cmbLVSection.EditValue;
                ObjEPosition.WG = Convert.ToString(dr["WG"]);
                ObjEPosition.WA = Convert.ToString(dr["WA"]);
                ObjEPosition.WI = Convert.ToString(dr["WI"]);
                ObjEPosition.Dim1 = Convert.ToString(dr["A"]);
                ObjEPosition.Dim2 = Convert.ToString(dr["B"]);
                ObjEPosition.Dim3 = Convert.ToString(dr["L"]);

                if (decimal.TryParse(Convert.ToString(dr["Menge"]), out dValue))
                    ObjEPosition.Menge = dValue;
                else
                    ObjEPosition.Menge = 1;

                ObjEArticles.WG = ObjEPosition.WG;
                ObjEArticles.WA = ObjEPosition.WA;
                ObjEArticles.WI = ObjEPosition.WI;
                ObjEArticles.A = ObjEPosition.Dim1;
                ObjEArticles.B = ObjEPosition.Dim2;
                ObjEArticles.L = ObjEPosition.Dim3;
                ObjEArticles.ValidityDate = ObjEProject.SubmitDate;
                ObjEArticles = ObjBArticles.GetArticleDetailsForAccessories(ObjEArticles);
                ObjEPosition.PreisText = string.Empty;
                ObjEPosition.ME = Convert.ToString(ObjEArticles.dtArticleDetails.Rows[0]["Menegenheit"]);
                ObjEPosition.Fabricate = Convert.ToString(ObjEArticles.dtArticleDetails.Rows[0]["Fabrikate"]);
                ObjEPosition.LiefrantMA = Convert.ToString(ObjEArticles.dtArticleDetails.Rows[0]["ShortName"]);
                ObjEPosition.Type = Convert.ToString(ObjEArticles.dtArticleDetails.Rows[0]["Typ"]);
                ObjEPosition.LongDescription = string.Empty;
                ObjEPosition.ShortDescription = Utility.GetRTFFormat(Convert.ToString(ObjEArticles.dtArticleDetails.Rows[0]["ArticleDescription"]));
                ObjEPosition.Surcharge_From = string.Empty;
                ObjEPosition.Surcharge_To = string.Empty;
                ObjEPosition.Surcharge_Per = 0;
                ObjEPosition.surchargePercentage_MO = 0;

                DateTime dtv = ObjEProject.SubmitDate;
                if (DateTime.TryParse(Convert.ToString(ObjEArticles.dtArticleDetails.Rows[0]["ValidityDate"]), out dtv))
                    ObjEPosition.ValidityDate = dtv;
                else
                    ObjEPosition.ValidityDate = ObjEProject.SubmitDate;
                ObjEPosition.MA = "X";
                ObjEPosition.MO = "X";
                if (string.IsNullOrEmpty(ObjEProject.CommissionNumber))
                    ObjEPosition.LVStatus = string.Empty;
                else
                    ObjEPosition.LVStatus = "B";

                ObjEPosition.LVStatusID = cmbLVStatus.EditValue;

                if (decimal.TryParse(Convert.ToString(ObjEArticles.dtArticleDetails.Rows[0]["Minuten"]), out dValue))
                    ObjEPosition.Mins = dValue;
                else
                    ObjEPosition.Mins = 0;

                if (decimal.TryParse(FActor, out dValue))
                    ObjEPosition.Faktor = dValue;
                else
                    ObjEPosition.Faktor = 1;

                ObjEPosition.StdSatz = ObjEProject.InternX;

                if (decimal.TryParse(Convert.ToString(ObjEArticles.dtArticleDetails.Rows[0]["ListPrice"]), out dValue))
                    ObjEPosition.LPMA = dValue;
                else
                    ObjEPosition.LPMA = 0;

                ObjEPosition.LPMO = (ObjEPosition.Mins / 60) * ObjEPosition.StdSatz * ObjEPosition.Faktor;

                if (decimal.TryParse(Convert.ToString(ObjEArticles.dtArticleDetails.Rows[0]["Multi1"]), out dValue))
                    ObjEPosition.Multi1MA = dValue;
                else
                    ObjEPosition.Multi1MA = 1;

                if (decimal.TryParse(Convert.ToString(ObjEArticles.dtArticleDetails.Rows[0]["Multi2"]), out dValue))
                    ObjEPosition.Multi2MA = dValue;
                else
                    ObjEPosition.Multi2MA = 1;

                if (decimal.TryParse(Convert.ToString(ObjEArticles.dtArticleDetails.Rows[0]["Multi3"]), out dValue))
                    ObjEPosition.Multi3MA = dValue;
                else
                    ObjEPosition.Multi3MA = 1;

                if (decimal.TryParse(Convert.ToString(ObjEArticles.dtArticleDetails.Rows[0]["Multi4"]), out dValue))
                    ObjEPosition.Multi4MA = dValue;
                else
                    ObjEPosition.Multi4MA = 1;

                ObjEPosition.Multi1MO = 1;
                ObjEPosition.Multi2MO = 1;
                ObjEPosition.Multi3MO = 1;
                ObjEPosition.Multi4MO = 1;
                ObjEPosition.EinkaufspreisMA = Math.Round(((ObjEPosition.LPMA) * (ObjEPosition.Multi1MA * ObjEPosition.Multi2MA * ObjEPosition.Multi3MA * ObjEPosition.Multi4MA)), 8);
                ObjEPosition.EinkaufspreisMO = ObjEPosition.LPMO;
                ObjEPosition.SelbstkostenMultiMA = 1;
                ObjEPosition.SelbstkostenValueMA = ObjEPosition.EinkaufspreisMA;
                ObjEPosition.SelbstkostenMultiMO = 1;
                ObjEPosition.SelbstkostenValueMO = ObjEPosition.EinkaufspreisMO;
                ObjEPosition.VerkaufspreisMultiMA = 1;
                ObjEPosition.VerkaufspreisValueMA = ObjEPosition.EinkaufspreisMA;
                ObjEPosition.VerkaufspreisMultiMO = 1;
                ObjEPosition.VerkaufspreisValueMO = ObjEPosition.EinkaufspreisMO;

                ObjEPosition.EinkaufspreisLockMA = false;
                ObjEPosition.EinkaufspreisLockMO = false;
                ObjEPosition.SelbstkostenLockMA = false;
                ObjEPosition.SelbstkostenLockMO = false;
                ObjEPosition.VerkaufspreisLockMA = false;
                ObjEPosition.VerkaufspreisLockMO = false;
                ObjEPosition.GrandTotalME = ObjEPosition.EinkaufspreisMA;
                ObjEPosition.GrandTotalMO = ObjEPosition.EinkaufspreisMO;
                ObjEPosition.EP = ObjEPosition.EinkaufspreisMA + ObjEPosition.EinkaufspreisMO;
                ObjEPosition.FinalGB = RoundValue(ObjEPosition.EP * ObjEPosition.Menge);
                ObjEPosition.SNO = Sno;
                ObjEPosition.MontageEntry = false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Code to compare article prices
        /// </summary>
        /// <param name="_type"></param>
        private void BindCoaprePrice(string _type)
        {
            try
            {
                int PosID = 0;
                if (ObjEPosition.dsPositionList.Tables[0].Rows.Count > 0)
                {
                    if (int.TryParse(tlPositions.FocusedNode["PositionID"].ToString(), out PosID))
                    {
                        if (_IsNewMode == true || Convert.ToBoolean(chkCreateNew.EditValue) == true)
                        {
                            PosID = -1;
                        }
                        ObjBProject.GetComparePrice(ObjEProject, txtWG.Text, txtWA.Text, txtWI.Text, txtDim1.Text, txtDim2.Text, txtDim3.Text, txtType.Text, _type, PosID);
                        if (ObjEProject.dsComaparePrice != null)
                        {
                            gcComparePrice.DataSource = ObjEProject.dsComaparePrice.Tables[0];
                            BgvComparePrice.BestFitColumns();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Code to fetch the dimensions based on article and show a popup a to select one of them
        /// </summary>
        private void FillDimension()
        {
            try
            {
                if (ObjBPosition == null)
                    ObjBPosition = new BPosition();
                if (ObjEPosition == null)
                    ObjEPosition = new EPosition();
                ObjEPosition.dtArticle = new DataTable();
                ObjEPosition.dtDimensions = new DataTable();
                ObjEPosition.Type = txtType.Text;
                ObjEPosition.ValidityDate = ObjEProject.SubmitDate;
                ObjEPosition = ObjBPosition.GetArticleByTyp(ObjEPosition);
                if (ObjEPosition.dtArticle != null && ObjEPosition.dtArticle.Rows.Count > 0)
                {
                    txtWGCD.Text = ObjEPosition.WG;
                    txtWACD.Text = ObjEPosition.WA;
                    txtWICD.Text = ObjEPosition.WI;
                    if (ObjEProject.FabVisible && !string.IsNullOrEmpty(ObjEPosition.Fabricate))
                        txtFabrikate.Text = ObjEPosition.Fabricate;
                    if (!string.IsNullOrEmpty(ObjEPosition.LiefrantMA))
                        txtLiefrantMA.Text = ObjEPosition.LiefrantMA;
                    if (ObjEProject.MeVisible && !string.IsNullOrEmpty(ObjEPosition.ME))
                        cmbCDME.SelectedIndex = cmbCDME.Properties.Items.IndexOf(ObjEPosition.ME);
                    txtFaktor.Text = Convert.ToString(ObjEPosition.Faktor);

                    decimal dv = 0;
                    if (decimal.TryParse(Convert.ToString(ObjEPosition.dtArticle.Rows[0]["Multi1"]), out dv))
                    {
                        if (ObjEProject.M1Visible)
                            txtMulti1ME.EditValue = dv;
                    }

                    if (decimal.TryParse(Convert.ToString(ObjEPosition.dtArticle.Rows[0]["Multi2"]), out dv))
                    {
                        if (ObjEProject.M2Visible)
                            txtMulti2ME.EditValue = dv;
                    }
                    if (decimal.TryParse(Convert.ToString(ObjEPosition.dtArticle.Rows[0]["Multi3"]), out dv))
                    {
                        if (ObjEProject.M3Visible)
                            txtMulti3ME.EditValue = dv;
                    }
                    if (decimal.TryParse(Convert.ToString(ObjEPosition.dtArticle.Rows[0]["Multi4"]), out dv))
                    {
                        if (ObjEProject.M4Visible)
                            txtMulti4ME.EditValue = dv;
                    }

                    if (ObjEProject.DimVisible && !string.IsNullOrEmpty(ObjEPosition.Dim))
                        txtDim.Text = ObjEPosition.Dim;
                }
                else
                {
                    txtWG.Text = string.Empty;
                    txtWA.Text = string.Empty;
                    txtWI.Text = string.Empty;
                }
                txtDim1.Text = string.Empty;
                txtDim2.Text = string.Empty;
                txtDim3.Text = string.Empty;
            }
            catch (Exception ex) { throw; }
        }

        /// <summary>
        /// Code to show tooltip message  on a textbox
        /// </summary>
        /// <param name="textedit"></param>
        private void ShowTooltip(TextEdit textedit)
        {
            try
            {
                string formattedMessage = "Press 'Enter/F8' to see the updated Verkaufspreis Multi";
                Point toolTipLocation = textedit.PointToScreen(new Point(0, textedit.Height));
                toolTipController1.ShowHint(formattedMessage, ToolTipLocation.Fixed, toolTipLocation);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Code to copy a position
        /// </summary>
        public void CopyPosition()
        {
            try
            {
                if (tlPositions.FocusedNode == null)
                    return;
                if (tcProjectDetails.SelectedTabPage != tbLVDetails)
                    return;
                string stPKz = Convert.ToString(tlPositions.FocusedNode["PositionKZ"]);
                if (stPKz == "NG" || stPKz == "H" || stPKz == "VR" || stPKz == "AB" || stPKz == "BA")
                    return;
                ObjEPosition.PositionID = -1;
                _IsNewMode = true;
                if (ObjBPosition == null)
                    ObjBPosition = new BPosition();
                LongDescription = ObjBPosition.GetLongDescription(Convert.ToInt32(tlPositions.FocusedNode["PositionID"]));
                btnSaveLVDetails_Click(null, null);
            }
            catch (Exception ex) { Utility.ShowError(ex); }
        }

        /// <summary>
        /// Code to set mask for onhe stude textbox based on extract from ratser
        /// </summary>
        private void SetOhnestuffeMask()
        {
            try
            {
                string[] Levels = ObjEProject.LVRaster.Split('.');
                int Count = Levels.Length;
                if (Count >= 2)
                {
                    int _Length = Levels[Count - 2].Length;
                    txtPosition.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
                    txtPosition.Properties.Mask.EditMask = "[A-Z0-9]{1," + _Length + "}((\\.)[A-Za-z0-9]{0,1})?";
                    txtPosition.Properties.Mask.UseMaskAsDisplayFormat = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Code to set readonly property to treelist columns
        /// </summary>
        /// <param name="_value"></param>
        private void LVDetailsColumnReadOnly(bool _value)
        {
            try
            {
                tlPositions.Columns["WG"].ColumnEdit.ReadOnly = _value;
                tlPositions.Columns["WA"].ColumnEdit.ReadOnly = _value;
                tlPositions.Columns["WI"].ColumnEdit.ReadOnly = _value;
                tlPositions.Columns["Menge"].ColumnEdit.ReadOnly = _value;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Code to set readonly property to treelist columns
        /// </summary>
        /// <param name="_value"></param>
        private void LVCalculationColumnReadOnly(bool _value)
        {
            try
            {
                tlPositions.Columns["MA_Multi1"].ColumnEdit.ReadOnly = _value;
                tlPositions.Columns["MA_multi2"].ColumnEdit.ReadOnly = _value;
                tlPositions.Columns["MA_multi3"].ColumnEdit.ReadOnly = _value;
                tlPositions.Columns["MA_multi4"].ColumnEdit.ReadOnly = _value;
                tlPositions.Columns["MA_selbstkostenMulti"].ColumnEdit.ReadOnly = _value;
                tlPositions.Columns["MO_multi1"].ColumnEdit.ReadOnly = _value;
                tlPositions.Columns["MO_multi2"].ColumnEdit.ReadOnly = _value;
                tlPositions.Columns["MO_multi3"].ColumnEdit.ReadOnly = _value;
                tlPositions.Columns["MO_multi4"].ColumnEdit.ReadOnly = _value;
                tlPositions.Columns["MO_selbstkostenMulti"].ColumnEdit.ReadOnly = _value;
                tlPositions.Columns["MA_verkaufspreis_Multi"].ColumnEdit.ReadOnly = _value;
                tlPositions.Columns["MO_verkaufspreisMulti"].ColumnEdit.ReadOnly = _value;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #endregion

        #endregion

        #region Project Import
        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (ObjEProject.ProjectID > 0)
                {
                    frmGAEBImport Obj = new frmGAEBImport();
                    Obj.ProjectID = ObjEProject.ProjectID;
                    Obj.KNr = ObjEProject.CommissionNumber;
                    Obj.ShowDialog();
                    ProjectID = Obj.ProjectID;
                    if (ProjectID > 0 && Obj.isbuild)
                    {
                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                        SplashScreenManager.Default.SetWaitFormDescription("Laden Projekt...");
                        LoadExistingRasters();
                        LoadExistingProject();
                        BindPositionData();
                        IntializeLVPositions();
                        if (ObjEProject.ActualLvs == 0)
                            ChkRaster.Enabled = true;
                        else
                            ChkRaster.Enabled = false;
                        SplashScreenManager.CloseForm(false);
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                if (Utility._IsGermany == true)
                {
                    throw new Exception("Das ausgewählte Datei-Raster ist nicht mit dem ausgewählten Projektraster kompatibel!");
                }
                else
                {
                    Utility.ShowError(ex);
                }
            }

        }
        #endregion

        #region BULK PROCESS

        #region Events
        private void btnApply_Click(object sender, EventArgs e)
        {

            try
            {
                if (cmbBulkProcessSelection.SelectedIndex != 2)
                {
                    if (gvAddRemovePositions.DefaultView.RowCount == 0)
                    {
                        if (Utility._IsGermany == true)
                        {
                            XtraMessageBox.Show("Bitte machen Sie VON / BIS Angaben.");
                        }
                        else
                        {
                            XtraMessageBox.Show("Please Add From and To values.");
                        }
                        return;
                    }
                }
                string Gridvalue = null;
                if (cmbBulkProcessSelection.SelectedIndex != 2)
                {
                    //foreach (DataGridViewRow dr in gvAddRemovePositions.Rows)
                    //{
                    
                    for (int i = 0; i < gridView1.RowCount; i++)
                    {
                        DataRow dr;
                        dr = gridView1.GetDataRow(i);                       
                        Gridvalue = Convert.ToString(dr[1]);
                        if (Gridvalue == null || Gridvalue == "")
                        {
                            if (Utility._IsGermany == true)
                            {
                                XtraMessageBox.Show("Bitte machen Sie VON / BIS Angaben.");
                            }
                            else
                            {
                                XtraMessageBox.Show("Please enter From And To values.!");
                            }
                            return;
                        }
                    }

                }

                AssignPosition_type();
                if (cmbBulkProcessSelection.SelectedIndex == 2)
                {
                    if (txtBulkProcessWG.Text == "")
                    {
                        if (Utility._IsGermany == true)
                        {
                            XtraMessageBox.Show("Bitte machen Sie eine Angabe zu WG.");
                        }
                        else
                        {
                            XtraMessageBox.Show("Please Enter WG value.!");
                        }
                        return;
                    }
                    else
                    {
                        if (txtBulkProcessWA.Text == "")
                        {
                            txtBulkProcessWA.Text = "0";
                        }
                        ObjBPosition.GetPositionOZListByWGWA(ObjEPosition, ObjEProject.ProjectID, tType, txtBulkProcessWG.Text, txtBulkProcessWA.Text);
                    }
                }
                else
                {
                    DataTable dtPos = new DataTable();
                    dtPos.Columns.Add("FromPos");
                    dtPos.Columns.Add("ToPos");

                    string tfrom = null;
                    string tTo = null;
                    //foreach (DataGridViewRow dr in gvAddRemovePositions.Rows)
                    for (int i = 0; i < gridView1.RowCount; i++)
                    {
                        DataRow dr = gridView1.GetDataRow(i);
                        if (dr[0] == null)
                        {
                            throw new Exception("Bitte machen Sie VON Angaben.");
                        }

                        DataRow drPos = dtPos.NewRow();
                        tfrom = dr[0].ToString();
                        tTo = dr[1].ToString();
                        if (cmbBulkProcessSelection.SelectedIndex == 0)
                        {
                            string _fromParent = string.Empty;
                            string _ToParent = string.Empty;
                            if (tfrom.Contains("."))
                                _fromParent = tfrom.Substring(0, tfrom.IndexOf('.'));
                            if (tTo.Contains("."))
                                _ToParent = tTo.Substring(0, tTo.IndexOf('.'));
                            if (_fromParent != _ToParent)
                            {
                                if (Utility._IsGermany)
                                    throw new Exception("Bitte geben Sie den gleichen Parent-Level ein..!");
                                else
                                    throw new Exception("Please enter the same Parent level..!");
                            }
                        }
                        drPos["fromPos"] = Utility.PrepareOZ(tfrom.Replace(',', '.'), ObjEProject.LVRaster);
                        drPos["toPos"] = Utility.PrepareOZ(tTo.Replace(',', '.'), ObjEProject.LVRaster);
                        dtPos.Rows.Add(drPos);
                    }
                    ObjBPosition.GetPositionOZList(ObjEPosition, ObjEProject.ProjectID, tType, dtPos);
                }


                if (ObjEPosition.dsPositionOZList != null)
                {
                    tlBulkProcessPositionDetails.DataSource = ObjEPosition.dsPositionOZList;
                    tlBulkProcessPositionDetails.DataMember = "Positions";
                    tlBulkProcessPositionDetails.ParentFieldName = "Parent_OZ";
                    tlBulkProcessPositionDetails.KeyFieldName = "PositionID";
                    tlBulkProcessPositionDetails.ForceInitialize();
                    tlBulkProcessPositionDetails.ExpandAll();
                }
                tlBulkProcessPositionDetails.BestFitColumns();
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void checkEditSectionAMulti5MA_CheckedChanged(object sender, EventArgs e)
        {
            VisibleBulkProcess_SectionA_Controls(checkEditSectionAMulti5MA, lciMulti5MA, txtMulti5MA);
        }

        private void checkEditSectionAMulti5MO_CheckedChanged(object sender, EventArgs e)
        {
            VisibleBulkProcess_SectionA_Controls(checkEditSectionAMulti5MO, lciMulti5MO, txtMulti5MO);
        }

        private void checkEditSectionAMulti6MA_CheckedChanged(object sender, EventArgs e)
        {
            VisibleBulkProcess_SectionA_Controls(checkEditSectionAMulti6MA, lciMulti6MA, txtMulti6MA);
        }

        private void checkEditSectionAMulti6MO_CheckedChanged(object sender, EventArgs e)
        {
            VisibleBulkProcess_SectionA_Controls(checkEditSectionAMulti6MO, lciMulti6MO, txtMulti6MO);
        }

        private void radioGroupActionA_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioGroupActionA.SelectedIndex == 1)
                {
                    lciMulti5MA.Enabled = false;
                    lciMulti5MO.Enabled = false;
                    lciMulti6MA.Enabled = false;
                    lciMulti6MO.Enabled = false;

                    txtMulti5MA.EditValue = null;
                    txtMulti5MO.EditValue = null;
                    txtMulti6MA.EditValue = null;
                    txtMulti6MO.EditValue = null;

                    checkEditSectionAMulti5MA.Checked = false;
                    checkEditSectionAMulti5MO.Checked = false;
                    checkEditSectionAMulti6MA.Checked = false;
                    checkEditSectionAMulti6MO.Checked = false;
                    tType = "Remove";
                }
                else
                {
                    checkEditSectionAMulti5MA.Checked = false;
                    checkEditSectionAMulti5MO.Checked = false;
                    checkEditSectionAMulti6MA.Checked = false;
                    checkEditSectionAMulti6MO.Checked = false;
                    tType = "Set";
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void radioGroupActionB_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioGroupActionB.SelectedIndex == 1)
                {
                    HideTextBoxes();
                }
                else
                {
                    checkEditPositionMenge.Checked = false;
                    checkEditMaterialKz.Checked = false;
                    checkEditMontageKZ.Checked = false;
                    checkEditPreisErstaztext.Checked = false;
                    checkEditArtikelnummerWG.Checked = false;
                    checkEditFabrikat.Checked = false;
                    checkEditTyp.Checked = false;
                    checkEditLieferantMA.Checked = false;
                    checkEditNachtragsnummer.Checked = false;

                    if (txtkommissionNumber.Text != "")
                    {
                        checkEditNachtragsnummer.Enabled = true;
                        btnSavesectionB.Enabled = true;
                    }
                    else
                    {
                        checkEditNachtragsnummer.Enabled = false;
                        btnSavesectionB.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnSaveActionA_Click(object sender, EventArgs e)
        {
            try
            {
                if (tlBulkProcessPositionDetails.DataSource == null)
                {
                    if (Utility._IsGermany == true)
                    {
                        XtraMessageBox.Show("Bitte machen Sie VON / BIS Angaben.");
                    }
                    else
                    {
                        XtraMessageBox.Show("Please enter From And To values.!");
                    }
                    return;
                }

                DataTable dtPos = new DataTable();
                dtPos.Columns.Add("ID");

                if (tlBulkProcessPositionDetails.DataSource != null)
                {
                    foreach (TreeListNode node in tlBulkProcessPositionDetails.GetNodeList())
                    {
                        string _PosKZ = node["PositionKZ"].ToString();
                        if (_PosKZ != "NG")
                        {
                            DataRow drPos = dtPos.NewRow();
                            string tID = node["PositionID"].ToString();
                            drPos["ID"] = tID;
                            dtPos.Rows.Add(drPos);
                        }
                    }
                }
                if (radioGroupActionA.SelectedIndex == 0)
                {
                    if (txtMulti5MA.Text == "")
                    {
                        txtMulti5MA.Text = "0";
                    }
                    if (txtMulti5MO.Text == "")
                    {
                        txtMulti5MO.Text = "0";
                    }
                    if (txtMulti6MA.Text == "")
                    {
                        txtMulti6MA.Text = "0";
                    }
                    if (txtMulti6MO.Text == "")
                    {
                        txtMulti6MO.Text = "0";
                    }
                }
                if (radioGroupActionA.SelectedIndex == 1)
                {

                    if (checkEditSectionAMulti5MA.Checked == true)
                    {
                        txtMulti5MA.Text = "1";
                    }
                    else
                    {
                        txtMulti5MA.Text = "0";
                    }
                    if (checkEditSectionAMulti5MO.Checked == true)
                    {
                        txtMulti5MO.Text = "1";
                    }
                    else
                    {
                        txtMulti5MO.Text = "0";
                    }
                    if (checkEditSectionAMulti6MA.Checked == true)
                    {
                        txtMulti6MA.Text = "1";
                    }
                    else
                    {
                        txtMulti6MA.Text = "0";
                    }
                    if (checkEditSectionAMulti6MO.Checked == true)
                    {
                        txtMulti6MO.Text = "1";
                    }
                    else
                    {
                        txtMulti6MO.Text = "0";
                    }
                }
                if (radioGroupActionA.SelectedIndex == 1)
                {
                    tType = "Remove";
                }
                else
                {
                    tType = "Set";
                }

                ObjBPosition.UpdateBulkProcess_ActionA(ObjEPosition, ObjEProject.ProjectID, tType,
                    Convert.ToDecimal(txtMulti5MA.Text),
                    Convert.ToDecimal(txtMulti5MO.Text),
                    Convert.ToDecimal(txtMulti6MA.Text),
                    Convert.ToDecimal(txtMulti6MO.Text),
                    dtPos);

                if (Utility._IsGermany == true)
                {
                    frmCalcPro.UpdateStatus("Vorgang abgeschlossen: Speichern der LV Positionen");
                }
                else
                {
                    frmCalcPro.UpdateStatus("LV Positions Saved Successfully");
                }
                BindPositionData();
                btnApply_Click(null, null);
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }

        }

        private void checkEditLVPositionKZ_CheckedChanged(object sender, EventArgs e)
        {
            VisibleBulkProcess_SectionB_Controls(checkEditLVPositionKZ, lciPositionKZ, txtLVPositionKZ);
        }

        private void checkEditPositionMenge_CheckedChanged(object sender, EventArgs e)
        {
            VisibleBulkProcess_SectionB_Controls(checkEditPositionMenge, lciPosMenge, txtPositionMenge);
        }

        private void checkEditMaterialKz_CheckedChanged(object sender, EventArgs e)
        {
            VisibleBulkProcess_SectionB_Controls(checkEditMaterialKz, lciMaterialKZ, txtMaterialKz);
        }

        private void checkEditMontageKZ_CheckedChanged(object sender, EventArgs e)
        {
            VisibleBulkProcess_SectionB_Controls(checkEditMontageKZ, lciMontageKZ, txtMontageKZ);
        }

        private void checkEditPreisErstaztext_CheckedChanged(object sender, EventArgs e)
        {
            VisibleBulkProcess_SectionB_Controls(checkEditPreisErstaztext, lciPreisErstaztext, txtPreisErstaztext);
        }

        private void checkEditArtikelnummerWG_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioGroupActionB.SelectedIndex == 0)
                {
                    if (checkEditArtikelnummerWG.Checked == true)
                    {
                        lciArtikelnummerWG.Enabled = true;
                        lciWarenart.Enabled = true;
                        lciWarenident.Enabled = true;
                    }
                    else
                    {
                        lciArtikelnummerWG.Enabled = false;
                        lciWarenart.Enabled = false;
                        lciWarenident.Enabled = false;
                    }
                    txtArtikelnummerWG.EditValue = null;
                    txtArtikelnummerWA.EditValue = null;
                    txtArtikelnummerWI.EditValue = null;
                }
                else
                {
                    lciPositionKZ.Enabled = false;
                    lciPosMenge.Enabled = false;
                    lciMaterialKZ.Enabled = false;
                    lciMontageKZ.Enabled = false;
                    lciPreisErstaztext.Enabled = false;
                    lciArtikelnummerWG.Enabled = false;
                    lciWarenart.Enabled = false;
                    lciWarenident.Enabled = false;
                    lciFabrikat.Enabled = false;
                    lciTyp.Enabled = false;
                    lciLieferantMA.Enabled = false;
                    lciNachtragsnummer.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void checkEditFabrikat_CheckedChanged(object sender, EventArgs e)
        {
            VisibleBulkProcess_SectionB_Controls(checkEditFabrikat, lciFabrikat, txtFabrikat);
        }

        private void checkEditTyp_CheckedChanged(object sender, EventArgs e)
        {
            VisibleBulkProcess_SectionB_Controls(checkEditTyp, lciTyp, txtTyp);
        }

        private void checkEditLieferantMA_CheckedChanged(object sender, EventArgs e)
        {
            VisibleBulkProcess_SectionB_Controls(checkEditLieferantMA, lciLieferantMA, txtBulkLieferantMA);
        }

        private void checkEditNachtragsnummer_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkEditNachtragsnummer.Checked == true)
                {
                    btnSavesectionB.Enabled = true;
                }
                VisibleBulkProcess_SectionB_Controls(checkEditNachtragsnummer, lciNachtragsnummer, txtNachtragsnummer);
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnSavesectionB_Click(object sender, EventArgs e)
        {
            try
            {
                if (tlBulkProcessPositionDetails.DataSource == null)
                {
                    if (Utility._IsGermany == true)
                    {
                        XtraMessageBox.Show("Bitte machen Sie VON / BIS Angaben.");
                    }
                    else
                    {
                        XtraMessageBox.Show("Please enter From And To values.!");
                    }
                    return;
                }

                DataTable dtPos = new DataTable();
                dtPos.Columns.Add("ID");

                if (tlBulkProcessPositionDetails.DataSource != null)
                {
                    foreach (TreeListNode node in tlBulkProcessPositionDetails.GetNodeList())
                    {
                        string _PosKZ = node["PositionKZ"].ToString();
                        if (_PosKZ != "NG")
                        {
                            DataRow drPos = dtPos.NewRow();
                            string tID = node["PositionID"].ToString();
                            drPos["ID"] = tID;
                            dtPos.Rows.Add(drPos);
                        }
                    }

                }
                if (radioGroupActionB.SelectedIndex == 0)
                {
                    if (checkEditArtikelnummerWG.Checked == true)
                    {
                        if (txtArtikelnummerWG.Text == "")
                        {
                            if (Utility._IsGermany == true)
                            {
                                XtraMessageBox.Show("Bitte machen Sie eine Angabe zu WG.");
                            }
                            else
                            {
                                XtraMessageBox.Show("Please enter WG value");
                            }
                            return;
                        }
                        if (txtArtikelnummerWA.Text == "")
                        {
                            if (Utility._IsGermany == true)
                            {
                                XtraMessageBox.Show("Bitte machen Sie eine Angabe zu WA.");
                            }
                            else
                            {
                                XtraMessageBox.Show("Please enter WA value");
                            }
                            return;
                        }
                    }
                }
                else
                {
                    if (checkEditPositionMenge.Checked == true)
                    {
                        txtPositionMenge.Text = "1";
                    }
                    else
                    {
                        txtPositionMenge.Text = "";
                    }
                    if (checkEditMaterialKz.Checked == true)
                    {
                        txtMaterialKz.Text = "X";
                    }
                    else
                    {
                        txtMaterialKz.Text = "";
                    }
                    if (checkEditMontageKZ.Checked == true)
                    {
                        txtMontageKZ.Text = "X";
                    }
                    else
                    {
                        txtMontageKZ.Text = "";
                    }
                    if (checkEditPreisErstaztext.Checked == true)
                    {
                        txtPreisErstaztext.Text = "0";
                    }
                    else
                    {
                        txtPreisErstaztext.Text = "";
                    }
                    if (checkEditFabrikat.Checked == true)
                    {
                        txtFabrikat.Text = "0";
                    }
                    else
                    {
                        txtFabrikat.Text = "";
                    }
                    if (checkEditTyp.Checked == true)
                    {
                        txtTyp.Text = "0";
                    }
                    else
                    {
                        txtTyp.Text = "";
                    }
                    if (checkEditLieferantMA.Checked == true)
                    {
                        txtBulkLieferantMA.Text = "0";
                    }
                    else
                    {
                        txtBulkLieferantMA.Text = "";
                    }
                    if (checkEditArtikelnummerWG.Checked == true)
                    {
                        txtArtikelnummerWG.Text = "0";
                        txtArtikelnummerWA.Text = "0";
                        txtArtikelnummerWI.Text = "0";
                    }
                    else
                    {
                        txtArtikelnummerWG.Text = "";
                        txtArtikelnummerWA.Text = "";
                        txtArtikelnummerWI.Text = "";
                    }

                    if (checkEditNachtragsnummer.Checked == true)
                    {
                        txtNachtragsnummer.Text = "";
                    }
                    else
                    {
                        txtNachtragsnummer.Text = "";
                    }
                }

                if (txtNachtragsnummer.Text == "NT" || txtNachtragsnummer.Text == "NTM")
                {
                    if (Utility._IsGermany == true)
                    {
                        throw new Exception("Positionen mit NT oder NTM kÃ¶nnen nicht gespeichert werden");
                    }
                    else
                    {
                        throw new Exception("Cannot Save Positions With NT Or NTM");
                    }
                }

                ObjBPosition.UpdateBulkProcess_ActionB(ObjEPosition, ObjEProject.ProjectID, tType, txtPositionMenge.Text,
                    txtMaterialKz.Text, txtMontageKZ.Text,
                    txtPreisErstaztext.Text, txtFabrikat.Text, txtTyp.Text, txtBulkLieferantMA.Text, txtArtikelnummerWG.Text,
                    txtArtikelnummerWA.Text, txtArtikelnummerWI.Text, txtNachtragsnummer.Text, dtPos);

                if (Utility._IsGermany == true)
                {
                    frmCalcPro.UpdateStatus("Vorgang abgeschlossen: Speichern der LV Positionen");
                }
                else
                {
                    frmCalcPro.UpdateStatus("LV Positions Saved Successfully");
                }
                btnApply_Click(null, null);
                ObjBProject.GetProjectDetails(ObjEProject);
                if (ObjEProject.dtLVSection != null && ObjEProject.dtLVSection.Rows.Count > 0)
                {
                    cmbLVSection.Properties.DataSource = ObjEProject.dtLVSection;
                    cmbLVSection.Properties.DisplayMember = "LVSectionName";
                    cmbLVSection.Properties.ValueMember = "LVSectionID";
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtMaterialKz_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtMaterialKz.Text.ToLower() == "s")
                {
                    txtMontageKZ.EditValue = null;
                    txtMontageKZ.Enabled = false;
                }
                else
                {
                    txtMontageKZ.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gvAddRemovePositions_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                System.Windows.Forms.TextBox tb = e.Control as System.Windows.Forms.TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(DataGrid_KeyPress);
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void DataGrid_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (cmbBulkProcessSelection.SelectedIndex == 0)
                {
                    if ((e.KeyChar != (char)Keys.Back) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                    {
                        e.Handled = true;
                    }
                }
                else
                {
                    e.Handled = false;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void cmbBulkProcessSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbBulkProcessSelection.SelectedIndex == 2)
                {
                    gvAddRemovePositions.Enabled = false;
                    txtBulkProcessWG.Enabled = true;
                    txtBulkProcessWA.Enabled = true;
                }
                else
                {
                    gvAddRemovePositions.Enabled = true;
                    txtBulkProcessWG.Enabled = false;
                    txtBulkProcessWA.Enabled = false;
                }
                if (ObjEPosition.dsPositionOZList != null)
                {
                   // gvAddRemovePositions.DefaultView.DataSource = null;
                    tlBulkProcessPositionDetails.DataSource = null;
                }

                radioGroupActionA.SelectedIndex = 1;
                radioGroupActionB.SelectedIndex = 1;
                radioGroupActionA_SelectedIndexChanged(null, null);
                radioGroupActionB_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
            txtBulkProcessWG.Text = "";
            txtBulkProcessWA.Text = "";
        }

      

      

      

        #endregion

        #region Functions

        /// <summary>
        ///  Code to Assign filter type
        /// </summary>
        private void AssignPosition_type()
        {
            try
            {
                if (cmbBulkProcessSelection.SelectedIndex == 0)
                {
                    tType = "Parent Position";
                }
                if (cmbBulkProcessSelection.SelectedIndex == 1)
                {
                    tType = "LV Position";
                }
                if (cmbBulkProcessSelection.SelectedIndex == 2)
                {
                    tType = "WG/WA";
                }
                if (cmbBulkProcessSelection.SelectedIndex == 3)
                {
                    tType = "Supplier";
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        ///  Code set visible property for controls on bulk proccess tab Section A
        /// </summary>
        /// <param name="tCheckEditEdit"></param>
        /// <param name="tLayoutItem"></param>
        /// <param name="tText"></param>
        private void VisibleBulkProcess_SectionA_Controls(CheckEdit tCheckEditEdit, LayoutControlItem tLayoutItem, TextEdit tText)
        {
            try
            {

                if (radioGroupActionA.SelectedIndex == 0)
                {
                    if (tCheckEditEdit.Checked == true)
                    {
                        tLayoutItem.Enabled = true;
                        tText.EditValue = null;
                    }
                    else
                    {
                        tLayoutItem.Enabled = false;
                        tText.EditValue = null;
                    }
                }
                else
                {
                    lciMulti5MA.Enabled = false;
                    lciMulti5MO.Enabled = false;
                    lciMulti6MA.Enabled = false;
                    lciMulti6MO.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        /// <summary>
        /// Code to hide controls on bulk procces tab
        /// </summary>
        private void HideTextBoxes()
        {
            lciPositionKZ.Enabled = false;
            lciPosMenge.Enabled = false;
            lciMaterialKZ.Enabled = false;
            lciMontageKZ.Enabled =  false;
            lciPreisErstaztext.Enabled = false;
            lciArtikelnummerWG.Enabled = false;
            lciWarenart.Enabled = false;
            lciWarenident.Enabled = false;
            lciFabrikat.Enabled = false;
            lciTyp.Enabled = false;
            lciLieferantMA.Enabled = false;
            lciNachtragsnummer.Enabled = false;

            checkEditPositionMenge.Checked = false;
            checkEditMaterialKz.Checked = false;
            checkEditMontageKZ.Checked = false;
            checkEditPreisErstaztext.Checked = false;
            checkEditArtikelnummerWG.Checked = false;
            checkEditFabrikat.Checked = false;
            checkEditTyp.Checked = false;
            checkEditLieferantMA.Checked = false;
            checkEditNachtragsnummer.Checked = false;
            if (txtkommissionNumber.Text != "")
            {
                checkEditNachtragsnummer.Enabled = false;
                btnSavesectionB.Enabled = false;
            }
            else
            {
                checkEditNachtragsnummer.Enabled = false;
                btnSavesectionB.Enabled = true;
            }

        }

        /// <summary>
        ///  Code set visible property for controls on bulk proccess tab Section B
        /// </summary>
        /// <param name="tCheckEditEdit"></param>
        /// <param name="tLayoutItem"></param>
        /// <param name="tText"></param>
        private void VisibleBulkProcess_SectionB_Controls(CheckEdit tCheckEditEdit, LayoutControlItem tLayoutItem, TextEdit tText)
        {
            try
            {
                if (radioGroupActionB.SelectedIndex == 0)
                {
                    if (tCheckEditEdit.Checked == true)
                    {
                        tLayoutItem.Enabled = true;
                        tText.EditValue = null;
                    }
                    else
                    {
                        tLayoutItem.Enabled = false;
                        tText.EditValue = null;
                    }
                }
                else
                {
                    lciPositionKZ.Enabled = false;
                    lciPosMenge.Enabled = false;
                    lciMaterialKZ.Enabled = false;
                    lciMontageKZ.Enabled = false;
                    lciPreisErstaztext.Enabled = false;
                    lciArtikelnummerWG.Enabled = false;
                    lciWarenart.Enabled = false;
                    lciWarenident.Enabled = false;
                    lciFabrikat.Enabled = false;
                    lciTyp.Enabled = false;
                    lciLieferantMA.Enabled = false;
                    lciNachtragsnummer.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        #endregion

        #endregion

        #region MULTIES

        #region Events
        private void btnMulti5LoadArticles_Click(object sender, EventArgs e)
        {
            try
            {
                if (ObjEMulti == null)
                    ObjEMulti = new EMulti();
                if (ObjBMulti == null)
                    ObjBMulti = new BMulti();
                ObjEMulti.ProjectID = ObjEProject.ProjectID;
                ObjEMulti.LVSection = cmbLVSectionFilter.Properties.GetCheckedItems();
                if (!string.IsNullOrEmpty(Convert.ToString(ObjEMulti.LVSection)))
                {
                    ObjEMulti = ObjBMulti.GetArticleGroups(ObjEMulti);
                    gcMulti5.DataSource = ObjEMulti.dtArticles;
                }
            }
            catch (Exception ex) { Utility.ShowError(ex); }
        }

        private void btnMulti5UpdateSelbekosten_Click(object sender, EventArgs e)
        {
            try
            {
                if (ObjEMulti == null)
                    ObjEMulti = new EMulti();
                if (ObjBMulti == null)
                    ObjBMulti = new BMulti();
                ObjEMulti.ProjectID = ObjEProject.ProjectID;
                ObjEMulti.LVSection = cmbLVSectionFilter.Properties.GetCheckedItems();
                ObjEMulti = ObjBMulti.UpdateMulti5(ObjEMulti);
                if (Utility._IsGermany == true)
                {
                    frmCalcPro.UpdateStatus("Vorgang abgeschlossen: Speichern der Selbstkosten");
                }
                else
                {
                    frmCalcPro.UpdateStatus("Selbstkosten Saved Successfully");
                }
            }
            catch (Exception EX)
            {
                Utility.ShowError(EX);
            }
        }

        private void gvMulti5_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                if (ObjEMulti == null)
                    ObjEMulti = new EMulti();

                if (ObjEMulti.dtArticles != null && ObjEMulti.dtArticles.Rows.Count > 0)
                {
                    string strTag = (e.Item as GridSummaryItem).Tag.ToString();
                    if (strTag.ToLower() == "xfactor")
                    {
                        e.TotalValue = GetWeightedPercentage("XValue", strTag);
                    }
                    else if (strTag.ToLower() == "sfactor")
                    {
                        e.TotalValue = GetWeightedPercentage("SValue", strTag);
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void cmbLVSectionFilter_Closed(object sender, ClosedEventArgs e)
        {
            try
            {
                btnMulti5LoadArticles_Click(null, null);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmbLVSectionMulti5_Closed(object sender, ClosedEventArgs e)
        {
            try
            {
                if (ObjEMulti == null)
                    ObjEMulti = new EMulti();
                if (ObjBMulti == null)
                    ObjBMulti = new BMulti();
                ObjEMulti.ProjectID = ObjEProject.ProjectID;
                ObjEMulti.LVSection = cmbLVSectionMulti5.EditValue;
                ObjEMulti = ObjBMulti.GetSOldMultis(ObjEMulti);
                gcSoldValues.DataSource = ObjEMulti.dtSoldMultis;
            }
            catch (Exception ex) { }
        }

        private void gvMulti5_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                view.UpdateTotalSummary();
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gvMulti5_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    gvMulti5.MoveNext();
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gvMulti5_CustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e)
        {
            try
            {
                var gv = sender as GridView;
                object _XField = gv.GetRowCellValue(e.RowHandle, gv.Columns["XValue"]);
                gv.SetRowCellValue(e.RowHandle, "XValue", string.Format("{0:N8}", _XField));

                object _SField = gv.GetRowCellValue(e.RowHandle, gv.Columns["SValue"]);
                gv.SetRowCellValue(e.RowHandle, "SValue", string.Format("{0:N8}", _SField));
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnMulti6LoadArticles_Click(object sender, EventArgs e)
        {
            try
            {
                if (ObjEMulti == null)
                    ObjEMulti = new EMulti();
                if (ObjBMulti == null)
                    ObjBMulti = new BMulti();
                ObjEMulti.ProjectID = ObjEProject.ProjectID;
                ObjEMulti.LVSection = cmbMulti6LVFilter.Properties.GetCheckedItems();
                ObjEMulti.Type = cmbType.Text;
                if (!string.IsNullOrEmpty(Convert.ToString(ObjEMulti.LVSection)))
                {
                    ObjEMulti = ObjBMulti.GetArticleGroupsForMulti6(ObjEMulti);
                    gcMulti6.DataSource = ObjEMulti.dtArticles;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void cmbMulti6LVFilter_Closed(object sender, ClosedEventArgs e)
        {
            btnMulti6LoadArticles_Click(null, null);
        }

        private void cmbType_SelectedValueChanged(object sender, EventArgs e)
        {
            btnMulti6LoadArticles_Click(null, null);
        }

        private void gvMulti6_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    gvMulti6.MoveNext();
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnMulti6UpdateSelbekosten_Click(object sender, EventArgs e)
        {
            try
            {
                if (ObjEMulti == null)
                    ObjEMulti = new EMulti();
                if (ObjBMulti == null)
                    ObjBMulti = new BMulti();
                ObjEMulti.ProjectID = ObjEProject.ProjectID;
                ObjEMulti.LVSection = cmbMulti6LVFilter.EditValue;
                ObjEMulti.Type = cmbType.Text;
                ObjEMulti = ObjBMulti.UpdateMulti6(ObjEMulti);
                if (Utility._IsGermany == true)
                    frmCalcPro.UpdateStatus("Vorgang abgeschlossen: Speichern der Verkaufskosten");
                else
                    frmCalcPro.UpdateStatus("Verkaufskosten Saved Successfully");
            }
            catch (Exception EX) { Utility.ShowError(EX); }
        }

        private void cmbLvSectionMulti6_Closed(object sender, ClosedEventArgs e)
        {
            try
            {
                if (cmbLvSectionMulti6.Text == string.Empty)
                    return;
                if (cmbISMaterialMulti6.Text == string.Empty)
                    return;
                if (ObjEMulti == null)
                    ObjEMulti = new EMulti();
                if (ObjBMulti == null)
                    ObjBMulti = new BMulti();
                ObjEMulti.ProjectID = ObjEProject.ProjectID;
                ObjEMulti.LVSection = cmbLvSectionMulti6.EditValue;
                if (cmbISMaterialMulti6.Text == "Material")
                    ObjEMulti.ISMaterial = true;
                else
                    ObjEMulti.ISMaterial = false;
                ObjEMulti = ObjBMulti.GetVOldMultis(ObjEMulti);
                gcVOldMultis.DataSource = ObjEMulti.dtVoldMultis;
            }
            catch (Exception ex) { }
        }

        private void cmbISMaterialMulti6_QueryCloseUp(object sender, CancelEventArgs e)
        {
            cmbLvSectionMulti6_Closed(null, null);
        }
        #endregion

        #region Functions

        /// <summary>
        /// Code to calculate weighted percentage in Multi 5 and Multi 6
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="strFactor"></param>
        /// <returns></returns>
        private decimal GetWeightedPercentage(string strValue, string strFactor)
        {
            decimal newTotalValue = 0;
            decimal OldTotalValue = 0;
            decimal TotalValue = 0;
            try
            {
                foreach (DataRow dr in ObjEMulti.dtArticles.Rows)
                {
                    decimal XValue = 0;
                    decimal xFactor = 0;
                    if (decimal.TryParse(dr[strFactor].ToString(), out xFactor))
                    {
                        if (xFactor == 0)
                        {
                            if (decimal.TryParse(dr[strValue].ToString(), out XValue))
                            {
                                newTotalValue += XValue;
                            }
                        }
                        else
                        {
                            if (decimal.TryParse(dr[strValue].ToString(), out XValue))
                            {
                                newTotalValue += (XValue * xFactor);
                            }
                        }
                    }
                    else
                    {
                        if (decimal.TryParse(dr[strValue].ToString(), out XValue))
                        {
                            newTotalValue += XValue;
                        }
                    }
                    XValue = 0;
                    if (decimal.TryParse(dr[strValue].ToString(), out XValue))
                    {
                        OldTotalValue += XValue;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            if (OldTotalValue > 0)
            {
                TotalValue = ((newTotalValue / OldTotalValue) - 1) * 100;
            }
            return TotalValue;
        }

        #endregion

        #endregion

        #region Special Cost
        private void btnAddedCost_Click(object sender, EventArgs e)
        {
            try
            {
                if (ObjEUmlage == null)
                    ObjEUmlage = new EUmlage();
                if (ObjBUmlage == null)
                    ObjBUmlage = new BUmlage();
                ObjEUmlage.ProjectID = ObjEProject.ProjectID;
                if (ObjEUmlage.dtSpecialCost.Rows.Count > 0)
                {
                    btnUmlageSave_Click(null, null);
                    ObjEUmlage = ObjBUmlage.UpdateSpecialCost(ObjEUmlage);
                    if (Utility._IsGermany == true)
                        frmCalcPro.UpdateStatus("Vorgang abgeschlossen: Aktualisierung der Umlage");
                    else
                        frmCalcPro.UpdateStatus("Umlage Updated Successfully");
                }
                else
                {
                    if (Utility._IsGermany == true)
                        throw new Exception("Bitte fügen Sie Generelle Kosten zur Verteilung hinzu");
                    else
                        throw new Exception("Add special cost to distribute");
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnUmlageSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ObjEUmlage == null)
                    ObjEUmlage = new EUmlage();
                if (ObjBUmlage == null)
                    ObjBUmlage = new BUmlage();
                ObjEUmlage.ProjectID = ObjEProject.ProjectID;

                if (rgUmlageMode.SelectedIndex == 1)
                {
                    var summaryValue = gvOmlage.Columns["Price"].SummaryItem.SummaryValue;
                    double dValue = 0;
                    if (double.TryParse(Convert.ToString(summaryValue), out dValue))
                    {
                        if (dValue > 100)
                            throw new Exception("Value cannot be morethan 100");
                    }
                    else
                        throw new Exception("Please enter valid value");
                }

                ObjEUmlage.UmlageMode = rgUmlageMode.SelectedIndex;
                if (ObjEUmlage.dtSpecialCost.Rows.Count > 0)
                {
                    ObjEUmlage = ObjBUmlage.SaveSpecialCost(ObjEUmlage);
                    txtUmlageValue.Text = ObjEUmlage.UmlageValue.ToString();
                    txtUmlageFactor.Text = ObjEUmlage.UmlageFactor.ToString();
                    if (Utility._IsGermany == true)
                        frmCalcPro.UpdateStatus("Vorgang abgeschlossen: Speichern der erfassten Generalkosten");
                    else
                        frmCalcPro.UpdateStatus("Umlage Values Saved Successfully");
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnUmlageExport_Click(object sender, EventArgs e)
        {
            try
            {
                string filePath = @"D:\Umlage.xlsx";
                XlsxExportOptionsEx opt = new XlsxExportOptionsEx();
                gvOmlage.ExportToXlsx(filePath, opt);
                gvOmlage.BestFitColumns();
                Process.Start(filePath);
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void rgUmlageMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rgUmlageMode.SelectedIndex == 1)
                {
                    rpumlagePrice.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                    rpumlagePrice.Mask.ShowPlaceHolders = false;
                    rpumlagePrice.Mask.UseMaskAsDisplayFormat = true;
                    rpumlagePrice.Mask.EditMask = "P1";
                    GridColumn col = gvOmlage.Columns["Price"];
                    col.SummaryItem.DisplayFormat = "SUM={0:n1}";
                }
                else if (rgUmlageMode.SelectedIndex == 0)
                {
                    rpumlagePrice.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                    rpumlagePrice.Mask.ShowPlaceHolders = false;
                    rpumlagePrice.Mask.UseMaskAsDisplayFormat = true;
                    rpumlagePrice.Mask.EditMask = "C2";
                    GridColumn col = gvOmlage.Columns["Price"];
                    col.SummaryItem.DisplayFormat = "SUM={0:n2}";
                }
            }
            catch (Exception ex) { }
        }

        #endregion

        #region "Delivery Notes Code"

        #region Events
        private void gcPositions_DragOver(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(typeof(DataRow)))
                    e.Effect = DragDropEffects.Move;
                else
                    e.Effect = DragDropEffects.None;
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gcPositions_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (Utility.DeliveryAccess == "7" && ObjEProject.IsFinalInvoice)
                    return;
                int MaxValue = 0;
                GridControl grid = sender as GridControl;
                DataTable table = grid.DataSource as DataTable;

                foreach (int i in gvPositions.GetSelectedRows())
                {
                    DataRow row = gvPositions.GetDataRow(i);
                    if (row != null && table != null && row.Table != table)
                    {
                        object strPositionID = row["PositionID"].ToString();
                        DataRow[] foundRows = table.Select("PositionID = '" + strPositionID + "'");
                        if (table.Rows.Count > 0)
                        {
                            MaxValue = Convert.ToInt32(table.Compute("MAX([SNO])", string.Empty));
                        }
                        if (foundRows.Count() <= 0)
                        {

                            DataRow drTemp = table.NewRow();
                            drTemp.ItemArray = row.ItemArray.Clone() as object[];
                            drTemp["Menge"] = 0;
                            drTemp["SNO"] = MaxValue + 1;
                            table.Rows.Add(drTemp);
                            Utility.Setfocus(gvDelivery, "PositionID", Convert.ToInt32(strPositionID));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gvPositions_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                if (e.Button == MouseButtons.Left && downHitInfo != null)
                {
                    Size dragSize = SystemInformation.DragSize;
                    Rectangle dragRect = new Rectangle(new Point(downHitInfo.HitPoint.X - dragSize.Width / 2,
                        downHitInfo.HitPoint.Y - dragSize.Height / 2), dragSize);

                    if (!dragRect.Contains(new Point(e.X, e.Y)))
                    {
                        DataRow row = view.GetDataRow(downHitInfo.RowHandle);
                        view.GridControl.DoDragDrop(row, DragDropEffects.Move);
                        downHitInfo = null;
                        DevExpress.Utils.DXMouseEventArgs.GetMouseArgs(e).Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gvPositions_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                downHitInfo = null;
                GridHitInfo hitInfo = view.CalcHitInfo(new Point(e.X, e.Y));
                if (Control.ModifierKeys != Keys.None) return;
                if (e.Button == MouseButtons.Left && hitInfo.RowHandle >= 0)
                    downHitInfo = hitInfo;
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gvDelivery_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (gvDelivery.GetFocusedRowCellValue("Position_OZ") != null)
                {
                    string strPosition = gvDelivery.GetFocusedRowCellValue("Position_OZ") == DBNull.Value ? "" : gvDelivery.GetFocusedRowCellValue("Position_OZ").ToString();
                    lblSelectedLVPosition.Text = "Ausgewählte LV Positionen  : " + strPosition;
                    txtOrderedQnty.Text = gvDelivery.GetFocusedRowCellValue("OrderedQuantity") == DBNull.Value ? "" : gvDelivery.GetFocusedRowCellValue("OrderedQuantity").ToString();
                    txtDeliveredQnty.Text = gvDelivery.GetFocusedRowCellValue("DeliveredQuantity") == DBNull.Value ? "" : gvDelivery.GetFocusedRowCellValue("DeliveredQuantity").ToString();
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            try
            {
                if (string.IsNullOrEmpty(txtBlattNumber.Text))
                    if (!Utility._IsGermany)
                    {
                        throw new Exception("Bitte geben Sie eine BLATT Nummer an");
                    }
                    else
                    {
                        throw new Exception("Please Enter Blatt Number");
                    }
                if (gvDelivery.RowCount == 0)
                {
                    if (!Utility._IsGermany)
                        throw new Exception("Select Positions for Delivery");
                    else
                        throw new Exception("Bitte wählen Sie LV Positionen für das Aufmass");
                }
                if (!chkActiveDelivery.Checked)
                {
                    string strConfirmation = XtraMessageBox.Show("Wollen Sie dies als nicht-aktives Aufmass abspeichern?", "Frage"
                            , MessageBoxButtons.OKCancel, MessageBoxIcon.Question).ToString();
                    if (strConfirmation.ToLower() == "cancel")
                        return;
                }
                if (ObjEDeliveryNotes == null)
                    ObjEDeliveryNotes = new EDeliveryNotes();
                if (ObjBDeliveryNotes == null)
                    ObjBDeliveryNotes = new BDeliveryNotes();
                ObjEDeliveryNotes.BlattName = txtBlattNumber.Text;
                ObjEDeliveryNotes.ISActiveDelivery = chkActiveDelivery.Checked == true ? true : false;
                DataTable table = gcDelivery.DataSource as DataTable;

                DataTable Temp = table.Copy();
                Temp.Columns.Remove("Position_OZ");
                Temp.Columns.Remove("PositionKZ");
                Temp.Columns.Remove("Ordered");
                Temp.Columns.Remove("ShortDescription");
                Temp.Columns.Remove("OrderedQuantity");
                Temp.Columns.Remove("RemainingQuantity");
                Temp.Columns.Remove("DeliveredQuantity");
                Temp.Columns.Remove("LVSection");
                Temp.Columns.Remove("LVStatus");
                ObjEDeliveryNotes.dtDelivery = Temp;

                ObjEDeliveryNotes = ObjBDeliveryNotes.SaveDelivery(ObjEDeliveryNotes);
                frmCalcPro.UpdateStatus("'" + txtBlattNumber.Text + "'" + " wurde erfolgreich gespeichert");
                if (chkActiveDelivery.Checked == false)
                    LoadNonActiveDelivery();
                else
                {
                    table.Rows.Clear();
                    txtDeliveredQnty.Text = string.Empty;
                    txtOrderedQnty.Text = string.Empty;
                    lblSelectedLVPosition.Text = "Ausgewählte LV Positionen  : ";
                    gcDeliveryNumbers.DataSource = ObjEDeliveryNotes.dtBlattNumbers;
                    gcPositions.DataSource = ObjEDeliveryNotes.dtPositions;
                    Utility.Setfocus(gvDeliveryNumbers, "BlattID", ObjEDeliveryNotes.BlattID);
                    ObjEDeliveryNotes.BlattID = -1;
                    SNO = 1;
                    NewBlattnumber();
                }

            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gvDelivery_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                double dValue = 0;
                double OrderedQuantity = 0;
                double RemainingQuantity = 0;
                double notSavedQnty = 0;
                double AvailedQnty = 0;
                double DeliveredQnty = 0;

                string sPositionID = gvDelivery.GetFocusedRowCellValue("PositionID").ToString();
                if (double.TryParse(gvDelivery.GetFocusedRowCellValue("OrderedQuantity").ToString(), out dValue))
                    OrderedQuantity = dValue;
                if (double.TryParse(gvDelivery.GetFocusedRowCellValue("RemainingQuantity").ToString(), out dValue))
                    RemainingQuantity = dValue;
                if (double.TryParse(gvDelivery.GetFocusedRowCellValue("DeliveredQuantity").ToString(), out dValue))
                    DeliveredQnty = dValue;
                DataTable table = gcDelivery.DataSource as DataTable;
                int i = e.RowHandle;
                string str = table.Rows[i][e.Column.FieldName] == DBNull.Value ? "" : table.Rows[i][e.Column.FieldName].ToString();
                if (string.IsNullOrEmpty(str))
                    table.Rows[i][e.Column.FieldName] = 0;
                notSavedQnty = Convert.ToDouble(table.Compute("SUM(Menge)", "PositionID = " + sPositionID));
                AvailedQnty = RemainingQuantity + (OrderedQuantity * 0.1);
                if (DeliveredQnty + notSavedQnty < 0)
                {
                    table.Rows[i][e.Column.FieldName] = 0;
                    throw new Exception("Die Gesamt-Menge ist nicht vereinbar mit der Liefermenge");
                }
                else
                {
                    if (notSavedQnty != 0 && notSavedQnty > AvailedQnty)
                    {
                        if (!Utility._IsGermany)
                            XtraMessageBox.Show("Total Quantity Exceeding Ordered Quantity");
                        else
                            XtraMessageBox.Show("Die Gesamtmenge übersteigt die beauftragte Menge");
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gvDelivery_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            try
            {
                if (e.HitInfo.InRow)
                    e.Menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("Löschen", gcDeliveryDelete_ItemClick));
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gcDeliveryDelete_ItemClick(object sender, EventArgs e)
        {
            try
            {
                int iRowHandle = gvDelivery.FocusedRowHandle;
                DataTable table = gcDelivery.DataSource as DataTable;
                table.Rows.RemoveAt(iRowHandle);

                int SNo = 1;
                foreach (DataRow row in table.Rows)
                {
                    row["SNO"] = SNo;
                    SNo++;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gvDeliveryNumbers_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            try
            {
                if (e.HitInfo.InRow)
                {
                    if (gvDeliveryNumbers.FocusedRowHandle != null)
                    {
                        if (Utility.DeliveryAccess != "7" && !ObjEProject.IsFinalInvoice)
                            e.Menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("Ändern", gcBlattEdit_Click));
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gcBlattEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvDeliveryNumbers.FocusedRowHandle != null)
                {
                    int IValue = 0;
                    string strBlattID = gvDeliveryNumbers.GetFocusedRowCellValue("BlattID").ToString();
                    if (int.TryParse(strBlattID, out IValue))
                    {
                        if (ObjEDeliveryNotes == null)
                            ObjEDeliveryNotes = new EDeliveryNotes();
                        if (ObjBDeliveryNotes == null)
                            ObjBDeliveryNotes = new BDeliveryNotes();
                        ObjEDeliveryNotes.BlattID = IValue;
                        ObjEDeliveryNotes.BlattName = gvDeliveryNumbers.GetFocusedRowCellValue("BlattNumber").ToString();
                        txtBlattNumber.Text = ObjEDeliveryNotes.BlattName;
                        ObjEDeliveryNotes = ObjBDeliveryNotes.GetBlattDetails(ObjEDeliveryNotes);
                        gcDelivery.DataSource = ObjEDeliveryNotes.dtNonActivedelivery;
                        gcPositions.DataSource = ObjEDeliveryNotes.dtPositions;
                        SNO = ObjEDeliveryNotes.dtNonActivedelivery.Rows.Count;
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gvPositions_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (Utility.DeliveryAccess == "7" && ObjEProject.IsFinalInvoice)
                    return;
                int MaxValue = 0;
                DataView dvTable = (DataView)gvDelivery.DataSource;
                DataTable table = dvTable.ToTable();
                foreach (int i in gvPositions.GetSelectedRows())
                {
                    DataRow row = gvPositions.GetDataRow(i);
                    if (row != null && table != null && row.Table != table)
                    {
                        object strPositionID = row["PositionID"].ToString();
                        DataRow[] foundRows = table.Select("PositionID = '" + strPositionID + "'");
                        if (table.Rows.Count > 0)
                        {
                            MaxValue = Convert.ToInt32(table.Compute("MAX([SNO])", string.Empty));
                        }
                        if (foundRows.Count() <= 0)
                        {
                            DataRow drTemp = table.NewRow();
                            drTemp.ItemArray = row.ItemArray.Clone() as object[];
                            drTemp["Menge"] = 0;
                            drTemp["SNO"] = MaxValue + 1;
                            table.Rows.Add(drTemp);
                            Utility.Setfocus(gvDelivery, "PositionID", Convert.ToInt32(strPositionID));
                        }
                    }
                }
                gcDelivery.DataSource = table;
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        #endregion

        #region Functions

        /// <summary>
        /// Code to load non active deliveries
        /// </summary>
        private void LoadNonActiveDelivery()
        {
            try
            {
                ObjEDeliveryNotes = ObjBDeliveryNotes.GetNonActiveDelivery(ObjEDeliveryNotes);
                if (ObjEDeliveryNotes.dtNonActivedelivery.Rows.Count > 0)
                {
                    txtBlattNumber.Text = ObjEDeliveryNotes.BlattName;
                    chkActiveDelivery.Checked = false;
                    gcDelivery.DataSource = ObjEDeliveryNotes.dtNonActivedelivery;
                    SNO = ObjEDeliveryNotes.dtNonActivedelivery.Rows.Count;
                }
                else
                {
                    gcDelivery.DataSource = ObjEDeliveryNotes.dtPositions.Clone();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Code to fetch new blatt number
        /// </summary>
        private void NewBlattnumber()
        {
            try
            {
                ObjEDeliveryNotes = ObjBDeliveryNotes.GetNewBlattNumber(ObjEDeliveryNotes);
                txtBlattNumber.Text = ObjEDeliveryNotes.BlattName;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #endregion

        #region "Invoice Changes"
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtInvoiceNumber.Text))
                    throw new Exception("Bitte geben Sie eine Rechnungsnummer ein");
                if (ObjEInvoice == null)
                    ObjEInvoice = new EInvoice();
                if (oBJBInvoice == null)
                    oBJBInvoice = new BInvoice();
                ObjEInvoice.InvoiceID = -1;
                ObjEInvoice.ProjectID = ObjEProject.ProjectID;
                ObjEInvoice.InvoiceNumber = txtInvoiceNumber.Text;
                ObjEInvoice.InvoiceDescription = txtInvoiceDescription.Text;
                ObjEInvoice.InvoiceType = txtInvoiceType.Text;
                ObjEInvoice.IsFinalInvoice = chkFinalInvoice.Checked == true ? true : false;
                DataTable dtTemp = ObjEInvoice.dtBlattNumbers.Copy();
                dtTemp.Columns.Remove("BlattNumber");
                dtTemp.Columns.Remove("IsActiveDelivery");
                dtTemp.Columns.Remove("IsInvoiced");
                dtTemp.Columns.Remove("CreatedBy");
                dtTemp.Columns.Remove("CreatedDate");
                dtTemp.Columns.Remove("NoOfPositions");
                ObjEInvoice.dtInvoice = dtTemp;
                ObjEInvoice = oBJBInvoice.SaveInvoice(ObjEInvoice);
                gcDeliveryNotes.DataSource = ObjEInvoice.dtBlattNumbers;
                gcInvoices.DataSource = ObjEInvoice.dtInvoices;
                if (Utility._IsGermany == true)
                {
                    frmCalcPro.UpdateStatus("'" + txtInvoiceNumber.Text + "' Vorgang abgeschlossen: Speichern der Rechnung");
                }
                else
                {
                    frmCalcPro.UpdateStatus("'" + txtInvoiceNumber.Text + "' Invoice Saved Successfully");
                }
                txtInvoiceNumber.Text = string.Empty;
                txtInvoiceDescription.Text = string.Empty;
                txtInvoiceType.Text = string.Empty;
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private bool IsDisable(GridView view, int row)
        {
            bool result = false;
            try
            {

                string val = Convert.ToString(view.GetRowCellValue(row, "IsInvoiced"));
                if (val == "Ja")
                    result = true;
                else if (val == "Nein")
                    result = false;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        private void gvDeliveryNotes_ShowingEditor(object sender, CancelEventArgs e)
        {
            try
            {
                if (gvDeliveryNotes.FocusedColumn.FieldName == "SELCETED"
                                && IsDisable(gvDeliveryNotes, gvDeliveryNotes.FocusedRowHandle))
                    e.Cancel = true;
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }
        #endregion

        #region Supplier Proposal

        #region Events

        private void cmbLVSectionProposal_Closed(object sender, ClosedEventArgs e)
        {
            try
            {
                if (Utility.LVSectionEditAccess == "7")
                {
                    if (Convert.ToString(cmbLVSectionProposal.GetDisplayValueByKeyValue(cmbLVSectionProposalItem.EditValue)).ToLower() != "ha")
                        btnSaveSupplierProposal.Enabled = false;
                    else if (Utility.CalcAccess != "7")
                        btnSaveSupplierProposal.Enabled = true;
                }

                if (ObjESupplier == null)
                    ObjESupplier = new ESupplier();
                if (ObjBSupplier == null)
                    ObjBSupplier = new BSupplier();
                ObjESupplier.ProjectID = ObjEProject.ProjectID;
                ObjESupplier.LVSection = Convert.ToString(cmbLVSectionProposal.GetDisplayValueByKeyValue(cmbLVSectionProposalItem.EditValue));
                ObjESupplier.LVSectionID = cmbLVSectionProposalItem.EditValue;

                if (ObjESupplier.LVSectionID != null)
                {
                    //retriving Proposal Numbers
                    ObjESupplier = ObjBSupplier.GetProposalNumber(ObjESupplier);

                    DataColumn keyColumn = ObjESupplier.dsProposalNumbers.Tables[0].Columns["SupplierProposalID"];
                    DataColumn foreignKeyColumn = ObjESupplier.dsProposalNumbers.Tables[1].Columns["SupplierProposalID"];
                    ObjESupplier.dsProposalNumbers.Relations.Add("ProposalArticles", keyColumn, foreignKeyColumn);

                    gcProposedSupplier.DataSource = ObjESupplier.dsProposalNumbers.Tables[0];

                    gcProposedSupplier.LevelTree.Nodes.Add("ProposalArticles", gvProposalArticles);
                    gvProposalArticles.ViewCaption = "Proposal Articles";

                    ExpandAllRows(gvProposedSupplier);

                    gcLVDetailsforSupplier.DataSource = null;
                    gcProposedDetails.DataSource = null;
                    gcDeletedDetails.DataSource = null;
                    chkSupplierLists.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gvProposedSupplier_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try
            {
                if (gvProposedSupplier.FocusedRowHandle >= 0)
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    SplashScreenManager.Default.SetWaitFormDescription("Loading...");
                    int Ivalue = 0;
                    if (int.TryParse(Convert.ToString(gvProposedSupplier.GetFocusedRowCellValue("SupplierProposalID")), out Ivalue))
                    {
                        ObjESupplier.ProjectID = ObjEProject.ProjectID;
                        ObjESupplier.LVSection = Convert.ToString(cmbLVSectionProposal.GetDisplayValueByKeyValue(cmbLVSectionProposalItem.EditValue));
                        ObjESupplier.LVSectionID = cmbLVSectionProposalItem.EditValue;
                        ObjESupplier.ProposalID = Ivalue;
                        ObjESupplier.dtArticleID = new DataTable();
                        ObjESupplier.dtArticleID.Columns.Add("ID", typeof(int));
                        ColumnView childView = (ColumnView)gvProposedSupplier.GetDetailView(gvProposedSupplier.FocusedRowHandle, 0);
                        if (childView == null || childView.DataRowCount == 0)
                            return;
                        DataRow dr = null;
                        DataRow drNew = null;
                        for (int i = 0; i < childView.DataRowCount; i++)
                        {
                            dr = childView.GetDataRow(i);
                            drNew = ObjESupplier.dtArticleID.NewRow();
                            drNew["ID"] = dr["WGID"];
                            ObjESupplier.dtArticleID.Rows.Add(drNew);
                        }
                        if (Ivalue > 0)
                        {
                            ObjBSupplier.GetPositionsByProposalID(ObjESupplier);
                        }
                        else
                        {
                            ObjBSupplier.GetPositionsforsupplierProposal(ObjESupplier);
                            ObjESupplier.dtDeletedPositions = new DataTable();
                            ObjESupplier.dtDeletedPositions.Columns.Add("PositionID", typeof(int));
                            ObjESupplier.dtDeletedPositions.Columns.Add("Position_OZ", typeof(string));
                            ObjESupplier.dtDeletedPositions.Columns.Add("DetailKZ", typeof(int));
                            ObjESupplier.dtDeletedPositions.Columns.Add("Longdiscription", typeof(string));
                            ObjESupplier.dtDeletedPositions.Columns.Add("Menge", typeof(decimal));
                            ObjESupplier.dtDeletedPositions.Columns.Add("PositionKZ", typeof(string));
                            ObjESupplier.dtProposedPositions = null;
                        }
                        gcLVDetailsforSupplier.DataSource = ObjESupplier.dtNewPositions;
                        gcDeletedDetails.DataSource = ObjESupplier.dtDeletedPositions;
                        gcProposedDetails.DataSource = ObjESupplier.dtProposedPositions;
                        chkSupplierLists.DataSource = ObjESupplier.dtSupplier_SP;
                        chkSupplierLists.DisplayMember = "ShortName";
                        chkSupplierLists.ValueMember = "SupplierID";
                        if (Ivalue > 0)
                        {
                            DataRowView row = null;
                            for (int i = 0; i < ObjESupplier.dtSupplier_SP.Rows.Count; i++)
                            {
                                row = chkSupplierLists.GetItem(i) as DataRowView;
                                if (Convert.ToInt16(row["ItemChecked"]) == 1)
                                    chkSupplierLists.SetItemChecked(i, true);
                            }
                        }
                        gvLVDetailsforSupplier.BestFitColumns();
                        gvDeletedDetails.BestFitColumns();
                        gvProposedDetails.BestFitColumns();
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
            finally { SplashScreenManager.CloseForm(false); }
        }

        private void gvLVDetailsforSupplier_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            try
            {
                if (e.HitInfo.InRow)
                    e.Menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("Löschen", gcLVDetailsDeletefoSupplier_ItemClick));
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gcLVDetailsDeletefoSupplier_ItemClick(object sender, EventArgs e)
        {

            try
            {
                int iIndex = gvLVDetailsforSupplier.FocusedRowHandle;
                DataRow dr = gvLVDetailsforSupplier.GetDataRow(gvLVDetailsforSupplier.FocusedRowHandle);
                DataTable _dt = gcDeletedDetails.DataSource as DataTable;
                _dt.ImportRow(dr);
                Utility.Setfocus(gvDeletedDetails, "PositionID", Convert.ToInt32(gvLVDetailsforSupplier.GetFocusedRowCellValue("PositionID").ToString()));
                ObjESupplier.dtNewPositions.Rows.RemoveAt(iIndex);
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gcLVDetailsforSupplier_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (Utility.CalcAccess == "7" || ObjEProject.IsFinalInvoice)
                    return;
                GridControl grid = sender as GridControl;
                DataTable table = grid.DataSource as DataTable;
                DataRow row = e.Data.GetData(typeof(DataRow)) as DataRow;
                if (row != null && table != null && row.Table != table)
                {
                    object strPositionID = row["PositionID"].ToString();
                    DataRow[] foundRows = table.Select("PositionID = '" + strPositionID + "'");
                    if (foundRows.Count() <= 0)
                    {
                        table.ImportRow(row);
                        string _type = Convert.ToString(row["PositionsStatus"]);
                        if (_type == "D")
                        {
                            DataRow[] result = ObjESupplier.dtDeletedPositions.Select("PositionID = '" + strPositionID + "'");
                            foreach (DataRow dr in result)
                            {
                                ObjESupplier.dtDeletedPositions.Rows.Remove(row);
                            }
                        }
                        Utility.Setfocus(gvLVDetailsforSupplier, "PositionID", Convert.ToInt32(strPositionID));
                    }
                    else
                        throw new Exception("Position Already Exists");
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnSaveSupplierProposal_Click(object sender, EventArgs e)
        {
            try
            {
                if (chkSupplierLists.CheckedItems.Count == 0)
                {
                    if (!Utility._IsGermany)
                        throw new Exception("Please select atleast one Supplier");
                    else
                        throw new Exception("Bitte wählen Sie mindestens einen Lieferanten aus");
                }

                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                SplashScreenManager.Default.SetWaitFormDescription("Loading...");

                ObjESupplier.dtPositionsToDB = ObjESupplier.dtNewPositions.Copy();
                ObjESupplier.dtDeletedPositionsToDB = ObjESupplier.dtDeletedPositions.Copy();

                foreach (DataColumn dc in ObjESupplier.dtNewPositions.Columns)
                {
                    if (dc.ColumnName != "PositionID")
                    {
                        ObjESupplier.dtPositionsToDB.Columns.Remove(dc.ColumnName);
                        ObjESupplier.dtDeletedPositionsToDB.Columns.Remove(dc.ColumnName);
                    }
                }

                ObjESupplier.dtSupplierToDB = new DataTable();
                ObjESupplier.dtSupplierToDB.Columns.Add("SupplierID");
                DataRowView row = null;
                DataRow dr = null;
                foreach (object item in chkSupplierLists.CheckedItems)
                {
                    row = item as DataRowView;
                    dr = ObjESupplier.dtSupplierToDB.NewRow();
                    dr["SupplierID"] = row["SupplierID"];
                    ObjESupplier.dtSupplierToDB.Rows.Add(dr);
                }

                ObjESupplier.dtArticleID = new DataTable();
                ObjESupplier.dtArticleID.Columns.Add("ID", typeof(int));
                ColumnView childView = (ColumnView)gvProposedSupplier.GetDetailView(gvProposedSupplier.FocusedRowHandle, 0);
                if (childView == null || childView.DataRowCount == 0)
                    return;
                DataRow drNew = null;
                for (int i = 0; i < childView.DataRowCount; i++)
                {
                    drNew = ObjESupplier.dtArticleID.NewRow();
                    dr = childView.GetDataRow(i);
                    drNew["ID"] = dr["WGID"];
                    ObjESupplier.dtArticleID.Rows.Add(drNew);
                }
                ObjESupplier.ProjectID = ObjEProject.ProjectID;
                ObjESupplier.LVSection = Convert.ToString(cmbLVSectionProposal.GetDisplayValueByKeyValue(cmbLVSectionProposalItem.EditValue));
                ObjESupplier.LVSectionID = cmbLVSectionProposalItem.EditValue;
                ObjBSupplier.SaveSupplierProposal(ObjESupplier);
                cmbLVSectionProposal_Closed(null, null);
                Utility.Setfocus(gvProposedSupplier, "SupplierProposalID", ObjESupplier.ProposalID);
                gvProposedSupplier_RowClick(null, null);
                if (Utility._IsGermany == true)
                    frmCalcPro.UpdateStatus("Vorgang abgeschlossen: Generierung des Angebots");
                else
                    frmCalcPro.UpdateStatus("Proposal generated successfully");
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
            finally { SplashScreenManager.CloseForm(false); }
        }

        private void gvProposedDetails_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            try
            {
                if (e.HitInfo.InRow)
                    e.Menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("Löschen", gvDeleteProposalPossitions_ItemClick));
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gvDeleteProposalPossitions_ItemClick(object sender, EventArgs e)
        {
            try
            {
                if (gvProposedDetails.FocusedRowHandle >= 0)
                {
                    int PID = 0;
                    int SID = 0;
                    if (int.TryParse(Convert.ToString(gvProposedDetails.GetFocusedRowCellValue("PositionID")), out PID))
                    {
                        if (int.TryParse(Convert.ToString(gvProposedSupplier.GetFocusedRowCellValue("SupplierProposalID")), out SID))
                        {
                            DSupplier ObjDSupplier = new DSupplier();
                            ObjDSupplier.DeleteProposalPositions(SID, PID);
                            gvProposedDetails.DeleteRow(gvProposedDetails.FocusedRowHandle);
                            gvSupplier.Columns.Clear();
                            gcSupplier.Controls.Clear();
                            gcSupplier.DataSource = null;
                            gcSuppliers.DataSource = null;
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }

        private void gvProposedSupplier_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            try
            {
                int _PrID;
                if (e.HitInfo.InRow)
                {
                    if (gvProposedSupplier.FocusedRowHandle >= 0 && gvProposedSupplier.GetFocusedRowCellValue("ProposalID") != null)
                    {
                        if (int.TryParse(Convert.ToString(gvProposedSupplier.GetFocusedRowCellValue("ProposalID")), out _PrID) && _PrID > 0)
                        {
                            _ProposalID = _PrID;
                            e.Menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("Spezifikationsdokument für Lieferantenanfrage generieren", btnGeneratePDF_Click));
                            e.Menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("Email für Lieferantenanfrage generieren", btnSendEmail_Click));
                            e.Menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("Export", btnSupplierProposalExport_Click));
                            e.Menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("Import", btnSupplierProposalImport_Click));
                            e.Menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("Add/Merge Article", btnMergeArticle_Click));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnSupplierProposalExport_Click(object sender, EventArgs e)
        {
            try
            {
                XmlDocument XMLDoc = null;
                if (objBGAEB == null)
                    objBGAEB = new BGAEB();
                if (objEGAEB == null)
                    objEGAEB = new EGAEB();
                objEGAEB.ProjectID = ObjEProject.ProjectID;
                objEGAEB.ProjectNumber = ObjEProject.ProjectNumber;
                objEGAEB.LvRaster = ObjEProject.LVRaster;
                frmGAEBFormat Obj = new frmGAEBFormat(objEGAEB);
                Obj.ShowDialog();
                if (!string.IsNullOrEmpty(objEGAEB.FileFormat) && !string.IsNullOrEmpty(objEGAEB.FileNAme) && objEGAEB.IsSave)
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    SplashScreenManager.Default.SetWaitFormDescription("Exportieren...");
                    objEGAEB.IsSave = false;
                    int IValue = 0;
                    if (gvProposedSupplier.FocusedRowHandle >= 0 &&
                        int.TryParse(Convert.ToString(gvProposedSupplier.GetFocusedRowCellValue("SupplierProposalID")), out IValue))
                    {
                        string strOTTOFilePath = objEGAEB.DirPath = Application.UserAppDataPath + "\\";
                        XMLDoc = objBGAEB.ExportSupplierproposal(IValue, ObjEProject.ProjectID, objEGAEB.FileFormat, ObjEProject.LVRaster, objEGAEB);
                        if (!Directory.Exists(strOTTOFilePath))
                            Directory.CreateDirectory(strOTTOFilePath);
                        string strOutputFilePath = string.Empty;
                        strOutputFilePath = objEGAEB.OutputPath + "\\" + objEGAEB.FileNAme + "." + objEGAEB.FileFormat;
                        string strInputFilePath = strOTTOFilePath + objEGAEB.FileNAme + ".tml";
                        XMLDoc.Save(strInputFilePath);
                        Utility.ProcesssFile(strInputFilePath, strOutputFilePath);
                        SplashScreenManager.CloseForm(false);
                        if (File.Exists(strOutputFilePath))
                            Process.Start("explorer.exe", "/select, \"" + strOutputFilePath + "\"");
                    }
                    else
                        SplashScreenManager.CloseForm(false);
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Utility.ShowError(ex);
            }
        }

        private void btnSupplierProposalImport_Click(object sender, EventArgs e)
        {
            try
            {
                string strFilePath = string.Empty;
                XtraOpenFileDialog dlg = new XtraOpenFileDialog();
                dlg.Title = "Dateiauswahl für GAEB Import";
                dlg.CheckFileExists = true;
                dlg.CheckPathExists = true;

                dlg.Filter = "GAEB Files(*.D84;*.P84;*.X84) | *.D84;*.P84;*.X84";
                dlg.RestoreDirectory = true;

                if (dlg.ShowDialog() == DialogResult.OK)
                    strFilePath = dlg.FileName;
                if (!string.IsNullOrEmpty(strFilePath))
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    SplashScreenManager.Default.SetWaitFormDescription("Importieren...");
                    string strOutputFilepath = string.Empty;
                    string strOTTOFilePath = Application.UserAppDataPath + "\\";
                    if (!Directory.Exists(strOTTOFilePath))
                        Directory.CreateDirectory(strOTTOFilePath);
                    string strFileName = Path.GetFileNameWithoutExtension(strFilePath);
                    strOutputFilepath = strOTTOFilePath + strFileName + ".tml";
                    Utility.ProcesssFile(strFilePath, strOutputFilepath);
                    SplashScreenManager.CloseForm(false);
                    int IValue = 0;
                    if (gvProposedSupplier.FocusedRowHandle >= 0 &&
                        int.TryParse(Convert.ToString(gvProposedSupplier.GetFocusedRowCellValue("SupplierProposalID")), out IValue))
                    {
                        if (objBGAEB == null)
                            objBGAEB = new BGAEB();
                        if (objEGAEB == null)
                            objEGAEB = new EGAEB();
                        objEGAEB.Supplier = Convert.ToString(gvProposedSupplier.GetFocusedRowCellValue("Supplier"));
                        frmSelectsupplier Obj = new frmSelectsupplier(objEGAEB);
                        Obj.ShowDialog();
                        if (objEGAEB.IsSave)
                        {
                            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                            SplashScreenManager.Default.SetWaitFormDescription("Importieren...");
                            objEGAEB.IsSave = false;
                            objEGAEB.UserID = Utility.UserID;
                            string strRaster = Utility.GetRaster(strOutputFilepath);
                            objEGAEB.dsLVData = Utility.CreateDatasetSchema(strOutputFilepath, string.Empty, strRaster, objEGAEB);
                            objEGAEB.LvRaster = strRaster;
                            objEGAEB.ProjectID = ObjEProject.ProjectID;
                            objEGAEB.SupplierProposalID = IValue;
                            DataTable dtTemp = objEGAEB.dsLVData.Tables[0].Copy();
                            foreach (DataColumn dc in dtTemp.Columns)
                            {
                                if (dc.ColumnName != "OZ")
                                {
                                    if (dc.ColumnName != "EP")
                                    {
                                        objEGAEB.dsLVData.Tables[0].Columns.Remove(dc.ColumnName);
                                    }
                                }
                            }
                            objEGAEB = objBGAEB.SupplierProposalImport(objEGAEB);
                            frmCalcPro.UpdateStatus("Import von Angebotsdaten mit Erfolg abgeschlossen");
                            SplashScreenManager.CloseForm(false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Utility.ShowError(ex);
            }
        }

        private void btnMergeArticle_Click(object sender, EventArgs e)
        {
            try
            {
                int Ivalue = 0;
                if (gvProposedSupplier.FocusedRowHandle >= 0 && int.TryParse(Convert.ToString(gvProposedSupplier.GetFocusedRowCellValue("SupplierProposalID")), out Ivalue))
                {
                    if (ObjESupplier == null)
                        ObjESupplier = new ESupplier();
                    if (ObjBSupplier == null)
                        ObjBSupplier = new BSupplier();
                    ObjESupplier.dtArticleID = new DataTable();
                    ObjESupplier.dtArticleID.Columns.Add("ID", typeof(int));
                    ColumnView childView = (ColumnView)gvProposedSupplier.GetDetailView(gvProposedSupplier.FocusedRowHandle, 0);
                    if (childView == null || childView.DataRowCount == 0)
                        return;
                    DataRow drNew = null;
                    DataRow dr = null;
                    for (int i = 0; i < childView.DataRowCount; i++)
                    {
                        drNew = ObjESupplier.dtArticleID.NewRow();
                        dr = childView.GetDataRow(i);
                        drNew["ID"] = dr["WGID"];
                        ObjESupplier.dtArticleID.Rows.Add(drNew);
                    }
                    ObjESupplier.ProjectID = ObjEProject.ProjectID;
                    ObjESupplier.LVSectionID = cmbLVSectionProposalItem.EditValue;
                    ObjESupplier.LVSection = Convert.ToString(cmbLVSectionProposal.GetDisplayValueByKeyValue(cmbLVSectionProposalItem.EditValue));
                    ObjESupplier.SupplierProposalID = Ivalue;

                    ObjBSupplier.GetArticlesForProposal(ObjESupplier);
                    frmMergeArticle Obj = new frmMergeArticle(ObjESupplier);
                    Obj.ShowDialog();
                    if (ObjESupplier.IsActive)
                    {
                        cmbLVSectionProposal_Closed(null, null);
                        Utility.Setfocus(gvProposedSupplier, "SupplierProposalID", ObjESupplier.SupplierProposalID);
                        gvProposedSupplier_RowClick(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnGeneratePDF_Click(object sender, EventArgs e)
        {
            try
            {
                rptSupplierProposal rpt = new rptSupplierProposal();
                ReportPrintTool printTool = new ReportPrintTool(rpt);
                rpt.Parameters["ProposalID"].Value = _ProposalID;
                rpt.Parameters["ProjectID"].Value = ObjEProject.ProjectID;
                rpt.Parameters["ProjectNumber"].Value = ObjEProject.ProjectNumber;
                rpt.Parameters["ProjectDescription"].Value = ObjEProject.ProjectDescription;
                rpt.Parameters["ReportDate"].Value = DateTime.Now;
                saveFileDialog1.FileName = ObjEProject.ProjectNumber;
                saveFileDialog1.Filter = "PDF Files|*.pdf";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    rpt.ExportToPdf(saveFileDialog1.FileName);
                    rpt.ShowRibbonPreview();
                    _pdfpath = saveFileDialog1.FileName;
                }
            }
            catch (Exception ex) { }
        }

        private void btnSendEmail_Click(object sender, EventArgs e)
        {
            StringBuilder strArr = new StringBuilder();
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                SplashScreenManager.Default.SetWaitFormDescription("Checking outlook configuration...");
                Type officeType = Type.GetTypeFromProgID("Outlook.Application");
                if (officeType == null)
                {
                    throw new Exception("Outlook wird konfiguriert / installiert");
                }
                else
                {
                    int IValue = 0;
                    if (int.TryParse(Convert.ToString(gvProposedSupplier.GetFocusedRowCellValue("SupplierProposalID")), out IValue))
                    {
                        SplashScreenManager.Default.SetWaitFormDescription("Generating supplier Mails...");
                        string strLVSection = string.Empty;
                        if (gvProposedSupplier.GetFocusedRowCellValue("LVSection") != null)
                            strLVSection = Convert.ToString(gvProposedSupplier.GetFocusedRowCellValue("LVSection"));

                        string strFileName = "Proposal_" + ObjEProject.ProjectNumber + "_" + strLVSection + "_" + Convert.ToString(IValue) + ".pdf";
                        string appPath = Application.UserAppDataPath + "\\SupplierProposal_pdf";
                        string appPathGAEB = Application.UserAppDataPath + "\\SupplierProposal_GAEB";
                        string _GAEBPath = string.Empty;
                        if (!Directory.Exists(appPath))
                            Directory.CreateDirectory(appPath);
                        if (!Directory.Exists(appPathGAEB))
                            Directory.CreateDirectory(appPathGAEB);

                        //GAEB Generation
                        XmlDocument XMLDoc = null;
                        if (objBGAEB == null)
                            objBGAEB = new BGAEB();
                        if (objEGAEB == null)
                            objEGAEB = new EGAEB();
                        objEGAEB.ProjectID = ObjEProject.ProjectID;
                        objEGAEB.ProjectNumber = ObjEProject.ProjectNumber;
                        objEGAEB.LvRaster = ObjEProject.LVRaster;
                        objEGAEB.IsMail = true;
                        objEGAEB.FileNAme = "Proposal_" + ObjEProject.ProjectNumber + "_" + strLVSection + "_" + Convert.ToString(IValue);
                        objEGAEB.OutputPath = appPathGAEB;
                        SplashScreenManager.CloseForm(false);
                        frmGAEBFormat Obj = new frmGAEBFormat(objEGAEB);
                        Obj.ShowDialog();
                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                        SplashScreenManager.Default.SetWaitFormDescription("Exportieren...");
                        if (!string.IsNullOrEmpty(objEGAEB.FileFormat) && !string.IsNullOrEmpty(objEGAEB.FileNAme) && objEGAEB.IsSave)
                        {
                            objEGAEB.IsMail = false;
                            objEGAEB.IsSave = false;
                            string strOTTOFilePath = objEGAEB.DirPath = Application.UserAppDataPath + "\\";
                            XMLDoc = objBGAEB.ExportSupplierproposal(IValue, ObjEProject.ProjectID, objEGAEB.FileFormat, ObjEProject.LVRaster, objEGAEB);
                            if (!Directory.Exists(strOTTOFilePath))
                                Directory.CreateDirectory(strOTTOFilePath);
                            string strOutputFilePath = string.Empty;
                            strOutputFilePath = _GAEBPath = objEGAEB.OutputPath + "\\" + objEGAEB.FileNAme + "." + objEGAEB.FileFormat;
                            string strInputFilePath = strOTTOFilePath + objEGAEB.FileNAme + ".tml";
                            XMLDoc.Save(strInputFilePath);
                            Utility.ProcesssFile(strInputFilePath, strOutputFilePath);
                            SplashScreenManager.Default.SetWaitFormDescription("Generating Pdf...");
                            //PDF Save
                            rptSupplierProposal rpt = new rptSupplierProposal();
                            ReportPrintTool printTool = new ReportPrintTool(rpt);
                            rpt.Parameters["ProposalID"].Value = _ProposalID;
                            rpt.Parameters["ProjectID"].Value = ObjEProject.ProjectID;
                            rpt.Parameters["ProjectNumber"].Value = ObjEProject.ProjectNumber;
                            rpt.Parameters["ProjectDescription"].Value = ObjEProject.ProjectDescription;
                            rpt.Parameters["ReportDate"].Value = DateTime.Now;
                            _pdfpath = appPath + "\\" + strFileName;
                            rpt.CreateDocument();
                            rpt.ExportToPdf(_pdfpath);
                            //Sending Mail
                            ObjBSupplier.GetSupplierMail(ObjESupplier, _ProposalID, ObjEProject.ProjectID);
                            if (ObjESupplier.dtSupplierMail.Rows.Count > 0)
                            {
                                List<Microsoft.Office.Interop.Outlook.MailItem> MailList = new List<Microsoft.Office.Interop.Outlook.MailItem>();
                                foreach (DataRow dr in ObjESupplier.dtSupplierMail.Rows)
                                {
                                    Microsoft.Office.Interop.Outlook.Application app = new Microsoft.Office.Interop.Outlook.Application();
                                    Microsoft.Office.Interop.Outlook.MailItem mailItem = app.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);

                                    mailItem.Subject = "Preisanfrage";
                                    mailItem.BCC = dr["Suppliermail"].ToString();//strArr.ToString();
                                    mailItem.Body = "Bitte stellen Sie uns für beigefügte Anfrage Ihr Preisangebot zur Verfügung";

                                    SplashScreenManager.Default.SetWaitFormDescription("Generating Proposal Letter for " + Convert.ToString(dr["ProposalName"]) + "...");

                                    string stLetterName = GenerateProposalLetter(Convert.ToString(dr["ProposalName"]),
                                         Convert.ToString(dr["StreetNo"]), objEGAEB.DeliveryDeadline, IValue);

                                    mailItem.Attachments.Add(stLetterName);
                                    mailItem.Attachments.Add(_pdfpath);
                                    mailItem.Attachments.Add(_GAEBPath);
                                    mailItem.Importance = Microsoft.Office.Interop.Outlook.OlImportance.olImportanceHigh;
                                    MailList.Add(mailItem);
                                }

                                foreach (Microsoft.Office.Interop.Outlook.MailItem mailItem in MailList)
                                    mailItem.Display();
                                UpdateProposalDate();
                            }
                        }
                        else
                        {
                            objEGAEB.IsMail = false;
                            objEGAEB.IsSave = false;
                        }
                    }
                }
                SplashScreenManager.CloseForm(false);
            }
            catch (System.Exception ex)
            {
                if (ex.Message.Contains("Sequence contains no elements"))
                    XtraMessageBox.Show("Die erforderliche Dokumentenvorlage ist nicht eingestellt!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    Utility.ShowError(ex);
                SplashScreenManager.CloseForm(false);
            }
        }

        private string GenerateProposalLetter(string SupplierName, string SupplierAddress, DateTime dtDeliveryDeadLine, int ProposalID)
        {
            string stLetterName = string.Empty;
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document wordDoc = new Microsoft.Office.Interop.Word.Document();
            try
            {
                if (ObjEProject == null)
                    ObjEProject = new EProject();
                if (ObjBProject == null)
                    ObjBProject = new BProject();
                ObjEProject = ObjBProject.GetPath(ObjEProject);
                if (string.IsNullOrEmpty(ObjEProject.CoverSheetPath))
                    throw new Exception("Der angegeben Ordnerpfad zum Titelblatt existiert nicht (mehr)");
                if (!Directory.Exists(ObjEProject.CoverSheetPath))
                    throw new Exception("Kein gültiger Ordnerpfad zum Titelblatt");
                stLetterName = ObjEProject.CoverSheetPath + "\\" + "V-015-" + ObjEProject.ProjectNumber + "-" +
                    SupplierName + "-" + Convert.ToString(ProposalID) + ".Docx";
                if (!File.Exists(stLetterName))
                {
                    var FolderPath = new DirectoryInfo(ObjEProject.TemplatePath).GetFiles("V-015*.dotx",
                        SearchOption.AllDirectories).OrderByDescending(d => d.LastWriteTimeUtc).First();
                    Object oMissing = System.Reflection.Missing.Value;
                    Object AppdataPath = Application.UserAppDataPath + "\\" + FolderPath;
                    Object oTemplatePath = ObjEProject.TemplatePath + "\\" + FolderPath;
                    System.IO.File.Copy(Convert.ToString(oTemplatePath), Convert.ToString(AppdataPath), true);
                    wordDoc = wordApp.Documents.Add(ref AppdataPath, ref oMissing, ref oMissing, ref oMissing);
                    UdpateMergefields(wordDoc, wordApp, ObjEProject, SupplierName, SupplierAddress, dtDeliveryDeadLine);
                    wordDoc.SaveAs(stLetterName);
                    wordDoc.Close();
                    Thread.Sleep(1000);
                }
                wordApp.Quit();
            }
            catch (Exception ex)
            {
                if (wordDoc != null)
                    wordDoc.Close();
                if (wordApp != null)
                    wordApp.Quit();
                throw ex;
            }
            return stLetterName;
        }

        private void chkSupplierLists_GetItemEnabled(object sender, GetItemEnabledEventArgs e)
        {
            try
            {
                CheckedListBoxControl control = sender as CheckedListBoxControl;
                DataRowView row = control.GetItem(e.Index) as DataRowView;
                if (Convert.ToInt16(row["ItemChecked"]) == 1)
                    e.Enabled = false;
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void chkSupplierLists_ItemChecking(object sender, ItemCheckingEventArgs e)
        {
            try
            {
                if (Convert.ToBoolean(e.NewValue) == true && chkSupplierLists.CheckedItems.Count >= 8)
                    e.Cancel = true;
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }
        #endregion

        #region Functions

        /// <summary>
        /// Code to fetch LV section related to active project and bind to combobox for selection
        /// </summary>
        private void FillLVSection()
        {
            try
            {
                ObjESupplier = ObjBSupplier.GetLVSectionForProposal(ObjESupplier);
                if (ObjESupplier.dtLVSection != null)
                {
                    if (Utility.LVSectionEditAccess == "9")
                    {
                        DataView dv = ObjESupplier.dtLVSection.DefaultView;
                        dv.RowFilter = "LVSectionName= 'HA'";
                        cmbLVSectionProposal.DataSource = dv;
                        cmbLVSectionProposal.DisplayMember = "LVSectionName";
                        cmbLVSectionProposal.ValueMember = "LVSectionID";
                    }
                    else
                    {
                        cmbLVSectionProposal.DataSource = ObjESupplier.dtLVSection;
                        cmbLVSectionProposal.DisplayMember = "LVSectionName";
                        cmbLVSectionProposal.ValueMember = "LVSectionID";
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        /// <summary>
        /// Code to update proposal date to a proposal
        /// </summary>
        private void UpdateProposalDate()
        {
            try
            {
                int IValue = 0;
                if (gvProposedSupplier.FocusedRowHandle >= 0 &&
                    int.TryParse(Convert.ToString(gvProposedSupplier.GetFocusedRowCellValue("SupplierProposalID")), out IValue))
                {
                    if (ObjESupplier == null)
                        ObjESupplier = new ESupplier();
                    if (ObjBSupplier == null)
                        ObjBSupplier = new BSupplier();
                    ObjESupplier.SupplierProposalID = IValue;
                    ObjESupplier = ObjBSupplier.UpdateProposalDate(ObjESupplier);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #endregion

        #region Update Supplier

        #region Events

        private void gvProposal_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try
            {
                if (gvProposal.FocusedRowHandle >= 0 &&
                    gvProposal.GetFocusedRowCellValue("SupplierProposalID") != null)
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    SplashScreenManager.Default.SetWaitFormDescription("Loading...");
                    string strLVSection = Convert.ToString(gvProposal.GetFocusedRowCellValue("LVSection"));
                    if (strLVSection.ToLower() != "ha")
                    {
                        if (Utility.LVSectionEditAccess == "7")
                        {
                            //layoutControl16.Enabled = false;
                            btnSubmit.Enabled = false;
                            gvSupplier.OptionsBehavior.Editable = false;
                        }
                    }
                    else
                    {
                        if (!ObjEProject.IsFinalInvoice)
                        {
                            if (Utility.CalcAccess != "7")
                            {
                                //layoutControl16.Enabled = true;
                                btnSubmit.Enabled = true;
                                gvSupplier.OptionsBehavior.Editable = true;
                            }
                        }
                    }

                    int iValue = 0;
                    if (int.TryParse(gvProposal.GetFocusedRowCellValue("SupplierProposalID").ToString(), out iValue))
                    {
                        List<Control> l = new List<Control>();
                        foreach (Control c in gcSupplier.Controls)
                        {
                            if (c.GetType() == typeof(CheckEdit))
                                l.Add(c);
                        }
                        foreach (Control c in l)
                            gcSupplier.Controls.Remove(c);
                        gvSupplier.Columns.Clear();
                        ObjESupplier.SupplierProposalID = iValue;
                        if (Convert.ToInt16(rgProposalViews.EditValue) == 0)
                        {
                            ObjESupplier = ObjBSupplier.GetProposalPostions(ObjESupplier);
                            gcSupplier.DataSource = ObjESupplier.dtPositions;
                        }
                        else
                            rgProposalViews.EditValue = 0;

                        gvSupplier.FormatRules.Clear();

                        GridFormatRule GridFormatRule1 = new GridFormatRule();
                        GridFormatRule1.ApplyToRow = true;
                        GridFormatRule1.Column = gvSupplier.Columns["PositionKZ"];

                        FormatConditionRuleValue formatConditionRuleValue1 = new FormatConditionRuleValue();
                        formatConditionRuleValue1.Appearance.BackColor = Color.White;
                        formatConditionRuleValue1.Appearance.Options.UseBackColor = true;
                        formatConditionRuleValue1.Condition = FormatCondition.Equal;
                        formatConditionRuleValue1.Value1 = "E";
                        GridFormatRule1.Rule = formatConditionRuleValue1;
                        gvSupplier.FormatRules.Add(GridFormatRule1);

                        GridFormatRule GridFormatRule3 = new GridFormatRule();
                        GridFormatRule3.ApplyToRow = true;
                        GridFormatRule3.Column = gvSupplier.Columns["DetailKZ"];

                        FormatConditionRuleValue formatConditionRuleValue2 = new FormatConditionRuleValue();
                        formatConditionRuleValue2.Appearance.BackColor = Color.FromArgb(132, 107, 75);
                        formatConditionRuleValue2.Appearance.ForeColor = Color.White;
                        formatConditionRuleValue2.Appearance.Options.UseBackColor = true;
                        formatConditionRuleValue2.Condition = FormatCondition.Greater;
                        formatConditionRuleValue2.Value1 = 0;
                        GridFormatRule3.Rule = formatConditionRuleValue2;
                        gvSupplier.FormatRules.Add(GridFormatRule3);

                        if (ObjESupplier.dtPositions != null)
                        {
                            int Columncount = ObjESupplier.dtPositions.Columns.Count;
                            gvSupplier.Columns.ColumnByFieldName("PositionID").Visible = false;
                            gvSupplier.Columns.ColumnByFieldName("PositionID1").Visible = false;
                            gvSupplier.Columns.ColumnByFieldName("ShortDescription").Visible = false;
                            gvSupplier.Columns.ColumnByFieldName("Menge").Visible = false;
                            gvSupplier.Columns.ColumnByFieldName("A").Visible = false;
                            gvSupplier.Columns.ColumnByFieldName("B").Visible = false;
                            gvSupplier.Columns.ColumnByFieldName("L").Visible = false;
                            gvSupplier.Columns.ColumnByFieldName("ME").Visible = false;
                            gvSupplier.Columns.ColumnByFieldName("MA_Multi1").Visible = false;
                            gvSupplier.Columns.ColumnByFieldName("MA_multi2").Visible = false;
                            gvSupplier.Columns.ColumnByFieldName("MA_multi3").Visible = false;
                            gvSupplier.Columns.ColumnByFieldName("MA_multi4").Visible = false;
                            gvSupplier.Columns.ColumnByFieldName("LiefrantMA").Visible = false;
                            gvSupplier.Columns.ColumnByFieldName("MA_listprice").Visible = false;
                            gvSupplier.Columns.ColumnByFieldName("Fabricate").Visible = false;

                            gvSupplier.Columns.ColumnByFieldName("sno").Visible = false;
                            gvSupplier.Columns.ColumnByFieldName("PositionID2").Visible = false;
                            gvSupplier.Columns.ColumnByFieldName("PositionID3").Visible = false;
                            gvSupplier.Columns.ColumnByFieldName("Parent_OZ").Visible = false;
                            gvSupplier.Columns.ColumnByFieldName("ID").Visible = false;
                            gvSupplier.Columns.ColumnByFieldName("SID").Visible = false;
                            gvSupplier.Columns.ColumnByFieldName("MA_einkaufspreis").Visible = false;

                            gvSupplier.Columns.ColumnByFieldName("Cheapest").VisibleIndex = Columncount - 1;
                            gvSupplier.Columns.ColumnByFieldName("Position_OZ").VisibleIndex = 0;
                            gvSupplier.Columns.ColumnByFieldName("DetailKZ").VisibleIndex = 1;
                            gvSupplier.Columns.ColumnByFieldName("PositionKZ").VisibleIndex = 2;
                            gvSupplier.Columns.ColumnByFieldName("Cheapest").OptionsColumn.AllowEdit = false;
                            gvSupplier.Columns.ColumnByFieldName("Position_OZ").OptionsColumn.AllowEdit = false;
                            gvSupplier.Columns.ColumnByFieldName("PositionKZ").OptionsColumn.AllowEdit = false;
                            gvSupplier.Columns.ColumnByFieldName("DetailKZ").OptionsColumn.AllowEdit = false;

                            foreach (DevExpress.XtraGrid.Columns.GridColumn col in ((ColumnView)gcSupplier.Views[0]).Columns)
                            {
                                if (col.FieldName.Contains("Multi") || col.FieldName.Contains("Fabricate") || col.FieldName.Contains("SupplierName") || col.FieldName.Contains("submitted"))
                                {
                                    if (col.FieldName.Contains("submitted"))
                                    {
                                        GridFormatRule GridFormatRule2 = new GridFormatRule();
                                        FormatConditionRuleValue formatConditionRuleValue = new FormatConditionRuleValue();
                                        GridFormatRule2.Column = gvSupplier.Columns[col.FieldName];
                                        GridFormatRule2.ColumnApplyTo = gvSupplier.Columns[col.FieldName.Replace("submitted", "")];
                                        formatConditionRuleValue.Appearance.BackColor = Color.YellowGreen;
                                        formatConditionRuleValue.Appearance.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                                        formatConditionRuleValue.Appearance.Options.UseBackColor = true;
                                        formatConditionRuleValue.Condition = FormatCondition.Equal;
                                        formatConditionRuleValue.Value1 = 1;
                                        GridFormatRule2.Rule = formatConditionRuleValue;
                                        gvSupplier.FormatRules.Add(GridFormatRule2);
                                    }
                                    col.Visible = false;
                                }
                                else if (col.FieldName.Contains("Check"))
                                {
                                    string strSupplierColumnName = col.FieldName.Replace("Check", "");
                                    int IColumnIndex = gvSupplier.Columns.ColumnByFieldName(strSupplierColumnName).VisibleIndex;
                                    col.VisibleIndex = IColumnIndex + 1;
                                }
                            }
                            CalculateSupplierColumns();
                            foreach (DataColumn dc in ObjESupplier.dtPositions.Columns)
                            {
                                if (dc.ColumnName.Contains("Check"))
                                {
                                    _Usertrigger = false;
                                    CheckEdit chk = new CheckEdit();
                                    UpdatePosition(gvSupplier, dc.ColumnName, chk);
                                    chk.Size = new System.Drawing.Size(18, 18);
                                    chk.Name = dc.ColumnName;
                                    chk.CheckedChanged += new EventHandler(ckBox_CheckedChanged);
                                    gcSupplier.Controls.Add(chk);
                                    DataRow[] FindRows = ObjESupplier.dtPositions.Select(dc.ColumnName + " = 0");
                                    if (FindRows.Count() == 0)
                                        chk.Checked = true;
                                    _Usertrigger = true;
                                }
                            }
                            gcSuppliers.DataSource = ObjESupplier.dtSuppliers;
                            gvSupplier_FocusedRowChanged(null, null);

                            this.gvSupplier.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
            finally { SplashScreenManager.CloseForm(false); }
        }

        void ckBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (_Usertrigger)
                {
                    CheckEdit ce = sender as CheckEdit;
                    bool _IsSelceted = false;
                    if (ce.Checked == true)
                    {
                        int iValue = -1;
                        foreach (DataRow dr in ObjESupplier.dtPositions.Rows)
                        {
                            bool _rtn = false;
                            iValue++;
                            if (bool.TryParse(Convert.ToString(dr[ce.Name]), out _rtn) && !_rtn)
                            {
                                int iPositonID = 0;
                                if (int.TryParse(Convert.ToString(dr["PositionID"]), out iPositonID))
                                {
                                    dr[ce.Name] = true;
                                    UpdateCheckBoxes(iValue, ce.Name, true, iPositonID);
                                }
                            }
                        }
                        _IsSelceted = true;
                    }
                    else
                    {
                        int iValue = -1;
                        foreach (DataRow dr in ObjESupplier.dtPositions.Rows)
                        {
                            bool _rtn = false;
                            iValue++;
                            if (bool.TryParse(Convert.ToString(dr[ce.Name]), out _rtn) && _rtn)
                            {
                                int iPositonID = 0;
                                if (int.TryParse(Convert.ToString(dr["PositionID"]), out iPositonID))
                                {
                                    dr[ce.Name] = false;
                                    UpdateCheckBoxes(iValue, ce.Name, false, iPositonID);
                                }
                            }
                        }
                    }
                    ObjESupplier.SupplierProposalID = Convert.ToInt32(gvProposal.GetFocusedRowCellValue("SupplierProposalID"));
                    int ivalue = gvSuppliers.LocateByDisplayText(0, gvSuppliers.Columns[1], ce.Name.Replace("Check", ""));
                    ObjESupplier.PSupplierID = gvSuppliers.GetRowCellValue(ivalue, "ProposalSupplierID");
                    ObjESupplier.IsSelected = _IsSelceted;
                    if (ObjESupplier.PSupplierID != null)
                        ObjESupplier = ObjBSupplier.SaveBulkSelection(ObjESupplier);
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gvSupplier_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int iIvalue = e.RowHandle;
                string strColumnboolField = e.Column.FieldName + "Check";
                if (gvSupplier.Columns[strColumnboolField] != null)
                {
                    List<decimal> l = new List<decimal>();
                    foreach (DevExpress.XtraGrid.Columns.GridColumn col in ((ColumnView)gvSupplier).Columns)
                    {
                        if (gvSupplier.Columns[col.FieldName + "Check"] != null)
                        {
                            decimal iValue = 0;
                            string str = Convert.ToString(gvSupplier.GetFocusedRowCellValue(col.FieldName));
                            if (decimal.TryParse(str, out iValue) && iValue > 0)
                            {
                                l.Add(iValue);
                            }
                        }
                    }
                    if (l != null && l.Count() > 0)
                        ObjESupplier.dtPositions.Rows[iIvalue]["Cheapest"] = Math.Round(Convert.ToDecimal(l.Min()), 2);
                    else
                        ObjESupplier.dtPositions.Rows[iIvalue]["Cheapest"] = 0.00;
                    SaveListPrice(iIvalue, e.Column.FieldName);
                }
                gvSupplier.UpdateTotalSummary();
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gvSupplier_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (e.Column.ColumnType == typeof(bool))
                {
                    int iIvalue = e.RowHandle;
                    string strFieldName = e.Column.FieldName;
                    if (Convert.ToBoolean(e.Value))
                    {
                        foreach (DataColumn dc in ObjESupplier.dtPositions.Columns)
                        {
                            if (dc.DataType == typeof(bool))
                            {
                                if (dc.ColumnName != strFieldName)
                                {
                                    bool _rtn = false;
                                    if (bool.TryParse(Convert.ToString(ObjESupplier.dtPositions.Rows[iIvalue][dc.ColumnName]), out _rtn) && _rtn)
                                    {
                                        _Usertrigger = false;
                                        ObjESupplier.dtPositions.Rows[iIvalue][dc.ColumnName] = false;
                                        gvSupplier.SetRowCellValue(iIvalue, dc.ColumnName, false);
                                        ObjESupplier.UncheckedColumn = dc.ColumnName;

                                        int iv = gvSuppliers.LocateByDisplayText(0, gvSuppliers.Columns[1], dc.ColumnName.Replace("Check", ""));
                                        ObjESupplier.NotSelectedPSupplierID = gvSuppliers.GetRowCellValue(iv, "ProposalSupplierID");

                                        CheckEdit ch = (CheckEdit)this.Controls.Find(dc.ColumnName, true)[0];
                                        ch.Checked = false;
                                    }
                                }
                                else
                                {
                                    ObjESupplier.dtPositions.Rows[iIvalue][dc.ColumnName] = true;
                                    gvSupplier.SetRowCellValue(iIvalue, dc.ColumnName, true);
                                }
                            }
                        }
                        bool _IsCheck = false;
                        foreach (DataRow dr in ObjESupplier.dtPositions.Rows)
                        {
                            if (!Convert.ToBoolean(dr[strFieldName]))
                            {
                                _IsCheck = false;
                                break;
                            }
                            else
                                _IsCheck = true;
                        }
                        if (_IsCheck)
                        {
                            CheckEdit ch = (CheckEdit)this.Controls.Find(strFieldName, true)[0];
                            ch.Checked = true;
                        }
                        _Usertrigger = true;
                        ObjESupplier.IsSelected = true;
                    }
                    else
                    {
                        _Usertrigger = false;
                        ObjESupplier.UncheckedColumn = string.Empty;
                        ObjESupplier.IsSelected = false;
                        CheckEdit ch = (CheckEdit)this.Controls.Find(strFieldName, true)[0];
                        ch.Checked = false;
                        _Usertrigger = true;
                    }
                    ObjESupplier.dtPositions.AcceptChanges();
                    ObjESupplier.PositionID = Convert.ToInt32(gvSupplier.GetFocusedRowCellValue("PositionID"));
                    ObjESupplier.SupplierProposalID = Convert.ToInt32(gvProposal.GetFocusedRowCellValue("SupplierProposalID"));
                    ObjESupplier.SelectedColumn = strFieldName;

                    int ivalue = gvSuppliers.LocateByDisplayText(0, gvSuppliers.Columns[1], strFieldName.Replace("Check", ""));
                    ObjESupplier.SelectedPSupplierID = gvSuppliers.GetRowCellValue(ivalue, "ProposalSupplierID");

                    ObjESupplier = ObjBSupplier.SaveSelection(ObjESupplier);
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gvSupplier_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            try
            {
                if (gvSupplier.FocusedColumn != null)
                {
                    _IsValueChanged = false;
                    string strSuppliercolumnName = gvSupplier.FocusedColumn.FieldName;
                    int iIndex = gvSupplier.FocusedRowHandle;
                    string strBoolColumnName = gvSupplier.FocusedColumn.FieldName + "Check";
                    if (gvSupplier.Columns[strBoolColumnName] != null)
                    {
                        string strPositionOZ = ObjESupplier.dtPositions.Rows[iIndex]["Position_OZ"] == DBNull.Value ? "" :
                            ObjESupplier.dtPositions.Rows[iIndex]["Position_OZ"].ToString();
                        gcNewValues.Text = "spezifische Angaben : " + strPositionOZ + "/" + strSuppliercolumnName;
                        gcExistingValues.Text = "Bestehende Angaben je LV  : " + strPositionOZ + "/" + strSuppliercolumnName;

                        txtNewSupplierName.EditValue = strSuppliercolumnName;

                        if (gvSupplier.Columns[strSuppliercolumnName + "Fabricate"] != null)
                            txtNewFabrikate.EditValue = ObjESupplier.dtPositions.Rows[iIndex][strSuppliercolumnName + "Fabricate"];

                        if (gvSupplier.Columns[strSuppliercolumnName + "Multi1"] != null)
                            txtNewMulti1.EditValue = ObjESupplier.dtPositions.Rows[iIndex][strSuppliercolumnName + "Multi1"];

                        if (gvSupplier.Columns[strSuppliercolumnName + "Multi2"] != null)
                            txtNewMulti2.EditValue = ObjESupplier.dtPositions.Rows[iIndex][strSuppliercolumnName + "Multi2"];

                        if (gvSupplier.Columns[strSuppliercolumnName + "Multi3"] != null)
                            txtNewMulti3.EditValue = ObjESupplier.dtPositions.Rows[iIndex][strSuppliercolumnName + "Multi3"];

                        if (gvSupplier.Columns[strSuppliercolumnName + "Multi4"] != null)
                            txtNewMulti4.EditValue = ObjESupplier.dtPositions.Rows[iIndex][strSuppliercolumnName + "Multi4"];

                        if (gvSupplier.Columns[strSuppliercolumnName] != null)
                            txtListPreis.EditValue = ObjESupplier.dtPositions.Rows[iIndex][strSuppliercolumnName];
                    }
                    else
                    {
                        txtNewFabrikate.Text = "";
                        txtNewSupplierName.Text = "";
                        txtNewMulti1.Text = "1";
                        txtNewMulti2.Text = "1";
                        txtNewMulti3.Text = "1";
                        txtNewMulti4.Text = "1";
                        txtListPreis.Text = "0";
                        gcNewValues.Text = "Lieferantenspezifische Angaben";
                        gcExistingValues.Text = "Bestehende Angaben je LV ";
                    }
                    _IsValueChanged = true;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gvSupplier_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (gvSupplier.FocusedRowHandle >= 0)
                {
                    txtListPrice.Text = ObjESupplier.dtPositions.Rows[gvSupplier.FocusedRowHandle]["MA_listprice"] == DBNull.Value ? ""
                        : ObjESupplier.dtPositions.Rows[gvSupplier.FocusedRowHandle]["MA_listprice"].ToString();
                    txtUpdatesuppliertext.Rtf = ObjESupplier.dtPositions.Rows[gvSupplier.FocusedRowHandle]["ShortDescription"] == DBNull.Value ? ""
                        : ObjESupplier.dtPositions.Rows[gvSupplier.FocusedRowHandle]["ShortDescription"].ToString();
                    txtProposalMenge.Text = ObjESupplier.dtPositions.Rows[gvSupplier.FocusedRowHandle]["Menge"] == DBNull.Value ? ""
                        : ObjESupplier.dtPositions.Rows[gvSupplier.FocusedRowHandle]["Menge"].ToString();
                    txtProposalDim1.Text = ObjESupplier.dtPositions.Rows[gvSupplier.FocusedRowHandle]["A"] == DBNull.Value ? ""
                        : ObjESupplier.dtPositions.Rows[gvSupplier.FocusedRowHandle]["A"].ToString();
                    txtProposalDim2.Text = ObjESupplier.dtPositions.Rows[gvSupplier.FocusedRowHandle]["B"] == DBNull.Value ? ""
                        : ObjESupplier.dtPositions.Rows[gvSupplier.FocusedRowHandle]["B"].ToString();
                    txtProposalDim3.Text = ObjESupplier.dtPositions.Rows[gvSupplier.FocusedRowHandle]["L"] == DBNull.Value ? ""
                        : ObjESupplier.dtPositions.Rows[gvSupplier.FocusedRowHandle]["L"].ToString();
                    txtME.Text = ObjESupplier.dtPositions.Rows[gvSupplier.FocusedRowHandle]["ME"] == DBNull.Value ? ""
                        : ObjESupplier.dtPositions.Rows[gvSupplier.FocusedRowHandle]["ME"].ToString();
                    txtMulti1.Text = ObjESupplier.dtPositions.Rows[gvSupplier.FocusedRowHandle]["MA_Multi1"] == DBNull.Value ? ""
                        : ObjESupplier.dtPositions.Rows[gvSupplier.FocusedRowHandle]["MA_Multi1"].ToString();
                    txtMulti2.Text = ObjESupplier.dtPositions.Rows[gvSupplier.FocusedRowHandle]["MA_multi2"] == DBNull.Value ? ""
                        : ObjESupplier.dtPositions.Rows[gvSupplier.FocusedRowHandle]["MA_multi2"].ToString();
                    txtMulti3.Text = ObjESupplier.dtPositions.Rows[gvSupplier.FocusedRowHandle]["MA_multi3"] == DBNull.Value ? ""
                        : ObjESupplier.dtPositions.Rows[gvSupplier.FocusedRowHandle]["MA_multi3"].ToString();
                    txtMulti4.Text = ObjESupplier.dtPositions.Rows[gvSupplier.FocusedRowHandle]["MA_multi4"] == DBNull.Value ? ""
                        : ObjESupplier.dtPositions.Rows[gvSupplier.FocusedRowHandle]["MA_multi4"].ToString();
                    txtSupplierName.Text = ObjESupplier.dtPositions.Rows[gvSupplier.FocusedRowHandle]["LiefrantMA"] == DBNull.Value ? ""
                        : ObjESupplier.dtPositions.Rows[gvSupplier.FocusedRowHandle]["LiefrantMA"].ToString();
                    txtEinkuafpreisUSP.EditValue = ObjESupplier.dtPositions.Rows[gvSupplier.FocusedRowHandle]["MA_einkaufspreis"];
                    gvSupplier_FocusedColumnChanged(null, null);
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvSupplier.RowCount == 0)
                    return;
                rgProposalViews.EditValue = 0;

                if (Convert.ToInt16(rgProposalViews.EditValue) != 0)
                {
                    if (!Utility._IsGermany)
                        throw new Exception("Please Select List Price Per Unit Proposal View");
                    else
                        throw new Exception("Bitte wählen Sie die Ansicht 'Listenpreise pro Einheit'");
                }
                ObjESupplier.ProjectID = ObjEProject.ProjectID;
                ObjESupplier.dtUpdateSupplierPrice = new DataTable();
                ObjESupplier.dtUpdateSupplierPrice.Columns.Add("PositionID", typeof(int));
                ObjESupplier.dtUpdateSupplierPrice.Columns.Add("ListPrice", typeof(decimal));
                ObjESupplier.dtUpdateSupplierPrice.Columns.Add("Fabrikate", typeof(string));
                ObjESupplier.dtUpdateSupplierPrice.Columns.Add("Supplier", typeof(string));
                ObjESupplier.dtUpdateSupplierPrice.Columns.Add("Multi1", typeof(decimal));
                ObjESupplier.dtUpdateSupplierPrice.Columns.Add("Multi2", typeof(decimal));
                ObjESupplier.dtUpdateSupplierPrice.Columns.Add("Multi3", typeof(decimal));
                ObjESupplier.dtUpdateSupplierPrice.Columns.Add("Multi4", typeof(decimal));

                List<string> boolColumnNames = new List<string>();
                foreach (DataColumn dc in ObjESupplier.dtPositions.Columns)
                {
                    if (dc.ColumnName.Contains("Check"))
                        boolColumnNames.Add(dc.ColumnName);
                }

                foreach (DataRow dr in ObjESupplier.dtPositions.Rows)
                {
                    foreach (string s in boolColumnNames)
                    {
                        if (Convert.ToBoolean(dr[s]))
                        {
                            string strSupplierColumnName = s.Replace("Check", "");
                            DataRow drNew = ObjESupplier.dtUpdateSupplierPrice.NewRow();
                            drNew["PositionID"] = dr["PositionID"] == DBNull.Value ? -1 : dr["PositionID"];
                            drNew["ListPrice"] = dr[strSupplierColumnName] == DBNull.Value ? 0 : dr[strSupplierColumnName];
                            drNew["Fabrikate"] = dr[strSupplierColumnName + "Fabricate"] == DBNull.Value ? "" : dr[strSupplierColumnName + "Fabricate"];
                            drNew["Supplier"] = dr[strSupplierColumnName + "SupplierName"] == DBNull.Value ? "" : dr[strSupplierColumnName + "SupplierName"];
                            drNew["Multi1"] = dr[strSupplierColumnName + "Multi1"] == DBNull.Value ? 1 : dr[strSupplierColumnName + "Multi1"];
                            drNew["Multi2"] = dr[strSupplierColumnName + "Multi2"] == DBNull.Value ? 1 : dr[strSupplierColumnName + "Multi2"];
                            drNew["Multi3"] = dr[strSupplierColumnName + "Multi3"] == DBNull.Value ? 1 : dr[strSupplierColumnName + "Multi3"];
                            drNew["Multi4"] = dr[strSupplierColumnName + "Multi4"] == DBNull.Value ? 1 : dr[strSupplierColumnName + "Multi4"];
                            ObjESupplier.dtUpdateSupplierPrice.Rows.Add(drNew);
                        }
                    }
                }

                DataRow[] FindRows = ObjESupplier.dtUpdateSupplierPrice.Select("ListPrice = 0");
                if (FindRows.Count() > 0)
                {
                    var Result = XtraMessageBox.Show("Möchten Sie einen NULL-Wert als NULL in das LV übertragen oder soll stattdessen der bisherige Wert im LV beibehalten werden?", "Prüffrage..?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (Convert.ToString(Result).ToLower() == "no")
                    {
                        DataTable dtTemp = ObjESupplier.dtUpdateSupplierPrice.Copy();
                        DataView dvPrice = dtTemp.DefaultView;
                        dvPrice.RowFilter = "ListPrice <> 0";
                        ObjESupplier.dtUpdateSupplierPrice = new DataTable();
                        ObjESupplier.dtUpdateSupplierPrice = dvPrice.ToTable();
                    }
                }
                ObjESupplier = ObjBSupplier.UpdateSupplierPrice(ObjESupplier);
                frmCalcPro.UpdateStatus("Preisübersicht für Lieferanten wurde erfolgreich aktualisiert");
                gvProposal_RowClick(null, null);
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnSaveTemparary_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvSupplier.RowCount == 0)
                    return;
                if (Convert.ToInt16(rgProposalViews.EditValue) != 0)
                {
                    if (!Utility._IsGermany)
                        throw new Exception("Please Select List Price Per Unit Proposal View");
                    else
                        throw new Exception("Bitte wählen Sie die Ansicht 'Listenpreise pro Einheit'");
                }
                string strSupliercolumnName = gvSupplier.FocusedColumn.FieldName;
                string strBoolColumnName = gvSupplier.FocusedColumn.FieldName + "Check";
                if (gvSupplier.Columns[strBoolColumnName] != null)
                {
                    int iRowIndex = gvSupplier.FocusedRowHandle;
                    ObjESupplier.PositionID = Convert.ToInt32(gvSupplier.GetFocusedRowCellValue("PositionID"));
                    ObjESupplier.SupplierProposalID = Convert.ToInt32(gvProposal.GetFocusedRowCellValue("SupplierProposalID"));
                    ObjESupplier.dtPositions.Rows[iRowIndex][strSupliercolumnName + "Fabricate"] = ObjESupplier.Fabrikate = txtNewFabrikate.Text;
                    ObjESupplier.dtPositions.Rows[iRowIndex][strSupliercolumnName + "SupplierName"] = ObjESupplier.SupplierName = txtNewSupplierName.Text;

                    decimal dValue = 1;
                    if (decimal.TryParse(txtListPreis.Text, out dValue))
                        ObjESupplier.dtPositions.Rows[iRowIndex][strSupliercolumnName] = ObjESupplier.SupplierPrice = dValue;
                    else
                        ObjESupplier.SupplierPrice = 0;

                    if (decimal.TryParse(txtNewMulti1.Text, out dValue))
                        ObjESupplier.dtPositions.Rows[iRowIndex][strSupliercolumnName + "Multi1"] = ObjESupplier.Multi1 = dValue;
                    else
                        ObjESupplier.dtPositions.Rows[iRowIndex][strSupliercolumnName + "Multi1"] = ObjESupplier.Multi1 = 1;

                    if (decimal.TryParse(txtNewMulti2.Text, out dValue))
                        ObjESupplier.dtPositions.Rows[iRowIndex][strSupliercolumnName + "Multi2"] = ObjESupplier.Multi2 = dValue;
                    else
                        ObjESupplier.dtPositions.Rows[iRowIndex][strSupliercolumnName + "Multi2"] = ObjESupplier.Multi2 = 1;

                    if (decimal.TryParse(txtNewMulti3.Text, out dValue))
                        ObjESupplier.dtPositions.Rows[iRowIndex][strSupliercolumnName + "Multi3"] = ObjESupplier.Multi3 = dValue;
                    else
                        ObjESupplier.dtPositions.Rows[iRowIndex][strSupliercolumnName + "Multi3"] = ObjESupplier.Multi3 = 1;

                    if (decimal.TryParse(txtNewMulti4.Text, out dValue))
                        ObjESupplier.dtPositions.Rows[iRowIndex][strSupliercolumnName + "Multi4"] = ObjESupplier.Multi4 = dValue;
                    else
                        ObjESupplier.dtPositions.Rows[iRowIndex][strSupliercolumnName + "Multi4"] = ObjESupplier.Multi4 = 1;
                    ObjESupplier.PID = new DataTable();
                    ObjESupplier.PID.Columns.Add("ID", typeof(int));
                    if (Convert.ToInt16(rgFilter.EditValue) == 0)
                        ObjESupplier.IsSingle = true;
                    else if (Convert.ToInt16(rgFilter.EditValue) == 1)
                        ObjESupplier.IsSingle = false;
                    else if (Convert.ToInt16(rgFilter.EditValue) == 2)
                    {
                        DataTable dtTemp = ObjESupplier.dtPositions.Copy();
                        DataRow[] FilteredFromRow = dtTemp.Select("Position_OZ = '" + Utility.PrepareOZ(txtFromOZSupplier.Text, ObjEProject.LVRaster) + "'");
                        DataRow[] FilteredToRow = dtTemp.Select("Position_OZ = '" + Utility.PrepareOZ(txtToOZsupplier.Text, ObjEProject.LVRaster) + "'");

                        int ifromValue = 0;
                        int itoValue = 0;

                        if (FilteredFromRow != null && FilteredFromRow.Count() > 0)
                            ifromValue = Convert.ToInt32(FilteredFromRow[0]["SID"]);

                        if (FilteredToRow != null && FilteredToRow.Count() > 0)
                            itoValue = Convert.ToInt32(FilteredToRow[0]["SID"]);

                        if (ifromValue == 0)
                            throw new Exception("From oz invalid!");
                        if (itoValue == 0)
                            throw new Exception("To oz invalid!");
                        if (ifromValue > itoValue)
                            throw new Exception("FromOZ should be less than ToOZ!");

                        DataView dvTemp = dtTemp.DefaultView;
                        dvTemp.RowFilter = "SID >= " + ifromValue + " AND SID <= " + itoValue;
                        DataTable dtp = dvTemp.ToTable();
                        DataRow drnew = ObjESupplier.PID.NewRow();
                        foreach (DataRow dr in dtp.Rows)
                        {
                            drnew = ObjESupplier.PID.NewRow();
                            drnew["ID"] = dr["PositionID"];
                            ObjESupplier.PID.Rows.Add(drnew);
                        }
                    }

                    int ivalue = gvSuppliers.LocateByDisplayText(0, gvSuppliers.Columns["ProposalName"], ObjESupplier.SupplierName);
                    ObjESupplier.PSupplierID = gvSuppliers.GetRowCellValue(ivalue, "ProposalSupplierID");

                    ObjESupplier = ObjBSupplier.SaveProposaleValues(ObjESupplier);
                    if (Convert.ToInt16(rgFilter.EditValue) != 0)
                    {
                        gvProposal_RowClick(null, null);
                        gvSupplier.FocusedRowHandle = iRowIndex;
                        gvSupplier.FocusedColumn = gvSupplier.Columns[strSupliercolumnName];
                        ObjESupplier.IsSingle = false;
                    }
                }
                frmCalcPro.UpdateStatus("Eingaben zu Multi erfolgreich gespeichert");
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (gvProposal.RowCount > 0 && gvSupplier.RowCount > 0)
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    SplashScreenManager.Default.SetWaitFormDescription("Loading...");
                    if (rgProposalViews.EditValue != null && Convert.ToInt32(rgProposalViews.EditValue) == 0)
                        gcSupplier.DataSource = ObjESupplier.dtPositions;
                    else if(rgProposalViews.EditValue != null)
                    {
                        ObjESupplier = ObjBSupplier.ChangeProposalView(ObjESupplier, Convert.ToInt32(rgProposalViews.EditValue));
                        gcSupplier.DataSource = ObjESupplier.dtPositionscopy;
                    }
                    CalculateSupplierColumns();
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
            finally { SplashScreenManager.CloseForm(false); }

        }

        private void gvSupplier_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                if (e.IsTotalSummary)
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    switch (e.SummaryProcess)
                    {
                        case CustomSummaryProcess.Start:
                            sum = 0;
                            break;
                        case CustomSummaryProcess.Calculate:
                            string stPKz = Convert.ToString(view.GetRowCellValue(e.RowHandle, "PositionKZ"));
                            if (stPKz != "A")
                            {
                                if (stPKz != "E")
                                {
                                    double d = 0;
                                    if (double.TryParse(Convert.ToString(e.FieldValue), out d))
                                        sum += d;
                                }
                            }
                            break;
                        case CustomSummaryProcess.Finalize:
                            e.TotalValue = sum;
                            break;
                    }
                }
            }
            catch (Exception ex) { throw ex; }
        }

        private void gvSuppliers_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            try
            {
                GridHitInfo hitInfo = (sender as GridView).CalcHitInfo(e.Point);
                if (hitInfo.InColumn)
                    return;

                e.Menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("Löschen", bbDelete_SupplierClick));
            }
            catch (Exception ex) { }
        }

        private void bbDelete_SupplierClick(object sender, EventArgs e)
        {
            try
            {
                if (ObjESupplier == null)
                    ObjESupplier = new ESupplier();

                int IValue = 0;
                if (gvProposal.FocusedRowHandle >= 0 &&
                    int.TryParse(Convert.ToString(gvProposal.GetFocusedRowCellValue("SupplierProposalID")), out IValue))
                {
                    if (objEGAEB == null)
                        objEGAEB = new EGAEB();
                    objEGAEB.Supplier = Convert.ToString(gvProposal.GetFocusedRowCellValue("Supplier"));
                    frmSelectsupplier Obj = new frmSelectsupplier(objEGAEB);
                    Obj.ShowDialog();
                    if (objEGAEB.IsSave)
                    {
                        int IValuie = gvSuppliers.LocateByDisplayText(0, gvSuppliers.Columns[1], objEGAEB.SupplierName);
                        ObjESupplier.ProposalSupplierID = Convert.ToInt32(gvSuppliers.GetRowCellValue(IValuie, "ProposalSupplierID"));
                        ObjESupplier.ProposalName = Convert.ToString(gvSuppliers.GetFocusedRowCellValue("ProposalName"));
                        if (int.TryParse(Convert.ToString(gvProposal.GetFocusedRowCellValue("SupplierProposalID")), out IValuie))
                        {
                            ObjESupplier.SupplierProposalID = IValuie;
                            ObjBSupplier.DeleteSuipplierProposal(ObjESupplier);
                            FillProposalNumbers();
                            if (ObjESupplier.dsProposal != null && ObjESupplier.dsProposal.Tables[0].Rows.Count > 0)
                            {
                                Utility.Setfocus(gvProposal, "SupplierProposalID", ObjESupplier.SupplierProposalID);
                                gvProposal_RowClick(null, null);
                            }
                            else
                            {
                                List<Control> l = new List<Control>();
                                foreach (Control c in gcSupplier.Controls)
                                {
                                    if (c.GetType() == typeof(CheckEdit))
                                        l.Add(c);
                                }
                                foreach (Control c in l)
                                    gcSupplier.Controls.Remove(c);
                                gvSupplier.Columns.Clear();
                                gcSupplier.DataSource = null;
                                gcSuppliers.DataSource = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtListPreis_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == (char)Keys.Enter)
                    txtListPreis_Leave(null, null);
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtListPreis_Leave(object sender, EventArgs e)
        {
            //try
            //{
            //    if (_IsSave)
            //    {
            //        btnSaveTemparary_Click(null, null);
            //        _IsSave = false;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Utility.ShowError(ex);
            //}
        }

        private void gvSupplier_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    gvSupplier.MoveNext();
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void chkUpdateAll_CheckedChanged(object sender, EventArgs e)
        {
            _IsSave = true;
        }

        private void txtListPreis_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (_IsValueChanged)
                    _IsSave = true;
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtListPreis_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                    e.Handled = true;
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void rgFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt16(rgFilter.EditValue) == 2)
            {
                lvFromOZSupplier.Enabled = true;
                lvToOZSupplier.Enabled = true;
            }
            else
            {
                lvFromOZSupplier.Enabled = false;
                lvToOZSupplier.Enabled = false;
                txtFromOZSupplier.Text = string.Empty;
                txtToOZsupplier.Text = string.Empty;
            }
        }

        private void gvProposal_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            try
            {
                GridHitInfo hitInfo = (sender as GridView).CalcHitInfo(e.Point);
                if (hitInfo.InColumn)
                    return;

                e.Menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("Löschen Lieferant", bbDelete_SupplierClick));
            }
            catch (Exception ex) { }
        }

        private void btnExportProposalPositions_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = @"D:\";
                saveFileDialog1.Title = "Export Proposal Positons";
                saveFileDialog1.CheckPathExists = true;
                saveFileDialog1.DefaultExt = "xlsx";
                saveFileDialog1.Filter = "Excel files (*.xlsx)|*.xlsx";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog1.FileName;
                    XlsxExportOptionsEx opt = new XlsxExportOptionsEx();
                    gcSupplier.ExportToXlsx(filePath, opt);
                    Process.Start(filePath);
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        #endregion

        #region Functions

        /// <summary>
        /// Code to fetch proposal from database and bind to grid control
        /// </summary>
        private void FillProposalNumbers()
        {
            try
            {
                ObjESupplier.ProjectID = ObjEProject.ProjectID;
                ObjESupplier.LVSection = string.Empty;
                int SPID = 0;
                int.TryParse(Convert.ToString(gvProposal.GetFocusedRowCellValue("SupplierProposalID")), out SPID);
                ObjESupplier = ObjBSupplier.GetUpdateSupplierProposal(ObjESupplier);
                if (Utility.LVSectionEditAccess == "9")
                {
                    DataColumn keyColumn = ObjESupplier.dsProposal.Tables[0].Columns["SupplierProposalID"];
                    DataColumn foreignKeyColumn = ObjESupplier.dsProposal.Tables[1].Columns["SupplierProposalID"];
                    ObjESupplier.dsProposal.Relations.Add("ProposalArticles", keyColumn, foreignKeyColumn);
                    DataView dv = ObjESupplier.dsProposal.Tables[0].DefaultView;
                    dv.RowFilter = "LVSection = 'HA'";
                    gcProposal.DataSource = dv;
                    gcProposal.LevelTree.Nodes.Add("ProposalArticles", gvProposalArt);
                    gvProposalArt.ViewCaption = "Proposal Articles";
                    ExpandAllRows(gvProposal);
                }
                else
                {
                    DataColumn keyColumn = ObjESupplier.dsProposal.Tables[0].Columns["SupplierProposalID"];
                    DataColumn foreignKeyColumn = ObjESupplier.dsProposal.Tables[1].Columns["SupplierProposalID"];
                    ObjESupplier.dsProposal.Relations.Add("ProposalArticles", keyColumn, foreignKeyColumn);
                    gcProposal.DataSource = ObjESupplier.dsProposal.Tables[0];
                    gcProposal.LevelTree.Nodes.Add("ProposalArticles", gvProposalArt);
                    gvProposalArt.ViewCaption = "Proposal Articles";
                    ExpandAllRows(gvProposal);
                }
                if (SPID > 0)
                    Utility.Setfocus(gvProposal, "SupplierProposalID", SPID);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Code to place Checkboxes on grid control on each checkbox column of grid
        /// </summary>
        /// <param name="gridView"></param>
        /// <param name="columnName"></param>
        /// <param name="control"></param>
        private void UpdatePosition(GridView gridView, string columnName, Control control)
        {
            try
            {
                var column = gridView.Columns[columnName];
                if (column == null) return;

                var viewInfo = (GridViewInfo)gridView.GetViewInfo(); //using DevExpress.XtraGrid.Views.Grid.ViewInfo
                var columnInfo = viewInfo.ColumnsInfo[column];

                if (columnInfo != null)
                {
                    var bounds = columnInfo.Bounds; //column's rectangle of coordinates relative to GridControl

                    var point = PointToClient(gridView.GridControl.PointToScreen(bounds.Location)); //translating to form's coordinates

                    control.Left = bounds.X + 6;
                    control.Top = bounds.Y;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        /// <summary>
        /// Code to Change the check status of checkbox column on changing header check box status
        /// </summary>
        /// <param name="iIvalue"></param>
        /// <param name="strFieldName"></param>
        /// <param name="IsChecked"></param>
        /// <param name="iPositionID"></param>
        private void UpdateCheckBoxes(int iIvalue, string strFieldName, bool IsChecked, int iPositionID)
        {
            try
            {
                if (IsChecked)
                {
                    foreach (DataColumn dc in ObjESupplier.dtPositions.Columns)
                    {
                        if (dc.DataType == typeof(bool))
                        {
                            if (dc.ColumnName != strFieldName)
                            {
                                bool _rtn = false;
                                if (bool.TryParse(Convert.ToString(ObjESupplier.dtPositions.Rows[iIvalue][dc.ColumnName]), out _rtn) && _rtn)
                                {
                                    _Usertrigger = false;
                                    ObjESupplier.dtPositions.Rows[iIvalue][dc.ColumnName] = false;
                                    gvSupplier.SetRowCellValue(iIvalue, dc.ColumnName, false);
                                    ObjESupplier.UncheckedColumn = dc.ColumnName;
                                    CheckEdit ch = (CheckEdit)this.Controls.Find(dc.ColumnName, true)[0];
                                    ch.Checked = false;
                                }
                            }
                            else
                            {
                                ObjESupplier.dtPositions.Rows[iIvalue][dc.ColumnName] = true;
                                gvSupplier.SetRowCellValue(iIvalue, dc.ColumnName, true);
                            }

                        }
                    }
                    ObjESupplier.IsSelected = true;
                }
                else
                {
                    ObjESupplier.UncheckedColumn = string.Empty;
                    ObjESupplier.IsSelected = false;
                }
                ObjESupplier.dtPositions.AcceptChanges();
                gvSupplier.RefreshData();
                //ObjESupplier.PositionID = iPositionID;
                //ObjESupplier.SupplierProposalID = Convert.ToInt32(gvProposal.GetFocusedRowCellValue("SupplierProposalID"));
                //ObjESupplier.SelectedColumn = strFieldName;
                //ObjESupplier = ObjBSupplier.SaveSelection(ObjESupplier);
                _Usertrigger = true;
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        /// <summary>
        /// Code to save list price to positions
        /// </summary>
        /// <param name="iRowIndex"></param>
        /// <param name="strSupliercolumnName"></param>
        private void SaveListPrice(int iRowIndex, string strSupliercolumnName)
        {
            try
            {
                if (Convert.ToInt16(rgProposalViews.EditValue) != 0)
                {
                    if (!Utility._IsGermany)
                        throw new Exception("Please Select List Price Per Unit Proposal View");
                    else
                        throw new Exception("Bitte wählen Sie die Ansicht 'Listenpreise pro Einheit'");
                }
                string strBoolColumnName = strSupliercolumnName + "Check";
                if (gvSupplier.Columns[strBoolColumnName] != null)
                {
                    ObjESupplier.PositionID = Convert.ToInt32(ObjESupplier.dtPositions.Rows[iRowIndex]["PositionID"]);
                    ObjESupplier.SupplierProposalID = Convert.ToInt32(gvProposal.GetFocusedRowCellValue("SupplierProposalID"));
                    decimal dValue = 1;
                    string strName = Convert.ToString(ObjESupplier.dtPositions.Rows[iRowIndex][txtNewSupplierName.Text]);
                    if (decimal.TryParse(strName, out dValue))
                        ObjESupplier.SupplierPrice = dValue;
                    else
                        ObjESupplier.SupplierPrice = 0;
                    txtListPreis.EditValue = ObjESupplier.SupplierPrice;

                    int ivalue = gvSuppliers.LocateByDisplayText(0, gvSuppliers.Columns["ProposalName"], txtNewSupplierName.Text);
                    ObjESupplier.PSupplierID = gvSuppliers.GetRowCellValue(ivalue, "ProposalSupplierID");

                    ObjESupplier.IsSingle = true;
                    ObjESupplier = ObjBSupplier.SaveSupplierPrice(ObjESupplier);
                    ObjESupplier.IsSingle = false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Code to calculate supplier cheapest columns
        /// </summary>
        private void CalculateSupplierColumns()
        {
            try
            {
                if (gvProposal.FocusedColumn != null && gvProposal.GetFocusedRowCellValue("SupplierProposalID") != null && gvSupplier.RowCount > 0)
                {
                    string strSupplier = Convert.ToString(gvProposal.GetFocusedRowCellValue("Supplier"));
                    if (!string.IsNullOrEmpty(strSupplier))
                    {
                        if (gvSupplier.Columns["Position_OZ"] != null)
                            gvSupplier.Columns["Position_OZ"].Width = 150;

                        if (gvSupplier.Columns["PositionKZ"] != null)
                            gvSupplier.Columns["PositionKZ"].Width = 80;
                        string maskstring = "n" + Convert.ToString(ObjEProject.RoundingPrice);
                        this.riSupplier.Mask.EditMask = maskstring;
                        string[] ShortName = strSupplier.Split(',');
                        foreach (DataRow dr in ObjESupplier.dtSuppliers.Rows)
                        {
                            string _strShort = Convert.ToString(dr["ProposalName"]).Trim();
                            if (gvSupplier.Columns[_strShort] != null)
                            {
                                gvSupplier.Columns[_strShort].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Custom;
                                gvSupplier.Columns[_strShort].SummaryItem.FieldName = _strShort;
                                gvSupplier.Columns[_strShort].SummaryItem.DisplayFormat = "{0:n2}";
                                gvSupplier.Columns[_strShort].ColumnEdit = riSupplier;
                                gvSupplier.Columns[_strShort].Width = 120;
                                if (gvSupplier.Columns[_strShort + "Check"] != null)
                                    gvSupplier.Columns[_strShort + "Check"].Width = 28;
                            }
                        }
                        if (gvSupplier.Columns["Cheapest"] != null)
                        {
                            gvSupplier.Columns["Cheapest"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Custom;
                            gvSupplier.Columns["Cheapest"].SummaryItem.FieldName = "Cheapest";
                            gvSupplier.Columns["Cheapest"].SummaryItem.DisplayFormat = "{0:n2}";
                            gvSupplier.Columns["Cheapest"].ColumnEdit = riSupplier;
                            gvSupplier.Columns["Cheapest"].Width = 120;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #endregion

        #region Copy LVs

        #region Events

        private void lookUpEditOldProject_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                ds_oldPrj = ObjBPosition.GetOldPositionList(Convert.ToInt32(lookUpEditOldProject.EditValue));
                if (ds_oldPrj != null && ds_oldPrj.Tables.Count > 0)
                {
                    tlOldProject.DataSource = ds_oldPrj.Tables[0];
                    tlOldProject.ParentFieldName = "Parent_OZ";
                    tlOldProject.KeyFieldName = "PositionID";
                    tlOldProject.ForceInitialize();
                    tlOldProject.ExpandAll();
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void tlNewProject_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void tlNewProject_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (tlNewProject.Nodes.Count() == 0 || Utility.LVDetailsAccess == "7" || ObjEProject.IsFinalInvoice)
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }
                DXDragEventArgs args = tlNewProject.GetDXDragEventArgs(e);
                DataRow dataRow = (tlOldProject.GetDataRecordByNode(tlOldProject.FocusedNode) as DataRowView).Row;
                if (dataRow == null) return;
                string _OldPosKZ = dataRow["PositionKZ"] == DBNull.Value ? "" : dataRow["PositionKZ"].ToString();
                int IPositionID = dataRow["PositionID"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PositionID"]);
                if (_OldPosKZ == "NG")
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }
                TreeListNode node = args.TargetNode;
                if (args.TargetNode == null)
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }
                string _PosKZ = Convert.ToString(node["PositionKZ"]);
                string strOZ = Convert.ToString(node["Position_OZ"]);
                int I_index = 0;
                string ParentOZ = string.Empty;
                string Position_OZ = string.Empty;
                if (string.IsNullOrEmpty(strOZ))
                    return;
                if (_PosKZ == "NG")
                {
                    if (Convert.ToInt16(rgDropMode1.EditValue) == 2)
                    {
                        if (!Utility._IsGermany)
                            throw new Exception("Please select different copy mode");
                        else
                            throw new Exception("Bitte wählen Sie einen anderen Kopiermodus");
                    }

                    string[] _Raster = ObjEProject.LVRaster.Split('.');
                    int _Rastercount = _Raster.Count();
                    ParentOZ = node["Position_OZ"].ToString();
                    string[] _OZ = ParentOZ.Split('.');
                    int _OZCount = _OZ.Count();
                    if (_OZCount != _Rastercount - 1)
                    {
                        e.Effect = DragDropEffects.None;
                        return;
                    }
                    string strSelectedOZ = "";
                    string strNO = "";
                    if (Convert.ToInt16(rgDropMode1.EditValue) == 0)
                    {
                        if (node.FirstNode != null)
                        {
                            strNO = Convert.ToString(node.FirstNode["SNO"]);
                            int INodeIndex = 0;
                            bool _Continue = true;
                            int IValue = -1;
                            while (_Continue)
                            {
                                IValue++;
                                if (node.Nodes.Count > INodeIndex + IValue)
                                {
                                    strSelectedOZ = Convert.ToString(node.Nodes[INodeIndex + IValue]["Position_OZ"]);
                                    if (!string.IsNullOrEmpty(strSelectedOZ))
                                        _Continue = false;
                                }
                                else
                                    _Continue = false;
                            }
                        }
                        int iTemp = 0;
                        if (!int.TryParse(strNO, out iTemp))
                            I_index = 0;
                        else
                            I_index = iTemp - 1;
                    }
                    else if (Convert.ToInt16(rgDropMode1.EditValue) == 1)
                    {
                        if (node.LastNode != null)
                        {
                            strNO = Convert.ToString(node.LastNode["SNO"]);
                            strSelectedOZ = Convert.ToString(node.LastNode["Position_OZ"]);
                            if (string.IsNullOrEmpty(strSelectedOZ))
                                return;
                        }
                        if (!int.TryParse(strNO, out I_index))
                            I_index = 0;
                    }
                    string _Suggested_OZ = SuggestOZForCopy(strSelectedOZ, "", Convert.ToInt16(rgDropMode1.EditValue));

                    string str = string.Empty;
                    string strRaster = ObjEProject.LVRaster;
                    string[] strPOZ = strSelectedOZ.Split('.');
                    string[] strPRaster = strRaster.Split('.');
                    int Count = -1;
                    int i = -1;
                    Count = strPOZ.Count();
                    while (Count > 0)
                    {
                        i = i + 1;
                        Count = Count - 1;
                        int OZLength = 0;
                        int RasterLength = 0;
                        string OZ = string.Empty;
                        OZ = strPOZ[i].Trim();
                        RasterLength = strPRaster[i].Length;
                        OZLength = OZ.Trim().Length;
                        if (Count > 0)
                        {
                            if (OZ == "")
                            {
                                str = str + string.Concat(Enumerable.Repeat(" ", RasterLength - OZLength)) + OZ + ".";
                            }
                        }
                    }
                    if (str.Length > 0)
                    {
                        string strBlankNewOz = ParentOZ + str + _Suggested_OZ;
                        Position_OZ = strBlankNewOz;
                    }
                    else
                    {
                        string strNewOz = ParentOZ + _Suggested_OZ;
                        Position_OZ = strNewOz;
                    }
                }
                else
                {
                    ParentOZ = Convert.ToString(node.ParentNode.GetValue("Position_OZ"));
                    string strSelectedOZ = "";
                    string strnextLV = "";
                    if (Convert.ToInt16(rgDropMode1.EditValue) == 2)
                    {
                        strSelectedOZ = Convert.ToString(node["Position_OZ"]);
                        int INodeIndex = tlNewProject.GetNodeIndex(node);
                        if (INodeIndex != null)
                        {
                            bool _Continue = true;
                            int IValue = 0;
                            while (_Continue)
                            {
                                IValue++;
                                if (node.ParentNode.Nodes.Count > INodeIndex + IValue)
                                {
                                    strnextLV = Convert.ToString(node.ParentNode.Nodes[INodeIndex + IValue]["Position_OZ"]);
                                    if (!string.IsNullOrEmpty(strnextLV))
                                        _Continue = false;
                                }
                                else
                                    _Continue = false;
                            }
                        }
                        string strNo = Convert.ToString(node["SNO"]);
                        if (!int.TryParse(strNo, out I_index))
                            I_index = 0;
                    }
                    else if (Convert.ToInt16(rgDropMode1.EditValue) == 1)
                    {
                        string strNO = Convert.ToString(node.ParentNode.LastNode["SNO"]);
                        strSelectedOZ = Convert.ToString(node.ParentNode.LastNode["Position_OZ"]);
                        if (string.IsNullOrEmpty(strSelectedOZ))
                            return;
                        if (!int.TryParse(strNO, out I_index))
                            I_index = 0;
                    }
                    else
                    {
                        string strNO = Convert.ToString(node.ParentNode.FirstNode["SNO"]);
                        int INodeIndex = 0;
                        bool _Continue = true;
                        int IValue = -1;
                        while (_Continue)
                        {
                            IValue++;
                            if (node.ParentNode.Nodes.Count > INodeIndex + IValue)
                            {
                                strSelectedOZ = Convert.ToString(node.ParentNode.Nodes[INodeIndex + IValue]["Position_OZ"]);
                                if (!string.IsNullOrEmpty(strSelectedOZ))
                                    _Continue = false;
                            }
                            else
                                _Continue = false;
                        }
                        int iTemp = 0;
                        if (!int.TryParse(strNO, out iTemp))
                            I_index = 0;
                        else
                            I_index = iTemp - 1;
                    }
                    string _Suggested_OZ = SuggestOZForCopy(strSelectedOZ, strnextLV, Convert.ToInt16(rgDropMode1.EditValue));

                    frmNewOZ Obj = new frmNewOZ();
                    Obj.strNewOZ = _Suggested_OZ;
                    Obj.LVRaster = ObjEProject.LVRaster;
                    Obj.ShowDialog();
                    if (!Obj.IsSave)
                        return;
                    _Suggested_OZ = Obj.strNewOZ;

                    string str = string.Empty;
                    string strRaster = ObjEProject.LVRaster;
                    string[] strPOZ = strSelectedOZ.Split('.');
                    string[] strPRaster = strRaster.Split('.');
                    int Count = -1;
                    int i = -1;
                    Count = strPOZ.Count();
                    while (Count > 0)
                    {
                        i = i + 1;
                        Count = Count - 1;
                        int OZLength = 0;
                        int RasterLength = 0;
                        string OZ = string.Empty;
                        OZ = strPOZ[i].Trim();
                        RasterLength = strPRaster[i].Length;
                        OZLength = OZ.Trim().Length;
                        if (Count > 0)
                        {
                            if (OZ == "")
                            {
                                str = str + string.Concat(Enumerable.Repeat(" ", RasterLength - OZLength)) + OZ + ".";
                            }
                        }
                    }
                    if (str.Length > 0)
                    {
                        string strBlankNewOz = ParentOZ + str + _Suggested_OZ;
                        Position_OZ = Utility.PrepareOZ(strBlankNewOz, ObjEProject.LVRaster);
                    }
                    else
                    {
                        string strNewOz = ParentOZ + _Suggested_OZ;
                        Position_OZ = Utility.PrepareOZ(strNewOz, ObjEProject.LVRaster);
                    }
                }
                string strLongDescription = ObjBPosition.GetLongDescription(IPositionID);

                string _Selected_OZ = dataRow["Position_OZ"] == DBNull.Value ? "" : dataRow["Position_OZ"].ToString();
                DataTable dtTable = ds_oldPrj.Tables[0].Copy();

                DataView dv = dtTable.DefaultView;
                dv.RowFilter = "Position_OZ= '" + _Selected_OZ + "'";
                DataTable dt = dv.ToTable();
                int NewPositionID = 0;
                ObjEPosition.RoundOffValue = ObjEProject.RoundingPrice;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        ParseLVAndDetailKZCopyLV(row, Position_OZ, ParentOZ, I_index, strLongDescription, true);
                        ObjBPosition.SavePositionDetails(ObjEPosition, ObjEProject.LVRaster, ObjEProject.LVSprunge, true);
                        NewPositionID = ObjEPosition.PositionID;
                        I_index++;
                    }
                }
                else
                {
                    ParseLVAndDetailKZCopyLV(dataRow, Position_OZ, ParentOZ, I_index, strLongDescription, true);
                    ObjBPosition.SavePositionDetails(ObjEPosition, ObjEProject.LVRaster, ObjEProject.LVSprunge, true);
                    NewPositionID = ObjEPosition.PositionID;
                }

                ObjBPosition.GetPositionList(ObjEPosition, Convert.ToInt32(ObjEProject.ProjectID));
                if (ObjEPosition.dsPositionList != null && ObjEPosition.dsPositionList.Tables.Count > 0)
                {
                    tlNewProject.DataSource = ObjEPosition.dsPositionList.Tables[0];
                    tlNewProject.ParentFieldName = "Parent_OZ";
                    tlNewProject.KeyFieldName = "PositionID";
                    tlNewProject.ForceInitialize();
                    tlNewProject.ExpandAll();
                }
                SetFocus(NewPositionID, tlNewProject);
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        #endregion

        #region Functions
        /// <summary>
        ///  Code to parse position details to entitites while copying a position from another project
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="strPositionsOZ"></param>
        /// <param name="strParetntOZ"></param>
        /// <param name="iTempSNO"></param>
        /// <param name="strLongDescription"></param>
        /// <param name="iDetailKZ"></param>
        private void ParsePositionDetailsfoCopyLV(DataRow dr, string strPositionsOZ, string strParetntOZ, int iTempSNO, string strLongDescription, int iDetailKZ = -1)
        {
            try
            {
                int iValue = 0;
                DateTime dt = DateTime.Now;
                ObjEPosition.ProjectID = ObjEProject.ProjectID;
                ObjEPosition.PositionID = -1;
                ObjEPosition.PositionKZ = Convert.ToString(dr["PositionKZ"]);
                ObjEPosition.PositionKZID = Convert.ToString(dr["PositionKZID"]);
                ObjEPosition.Position_OZ = strPositionsOZ;
                ObjEPosition.Parent_OZ = strParetntOZ;
                if (iDetailKZ > 0)
                    ObjEPosition.DetailKZ = iDetailKZ;
                else
                {
                    if (int.TryParse(Convert.ToString(dr["DetailKZ"]), out iValue))
                        ObjEPosition.DetailKZ = iValue;
                    else
                        ObjEPosition.DetailKZ = 0;
                }

                if (cmbLVSection.Text == string.Empty)
                    ObjEPosition.LVSection = "HA";
                else
                    ObjEPosition.LVSection = cmbLVSection.Text;
                ObjEPosition.LVSectionID = cmbLVSection.EditValue;

                if (iDetailKZ == 1)
                {
                    if (ObjBPosition == null)
                        ObjBPosition = new BPosition();
                    int PosID = 0;
                    PosID = dr["PositionID"] == DBNull.Value ? -1 : Convert.ToInt32(dr["PositionID"]);
                    string strLongDesc = ObjBPosition.GetLongDescription(PosID);
                    ObjEPosition.WG = Convert.ToString(dr["WG"]);
                    ObjEPosition.WA = Convert.ToString(dr["WA"]);
                    ObjEPosition.WI = Convert.ToString(dr["WI"]);
                    ObjEPosition.Menge = 1;
                    ObjEPosition.ME = Convert.ToString(dr["ME"]);
                    ObjEPosition.Fabricate = Convert.ToString(dr["Fabricate"]);
                    ObjEPosition.LiefrantMA = Convert.ToString(dr["LiefrantMA"]);
                    ObjEPosition.Type = Convert.ToString(dr["Type"]);
                    ObjEPosition.LongDescription = strLongDesc;
                    ObjEPosition.ShortDescription = Utility.GetRTFFormat(txtShortDescription.Text.Trim());
                    ObjEPosition.Surcharge_From = Convert.ToString(dr["surchargefrom"]);
                    ObjEPosition.Surcharge_To = Convert.ToString(dr["surchargeto"]);
                    ObjEPosition.Surcharge_Per = 0;
                    ObjEPosition.surchargePercentage_MO = 0;
                    DateTime dtv = ObjEProject.SubmitDate;
                    if (DateTime.TryParse(Convert.ToString(dr["validitydate"]), out dtv))
                        ObjEPosition.ValidityDate = dtv;
                    else
                        ObjEPosition.ValidityDate = ObjEProject.SubmitDate;
                    ObjEPosition.MA = Convert.ToString(dr["MA"]);
                    ObjEPosition.MO = Convert.ToString(dr["MO"]);
                    ObjEPosition.Mins = dr["MINUTES"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["MINUTES"]);
                    ObjEPosition.Faktor = dr["Faktor"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["Faktor"]);
                    ObjEPosition.LPMA = dr["MA_listprice"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["MA_listprice"]);
                    ObjEPosition.LPMO = dr["MO_listprice"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["MO_listprice"]);
                    ObjEPosition.Multi1MA = dr["MA_Multi1"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["MA_Multi1"]);
                    ObjEPosition.Multi2MA = dr["MA_multi2"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["MA_multi2"]);
                    ObjEPosition.Multi3MA = dr["MA_multi3"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["MA_multi3"]);
                    ObjEPosition.Multi4MA = dr["MA_multi4"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["MA_multi4"]);
                    ObjEPosition.Multi1MO = dr["MO_multi1"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["MO_multi1"]);
                    ObjEPosition.Multi2MO = dr["MO_multi2"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["MO_multi2"]);
                    ObjEPosition.Multi3MO = dr["MO_multi3"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["MO_multi3"]);
                    ObjEPosition.Multi4MO = dr["MO_multi4"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["MO_multi4"]);
                    ObjEPosition.EinkaufspreisMA = dr["MA_einkaufspreis"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["MA_einkaufspreis"]);
                    ObjEPosition.EinkaufspreisMO = dr["MO_einkaufspreis"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["MO_einkaufspreis"]);
                    ObjEPosition.SelbstkostenMultiMA = dr["MA_selbstkostenMulti"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["MA_selbstkostenMulti"]);
                    ObjEPosition.SelbstkostenValueMA = dr["MA_selbstkosten"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["MA_selbstkosten"]);
                    ObjEPosition.SelbstkostenMultiMO = dr["MO_selbstkostenMulti"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["MO_selbstkostenMulti"]);
                    ObjEPosition.SelbstkostenValueMO = dr["MO_selbstkosten"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["MO_selbstkosten"]);
                    ObjEPosition.VerkaufspreisMultiMA = dr["MA_verkaufspreis_Multi"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["MA_verkaufspreis_Multi"]);
                    ObjEPosition.VerkaufspreisValueMA = dr["MA_verkaufspreis"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["MA_verkaufspreis"]);
                    ObjEPosition.VerkaufspreisMultiMO = dr["MO_verkaufspreisMulti"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["MO_verkaufspreisMulti"]);
                    ObjEPosition.VerkaufspreisValueMO = dr["MO_verkaufspreis"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["MO_verkaufspreis"]);
                    ObjEPosition.StdSatz = dr["std_satz"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["std_satz"]);
                    ObjEPosition.PreisText = Convert.ToString(dr["PreisText"]);
                    ObjEPosition.EinkaufspreisLockMA = dr["MA_einkaufspreis_lck"] == DBNull.Value ? false : Convert.ToBoolean(dr["MA_einkaufspreis_lck"]);
                    ObjEPosition.EinkaufspreisLockMO = dr["MO_Einkaufspreis_lck"] == DBNull.Value ? false : Convert.ToBoolean(dr["MO_Einkaufspreis_lck"]);
                    ObjEPosition.SelbstkostenLockMA = dr["MA_selbstkosten_lck"] == DBNull.Value ? false : Convert.ToBoolean(dr["MA_selbstkosten_lck"]);
                    ObjEPosition.SelbstkostenLockMO = dr["MO_selbstkosten_lck"] == DBNull.Value ? false : Convert.ToBoolean(dr["MO_selbstkosten_lck"]);
                    ObjEPosition.VerkaufspreisLockMA = dr["MA_verkaufspreis_lck"] == DBNull.Value ? false : Convert.ToBoolean(dr["MA_verkaufspreis_lck"]);
                    ObjEPosition.VerkaufspreisLockMO = dr["MO_verkaufspreis_lck"] == DBNull.Value ? false : Convert.ToBoolean(dr["MO_verkaufspreis_lck"]);
                    ObjEPosition.Dim1 = Convert.ToString(dr["A"]);
                    ObjEPosition.Dim2 = Convert.ToString(dr["B"]);
                    ObjEPosition.Dim3 = Convert.ToString(dr["L"]);
                    ObjEPosition.LVStatus = Convert.ToString(dr["LVStatus"]);
                    ObjEPosition.LVStatusID = dr["LVStatusID"];
                    ObjEPosition.GrandTotalME = dr["GrandTotalME"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["GrandTotalME"]);
                    ObjEPosition.GrandTotalMO = dr["GrandTotalMO"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["GrandTotalMO"]);
                    ObjEPosition.FinalGB = dr["GB"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["GB"]);
                    ObjEPosition.EP = dr["EP"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["EP"]);
                    bool MontageCheck = false;
                    if (bool.TryParse(Convert.ToString(dr["MontageEntry"]), out MontageCheck))
                        ObjEPosition.MontageEntry = MontageCheck;
                    else
                        ObjEPosition.MontageEntry = MontageCheck;
                }
                else
                {
                    ObjEPosition.WG = string.Empty;
                    ObjEPosition.WA = string.Empty;
                    ObjEPosition.WI = string.Empty;
                    ObjEPosition.Menge = 1;
                    ObjEPosition.ME = string.Empty;
                    ObjEPosition.Fabricate = string.Empty;
                    ObjEPosition.LiefrantMA = string.Empty;
                    ObjEPosition.Type = string.Empty;
                    ObjEPosition.LongDescription = string.Empty;
                    ObjEPosition.ShortDescription = Utility.GetRTFFormat(txtShortDescription.Text.Trim());
                    ObjEPosition.Surcharge_From = string.Empty;
                    ObjEPosition.Surcharge_To = string.Empty;
                    ObjEPosition.Surcharge_Per = 0;
                    ObjEPosition.surchargePercentage_MO = 0;
                    ObjEPosition.ValidityDate = DateTime.Now;
                    ObjEPosition.MA = "X";
                    ObjEPosition.MO = "X";
                    ObjEPosition.Mins = 0;
                    ObjEPosition.Faktor = 1;
                    ObjEPosition.LPMA = 0;
                    ObjEPosition.LPMO = 0;
                    ObjEPosition.Multi1MA = 1;
                    ObjEPosition.Multi2MA = 1;
                    ObjEPosition.Multi3MA = 1;
                    ObjEPosition.Multi4MA = 1;
                    ObjEPosition.Multi1MO = 1;
                    ObjEPosition.Multi2MO = 1;
                    ObjEPosition.Multi3MO = 1;
                    ObjEPosition.Multi4MO = 1;
                    ObjEPosition.EinkaufspreisMA = 0;
                    ObjEPosition.EinkaufspreisMO = 0;
                    ObjEPosition.SelbstkostenMultiMA = 1;
                    ObjEPosition.SelbstkostenValueMA = 0;
                    ObjEPosition.SelbstkostenMultiMO = 1;
                    ObjEPosition.SelbstkostenValueMO = 0;
                    ObjEPosition.VerkaufspreisMultiMA = 1;
                    ObjEPosition.VerkaufspreisValueMA = 0;
                    ObjEPosition.VerkaufspreisMultiMO = 1;
                    ObjEPosition.VerkaufspreisValueMO = 0;
                    ObjEPosition.StdSatz = 0;
                    ObjEPosition.PreisText = string.Empty;
                    ObjEPosition.EinkaufspreisLockMA = false;
                    ObjEPosition.EinkaufspreisLockMO = false;
                    ObjEPosition.SelbstkostenLockMA = false;
                    ObjEPosition.SelbstkostenLockMO = false;
                    ObjEPosition.VerkaufspreisLockMA = false;
                    ObjEPosition.VerkaufspreisLockMO = false;
                    ObjEPosition.Dim1 = "";
                    ObjEPosition.Dim2 = "";
                    ObjEPosition.Dim3 = "";
                    ObjEPosition.LVStatus = string.Empty;
                    ObjEPosition.LVStatusID = 0;
                    ObjEPosition.GrandTotalME = 0;
                    ObjEPosition.GrandTotalMO = 0;
                    ObjEPosition.FinalGB = 0;
                    ObjEPosition.EP = 0;
                    ObjEPosition.MontageEntry = false;
                }
                ObjEPosition.SNO = iTempSNO;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Code to Suggest OZ while copying from another project
        /// </summary>
        /// <param name="PositionOZ"></param>
        /// <param name="strNextLV"></param>
        /// <param name="Index"></param>
        /// <returns></returns>
        private string SuggestOZForCopy(string PositionOZ, string strNextLV, int Index = 0)
        {
            string strNewOZ = string.Empty;
            try
            {
                if (Index == 0)
                {
                    string[] OZList = PositionOZ.Split('.');
                    if (OZList != null && OZList.Count() > 1)
                    {
                        string OnheStufe = OZList[OZList.Count() - 2];
                        int Ivalue = 0;
                        if (int.TryParse(OnheStufe.Trim(), out Ivalue))
                        {
                            int iNewOZ = Ivalue - ObjEProject.LVSprunge;
                            int iTemp = 0;
                            if (iNewOZ <= 0)
                            {
                                iTemp = Ivalue - 1;
                                if (iTemp <= 0)
                                    throw new Exception("Gemäß LV Sprung kann die Position nicht an den Anfang des Positionsbereichs kopiert werden");
                                else
                                    strNewOZ = iTemp.ToString() + ".";
                            }
                            else
                                strNewOZ = iNewOZ.ToString() + ".";
                        }
                        else if (Regex.IsMatch(OnheStufe, "[a-zA-Z]"))
                            strNewOZ = string.Empty;
                        else
                            throw new Exception("Bitte wählen Sie einen anderen Kopiermodus");
                    }
                    else
                        strNewOZ = ObjEProject.LVSprunge.ToString() + ".";
                }
                else if (Index == 1)
                {
                    string[] OZList = PositionOZ.Split('.');
                    if (OZList != null && OZList.Count() > 1)
                    {
                        string OnheStufe = OZList[OZList.Count() - 2];
                        int Ivalue = 0;
                        if (int.TryParse(OnheStufe.Trim(), out Ivalue))
                        {
                            int iNewOZ = Ivalue + ObjEProject.LVSprunge;
                            strNewOZ = iNewOZ.ToString() + ".";
                        }
                        else if (Regex.IsMatch(OnheStufe, "[a-zA-Z]"))
                            strNewOZ = string.Empty;
                        else
                            throw new Exception("Bitte wählen Sie einen anderen Kopiermodus");
                    }
                    else
                        strNewOZ = ObjEProject.LVSprunge.ToString() + ".";
                }
                else
                {
                    if (string.IsNullOrEmpty(strNextLV))
                    {
                        string[] OZList = PositionOZ.Split('.');
                        if (OZList != null && OZList.Count() > 0)
                        {
                            string OnheStufe = OZList[OZList.Count() - 2];
                            int Ivalue = 0;
                            if (int.TryParse(OnheStufe.Trim(), out Ivalue))
                            {
                                int iNewOZ = Ivalue + ObjEProject.LVSprunge;
                                strNewOZ = iNewOZ.ToString() + ".";
                            }
                            else if (Regex.IsMatch(OnheStufe, "[a-zA-Z]"))
                                strNewOZ = string.Empty;
                            else
                                throw new Exception("Bitte wählen Sie einen anderen Kopiermodus");
                        }
                        else
                            throw new Exception("Bitte wählen Sie einen anderen Kopiermodus");
                    }
                    else
                    {
                        string[] OZList = PositionOZ.Split('.');
                        string[] NextOZList = strNextLV.Split('.');
                        if (OZList != null && OZList.Count() > 0)
                        {
                            string SelectedOnheStufe = OZList[OZList.Count() - 2];
                            string NextOnheStufe = NextOZList[NextOZList.Count() - 2];
                            int ISelectedvalue = 0;
                            int INextLVValue = 0;
                            if (int.TryParse(SelectedOnheStufe.Trim(), out ISelectedvalue))
                            {
                                int iTemp = ISelectedvalue + ObjEProject.LVSprunge;
                                if (int.TryParse(NextOnheStufe.Trim(), out INextLVValue))
                                {
                                    if (iTemp < INextLVValue)
                                    {
                                        strNewOZ = iTemp.ToString() + ".";
                                    }
                                    else if (iTemp >= INextLVValue)
                                    {
                                        int INewValue = ISelectedvalue + 1;
                                        if (INewValue == INextLVValue)
                                        {
                                            string strIndex = "";
                                            int iIndex = 0;
                                            strIndex = OZList[OZList.Count() - 1];
                                            if (int.TryParse(strIndex.Trim(), out iIndex))
                                            {
                                                string strNextIndex = NextOZList[NextOZList.Count() - 1];
                                                int iNextOzIndex = 0;
                                                if (int.TryParse(strNextIndex, out iNextOzIndex))
                                                {
                                                    if (iNextOzIndex != iIndex + 1)
                                                    {
                                                        if (iIndex < 9)
                                                            strNewOZ = ISelectedvalue.ToString() + "." + (iIndex + 1).ToString();
                                                        else
                                                            throw new Exception("Gemäß LV Sprung kann die Position nicht an den Anfang des Positionsbereichs kopiert werden");
                                                    }
                                                    else
                                                        throw new Exception("Gemäß LV Sprung kann die Position nicht an den Anfang des Positionsbereichs kopiert werden");
                                                }
                                                else
                                                {
                                                    if (iIndex < 9)
                                                        strNewOZ = ISelectedvalue.ToString() + "." + (iIndex + 1).ToString();
                                                    else
                                                        throw new Exception("Gemäß LV Sprung kann die Position nicht an den Anfang des Positionsbereichs kopiert werden");
                                                }
                                            }
                                            else
                                            {
                                                string strNextIndex = NextOZList[NextOZList.Count() - 1];
                                                int iNextOzIndex = 0;
                                                if (int.TryParse(strNextIndex, out iNextOzIndex))
                                                {
                                                    if (iNextOzIndex != iIndex + 1)
                                                    {
                                                        if (iIndex < 9)
                                                            strNewOZ = ISelectedvalue.ToString() + "." + (iIndex + 1).ToString();
                                                        else
                                                            throw new Exception("Gemäß LV Sprung kann die Position nicht an den Anfang des Positionsbereichs kopiert werden");
                                                    }
                                                    else
                                                    {
                                                        throw new Exception("Gemäß LV Sprung kann die Position nicht an den Anfang des Positionsbereichs kopiert werden");
                                                    }
                                                }
                                                else
                                                {
                                                    if (iIndex < 9)
                                                        strNewOZ = ISelectedvalue.ToString() + "." + (iIndex + 1).ToString();
                                                    else
                                                        throw new Exception("Gemäß LV Sprung kann die Position nicht an den Anfang des Positionsbereichs kopiert werden");
                                                }
                                            }
                                        }
                                        else if (INewValue < INextLVValue)
                                            strNewOZ = INewValue.ToString() + ".";
                                        else
                                            throw new Exception("Gemäß LV Sprung kann die Position nicht an den Anfang des Positionsbereichs kopiert werden");
                                    }
                                }
                                else
                                {
                                    strNewOZ = iTemp.ToString();
                                }
                            }
                            else if (Regex.IsMatch(SelectedOnheStufe, "[a-zA-Z]"))
                                strNewOZ = string.Empty;
                            else
                                throw new Exception("Bitte wählen Sie einen anderen Kopiermodus");
                        }
                        else
                            throw new Exception("Bitte wählen Sie einen anderen Kopiermodus");
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return strNewOZ;
        }

        /// <summary>
        /// Code to fetch project number list from database and bind to combo box
        /// </summary>
        private void FillProjectNumber()
        {
            try
            {
                ObjBProject.GetProjectNumber(ObjEProject);
                if (ObjEProject.dtProjecNumber != null)
                {
                    lookUpEditOldProject.Properties.DataSource = null;
                    lookUpEditOldProject.Properties.DataSource = ObjEProject.dtProjecNumber;
                    lookUpEditOldProject.Properties.DisplayMember = "ProjectNumber";
                    lookUpEditOldProject.Properties.ValueMember = "ProjectID";

                    label25.Text = "New Project : " + ObjEProject.ProjectNumber;
                    ObjBPosition.GetPositionList(ObjEPosition, Convert.ToInt32(ObjEProject.ProjectID));
                    if (ObjEPosition.dsPositionList != null && ObjEPosition.dsPositionList.Tables.Count > 0)
                    {
                        tlNewProject.DataSource = ObjEPosition.dsPositionList.Tables[0];
                        tlNewProject.ParentFieldName = "Parent_OZ";
                        tlNewProject.KeyFieldName = "PositionID";
                        tlNewProject.ForceInitialize();
                        tlNewProject.ExpandAll();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #endregion

        #region Reports

        #region Invoice
        private void gvInvoices_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            try
            {
                if (e.HitInfo.InRow)
                {
                    e.Menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("Rechnung - Variante Langtext", gcInvoicesLangText_Click));
                    e.Menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("Rechnung - Variante Kurztext", gcInvoicesKurzText_Click));

                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gcInvoicesLangText_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvInvoices.FocusedRowHandle != null && gvInvoices.GetFocusedRowCellValue("InvoiceID") != null)
                {
                    int IValue = 0;
                    string strInvoiceID = gvInvoices.GetFocusedRowCellValue("InvoiceID").ToString();
                    if (int.TryParse(strInvoiceID, out IValue))
                    {
                        rptInvoicewithLangText Obj = new rptInvoicewithLangText();
                        ReportPrintTool printTool = new ReportPrintTool(Obj);
                        Obj.Parameters["InvoiceID"].Value = IValue;
                        Obj.Parameters["ProjectID"].Value = ObjEProject.ProjectID;
                        printTool.ShowRibbonPreview();
                    }
                }

            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gcInvoicesKurzText_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvInvoices.FocusedRowHandle != null && gvInvoices.GetFocusedRowCellValue("InvoiceID") != null)
                {
                    int IValue = 0;
                    string strInvoiceID = gvInvoices.GetFocusedRowCellValue("InvoiceID").ToString();
                    if (int.TryParse(strInvoiceID, out IValue))
                    {
                        rptInvoicewithKurzText Obj = new rptInvoicewithKurzText();
                        ReportPrintTool printTool = new ReportPrintTool(Obj);
                        Obj.Parameters["InvoiceID"].Value = IValue;
                        Obj.Parameters["ProjectID"].Value = ObjEProject.ProjectID;
                        printTool.ShowRibbonPreview();
                    }
                }

            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        #endregion

        #region Delivery

        private void gcDeliveryReportAddress_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvDeliveryNoteReport.FocusedRowHandle != null && gvDeliveryNoteReport.GetFocusedRowCellValue("BlattID") != null)
                {
                    int IValue = 0;
                    string strBlattID = gvDeliveryNoteReport.GetFocusedRowCellValue("BlattID").ToString();
                    if (int.TryParse(strBlattID, out IValue))
                    {
                        rptDeliveryNotes Obj = new rptDeliveryNotes();
                        ReportPrintTool printTool = new ReportPrintTool(Obj);
                        Obj.Parameters["ID"].Value = IValue;
                        Obj.Parameters["ProjectID"].Value = ObjEProject.ProjectID;
                        printTool.ShowRibbonPreview();

                    }
                }

            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gcDeliveryReportWithouAddress_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvDeliveryNoteReport.FocusedRowHandle != null && gvDeliveryNoteReport.GetFocusedRowCellValue("BlattID") != null)
                {
                    int IValue = 0;
                    string strBlattID = gvDeliveryNoteReport.GetFocusedRowCellValue("BlattID").ToString();

                    if (int.TryParse(strBlattID, out IValue))
                    {
                        rptDeliveryNotesWithoutAddress Obj = new rptDeliveryNotesWithoutAddress();
                        ReportPrintTool printTool = new ReportPrintTool(Obj);
                        Obj.Parameters["BlattID"].Value = IValue;
                        Obj.Parameters["ProjectID"].Value = ObjEProject.ProjectID;

                        printTool.ShowRibbonPreview();
                    }
                }

            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gvDeliveryNoteReport_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            try
            {
                if (e.HitInfo.InRow)
                {
                    e.Menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("Aufmaß mit Adresskopf", gcDeliveryReportAddress_Click));
                    e.Menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("Aufmaß ohne Adresskopf, mit LV Positionsnr", gcDeliveryReportWithouAddress_Click));
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        #endregion

        #region Form Blatt

        private void btnFomBlatt_221_Click(object sender, EventArgs e)
        {
            try
            {
                if (ObjEFormBlatt == null)
                    ObjEFormBlatt = new EFormBlatt();
                if (ObjBFormBlatt == null)
                    ObjBFormBlatt = new BFormBlatt();
                ObjEFormBlatt.ProjectID = ObjEProject.ProjectID;
                ObjEFormBlatt = ObjBFormBlatt.GetFormBlattMapping(ObjEFormBlatt);
                decimal Value = 0;
                if (Utility.ValidateRequiredFields(RequiredFieldsFormBlatt) == false)
                    return;
                DataTable table = new DataTable();
                table = gc221_2.DataSource as DataTable;

                rptFormBlatt_221_1 rpt = new rptFormBlatt_221_1(ObjEProject.ProjectID, table);
                rpt.Name = "FormBlatt221";
                ReportPrintTool printTool = new ReportPrintTool(rpt);
                rpt.Parameters["ProjectID"].Value = ObjEProject.ProjectID;
                if (decimal.TryParse(txtAmount.Text, out Value))
                {
                    rpt.Parameters["Amount"].Value = Value;
                }
                if (decimal.TryParse(txtsurcharge_1_2.Text, out Value))
                {
                    rpt.Parameters["SurchargePer"].Value = Value;
                }
                if (decimal.TryParse(txtSurcharge_1_3.Text, out Value))
                {
                    rpt.Parameters["Holding"].Value = Value;
                }

                rpt.Parameters["Description"].Value = richtxtformBlatt_221.Text;
                printTool.ShowRibbonPreview();
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void bgv221_2_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                BandedGridView view = sender as BandedGridView;
                view.UpdateTotalSummary();
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnFomBlatt_223_Click(object sender, EventArgs e)
        {
            try
            {
                if (ObjEFormBlatt == null)
                    ObjEFormBlatt = new EFormBlatt();
                if (ObjBFormBlatt == null)
                    ObjBFormBlatt = new BFormBlatt();
                ObjEFormBlatt.ProjectID = ObjEProject.ProjectID;
                ObjEFormBlatt = ObjBFormBlatt.GetFormBlattMapping(ObjEFormBlatt);

                rptFormBlatt_223 rpt = new rptFormBlatt_223();
                rpt.Name = "FormBlatt223";
                ReportPrintTool printTool = new ReportPrintTool(rpt);
                rpt.Parameters["ProjectID"].Value = ObjEProject.ProjectID;
                printTool.ShowRibbonPreview();
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        #endregion

        #region "Discount"

        private void btnAddDisc_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (ObjEProject.ProjectID <= 0)
                    return;
                frmAddDiscount Obj = new frmAddDiscount(ObjEProject);
                Obj.ShowDialog();
                if (ObjEProject.IsSave)
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    SplashScreenManager.Default.SetWaitFormDescription("Nachlass wird kalkuliert ... ");
                    tlTemp = new TreeListNode();
                    ObjEProject.IsSave = false;
                    if (ObjBProject == null)
                        ObjBProject = new BProject();
                    BindPositionData();
                    ObjEProject.FromOZ = Utility.PrepareOZ(ObjEProject.FromOZ, ObjEProject.LVRaster);
                    ObjEProject.ToOZ = Utility.PrepareOZ(ObjEProject.ToOZ, ObjEProject.LVRaster);
                    ObjEProject.UserID = Utility.UserID;

                    TreeListNode FromNode = tlPositions.FindNode((node) =>
                    {
                        return node["Position_OZ"].ToString() == ObjEProject.FromOZ;
                    });
                    TreeListNode ToNode = tlPositions.FindNode((node) =>
                    {
                        return node["Position_OZ"].ToString() == ObjEProject.ToOZ;
                    });
                    if (FromNode != null && ToNode != null)
                    {
                        if (FromNode.ParentNode == ToNode.ParentNode)
                        {
                            ObjEProject.dtDiscountList = new DataTable();
                            ObjEProject.dtDiscountList.Columns.Add("SNO", typeof(int));
                            ObjEProject.dtDiscountList.Columns.Add("Surcharge_From", typeof(string));
                            ObjEProject.dtDiscountList.Columns.Add("Surcharge_To", typeof(string));
                            ObjEProject.dtDiscountList.Columns.Add("Parent_OZ", typeof(int));
                            ObjEProject.dtDiscountList.Columns.Add("Position_OZ", typeof(string));
                            ObjEProject.dtDiscountList.Columns.Add("PositionKZ", typeof(string));
                            ObjEProject.dtDiscountList.Columns.Add("DetailKZ", typeof(int));
                            ObjEProject.dtDiscountList.Columns.Add("LVSection", typeof(string));
                            ObjEProject.dtDiscountList.Columns.Add("LVStatus", typeof(string));
                            ObjEProject.dtDiscountList.Columns.Add("UserID", typeof(int));
                            ObjEProject.dtDiscountList.Columns.Add("Discount", typeof(decimal));
                            ObjEProject.dtDiscountList.Columns.Add("ME", typeof(string));
                            ObjEProject.dtDiscountList.Columns.Add("OZID", typeof(decimal));
                            ObjEProject.dtDiscountList.Columns.Add("OZ1", typeof(string));
                            ObjEProject.dtDiscountList.Columns.Add("OZ2", typeof(string));
                            ObjEProject.dtDiscountList.Columns.Add("OZ3", typeof(string));
                            ObjEProject.dtDiscountList.Columns.Add("OZ4", typeof(string));
                            ObjEProject.dtDiscountList.Columns.Add("OZ5", typeof(string));
                            ObjEProject.dtDiscountList.Columns.Add("OZ6", typeof(string));

                            if (FromNode.ParentNode == null)
                            {
                                int FromNodeIndex = tlPositions.GetNodeIndex(FromNode);
                                int ToNodeIndex = tlPositions.GetNodeIndex(ToNode);
                                foreach (TreeListNode node in tlPositions.Nodes)
                                {
                                    int CurrentIndex = tlPositions.GetNodeIndex(node);
                                    if (node != null && CurrentIndex >= FromNodeIndex && CurrentIndex <= ToNodeIndex)
                                    {
                                        tlTemp = null;
                                        ObjEProject = CollectTitlesForDiscount(node, ObjEProject);
                                    }
                                }
                            }
                            else
                            {
                                int FromNodeIndex = tlPositions.GetNodeIndex(FromNode);
                                int ToNodeIndex = tlPositions.GetNodeIndex(ToNode);
                                foreach (TreeListNode node in FromNode.ParentNode.Nodes)
                                {
                                    int CurrentIndex = tlPositions.GetNodeIndex(node);
                                    if (node != null && CurrentIndex >= FromNodeIndex && CurrentIndex <= ToNodeIndex)
                                    {
                                        tlTemp = null;
                                        ObjEProject = CollectTitlesForDiscount(node, ObjEProject);
                                    }
                                }
                            }
                            if (ObjEProject.dtDiscountList.Rows.Count > 0)
                            {
                                ObjEProject = ObjBProject.SaveDiscount(ObjEProject);
                                gcDiscount.DataSource = ObjEProject.dtDiscount;
                                Utility.Setfocus(gvDiscount, "DiscountID", ObjEProject.DiscountID);
                            }
                        }
                        else
                            throw new Exception("Cannot Add Discount for Given From and To Titles");
                    }
                    else
                        throw new Exception("Von oder nach OZ existiert nicht..!!");
                    SplashScreenManager.CloseForm(false);
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Utility.ShowError(ex);
            }
        }

        private void btnDeleteDisc_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (gvDiscount.FocusedRowHandle >= 0)
                {
                    int IValue = 0;
                    string strTemp = Convert.ToString(gvDiscount.GetFocusedRowCellValue("DiscountID"));
                    if (int.TryParse(strTemp, out IValue))
                    {

                        if (ObjEProject == null)
                            ObjEProject = new EProject();
                        if (ObjBProject == null)
                            ObjBProject = new BProject();
                        ObjEProject.DiscountID = IValue;
                        ObjEProject = ObjBProject.DeleteDiscount(ObjEProject);
                        gvDiscount.DeleteRow(gvDiscount.FocusedRowHandle);
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        TreeListNode tlTemp = null;

        /// <summary>
        /// Code to find the cumulated titles in selected titles range
        /// </summary>
        /// <param name="node"></param>
        /// <param name="ObjEProject"></param>
        /// <returns></returns>
        private EProject CollectTitlesForDiscount(TreeListNode node, EProject ObjEProject)
        {
            try
            {
                if (node.HasChildren)
                {
                    foreach (TreeListNode tn in node.Nodes)
                        ObjEProject = CollectTitlesForDiscount(tn, ObjEProject);
                }
                else
                {
                    TreeListNode ParentNode = node.ParentNode;
                    if (tlTemp != ParentNode)
                    {
                        tlTemp = node.ParentNode;
                        TreeListNode ToNode = ParentNode.LastNode;
                        if (ToNode.ParentNode != null)
                        {
                            int iValue = 0;
                            int ParentID = 0;
                            string strTempParentOZ = Convert.ToString(ParentNode["Position_OZ"]);

                            string newOZ = string.Empty;
                            string str = string.Empty;
                            string strRaster = ObjEProject.LVRaster;
                            string[] strPOZ = Convert.ToString(ToNode["Position_OZ"]).Split('.');
                            string[] strPRaster = strRaster.Split('.');
                            int Count = -1;
                            int i = -1;
                            Count = strPOZ.Count();
                            while (Count > 0)
                            {
                                i = i + 1;
                                Count = Count - 1;
                                int OZLength = 0;
                                int RasterLength = 0;
                                string OZ = string.Empty;
                                OZ = strPOZ[i].Trim();
                                RasterLength = strPRaster[i].Length;
                                OZLength = OZ.Trim().Length;
                                if (Count > 0)
                                {
                                    if (OZ == "")
                                    {
                                        str = str + string.Concat(Enumerable.Repeat(" ", RasterLength - OZLength)) + OZ + ".";
                                    }
                                }
                            }
                            string stNewOZ = GetDiscountPositionIZ(ObjEProject.LVRaster);
                            if (str.Length > 0)
                            {
                                string strBlankNewOz = strTempParentOZ + str + stNewOZ;
                                newOZ = strBlankNewOz;
                            }
                            else
                            {
                                string strNewOz = strTempParentOZ + stNewOZ;
                                newOZ = strNewOz;
                            }

                            DataRow drnew = ObjEProject.dtDiscountList.NewRow();

                            if (int.TryParse(Convert.ToString(node["Parent_OZ"]), out ParentID))
                                drnew["Parent_OZ"] = ParentID;

                            if (int.TryParse(Convert.ToString(ToNode["SNO"]), out iValue))
                                drnew["SNO"] = iValue;
                            else
                                drnew["SNO"] = -1;

                            drnew["Surcharge_From"] = strTempParentOZ;
                            drnew["Surcharge_To"] = strTempParentOZ;
                            drnew["Position_OZ"] = Utility.PrepareOZ(newOZ, ObjEProject.LVRaster);
                            drnew["PositionKZ"] = "ZZ";
                            drnew["DetailKZ"] = 0;
                            drnew["LVSection"] = "HA";
                            drnew["LVStatus"] = "B";
                            drnew["UserID"] = Utility.UserID;
                            drnew["Discount"] = ObjEProject.Discount;
                            drnew["ME"] = "%";
                            decimal dValue = 0;
                            if (!decimal.TryParse(stNewOZ, out dValue))
                                dValue = 0;
                            drnew["OZID"] = dValue;

                            string OZ1 = string.Empty, OZ2 = string.Empty, OZ3 = string.Empty, OZ4 = string.Empty, OZ5 = string.Empty, OZ6 = string.Empty;
                            if (!string.IsNullOrEmpty(newOZ))
                            {
                                string[] strOZList = Utility.PrepareOZ(newOZ, ObjEProject.LVRaster).Split('.');
                                if (strOZList.Count() > 1)
                                {
                                    string strOZID = strOZList[strOZList.Count() - 2];
                                    string strIndex = strOZList[strOZList.Count() - 1];
                                    char[] OZcharList = strOZID.ToCharArray();
                                    int CharCount = OZcharList.Count();
                                    if (CharCount > 0)
                                    {
                                        OZ1 = Convert.ToString(OZcharList[0]);
                                        if (CharCount > 1)
                                        {
                                            OZ2 = Convert.ToString(OZcharList[1]);
                                            if (CharCount > 2)
                                            {
                                                OZ3 = Convert.ToString(OZcharList[2]);
                                                if (CharCount > 3)
                                                {
                                                    OZ4 = Convert.ToString(OZcharList[3]);
                                                    if (CharCount > 4)
                                                    {
                                                        OZ5 = Convert.ToString(OZcharList[4]);
                                                        OZ6 = strIndex;
                                                    }
                                                    else
                                                        OZ5 = strIndex;
                                                }
                                                else
                                                    OZ4 = strIndex;
                                            }
                                            else
                                                OZ3 = strIndex;
                                        }
                                        else
                                            OZ2 = strIndex;
                                    }
                                }
                            }
                            drnew["OZ1"] = OZ1;
                            drnew["OZ2"] = OZ2;
                            drnew["OZ3"] = OZ3;
                            drnew["OZ4"] = OZ4;
                            drnew["OZ5"] = OZ5;
                            drnew["OZ6"] = OZ6;
                            ObjEProject.dtDiscountList.Rows.Add(drnew);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEProject;
        }

        /// <summary>
        /// Code to find new OZ while adding discount position
        /// </summary>
        /// <param name="stRaster"></param>
        /// <returns></returns>
        private string GetDiscountPositionIZ(string stRaster)
        {
            string stOZ = string.Empty;
            try
            {
                string[] stOZlist = stRaster.Split('.');
                if (stOZlist.Count() > 0)
                {
                    string Onhestufe = stOZlist[stOZlist.Count() - 2];
                    stOZ = Onhestufe.Replace('1', 'Z') + ".Z";
                }

            }
            catch (Exception ex) { }
            return stOZ;
        }

        #endregion

        #region "Title Blatt"

        private void btnTitleBlattV011_ItemClick(object sender, ItemClickEventArgs e)
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
            SplashScreenManager.Default.SetWaitFormDescription("Bitte warten…");
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            try
            {
                if (ObjEProject == null)
                    ObjEProject = new EProject();
                if (ObjBProject == null)
                    ObjBProject = new BProject();

                ObjEProject = ObjBProject.GetPath(ObjEProject);
                if (string.IsNullOrEmpty(ObjEProject.CoverSheetPath))
                    throw new Exception("Der angegeben Ordnerpfad zum Titelblatt existiert nicht (mehr)");
                if (!Directory.Exists(ObjEProject.CoverSheetPath))
                    throw new Exception("Kein gültiger Ordnerpfad zum Titelblatt");
                string strFileName = ObjEProject.CoverSheetPath + "\\" + "V-011-" + ObjEProject.ProjectNumber + ".Docx";
                if (!File.Exists(strFileName))
                {
                    var FolderPath = new DirectoryInfo(ObjEProject.TemplatePath).GetFiles("V-011*.dotx", SearchOption.AllDirectories).OrderByDescending(d => d.LastWriteTimeUtc).First();
                    Object oMissing = System.Reflection.Missing.Value;
                    Object appPath = Application.UserAppDataPath + "\\" + FolderPath;
                    Object oTemplatePath = ObjEProject.TemplatePath + "\\" + FolderPath;
                    System.IO.File.Copy(Convert.ToString(oTemplatePath), Convert.ToString(appPath), true);
                    Microsoft.Office.Interop.Word.Document wordDoc = new Microsoft.Office.Interop.Word.Document();
                    wordDoc = wordApp.Documents.Add(ref appPath, ref oMissing, ref oMissing, ref oMissing);
                    UdpateMergefields(wordDoc, wordApp, ObjEProject, string.Empty, string.Empty, DateTime.Now);
                    wordDoc.SaveAs(strFileName);
                    wordDoc.Close();
                    Thread.Sleep(1000);
                }
                if (!Utility.fileIsOpen(strFileName))
                {
                    Microsoft.Office.Interop.Word.Application ap = new Microsoft.Office.Interop.Word.Application();
                    ap.Documents.Open(strFileName);
                    ap.Visible = true;
                    ap.Activate();
                }
                else
                    throw new Exception("Das Angebotdokument für die '" + ObjEProject.ProjectNumber + "' ist bereits geöffnet");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Sequence contains no elements"))
                    XtraMessageBox.Show("Die erforderliche Dokumentenvorlage ist nicht eingestellt!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
                wordApp.Quit();
            }
        }

        private void btnTitleBlattV012_ItemClick(object sender, ItemClickEventArgs e)
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
            SplashScreenManager.Default.SetWaitFormDescription("Bitte warten…");
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            try
            {
                if (ObjEProject == null)
                    ObjEProject = new EProject();
                if (ObjBProject == null)
                    ObjBProject = new BProject();

                ObjEProject = ObjBProject.GetPath(ObjEProject);
                if (string.IsNullOrEmpty(ObjEProject.CoverSheetPath))
                    throw new Exception("Der angegeben Ordnerpfad zum Titelblatt existiert nicht (mehr)");
                if (!Directory.Exists(ObjEProject.CoverSheetPath))
                    throw new Exception("Kein gültiger Ordnerpfad zum Titelblatt");
                string strFileName = ObjEProject.CoverSheetPath + "\\" + "V-012-" + ObjEProject.ProjectNumber + ".Docx";
                if (!File.Exists(strFileName))
                {
                    var FolderPath = new DirectoryInfo(ObjEProject.TemplatePath).GetFiles("V-012*.dotx", SearchOption.AllDirectories).OrderByDescending(d => d.LastWriteTimeUtc).First();
                    Object oMissing = System.Reflection.Missing.Value;
                    Object appPath = Application.UserAppDataPath + "\\" + FolderPath;
                    Object oTemplatePath = ObjEProject.TemplatePath + "\\" + FolderPath;
                    System.IO.File.Copy(Convert.ToString(oTemplatePath), Convert.ToString(appPath), true);
                    Microsoft.Office.Interop.Word.Document wordDoc = new Microsoft.Office.Interop.Word.Document();
                    wordDoc = wordApp.Documents.Add(ref appPath, ref oMissing, ref oMissing, ref oMissing);
                    UdpateMergefields(wordDoc, wordApp, ObjEProject, string.Empty, string.Empty, DateTime.Now);
                    wordDoc.SaveAs(strFileName);
                    wordDoc.Close();
                    Thread.Sleep(1000);
                }
                if (!Utility.fileIsOpen(strFileName))
                {
                    Microsoft.Office.Interop.Word.Application ap = new Microsoft.Office.Interop.Word.Application();
                    ap.Documents.Open(strFileName);
                    ap.Visible = true;
                    ap.Activate();
                }
                else
                    throw new Exception("Das Angebotdokument für die '" + ObjEProject.ProjectNumber + "' ist bereits geöffnet");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Sequence contains no elements"))
                    XtraMessageBox.Show("Die erforderliche Dokumentenvorlage ist nicht eingestellt!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { SplashScreenManager.CloseForm(false); wordApp.Quit(); }
        }

        private void btnTitleBlattV013_ItemClick(object sender, ItemClickEventArgs e)
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
            SplashScreenManager.Default.SetWaitFormDescription("Bitte warten…");
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document wordDoc = new Microsoft.Office.Interop.Word.Document();
            try
            {
                if (ObjEProject == null)
                    ObjEProject = new EProject();
                if (ObjBProject == null)
                    ObjBProject = new BProject();

                ObjEProject = ObjBProject.GetPath(ObjEProject);
                if (string.IsNullOrEmpty(ObjEProject.CoverSheetPath))
                    throw new Exception("Der angegeben Ordnerpfad zum Titelblatt existiert nicht (mehr)");
                if (!Directory.Exists(ObjEProject.CoverSheetPath))
                    throw new Exception("Kein gültiger Ordnerpfad zum Titelblatt");
                string strFileName = ObjEProject.CoverSheetPath + "\\" + "V-013-" + ObjEProject.ProjectNumber + ".Docx";
                if (!File.Exists(strFileName))
                {
                    var FolderPath = new DirectoryInfo(ObjEProject.TemplatePath).GetFiles("V-013*.dotx", SearchOption.AllDirectories).OrderByDescending(d => d.LastWriteTimeUtc).First();
                    Object oMissing = System.Reflection.Missing.Value;
                    Object appPath = Application.UserAppDataPath + "\\" + FolderPath;
                    Object oTemplatePath = ObjEProject.TemplatePath + "\\" + FolderPath;
                    System.IO.File.Copy(Convert.ToString(oTemplatePath), Convert.ToString(appPath), true);
                    wordDoc = wordApp.Documents.Add(ref appPath, ref oMissing, ref oMissing, ref oMissing);
                    UdpateMergefields(wordDoc, wordApp, ObjEProject, string.Empty, string.Empty, DateTime.Now);
                    wordDoc.SaveAs(strFileName);
                    wordDoc.Close();
                    Thread.Sleep(1000);
                }
                if (!Utility.fileIsOpen(strFileName))
                {
                    Microsoft.Office.Interop.Word.Application ap = new Microsoft.Office.Interop.Word.Application();
                    ap.Documents.Open(strFileName);
                    ap.Visible = true;
                    ap.Activate();
                }
                else
                    throw new Exception("Das Angebotdokument für die '" + ObjEProject.ProjectNumber + "' ist bereits geöffnet");
            }
            catch (Exception ex)
            {

                if (wordDoc != null)
                    wordDoc.Close();
                if (wordApp != null)
                    wordApp.Quit();

                if (ex.Message.Contains("Sequence contains no elements"))
                    XtraMessageBox.Show("Die erforderliche Dokumentenvorlage ist nicht eingestellt!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    Utility.ShowError(ex);
            }
            finally { SplashScreenManager.CloseForm(false); }
        }

        private void btnTitleBlattV014_ItemClick(object sender, ItemClickEventArgs e)
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
            SplashScreenManager.Default.SetWaitFormDescription("Bitte warten…");
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            try
            {
                if (ObjEProject == null)
                    ObjEProject = new EProject();
                if (ObjBProject == null)
                    ObjBProject = new BProject();

                ObjEProject = ObjBProject.GetPath(ObjEProject);
                if (string.IsNullOrEmpty(ObjEProject.CoverSheetPath))
                    throw new Exception("Der angegeben Ordnerpfad zum Titelblatt existiert nicht (mehr)");
                if (!Directory.Exists(ObjEProject.CoverSheetPath))
                    throw new Exception("Kein gültiger Ordnerpfad zum Titelblatt");
                string strFileName = ObjEProject.CoverSheetPath + "\\" + "V-014-" + ObjEProject.ProjectNumber + ".Docx";
                if (!File.Exists(strFileName))
                {
                    var FolderPath = new DirectoryInfo(ObjEProject.TemplatePath).GetFiles("V-014*.dotx", SearchOption.AllDirectories).OrderByDescending(d => d.LastWriteTimeUtc).First();
                    Object oMissing = System.Reflection.Missing.Value;
                    Object appPath = Application.UserAppDataPath + "\\" + FolderPath;
                    Object oTemplatePath = ObjEProject.TemplatePath + "\\" + FolderPath;
                    System.IO.File.Copy(Convert.ToString(oTemplatePath), Convert.ToString(appPath), true);
                    Microsoft.Office.Interop.Word.Document wordDoc = new Microsoft.Office.Interop.Word.Document();
                    wordDoc = wordApp.Documents.Add(ref appPath, ref oMissing, ref oMissing, ref oMissing);
                    UdpateMergefields(wordDoc, wordApp, ObjEProject, string.Empty, string.Empty, DateTime.Now);
                    wordDoc.SaveAs(strFileName);
                    wordDoc.Close();
                    Thread.Sleep(1000);
                }
                if (!Utility.fileIsOpen(strFileName))
                {
                    Microsoft.Office.Interop.Word.Application ap = new Microsoft.Office.Interop.Word.Application();
                    ap.Documents.Open(strFileName);
                    ap.Visible = true;
                    ap.Activate();
                }
                else
                    throw new Exception("Das Angebotdokument für die '" + ObjEProject.ProjectNumber + "' ist bereits geöffnet");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Sequence contains no elements"))
                    XtraMessageBox.Show("Die erforderliche Dokumentenvorlage ist nicht eingestellt!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { SplashScreenManager.CloseForm(false); wordApp.Quit(); }
        }

        private void btnTitleBlattDelivery_ItemClick(object sender, ItemClickEventArgs e)
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
            SplashScreenManager.Default.SetWaitFormDescription("Bitte warten…");
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            try
            {
                if (ObjEProject == null)
                    ObjEProject = new EProject();
                if (ObjBProject == null)
                    ObjBProject = new BProject();

                ObjEProject = ObjBProject.GetPath(ObjEProject);
                if (string.IsNullOrEmpty(ObjEProject.CoverSheetPath))
                    throw new Exception("Der angegeben Ordnerpfad zum Titelblatt existiert nicht (mehr)");

                if (!Directory.Exists(ObjEProject.CoverSheetPath))
                    throw new Exception("Kein gültiger Ordnerpfad zum Titelblatt");

                string strFileName = ObjEProject.CoverSheetPath + "\\" + ObjEProject.ProjectNumber + "_Aufmass.Docx";
                if (!File.Exists(strFileName))
                {
                    Object oMissing = System.Reflection.Missing.Value;
                    Object appPath = Application.UserAppDataPath + "\\Aufmass_Template.dotx";
                    Object oTemplatePath = ObjEProject.TemplatePath + "\\Aufmass_Template.dotx";
                    System.IO.File.Copy(Convert.ToString(oTemplatePath), Convert.ToString(appPath), true);
                    Microsoft.Office.Interop.Word.Document wordDoc = new Microsoft.Office.Interop.Word.Document();
                    wordDoc = wordApp.Documents.Add(ref appPath, ref oMissing, ref oMissing, ref oMissing);
                    UdpateMergefields(wordDoc, wordApp, ObjEProject, string.Empty, string.Empty, DateTime.Now);
                    wordDoc.SaveAs(strFileName);
                    wordDoc.Close();
                    Thread.Sleep(1000);
                }
                if (!Utility.fileIsOpen(strFileName))
                {
                    Microsoft.Office.Interop.Word.Application ap = new Microsoft.Office.Interop.Word.Application();
                    ap.Documents.Open(strFileName);
                    ap.Visible = true;
                    ap.Activate();
                }
                else
                    throw new Exception("Das Aufmassdokument für die '" + ObjEProject.ProjectNumber + "' ist bereits geöffnet");
            }
            catch (Exception ex) { }
            finally
            {
                SplashScreenManager.CloseForm(false);
                wordApp.Quit();
            }
        }

        private void btnTitleBlattInvoice_ItemClick(object sender, ItemClickEventArgs e)
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
            SplashScreenManager.Default.SetWaitFormDescription("Bitte warten…");
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            try
            {
                if (ObjEProject == null)
                    ObjEProject = new EProject();
                if (ObjBProject == null)
                    ObjBProject = new BProject();

                ObjEProject = ObjBProject.GetPath(ObjEProject);
                if (string.IsNullOrEmpty(ObjEProject.CoverSheetPath))
                    throw new Exception("Der angegeben Ordnerpfad zum Titelblatt existiert nicht (mehr)");
                if (!Directory.Exists(ObjEProject.CoverSheetPath))
                    throw new Exception("Kein gültiger Ordnerpfad zum Titelblatt");
                string strFileName = ObjEProject.CoverSheetPath + "\\" + ObjEProject.ProjectNumber + "_Rechnung.Docx";
                if (!File.Exists(strFileName))
                {
                    Object oMissing = System.Reflection.Missing.Value;
                    Object appPath = Application.UserAppDataPath + "\\Rechnung_Template.dotx";
                    Object oTemplatePath = ObjEProject.TemplatePath + "\\Rechnung_Template.dotx";
                    System.IO.File.Copy(Convert.ToString(oTemplatePath), Convert.ToString(appPath), true);
                    Microsoft.Office.Interop.Word.Document wordDoc = new Microsoft.Office.Interop.Word.Document();
                    wordDoc = wordApp.Documents.Add(ref appPath, ref oMissing, ref oMissing, ref oMissing);
                    UdpateMergefields(wordDoc, wordApp, ObjEProject, string.Empty, string.Empty, DateTime.Now);
                    wordDoc.SaveAs(strFileName);
                    wordDoc.Close();
                    Thread.Sleep(1000);
                }
                if (!Utility.fileIsOpen(strFileName))
                {
                    Microsoft.Office.Interop.Word.Application ap = new Microsoft.Office.Interop.Word.Application();
                    ap.Documents.Open(strFileName);
                    ap.Visible = true;
                    ap.Activate();
                }
                else
                    throw new Exception("Das Angebotsdokument für die '" + ObjEProject.ProjectNumber + "' ist bereits geöffnet");
            }
            catch (Exception ex) { }
            finally
            {
                SplashScreenManager.CloseForm(false);
                wordApp.Quit();
            }
        }

        private void UdpateMergefields(Microsoft.Office.Interop.Word.Document wordDoc,
            Microsoft.Office.Interop.Word.Application wordApp, EProject eproject, string SupplierName,
            string SupplierAddress, DateTime dtDeliveryDeadLine)
        {
            try
            {
                foreach (Microsoft.Office.Interop.Word.Shape myShape in wordDoc.Shapes)
                {
                    if (myShape.TextFrame.HasText != 0)
                    {
                        foreach (Microsoft.Office.Interop.Word.Field field in myShape.TextFrame.ContainingRange.Fields)
                            ReplaceMergeFieldValues(field, wordApp, ObjEProject, SupplierName, SupplierAddress, dtDeliveryDeadLine);
                    }
                }

                foreach (Microsoft.Office.Interop.Word.Field myMergeField in wordDoc.Fields)
                    ReplaceMergeFieldValues(myMergeField, wordApp, ObjEProject, SupplierName, SupplierAddress, dtDeliveryDeadLine);
            }
            catch (Exception ex) { throw ex; }
        }

        private void ReplaceMergeFieldValues(Microsoft.Office.Interop.Word.Field myMergeField,
            Microsoft.Office.Interop.Word.Application wordApp, EProject eproject, string SupplierName,
            string SupplierAddress, DateTime dtDeliveryDeadLine)
        {
            try
            {
                Microsoft.Office.Interop.Word.Range rngFieldCode = myMergeField.Code;
                String fieldText = rngFieldCode.Text;
                if (fieldText.StartsWith(" MERGEFIELD"))
                {
                    Int32 endMerge = fieldText.IndexOf("\\");
                    Int32 fieldNameLength = fieldText.Length - endMerge;
                    String fieldName = fieldText.Substring(11, endMerge - 11);
                    fieldName = fieldName.Trim();

                    switch (fieldName)
                    {
                        case "CustName":
                            myMergeField.Select();
                            wordApp.Selection.TypeText(ObjEProject.KundeName);
                            break;
                        case "CustAddress":
                            myMergeField.Select();
                            wordApp.Selection.TypeText(ObjEProject.KundeAddress);
                            break;
                        case "ProjectNr":
                            myMergeField.Select();
                            wordApp.Selection.TypeText(ObjEProject.ProjectNumber);
                            break;
                        case "KommissionNr":
                            myMergeField.Select();
                            wordApp.Selection.TypeText(ObjEProject.CommissionNumber);
                            break;
                        case "Bauvorhaben":
                            myMergeField.Select();
                            wordApp.Selection.TypeText(ObjEProject.ProjectDescription);
                            break;
                        case "Planner":
                            myMergeField.Select();
                            wordApp.Selection.TypeText(ObjEProject.PlannerName);
                            break;
                        case "LieferantName":
                            myMergeField.Select();
                            wordApp.Selection.TypeText(SupplierName);
                            break;
                        case "LieferantAddress":
                            myMergeField.Select();
                            wordApp.Selection.TypeText(SupplierAddress);
                            break;
                        case "DeliveryDeadline":
                            myMergeField.Select();
                            wordApp.Selection.TypeText(dtDeliveryDeadLine.ToString("dd.MM.yyyy"));
                            break;
                    }
                }
            }
            catch (Exception ex) { throw ex; }
        }

        #endregion

        #region "Report Buttons"
        private void btnAngebotReport_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                frmReportSetting Obj = new frmReportSetting(ObjEProject.ProjectID, ObjEProject.ProjectDescription, ObjEProject.LVRaster);
                Obj.ShowDialog();
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnQuerCalcV1_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                frmQuerKalculation frm = new frmQuerKalculation(ObjEProject.ProjectID, ObjEProject.ProjectDescription);
                frm.stRaster = ObjEProject.LVRaster;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnQuerCalcV2_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                frmQuerKalculation frm = new frmQuerKalculation(ObjEProject.ProjectID, ObjEProject.ProjectDescription, 2);
                frm.stRaster = ObjEProject.LVRaster;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnFormBlatt_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                ObjBProject.GetProjectDetails(ObjEProject);
                if (ObjEProject.ProjectID > 0 && ObjEProject.ActualLvs > 0)
                {
                    ObjTabDetails = tbFormBlatt1;
                    TabChange(ObjTabDetails);
                    RequiredFieldsFormBlatt.Add(txtAmount);
                    RequiredFieldsFormBlatt.Add(txtsurcharge_1_2);
                    RequiredFieldsFormBlatt.Add(txtSurcharge_1_3);
                    if (ObjEFormBlatt == null)
                        ObjEFormBlatt = new EFormBlatt();
                    if (ObjBFormBlatt == null)
                        ObjBFormBlatt = new BFormBlatt();
                    ObjEFormBlatt.ProjectID = ObjEProject.ProjectID;

                    ObjBFormBlatt.Get_tbl221_2(ObjEFormBlatt);
                    if (ObjEFormBlatt.dtBlatt221_2 != null)
                    {
                        gc221_2.DataSource = ObjEFormBlatt.dtBlatt221_2;
                        bgv221_2.BestFitColumns();
                    }
                }
                txtAmount.Text = txtInternX.Text;
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnBreakdownReport_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                rptSampleBreakdown rptMA = new rptSampleBreakdown();
                rptMA.Name = "Einzelaufgliederung Beispiel";
                //rptMA.Name = "Einzelaufgliederung Beispiel_" + ObjEProject.ProjectDescription.Replace("-", "");
                ReportPrintTool printTool = new ReportPrintTool(rptMA);
                rptMA.Parameters["ProjectID"].Value = ObjEProject.ProjectID;
                rptMA.Parameters["InternX"].Value = ObjEProject.InternX;
                rptMA.Parameters["InternS"].Value = ObjEProject.InternS;
                rptMA.PrintingSystem.Document.AutoFitToPagesWidth = 1;
                printTool.ShowRibbonPreview();
            }
            catch (Exception ex) { }
        }

        private void btnConsolidatedBlatt_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                rptConsolidatedBlatt rpt = new rptConsolidatedBlatt();
                ReportPrintTool printTool = new ReportPrintTool(rpt);
                rpt.Parameters["ProjectID"].Value = ObjEProject.ProjectID;
                printTool.ShowRibbonPreview();
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnDeliveryNotes_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                TabChange(tbAufmassReport);
                if (ObjEDeliveryNotes == null)
                    ObjEDeliveryNotes = new EDeliveryNotes();
                if (ObjBDeliveryNotes == null)
                    ObjBDeliveryNotes = new BDeliveryNotes();
                ObjEDeliveryNotes.ProjectID = ObjEProject.ProjectID;
                ObjEDeliveryNotes = ObjBDeliveryNotes.GetBlattNumbers(ObjEDeliveryNotes);
                gcDeliveryNoteReport.DataSource = ObjEDeliveryNotes.dtBlattNumbers;
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }
        #endregion

        #endregion

        private void btnLVNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            btnNew_Click(null, null);
        }

        private void btnLVSave_ItemClick(object sender, ItemClickEventArgs e)
        {
            btnSaveLVDetails_Click(null, null);
        }

        private void btnLVCancel_ItemClick(object sender, ItemClickEventArgs e)
        {
            btnCancel_Click(null, null);
        }

        private void btnLVExport_ItemClick(object sender, ItemClickEventArgs e)
        {
            btnExportExcel_Click(null, null);
        }

        private void btnLVAddLVSec_ItemClick(object sender, ItemClickEventArgs e)
        {
            btnAddPositionKZ_Click(null, null);
        }

        private void btnLVPrevious_ItemClick(object sender, ItemClickEventArgs e)
        {
            btnPrevious_Click(null, null);
        }

        private void btnLVNext_ItemClick(object sender, ItemClickEventArgs e)
        {
            btnNext_Click(null, null);
        }

        private void btnLVLang_ItemClick(object sender, ItemClickEventArgs e)
        {
            btnLongDescription_Click(null, null);
        }

        private void btnCostDocuware_ItemClick(object sender, ItemClickEventArgs e)
        {
            btnDocuwareLink_Click(null, null);
        }

        private void btnCostAddAceess_ItemClick(object sender, ItemClickEventArgs e)
        {
            btnAddAccessories_Click(null, null);
        }

        private void btnCostValidityDate_ItemClick(object sender, ItemClickEventArgs e)
        {
            btnValiditydates_Click(null, null);
        }

        private void btnRefershTabularView_ItemClick(object sender, ItemClickEventArgs e)
        {
            btnRefreshTreeView_Click(null, null);
        }

        private void btnProjRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            RefreshProject();
        }

        private void chkCreateNewBarItem_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToBoolean(chkCreateNew.EditValue) == true)
                    txtLPMO.ReadOnly = false;
                else
                    txtLPMO.ReadOnly = true;
                chkCreateNew_CheckedChanged(null, null);
            }
            catch (Exception ex) { Utility.ShowError(ex); }
        }

        private void chkCostMontageEntryItem_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToBoolean(ChkManualMontageentry.EditValue) == true)
                    txtLPMO.ReadOnly = false;
                else
                    txtLPMO.ReadOnly = true;
            }
            catch (Exception ex) { Utility.ShowError(ex); }
        }

        private void cmbSelectTreelistColumnsItem_EditValueChanged(object sender, EventArgs e)
        {
            bool isFound_MA = false;
            bool isFound_MO = false;
            bool isFound_Einfr = false;
            bool isFound_Seleb = false;
            bool isFound_Verkf = false;
            try
            {
                foreach (var item in Convert.ToString(cmbSelectTreelistColumnsItem.EditValue).Split(','))
                {
                    if (item.Trim() == "MA Multies")
                        isFound_MA = true;
                    if (item.Trim() == "MO Multies")
                        isFound_MO = true;
                    if (item.Trim() == "Einkaufspreis")
                        isFound_Einfr = true;
                    if (item.Trim() == "Selbstkosten")
                        isFound_Seleb = true;
                    if (item.Trim() == "Verkaufspreis")
                        isFound_Verkf = true;
                }
                tlPositions.Columns["MA_Multi1"].VisibleIndex = 11;
                tlPositions.Columns["MA_multi2"].VisibleIndex = 12;
                tlPositions.Columns["MA_multi3"].VisibleIndex = 13;
                tlPositions.Columns["MA_multi4"].VisibleIndex = 14;
                tlPositions.Columns["MA_Multi1"].Visible = isFound_MA;
                tlPositions.Columns["MA_multi2"].Visible = isFound_MA;
                tlPositions.Columns["MA_multi3"].Visible = isFound_MA;
                tlPositions.Columns["MA_multi4"].Visible = isFound_MA;

                tlPositions.Columns["MO_multi1"].VisibleIndex = 15;
                tlPositions.Columns["MO_multi2"].VisibleIndex = 16;
                tlPositions.Columns["MO_multi3"].VisibleIndex = 17;
                tlPositions.Columns["MO_multi4"].VisibleIndex = 18;
                tlPositions.Columns["MO_multi1"].Visible = isFound_MO;
                tlPositions.Columns["MO_multi2"].Visible = isFound_MO;
                tlPositions.Columns["MO_multi3"].Visible = isFound_MO;
                tlPositions.Columns["MO_multi4"].Visible = isFound_MO;

                tlPositions.Columns["MA_einkaufspreis"].VisibleIndex = 19;
                tlPositions.Columns["MO_Einkaufspreis"].VisibleIndex = 20;
                tlPositions.Columns["MA_einkaufspreis"].Visible = isFound_Einfr;
                tlPositions.Columns["MO_Einkaufspreis"].Visible = isFound_Einfr;

                tlPositions.Columns["MA_selbstkostenMulti"].VisibleIndex = 21;
                tlPositions.Columns["MA_selbstkosten"].VisibleIndex = 22;
                tlPositions.Columns["MO_selbstkostenMulti"].VisibleIndex = 23;
                tlPositions.Columns["MO_selbstkosten"].VisibleIndex = 24;
                tlPositions.Columns["MA_selbstkostenMulti"].Visible = isFound_Seleb;
                tlPositions.Columns["MO_selbstkostenMulti"].Visible = isFound_Seleb;
                tlPositions.Columns["MA_selbstkosten"].Visible = isFound_Seleb;
                tlPositions.Columns["MO_selbstkosten"].Visible = isFound_Seleb;

                tlPositions.Columns["MA_verkaufspreis_Multi"].VisibleIndex = 25;
                tlPositions.Columns["MA_verkaufspreis"].VisibleIndex = 26;
                tlPositions.Columns["MO_verkaufspreisMulti"].VisibleIndex = 27;
                tlPositions.Columns["MO_verkaufspreis"].VisibleIndex = 28;
                tlPositions.Columns["MA_verkaufspreis_Multi"].Visible = isFound_Verkf;
                tlPositions.Columns["MO_verkaufspreisMulti"].Visible = isFound_Verkf;
                tlPositions.Columns["MA_verkaufspreis"].Visible = isFound_Verkf;
                tlPositions.Columns["MO_verkaufspreis"].Visible = isFound_Verkf;

                tlPositions.Columns["EP"].VisibleIndex = 29;
                tlPositions.Columns["GB"].VisibleIndex = 30;
                //tlPositions.BestFitColumns();
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnModifyItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            btnModify_Click(null, null);
        }

        private void btnBulkSaveA_ItemClick(object sender, ItemClickEventArgs e)
        {
            btnSaveActionA_Click(null, null);
        }

        private void btnBulkSaveB_ItemClick(object sender, ItemClickEventArgs e)
        {
            btnSavesectionB_Click(null, null);
        }

        private void btnBulkApply_ItemClick(object sender, ItemClickEventArgs e)
        {
            btnApply_Click(null, null);
        }

        private void rpLVDropMode_SelectedIndexChanged(object sender, System.EventArgs e)
        {

        }

        private void btnSelbLoad_ItemClick(object sender, ItemClickEventArgs e)
        {
            btnMulti5LoadArticles_Click(null, null);
        }

        private void btnSelbSubmit_ItemClick(object sender, ItemClickEventArgs e)
        {
            btnMulti5UpdateSelbekosten_Click(null, null);
        }

        private void btnVerkLoad_ItemClick(object sender, ItemClickEventArgs e)
        {
            btnMulti6LoadArticles_Click(null, null);
        }

        private void btnVerkSubmit_ItemClick(object sender, ItemClickEventArgs e)
        {
            btnMulti6UpdateSelbekosten_Click(null, null);
        }

        private void btnUmlExport_ItemClick(object sender, ItemClickEventArgs e)
        {
            btnUmlageExport_Click(null, null);
        }

        private void btnUmlSave_ItemClick(object sender, ItemClickEventArgs e)
        {
            btnUmlageSave_Click(null, null);
        }

        private void btnUmlSubmit_ItemClick(object sender, ItemClickEventArgs e)
        {
            btnAddedCost_Click(null, null);
        }

        private void barButtonItem57_ItemClick(object sender, ItemClickEventArgs e)
        {
            btnSaveSupplierProposal_Click(null, null);
        }

        private void btnSaveUSP_ItemClick(object sender, ItemClickEventArgs e)
        {
            btnSubmit_Click(null, null);
        }

        private void btnExportUSP_ItemClick(object sender, ItemClickEventArgs e)
        {
            btnExportProposalPositions_Click(null, null);
        }

        private void bnSaveTemparary1_ItemClick(object sender, ItemClickEventArgs e)
        {
            btnSaveTemparary_Click(null, null);
        }

        private void rgFilter1_EditValueChanged(object sender, EventArgs e)
        {
            rgFilter_SelectedIndexChanged(null, null);
        }

        private void rgProposalViews_EditValueChanged(object sender, EventArgs e)
        {
            radioGroup1_SelectedIndexChanged(null, null);
        }

        private void ribbonControl1_SelectedPageChanged(object sender, EventArgs e)
        {
            try
            {
                if (ribbonControl1.SelectedPage == rpReports)
                    return;
                if (ribbonControl1.SelectedPage == rpGAEB)
                    return;
                foreach (DevExpress.XtraBars.Ribbon.RibbonPage Rb in this.Ribbon.Pages)
                {
                    if (Rb != ribbonControl1.SelectedPage)
                    {
                        if (Rb != rpReports)
                        {
                            if (Rb != rpGAEB)
                                Rb.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }

        private void cmbLVSectionProposalItem_EditValueChanged(object sender, EventArgs e)
        {
            cmbLVSectionProposal_Closed(null, null);
        }

        private void btnCopyPosition_ItemClick(object sender, ItemClickEventArgs e)
        {
            CopyPosition();
        }

        private void btnDeletePosition_ItemClick(object sender, ItemClickEventArgs e)
        {
            DeletePosition();
        }

        private void gvAddRemovePositions_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void btnrowdelete_Click(object sender, EventArgs e)
        {
            gridView1.DeleteSelectedRows();
        }

        private void splitContainerControl2_SplitterPositionChanged(object sender, EventArgs e)
        {
            if (splitContainerControl2.SplitterPosition!=0)
            {
                btnAddAccessories.Enabled = false;
                btnValiditydates.Enabled = false;
            }
            else
            {
                btnAddAccessories.Enabled = true;
                btnValiditydates.Enabled = true;
            }
        }

    }
}