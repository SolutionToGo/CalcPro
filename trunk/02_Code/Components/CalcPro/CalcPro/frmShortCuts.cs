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
    public partial class frmShortCuts : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// This form is to show application shortcuts with static data
        /// </summary>

        #region Constructors
        public frmShortCuts()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        private void frmShortCuts_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.Escape)
                    this.Close();
            }
            catch (Exception ex) { }
        }

        #endregion
    }
}