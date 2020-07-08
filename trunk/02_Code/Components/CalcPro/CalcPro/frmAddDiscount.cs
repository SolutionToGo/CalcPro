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
    public partial class frmAddDiscount : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// This form is for adding discount positions and popup when user clicks on project meta data
        /// </summary>

        #region Varibales
        EProject ObjEProject = null;
        #endregion

        #region Constructors
        public frmAddDiscount(EProject _ObjEProject)
        {
            ObjEProject = _ObjEProject;
            InitializeComponent();
        }
        #endregion

        #region Events
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtFromOZ.Text))
                    throw new Exception("From OZ Cannot be Empty");
                if (string.IsNullOrEmpty(txtToOZ.Text))
                    throw new Exception("To OZ Cannot be Empty");
                if (string.IsNullOrEmpty(txtShortdescription.Text))
                    throw new Exception("Kurz Text Cannot be Empty");
                if (txtFromOZ.Text.Length > 0)
                {
                    char cLastCharacter = txtFromOZ.Text[txtFromOZ.Text.Length - 1];
                    if (cLastCharacter == '.')
                        ObjEProject.FromOZ = txtFromOZ.Text.Replace(",", ".");
                    else
                        ObjEProject.FromOZ = txtFromOZ.Text.Replace(",", ".") + ".";
                }
                if (txtToOZ.Text.Length > 0)
                {
                    char cLastCharacter = txtToOZ.Text[txtToOZ.Text.Length - 1];
                    if (cLastCharacter == '.')
                        ObjEProject.ToOZ = txtToOZ.Text.Replace(",", ".");
                    else
                        ObjEProject.ToOZ = txtToOZ.Text.Replace(",", ".") + ".";
                }
                decimal dValue = 0;
                if (decimal.TryParse(txtDiscount.Text, out dValue))
                    ObjEProject.Discount = dValue;
                else
                    ObjEProject.Discount = dValue;
                ObjEProject.ShortDescription = txtShortdescription.Rtf;
                ObjEProject.IsSave = true;
                this.Close();
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btbCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFromOZ_Enter(object sender, EventArgs e)
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

        private void txtToOZ_Enter(object sender, EventArgs e)
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