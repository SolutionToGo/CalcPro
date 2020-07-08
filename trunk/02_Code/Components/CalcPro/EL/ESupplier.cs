using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EL
{
    public class ESupplier
    {
        //To hold the Supplier Details
        private int _ProjectID = -1;
        private int _SupplierID = -1;
        private string _SupplierEmailID;
        private string _SupplierFullName;
        private string _SupplierShortName;
        private string _PaymentCondition;
        private string _CreatedBy;
        private DateTime _CreatedDate;
        private string _LastUpdatedBy;
        private DateTime _LastUpdatedDate;
        private bool _IsActive;
        private string _Commentary;
        private DataSet _dsSupplier;
        private DataTable _dtSupplier;
        private string _SupplierStreet;
        private string _SupplierTelephone;
        private string _SupplierFax;


        // To hold the Supplier Contacts
        private int _ContactPersonID = -1;
        private int _Cont_supplierID;
        private string _ContactName;
        private string _Salutation;
        private string _Designation;
        private string _ContEmailID;
        private string _ContTelephone;
        private string _ContFax;
        private bool _DefaultContact;
        private DataTable _dtContact;

        // To hold the Supplier Address
        private int _AddressID = -1;
        private int _Addr_supplierID;
        private string _AddressShortName;
        private string _StreetNo;
        private string _AddrPostalCode;
        private string _AddrCity;
        private string _AddrCountry;
        private bool _DefaultAddress;
        private DataTable _dtAddress;

        //To hold the article data
        private int _WGWAID = -1;
        private string _WG;
        private string _WA;
        private string _WGDescription;
        private DataTable _dtArticle;
        private DataSet _Articles;
        private DataSet _SupplierProposal;

        private DataTable _dtProposal;
        private DataTable _dtPositions;
        private int _SupplierProposalID;
        private DataTable _dtUpdateSupplierPrice;
        private string _LVSection;

        //To hold delete position
        private int _DeletePositionID = -1;
        private int _PositionID = -1;

        private DataTable _dtNewPositions;
        private DataTable _dtDeletedPositions;
        private DataTable _dtProposedPositions;
        private DataTable _dtSupplierMail;
        private bool _IsSingle = false;
        private string _UncheckedColumn = string.Empty;
        private string _strArticleExists = string.Empty;
        private string _strSupplierExists = string.Empty;

        public DataTable dtNewPositions
        {
            get { return _dtNewPositions; }
            set { _dtNewPositions = value; }
        }
        public DataTable dtDeletedPositions
        {
            get { return _dtDeletedPositions; }
            set { _dtDeletedPositions = value; }
        }
        public DataTable dtProposedPositions
        {
            get { return _dtProposedPositions; }
            set { _dtProposedPositions = value; }
        }
        public DataTable dtSupplierMail
        {
            get { return _dtSupplierMail; }
            set { _dtSupplierMail = value; }
        }

        public DataTable dtSupplier_SP = new DataTable();

        public DataTable PID = new DataTable();
        // Article Properties and entities
        public int WGWAID
        {
            get { return _WGWAID; }
            set { _WGWAID = value; }
        }
        public string WG
        {
            get { return _WG; }
            set { _WG = value; }
        }
        public string WA
        {
            get { return _WA; }
            set { _WA = value; }
        }
        public string WGDescription
        {
            get { return _WGDescription; }
            set { _WGDescription = value; }
        }
        public DataTable dtArticle
        {
            get { return _dtArticle; }
            set { _dtArticle = value; }
        }

        public DataTable dtArticleID = new DataTable();

        public DataTable dtArticlesMerge = null;

        public DataTable dtLVSection = null;

        public DataSet SupplierProposal
        {
            get { return _SupplierProposal; }
            set { _SupplierProposal = value; }
        }
        // Supplier Properties
        public DataSet dsSupplier
        {
            get { return _dsSupplier; }
            set { _dsSupplier = value; }
        }
        public int SupplierID
        {
            get { return _SupplierID; }
            set { _SupplierID = value; }
        }
        public string SupplierEmailID
        {
            get { return _SupplierEmailID; }
            set { _SupplierEmailID = value; }
        }
        public string SupplierFullName
        {
            get { return _SupplierFullName; }
            set { _SupplierFullName = value; }
        }
        public string SupplierShortName
        {
            get { return _SupplierShortName; }
            set { _SupplierShortName = value; }
        }
        public string PaymentCondition
        {
            get { return _PaymentCondition; }
            set { _PaymentCondition = value; }
        }
        public string Commentary
        {
            get { return _Commentary; }
            set { _Commentary = value; }
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
        public DataTable dtSupplier
        {
            get { return _dtSupplier; }
            set { _dtSupplier = value; }
        }

        public string SupplierStreet
        {
            get { return _SupplierStreet; }
            set { _SupplierStreet = value; }
        }

        public string SupplierTelephone
        {
            get { return _SupplierTelephone; }
            set { _SupplierTelephone = value; }
        }

        public string SupplierFax
        {
            get { return _SupplierFax; }
            set { _SupplierFax = value; }
        }

        // Supplier Contact Properties
        public int ContactPersonID
        {
            get { return _ContactPersonID; }
            set { _ContactPersonID = value; }
        }
        public int Cont_supplierID
        {
            get { return _Cont_supplierID; }
            set { _Cont_supplierID = value; }
        }
        public string Salutation
        {
            get { return _Salutation; }
            set { _Salutation = value; }
        }
        public string ContactName
        {
            get { return _ContactName; }
            set { _ContactName = value; }
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
        public DataTable dtContact
        {
            get { return _dtContact; }
            set { _dtContact = value; }
        }

        // Supplier Address Properties
        public int AddressID
        {
            get { return _AddressID; }
            set { _AddressID = value; }
        }
        public int Addr_supplierID
        {
            get { return _Addr_supplierID; }
            set { _Addr_supplierID = value; }
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
        public DataTable dtAddress
        {
            get { return _dtAddress; }
            set { _dtAddress = value; }
        }
        public DataSet dsProposal = new DataSet();

        public DataSet dsProposalNumbers = new DataSet();

        public DataTable dtProposalArticles = new DataTable();

        public DataTable dtSupplierProposal = null;

        public DataTable dtPositionsToDB = new DataTable();

        public DataTable dtDeletedPositionsToDB = new DataTable();

        public DataTable dtSupplierToDB = new DataTable();

        public DataTable dtProposedArticles = new DataTable();

        public DataTable dtPositions
        {
            get { return _dtPositions; }
            set { _dtPositions = value; }
        }

        public DataTable dtPositionscopy = null;

        public DataTable dtSuppliers = null;

        public DataTable dtSupplierForproposal = new DataTable();

        //Supplier Proposal
        private int _ProposalID = -1;
        public int ProposalID
        {
            get { return _ProposalID; }
            set { _ProposalID = value; }
        }

        public int ProjectID
        {
            get { return _ProjectID; }
            set { _ProjectID = value; }
        }
        public int SupplierProposalID
        {
            get { return _SupplierProposalID; }
            set { _SupplierProposalID = value; }
        }

        public int ProposalSupplierID = -1;
        public string ProposalName = string.Empty;
        public DataTable dtUpdateSupplierPrice
        {
            get { return _dtUpdateSupplierPrice; }
            set { _dtUpdateSupplierPrice = value; }
        }

        //Position status
        public int DeletePositionID
        {
            get { return _DeletePositionID; }
            set { _DeletePositionID = value; }
        }
        public int PositionID
        {
            get { return _PositionID; }
            set { _PositionID = value; }
        }

        private string _Suppliers;
        public string Suppliers
        {
            get { return _Suppliers; }
            set { _Suppliers = value; }
        }

        private decimal _SupplierPrice = 0;
        private decimal _Multi1 = 1;
        private decimal _Multi2 = 1;
        private decimal _Multi3 = 1;
        private decimal _Multi4 = 1;
        private string _Fabrikate;
        private string _SupplierName;
        private bool _IsSelected;

        public decimal SupplierPrice
        {
            get { return _SupplierPrice; }
            set { _SupplierPrice = value; }
        }
        public decimal Multi1
        {
            get { return _Multi1; }
            set { _Multi1 = value; }
        }
        public decimal Multi2
        {
            get { return _Multi2; }
            set { _Multi2 = value; }
        }
        public decimal Multi3
        {
            get { return _Multi3; }
            set { _Multi3 = value; }
        }
        public decimal Multi4
        {
            get { return _Multi4; }
            set { _Multi4 = value; }
        }
        public string Fabrikate
        {
            get { return _Fabrikate; }
            set { _Fabrikate = value; }
        }
        public string SupplierName
        {
            get { return _SupplierName; }
            set { _SupplierName = value; }
        }
        public bool IsSelected
        {
            get { return _IsSelected; }
            set { _IsSelected = value; }
        }

        private DataTable _dtStrings;
        private string _SelectedColumn;
        public DataTable dtStrings
        {
            get { return _dtStrings; }
            set { _dtStrings = value; }
        }
        public string SelectedColumn
        {
            get { return _SelectedColumn; }
            set { _SelectedColumn = value; }
        }
        public bool IsSingle
        {
            get { return _IsSingle; }
            set { _IsSingle = value; }
        }
        public string UncheckedColumn 
        {
            get { return _UncheckedColumn; }
            set { _UncheckedColumn = value; }
        }
        public string strArticleExists
        {
            get { return _strArticleExists; }
            set { _strArticleExists = value; }
        }
        public string strSupplierExists
        {
            get { return _strSupplierExists; }
            set { _strSupplierExists = value; }
        }

        public string LVSection = string.Empty;
        public object LVSectionID = null;

        public object PSupplierID = null;
        public object NotSelectedPSupplierID = null;
        public object SelectedPSupplierID = null;
    }
}