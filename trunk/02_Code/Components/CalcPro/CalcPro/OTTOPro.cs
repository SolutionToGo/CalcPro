using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Helpers;
using System.Threading;
using DevExpress.XtraSplashScreen;
using EL;
using BL;
using System.Configuration;
using System.IO;
using CalcPro.Report_Design;
using DevExpress.XtraReports.UI;
using System.Data.OleDb;
using DataAccess;
using DevExpress.XtraEditors;
using Outlook = Microsoft.Office.Interop.Outlook;
using CalcPro.Report_Design.dsProposalTableAdapters;
using DAL;

namespace CalcPro
{
    public partial class frmCalcPro : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        /// <summary>
        /// This is main and parent form where all other forms opned as child s 
        /// </summary>
        #region Varibales
        public static frmCalcPro ObjCalcPro;
        BProject ObjBProject = null;
        EProject ObjEProject = null;
        bool _TriggerFromLogin = false;
        bool CloseFromVersionMismatch = false;
        private static frmCalcPro frmObject;
        #endregion

        #region Constructors
        private frmCalcPro()
        {
            InitializeComponent();
        }

        static frmCalcPro()
        {
            frmObject = new frmCalcPro();
        }
        #endregion

        #region Events
        private void btnNewProject_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                FormCollection fc = Application.OpenForms;
                foreach (Form frm in fc)
                {
                    if (fc != null)
                    {
                        if (frm.Name == "frmLoadProject")
                        {
                            frm.Close();
                            break;
                        }
                    }
                }
                frmProject Obj = new frmProject();
                Obj.MdiParent = this;
                pictureBox1.Visible = false;
                Obj.Show();
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnLoadProject_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                FormCollection fc = Application.OpenForms;
                foreach (Form frm in fc)
                {
                    if (fc != null)
                    {
                        if (frm.Name == "frmLoadProject")
                        {
                            frm.Close();
                            break;
                        }
                    }
                }
                frmLoadProject Obj = new frmLoadProject();
                Obj.MdiParent = this;
                pictureBox1.Visible = false;
                Obj.Show();
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        public void tmrStatus_Tick(object sender, EventArgs e)
        {
            try
            {
                frmCalcPro.Instance.tsStatus.Caption = null;
                tmrStatus.Stop();
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void frmCalcPro_Load(object sender, EventArgs e)
        {
          
            try
            {
                txtAppVersion.Caption = "Version Software : " + Utility.Appversion + Utility.VersionDate;
                txtDBVersion.Caption = "Version Datenbank : " + Utility.DBVersion;
                txtUsername.Caption = "Nutzername : " + Utility.UserName;
                if (Utility.Appversion != Utility.DBVersion)
                {
                    CloseFromVersionMismatch = true;
                    XtraMessageBox.Show("Sie nutzen eine veraltete Softwareversion. \r\nBitte kontaktieren Sie Ihren Administrator.", "Versionsangabe", MessageBoxButtons.OK,MessageBoxIcon.Stop);
                    Application.Exit();
                }

                _TriggerFromLogin = true;
                chkAutoSave1.EditValue = Utility.AutoSave;
                if (Utility.ArticleDataAccess == "9")
                    rpgArticleMaster.Visible = false;

                if (Utility.CustomerDataAccess == "9")
                    btnCustomer.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

                if (Utility.SupplierDataAccess == "9")
                    btnSupplier.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

                if (Utility.OTTODataAccess == "9")
                    btnOTTO.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                if (Utility.CustomerDataAccess == "9" && Utility.SupplierDataAccess == "9" && Utility.SupplierDataAccess == "9")
                    ribbonPageGroup5.Visible = false;
                if (Utility.UserDataAccess == "9")
                    ribbonPageGroup2.Visible = false;
                if (Utility.GeneralTextModuleAccess == "9" &&
                    Utility.CalculationTextModuleAccess == "9" &&
                    Utility.InvoiceTextModuleAccess == "9")
                {
                    btnTextModule.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
            }
            catch (Exception ex){}

           
        }

        private void btnShortCuts_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmShortCuts frm = new frmShortCuts();
            frm.ShowDialog();

        }

        private void barButtonItemExitProject_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (ActiveMdiChild != null)
                ActiveMdiChild.Close();
        }

        private void btnCustomer_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                frmLoadCustomerMaster Obj = new frmLoadCustomerMaster();
                // Obj.ShowDialog();
                ShowForm(Obj);
            }
            catch (Exception ex) { Utility.ShowError(ex); }
        }

        private void btnOTTO_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                frmLoadOTTOMaster Obj = new frmLoadOTTOMaster();
                //Obj.ShowDialog();
                ShowForm(Obj);
            }
            catch (Exception ex){Utility.ShowError(ex);}
        }

        private void btnSupplier_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmLoadSupplier Obj = new frmLoadSupplier();
            //Obj.ShowDialog();
            ShowForm(Obj);
        }

        private void btnArticledata_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmArticlesData obj = new frmArticlesData();
            ShowForm( obj);
           
        }


        void ShowForm(DevExpress.XtraBars.Ribbon.RibbonForm MdiChild,bool Isnew =true)
        {
            try
            {
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm.Name == MdiChild.Name)
                    {

                        if ( ! Isnew)
                        {

                            MdiChild.Activate();
                            this.ActivateMdiChild(MdiChild);
                            this.ribbon.SelectPage(MdiChild.Ribbon.Pages[0]);
                            MdiChild.WindowState = FormWindowState.Maximized;
                            if (MdiChild.Name == "frmArticleAccessories")
                                this.ribbon.Minimized = true;
                            else
                                this.ribbon.Minimized = false;
                            return;
                        }
                        else
                        {
                            frm.Close();
                            frm.Dispose();
                            break;
                        }

                    }
                }
                this.pictureBox1.Visible = false;
                MdiChild.MdiParent = this;
                MdiChild.WindowState = FormWindowState.Maximized;
                MdiChild.Show();
                this.Ribbon.MdiMergeStyle = DevExpress.XtraBars.Ribbon.RibbonMdiMergeStyle.Always;
                this.ribbon.SelectPage(MdiChild.Ribbon.Pages[0]);
                if (MdiChild.Name == "frmArticleAccessories")
                  this.ribbon.Minimized = true;
                else
                    this.ribbon.Minimized = false;
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnTextModule_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmTextModule Obj = new frmTextModule();
            Obj.ShowDialog();
        }

        private void btnTyp_ItemClick(object sender, ItemClickEventArgs e)
        {            
            frmType Obj = new frmType();
            ShowForm(Obj);
        }

        private void btnRabatt_ItemClick(object sender, ItemClickEventArgs e)
        {            
            frmRabattGroup Obj = new frmRabattGroup();
            //Obj.ShowDialog();
            ShowForm(Obj);
        }

        private void xtraTabbedMdiManager1_PageRemoved(object sender, DevExpress.XtraTabbedMdi.MdiTabPageEventArgs e)
        {
            //if (xtraTabbedMdiManager1.Pages.Count > 0)
            //{
            //    BackgroudImageVisibility(false);
            //}
            //else { BackgroudImageVisibility(true); }
        }

        public void BackgroudImageVisibility(bool visibility)
        {
            //if (visibility)
            //{
            //    pictureBox1.Visible = true;
            //    label2.Visible = true;
            //}
            //else
            //{
            //    pictureBox1.Visible = false;
            //    label2.Visible = false;
            //}
        }

        private void btnUserData_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {                
                frmLoadUsers Obj = new frmLoadUsers();                
                Obj.ShowDialog();
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnFeature_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {                
                frmFeature Obj = new frmFeature();              
                Obj.ShowDialog();
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnImportArticleData_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                DialogResult result = fdImportFile.ShowDialog();
                
                if (result == DialogResult.OK)
                {
                    BArticles ObjBArticle = new BArticles();
                    EArticles ObjEArticle = new EArticles();
                    ObjEArticle = ObjBArticle.ImportExcelXLS(fdImportFile.FileName, ObjEArticle);
                    ObjEArticle = ObjBArticle.ImportArticleData(ObjEArticle);
                    Utility.ShowSucces("Data Imported Sucessfully");
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnChangePassword_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                frmResetPassword Obj = new frmResetPassword();
                Obj.ShowDialog();
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }
        
        private void btnAddAccessories_ItemClick(object sender, ItemClickEventArgs e)
        {           
            frmArticleAccessories Obj = new frmArticleAccessories();
            //Obj.ShowDialog();
            ShowForm(Obj);
        }

        private void btnProjectImport_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                string strFilePath = string.Empty;
                XtraOpenFileDialog  dlg = new XtraOpenFileDialog();
                dlg.Title = "Dateiauswahl für GAEB Import";
                dlg.CheckFileExists = true;
                dlg.CheckPathExists = true;
                dlg.Filter = "GAEB Files(*.D81;*.D83;*.D86;*.P81;*.P83;*.P86;*.X81;*.X83;*.X86) | *.D81;*.D83;*.D86;*.P81;*.P83;*.P86;*.X81;*.X83;*.X86";
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
                    BGAEB ObjBGAEB = new BGAEB();
                    EGAEB ObjEGAEB = new EGAEB();
                    ObjEGAEB.UserID = Utility.UserID;
                    string strRaster = Utility.GetRaster(strOutputFilepath);
                    ObjEGAEB.dsLVData = Utility.CreateDatasetSchema(strOutputFilepath, string.Empty, strRaster, ObjEGAEB);
                    ObjEGAEB.LvRaster = strRaster;
                    ObjEGAEB = ObjBGAEB.ProjectImport(ObjEGAEB);
                    SplashScreenManager.CloseForm(false);
                    frmViewProject Obj = new frmViewProject(ObjEGAEB);
                    Obj.ShowDialog();
                    UpdateStatus("Projektdatenimport mit Erfolg abgeschlossen");
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Utility.ShowError(ex);
            }   
        }

        private void btnFormBlattarticles_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                //frmFormBlattarticles frm = new frmFormBlattarticles();
                //frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnReportSetting_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                frmReportSetting frm = new frmReportSetting();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        #region "Title Blatt"
        private void bbCoverSheetPath_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                frmCoverSheetPath Obj = new frmCoverSheetPath();
                Obj.ShowDialog();
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void bbAngebot_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                SplashScreenManager.Default.SetWaitFormDescription("Bitte warten…");

                if (ObjEProject == null)
                    ObjEProject = new EProject();
                if(ObjBProject == null)
                    ObjBProject = new BProject();
                ObjEProject = ObjBProject.GetPath(ObjEProject);
                var FolderPath = new DirectoryInfo(ObjEProject.TemplatePath).GetFiles("V-011*.dotx", SearchOption.AllDirectories).OrderByDescending(d => d.LastWriteTimeUtc).First();
                Object oTemplatePath = ObjEProject.TemplatePath + "\\" + FolderPath;
                if (File.Exists(Convert.ToString(oTemplatePath)))
                {
                    if (!Utility.fileIsOpen(Convert.ToString(oTemplatePath)))
                    {
                        Microsoft.Office.Interop.Word.Application ap = new Microsoft.Office.Interop.Word.Application();
                        ap.Documents.Open(oTemplatePath);
                        ap.Visible = true;
                        ap.Activate();
                    }
                    else
                        throw new Exception("Bitte schließen Sie die Angebots-Dokumente aller Projekte");
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Sequence contains no elements"))
                    XtraMessageBox.Show("Die erforderliche Dokumentenvorlage ist nicht eingestellt!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { SplashScreenManager.CloseForm(false); }
        }

        private void bbAufmass_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                SplashScreenManager.Default.SetWaitFormDescription("Bitte warten…");
                if (ObjEProject == null)
                    ObjEProject = new EProject();
                if (ObjBProject == null)
                    ObjBProject = new BProject();
                ObjEProject = ObjBProject.GetPath(ObjEProject);

                Object oTemplatePath = ObjEProject.TemplatePath + "\\Aufmass_Template.dotx";
                if (File.Exists(Convert.ToString(oTemplatePath)))
                {
                    if (!Utility.fileIsOpen(Convert.ToString(oTemplatePath)))
                    {
                        Microsoft.Office.Interop.Word.Application ap = new Microsoft.Office.Interop.Word.Application();
                        ap.Documents.Open(oTemplatePath);
                        ap.Visible = true;
                        ap.Activate();
                    }
                    else
                        throw new Exception("Bitte schließen Sie die Aufmass-Dokumente aller Projekte");

                }
                SplashScreenManager.CloseForm(false);
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Utility.ShowError(ex);
            }
        }

        private void bbRechnung_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                SplashScreenManager.Default.SetWaitFormDescription("Bitte warten…");
                if (ObjEProject == null)
                    ObjEProject = new EProject();
                if (ObjBProject == null)
                    ObjBProject = new BProject();
                ObjEProject = ObjBProject.GetPath(ObjEProject);
                Object oTemplatePath = ObjEProject.TemplatePath + "\\Rechnung_Template.dotx";
                if (File.Exists(Convert.ToString(oTemplatePath)))
                {
                    if (!Utility.fileIsOpen(Convert.ToString(oTemplatePath)))
                    {
                        Microsoft.Office.Interop.Word.Application ap = new Microsoft.Office.Interop.Word.Application();
                        ap.Documents.Open(oTemplatePath);
                        ap.Visible = true;
                        ap.Activate();
                    }
                    else
                        throw new Exception("Bitte schließen Sie die Rechnung-Dokumente aller Projekte");

                }
                SplashScreenManager.CloseForm(false);
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Utility.ShowError(ex);
            }
        }

        private void btnAngebot1_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                SplashScreenManager.Default.SetWaitFormDescription("Bitte warten…");

                if (ObjEProject == null)
                    ObjEProject = new EProject();
                if (ObjBProject == null)
                    ObjBProject = new BProject();
                ObjEProject = ObjBProject.GetPath(ObjEProject);
                var FolderPath = new DirectoryInfo(ObjEProject.TemplatePath).GetFiles("V-014*.dotx", SearchOption.AllDirectories).OrderByDescending(d => d.LastWriteTimeUtc).First();
                Object oTemplatePath = ObjEProject.TemplatePath + "\\" + FolderPath;
                if (File.Exists(Convert.ToString(oTemplatePath)))
                {
                    if (!Utility.fileIsOpen(Convert.ToString(oTemplatePath)))
                    {
                        Microsoft.Office.Interop.Word.Application ap = new Microsoft.Office.Interop.Word.Application();
                        ap.Documents.Open(oTemplatePath);
                        ap.Visible = true;
                        ap.Activate();
                    }
                    else
                        throw new Exception("Bitte schließen Sie die Angebots-Dokumente aller Projekte");
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Sequence contains no elements"))
                    XtraMessageBox.Show("Die erforderliche Dokumentenvorlage ist nicht eingestellt!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { SplashScreenManager.CloseForm(false); }
        }

        private void btnAngebot2_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                SplashScreenManager.Default.SetWaitFormDescription("Bitte warten…");

                if (ObjEProject == null)
                    ObjEProject = new EProject();
                if (ObjBProject == null)
                    ObjBProject = new BProject();
                ObjEProject = ObjBProject.GetPath(ObjEProject);
                var FolderPath = new DirectoryInfo(ObjEProject.TemplatePath).GetFiles("V-012*.dotx", SearchOption.AllDirectories).OrderByDescending(d => d.LastWriteTimeUtc).First();
                Object oTemplatePath = ObjEProject.TemplatePath + "\\"+ FolderPath;
                if (File.Exists(Convert.ToString(oTemplatePath)))
                {
                    if (!Utility.fileIsOpen(Convert.ToString(oTemplatePath)))
                    {
                        Microsoft.Office.Interop.Word.Application ap = new Microsoft.Office.Interop.Word.Application();
                        ap.Documents.Open(oTemplatePath);
                        ap.Visible = true;
                        ap.Activate();
                    }
                    else
                        throw new Exception("Bitte schließen Sie die Angebots-Dokumente aller Projekte");
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Sequence contains no elements"))
                    XtraMessageBox.Show("Die erforderliche Dokumentenvorlage ist nicht eingestellt!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { SplashScreenManager.CloseForm(false); }
        }

        private void btnAngebot3_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                SplashScreenManager.Default.SetWaitFormDescription("Bitte warten…");

                if (ObjEProject == null)
                    ObjEProject = new EProject();
                if (ObjBProject == null)
                    ObjBProject = new BProject();
                ObjEProject = ObjBProject.GetPath(ObjEProject);
                var FolderPath = new DirectoryInfo(ObjEProject.TemplatePath).GetFiles("V-013*.dotx", SearchOption.AllDirectories).OrderByDescending(d => d.LastWriteTimeUtc).First();
                Object oTemplatePath = ObjEProject.TemplatePath + "\\" + FolderPath;
                if (File.Exists(Convert.ToString(oTemplatePath)))
                {
                    if (!Utility.fileIsOpen(Convert.ToString(oTemplatePath)))
                    {
                        Microsoft.Office.Interop.Word.Application ap = new Microsoft.Office.Interop.Word.Application();
                        ap.Documents.Open(oTemplatePath);
                        ap.Visible = true;
                        ap.Activate();
                    }
                    else
                        throw new Exception("Bitte schließen Sie die Angebots-Dokumente aller Projekte");
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Sequence contains no elements"))
                    XtraMessageBox.Show("Die erforderliche Dokumentenvorlage ist nicht eingestellt!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { SplashScreenManager.CloseForm(false); }
        }

        private void bbProposalLetter_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                SplashScreenManager.Default.SetWaitFormDescription("Bitte warten…");

                if (ObjEProject == null)
                    ObjEProject = new EProject();
                if (ObjBProject == null)
                    ObjBProject = new BProject();
                ObjEProject = ObjBProject.GetPath(ObjEProject);
                var FolderPath = new DirectoryInfo(ObjEProject.TemplatePath).GetFiles("V-015*.dotx", SearchOption.AllDirectories).OrderByDescending(d => d.LastWriteTimeUtc).First();
                Object oTemplatePath = ObjEProject.TemplatePath + "\\" + FolderPath;
                if (File.Exists(Convert.ToString(oTemplatePath)))
                {
                    if (!Utility.fileIsOpen(Convert.ToString(oTemplatePath)))
                    {
                        Microsoft.Office.Interop.Word.Application ap = new Microsoft.Office.Interop.Word.Application();
                        ap.Documents.Open(oTemplatePath);
                        ap.Visible = true;
                        ap.Activate();
                    }
                    else
                        throw new Exception("Bitte schließen Sie die Angebots-Dokumente aller Projekte");
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Sequence contains no elements"))
                    XtraMessageBox.Show("Die erforderliche Dokumentenvorlage ist nicht eingestellt!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { SplashScreenManager.CloseForm(false); }
        }

        #endregion

        private void simpleButton1_Click(object sender, EventArgs e)
        {

            if (ObjBProject == null)
                ObjBProject = new BProject();
            DataTable dt = new DataTable();
            dt = ObjBProject.GetProjectCustomerDetails(147);

            dsProposal ds = new dsProposal();
            P_Rpt_PositionForProposalPriceTableAdapter adapter = new P_Rpt_PositionForProposalPriceTableAdapter();
            adapter.Connection.ConnectionString = SQLCon.ConnectionString();
            adapter.ClearBeforeFill = true;
            adapter.Fill(ds.P_Rpt_PositionForProposalPrice, "", 147, "", "");
            rptProposalPositions rpt = new rptProposalPositions();
            rpt.DataSource = ds;
            rpt.Parameters["ProjectID"].Value = 147;
            rpt.Parameters["TitleList"].Value = string.Empty;
            rpt.Parameters["Type"].Value = "";
            rpt.Parameters["LVSection"].Value = "";
            rpt.Parameters["ReportName"].Value = "NEW ANGEBOT REPORT";
            rpt.Parameters["ReportDate"].Value = DateTime.Now;
            rpt.Parameters["CustomerName"].Value = dt.Rows[0]["CustomerFullName"];
            rpt.Parameters["CustomerAddress"].Value = dt.Rows[0]["CustomerStreet"];
            rpt.Parameters["ProjectDesc"].Value = dt.Rows[0]["ProjectDescription"];
            rpt.Parameters["ProjectNumber"].Value = dt.Rows[0]["ProjectNumber"];
            rpt.CreateDocument();
            rptProposalSummary rptSummary = new rptProposalSummary();
            rptSummary.DataSource = ds;
            rptSummary.Parameters["ProjectID"].Value = 147;
            rptSummary.Parameters["TitleList"].Value = string.Empty;
            rptSummary.Parameters["Type"].Value = "";
            rptSummary.Parameters["LVSection"].Value = "";
            rptSummary.Parameters["ReportName"].Value = "NEW ANGEBOT REPORT";
            rptSummary.Parameters["ReportDate"].Value = DateTime.Now;
            rpt.Parameters["ProjectDesc"].Value = dt.Rows[0]["ProjectDescription"];
            rpt.Parameters["ProjectNumber"].Value = dt.Rows[0]["ProjectNumber"];
            rptSummary.Parameters["Vat"].Value = dt.Rows[0]["Vat"];
            rptSummary.Parameters["AngebotDescription"].Value = dt.Rows[0]["AngebotDescription"];
            rptSummary.CreateDocument();
            rpt.Pages.AddRange(rptSummary.Pages);

            rpt.PrintingSystem.ContinuousPageNumbering = true;
            ReportPrintTool printTool = new ReportPrintTool(rpt);
            printTool.ShowRibbonPreview();



            //rptSampleBreakdown rptMA = new rptSampleBreakdown();
            //ReportPrintTool printTool = new ReportPrintTool(rptMA);
            //rptMA.Parameters["ProjectID"].Value = 113;
            //printTool.ShowRibbonPreview();

            //try
            //{
            //    string strFilePath = string.Empty;
            //    OpenFileDialog dlg = new OpenFileDialog();

            //    dlg.InitialDirectory = @"C:\";
            //    dlg.Title = "Dateiauswahl für Data File Import";

            //    dlg.CheckFileExists = true;
            //    dlg.CheckPathExists = true;

            //    dlg.Filter = "All files (*.*)|*.*";
            //    dlg.RestoreDirectory = true;

            //    dlg.ReadOnlyChecked = true;
            //    dlg.ShowReadOnly = true;
            //    if (dlg.ShowDialog() == DialogResult.OK)
            //    {
            //        strFilePath = dlg.FileName;
            //        string fileExt = Path.GetExtension(strFilePath);
            //        if (fileExt.CompareTo(".xls") == 0 || fileExt.CompareTo(".xlsx") == 0)
            //        {
            //            DataTable dtExcel = new DataTable();
            //            dtExcel = Utility.ReadExcel(strFilePath, fileExt); //read excel file  

            //            foreach(DataRow dr in dtExcel.Rows)
            //            {
            //                string str = Convert.ToString(dr["F2"]);
            //                if(str.Substring(0,1) == "\n")
            //                    str = str.Substring(1);
            //                if (str.Contains("\r"))
            //                    str = str.Replace("\r", "");
            //                dr["F2"] = str;
            //            }
            //            DataTable dt = dtExcel.Copy();
            //            BOTTO ObjBOTTO = new BOTTO();
            //            ObjBOTTO.ImportCustomerData(dt);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Utility.ShowError(ex);
            //}
        }

        private void barEditItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void chkAutoSave1_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!_TriggerFromLogin)
                {
                    DUserInfo ObjDUserInfo = new DUserInfo();
                    EUserInfo ObjEUserInfo = new EUserInfo();
                    ObjEUserInfo.UserID = Utility.UserID;
                    ObjEUserInfo.AutoSaveMode = Convert.ToBoolean(chkAutoSave1.EditValue);
                    ObjDUserInfo.UpdateAutoSave(ObjEUserInfo);
                    Utility.AutoSave = ObjEUserInfo.AutoSaveMode;
                }
                _TriggerFromLogin = false;
            }
            catch (Exception ex) { Utility.ShowError(ex); }
        }

        private void nbDeletePosition_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                frmProject f = (frmProject)this.ActiveMdiChild;
                f.DeletePosition();
            }
            catch (Exception ex){}
        }

        private void nbCopyPosition_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                frmProject f = (frmProject)this.ActiveMdiChild;
                f.CopyPosition();
            }
            catch (Exception ex) { }
        }

        private void frmCalcPro_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (!CloseFromVersionMismatch)
                {
                    var dlgrslt = XtraMessageBox.Show("Wollen Sie das Programm OTTO PRO jetzt beenden?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (Convert.ToString(dlgrslt) == "No")
                        e.Cancel = true;
                }
            }
            catch (Exception ex){}
        }

        private void btnRefreshProject_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                try
                {
                    frmProject f = (frmProject)this.ActiveMdiChild;
                    f.RefreshProject();
                }
                catch (Exception ex) { }
            }
            catch (Exception ex){}
        }

        private void btnSendLogfile_ItemClick(object sender, ItemClickEventArgs e)
        {
            sendEMailThroughOUTLOOK();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnDataNormImport_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                bool IsOpen = false;
                FormCollection fc = Application.OpenForms;
                foreach (Form frm in fc)
                {
                    if (fc != null)
                    {
                        if (frm.Name == "frmSpreadsheetControl")
                        {
                            frm.Activate();
                            IsOpen = true;
                            break;
                        }
                    }
                }
                if(!IsOpen)
                {
                    frmSpreadsheetControl Obj = new frmSpreadsheetControl();
                    Obj.MdiParent = this;
                    pictureBox1.Visible = false;
                    Obj.Show();
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
        /// Code to send a mail by attaching log file using log4net
        /// </summary>
        public void sendEMailThroughOUTLOOK()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                SplashScreenManager.Default.SetWaitFormDescription("Sending aail to administrator...");
                Outlook.Application oApp = new Outlook.Application();
                Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
                oMsg.HTMLBody = "Hello, Here is the log file!!";
                String sDisplayName = "CalcPro Log File";
                int iPosition = (int)oMsg.Body.Length + 1;
                int iAttachType = (int)Outlook.OlAttachmentType.olByValue;
                string st = Environment.ExpandEnvironmentVariables(@"%AppData%");
                Outlook.Attachment oAttach = oMsg.Attachments.Add(st + "\\CalcPro.log", iAttachType, iPosition, sDisplayName);
                oMsg.Subject = "CalcPro Log File";
                Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;
                Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add("narendar.reddy@betasystems.com");
                oRecip.Resolve();
                oMsg.Send();
                oRecip = null;
                oRecips = null;
                oMsg = null;
                oApp = null;
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Log file mail sent to administrator.!");
            }
            catch (Exception ex){}
        }

        /// <summary>
        /// Code to expose a static object of form
        /// </summary>
        public static frmCalcPro Instance
        {
            get { return frmObject; }
        }

        /// <summary>
        /// Code to view parent form 
        /// </summary>
        public static void LoadParentForm()
        {
            Instance.Show();
        }

        /// <summary>
        /// code to set visible property of picture box
        /// </summary>
        /// <param name="_result"></param>
        public void SetPictureBoxVisible(bool _result)
        {
            this.pictureBox1.Visible = _result;
        }

        /// <summary>
        /// Code to show status of messages on status bar
        /// </summary>
        /// <param name="Status"></param>
        public static void UpdateStatus(string Status)
        {
            frmCalcPro.Instance.tsStatus.Caption = Status;
            frmCalcPro.Instance.tmrStatus.Start();
        }

        /// <summary>
        /// Overrided method to handle keys on parent form
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Alt | Keys.F5))
            {
                nbDeletePosition_ItemClick(null, null);
                return true;
            }
            else if (keyData == Keys.F4)
            {
                nbCopyPosition_ItemClick(null, null);
                return true;
            }
            else if (keyData == (Keys.F5))
                btnRefreshProject_ItemClick(null, null);
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion

        private void barMdiChildrenListItemProject_ListItemClick(object sender, ListItemClickEventArgs e)
        {
            
            try
            {
                ShowForm(this.ActiveMdiChild as DevExpress.XtraBars.Ribbon.RibbonForm,false );
            }
            catch
            {

            }
           

        }
    }
}