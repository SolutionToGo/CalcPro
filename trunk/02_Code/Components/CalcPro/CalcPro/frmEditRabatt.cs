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
    public partial class frmEditRabatt : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// This form is for editing existing rabatt
        /// </summary>

        #region Variables
        private EArticles ObjEArticle = null;
        private bool IsCopy = false;
        #endregion

        #region Constructor

        public frmEditRabatt(EArticles _ObjEArticle, bool _IsCopy)
        {
            InitializeComponent();
            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;
            ObjEArticle = _ObjEArticle;
            IsCopy = _IsCopy;
        }

        #endregion

        #region Events
        private void frmEditRabatt_Load(object sender, EventArgs e)
        {
            if (IsCopy)
                dtpValidityDate.Enabled = true;

            txtRabatt.Text = ObjEArticle.Rabatt;
            txtMulti1.Text = Convert.ToString(ObjEArticle.Multi1);
            txtMulti2.Text = Convert.ToString(ObjEArticle.Multi2);
            txtMulti3.Text = Convert.ToString(ObjEArticle.Multi3);
            txtMulti4.Text = Convert.ToString(ObjEArticle.Multi4);
            dtpValidityDate.DateTime = ObjEArticle.ValidityDate;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                decimal DValue = 0;
                DArticles ObjDArticles = new DArticles();
                ObjEArticle.Rabatt = txtRabatt.Text;
                ObjEArticle.ValidityDate = dtpValidityDate.DateTime;
                if (decimal.TryParse(txtMulti1.Text,out DValue))
                    ObjEArticle.Multi1 = DValue;
                else
                    ObjEArticle.Multi1 = 1;

                if (decimal.TryParse(txtMulti2.Text, out DValue))
                    ObjEArticle.Multi2 = DValue;
                else
                    ObjEArticle.Multi2 = 1;

                if (decimal.TryParse(txtMulti3.Text, out DValue))
                    ObjEArticle.Multi3 = DValue;
                else
                    ObjEArticle.Multi3 = 1;

                if (decimal.TryParse(txtMulti4.Text, out DValue))
                    ObjEArticle.Multi4 = DValue;
                else
                    ObjEArticle.Multi4 = 1;

                if (!IsCopy)
                    ObjEArticle.Flag = "E";
                else
                    ObjEArticle.Flag = "A";

                ObjEArticle = ObjDArticles.UpdateRabatt(ObjEArticle);
                ObjEArticle.IsContinue = true;
                this.Close();
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtRabatt_Enter(object sender, EventArgs e)
        {
            try
            {
                var edit = ((DevExpress.XtraEditors.TextEdit)sender);
                BeginInvoke(new MethodInvoker(() =>
                {
                    edit.SelectionStart = 0;
                    edit.SelectionLength = edit.Text.Length;
                }));
            }
            catch (Exception ex) { }
        }
        #endregion
    }
}