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
using DAL;

namespace CalcPro
{
    public partial class frmArticleSettings : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// This form is to saving the article settings ot definations for each and every project.
        /// </summary>

        #region Variables

        EProject ObjEProject = null;
        #endregion

        #region Constructors

        public frmArticleSettings(EProject _ObjEProject)
        {
            InitializeComponent();
            ObjEProject = _ObjEProject;
        }
        #endregion

        #region Events

        private void frmArticleSettings_Load(object sender, EventArgs e)
        {
            gcArticleSettings.DataSource = ObjEProject.dtArticleSettings;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DProject ObjDProject = new DProject();
                ObjEProject.UserID = Utility.UserID;
                ObjDProject.UpdateArticleSettings(ObjEProject);
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
                this.Close();
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}