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
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DataAccess;

namespace CalcPro
{
    public partial class frmRabattGroup : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        /// <summary>
        /// This form is to add , edit and view the rabatt
        /// User can map typ with rabatt
        /// user can save rabatt with multiple validity dates
        /// </summary>
        #region Varibales
        BArticles ObjBArticle = null;
        EArticles ObjEArticle = null;
        DArticles ObjDArticle = null;
        #endregion

        #region Constructors
        public frmRabattGroup()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmRabattGroup_Load(object sender, EventArgs e)
        {
            try
            {
                if (Utility.ArticleDataAccess == "7")
                    RbtnAdd.Enabled = false;

                if (ObjEArticle == null)
                    ObjEArticle = new EArticles();
                if (ObjBArticle == null)
                    ObjBArticle = new BArticles();
                ObjEArticle = ObjBArticle.GetRabatt(ObjEArticle);
                BindRabattData();
                dateEditValidityDate.DateTime = DateTime.Now;
                dateEditValidityDate.Properties.MinValue = DateTime.Now;
                txtMulti1.Text = "1";
                txtMulti2.Text = "1";
                txtMulti3.Text = "1";
                txtMulti4.Text = "1";
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gvRabatt_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                int _IDValue = -1;
                if (gvRabatt.FocusedColumn != null && gvRabatt.GetFocusedRowCellValue("RabattID") != null)
                {
                    if (int.TryParse(Convert.ToString(gvRabatt.GetFocusedRowCellValue("RabattID")), out _IDValue))
                    {
                        if (ObjEArticle == null)
                            ObjEArticle = new EArticles();
                        ObjEArticle.RID = _IDValue;
                        string stRabatt = Convert.ToString(gvRabatt.GetFocusedRowCellValue("Rabatt"));
                        layoutControlItem1.Text = "Typs Associated with Rabbat : " + stRabatt;
                        ObjEArticle = ObjBArticle.GetTypByRabatt(ObjEArticle);
                        gcTyp.DataSource = ObjEArticle.dtTypID;
                        gvTyp.Columns["RabattTypMapID"].Visible = false;
                        gvTyp.BestFitColumns();
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtMulti1_Leave(object sender, EventArgs e)
        {
            TextEdit textbox = (TextEdit)sender;
            decimal dValue = 0;
            if (textbox.Text == string.Empty || decimal.TryParse(textbox.Text, out dValue))
            {
                if (dValue == 0)
                {
                    textbox.Text = "1";
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int iValue = 0;
                if (ObjBArticle == null)
                    ObjBArticle = new BArticles();
                if (ObjEArticle == null)
                    ObjEArticle = new EArticles();
                ParseRabattDetails();
                ObjEArticle = ObjBArticle.SaveRabatt(ObjEArticle);
                iValue = ObjEArticle.RabattID;
                BindRabattData();
                Utility.Setfocus(gvRabatt, "RabattID", iValue);
                ObjEArticle.RabattID = -1;
                if (Utility._IsGermany == true)
                    frmCalcPro.UpdateStatus("Vorgang abgeschlossen: Speichern der Rabattgruppe(n)");
                else
                    frmCalcPro.UpdateStatus("Rabatt group Saved Successfully");
            }
            catch (Exception ex){Utility.ShowError(ex);}
        }

        private void btnAddtype_Click(object sender, EventArgs e)
        {
            try
            {
                frmTypList Obj = new frmTypList();
                Obj.ShowDialog();
                if(Obj.IScontinue)
                {
                    ObjEArticle = new EArticles();
                    DArticles ObjDArticles = new DArticles(); ;
                    int ivalue = 0;
                    if(int.TryParse(Convert.ToString(gvRabatt.GetFocusedRowCellValue("RabattID")),out ivalue))
                    {
                        ObjEArticle.RabattID = ivalue;
                        ObjEArticle.TypID = Obj.TypID;
                        ObjEArticle = ObjDArticles.SaveTypRabattMapping(ObjEArticle);
                        gcTyp.DataSource = ObjEArticle.dtTypID;
                        gvTyp.Columns["RabattTypMapID"].Visible = false;
                        Utility.Setfocus(gvTyp, "RabattTypMapID", ObjEArticle.RabattTypID);
                        ObjEArticle.RabattTypID = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gcRabatt_ProcessGridKey(object sender, KeyEventArgs e)
        {
            try
            {
                var grid = sender as GridControl;
                var view = grid.FocusedView as GridView;
                if (e.KeyData == Keys.Delete)
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gvRabatt_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                int IValue = 0; decimal DValue = 0;
                if (int.TryParse(Convert.ToString(gvRabatt.GetFocusedRowCellValue("RabattID")), out IValue))
                {
                    ObjEArticle.RabattID = IValue;
                    ObjEArticle.Rabatt = Convert.ToString(gvRabatt.GetFocusedRowCellValue("Rabatt"));
                    if (decimal.TryParse(Convert.ToString(gvRabatt.GetFocusedRowCellValue("Multi1")), out DValue))
                        ObjEArticle.Multi1 = DValue;
                    else
                        ObjEArticle.Multi1 = 1;

                    if (decimal.TryParse(Convert.ToString(gvRabatt.GetFocusedRowCellValue("Multi2")), out DValue))
                        ObjEArticle.Multi2 = DValue;
                    else
                        ObjEArticle.Multi2 = 1;

                    if (decimal.TryParse(Convert.ToString(gvRabatt.GetFocusedRowCellValue("Multi3")), out DValue))
                        ObjEArticle.Multi3 = DValue;
                    else
                        ObjEArticle.Multi3 = 1;

                    if (decimal.TryParse(Convert.ToString(gvRabatt.GetFocusedRowCellValue("Multi4")), out DValue))
                        ObjEArticle.Multi4 = DValue;
                    else
                        ObjEArticle.Multi4 = 1;

                    ObjEArticle.ValidityDate = gvRabatt.GetFocusedRowCellValue("Gültigkeit Datum") == DBNull.Value ?
                        DateTime.Now : Convert.ToDateTime(gvRabatt.GetFocusedRowCellValue("Gültigkeit Datum"));

                    frmEditRabatt Obj = new frmEditRabatt(ObjEArticle, false);
                    Obj.ShowDialog();
                    if (ObjEArticle.IsContinue)
                    {
                        ObjEArticle.IsContinue = false;
                        gcRabatt.DataSource = ObjEArticle.dtRabatt;
                        Utility.Setfocus(gvRabatt, "RabattID", ObjEArticle.RabattID);
                        ObjEArticle.RabattID = -1;
                    }
                }
            }
            catch (Exception EX)
            {
                Utility.ShowError(EX);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if(ObjEArticle == null)
                ObjEArticle = new EArticles();
            ObjEArticle.RabattID = -1;
            txtRabatt.Text = string.Empty;
            txtMulti1.Text = "1";
            txtMulti2.Text = "1";
            txtMulti3.Text = "1";
            txtMulti4.Text = "1";
        }

        private void btnCopyRabatt_Click(object sender, EventArgs e)
        {
            try
            {
                int IValue = 0; decimal DValue = 0;
                if(int.TryParse(Convert.ToString(gvRabatt.GetFocusedRowCellValue("RabattID")),out IValue))
                {
                    ObjEArticle.RabattID = IValue;
                    ObjEArticle.Rabatt = Convert.ToString(gvRabatt.GetFocusedRowCellValue("Rabatt"));
                    if(decimal.TryParse(Convert.ToString(gvRabatt.GetFocusedRowCellValue("Multi1")),out DValue))
                        ObjEArticle.Multi1 = DValue;
                    else
                        ObjEArticle.Multi1 = 1;

                    if (decimal.TryParse(Convert.ToString(gvRabatt.GetFocusedRowCellValue("Multi2")), out DValue))
                        ObjEArticle.Multi2 = DValue;
                    else
                        ObjEArticle.Multi2 = 1;

                    if (decimal.TryParse(Convert.ToString(gvRabatt.GetFocusedRowCellValue("Multi3")), out DValue))
                        ObjEArticle.Multi3 = DValue;
                    else
                        ObjEArticle.Multi3 = 1;
                    
                    if (decimal.TryParse(Convert.ToString(gvRabatt.GetFocusedRowCellValue("Multi4")), out DValue))
                        ObjEArticle.Multi4 = DValue;
                    else
                        ObjEArticle.Multi4 = 1;

                    ObjEArticle.ValidityDate = gvRabatt.GetFocusedRowCellValue("Gültigkeit Datum") == DBNull.Value ?
                        DateTime.Now : Convert.ToDateTime(gvRabatt.GetFocusedRowCellValue("Gültigkeit Datum"));

                    frmEditRabatt Obj = new frmEditRabatt(ObjEArticle,true);
                    Obj.ShowDialog();
                    if(ObjEArticle.IsContinue)
                    {
                        ObjEArticle.IsContinue = false;
                        gcRabatt.DataSource = ObjEArticle.dtRabatt;
                        Utility.Setfocus(gvRabatt, "RabattID", ObjEArticle.RabattID);
                        ObjEArticle.RabattID = -1;
                    }
                }
            }
            catch (Exception EX)
            {
                Utility.ShowError(EX);
            }
        }

        private void txtRabatt_Enter(object sender, EventArgs e)
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

        private void gvTyp_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            try
            {
                if (e.HitInfo.InRow)
                {
                    e.Menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("Löschen", gvDeleteRabattTypMap_Click));
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gvDeleteRabattTypMap_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvTyp.GetFocusedRowCellValue("RabattTypMapID") != null)
                {
                    int IValue = 0;
                    if (int.TryParse(Convert.ToString(gvTyp.GetFocusedRowCellValue("RabattTypMapID")), out IValue))
                    {
                        var dlgResult = XtraMessageBox.Show("Sind Sie sicher, dass Sie den ausgewählten TYP löschen möchten?", "Frage", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (Convert.ToString(dlgResult) == "Yes")
                        {
                            if (ObjEArticle == null)
                                ObjEArticle = new EArticles();
                            ObjEArticle.RabattTypID = IValue;
                            if (ObjDArticle == null)
                                ObjDArticle = new DArticles();
                            ObjDArticle.DeleteRabattTypMap(ObjEArticle);
                            gvTyp.DeleteSelectedRows();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }
        #endregion

        #region Functions

        /// <summary>
        /// Code to fetch Rabatt and typ list form database and bind to grid controls
        /// </summary>
        private void BindRabattData()
        {
            try
            {
                if (ObjEArticle == null)
                    ObjEArticle = new EArticles();
                gcRabatt.DataSource = ObjEArticle.dtRabatt;
                gvRabatt.Columns["RabattID"].Visible = false;
                gvRabatt.BestFitColumns();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Code to parse rabatt and typ details while adding or editing rabatt
        /// </summary>
        private void ParseRabattDetails()
        {
            try
            {
                decimal DValue = 1;
                DateTime dtTime = DateTime.Now;
                ObjEArticle.Rabatt = txtRabatt.Text;

                if (decimal.TryParse(txtMulti1.Text, out DValue))
                    ObjEArticle.Multi1 = DValue;
                else
                    ObjEArticle.Multi1 = 1;

                if (decimal.TryParse(txtMulti2.Text, out DValue))
                    ObjEArticle.Multi2 = DValue;
                else
                    ObjEArticle.Multi2 = 1;

                if (decimal.TryParse(txtMulti3.Text, out DValue))
                    ObjEArticle.Multi3 = DValue;
                else
                    ObjEArticle.Multi3 = 1;

                if (decimal.TryParse(txtMulti4.Text, out DValue))
                    ObjEArticle.Multi4 = DValue;
                else
                    ObjEArticle.Multi4 = 1;

                ObjEArticle.ValidityDate = dateEditValidityDate.DateTime;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        private void RbtnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btnCancel_Click(0, e);
        }

        private void RbtnReset_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btnReset_Click(0, e);
        }

        private void RbtnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btnAdd_Click(0, e);
        }

        private void RbtnAddtype_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btnAddtype_Click(0, e);
        }

        private void RbtnCopyRabatt_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btnCopyRabatt_Click(0, e);
        }

        private void frmRabattGroup_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.Escape)
                    this.Close();
            }
            catch (Exception ex) { }
        }
    }
}