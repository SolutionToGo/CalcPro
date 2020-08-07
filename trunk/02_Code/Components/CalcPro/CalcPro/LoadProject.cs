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
using BL;
using EL;
using System.Threading;
using System.Xml;
using System.IO;
using System.Configuration;
using System.Diagnostics;
using DevExpress.XtraBars;
using DevExpress.XtraSplashScreen;
using log4net;
using System.Runtime.InteropServices;

namespace CalcPro
{
    public partial class frmLoadProject : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// This form is to show list if projects
        /// </summary>
        #region Varibales
        BProject ObjBProject = new BProject();
        EProject ObjEProject = new EProject();
        DataTable dtPRojectList;
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Constructors
        public frmLoadProject()
        {
            InitializeComponent();
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                SplashScreenManager.Default.SetWaitFormDescription("Loading...");
                BindData();
            }
            catch (Exception ex) { logger.Error(ex.Message, ex); }
            finally { SplashScreenManager.CloseForm(); }
        }
        #endregion

        #region Events

        private void btnLoadProject_Click(object sender, EventArgs e)
        {
            LoadProject(false);
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            LoadProject(true);
        }
        private void frmLoadProject_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.Escape)
                {
                    this.Close();
                }

            }
            catch (Exception ex) { }
        }
        private void frmLoadProject_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                btnLoadProject_Click(null, null);
            }
        }

        private void frmLoadProject_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (frmCalcPro.Instance.MdiChildren.Count() == 1)
            {
                frmCalcPro.Instance.SetPictureBoxVisible(true);
            }
        }

        private void dgProjectSearch_DoubleClick(object sender, EventArgs e)
        {
            LoadProject(false);
        }

        private void dgProjectSearch_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            try
            {
                if (e.HitInfo.InRow)
                {
                    e.Menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("Löschen", gvDeleteProject_Click));
                    e.Menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("Kopieren", gvCopyProject_Click));
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gvDeleteProject_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgProjectSearch.GetFocusedRowCellValue("ProjectID") != null)
                {
                    int IValue = 0;
                    if(int.TryParse(Convert.ToString(dgProjectSearch.GetFocusedRowCellValue("ProjectID")),out IValue))
                    {
                        var dlgResult = XtraMessageBox.Show("Sind Sie sicher, dass Sie dieses Projekt unwiderruflich löschen möchten?", "Frage", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (Convert.ToString(dlgResult) == "Yes")
                        {
                            string _PrNr = dgProjectSearch.GetFocusedDataRow()["ProjectNumber"].ToString();
                            if (ObjBProject == null)
                                ObjBProject = new BProject();
                            ObjBProject.DeleteProject(IValue);
                            dgProjectSearch.DeleteSelectedRows();
                            foreach (Form form in Application.OpenForms)
                            {
                                if (form.Text == _PrNr)
                                {
                                    Utility.Isclose = true;
                                    form.Close();
                                    break;
                                }
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

        private void gvCopyProject_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgProjectSearch.GetFocusedRowCellValue("ProjectID") != null)
                {
                    int IValue = 0;
                    if (int.TryParse(Convert.ToString(dgProjectSearch.GetFocusedRowCellValue("ProjectID")), out IValue))
                    {
                        frmCopyProject Obj = new frmCopyProject();
                        Obj.ShowDialog();
                        if (!string.IsNullOrEmpty(Obj._NewProjectNumber))
                        {
                            if (ObjBProject == null)
                                ObjBProject = new BProject();
                            if (ObjEProject == null)
                                ObjEProject = new EProject();
                            ObjEProject.ProjectID = IValue;
                            ObjEProject.UserID = Utility.UserID;
                            ObjEProject.ProjectNumber = Obj._NewProjectNumber;
                            ObjEProject = ObjBProject.CopyProject(ObjEProject);
                            BindData();
                            Utility.Setfocus(dgProjectSearch, "ProjectID", ObjEProject.ProjectID);
                        }
                    }
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
        /// Code to fetch list projects from database and bind to grid control
        /// </summary>
        public void BindData()
        {
            try
            {

                logger.Info("Transaction Started");
                ObjBProject.GetProjectList(ObjEProject);
                logger.Info("Transaction Completed");

                logger.Info("Grid Binding Started");
                dtPRojectList = ObjEProject.dtProjectList;
                gcProjectSearch.DataSource = dtPRojectList;
                logger.Info("Grid Binding completed");
            }
            catch (Exception ex)
            {
                if (Utility._IsGermany == true)
                    throw new Exception("Die Projektübersicht konnte nicht generiert werden");
                else
                {
                    throw new Exception("Failed to retreive the Project List");
                }
            }
        }

        /// <summary>
        /// Code to load and project in new window  and copy project into another project
        /// </summary>
        /// <param name="IsCopy"></param>
        private void LoadProject(bool IsCopy)
        {
            try
            {

                if (gcProjectSearch != null && dgProjectSearch != null && dgProjectSearch.GetFocusedDataRow() != null
                    && dgProjectSearch.GetFocusedDataRow()["ProjectId"] != null)
                {
                    int ProjectID = 0;
                    if (int.TryParse(dgProjectSearch.GetFocusedDataRow()["ProjectId"].ToString(), out ProjectID))
                    {
                        string _PrNr = dgProjectSearch.GetFocusedDataRow()["ProjectDescription"] + " - " + dgProjectSearch.GetFocusedDataRow()["ProjectNumber"];
                        foreach (Form form in Application.OpenForms)
                        {
                            if (form.Text == _PrNr)
                            {
                                form.Activate();
                                return;
                            }
                        }
                        frmProject Obj = new frmProject();
                        Obj.ProjectID = ProjectID;
                        Obj.IsCopy = IsCopy;
                        Obj.MdiParent = this.MdiParent;
                        //this.Close();
                        Obj.Show();

                    }
                    else
                    {
                        throw new Exception("Invalid Project Selected");
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }
        #endregion

        
    }
}