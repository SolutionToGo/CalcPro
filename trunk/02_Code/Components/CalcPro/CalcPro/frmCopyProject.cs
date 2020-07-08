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
    public partial class frmCopyProject : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// This form is to Creating a copy of selected project
        /// </summary>

        #region Variables
        private bool _IsSave = false;
        public string _NewProjectNumber = string.Empty;
        #endregion

        #region Constrctors
        public frmCopyProject()
        {
            InitializeComponent();
        }
        #endregion

        #region Events

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtProjectNumber.Text.Trim()))
                    throw new Exception("Bitte geben Sie eine gültige Projektnummer ein");
                _IsSave = true;
                this.Close();
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _IsSave = false;
            this.Close();
        }

        private void frmCopyProject_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_IsSave)
                _NewProjectNumber = txtProjectNumber.Text.Trim();
            else
                _NewProjectNumber = string.Empty;
        }

        private void frmCopyProject_Load(object sender, EventArgs e)
        {

        }

        private void txtProjectNumber_Enter(object sender, EventArgs e)
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