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
using BL;

namespace CalcPro
{
    public partial class frmSelectAccessories : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// This form is to show and enter quantities for accessories while assigning to positions
        /// </summary>

        #region Varibales
        private BArticles ObjBArticle = null;
        private EArticles _ObjEArticle = null;
        public bool _ISSave = false;
        public EArticles ObjEArticle
        {
            get { return _ObjEArticle; }
            set { _ObjEArticle = value; }
        }
        #endregion

        #region Constructors
        public frmSelectAccessories()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        private void frmSelectAccessories_Load(object sender, EventArgs e)
        {
            try
            {
                gcArticles.DataSource = _ObjEArticle.dtAccessories;
                gvArticles.SetRowCellValue(0, "A", _ObjEArticle.A);
                gvArticles.SetRowCellValue(0, "B", _ObjEArticle.B);
                gvArticles.SetRowCellValue(0, "L", _ObjEArticle.L);
                gvArticles.FocusedColumn = gvArticles.Columns["MENGE"];
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _ISSave = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gvArticles_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.Enter)
                {
                    if (gvArticles.FocusedRowHandle > 0 && gvArticles.GetFocusedRowCellValue("WG") != null)
                    {
                        decimal dv = 0;
                        if (decimal.TryParse(Convert.ToString(gvArticles.GetFocusedRowCellValue("WG")), out dv) && dv > 0)
                        {
                            if (ObjBArticle == null)
                                ObjBArticle = new BArticles();
                            _ObjEArticle.ChildWG = Convert.ToString(gvArticles.GetFocusedRowCellValue("WG"));
                            _ObjEArticle.ChildWA = Convert.ToString(gvArticles.GetFocusedRowCellValue("WA"));
                            _ObjEArticle.ChildWI = Convert.ToString(gvArticles.GetFocusedRowCellValue("WI"));
                            ObjBArticle.GetAccessoryDimension(_ObjEArticle);
                            if (_ObjEArticle.dtArt != null && _ObjEArticle.dtArt.Rows.Count > 0)
                            {
                                EPosition ObjEPosition = new EPosition();
                                if (_ObjEArticle.dtArt.Rows.Count == 1)
                                {
                                    string stA = Convert.ToString(_ObjEArticle.dtArt.Rows[0]["A"]);
                                    string stB = Convert.ToString(_ObjEArticle.dtArt.Rows[0]["B"]);
                                    string stL = Convert.ToString(_ObjEArticle.dtArt.Rows[0]["L"]);
                                    if (_ObjEArticle.A == stA && _ObjEArticle.B == stB && _ObjEArticle.L == stL)
                                    {
                                        gvArticles.SetFocusedRowCellValue("A", stA);
                                        gvArticles.SetFocusedRowCellValue("B", stB);
                                        gvArticles.SetFocusedRowCellValue("L", stL);
                                    }
                                    else
                                    {
                                        ObjEPosition.dtDimensions = _ObjEArticle.dtArt;
                                        frmSelectDimension Obj = new frmSelectDimension();
                                        Obj.ObjEPosition = ObjEPosition;
                                        Obj.ShowInTaskbar = false;
                                        Obj.ShowDialog();
                                        gvArticles.SetFocusedRowCellValue("A", ObjEPosition.Dim1);
                                        gvArticles.SetFocusedRowCellValue("B", ObjEPosition.Dim2);
                                        gvArticles.SetFocusedRowCellValue("L", ObjEPosition.Dim3);
                                    }
                                }
                                else
                                {
                                    ObjEPosition.dtDimensions = _ObjEArticle.dtArt;
                                    frmSelectDimension Obj = new frmSelectDimension();
                                    Obj.ObjEPosition = ObjEPosition;
                                    Obj.ShowInTaskbar = false;
                                    Obj.ShowDialog();
                                    gvArticles.SetFocusedRowCellValue("A", ObjEPosition.Dim1);
                                    gvArticles.SetFocusedRowCellValue("B", ObjEPosition.Dim2);
                                    gvArticles.SetFocusedRowCellValue("L", ObjEPosition.Dim3);
                                }
                            }
                        }
                    }
                    gvArticles.MoveNext();
                }
            }
            catch (Exception ex) { }
        }
        #endregion
    }
}