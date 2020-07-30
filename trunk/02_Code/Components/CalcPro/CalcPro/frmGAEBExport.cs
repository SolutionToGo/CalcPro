using BL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using DevExpress.XtraSplashScreen;
using System.Threading;
using System.Globalization;
using EL;
namespace CalcPro
{
    public partial class frmGAEBExport : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// This form is to exporting a project to GAEB file
        /// </summary>
        #region variables

        private int _ProjectID = 0;
        private int _Raster = 0;
        private string ProjectNumber = string.Empty;
        public string OutputFilePath = string.Empty;
        public string KNr = string.Empty;
        BGAEB ObjBGAEB = null;
        EGAEB ObjEGAEB = null;
        private string _NewRaster=string.Empty;
        private bool rtnOldRaster = false;
        #endregion

        #region Constructor
        public frmGAEBExport(string tProjectNo, int ProjectID, int Raster_Count, string _raster, bool _rtnOldRaster)
        {
            InitializeComponent();
            txtProjectName.Text = tProjectNo;
            txtFileName.Text = tProjectNo;
            _ProjectID = ProjectID;
            ProjectNumber = tProjectNo;
            _Raster = Raster_Count;
            _NewRaster = _raster;
            rtnOldRaster = _rtnOldRaster;
        }
        #endregion

        #region Events

        private void frmGAEBExport_Load(object sender, EventArgs e)
        {
            try
            {
                if (Utility.LVDetailsAccess == "7")
                    layoutControl1.Enabled = false;
                string[] _D_formats = { "D81", "D84", "D86", "P81", "P84", "P86", "X81", "X84", "X86" };
                string[] _P_formats = { "P81", "P84", "P86", "X81", "X84", "X86" };
                if (_Raster > 9)
                {
                    foreach (string p in _P_formats)
                    {
                        cmbFormatType.Properties.Items.Add(p);
                    }
                    cmbFormatType.SelectedIndex = cmbFormatType.Properties.Items.IndexOf("P86");
                }
                else
                {
                    foreach (string d in _D_formats)
                    {
                        cmbFormatType.Properties.Items.Add(d);
                    }
                    cmbFormatType.SelectedIndex = cmbFormatType.Properties.Items.IndexOf("D86");
                }
                txtFilePath.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                if (ObjBGAEB == null)
                {
                    ObjBGAEB = new BGAEB();
                }
                cmbLVSection.Enabled = true;
                DataTable dtLVSection = new DataTable();
                dtLVSection = ObjBGAEB.GetLVSection(_ProjectID);
                if (rtnOldRaster)
                {
                    DataView dvLVSection = dtLVSection.DefaultView;
                    dvLVSection.RowFilter = "LVSectionName =  'HA'";
                }
                cmbLVSection.Properties.DataSource = dtLVSection;
                cmbLVSection.Properties.DisplayMember = "LVSectionName";
                cmbLVSection.Properties.ValueMember = "LVSectionID";
                Utility.SetCheckedComboexitValue(cmbLVSection, "HA");
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtFilePath.Text = dlg.SelectedPath;
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                SplashScreenManager.Default.SetWaitFormDescription("Exportieren...");
                XmlDocument XMLDoc = null;
                if (cmbFormatType.Text != string.Empty)
                {
                    if (ObjBGAEB == null)
                        ObjBGAEB = new BGAEB();
                    if (ObjEGAEB == null)
                        ObjEGAEB = new EGAEB();
                    ObjEGAEB.ProjectNumber = txtProjectName.Text;
                    string strOTTOFilePath = ObjEGAEB.DirPath = Application.UserAppDataPath + "\\";
                    XMLDoc = ObjBGAEB.Export(_ProjectID, cmbLVSection.EditValue, cmbFormatType.Text, _NewRaster,ObjEGAEB);
                    if (!Directory.Exists(strOTTOFilePath))
                        Directory.CreateDirectory(strOTTOFilePath);
                    string strOutputFilePath = string.Empty;
                    strOutputFilePath = OutputFilePath = txtFilePath.Text + "\\" + txtFileName.Text + "." + cmbFormatType.Text;
                    string strInputFilePath = strOTTOFilePath + txtFileName.Text + ".tml";
                    XMLDoc.Save(strInputFilePath);
                    Utility.ProcesssFile(strInputFilePath, strOutputFilePath);
                }
                else
                {
                    throw new Exception("Bitte wählen Sie das gewünschte GAEB Exportformat");
                }
                SplashScreenManager.CloseForm(false);
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Utility.ShowError(ex);
            }
        }

        private void frmGAEBExport_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void frmGAEBExport_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.Escape)
                    this.Close();
            }
            catch (Exception ex) { }
        }

        #endregion
    }
}
