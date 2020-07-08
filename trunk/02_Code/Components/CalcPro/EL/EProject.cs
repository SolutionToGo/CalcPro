using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EL
{
    public class EProject
    {
        /// <summary>
        /// Private Variables for Project Modules
        /// </summary>
        private int _ProjectID = -1;
        private string _Location = "Bangalore";
        private string _ProjectNumber;
        private string _CommissionNumber;
        private string _LVRaster;
        private string _LVRaster_GAEB = "fdsfdsafdsa";
        private decimal _Vat;
        private int _LVSprunge;
        private string _ProjectDescription;
        private int _KundeID = 1;
        private int _PlannnerID = 1;
        private int _ProjectDuration;
        private decimal _InternX;
        private decimal _InternS;
        private string _Submitlocation;
        private DateTime _SubmitDateTime;
        private DateTime _SubmitTime;
        private int _EstimatedLVs;
        private int _ActualLvs;
        private int _RoundingPriceID = 1;
        private decimal _Discount;
        private string _Remarks;
        private bool _LockHierarcy;
        private DateTime _ProjectStartDate;
        private DateTime _ProjectEndDate;
        private DataTable _dtProjectList = null;
        private bool _IsDisable = false;
        private bool _IsFinalInvoice = false;
        private string _Status = string.Empty;
        private string _OldRaster;
        private string _FromOZ = string.Empty;
        private string _ToOZ = string.Empty;
        private bool _IsSave = false;
        private int _UserID = -1;
        private int _DiscountID = -1;
        private int _DiscountPosID = -1;
        private DataTable _dtDiscountList;
        private string _ShortDescription = string.Empty;
        private bool _IsRasterChange = false;
        private string _UserName = string.Empty;
        private string _CoverSheetPath = string.Empty;
        private string _TemplatePath = string.Empty;

        private DataTable _dtCockpitData;
        private DataTable _dtTemplateData;

        private decimal _SValue = 0;
        private decimal _XValue = 0;
        private decimal _UmlageCost = 0;
        private decimal _RevenueTotal = 0;

        public string FromOZ
        {
            get { return _FromOZ; }
            set { _FromOZ = value; }
        }
        public string ToOZ
        {
            get { return _ToOZ; }
            set { _ToOZ = value; }
        }
        public bool IsSave
        {
            get { return _IsSave; }
            set { _IsSave = value; }
        }
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        public int DiscountID
        {
            get { return _DiscountID; }
            set { _DiscountID = value; }
        }
        public int DiscountPosID
        {
            get { return _DiscountPosID; }
            set { _DiscountPosID = value; }
        }


        /// <summary>
        /// Temparary Private Variables for Project Modules
        /// </summary>
        private string _PlannerName;
        private string _KundeNr;
        private string _KundeName;
        private string _KundeAddress;
        private int _RoundingPrice =2;
        private DataTable _dtLVSection;
        private bool _IsCumulated = false;
        private DataTable _dtProjecNumber = null;
        private DataTable _dtArticleSettings = new DataTable();
        private DataTable _dtDiscount = null;
        public DataTable dtProjecNumber
        {
            get { return _dtProjecNumber; }
            set { _dtProjecNumber = value; }
        }
        public DataTable dtDiscount
        {
            get { return _dtDiscount; }
            set { _dtDiscount = value; }
        }

        public DataTable dtArticleSettings
        {
            get { return _dtArticleSettings; }
            set { _dtArticleSettings = value; }
        }

        public DataTable dtQuerCalc = null;

        private DataSet _dsComaparePrice = null;
        public DataSet dsComaparePrice
        {
            get { return _dsComaparePrice; }
            set { _dsComaparePrice = value; }
        }

        /// <summary>
        /// Public Properties for Project Module
        /// </summary>
        public int ProjectID
        {
            get { return _ProjectID; }
            set { _ProjectID = value; }
        }
        public string Location
        { get { return _Location; } set { _Location = value; } }
        public string ProjectNumber
        {
            get { return _ProjectNumber; }
            set { _ProjectNumber = value; }
        }

        public string ProjectName;
        public string CommissionNumber
        {
            get { return _CommissionNumber; }
            set { _CommissionNumber = value; }
        }
        public string LVRaster
        {
            get { return _LVRaster; }
            set { _LVRaster = value; }
        }
        public string LVRaster_GAEB
        { get { return _LVRaster_GAEB; } set { _LVRaster_GAEB = value; } }
        public decimal Vat
        {
            get { return _Vat; }
            set { _Vat = value; }
        }
        public int LVSprunge
        {
            get { return _LVSprunge; }
            set { _LVSprunge = value; }
        }
        public string ProjectDescription
        {
            get { return _ProjectDescription; }
            set { _ProjectDescription = value; }
        }
        public int KundeID
        {
            get { return _KundeID; }
            set { _KundeID = value; }
        }
        public int PlannedID
        {
            get { return _PlannnerID; }
            set { _PlannnerID = value; }
        }
        public int ProjectDuration
        {
            get { return _ProjectDuration; }
            set { _ProjectDuration = value; }
        }
        public decimal InternX
        {
            get { return _InternX; }
            set { _InternX = value; }
        }
        public decimal InternS
        {
            get { return _InternS; }
            set { _InternS = value; }
        }
        public string Submitlocation
        {
            get { return _Submitlocation; }
            set { _Submitlocation = value; }
        }
        public DateTime SubmitDate
        {
            get { return _SubmitDateTime; }
            set { _SubmitDateTime = value; }
        }
        public DateTime SubmitTime
        { get { return _SubmitTime; } set { _SubmitTime = value; } }
        public int EstimatedLvs
        {
            get { return _EstimatedLVs; }
            set { _EstimatedLVs = value; }
        }
        public int RoundingPriceID
        {
            get { return _RoundingPriceID; }
            set { _RoundingPriceID = value; }
        }
        public decimal Discount
        {
            get { return _Discount; }
            set { _Discount = value; }
        }
        public string Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
        }
        public bool LockHierarchy
        {
            get { return _LockHierarcy; }
            set { _LockHierarcy = value; }
        }
        public DataTable dtProjectList
        {
            get { return _dtProjectList; }
            set { _dtProjectList = value; }
        }
        public int ActualLvs
        {
            get { return _ActualLvs; }
            set { _ActualLvs = value; }
        }
        public DateTime ProjectStartDate
        {
            get { return _ProjectStartDate; }
            set { _ProjectStartDate = value; }
        }
        public DateTime ProjectEndDate
        {
            get { return _ProjectEndDate; }
            set { _ProjectEndDate = value; }
        }
        public string OldRaster
        {
            get { return _OldRaster; }
            set { _OldRaster = value; }
        }

        public int KundeNr{get;set;}
        public string KundeName
        {
            get { return _KundeName; }
            set { _KundeName = value; }
        }
        public string KundeAddress
        {
            get { return _KundeAddress; }
            set { _KundeAddress = value; }
        }
        public string PlannerName
        {
            get { return _PlannerName; }
            set { _PlannerName = value; }
        }
        public int RoundingPrice
        {
            get { return _RoundingPrice; }
            set { _RoundingPrice = value; }
        }
        public bool IsDisable
        {
            get { return _IsDisable; }
            set { _IsDisable = value; }
        }
        public DataTable dtLVSection
        {
            get { return _dtLVSection; }
            set { _dtLVSection = value; }
        }
        public bool IsCumulated
        {
            get { return _IsCumulated; }
            set { _IsCumulated = value; }
        }
        public bool IsFinalInvoice
        {
            get { return _IsFinalInvoice; }
            set { _IsFinalInvoice = value; }
        }
        public string Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        public DataTable dtDiscountList
        {
            get { return _dtDiscountList; }
            set { _dtDiscountList = value; }
        }
        public string ShortDescription
        {
            get { return _ShortDescription; }
            set { _ShortDescription = value; }
        }
        public bool IsRasterChange
        {
            get { return _IsRasterChange; }
            set { _IsRasterChange = value; }
        }
        public DataTable dtCockpitData
        {
            get { return _dtCockpitData; }
            set { _dtCockpitData = value; }
        }
        public DataTable dtTemplateData
        {
            get { return _dtTemplateData; }
            set { _dtTemplateData = value; }
        }
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }
        public string CoverSheetPath
        {
            get { return _CoverSheetPath; }
            set { _CoverSheetPath = value; }
        }
        public string TemplatePath
        {
            get { return _TemplatePath; }
            set { _TemplatePath = value; }
        }

        public decimal SValue
        {
            get { return _SValue; }
            set { _SValue = value; }
        }
        public decimal XValue
        {
            get { return _XValue; }
            set { _XValue = value; }
        }
        public decimal UmlageCost
        {
            get { return _UmlageCost; }
            set { _UmlageCost = value; }
        }
        public decimal RevenueTotal
        {
            get { return _RevenueTotal; }
            set { _RevenueTotal = value; }
        }


        public bool lPVisible = true;
        public bool M1Visible = true;
        public bool M2Visible = true;
        public bool M3Visible = true;
        public bool M4Visible = true;
        public bool DimVisible = true;
        public bool MinVisible = true;
        public bool FabVisible = true;
        public bool MeVisible = true;

        public bool ShowVK = false;

        public object LVSectionID = null;
    }
}

