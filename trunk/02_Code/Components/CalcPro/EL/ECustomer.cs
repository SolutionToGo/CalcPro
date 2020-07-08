using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EL
{
    public class ECustomer
    {
        //To hold the Customer Details
        private int _Customer_CustomerID = -1;
        private string _CustomerFullName;
        private string _CustomerShortName;
        private string _CustStreet;
        private string _CustPostalCode;
        private string _CustCity;
        private string _CustCountry;
        private string _ILN;
        private string _Telephone;
        private string _CustFax;
        private string _CustEmailID;
        private string _CustTaxNumber;
        private string _BankName;
        private string _BankPostalCode;
        private string _BankAccountNumber;
        private string _DVNr;
        private string _TenderNumber;
        private string _DebitorNumber;
        private string _CountryType;
        private string _CountryName;
        private string _CreatedBy;
        private DateTime _CreatedDate;
        private string _LastUpdatedBy;
        private DateTime _LastUpdatedDate;
        private bool _IsActive;
        private string _Commentary;
        private DataSet _dtCustomer;


        // To hold the Customer Contacts
        private int _ContactPersonID = -1;
        private int _Cont_CustomerID;
        private string _Salutation;
        private string _ContatPersonName;
        private string _Designation;
        private string _ContEmailID;
        private string _ContTelephone;
        private string _ContFax;
        private bool _DefaultContact;

        // To hold the Customer Address
        private int _AddressID = -1;
        private int _Addr_CustomerID;
        private string _AddressShortName;
        private string _StreetNo;
        private string _AddrPostalCode;
        private string _AddrCity;
        private string _AddrCountry;
        private bool _DefaultAddress;


        // Customers Properties
        public DataSet dsCustomer
        {
            get { return _dtCustomer; }
            set { _dtCustomer = value; }
        }
        public int Customer_CustomerID
        {
            get { return _Customer_CustomerID; }
            set { _Customer_CustomerID = value; }
        }
        public string CustomerFullName
        {
            get { return _CustomerFullName; }
            set { _CustomerFullName = value; }
        }
        public string CustomerShortName
        {
            get { return _CustomerShortName; }
            set { _CustomerShortName = value; }
        }
        public string CustStreet
        {
            get { return _CustStreet; }
            set { _CustStreet = value; }
        }
        public string CustPostalCode
        {
            get { return _CustPostalCode; }
            set { _CustPostalCode = value; }
        }
        public string CustCity
        {
            get { return _CustCity; }
            set { _CustCity = value; }
        }
        public string CustCountry
        {
            get { return _CustCountry; }
            set { _CustCountry = value; }
        }
        public string ILN
        {
            get { return _ILN; }
            set { _ILN = value; }
        }
        public string Telephone
        {
            get { return _Telephone; }
            set { _Telephone = value; }
        }
        public string CustFax
        {
            get { return _CustFax; }
            set { _CustFax = value; }
        }
        public string CustEmailID
        {
            get { return _CustEmailID; }
            set { _CustEmailID = value; }
        }
        public string CustTaxNumber
        {
            get { return _CustTaxNumber; }
            set { _CustTaxNumber = value; }
        }
        public string BankName
        {
            get { return _BankName; }
            set { _BankName = value; }
        }
        public string BankPostalCode
        {
            get { return _BankPostalCode; }
            set { _BankPostalCode = value; }
        }
        public string BankAccountNumber
        {
            get { return _BankAccountNumber; }
            set { _BankAccountNumber = value; }
        }
        public string DVNr
        {
            get { return _DVNr; }
            set { _DVNr = value; }
        }
        public string TenderNumber
        {
            get { return _TenderNumber; }
            set { _TenderNumber = value; }
        }
        public string DebitorNumber
        {
            get { return _DebitorNumber; }
            set { _DebitorNumber = value; }
        }
        public string CountryType
        {
            get { return _CountryType; }
            set { _CountryType = value; }
        }
        public string CountryName
        {
            get { return _CountryName; }
            set { _CountryName = value; }
        }
        public string CreatedBy
        {
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        }
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { _CreatedDate = value; }
        }
        public string LastUpdatedBy
        {
            get { return _LastUpdatedBy; }
            set { _LastUpdatedBy = value; }
        }
        public DateTime LastUpdatedDate
        {
            get { return _LastUpdatedDate; }
            set { _LastUpdatedDate = value; }
        }
        public bool IsActive
        {
            get { return _IsActive; }
            set { _IsActive = value; }
        }
        public string Commentary
        {
            get { return _Commentary; }
            set { _Commentary = value; }
        }

        // Customers Contact Properties
        public int ContactPersonID
        {
            get { return _ContactPersonID; }
            set { _ContactPersonID = value; }
        }
        public int Cont_CustomerID
        {
            get { return _Cont_CustomerID; }
            set { _Cont_CustomerID = value; }
        }
        public string Salutation
        {
            get { return _Salutation; }
            set { _Salutation = value; }
        }
        public string ContatPersonName
        {
            get { return _ContatPersonName; }
            set { _ContatPersonName = value; }
        }
        public string Designation
        {
            get { return _Designation; }
            set { _Designation = value; }
        }
        public string ContEmailID
        {
            get { return _ContEmailID; }
            set { _ContEmailID = value; }
        }
        public string ContTelephone
        {
            get { return _ContTelephone; }
            set { _ContTelephone = value; }
        }
        public string ContFax
        {
            get { return _ContFax; }
            set { _ContFax = value; }
        }
        public bool DefaultContact
        {
            get { return _DefaultContact; }
            set { _DefaultContact = value; }
        }

        // Customers Address Properties
        public int AddressID
        {
            get { return _AddressID; }
            set { _AddressID = value; }
        }
        public int Addr_CustomerID
        {
            get { return _Addr_CustomerID; }
            set { _Addr_CustomerID = value; }
        }
        public string AddressShortName
        {
            get { return _AddressShortName; }
            set { _AddressShortName = value; }
        }
        public string StreetNo
        {
            get { return _StreetNo; }
            set { _StreetNo = value; }
        }
        public string AddrPostalCode
        {
            get { return _AddrPostalCode; }
            set { _AddrPostalCode = value; }
        }
        public string AddrCity
        {
            get { return _AddrCity; }
            set { _AddrCity = value; }
        }
        public string AddrCountry
        {
            get { return _AddrCountry; }
            set { _AddrCountry = value; }
        }
        public bool DefaultAddress
        {
            get { return _DefaultAddress; }
            set { _DefaultAddress = value; }
        }
    }
}
