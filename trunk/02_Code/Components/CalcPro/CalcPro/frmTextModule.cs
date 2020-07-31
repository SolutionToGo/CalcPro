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
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace CalcPro
{
    public partial class frmTextModule : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// This form is to create and view text modules for title blatt
        /// </summary>

        #region Variables

        EProposal ObjEProposal = new EProposal();
        BProposal ObjBProposal = new BProposal();
        DataTable _dtContents = new DataTable();
        int _TextAreaID;
        int _CategoryID;
        int _IDValue = -1;
        #endregion

        #region CONSTRUCTOR

        public frmTextModule()
        {
            InitializeComponent();
        }      
        #endregion  

        #region EVENTS

        /// <summary>
        /// to add category for particular text area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ObjEProposal = new EProposal();
                ObjEProposal.CategoryID = -1;

                FrmCategory frm = new FrmCategory(Convert.ToInt32(_TextAreaID), ObjEProposal);
                frm.ShowDialog();
                if (frm.DialogResult == DialogResult.OK)
                {
                    BindCategories();
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        /// <summary>
        /// form load event to bind the data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTextModule_Load(object sender, EventArgs e)
        {
            try
            {
                BindTextModuleAreas();
                cmbTextArea_SelectionChangeCommitted(null, null);
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        /// <summary>
        /// to save the entered data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbCategory.Text == "")
                {
                    throw new Exception("Bitte wähle Kategorie");
                }
                if (string.IsNullOrEmpty(richEditControlContent.Text))
                {
                    throw new Exception("Bitte wähle Inhalt");
                }
                if (ObjEProposal == null)
                    ObjEProposal = new EProposal();
                ParseTextModuleDetails();
                ObjBProposal = new BProposal();
                ObjBProposal.SaveTextModule(ObjEProposal);
                cmbCategory_SelectionChangeCommitted(null,null);
               // BindTextModuleGrid();
               // Setfocus(gvContentDetails, "TextID", ObjEProposal.TextID);

            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        /// <summary>
        ///form cancel button to close form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// to load the data when we double click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvContentDetails_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (cmbTextArea.Text == "ALLGEMEIN" && Utility.GeneralTextModuleAccess == "7")
                    return;
                else if (cmbTextArea.Text == "KALKULATION" && Utility.CalculationTextModuleAccess == "7")
                    return;
                else if (cmbTextArea.Text == "RECHNUNGSLEGUNG" && Utility.InvoiceTextModuleAccess == "7")
                    return;
                GridView view = (GridView)sender;
                Point pt = view.GridControl.PointToClient(Control.MousePosition);
                GridHitInfo info = view.CalcHitInfo(pt);

                if (info.InRow || info.InRowCell)
                {
                    if (gvContentDetails.SelectedRowsCount == 0)
                    {
                        return;
                    }
                    ObjEProposal = new EProposal();
                  //  BindTextModuleAreas();
                    GetTextModuleDetails();
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }


        /// <summary>
        /// to load category data according text area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTextArea_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (cmbTextArea.Text != string.Empty)
                {
                    if (cmbTextArea.Text == "ALLGEMEIN")
                    {
                        if(Utility.GeneralTextModuleAccess == "7")
                            btnAdd.Enabled = false;
                        else
                            btnAdd.Enabled = true;
                    }
                    else if (cmbTextArea.Text == "KALKULATION")
                    {
                        if (Utility.CalculationTextModuleAccess == "7")
                            btnAdd.Enabled = false;
                        else
                            btnAdd.Enabled = true;
                    }
                    else if (cmbTextArea.Text == "RECHNUNGSLEGUNG")
                    {
                        if(Utility.InvoiceTextModuleAccess == "7")
                            btnAdd.Enabled = false;
                        else
                            btnAdd.Enabled = true;
                    }

                    if (int.TryParse(Convert.ToString(cmbTextArea.EditValue), out _TextAreaID))
                    {
                        if (_TextAreaID > 0)
                        {
                            BindCategories();
                            richEditControlContent.Text = "";
                        }
                    }                        
                }
                gcContentDetails.DataSource = null;
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        /// <summary>
        /// to load the data according to category
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCategory_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (cmbCategory.Text != string.Empty)
                {
                    if (int.TryParse(Convert.ToString(cmbCategory.EditValue), out _CategoryID))

                        if (_CategoryID > 0)
                        {
                            ObjEProposal = new EProposal();
                            ObjEProposal.TextID = -1;
                            gcContentDetails.DataSource = null;
                            richEditControlContent.Text = "";
                            ObjBProposal.GetTextModuleAreas(ObjEProposal);
                            if (ObjEProposal.dsTextModuleAreas != null)
                            {
                                DataView dvProposalContents = ObjEProposal.dsTextModuleAreas.Tables[1].DefaultView;
                                dvProposalContents.RowFilter = "TextAreaID = " + _TextAreaID + " AND CategoryID = '" + _CategoryID + "'";
                                _dtContents = dvProposalContents.ToTable();
                                gcContentDetails.DataSource = _dtContents;
                                //this.gvContentDetails.Columns[4].Visible = false;
                            }
                        }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gvContentDetails_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            try
            {
                if (e.HitInfo.InRow)
                {
                    e.Menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("Löschen", gvDeleteContent_Click));
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gvDeleteContent_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvContentDetails.FocusedRowHandle != null)
                {
                    int IVlaue = 0;
                    if (int.TryParse(Convert.ToString(gvContentDetails.GetFocusedRowCellValue("TextID")), out IVlaue))
                    {
                        if (ObjEProposal == null)
                            ObjEProposal = new EProposal();
                        ObjBProposal = new BProposal();
                        ObjBProposal.DeleteTextModuleAreas(ObjEProposal, IVlaue);
                        cmbCategory_SelectionChangeCommitted(null, null);

                        gvContentDetails.FocusedRowHandle = IVlaue;
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        #endregion

        #region METHODS

        /// <summary>
        /// to bind text module areas into combobox
        /// </summary>
        private void BindTextModuleAreas()
        {
            try
            {
                ObjBProposal.GetTextModuleAreas(ObjEProposal);
                if (ObjEProposal.dsTextModuleAreas != null)
                {
                    DataTable dt = ObjEProposal.dsTextModuleAreas.Tables[0].Copy();
                    DataView dv = dt.DefaultView;
                    if (Utility.GeneralTextModuleAccess == "9")
                    {
                        dv.RowFilter = "TextAreas <> 'ALLGEMEIN'";
                        dt = dv.ToTable().Copy();
                    }
                    if (Utility.CalculationTextModuleAccess == "9")
                    {
                        dv = dt.DefaultView;
                        dv.RowFilter = "TextAreas <> 'KALKULATION'";
                        dt = dv.ToTable().Copy();
                    }
                    if (Utility.InvoiceTextModuleAccess == "9")
                    {
                        dv = dt.DefaultView;
                        dv.RowFilter = "TextAreas <> 'RECHNUNGSLEGUNG'";
                        dt = dv.ToTable().Copy();
                    }

                    cmbTextArea.Properties.DataSource = dt;
                    cmbTextArea.Properties.DisplayMember = "TextAreas";
                    cmbTextArea.Properties.ValueMember = "TextAreaID";
                    
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// to display all text module in gridview
        /// </summary>
        private void BindTextModuleGrid()
        {
            try
            {
                ObjBProposal.GetTextModuleAreas(ObjEProposal);
                if (ObjEProposal.dsTextModuleAreas != null)
                {
                    gcContentDetails.DataSource = ObjEProposal.dsTextModuleAreas.Tables[1];
                    gvContentDetails.BestFitColumns();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// to bind category data
        /// </summary>
        private void BindCategories()
        {
            try
            {
                ObjBProposal.GetCategories(ObjEProposal, Convert.ToInt32(_TextAreaID));
                if (ObjEProposal.dsCategory != null)
                {
                    cmbCategory.Properties.DataSource = null;
                    cmbCategory.Properties.DataSource = ObjEProposal.dsCategory.Tables[0];
                    cmbCategory.Properties.DisplayMember = "CategoryName";
                    cmbCategory.Properties.ValueMember = "CategoryID";
                   // cmbCategory.Properties. = -1;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// setting the focus for selected row
        /// </summary>
        /// <param name="view"></param>
        /// <param name="_id"></param>
        /// <param name="_IdValue"></param>
        private void Setfocus(GridView view, string _id, int _IdValue)
        {
            try
            {
                if (_IdValue > -1)
                {
                    int rowHandle = view.LocateByValue(_id, _IdValue);
                    if (rowHandle != DevExpress.XtraGrid.GridControl.InvalidRowHandle)
                        view.FocusedRowHandle = rowHandle;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// assigning text module data to object
        /// </summary>
        private void ParseTextModuleDetails()
        {
            try
            {
                ObjEProposal.TextModuleArea = cmbTextArea.Text;
                ObjEProposal.Category = cmbCategory.Text;
                ObjEProposal.Contents = richEditControlContent.Text;
                ObjEProposal.TextAreaID = _TextAreaID;
                ObjEProposal.TextCategoryID = _CategoryID;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        /// to get data from gridview to textboxes.
        /// </summary>
        private void GetTextModuleDetails()
        {
            try
            {
                if (gvContentDetails.GetFocusedRowCellValue("TextID") != DBNull.Value)
                {
                    if (int.TryParse(gvContentDetails.GetFocusedRowCellValue("TextID").ToString(), out _IDValue))
                        ObjEProposal.TextID = _IDValue;
                    cmbTextArea.Text = ObjEProposal.TextModuleArea = gvContentDetails.GetFocusedRowCellValue("TextModuleArea") == DBNull.Value ? "" : gvContentDetails.GetFocusedRowCellValue("TextModuleArea").ToString();
                    cmbCategory.Text = ObjEProposal.Category = gvContentDetails.GetFocusedRowCellValue("Category") == DBNull.Value ? "" : gvContentDetails.GetFocusedRowCellValue("Category").ToString();
                    richEditControlContent.Text = ObjEProposal.Contents = gvContentDetails.GetFocusedRowCellValue("Contents") == DBNull.Value ? "" : gvContentDetails.GetFocusedRowCellValue("Contents").ToString();

                }

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        #endregion

    }
}