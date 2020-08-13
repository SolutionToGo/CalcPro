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
using DevExpress.XtraSplashScreen;

namespace CalcPro
{
    public partial class frmViewProject : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// This is a popup form view imported project before saving into database
        /// </summary>
        #region Varibales
        private EGAEB ObjEGAEB = null;
        #endregion

        #region Constructors
        public frmViewProject(EGAEB _ObjEGAEB)
        {
            InitializeComponent();
            ObjEGAEB = _ObjEGAEB;
        }
        #endregion

        #region Events
        private void frmViewProject_Load(object sender, EventArgs e)
        {
            try
            {
                tlLVDetails.DataSource = ObjEGAEB.dsProject;
                tlLVDetails.DataMember = "Positions";
                tlLVDetails.ParentFieldName = "Parent_OZ";
                tlLVDetails.KeyFieldName = "PositionID";
                tlLVDetails.ForceInitialize();
                tlLVDetails.ExpandAll();

                txtProjectNumber.Text = ObjEGAEB.ProjectNumber.Replace("OTTO", "");
                txtProjectDescription.Text = ObjEGAEB.ProjectDescription;
                txtLVRaster.Text = ObjEGAEB.LvRaster;
                txtLVSprunge.Text = ObjEGAEB.LVSprunge.ToString();
                txtKundeName.Text = ObjEGAEB.CustomerName;
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void tlLVDetails_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            try
            {
                if (tlLVDetails.FocusedNode != null)
                    txtLAngtext.RtfText = Convert.ToString(tlLVDetails.FocusedNode["Longdiscription"]);
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
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                SplashScreenManager.Default.SetWaitFormDescription("Importieren...");
                BGAEB ObjBGAEB = new BGAEB();
                ObjEGAEB.ProjectNumber = txtProjectNumber.Text;
                ObjEGAEB.ProjectDescription = txtProjectDescription.Text;
                int Ivalue = 0;
                if (int.TryParse(txtLVSprunge.Text, out Ivalue))
                    ObjEGAEB.LVSprunge = Ivalue;
                else
                    ObjEGAEB.LVSprunge = Ivalue;
                ObjEGAEB.IsSave = true;
                ObjBGAEB.SaveeProject(ObjEGAEB);
                SplashScreenManager.CloseForm(false);
                this.Close();
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
        #endregion

        private void frmViewProject_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.Escape)
                    this.Close();
            }
            catch (Exception ex) { }
        }
    }
}