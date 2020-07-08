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
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing.Drawing2D;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraRichEdit;

namespace CalcPro
{
    public partial class frmLoadCustomerMaster : DevExpress.XtraEditors.XtraForm
    {
        ECustomer ObjECustomer = new ECustomer();
        BCustomer ObjBCustomer = new BCustomer();
        public frmLoadCustomerMaster()
        {
            InitializeComponent();
        }

        private void btnContactAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ObjECustomer = new ECustomer();
                int Ivalue = 0;
                if (int.TryParse(Convert.ToString(cmbCustomer.EditValue), out Ivalue))
                {
                    ObjECustomer.ContactPersonID = -1;
                    ObjECustomer.Customer_CustomerID = Ivalue;
                    frmCustomerMaster frm = new frmCustomerMaster("Contact", ObjECustomer);
                    frm.ShowDialog();
                    if (frm.DialogResult == DialogResult.OK)
                    {
                        BindContactData();
                        Setfocus(gvContacts, "ContactPersonID", ObjECustomer.ContactPersonID);
                        if (Utility._IsGermany == true)
                            frmCalcPro.UpdateStatus("Vorgang abgeschlossen: Speichern der Kunden-Kontaktdaten");
                        else
                            frmCalcPro.UpdateStatus("Customer contact saved successfully");
                    }
                }
            }
            catch (Exception ex) { Utility.ShowError(ex); }
        }

        private void btnAddressAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ObjECustomer = new ECustomer();
                int Ivalue = 0;
                if (int.TryParse(Convert.ToString(cmbCustomer.EditValue), out Ivalue))
                {
                    ObjECustomer.AddressID = -1;
                    ObjECustomer.Customer_CustomerID = Ivalue;
                    frmCustomerMaster frm = new frmCustomerMaster("Address", ObjECustomer);
                    frm.ShowDialog();
                    if (frm.DialogResult == DialogResult.OK)
                    {
                        BindAddressData();
                        Setfocus(gvAddress, "AddressID", ObjECustomer.AddressID);
                        if (Utility._IsGermany == true)
                            frmCalcPro.UpdateStatus("Vorgang abgeschlossen: Speichern der Kunden-Adressdaten");
                        else
                            frmCalcPro.UpdateStatus("Customer address saved successfully");
                    }
                }
            }
            catch (Exception ex) { Utility.ShowError(ex); }
        }

        private void frmLoadCustomerMaster_Load(object sender, EventArgs e)
        {
            try
            {
                if (Utility.CustomerDataAccess == "7")
                {
                    btnContactAdd.Enabled = false;
                    btnAddressAdd.Enabled = false;
                }
                BindCustomerData();
                if (ObjECustomer.dsCustomer != null && ObjECustomer.dsCustomer.Tables[0].Rows.Count > 0)
                    cmbCustomer.EditValue = ObjECustomer.dsCustomer.Tables[0].Rows[0][0];
                gvContacts.BestFitColumns();
                gvAddress.BestFitColumns();
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void gvContacts_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridView view = (GridView)sender;
                Point pt = view.GridControl.PointToClient(Control.MousePosition);
                GridHitInfo info = view.CalcHitInfo(pt);
                if (info.InRow || info.InRowCell)
                {
                    ObjECustomer = new ECustomer();
                    int Ivalue = 0;
                    if (int.TryParse(Convert.ToString(cmbCustomer.EditValue), out Ivalue))
                    {
                        ObjECustomer.Customer_CustomerID = Ivalue;
                        GetContactDetails();
                        frmCustomerMaster frm = new frmCustomerMaster("Contact", ObjECustomer);
                        frm.ObjEcustomer = ObjECustomer;
                        frm.ShowDialog();
                        if (frm.DialogResult == DialogResult.OK)
                        {
                            BindContactData();
                            Setfocus(gvContacts, "ContactPersonID", ObjECustomer.ContactPersonID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
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
                    ObjECustomer = new ECustomer();
                    int Ivalue = 0;
                    if (int.TryParse(Convert.ToString(cmbCustomer.EditValue), out Ivalue))
                    {
                        ObjECustomer.Customer_CustomerID = Ivalue;
                        GetAddressDetails();
                        frmCustomerMaster frm = new frmCustomerMaster("Address", ObjECustomer);
                        frm.ObjEcustomer = ObjECustomer;
                        frm.ShowDialog();
                        if (frm.DialogResult == DialogResult.OK)
                        {
                            BindAddressData();
                            Setfocus(gvAddress, "AddressID", ObjECustomer.AddressID);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        public void BindCustomerData()
        {
            try
            {
                ObjBCustomer.GetCustomers(ObjECustomer);
                if (ObjECustomer.dsCustomer != null)
                {
                    cmbCustomer.Properties.DataSource = ObjECustomer.dsCustomer.Tables[0];
                    cmbCustomer.Properties.DisplayMember = "CustomerFullName";
                    cmbCustomer.Properties.ValueMember = "CustomerID";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void BindContactData()
        {
            try
            {
                ObjBCustomer.GetCustomers(ObjECustomer);
                if (ObjECustomer.dsCustomer != null)
                {
                    DataView dvContact = ObjECustomer.dsCustomer.Tables["Table1"].DefaultView;
                    dvContact.RowFilter = "CustomerID = '" + ObjECustomer.Customer_CustomerID + "'";
                    gcContacts.DataSource = dvContact;
                    gvAddress.BestFitColumns();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void BindAddressData()
        {
            try
            {
                ObjBCustomer.GetCustomers(ObjECustomer);
                if (ObjECustomer.dsCustomer != null)
                {
                    DataView dvAddress = ObjECustomer.dsCustomer.Tables["Table2"].DefaultView;
                    dvAddress.RowFilter = "CustomerID = '" + ObjECustomer.Customer_CustomerID + "'";
                    gcAddress.DataSource = dvAddress;
                    gvAddress.BestFitColumns();
                }
            }
            catch (Exception ex){throw;}
        }

        private void GetContactDetails()
        {
            try
            {
                if (gvContacts.GetFocusedRowCellValue("ContactPersonID") != DBNull.Value)
                {
                    ObjECustomer.ContactPersonID = Convert.ToInt32(gvContacts.GetFocusedRowCellValue("ContactPersonID") == DBNull.Value ? "" : gvContacts.GetFocusedRowCellValue("ContactPersonID"));
                    ObjECustomer.Salutation = gvContacts.GetFocusedRowCellValue("Salutation") == DBNull.Value ? "" : gvContacts.GetFocusedRowCellValue("Salutation").ToString();
                    ObjECustomer.ContatPersonName = gvContacts.GetFocusedRowCellValue("ContatPersonName") == DBNull.Value ? "" : gvContacts.GetFocusedRowCellValue("ContatPersonName").ToString();
                    ObjECustomer.Designation = gvContacts.GetFocusedRowCellValue("Designation") == DBNull.Value ? "" : gvContacts.GetFocusedRowCellValue("Designation").ToString();
                    ObjECustomer.ContEmailID = gvContacts.GetFocusedRowCellValue("EmailID") == DBNull.Value ? "" : gvContacts.GetFocusedRowCellValue("EmailID").ToString();
                    ObjECustomer.ContTelephone = gvContacts.GetFocusedRowCellValue("Telephone") == DBNull.Value ? "" : gvContacts.GetFocusedRowCellValue("Telephone").ToString();
                    ObjECustomer.ContFax = gvContacts.GetFocusedRowCellValue("FAX") == DBNull.Value ? "" : gvContacts.GetFocusedRowCellValue("FAX").ToString();
                    ObjECustomer.DefaultContact = Convert.ToBoolean(gvContacts.GetFocusedRowCellValue("DefaultContact") == DBNull.Value ? "" : gvContacts.GetFocusedRowCellValue("DefaultContact"));
                }
            }
            catch (Exception ex){throw;}
        }

        private void GetAddressDetails()
        {
            try
            {
                if (gvAddress.GetFocusedRowCellValue("AddressID") != DBNull.Value)
                {
                    ObjECustomer.AddressID = Convert.ToInt32(gvAddress.GetFocusedRowCellValue("AddressID") == DBNull.Value ? "" : gvAddress.GetFocusedRowCellValue("AddressID"));
                    ObjECustomer.AddressShortName = gvAddress.GetFocusedRowCellValue("AddressShortName") == DBNull.Value ? "" : gvAddress.GetFocusedRowCellValue("AddressShortName").ToString();
                    ObjECustomer.StreetNo = gvAddress.GetFocusedRowCellValue("StreetNo") == DBNull.Value ? "" : gvAddress.GetFocusedRowCellValue("StreetNo").ToString();
                    ObjECustomer.AddrPostalCode = gvAddress.GetFocusedRowCellValue("PostalCode") == DBNull.Value ? "" : gvAddress.GetFocusedRowCellValue("PostalCode").ToString();
                    ObjECustomer.AddrCity = gvAddress.GetFocusedRowCellValue("City") == DBNull.Value ? "" : gvAddress.GetFocusedRowCellValue("City").ToString();
                    ObjECustomer.AddrCountry = gvAddress.GetFocusedRowCellValue("Country") == DBNull.Value ? "" : gvAddress.GetFocusedRowCellValue("Country").ToString();
                    ObjECustomer.DefaultAddress = Convert.ToBoolean(gvAddress.GetFocusedRowCellValue("DefaultAddress") == DBNull.Value ? "" : gvAddress.GetFocusedRowCellValue("DefaultAddress"));
                }
            }
            catch (Exception ex){throw;}
        }

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

        private void btnReset_Click(object sender, EventArgs e)
        {
            cmbCustomer_EditValueChanged(null, null);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ObjECustomer.Customer_CustomerID = -1;
            txtCustFullName.Text = string.Empty;
            txtCustShortName.Text = string.Empty;
            txtCustStreet.Text = string.Empty;
            txtCustTaxNo.Text = string.Empty;
            txtCustAccNo.Text = string.Empty;
            txtCustBankName.Text = string.Empty;
            txtCustBankPCode.Text = string.Empty;
            txtCustCountryName.Text = string.Empty;
            txtCustCountryType.Text = string.Empty;
            txtCustDebitorNo.Text = string.Empty;
            txtCustEmail.Text = string.Empty;
            txtCustFax.Text = string.Empty;
            txtCustTelephone.Text = string.Empty;
            txtCustTenderNo.Text = string.Empty;
            txtDVNr.Text = string.Empty;
            txtILN.Text = string.Empty;
            memoEditCommentary.Text = string.Empty;
            gcContacts.DataSource = null;
            gcAddress.DataSource = null;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtCustFullName.Text.Trim()))
                    throw new Exception("Vollständiger Name Cannot be empty");
                if (!dxReqValidationProvider.Validate())
                    return;
                if (ObjECustomer == null)
                    ObjECustomer = new ECustomer();
                ParseCustomerDetails();
                ObjBCustomer = new BCustomer();
                ObjECustomer.Customer_CustomerID = ObjBCustomer.SaveCustomerDetails(ObjECustomer);
                BindCustomerData();
                cmbCustomer.EditValue = ObjECustomer.Customer_CustomerID;
                cmbCustomer.Focus();
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void cmbCustomer_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                int IValue = 0;
                if (int.TryParse(Convert.ToString(cmbCustomer.EditValue), out IValue))
                {
                    DataRow dr = (cmbCustomer.GetSelectedDataRow() as DataRowView).Row;
                    ObjECustomer.Customer_CustomerID = IValue;
                    txtCustFullName.Text = Convert.ToString(dr["CustomerFullName"]);
                    txtCustShortName.Text = Convert.ToString(dr["CustomerShortName"]);
                    txtCustStreet.Text = Convert.ToString(dr["Street"]);
                    txtCustTaxNo.Text = Convert.ToString(dr["TaxNumber"]);
                    txtCustAccNo.Text = Convert.ToString(dr["BankAccountNumber"]);
                    txtCustBankName.Text = Convert.ToString(dr["BankName"]);
                    txtCustBankPCode.Text = Convert.ToString(dr["BankPostalCode"]);
                    txtCustCountryName.Text = Convert.ToString(dr["CountryName"]);
                    txtCustCountryType.Text = Convert.ToString(dr["CountryType"]);
                    txtCustDebitorNo.Text = Convert.ToString(dr["DebitorNumber"]);
                    txtCustEmail.Text = Convert.ToString(dr["EmailID"]);
                    txtCustFax.Text = Convert.ToString(dr["Fax"]);
                    txtCustTelephone.Text = Convert.ToString(dr["Telephone"]);
                    txtCustTenderNo.Text = Convert.ToString(dr["TenderNumber"]);
                    txtDVNr.Text = Convert.ToString(dr["DVNr"]);
                    txtILN.Text = Convert.ToString(dr["ILN"]);
                    memoEditCommentary.Text = Convert.ToString(dr["Commentary"]);

                    DataView dvContact = ObjECustomer.dsCustomer.Tables[1].DefaultView;
                    dvContact.RowFilter = "CustomerID = '" + IValue + "'";
                    gcContacts.DataSource = dvContact;

                    DataView dvAddress = ObjECustomer.dsCustomer.Tables[2].DefaultView;
                    dvAddress.RowFilter = "CustomerID = '" + IValue + "'";
                    gcAddress.DataSource = dvAddress;

                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void ParseCustomerDetails()
        {
            try
            {
                ObjECustomer.CustomerFullName = txtCustFullName.Text;
                ObjECustomer.CustomerShortName = txtCustShortName.Text;
                ObjECustomer.CustStreet = txtCustStreet.Text;
                ObjECustomer.ILN = txtILN.Text;
                ObjECustomer.Telephone = txtCustTelephone.Text;
                ObjECustomer.CustFax = txtCustFax.Text;
                ObjECustomer.CustEmailID = txtCustEmail.Text;
                ObjECustomer.CustTaxNumber = txtCustTaxNo.Text;
                ObjECustomer.BankName = txtCustBankName.Text;
                ObjECustomer.BankPostalCode = txtCustBankPCode.Text;
                ObjECustomer.BankAccountNumber = txtCustAccNo.Text;
                ObjECustomer.DVNr = txtDVNr.Text;
                ObjECustomer.TenderNumber = txtCustTenderNo.Text;
                ObjECustomer.DebitorNumber = txtCustDebitorNo.Text;
                ObjECustomer.CountryType = txtCustCountryType.Text;
                ObjECustomer.CountryName = txtCustCountryName.Text;
                ObjECustomer.Commentary = memoEditCommentary.Text;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private void txtCustShortName_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar != ' ' && !Char.IsDigit(e.KeyChar))
                    e.Handled = false;
                else
                    e.Handled = true;
            }
            catch (Exception ex){Utility.ShowError(ex);}
        }

        private void txtCustStreet_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == (Keys.Control | Keys.C) || e.KeyData == (Keys.Control | Keys.V))
                {
                    e.SuppressKeyPress = true;
                    e.Handled = true;
                }
            }
            catch (Exception ex){Utility.ShowError(ex);}
        }

        private void txtCustShortName_Enter(object sender, EventArgs e)
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

        private void frmLoadCustomerMaster_KeyDown(object sender, KeyEventArgs e)
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