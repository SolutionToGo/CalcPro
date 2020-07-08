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
    public partial class frmViewcommentory : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// This form is to view the project commentary section 
        /// </summary>
        #region Global variables 
        public string LongDescription = string.Empty;
        public bool _IsSave = false;
        #endregion

        #region Constructors
        public frmViewcommentory()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                LongDescription = txtDescription.RtfText;
                _IsSave = true;
                this.Close();
            }
            catch (Exception ex) { }
        }

        private void frmViewcommentory_Load(object sender, EventArgs e)
        {
            try
            {
                txtDescription.RtfText = LongDescription;
            }
            catch (Exception ex){}
        }

        private void frmViewcommentory_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.F1)
                    btnSave_Click(null, null);
            }
            catch (Exception ex) { }
        }
        #endregion
    }
}