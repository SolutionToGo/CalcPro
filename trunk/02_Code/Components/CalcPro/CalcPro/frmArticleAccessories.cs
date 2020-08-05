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
using BL;
using EL;

namespace CalcPro
{
    public partial class frmArticleAccessories : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// This form contains article accessories information
        /// </summary>

        #region Variables

        BArticles ObjBArticles = null;
        EArticles ObjEArticles = null;

        #endregion

        #region Constructors
        public frmArticleAccessories()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        private void frmArticleAccessories_Load(object sender, EventArgs e)
        {
          
            try
            {
                if (ObjBArticles == null)
                    ObjBArticles = new BArticles();
                if (ObjEArticles == null)
                    ObjEArticles = new EArticles();
                ObjBArticles.GetArticleForAccessories(ObjEArticles);
                gcArticles.DataSource = ObjEArticles.dtArt;
                
            }
            catch (Exception ex){ Utility.ShowError(ex); }
        }

        private void btnAddAccessory_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                if(gvArticles.FocusedRowHandle >= 0 && gvArticles.GetFocusedRowCellValue("WIID") != null)
                {
                    ObjEArticles = new EArticles();
                    frmAddAccessory Obj = new frmAddAccessory(ObjEArticles);
                    Obj.ShowDialog();
                    if (ObjEArticles.IsContinue)
                    {
                        int iv = 0;
                        if (int.TryParse(Convert.ToString(gvArticles.GetFocusedRowCellValue("WIID")), out iv))
                        {
                            ObjEArticles.ParentID = iv;
                            ObjEArticles.UserID = Utility.UserID;
                            ObjBArticles.SaveArticleMapping(ObjEArticles);
                            if (Convert.ToString(gvArticles.GetFocusedRowCellValue("HasAccessories")) == "NO")
                                gvArticles.SetRowCellValue(gvArticles.FocusedRowHandle, "HasAccessories", "YES");
                            gcAccessories.DataSource = ObjEArticles.dtAccessories;
                            Utility.Setfocus(gvAccessories, "AccessoriesID", ObjEArticles.AccessoriesID);
                        }
                    }
                }
            }
            catch (Exception ex){ Utility.ShowError(ex); }
        }

        private void gvArticles_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try
            {
                if (gvArticles.FocusedRowHandle >= 0 && gvArticles.GetFocusedRowCellValue("WIID") != null)
                {
                    if (Convert.ToString(gvArticles.GetFocusedRowCellValue("HasAccessories")) == "YES")
                    {
                        ObjEArticles = new EArticles();
                        int iv = 0;
                        if (int.TryParse(Convert.ToString(gvArticles.GetFocusedRowCellValue("WIID")), out iv))
                        {
                            ObjEArticles.ParentID = iv;
                            ObjEArticles = ObjBArticles.GetAccessories(ObjEArticles);
                            gcAccessories.DataSource = ObjEArticles.dtAccessories;
                        }
                    }
                    else
                    {
                        gcAccessories.DataSource = null;
                    }
                }
            }
            catch (Exception ex) { Utility.ShowError(ex); }
        }

        private void btnDelete_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                if (gvAccessories.FocusedRowHandle >= 0 && gvAccessories.GetFocusedRowCellValue("AccessoriesID") != null)
                {
                    ObjEArticles = new EArticles();
                    int iv = 0;
                    if (int.TryParse(Convert.ToString(gvAccessories.GetFocusedRowCellValue("AccessoriesID")), out iv))
                    {
                        ObjEArticles.AccessoriesID = iv;
                        ObjEArticles = ObjBArticles.DeleteAccessory(ObjEArticles);
                        gvAccessories.DeleteRow(gvAccessories.FocusedRowHandle);
                        if(gvAccessories.RowCount == 0)
                            gvArticles.SetRowCellValue(gvArticles.FocusedRowHandle, "HasAccessories", "NO");
                    }
                }
            }
            catch (Exception ex){ Utility.ShowError(ex); }
        }

        private void frmArticleAccessories_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F4)
                btnAddAccessory_ButtonClick(null, null);
            else if (e.KeyData == Keys.Escape)
                this.Close();
        }
        #endregion

        private void RbtnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

       

     
    }
}