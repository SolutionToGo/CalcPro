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

namespace CalcPro
{
    public partial class frmNewOZ : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        ///  This form is to suggest and enter new oz while copying a positions
        /// </summary>
        #region Varibales
        public string strNewOZ = string.Empty;
        public string LVRaster = string.Empty;
        public bool IsSave = false;
        #endregion

        #region Constructors
        public frmNewOZ()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        private void frmNewOZ_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
                this.Close();
            else if(e.KeyChar == (char)Keys.Enter)
            {
                btnOk_Click(null, null);
            }
        }

        private void frmNewOZ_Load(object sender, EventArgs e)
        {

            try
            {
                string[] Levels = LVRaster.Split('.');
                int Count = Levels.Length;
                if (Count >= 2)
                {
                    int _Length = Levels[Count - 2].Length;
                    txtNewOZ.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
                    txtNewOZ.Properties.Mask.EditMask = "[A-Z0-9]{1," + _Length + "}((\\.)[A-Za-z0-9]{0,1})?";
                    txtNewOZ.Properties.Mask.UseMaskAsDisplayFormat = true;
                }

                txtNewOZ.Text = strNewOZ;
            }
            catch (Exception ex){}
            


        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            IsSave = true;
            strNewOZ = txtNewOZ.Text;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtNewOZ_Enter(object sender, EventArgs e)
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