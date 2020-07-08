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
using CalcPro.Report_Design;
using DevExpress.XtraReports.UI;
using BL;

namespace CalcPro
{
    public partial class frmQuerKalculation : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// This form is to show filter for positions while generating querclaculation report
        /// </summary>
        #region Varibales
        int ProjectID = 0;
        int version = 1;
        BGAEB objBGAEB = null;
        public string stRaster = string.Empty;
        string stProjectNumber = string.Empty;
        #endregion

        #region Constructors
        public frmQuerKalculation()
        {
            InitializeComponent();
        }

        public frmQuerKalculation(int _PID ,string _stProjectNumber, int _version = 1)
        {
            InitializeComponent();
            ProjectID = _PID;
            version = _version;
            stProjectNumber = _stProjectNumber;
        }
        #endregion

        #region Events
        private void gvAddRemovePositions_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                TextBox tb = e.Control as TextBox;
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

        // user defined event named as DataGrid_KeyPress
        private void DataGrid_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (radioGroupSelection.SelectedIndex == 1)
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

        private void radioGroupSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioGroupSelection.SelectedIndex == 0)
                {
                    gvAddRemovePositions.Enabled = false;
                    cmbLVSection.Enabled = false;
                }
                else if (radioGroupSelection.SelectedIndex == 1)
                {
                    gvAddRemovePositions.Enabled = true;
                    cmbLVSection.Enabled = false;
                }
                else if (radioGroupSelection.SelectedIndex == 2)
                {
                    gvAddRemovePositions.Enabled = false;
                    cmbLVSection.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtPos = new DataTable();
                dtPos.Columns.Add("FromPos");
                dtPos.Columns.Add("ToPos");
                if (radioGroupSelection.SelectedIndex == 0)
                {
                    this.Hide();
                    if (version == 1)
                    {
                        rptQuerKalkulation rpt = new rptQuerKalkulation(ProjectID, dtPos, "Complete", cmbLVSection.Text);
                        rpt.Name = "QuerKalkulationV001";
                        //rpt.Name = "QuerKalkulationV001_" + stProjectNumber.Replace("-","");
                        ReportPrintTool printTool = new ReportPrintTool(rpt);
                        rpt.Parameters["ProjectID"].Value = ProjectID;
                        rpt.Parameters["UsrName"].Value = Utility.FirstName;
                        rpt.PrintingSystem.Document.AutoFitToPagesWidth = 1;
                        printTool.ShowRibbonPreview();
                    }else
                    {
                        rptQuercalcV2 rpt = new rptQuercalcV2(ProjectID, dtPos, "Complete", cmbLVSection.Text);
                        rpt.Name = "QuerKalkulationV002";
                        //rpt.Name = "QuerKalkulationV002_" + stProjectNumber.Replace("-", "");
                        ReportPrintTool printTool = new ReportPrintTool(rpt);
                        rpt.Parameters["ProjectID"].Value = ProjectID;
                        rpt.Parameters["UsrName"].Value = Utility.FirstName;
                        rpt.PrintingSystem.Document.AutoFitToPagesWidth = 1;
                        printTool.ShowRibbonPreview();
                    }
                    this.Close();
                }
                else if (radioGroupSelection.SelectedIndex == 1)
                {
                    if (gvAddRemovePositions.RowCount == 0)
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
                    string tfrom = null;
                    string tTo = null;
                    foreach (DataGridViewRow dr in gvAddRemovePositions.Rows)
                    {
                        DataRow drPos = dtPos.NewRow();
                        tfrom = dr.Cells[0].Value.ToString();
                        tTo = dr.Cells[1].Value.ToString();
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
                        drPos["fromPos"] = Utility.PrepareOZ(tfrom.Replace(',', '.'),stRaster);
                        drPos["toPos"] = Utility.PrepareOZ(tTo.Replace(',', '.'),stRaster);
                        dtPos.Rows.Add(drPos);
                    }

                    this.Hide();
                    if (version == 1)
                    {
                        rptQuerKalkulation rpt = new rptQuerKalkulation(ProjectID, dtPos, "Title", cmbLVSection.Text);
                        rpt.Name = "QuerKalkulationV001";
                        ReportPrintTool printTool = new ReportPrintTool(rpt);
                        rpt.Parameters["ProjectID"].Value = ProjectID;
                        rpt.Parameters["UsrName"].Value = Utility.FirstName;
                        rpt.PrintingSystem.Document.AutoFitToPagesWidth = 1;
                        printTool.ShowRibbonPreview();
                    }
                    else
                    {
                        rptQuercalcV2 rpt = new rptQuercalcV2(ProjectID, dtPos, "Title", cmbLVSection.Text);
                        rpt.Name = "QuerKalkulationV002";
                        ReportPrintTool printTool = new ReportPrintTool(rpt);
                        rpt.Parameters["ProjectID"].Value = ProjectID;
                        rpt.Parameters["UsrName"].Value = Utility.FirstName;
                        rpt.PrintingSystem.Document.AutoFitToPagesWidth = 1;
                        printTool.ShowRibbonPreview();
                    }
                    this.Close();
                }
                else if (radioGroupSelection.SelectedIndex == 2)
                {
                    this.Hide();
                    if (version == 1)
                    {
                        rptQuerKalkulation rpt = new rptQuerKalkulation(ProjectID, dtPos, "LVSection", cmbLVSection.Text);
                        rpt.Name = "QuerKalkulationV001";
                        ReportPrintTool printTool = new ReportPrintTool(rpt);
                        rpt.Parameters["ProjectID"].Value = ProjectID;
                        rpt.Parameters["UsrName"].Value = Utility.FirstName;
                        rpt.PrintingSystem.Document.AutoFitToPagesWidth = 1;
                        printTool.ShowRibbonPreview();
                    }
                    else
                    {
                        rptQuercalcV2 rpt = new rptQuercalcV2(ProjectID, dtPos, "LVSection", cmbLVSection.Text);
                        rpt.Name = "QuerKalkulationV002";
                        ReportPrintTool printTool = new ReportPrintTool(rpt);
                        rpt.Parameters["ProjectID"].Value = ProjectID;
                        rpt.Parameters["UsrName"].Value = Utility.FirstName;
                        rpt.PrintingSystem.Document.AutoFitToPagesWidth = 1;
                        printTool.ShowRibbonPreview();
                    }
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void frmQuerKalculation_Load(object sender, EventArgs e)
        {
            try
            {
                if (objBGAEB == null)
                    objBGAEB = new BGAEB();
                DataTable dtLVSection = new DataTable();
                dtLVSection = objBGAEB.GetLVSection(ProjectID);
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
            catch (Exception ex){}
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}