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
    public partial class frmMergeArticle : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        ///  This form is to Merge an article with existing supplier proposal
        /// </summary>
        #region Varibales
        ESupplier ObjESupplier = null;
        BSupplier ObjBSupplier = null;
        #endregion

        #region Constructors
        public frmMergeArticle(ESupplier _ObjESupplier)
        {
            InitializeComponent();
            ObjESupplier = _ObjESupplier;
        }
        #endregion

        #region Events
        private void frmMergeArticle_Load(object sender, EventArgs e)
        {
            try
            {
                gcArticles.DataSource = ObjESupplier.dtProposalArticles;
                gvArticles_RowClick(null, null);
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gvArticles_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try
            {
                if (gvArticles.FocusedRowHandle >= 0)
                {
                    ObjESupplier.dtArticlesMerge = ObjESupplier.dtArticleID.Copy();
                    DataRow dr = ObjESupplier.dtArticlesMerge.NewRow();
                    dr["ID"] = gvArticles.GetFocusedRowCellValue("WGID");
                    ObjESupplier.dtArticlesMerge.Rows.Add(dr);
                    if (ObjBSupplier == null)
                        ObjBSupplier = new BSupplier();
                    ObjBSupplier.GetSuppliersForProposalMerge(ObjESupplier);
                    chkSuppliersList.DataSource = ObjESupplier.dtSupplierForproposal;
                    chkSuppliersList.DisplayMember = "ShortName";
                    chkSuppliersList.ValueMember = "SupplierID";
                    DataRowView row = null;
                    for (int i = 0; i < ObjESupplier.dtSupplierForproposal.Rows.Count; i++)
                    {
                        row = chkSuppliersList.GetItem(i) as DataRowView;
                        if (Convert.ToInt16(row["ItemChecked"]) == 1)
                            chkSuppliersList.SetItemChecked(i, true);
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvArticles.FocusedRowHandle >= 0)
                {
                    if (ObjBSupplier == null)
                        ObjBSupplier = new BSupplier();
                    ObjESupplier.dtSupplierToDB = new DataTable();
                    ObjESupplier.dtSupplierToDB.Columns.Add("SupplierID");
                    DataRowView row = null;
                    DataRow dr = null;
                    foreach (object item in chkSuppliersList.CheckedItems)
                    {
                        row = item as DataRowView;
                        dr = ObjESupplier.dtSupplierToDB.NewRow();
                        dr["SupplierID"] = row["SupplierID"];
                        ObjESupplier.dtSupplierToDB.Rows.Add(dr);
                    }
                    ObjBSupplier.SupplierProposalMerge(ObjESupplier);
                    ObjESupplier.IsActive = true;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void chkSuppliersList_GetItemEnabled(object sender, DevExpress.XtraEditors.Controls.GetItemEnabledEventArgs e)
        {
            try
            {
                CheckedListBoxControl control = sender as CheckedListBoxControl;
                DataRowView row = control.GetItem(e.Index) as DataRowView;
                if (Convert.ToInt16(row["ItemChecked"]) == 1)
                    e.Enabled = false;
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void chkSuppliersList_ItemChecking(object sender, DevExpress.XtraEditors.Controls.ItemCheckingEventArgs e)
        {
            try
            {
                if (Convert.ToBoolean(e.NewValue) == true && chkSuppliersList.CheckedItems.Count >= 8)
                    e.Cancel = true;
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }
        #endregion
    }
}