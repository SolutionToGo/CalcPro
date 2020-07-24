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
using DevExpress.XtraGrid.Columns;
using BL;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraRichEdit;

namespace CalcPro
{
    public partial class frmLoadSupplier : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        /// <summary>
        /// This form is to show suppliers and its data
        /// </summary>
        #region Varibales
        EArticles ObjEArticles = new EArticles();
        ESupplier ObjESupplier = new ESupplier();
        BSupplier ObjBSupplier = new BSupplier();
        #endregion

        #region Constructors
        public frmLoadSupplier()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        private void btnAddArticles_Click(object sender, EventArgs e)
        {
            try
            {
                if (ObjESupplier.SupplierID > 0)
                {
                    if (ObjESupplier == null)
                        ObjESupplier = new ESupplier();
                    ObjESupplier.WGWAID = -1;
                    frmSaveArticle frm = new frmSaveArticle();
                    frm.ObjEsupplier = ObjESupplier;
                    frm.ShowDialog();
                    if (frm.DialogResult == DialogResult.OK)
                    {
                        BindArticleData(ObjESupplier.SupplierID);
                        Setfocus(gvArticles, "WGWAID", ObjESupplier.WGWAID);
                        if (Utility._IsGermany == true)
                            frmCalcPro.UpdateStatus("Vorgang abgeschlossen: Speichern des Artikels");
                        else
                            frmCalcPro.UpdateStatus("Article saved successfully");
                    }
                }
            }
            catch (Exception ex) { Utility.ShowError(ex); }
        }

