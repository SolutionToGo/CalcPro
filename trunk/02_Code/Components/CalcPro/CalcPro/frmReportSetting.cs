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
using EL;
using BL;
using DevExpress.XtraEditors.Controls;
using CalcPro.Report_Design;
using DevExpress.XtraReports.UI;
using CalcPro.Report_Design.dsProposalTableAdapters;
using DAL;
using CalcPro.Report_Design.dsProposalMinutesTableAdapters;

namespace CalcPro
{
    public partial class frmReportSetting : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// This form is to save the report settings of angebot report and generate a Angebot report when it is opened from project form
        /// </summary>
        #region Varibales
        EReportDesign ObjEReport = null;
        BReportDesign ObjBReport = null;
        public bool _ISave = false;
        public bool _ISMAMOChecked= false;
        BGAEB objBGAEB = null;
        int _ProjectID = 0;
        string stRaster = string.Empty;
        string stProjectNumber = string.Empty;
        #endregion

        #region Constructors
        public frmReportSetting()
        {
            InitializeComponent();
        }

        public frmReportSetting(int _ProjID,string _stProjectNumber,string _stRaster = "")
        {
            InitializeComponent();
            _ProjectID = _ProjID;
            stRaster = _stRaster;
            stProjectNumber = _stProjectNumber;
        }
        #endregion

