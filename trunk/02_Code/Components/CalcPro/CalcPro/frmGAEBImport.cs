using BL;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using EL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalcPro
{    public partial class frmGAEBImport : Form
    {
        /// <summary>
        /// This form is to import GAEB file on a project
        /// </summary>
        /// 
        #region Variables
        public string KNr = string.Empty;
        public BGAEB ObjBGAEB = new BGAEB();
        public int ProjectID = 0;
        public bool isbuild = false;
        #endregion

        #region Constructors
        public frmGAEBImport()
        {
            InitializeComponent();
        }

        #endregion

        
        #region Events
        private void frmGAEBImport_Load(object sender, EventArgs e)
        {
            try
            {
                if (Utility.LVDetailsAccess == "7")
                    layoutControl1.Enabled = false;
                cmbLVSection.Enabled = true;
                cmbLVSection.DataSource = ObjBGAEB.GetLVSection(ProjectID);
                cmbLVSection.DisplayMember = "LVSectionName";
                cmbLVSection.ValueMember = "LVSectionID";
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                string strInputFilePath = string.Empty;
                XtraOpenFileDialog dlg = new XtraOpenFileDialog();

                dlg.InitialDirectory = @"C:\";
                dlg.Title = "Dateiauswahl für GAEB Import";

                dlg.CheckFileExists = true;
                dlg.CheckPathExists = true;

                dlg.Filter = "GAEB Files(*.D81;*.D83;*.D86;*.P81;*.P83;*.P86;*.X81;*.X83;*.X86) | *.D81;*.D83;*.D86;*.P81;*.P83;*.P86;*.X81;*.X83;*.X86";
                dlg.RestoreDirectory = true;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtImportFilePath.Text = dlg.FileName;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                SplashScreenManager.Default.SetWaitFormDescription("Importieren...");
                if (txtImportFilePath.Text != string.Empty)
                {
                    string strOutputFilepath = string.Empty;
                    string strOTTOFilePath = Application.UserAppDataPath + "\\";
                    if (!Directory.Exists(strOTTOFilePath))
                        Directory.CreateDirectory(strOTTOFilePath);
                    string strFileName = Path.GetFileNameWithoutExtension(txtImportFilePath.Text);
                    strOutputFilepath = strOTTOFilePath + strFileName + ".tml";
                    Utility.ProcesssFile(txtImportFilePath.Text, strOutputFilepath);

                    EGAEB ObjEGAEB = new EGAEB();
                    string strRaster = Utility.GetRaster(strOutputFilepath);
                    DataSet dsTmlData = Utility.CreateDatasetSchema(strOutputFilepath, cmbLVSection.Text, strRaster, ObjEGAEB);

                    ProjectID = ObjBGAEB.Import(ProjectID, dsTmlData, strRaster);
                    isbuild = true;
                    this.Close();
                }
                else
                {
                    if (Utility._IsGermany == true)
                        throw new Exception("Bitte wählen Sie die zu importierende Datei aus");
                    else
                        throw new Exception("Please select the file to import");
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Utility.ShowError(ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmGAEBImport_FormClosing(object sender, FormClosingEventArgs e)
        {
            SplashScreenManager.CloseForm(false);
        }

        #endregion
    }
}