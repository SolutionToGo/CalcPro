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
using DataAccess;

namespace CalcPro
{
    public partial class frmAddProjectArticles : DevExpress.XtraEditors.XtraForm
    {

        /// <summary>
        /// This Form is to saving project specific articles While assigning
        /// </summary>
        
        #region Varibales
        private EPosition ObjEPosition = null;
        public bool IsContinue = false;
        #endregion

        #region Constructors
        public frmAddProjectArticles(EPosition _ObjEPosition)
        {
            InitializeComponent();
            ObjEPosition = _ObjEPosition;
            txtWG.Text = ObjEPosition.WG;
            txtWA.Text = ObjEPosition.WA;
            txtWG.Enabled = false;
            txtWA.Enabled = false;
        }
        #endregion

        #region Events
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!dxValidationProvider1.Validate())
                    return;
                ObjEPosition.WG = txtWG.Text;
                ObjEPosition.WGDescription = txtWGDesc.Text;
                ObjEPosition.WA = txtWA.Text;
                ObjEPosition.WADescription = txtWADesc.Text;
                DPosition ObjDPosition = new DPosition();
                ObjDPosition.SaveProjectArticle(ObjEPosition);
                IsContinue = true;
                this.Close();
            }
            catch (Exception ex) { Utility.ShowError(ex); }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}