        #region Events
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ObjBReport == null)
                    ObjBReport = new BReportDesign();
                if (ObjEReport == null)
                    ObjEReport = new EReportDesign();
                ParseReportSettings();
                ObjBReport.SaveReportSetting(ObjEReport);
                if (_ProjectID > 0)
                {
                    DataTable dtPos = new DataTable();
                    dtPos.Columns.Add("FromPos");
                    dtPos.Columns.Add("ToPos");
                    if (radioGroupSelection.SelectedIndex == 0)
                    {
                        this.Hide();
                        if (chkMAMO.Checked)
                            GenerateReportWithputMAMO("Complete", dtPos, cmbLVSection.Text);
                        else if (chkOnheMontagePrice.Checked)
                            GenerateReportWithMinutes("Complete", dtPos, cmbLVSection.Text);
                        else 
                            GenerateReport("Complete", dtPos, cmbLVSection.Text);
                        this.Close();
                    }
                    else if (radioGroupSelection.SelectedIndex == 1)
                    {
                        if (gvAddRemovePositions.RowCount == 0)
                        {
                            if (Utility._IsGermany == true)
                                XtraMessageBox.Show("Bitte machen Sie VON / BIS Angaben.");
                            else
                                XtraMessageBox.Show("Please Add From and To values.");
                            return;
                        }
                        string tfrom = null;
                        string tTo = null;
                        foreach (DataGridViewRow dr in gvAddRemovePositions.Rows)
                        {
                            DataRow drPos = dtPos.NewRow();
                            tfrom = Convert.ToString(dr.Cells[0].Value);
                            tTo = Convert.ToString(dr.Cells[1].Value);
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
                            drPos["fromPos"] = Utility.PrepareOZ(tfrom, stRaster);
                            drPos["toPos"] = Utility.PrepareOZ(tTo, stRaster);
                            dtPos.Rows.Add(drPos);
                        }
                        this.Hide();
                        if (chkMAMO.Checked)
                            GenerateReportWithputMAMO("Title", dtPos, cmbLVSection.Text);
                        else if (chkOnheMontagePrice.Checked)
                            GenerateReportWithMinutes("Title", dtPos, cmbLVSection.Text);
                        else
                            GenerateReport("Title", dtPos, cmbLVSection.Text);
                        this.Close();
                    }
                    else if (radioGroupSelection.SelectedIndex == 2)
                    {
                        if (string.IsNullOrEmpty(cmbLVSection.Text))
                            throw new Exception("Please select atleast one LV Section");
                        this.Hide();
                        if (chkMAMO.Checked)
                            GenerateReportWithputMAMO("LVSection", dtPos, cmbLVSection.Text);
                        else if (chkOnheMontagePrice.Checked)
                            GenerateReportWithMinutes("LVSection", dtPos, cmbLVSection.Text);
                        else
                            GenerateReport("LVSection", dtPos, cmbLVSection.Text);
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void frmReportSetting_Load(object sender, EventArgs e)
        {
            try
            {
                if (ObjBReport == null)
                    ObjBReport = new BReportDesign();
                if (ObjEReport == null)
                    ObjEReport = new EReportDesign();
                ObjBReport.GetReportSettings(ObjEReport);
                if (ObjEReport.dtReportSettings.Rows.Count > 0)
                {
                    BindData();
                }
                FillLVSection();
                txtReportName.Text = "ANGEBOT";
                dtpReportDate.DateTime = DateTime.Now;
                cmbLVSection.EditValue = null;
            }
            catch (Exception ex){Utility.ShowError(ex);}
        }

        private void radioGroupSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioGroupSelection.SelectedIndex == 0)
                {
                    gvAddRemovePositions.Enabled = false;
                    cmbLVSection.Properties.ReadOnly = true;
                    cmbLVSection.EditValue = null;
                }
                else if (radioGroupSelection.SelectedIndex == 1)
                {
                    gvAddRemovePositions.Enabled = true;
                    cmbLVSection.Properties.ReadOnly = true;
                    cmbLVSection.EditValue = null;
                }
                else if (radioGroupSelection.SelectedIndex == 2)
                {
                    gvAddRemovePositions.Enabled = false;
                    cmbLVSection.Properties.ReadOnly = false;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gvAddRemovePositions_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    this.contextMenuStrip1.Show(this.gvAddRemovePositions, e.Location);
                    contextMenuStrip1.Show(Cursor.Position);
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void ToolStripMenuItemAdd_Click(object sender, EventArgs e)
        {
            try
            {
                gvAddRemovePositions.Rows.Add();
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void ToolStripMenuItemRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.gvAddRemovePositions.SelectedRows.Count > 0)
                {
                    foreach (DataGridViewRow item in this.gvAddRemovePositions.SelectedRows)
                    {
                        gvAddRemovePositions.Rows.RemoveAt(item.Index);
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void chkNone_CheckedChanged(object sender, EventArgs e)
        {
            if(chkNone.Checked)
            {
                chkH.Checked = false;
                chkAB.Checked = false;
                chkBA.Checked = false;
                chkVR.Checked = false;
                chkUB.Checked = false;
                chkH.ReadOnly = true;
                chkAB.ReadOnly = true;
                chkBA.ReadOnly = true;
                chkVR.ReadOnly = true;
                chkUB.ReadOnly = true;
            }
            else
            {
                chkH.ReadOnly = false;
                chkAB.ReadOnly = false;
                chkBA.ReadOnly = false;
                chkVR.ReadOnly = false;
                chkUB.ReadOnly = false;
            }
        }

        #endregion

        #region Functions

        /// <summary>
        /// Code to generate complete angebot report
        /// </summary>
        /// <param name="stType"></param>
        /// <param name="dtPos"></param>
        /// <param name="LVSection"></param>
        private void GenerateReport(string stType,DataTable dtPos,string LVSection)
        {
            try
            {
                dsProposal ds = new dsProposal();
                P_Rpt_PositionForProposalPriceTableAdapter adapter = new P_Rpt_PositionForProposalPriceTableAdapter();
                adapter.Connection.ConnectionString = SQLCon.ConnectionString();
                adapter.ClearBeforeFill = true;
                adapter.Fill(ds.P_Rpt_PositionForProposalPrice, dtPos, _ProjectID, stType, LVSection);
                if (ds != null)
                {
                    DataTable dt = ObjBReport.GetProjectCustomerDetails(_ProjectID);
                    rptProposalPositions rpt = new rptProposalPositions();
                    rpt.DataSource = ds;
                    rpt.Parameters["ReportName"].Value = txtReportName.Text;
                    rpt.Parameters["ReportDate"].Value = dtpReportDate.DateTime;
                    rpt.Parameters["CustomerName"].Value = dt.Rows[0]["CustomerFullName"];
                    rpt.Parameters["CustomerAddress"].Value = dt.Rows[0]["CustomerStreet"];
                    rpt.Parameters["ProjectDesc"].Value = dt.Rows[0]["ProjectDescription"];
                    rpt.Parameters["ProjectNumber"].Value = dt.Rows[0]["ProjectNumber"];
                    if (chkWithDetailKZ.Checked)
                        rpt.Parameters["DetKZ"].Value = true;
                    else
                        rpt.Parameters["DetKZ"].Value = false;

                    if (radioGroupShowText.SelectedIndex == 0)
                        rpt.Parameters["Langtext"].Value = true;
                    else if (radioGroupShowText.SelectedIndex == 1)
                        rpt.Parameters["Kurztext"].Value = true;
                    else
                    {
                        rpt.Parameters["Langtext"].Value = true;
                        rpt.Parameters["Kurztext"].Value = true;
                    }

                    if (chksender.Checked)
                        rpt.Parameters["CustomerSender"].Value = false;
                    else
                        rpt.Parameters["CustomerSender"].Value = true;

                    if (chkmenge.Checked)
                        rpt.Parameters["WithMenge"].Value = false;
                    else
                        rpt.Parameters["WithMenge"].Value = true;

                    if (chkGB.Checked)
                        rpt.Parameters["WithGP"].Value = false;
                    else
                        rpt.Parameters["WithGP"].Value = true;

                    if (chkEP.Checked)
                        rpt.Parameters["WithEP"].Value = false;
                    else
                        rpt.Parameters["WithEP"].Value = true;

                    if (chkprices.Checked)
                        rpt.Parameters["WithPrice"].Value = false;
                    else
                        rpt.Parameters["WithPrice"].Value = true;

                    if (chkTitlesPreises.Checked)
                        rpt.Parameters["WithPrices"].Value = false;
                    else
                        rpt.Parameters["WithPrices"].Value = true;

                    if (chkOnheMengePositions.Checked)
                        rpt.Parameters["OnheMenge"].Value = true;
                    else
                        rpt.Parameters["OnheMenge"].Value = false;

                    rpt.CreateDocument();

                    DataView dv = ds.Tables[0].DefaultView;
                    dv.RowFilter = "PositionKZ NOT IN ('H','VR','UB','AB','BA')";
                    DataTable dtsource = dv.ToTable();

                    rptProposalSummary rptSummary = new rptProposalSummary();
                    rptSummary.DataSource = dtsource;
                    rptSummary.Parameters["ReportName"].Value = txtReportName.Text;
                    rptSummary.Parameters["ReportDate"].Value = dtpReportDate.DateTime;
                    rptSummary.Parameters["ProjectDesc"].Value = dt.Rows[0]["ProjectDescription"];
                    rptSummary.Parameters["ProjectNumber"].Value = dt.Rows[0]["ProjectNumber"];
                    rptSummary.Parameters["Vat"].Value = dt.Rows[0]["Vat"];
                    rptSummary.Parameters["AngebotDescription"].Value = dt.Rows[0]["AngebotDescription"];
                    if (chkGB.Checked)
                        rptSummary.Parameters["WithGP"].Value = false;
                    else
                        rptSummary.Parameters["WithGP"].Value = true;

                    if (chkprices.Checked)
                        rptSummary.Parameters["WithPrice"].Value = false;
                    else
                        rptSummary.Parameters["WithPrice"].Value = true;

                    rptSummary.CreateDocument();
                    rpt.Pages.AddRange(rptSummary.Pages);

                    rpt.PrintingSystem.ContinuousPageNumbering = true;
                    ReportPrintTool printTool = new ReportPrintTool(rpt);
                    printTool.ShowRibbonPreview();
                }
            }
            catch (Exception ex){}
        }

        /// <summary>
        /// Code to generate angebot report with material and montage prices
        /// </summary>
        /// <param name="stType"></param>
        /// <param name="dtPos"></param>
        /// <param name="LVSection"></param>
        private void GenerateReportWithputMAMO(string stType, DataTable dtPos, string LVSection)
        {
            try
            {
                dsProposal ds = new dsProposal();
                P_Rpt_PositionForProposalPriceTableAdapter adapter = new P_Rpt_PositionForProposalPriceTableAdapter();
                adapter.Connection.ConnectionString = SQLCon.ConnectionString();
                adapter.ClearBeforeFill = true;
                adapter.Fill(ds.P_Rpt_PositionForProposalPrice, dtPos, _ProjectID, stType, LVSection);
                if (ds != null)
                {
                    DataTable dt = ObjBReport.GetProjectCustomerDetails(_ProjectID);
                    rptProposalPositionsWithoutMAMO rpt = new rptProposalPositionsWithoutMAMO();
                    rpt.DataSource = ds;
                    rpt.Parameters["ReportName"].Value = txtReportName.Text;
                    rpt.Parameters["ReportDate"].Value = dtpReportDate.DateTime;
                    rpt.Parameters["CustomerName"].Value = dt.Rows[0]["CustomerFullName"];
                    rpt.Parameters["CustomerAddress"].Value = dt.Rows[0]["CustomerStreet"];
                    rpt.Parameters["ProjectDesc"].Value = dt.Rows[0]["ProjectDescription"];
                    rpt.Parameters["ProjectNumber"].Value = dt.Rows[0]["ProjectNumber"];
                    if (chkWithDetailKZ.Checked)
                        rpt.Parameters["DetKZ"].Value = true;
                    else
                        rpt.Parameters["DetKZ"].Value = false;

                    if (radioGroupShowText.SelectedIndex == 0)
                        rpt.Parameters["Langtext"].Value = true;
                    else if (radioGroupShowText.SelectedIndex == 1)
                        rpt.Parameters["Kurztext"].Value = true;
                    else
                    {
                        rpt.Parameters["Langtext"].Value = true;
                        rpt.Parameters["Kurztext"].Value = true;
                    }

                    if (chksender.Checked)
                        rpt.Parameters["CustomerSender"].Value = false;
                    else
                        rpt.Parameters["CustomerSender"].Value = true;

                    if (chkmenge.Checked)
                        rpt.Parameters["WithMenge"].Value = false;
                    else
                        rpt.Parameters["WithMenge"].Value = true;

                    if (chkGB.Checked)
                        rpt.Parameters["WithGP"].Value = false;
                    else
                        rpt.Parameters["WithGP"].Value = true;

                    if (chkEP.Checked)
                        rpt.Parameters["WithEP"].Value = false;
                    else
                        rpt.Parameters["WithEP"].Value = true;

                    if (chkprices.Checked)
                        rpt.Parameters["WithPrice"].Value = false;
                    else
                        rpt.Parameters["WithPrice"].Value = true;

                    if (chkTitlesPreises.Checked)
                        rpt.Parameters["WithPrices"].Value = false;
                    else
                        rpt.Parameters["WithPrices"].Value = true;

                    if (chkOnheMengePositions.Checked)
                        rpt.Parameters["OnheMenge"].Value = true;
                    else
                        rpt.Parameters["OnheMenge"].Value = false;

                    rpt.CreateDocument();

                    DataView dv = ds.Tables[0].DefaultView;
                    dv.RowFilter = "PositionKZ NOT IN ('H','VR','UB','AB','BA')";
                    DataTable dtsource = dv.ToTable();
                    rptProposalSummaryWithoutMAMO rptSummary = new rptProposalSummaryWithoutMAMO();
                    rptSummary.DataSource = dtsource;
                    rptSummary.Parameters["ReportName"].Value = txtReportName.Text;
                    rptSummary.Parameters["ReportDate"].Value = dtpReportDate.DateTime;
                    rptSummary.Parameters["ProjectDesc"].Value = dt.Rows[0]["ProjectDescription"];
                    rptSummary.Parameters["ProjectNumber"].Value = dt.Rows[0]["ProjectNumber"];
                    rptSummary.Parameters["Vat"].Value = dt.Rows[0]["Vat"];
                    rptSummary.Parameters["AngebotDescription"].Value = dt.Rows[0]["AngebotDescription"];
                    if (chkGB.Checked)
                        rptSummary.Parameters["WithGP"].Value = false;
                    else
                        rptSummary.Parameters["WithGP"].Value = true;

                    if (chkprices.Checked)
                        rptSummary.Parameters["WithPrice"].Value = false;
                    else
                        rptSummary.Parameters["WithPrice"].Value = true;

                    rptSummary.CreateDocument();
                    rpt.Pages.AddRange(rptSummary.Pages);

                    rpt.PrintingSystem.ContinuousPageNumbering = true;
                    ReportPrintTool printTool = new ReportPrintTool(rpt);
                    printTool.ShowRibbonPreview();
                }
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// Code to generate angebot report with minutes
        /// </summary>
        /// <param name="stType"></param>
        /// <param name="dtPos"></param>
        /// <param name="LVSection"></param>
        private void GenerateReportWithMinutes(string stType, DataTable dtPos, string LVSection)
        {
            try
            {
                dsProposalMinutes ds = new dsProposalMinutes();
                P_Rpt_PositionForProposalMinutesTableAdapter adapter = new P_Rpt_PositionForProposalMinutesTableAdapter();
                adapter.Connection.ConnectionString = SQLCon.ConnectionString();
                adapter.ClearBeforeFill = true;
                adapter.Fill(ds.P_Rpt_PositionForProposalMinutes, dtPos, _ProjectID, stType, LVSection);
                if (ds != null)
                {
                    DataTable dt = ObjBReport.GetProjectCustomerDetails(_ProjectID);
                    rptProposalPositionswithminutes rpt = new rptProposalPositionswithminutes();
                    rpt.DataSource = ds;
                    rpt.Parameters["ReportName"].Value = txtReportName.Text;
                    rpt.Parameters["ReportDate"].Value = dtpReportDate.DateTime;
                    rpt.Parameters["CustomerName"].Value = dt.Rows[0]["CustomerFullName"];
                    rpt.Parameters["CustomerAddress"].Value = dt.Rows[0]["CustomerStreet"];
                    rpt.Parameters["ProjectDesc"].Value = dt.Rows[0]["ProjectDescription"];
                    rpt.Parameters["ProjectNumber"].Value = dt.Rows[0]["ProjectNumber"];
                    if (chkWithDetailKZ.Checked)
                        rpt.Parameters["DetKZ"].Value = true;
                    else
                        rpt.Parameters["DetKZ"].Value = false;

                    if (radioGroupShowText.SelectedIndex == 0)
                        rpt.Parameters["Langtext"].Value = true;
                    else if (radioGroupShowText.SelectedIndex == 1)
                        rpt.Parameters["Kurztext"].Value = true;
                    else
                    {
                        rpt.Parameters["Langtext"].Value = true;
                        rpt.Parameters["Kurztext"].Value = true;
                    }

                    if (chksender.Checked)
                        rpt.Parameters["CustomerSender"].Value = false;
                    else
                        rpt.Parameters["CustomerSender"].Value = true;

                    if (chkmenge.Checked)
                        rpt.Parameters["WithMenge"].Value = false;
                    else
                        rpt.Parameters["WithMenge"].Value = true;

                    if (chkGB.Checked)
                        rpt.Parameters["WithGP"].Value = false;
                    else
                        rpt.Parameters["WithGP"].Value = true;

                    if (chkEP.Checked)
                        rpt.Parameters["WithEP"].Value = false;
                    else
                        rpt.Parameters["WithEP"].Value = true;

                    if (chkprices.Checked)
                        rpt.Parameters["WithPrice"].Value = false;
                    else
                        rpt.Parameters["WithPrice"].Value = true;

                    if (chkTitlesPreises.Checked)
                        rpt.Parameters["WithPrices"].Value = false;
                    else
                        rpt.Parameters["WithPrices"].Value = true;

                    if (chkOnheMengePositions.Checked)
                        rpt.Parameters["OnheMenge"].Value = true;
                    else
                        rpt.Parameters["OnheMenge"].Value = false;

                    rpt.CreateDocument();

                    DataView dv = ds.Tables[0].DefaultView;
                    dv.RowFilter = "PositionKZ NOT IN ('H','VR','UB','AB','BA')";
                    DataTable dtsource = dv.ToTable();

                    rptProposalSummarywithminutes rptSummary = new rptProposalSummarywithminutes();
                    rptSummary.DataSource = dtsource;
                    rptSummary.Parameters["ReportName"].Value = txtReportName.Text;
                    rptSummary.Parameters["ReportDate"].Value = dtpReportDate.DateTime;
                    rptSummary.Parameters["ProjectDesc"].Value = dt.Rows[0]["ProjectDescription"];
                    rptSummary.Parameters["ProjectNumber"].Value = dt.Rows[0]["ProjectNumber"];
                    rptSummary.Parameters["Vat"].Value = dt.Rows[0]["Vat"];
                    rptSummary.Parameters["AngebotDescription"].Value = dt.Rows[0]["AngebotDescription"];
                    if (chkGB.Checked)
                        rptSummary.Parameters["WithGP"].Value = false;
                    else
                        rptSummary.Parameters["WithGP"].Value = true;

                    if (chkprices.Checked)
                        rptSummary.Parameters["WithPrice"].Value = false;
                    else
                        rptSummary.Parameters["WithPrice"].Value = true;

                    rptSummary.CreateDocument();
                    rpt.Pages.AddRange(rptSummary.Pages);

                    rpt.PrintingSystem.ContinuousPageNumbering = true;
                    ReportPrintTool printTool = new ReportPrintTool(rpt);
                    printTool.ShowRibbonPreview();
                }
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// Code to fetch Lv Sections from database and bind to combobox
        /// </summary>
        private void FillLVSection()
        {
            try
            {
                if (objBGAEB == null)
                    objBGAEB = new BGAEB();
                DataTable dtLVSection = new DataTable();
                dtLVSection = objBGAEB.GetLVSection(_ProjectID);
                if (Utility.LVSectionEditAccess == "7")
                {
                    DataTable dttemp = dtLVSection.Copy();
                    DataView dv = dttemp.DefaultView;
                    dv.RowFilter = "LVSectionName = 'HA'";
                    dtLVSection = new DataTable();
                    dtLVSection = dv.ToTable();
                }
                cmbLVSection.Properties.DataSource = dtLVSection;
                cmbLVSection.Properties.DisplayMember = "LVSectionName";
                cmbLVSection.Properties.ValueMember = "LVSectionID";
                Utility.SetCheckedComboexitValue(cmbLVSection, "HA");
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// /Code to parse report settings while saving into database
        /// </summary>
        private void ParseReportSettings()
        {
            try
            {
                if (radioGroupSorting.SelectedIndex == 0)
                {
                    ObjEReport.LVPosition = true;
                }
                if (radioGroupSorting.SelectedIndex == 1)
                {
                    ObjEReport.ArticlNr = true;
                }
                if (radioGroupSorting.SelectedIndex == 2)
                {
                    ObjEReport.Lieferant = true;
                }
                if (radioGroupSorting.SelectedIndex == 3)
                {
                    ObjEReport.Fabrikat = true;
                }
                if (radioGroupShowText.SelectedIndex == 0)
                {
                    ObjEReport.LangText = true;
                }
                if (radioGroupShowText.SelectedIndex == 1)
                {
                    ObjEReport.KurzText = true;
                }
                if (radioGroupShowText.SelectedIndex == 2)
                {
                    ObjEReport.KurzAnLangText = true;
                }


                if (chksender.Checked)
                    ObjEReport.Sender = true;
                else
                    ObjEReport.Sender = false;
                if (chkmenge.Checked)
                    ObjEReport.Menge = true;
                else
                    ObjEReport.Menge = false;
                if (chkGB.Checked)
                    ObjEReport.GB = true;
                else
                    ObjEReport.GB = false;
                if (chkprices.Checked)
                    ObjEReport.Prices = true;
                else
                    ObjEReport.Prices = false;
                if (chkEP.Checked)
                    ObjEReport.EP = true;
                else
                    ObjEReport.EP = false;
                if (chkTitlesPreises.Checked)
                    ObjEReport.WithTitlePrices = true;
                else
                    ObjEReport.WithTitlePrices = false;
                if (chkMAMO.Checked)
                {
                    ObjEReport.MAMO = true;
                    _ISMAMOChecked = true;
                }
                else
                {
                    ObjEReport.MAMO = false;
                    _ISMAMOChecked = false;
                }
                if (chkH.Checked)
                    ObjEReport.HPos = true;
                else
                    ObjEReport.HPos = false;
                if (chkAB.Checked)
                    ObjEReport.ABPos = true;
                else
                    ObjEReport.ABPos = false;
                if (chkBA.Checked)
                    ObjEReport.BAPos = true;
                else
                    ObjEReport.BAPos = false;
                if (chkVR.Checked)
                    ObjEReport.VRPos = true;
                else
                    ObjEReport.VRPos = false;
                if (chkUB.Checked)
                    ObjEReport.UBPos = true;
                else
                    ObjEReport.UBPos = false;
                if (chkNone.Checked)
                    ObjEReport.NonePos = true;
                else
                    ObjEReport.NonePos = false;
                if (chkWithDetailKZ.Checked)
                    ObjEReport.WithDetailKZ = true;
                else
                    ObjEReport.WithDetailKZ = false;

                if (chkOnheMengePositions.Checked)
                    ObjEReport.OnheMengeZeroPositions = true;
                else
                    ObjEReport.OnheMengeZeroPositions = false;

                if (chkOnheMontagePrice.Checked)
                    ObjEReport.OnheMontagePrice = true;
                else
                    ObjEReport.OnheMontagePrice = false;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Code to Bind report settings to controls
        /// </summary>
        private void BindData()
        {
            try
            {
                bool _values = false;
                foreach (DataRow row in ObjEReport.dtReportSettings.Rows)
                {
                    //Sorting
                    if (bool.TryParse(row["LVPosition"].ToString(), out _values) && _values)
                        radioGroupSorting.SelectedIndex = 0;
                    if (bool.TryParse(row["Fabrikat"].ToString(), out _values) && _values)
                        radioGroupSorting.SelectedIndex = 3;
                    if (bool.TryParse(row["ArticleNr"].ToString(), out _values) && _values)
                        radioGroupSorting.SelectedIndex = 1;
                    if (bool.TryParse(row["LieferantMA"].ToString(), out _values) && _values)
                        radioGroupSorting.SelectedIndex = 2;
                    //Select Text
                    if (bool.TryParse(row["LangText"].ToString(), out _values) && _values)
                        radioGroupShowText.SelectedIndex = 0;
                    if (bool.TryParse(row["KurzText"].ToString(), out _values) && _values)
                        radioGroupShowText.SelectedIndex = 1;
                    if (bool.TryParse(row["KurzAndLangText"].ToString(), out _values) && _values)
                        radioGroupShowText.SelectedIndex = 2;
                    //Selection Option
                    if (bool.TryParse(row["Sender"].ToString(), out _values) && _values)
                        chksender.Checked = true;
                    if (bool.TryParse(row["Menge"].ToString(), out _values) && _values)
                        chkmenge.Checked = true;
                    if (bool.TryParse(row["GB"].ToString(), out _values) && _values)
                        chkGB.Checked = true;
                    if (bool.TryParse(row["EP"].ToString(), out _values) && _values)
                        chkEP.Checked = true;
                    if (bool.TryParse(row["Prices"].ToString(), out _values) && _values)
                        chkprices.Checked = true;
                    if (bool.TryParse(row["MAMO"].ToString(), out _values) && _values)
                        chkMAMO.Checked = true;
                    if (bool.TryParse(row["WithTitlePrices"].ToString(), out _values) && _values)
                        chkTitlesPreises.Checked = true;
                    if (bool.TryParse(row["NonePos"].ToString(), out _values))
                        chkNone.Checked = _values;
                    if (bool.TryParse(row["HPos"].ToString(), out _values))
                        chkH.Checked = _values;
                    if (bool.TryParse(row["ABPos"].ToString(), out _values))
                        chkAB.Checked = _values;
                    if (bool.TryParse(row["BAPos"].ToString(), out _values))
                        chkBA.Checked = _values;
                    if (bool.TryParse(row["VRPos"].ToString(), out _values))
                        chkVR.Checked = _values;
                    if (bool.TryParse(row["UBPos"].ToString(), out _values))
                        chkUB.Checked = _values;
                    if (bool.TryParse(row["WithDetKZ"].ToString(), out _values))
                        chkWithDetailKZ.Checked = _values;
                    if (bool.TryParse(row["OnheMengeZeroPositions"].ToString(), out _values))
                        chkOnheMengePositions.Checked = _values;
                    if (bool.TryParse(row["OnheMontagePrice"].ToString(), out _values))
                        chkOnheMontagePrice.Checked = _values;
                }
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion
    }
}
