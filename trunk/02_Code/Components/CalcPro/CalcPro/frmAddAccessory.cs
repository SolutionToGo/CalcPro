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

namespace CalcPro
{
    public partial class frmAddAccessory : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// This is a popup form for adding accesories
        /// </summary>

        #region Varibales
        EArticles ObjEArticles = null;
        #endregion

        #region Constructors
        public frmAddAccessory(EArticles _ObjEArticles)
        {
            InitializeComponent();
            ObjEArticles = _ObjEArticles;
        }
        #endregion

        #region Events
        private void frmAddAccessory_Load(object sender, EventArgs e)
        {
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!dxValidationProvider1.Validate())
                    return;
                ObjEArticles.WG = txtWG.Text;
                ObjEArticles.WA = txtWA.Text;
                ObjEArticles.WI = txtWI.Text;
                ObjEArticles.IsContinue = true;
                this.Close();
            }
            catch (Exception ex){}
        }
        #endregion
    }
}