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
    public partial class frmProjectArticles : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// This form is to show list of project specific articles
        /// </summary>
        #region Varibales
        private EPosition ObjEPosition = null;
        #endregion

        #region Constructors
        public frmProjectArticles(EPosition _ObjEPosition)
        {
            InitializeComponent();
            ObjEPosition = _ObjEPosition;
        }
        #endregion

        #region Events
        private void frmProjectArticles_Load(object sender, EventArgs e)
        {
            try
            {
                DPosition ObjDPosition = new DPosition();
                ObjDPosition.GetProjectArticles(ObjEPosition);
                if (ObjEPosition.dtProjectArticles != null)
                    gcProjectArticles.DataSource = ObjEPosition.dtProjectArticles;
            }
            catch (Exception ex){}
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}