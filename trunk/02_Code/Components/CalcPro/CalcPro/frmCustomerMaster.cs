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
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.Utils.Drawing;

namespace CalcPro
{
    public partial class frmCustomerMaster : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// This form is to add or edit customer, contacts and address details
        /// </summary>

        #region Variables
        ECustomer ObjECustomer =null;
        BCustomer ObjBCustomer = null;
        string _CustomerType = null;
        private ECustomer _ObjEcustomer = null;
        bool _isValidate = false;
        XtraTabPage ObjTabDetails = null;
        public ECustomer ObjEcustomer
        {
            get
            {
                return _ObjEcustomer;
            }
            set
            {
                _ObjEcustomer = value;
            }
        }
        #endregion

        #region Constructors

        public frmCustomerMaster()
        {
            InitializeComponent();
        }

        public frmCustomerMaster(string _Type, ECustomer ObjECustomer1)
        {
            InitializeComponent();
            _CustomerType = _Type;
            ObjECustomer = ObjECustomer1;

         }
        #endregion

        #region Events

       private void frmCustomerMaster_Load(object sender, EventArgs e)
        {
         
            try
            {
                if(Utility.CustomerDataAccess == "7")
                {
                    btnSaveContact.Enabled = false;
                    btnSaveAddress.Enabled = false;
                }
                if (_CustomerType == "Contact")
                {
                    this.Text = "Kundenkontact";
                    ObjTabDetails = tbCustomerContacts;
                    TabChange(ObjTabDetails);
                    cmbSalutation.Focus();
                }
                else if (_CustomerType == "Address")
                {
                    this.Text = "Kundenaddresse";
                    ObjTabDetails = tbCustomerAddress;
                    TabChange(ObjTabDetails);
                    txtAddShortName.Focus();
                }
                if (_CustomerType == "Contact")
                    BindContactsDetails();
                else if (_CustomerType == "Address")
                    BindAddressDetails();
                
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnCancelCustomer_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnSaveContact_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtContactName.Text.Trim()))
                {
                    _isValidate = false;
                    throw new Exception("Bitte eingeben: Name");
                }  
                bool isvalidName = dxValidationProviderContactName.Validate(txtContactName);
                if (!isvalidName)
                { _isValidate = false; }
                else
                {
                    _isValidate = true;
                }
                if (_isValidate == true)
                {
                    if (ObjECustomer == null)
                        ObjECustomer = new ECustomer();
                    ParseCustomerContactsDetails();
                    ObjBCustomer = new BCustomer();
                    ObjECustomer.ContactPersonID = ObjBCustomer.SaveCustomerContactDetails(ObjECustomer);
                    _isValidate = true;
                }                
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }
      
        private void btnSaveAddress_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtAddShortName.Text.Trim()))
                {
                    _isValidate = false;
                    throw new Exception("Bitte eingeben: Name");
                }     

                bool isvalidName = dxValidationProviderAddShortName.Validate(txtAddShortName);
                if (!isvalidName)
                { _isValidate = false; }
                else
                {
                    _isValidate = true;
                }
                if (_isValidate == true)
                {
                    if (ObjECustomer == null)
                        ObjECustomer = new ECustomer();
                    ParseCustomerAddressDetails();
                    ObjBCustomer = new BCustomer();
                    ObjECustomer.AddressID = ObjBCustomer.SaveCustomerAddressDetails(ObjECustomer);
                    _isValidate = true;
                }                
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void frmCustomerMaster_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.DialogResult != DialogResult.Cancel)
                {
                    if (_isValidate == false)
                        e.Cancel = true;
                }
                else
                    e.Cancel = false;
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }
        private void frmCustomerMaster_KeyDown_1(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.Escape)
                {
                    btnCancelCustomer_Click(0, e);
                }

            }
            catch (Exception ex) { }
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
        /// It contains code to change the tabs between Customer master, Address and Contacts
        /// </summary>
        /// <param name="ObjTabDetails"></param>
        private void TabChange(XtraTabPage ObjTabDetails)
        {
            try
            {
                if (ObjTabDetails != null)
                {
                    if (ObjTabDetails.PageVisible == true)
                        tcCustomers.SelectedTabPage = ObjTabDetails;
                    else
                    {
                        ObjTabDetails.PageVisible = true;
                        tcCustomers.SelectedTabPage = ObjTabDetails;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// It contains code to parsing new customer contact details or existing customer contact details to bind with properties in entity layer
        /// </summary>
        private void ParseCustomerContactsDetails()
        {
            try
            {
                ObjECustomer.Salutation = cmbSalutation.Text;
                ObjECustomer.ContatPersonName = txtContactName.Text;
                ObjECustomer.Designation = txtContDesignation.Text;
                ObjECustomer.ContEmailID = txtContMail.Text;
                ObjECustomer.ContTelephone = txtContTelephone.Text;
                ObjECustomer.ContFax = txtContFax.Text;
                ObjECustomer.DefaultContact = Convert.ToBoolean(chkDefaultContact.CheckState);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        ///  It contains code to fetch Contact details based on customerID and bind to grid control
        /// </summary>
        private void BindContactsDetails()
        {
            try
            {
                cmbSalutation.Text = ObjECustomer.Salutation;
                txtContactName.Text = ObjECustomer.ContatPersonName;
                txtContDesignation.Text = ObjECustomer.Designation;
                txtContMail.Text = ObjECustomer.ContEmailID;
                txtContTelephone.Text = ObjECustomer.ContTelephone;
                txtContFax.Text = ObjECustomer.ContFax;
                chkDefaultContact.EditValue = ObjECustomer.DefaultContact;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        ///  It contains code to parse the customer address details while adding or editing new address
        /// </summary>
        private void ParseCustomerAddressDetails()
        {
            try
            {
                ObjECustomer.AddressShortName = txtAddShortName.Text;
                ObjECustomer.StreetNo = txtAddStreetNo.Text;
                ObjECustomer.AddrPostalCode = txtAddPostalCode.Text;
                ObjECustomer.AddrCity = txtAddCity.Text;
                ObjECustomer.AddrCountry = txtAddCountry.Text;
                ObjECustomer.DefaultAddress = Convert.ToBoolean(chkDefaultAddress.CheckState);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        /// It contains code to Bind address details based on customer ID and bind to grid control
        /// </summary>
        private void BindAddressDetails()
        {
            try
            {
                txtAddShortName.Text = ObjECustomer.AddressShortName;
                txtAddStreetNo.Text = ObjECustomer.StreetNo;
                txtAddPostalCode.Text = ObjECustomer.AddrPostalCode;
                txtAddCity.Text = ObjECustomer.AddrCity;
                txtAddCountry.Text = ObjECustomer.AddrCountry;
                chkDefaultAddress.EditValue = ObjECustomer.DefaultAddress;
            }
            catch (Exception ex)
            {
                throw;
            }

        }



        #endregion


      
    }
}