        private void btnAddContact_Click(object sender, EventArgs e)
        {
            try
            {
                if (ObjESupplier.SupplierID > 0)
                {
                    if (ObjESupplier == null)
                        ObjESupplier = new ESupplier();
                    ObjESupplier.ContactPersonID = -1;
                    frmSupplierMaster frm = new frmSupplierMaster("Contact");
                    frm.ObjEsupplier = ObjESupplier;
                    frm.ShowDialog();
                    if (frm.DialogResult == DialogResult.OK)
                    {
                        BindContactData(ObjESupplier.SupplierID);
                        Setfocus(gvContact, "ContactPersonID", ObjESupplier.ContactPersonID);
                        if (Utility._IsGermany == true)
                            frmCalcPro.UpdateStatus("Vorgang abgeschlossen: Speichern der Lieferanten-Kontaktdaten");
                        else
                            frmCalcPro.UpdateStatus("Supplier Contact saved successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnAddAddress_Click(object sender, EventArgs e)
        {
            try
            {
                if (ObjESupplier.SupplierID > 0)
                {
                    if (ObjESupplier == null)
                        ObjESupplier = new ESupplier();
                    ObjESupplier.AddressID = -1;
                    frmSupplierMaster frm = new frmSupplierMaster("Address");
                    frm.ObjEsupplier = ObjESupplier;
                    frm.ShowDialog();
                    if (frm.DialogResult == DialogResult.OK)
                    {
                        BindAddressData(ObjESupplier.SupplierID);
                        Setfocus(gvAddress, "AddressID", ObjESupplier.AddressID);
                        if (Utility._IsGermany == true)
                            frmCalcPro.UpdateStatus("Vorgang abgeschlossen: Speichern der Lieferanten-Adressdaten");
                        else
                            frmCalcPro.UpdateStatus("Supplier Address saved successfully");
                    }
                }
            }
            catch (Exception ex) { Utility.ShowError(ex); }
        }

        private void gvContact_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridView view = (GridView)sender;
                Point pt = view.GridControl.PointToClient(Control.MousePosition);
                GridHitInfo info = view.CalcHitInfo(pt);
                if (info.InRow || info.InRowCell)
                {
                    GetContactDetails();
                    frmSupplierMaster frm = new frmSupplierMaster("Contact");
                    frm.ObjEsupplier = ObjESupplier;
                    frm.ShowDialog();
                    if (frm.DialogResult == DialogResult.OK)
                    {
                        BindContactData(ObjESupplier.SupplierID);
                        Setfocus(gvContact, "ContactPersonID", ObjESupplier.ContactPersonID);
                    }
                }
            }
            catch (Exception ex) { Utility.ShowError(ex); }
        }

        private void gvAddress_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridView view = (GridView)sender;
                Point pt = view.GridControl.PointToClient(Control.MousePosition);
                GridHitInfo info = view.CalcHitInfo(pt);

                if (info.InRow || info.InRowCell)
                {
                    GetAddressDetails();
                    frmSupplierMaster frm = new frmSupplierMaster("Address");
                    frm.ObjEsupplier = ObjESupplier;
                    frm.ShowDialog();
                    if (frm.DialogResult == DialogResult.OK)
                    {
                        BindAddressData(ObjESupplier.SupplierID);
                        Setfocus(gvAddress, "AddressID", ObjESupplier.AddressID);
                    }
                }

            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gvArticles_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridView view = (GridView)sender;
                Point pt = view.GridControl.PointToClient(Control.MousePosition);
                GridHitInfo info = view.CalcHitInfo(pt);
                if (info.InRow || info.InRowCell)
                {
                    GetArticleDetails();
                    frmSaveArticle frm = new frmSaveArticle();
                    frm.ObjEsupplier = ObjESupplier;
                    frm.ShowDialog();
                    if (frm.DialogResult == DialogResult.OK)
                    {
                        BindArticleData(ObjESupplier.SupplierID);
                        Setfocus(gvArticles, "WGWAID", ObjESupplier.WGWAID);
                    }
                }

            }
            catch (Exception ex) { Utility.ShowError(ex); }
        }

        private void frmLoadSupplier_Load(object sender, EventArgs e)
        {
            try
            {
                if (Utility.SupplierDataAccess == "7")
                {
                    RbtnAddAddress.Enabled = false;
                    RbtnAddArtikle.Enabled = false;
                    RbtnAddContact.Enabled = false;
                }
                ObjESupplier = ObjBSupplier.GetSupplier(ObjESupplier);
                BindSupplierData();
                if (ObjESupplier.dtSupplier != null && ObjESupplier.dtSupplier.Rows.Count > 0)
                    cmbSupplier.EditValue = ObjESupplier.dtSupplier.Rows[0][0];
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gvArticles_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            try
            {
                if (e.HitInfo.InRow)
                    e.Menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("Löschen", gcArticleDelete_ItemClick));
            }
            catch (Exception ex) { }
        }

        private void gcArticleDelete_ItemClick(object sender, EventArgs e)
        {
            try
            {
                if (ObjBSupplier == null)
                    ObjBSupplier = new BSupplier();
                int IVlaue = 0;
                if (int.TryParse(Convert.ToString(gvArticles.GetFocusedRowCellValue("WGWAID")), out IVlaue))
                {
                    ObjESupplier.WGWAID = IVlaue;
                    ObjESupplier = ObjBSupplier.DeleteSupplierArticleMap(ObjESupplier);
                    BindArticleData(ObjESupplier.SupplierID);
                }
            }
            catch (Exception ex) { Utility.ShowError(ex); }
        }

        private void cmbSupplier_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                int SupplierID = 0;
                if (int.TryParse(Convert.ToString(cmbSupplier.EditValue), out SupplierID))
                {
                    DataRow dr = (cmbSupplier.GetSelectedDataRow() as DataRowView).Row;
                    ObjESupplier.SupplierID = SupplierID;
                    txtFullName.Text = Convert.ToString(dr["FullName"]);
                    txtShortName.Text = Convert.ToString(dr["ShortName"]);
                    txtCommentary.Text = Convert.ToString(dr["Commentary"]);
                    txtPaymentConditions.Text = Convert.ToString(dr["PaymentCondition"]);
                    txtSupplierEmail.Text = Convert.ToString(dr["EmailID"]);
                    txtSupptreet.Text = Convert.ToString(dr["Street"]);
                    txtSuppTelephone.Text = Convert.ToString(dr["Telephone"]);
                    txtSuppFax.Text = Convert.ToString(dr["Fax"]);
                    BindContactData(SupplierID);
                    BindAddressData(SupplierID);
                    BindArticleData(SupplierID);
                }
            }
            catch (Exception ex) { Utility.ShowError(ex); }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(string.IsNullOrEmpty(txtFullName.Text.Trim()))
                    throw new Exception("Vollständiger Name Cannot be empty");
                if (!dxValidationProvider1.Validate())
                    return;
                if (ObjESupplier == null)
                    ObjESupplier = new ESupplier();
                ParseSupplierDetails();
                ObjBSupplier = new BSupplier();
                ObjESupplier = ObjBSupplier.SaveSupplierDetails(ObjESupplier);
                BindSupplierData();
                cmbSupplier.EditValue = ObjESupplier.SupplierID;
                if (Utility._IsGermany)
                    frmCalcPro.UpdateStatus("Vorgang abgeschlossen: Speichern der Lieferantendaten ");
                else
                    frmCalcPro.UpdateStatus("Supplier data saved successfully");
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ObjESupplier.SupplierID = -1;
            txtFullName.Text = string.Empty;
            txtShortName.Text = string.Empty;
            txtCommentary.Text = string.Empty;
            txtPaymentConditions.Text = string.Empty;
            txtSuppFax.Text = string.Empty;
            txtSupplierEmail.Text = string.Empty;
            txtSuppTelephone.Text = string.Empty;
            txtSupptreet.Text = string.Empty;
            gcAddress.DataSource = null;
            gcContact.DataSource = null;
            gcArticles.DataSource = null;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                cmbSupplier_EditValueChanged(null, null);
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }
        
        private void txtShortName_Enter(object sender, EventArgs e)
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

        private void frmLoadSupplier_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.Escape)
                    this.Close();
            }
            catch (Exception ex){}
        }

        #endregion

        #region  Functions

        /// <summary>
        /// Code to parse the supplier details to entity
        /// </summary>
        private void ParseSupplierDetails()
        {
            try
            {
                ObjESupplier.SupplierFullName = txtFullName.Text;
                ObjESupplier.SupplierShortName = txtShortName.Text;
                ObjESupplier.Commentary = txtCommentary.Text;
                ObjESupplier.PaymentCondition = txtPaymentConditions.Text;
                ObjESupplier.SupplierEmailID = txtSupplierEmail.Text;
                ObjESupplier.SupplierStreet = txtSupptreet.Text;
                ObjESupplier.SupplierTelephone = txtSuppTelephone.Text;
                ObjESupplier.SupplierFax = txtSuppFax.Text;
            }
            catch (Exception ex) { throw; }
        }

        /// <summary>
        /// Code to set focus on grid control using column name and key value
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
            catch (Exception ex) { throw; }
        }

        /// <summary>
        /// Code to bind suppliers to combo box
        /// </summary>
        public void BindSupplierData()
        {
            try
            {
                if (ObjESupplier.dtSupplier != null)
                {
                    cmbSupplier.Properties.DataSource = ObjESupplier.dtSupplier;
                    cmbSupplier.Properties.DisplayMember = "FullName";
                    cmbSupplier.Properties.ValueMember = "SupplierID";
                }
            }
            catch (Exception ex) { throw; }
        }

        /// <summary>
        /// Code to bind suppliers contact list of selected supplier to grid control
        /// </summary>
        /// <param name="SupplierID"></param>
        public void BindContactData(int SupplierID)
        {
            try
            {
                if (ObjESupplier.dtContact != null)
                {
                    DataView dvContact = ObjESupplier.dtContact.DefaultView;
                    dvContact.RowFilter = "SupplierID = '" + SupplierID + "'";
                    gcContact.DataSource = dvContact;
                }
            }
            catch (Exception ex) { throw; }
        }

        /// <summary>
        /// Code to bind suppliers Address list of selected supplier to grid control
        /// </summary>
        /// <param name="SupplierID"></param>
        public void BindAddressData(int SupplierID)
        {
            try
            {
                if (ObjESupplier.dtAddress != null)
                {
                    DataView dvAddress = ObjESupplier.dtAddress.DefaultView;
                    dvAddress.RowFilter = "SupplierID = '" + SupplierID + "'";
                    gcAddress.DataSource = dvAddress;
                }
            }
            catch (Exception ex) { throw; }
        }

        /// <summary>
        /// Code to bind suppliers article list of selected supplier to grid control
        /// </summary>
        /// <param name="SupplierID"></param>
        private void BindArticleData(int SupplierID)
        {
            try
            {
                if (ObjESupplier.dtArticle != null)
                {
                    DataView dvArticle = ObjESupplier.dtArticle.DefaultView;
                    dvArticle.RowFilter = "SupplierID = '" + SupplierID + "'";
                    gcArticles.DataSource = dvArticle;
                }
            }
            catch (Exception ex) { throw; }
        }

        /// <summary>
        /// Code to bind selected contact details to properties in entity layer
        /// </summary>
        private void GetContactDetails()
        {
            try
            {
                int iValue = 0;
                if (ObjESupplier == null)
                    ObjESupplier = new ESupplier();
                if (int.TryParse(Convert.ToString(gvContact.GetFocusedRowCellValue("ContactPersonID")), out iValue))
                {
                    ObjESupplier.ContactPersonID = iValue;
                    ObjESupplier.Salutation = gvContact.GetFocusedRowCellValue("Salutation") == DBNull.Value ? "" : gvContact.GetFocusedRowCellValue("Salutation").ToString();
                    ObjESupplier.ContactName = gvContact.GetFocusedRowCellValue("ContactName") == DBNull.Value ? "" : gvContact.GetFocusedRowCellValue("ContactName").ToString();
                    ObjESupplier.Designation = gvContact.GetFocusedRowCellValue("Designation") == DBNull.Value ? "" : gvContact.GetFocusedRowCellValue("Designation").ToString();
                    ObjESupplier.ContEmailID = gvContact.GetFocusedRowCellValue("EmailID") == DBNull.Value ? "" : gvContact.GetFocusedRowCellValue("EmailID").ToString();
                    ObjESupplier.ContTelephone = gvContact.GetFocusedRowCellValue("Telephone") == DBNull.Value ? "" : gvContact.GetFocusedRowCellValue("Telephone").ToString();
                    ObjESupplier.ContFax = gvContact.GetFocusedRowCellValue("FAX") == DBNull.Value ? "" : gvContact.GetFocusedRowCellValue("FAX").ToString();
                    ObjESupplier.DefaultContact = Convert.ToBoolean(gvContact.GetFocusedRowCellValue("DefaultContact") == DBNull.Value ? "" : gvContact.GetFocusedRowCellValue("DefaultContact"));
                }
            }
            catch (Exception ex) { throw; }
        }

        /// <summary>
        /// Code to Bind supplier adress details to properties in entity layer
        /// </summary>
        private void GetAddressDetails()
        {
            try
            {
                int iValue = 0;
                if (ObjESupplier == null)
                    ObjESupplier = new ESupplier();
                if (int.TryParse(Convert.ToString(gvAddress.GetFocusedRowCellValue("AddressID")), out iValue))
                {
                    ObjESupplier.AddressID = iValue;
                    ObjESupplier.AddressShortName = gvAddress.GetFocusedRowCellValue("ShortName") == DBNull.Value ? "" : gvAddress.GetFocusedRowCellValue("ShortName").ToString();
                    ObjESupplier.StreetNo = gvAddress.GetFocusedRowCellValue("StreetNo") == DBNull.Value ? "" : gvAddress.GetFocusedRowCellValue("StreetNo").ToString();
                    ObjESupplier.AddrPostalCode = gvAddress.GetFocusedRowCellValue("PostalCode") == DBNull.Value ? "" : gvAddress.GetFocusedRowCellValue("PostalCode").ToString();
                    ObjESupplier.AddrCity = gvAddress.GetFocusedRowCellValue("City") == DBNull.Value ? "" : gvAddress.GetFocusedRowCellValue("City").ToString();
                    ObjESupplier.AddrCountry = gvAddress.GetFocusedRowCellValue("Country") == DBNull.Value ? "" : gvAddress.GetFocusedRowCellValue("Country").ToString();
                    ObjESupplier.DefaultAddress = Convert.ToBoolean(gvAddress.GetFocusedRowCellValue("DefaultAddress") == DBNull.Value ? "" : gvAddress.GetFocusedRowCellValue("DefaultAddress"));
                }

            }
            catch (Exception ex) { throw; }
        }

        /// <summary>
        /// Code to  bind supplier articles to properties in entity layer
        /// </summary>
        private void GetArticleDetails()
        {
            try
            {
                int iValue = 0;
                if (ObjESupplier == null)
                    ObjESupplier = new ESupplier();
                if (int.TryParse(Convert.ToString(gvArticles.GetFocusedRowCellValue("WGWAID")), out iValue))
                {
                    ObjESupplier.WGWAID = iValue;
                    ObjESupplier.WG = gvArticles.GetFocusedRowCellValue("WG") == DBNull.Value ? "" : gvArticles.GetFocusedRowCellValue("WG").ToString();
                    ObjESupplier.WA = gvArticles.GetFocusedRowCellValue("WA") == DBNull.Value ? "" : gvArticles.GetFocusedRowCellValue("WA").ToString();
                    ObjESupplier.WGDescription = gvArticles.GetFocusedRowCellValue("WGDescription") == DBNull.Value ? "" : gvArticles.GetFocusedRowCellValue("WGDescription").ToString();
                }
            }
            catch (Exception ex) { throw; }
        }
        #endregion

        private void RbtnReset_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btnReset_Click(0, e);
        }

        private void RbtnNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btnNew_Click(0, e);
        }

        private void RbtnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btnSave_Click(0, e);
        }

        private void RbtnAddContact_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btnAddContact_Click(0, e);
        }

        private void RbtnAddAddress_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btnAddAddress_Click(0, e);
        }

        private void RbtnAddArtikle_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btnAddArticles_Click(0, e);
        }
    }
}