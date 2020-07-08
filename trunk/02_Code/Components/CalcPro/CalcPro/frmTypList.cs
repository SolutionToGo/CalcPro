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
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace CalcPro
{
    public partial class frmTypList : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// This form is to show the Typ list while mapping Typ to Rabbat
        /// </summary>
        #region Local and Global variables
        EArticles ObjEArticles = null;
        DArticles ObjDArticles = null;
        public int TypID = 0;
        public bool IScontinue = false;
        #endregion

        #region Constructors
        public frmTypList()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        private void frmTypList_Load(object sender, EventArgs e)
        {
            ObjEArticles = new EArticles();
            ObjDArticles = new DArticles();
            ObjEArticles = ObjDArticles.GetTypForRabatt(ObjEArticles);
            gcTyp.DataSource = ObjEArticles.dtTyp;
            gvTyp.Columns["TypID"].Visible = false;
            gvTyp.BestFitColumns();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                int IValue = 0;
                if(int.TryParse(Convert.ToString(gvTyp.GetFocusedRowCellValue("TypID")),out IValue))
                {
                    TypID = IValue;
                    IScontinue = true;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gcTyp_ProcessGridKey(object sender, KeyEventArgs e)
        {
            var grid = sender as GridControl;
            var view = grid.FocusedView as GridView;
            if (e.KeyData == Keys.Enter)
            {
                btnOk_Click(null, null);
                e.Handled = true;
            }
        }

        private void gvTyp_DoubleClick(object sender, EventArgs e)
        {
            GridView view = (GridView)sender;
                Point pt = view.GridControl.PointToClient(Control.MousePosition);
                GridHitInfo info = view.CalcHitInfo(pt);
                if (info.InRow || info.InRowCell)
                {
                    btnOk_Click(null, null);
                }
        }
#endregion
    }
}