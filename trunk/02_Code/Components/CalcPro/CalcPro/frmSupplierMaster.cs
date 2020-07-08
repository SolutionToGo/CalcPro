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
using DevExpress.XtraTab;
using EL;
using BL;
using System.Threading;

namespace CalcPro
{
    public partial class frmSupplierMaster : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// This form is to add, edit suppliers, supplier contacts and supplier address
        /// </summary>
        #region Varibales
        BSupplier ObjBSupplier = null;
        string _SupplierType = null;
        private ESupplier _ObjEsupplier = null;
        XtraTabPage ObjTabDetails = null;
        public ESupplier ObjEsupplier
        {
            get { return _ObjEsupplier; }
            set { _ObjEsupplier = value; }
        }
        #endregion

        #region Constructors
        public frmSupplierMaster()
        {
            InitializeComponent();
        }

        public frmSupplierMaster(string _Type)
        {
            InitializeComponent();
            _SupplierType = _Type;
        }
        #endregion

        #region Events
        private void frmSupplierMaster_Load(object sender, EventArgs e)
        {
            try
            {
                if(Utility.SupplierDataAccess == "7")
                {
                    btnSaveAddress.Enabled = false;
                    btnSaveContact.Enabled = false;
                }
                if (_SupplierType == "Contact")
                {
                    this.Text = "Lieferantenkontact";
                    ObjTabDetails = tbSupplierContact;
                    TabChange(ObjTabDetails);
                }
                else if (_SupplierType == "Address")
                {
                    this.Text = "Lieferantenaddresse";
                    ObjTabDetails = tbSupplierAddress;
                    TabChange(ObjTabDetails);
                }
                if (_SupplierType == "Contact" && _ObjEsupplier.ContactPersonID > 0)
                    BindContactsDetails();
                else if (_SupplierType == "Address" && _ObjEsupplier.AddressID > 0)
                    BindAddressDetails();

            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnSaveContact_Click(object sender, EventArgs e)
        {
            try
            {
                ParseSupplierContactsDetails();
                if (ObjBSupplier == null)
                    ObjBSupplier = new BSupplier();
                _ObjEsupplier = ObjBSupplier.SaveSupplierContactDetails(_ObjEsupplier);
            }
            catch (Exception ex) { Utility.ShowError(ex); }
        }

        private void btnSaveAddress_Click(object sender, EventArgs e)
        {
            try
            {
                ParseSupplierAddressDetails();
                if (ObjBSupplier == null)
                    ObjBSupplier = new BSupplier();
                _ObjEsupplier = ObjBSupplier.SaveSupplierAddressDetails(_ObjEsupplier);
                this.Close();
            }
            catch (Exception ex) { Utility.ShowError(ex); }
        }

        private void txtAddrStreetNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                RichTextBox txt = (RichTextBox)sender;
                if (e.KeyCode == Keys.Enter && txt.Lines.Length >= 2)
                {
                    e.Handled = true;
                }
                if (e.KeyData == (Keys.Control | Keys.C) || e.KeyData == (Keys.Control | Keys.V))
                {
                    e.SuppressKeyPress = true;
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtContactName_Enter(object sender, EventArgs e)
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

        #endregion

        #region Functions

        /// <summary>
        /// Code to navigate between cusomter, address and contacts
        /// </summary>
        /// <param name="ObjTabDetails"></param>
        private void TabChange(XtraTabPage ObjTabDetails)
        {
            try
            {
                if (ObjTabDetails != null)
                {
                    if (ObjTabDetails.PageVisible == true)
                        tcSupplier.SelectedTabPage = ObjTabDetails;
                    else
                    {
                        ObjTabDetails.PageVisible = true;
                        tcSupplier.SelectedTabPage = ObjTabDetails;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Code to populate contact details
        /// </summary>
        private void BindContactsDetails()
        {
            try
            {
                cmbSalutation.Text = _ObjEsupplier.Salutation;
                txtContactName.Text = _ObjEsupplier.ContactName;
                txtDesignation.Text = _ObjEsupplier.Designation;
                txtContactemail.Text = _ObjEsupplier.ContEmailID;
                txtContacttelephone.Text = _ObjEsupplier.ContTelephone;
                txtContactFax.Text = _ObjEsupplier.ContFax;
                chkDefaultContact.EditValue = _ObjEsupplier.DefaultContact;
            }
            catch (Exception ex){throw;}
        }

        /// <summary>
        /// Code to populate address details
        /// </summary>
        private void BindAddressDetails()
        {
            try
            {
                txtAddrShortName.Text = _ObjEsupplier.AddressShortName;
                txtAddrStreetNo.Text = _ObjEsupplier.StreetNo;
                txtAddrPostalCode.Text = _ObjEsupplier.AddrPostalCode;
                txtAddrcity.Text = _ObjEsupplier.AddrCity;
                txtAddrCountry.Text = _ObjEsupplier.AddrCountry;
                chkDefaultAddress.EditValue = _ObjEsupplier.DefaultAddress;
            }
            catch (Exception ex){throw;}
        }

        /// <summary>
        /// Code to parse supplier contact details while adding and editing
        /// </summary>
        private void ParseSupplierContactsDetails()
        {
            try
            {
                _ObjEsupplier.Salutation = cmbSalutation.Text;
                _ObjEsupplier.ContactName = txtContactName.Text;
                _ObjEsupplier.Designation = txtDesignation.Text;
                _ObjEsupplier.ContEmailID = txtContactemail.Text;
                _ObjEsupplier.ContTelephone = txtContacttelephone.Text;
                _ObjEsupplier.ContFax = txtContactFax.Text;
                _ObjEsupplier.DefaultContact = Convert.ToBoolean(chkDefaultContact.CheckState);
            }
            catch (Exception ex){throw;}
        }

        /// <summary>
        /// Code to parse supplier Address details while adding and editing
        /// </summary>
        private void ParseSupplierAddressDetails()
        {
            try
            {
                _ObjEsupplier.AddressShortName = txtAddrShortName.Text;
                _ObjEsupplier.StreetNo = txtAddrStreetNo.Text;
                _ObjEsupplier.AddrPostalCode = txtAddrPostalCode.Text;
                _ObjEsupplier.AddrCity = txtAddrcity.Text;
                _ObjEsupplier.AddrCountry = txtAddrCountry.Text;
                _ObjEsupplier.DefaultAddress = Convert.ToBoolean(chkDefaultAddress.CheckState);
            }
            catch (Exception ex){throw;}
        }
        #endregion

    }
}