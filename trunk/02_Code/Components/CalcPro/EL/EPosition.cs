using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EL
{
    public class EPosition
    {
        // Private variables to hold the LV Details values
        private int _PositionID = -1;
        private int _ProjectID = -1;
        private string _Stufe1 = string.Empty;
        private string _Stufe2 = string.Empty;
        private string _Stufe3 = string.Empty;
        private string _Stufe4 = string.Empty;
        private string _Stufe1Title = string.Empty;
        private string _Stufe2Title = string.Empty;
        private string _Stufe3Title = string.Empty;
        private string _Stufe4Title = string.Empty;
        private string _Position = string.Empty;
        private string _Position_OZ = string.Empty;
        private string _Parent_OZ = string.Empty;
        private string _Title = string.Empty;
        private string _ShortDescription = string.Empty;
        private string _LongDescription = string.Empty;
        private string _PositionKZ = string.Empty;
        private int _DetailsKZ = 0;
        private string _LVSection = string.Empty;
        private string _WG = string.Empty;
        private string _WA = string.Empty;
        private string _WI = string.Empty;
        private decimal _Menge = 0;
        private string _ME = string.Empty;
        private string _Fabricate = string.Empty;
        private string _LiefrantMA = string.Empty;
        private int _SupplierID = 0;
        private string _Type = string.Empty;
        private string _LVStatus = string.Empty;
        private int _ProposalNo = 0;
        private string _Surcharge_From = string.Empty;
        private string _Surcharge_To = string.Empty;
        private decimal _Surcharge_Per = 0;
        private int _UserID = -1;
        private DataSet _dtPositionList;
        private DataSet _dtPositionOZList;

        private decimal _surchargePercentage_MO = 0;

        //Private variables to hold the cost details values
        private DateTime _ValidityDate = DateTime.Now;
        private int _KalkMenge = 0;
        private int _MECost = 0;
        private string _MA = string.Empty;
        private string _MO = string.Empty;
        private string _Dim1 = string.Empty;
        private string _Dim2 = string.Empty;
        private string _Dim3 = string.Empty;
        private decimal _Mins = 0;
        private decimal _Faktor = 0;
        private decimal _LPMA = 0;
        private decimal _LPMO = 0;
        private decimal _Multi1MA = 1;
        private decimal _Multi1MO = 1;
        private decimal _Multi2MA = 1;
        private decimal _Multi2MO = 1;
        private decimal _Multi3MA = 1;
        private decimal _Multi3MO = 1;
        private decimal _Multi4MA = 1;
        private decimal _Multi4MO = 1;
        private decimal _Multi5MA = 1;
        private decimal _Multi5MO = 1;
        private decimal _EinkaufspreisMA = 0;
        private decimal _EinkaufspreisMO = 0;
        private decimal _SelbstkostenMultiMA = 1;
        private decimal _SelbstkostenMultiMO = 1;
        private decimal _SelbstkostenValueMA = 0;
        private decimal _SelbstkostenValueMO = 0;
        private decimal _VerkaufspreisMultiMA = 1;
        private decimal _VerkaufspreisMultiME = 1;
        private decimal _VerkaufspreisValueMA = 0;
        private decimal _VerkaufspreisValueME = 0;
        private string _PreisText = string.Empty;
        private bool _EinkaufspreisLockMA;
        private bool _EinkaufspreisLockMO;
        private bool _SelbstkostenLockMA;
        private bool _SelbstkostenLockMO;
        private bool _VerkaufspreisLockMA;
        private bool _VerkaufspreisLockMO;
        private decimal _StdSatz = 0;
        private string _DocuwareLink1 = string.Empty;
        private string _DocuwareLink2 = string.Empty;
        private string _DocuwareLink3 = string.Empty;
        private decimal _GrandTotalME = 0;
        private decimal _GrandTotalMO = 0;
        private decimal _FinalGB = 0;
        private decimal _EP = 0;
        private int _SNO = -1;
        private int _RasterCount = 0;
        private int _A = 0;
        private int _B = 0;
        private int _L = 0;
        private decimal _Discount = 0;
        private string _Dim = string.Empty;

        //Copy of LVs
        private DataTable _dtCopyNewLVs;
        private DataTable _dtCopyOldLVs;

        private DataTable _dtDimensions;
        public DataTable dtCopyNewLVs
        {
            get { return _dtCopyNewLVs; }
            set { _dtCopyNewLVs = value; }
        }
        public DataTable dtCopyOldLVs
        {
            get { return _dtCopyOldLVs; }
            set { _dtCopyOldLVs = value; }
        }

        // LV DEtails Properties
        public int PositionID
        {
            get { return _PositionID; }
            set { _PositionID = value; }
        }
        public int ProjectID
        {
            get { return _ProjectID; }
            set { _ProjectID = value; }
        }
        public string Stufe1
        {
            get { return _Stufe1; }
            set { _Stufe1 = value; }
        }
        public string Stufe2
        {
            get { return _Stufe2; }
            set { _Stufe2 = value; }
        }
        public string Stufe3
        {
            get { return _Stufe3; }
            set { _Stufe3 = value; }
        }
        public string Stufe4
        {
            get { return _Stufe4; }
            set { _Stufe4 = value; }
        }
        public string Stufe1Title
        {
            get { return _Stufe1Title; }
            set { _Stufe1Title = value; }
        }
        public string Stufe2Title
        {
            get { return _Stufe2Title; }
            set { _Stufe2Title = value; }
        }
        public string Stufe3Title
        {
            get { return _Stufe3Title; }
            set { _Stufe3Title = value; }
        }
        public string Stufe4Title
        {
            get { return _Stufe4Title; }
            set { _Stufe4Title = value; }
        }
        public string Position
        {
            get { return _Position; }
            set { _Position = value; }
        }
        public string Position_OZ
        {
            get { return _Position_OZ; }
            set { _Position_OZ = value; }
        }
        public string Parent_OZ
        {
            get { return _Parent_OZ; }
            set { _Parent_OZ = value; }
        }
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }
        public string ShortDescription 
        {
            get { return _ShortDescription; }
            set { _ShortDescription = value; }
        }
        public string LongDescription
        {
            get { return _LongDescription; }
            set { _LongDescription = value; }
        }
        public string PositionKZ
        {
            get { return _PositionKZ; }
            set { _PositionKZ = value; }
        }

        public object PositionKZID = null;

        public int DetailKZ
        {
            get { return _DetailsKZ; }
            set { _DetailsKZ = value; }
        }
        public string LVSection
        {
            get { return _LVSection; }
            set { _LVSection = value; }
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
        public string WI
        {
            get { return _WI; }
            set { _WI = value; }
        }
        public decimal Menge
        {
            get { return _Menge; }
            set { _Menge = value; }
        }
        public string ME
        {
            get { return _ME; }
            set { _ME = value; }
        }
        public string Fabricate
        {
            get { return _Fabricate; }
            set { _Fabricate = value; }
        }
        public string LiefrantMA
        {
            get { return _LiefrantMA; }
            set { _LiefrantMA = value; }
        }
        public int SupplierID
        {
            get { return _SupplierID; }
            set { _SupplierID = value; }
        }
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        public string LVStatus
        {
            get { return _LVStatus; }
            set { _LVStatus = value; }
        }

        public object LVStatusID = null;
        public int ProposalNo
        {
            get { return _ProposalNo; }
            set { _ProposalNo = value; }
        }
        public string Surcharge_From
        {
            get { return _Surcharge_From; }
            set { _Surcharge_From = value; }
        }
        public string Surcharge_To
        {
            get { return _Surcharge_To; }
            set { _Surcharge_To = value; }
        }
        public decimal Surcharge_Per
        {
            get { return _Surcharge_Per; }
            set { _Surcharge_Per = value; }
        }
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        public DataSet dsPositionList
        {
            get { return _dtPositionList; }
            set { _dtPositionList = value; }
        }
        public DataSet dsPositionOZList
        {
            get { return _dtPositionOZList; }
            set { _dtPositionOZList = value; }
        }
        public decimal surchargePercentage_MO
        {
            get { return _surchargePercentage_MO; }
            set { _surchargePercentage_MO = value; }
        }

        //Costdetails Properties
        public DateTime ValidityDate
        {
            get { return _ValidityDate; }
            set { _ValidityDate = value; }
        }
        public int KalkMenge
        {
            get { return _KalkMenge; }
            set { _KalkMenge = value; }
        }
        public int MECost
        {
            get { return _MECost; }
            set { _MECost = value; }
        }
        public string MA
        {
            get { return _MA; }
            set { _MA = value; }
        }
        public string MO
        {
            get { return _MO; }
            set { _MO = value; }
        }
        public string Dim1
        {
            get { return _Dim1; }
            set { _Dim1 = value; }
        }
        public string Dim2
        {
            get { return _Dim2; }
            set { _Dim2 = value; }
        }
        public string Dim3
        {
            get { return _Dim3; }
            set { _Dim3 = value; }
        }
        public decimal Mins
        {
            get { return _Mins; }
            set { _Mins = value;}
        }
        public decimal Faktor
        {
            get { return _Faktor; }
            set { _Faktor = value; }
        }
        public decimal LPMA
        {
            get { return _LPMA; }
            set { _LPMA = value; }
        }
        public decimal LPMO
        {
            get { return _LPMO; }
            set { _LPMO = value; }
        }
        public decimal Multi1MA
        {
            get { return _Multi1MA; }
            set { _Multi1MA = value; }
        }
        public decimal Multi1MO
        {
            get { return _Multi1MO; }
            set { _Multi1MO = value; }
        }
        public decimal Multi2MA
        {
            get { return _Multi2MA; }
            set { _Multi2MA = value; }
        }
        public decimal Multi2MO
        {
            get { return _Multi2MO; }
            set { _Multi2MO = value; }
        }
        public decimal Multi3MA
        {
            get { return _Multi3MA; }
            set { _Multi3MA = value; }
        }
        public decimal Multi3MO
        {
            get { return _Multi3MO; }
            set { _Multi3MO = value; }
        }
        public decimal Multi4MA
        {
            get { return _Multi4MA; }
            set { _Multi4MA = value; }
        }
        public decimal Multi4MO
        {
            get { return _Multi4MO; }
            set { _Multi4MO = value; }
        }
        public decimal Multi5MA
        {
            get { return _Multi5MA; }
            set { _Multi5MA = value; }
        }
        public decimal Multi5MO
        {
            get { return _Multi5MO; }
            set { _Multi5MO = value; }
        }
        public decimal EinkaufspreisMA
        {
            get { return _EinkaufspreisMA; }
            set { _EinkaufspreisMA = value; }
        }
        public decimal EinkaufspreisMO
        {
            get { return _EinkaufspreisMO; }
            set { _EinkaufspreisMO = value; }
        }
        public decimal SelbstkostenMultiMA
        {
            get { return _SelbstkostenMultiMA; }
            set { _SelbstkostenMultiMA = value; }
        }
        public decimal SelbstkostenMultiMO
        {
            get { return _SelbstkostenMultiMO; }
            set { _SelbstkostenMultiMO = value; }
        }
        public decimal SelbstkostenValueMA
        {
            get { return _SelbstkostenValueMA; }
            set { _SelbstkostenValueMA = value; }
        }
        public decimal SelbstkostenValueMO
        {
            get { return _SelbstkostenValueMO; }
            set { _SelbstkostenValueMO = value; }
        }
        public decimal VerkaufspreisMultiMA
        {
            get { return _VerkaufspreisMultiMA; }
            set { _VerkaufspreisMultiMA = value; }
        }
        public decimal VerkaufspreisMultiMO
        {
            get { return _VerkaufspreisMultiME; }
            set { _VerkaufspreisMultiME = value; }
        }
        public decimal VerkaufspreisValueMA
        {
            get { return _VerkaufspreisValueMA; }
            set { _VerkaufspreisValueMA = value; }
        }
        public decimal VerkaufspreisValueMO
        {
            get { return _VerkaufspreisValueME; }
            set { _VerkaufspreisValueME = value; }
        }
        public string PreisText
        {
            get { return _PreisText; }
            set { _PreisText = value; }
        }
        public bool EinkaufspreisLockMA
        {
            get { return _EinkaufspreisLockMA; }
            set { _EinkaufspreisLockMA = value; }
        }
        public bool EinkaufspreisLockMO
        {
            get { return _EinkaufspreisLockMO; }
            set { _EinkaufspreisLockMO = value; }
        }
        public bool SelbstkostenLockMA
        {
            get { return _SelbstkostenLockMA; }
            set { _SelbstkostenLockMA = value; }
        }
        public bool SelbstkostenLockMO
        {
            get { return _SelbstkostenLockMO; }
            set { _SelbstkostenLockMO = value; }
        }
        public bool VerkaufspreisLockMA
        {
            get { return _VerkaufspreisLockMA; }
            set { _VerkaufspreisLockMA = value; }
        }
        public bool VerkaufspreisLockMO
        {
            get { return _VerkaufspreisLockMO; }
            set { _VerkaufspreisLockMO = value; }
        }
        public decimal StdSatz
        {
            get { return _StdSatz; }
            set { _StdSatz = value; }
        }
        public string DocuwareLink1
        {
            get { return _DocuwareLink1; }
            set { _DocuwareLink1 = value; }
        }
        public string DocuwareLink2
        {
            get { return _DocuwareLink2; }
            set { _DocuwareLink2 = value; }
        }
        public string DocuwareLink3
        {
            get { return _DocuwareLink3; }
            set { _DocuwareLink3 = value; }
        }
        public object LVSectionID;
        public decimal GrandTotalME
        {
            get { return _GrandTotalME; }
            set { _GrandTotalME = value; }
        }
        public decimal GrandTotalMO
        {
            get { return _GrandTotalMO; }
            set { _GrandTotalMO = value; }
        }
        public decimal FinalGB
        {
            get { return _FinalGB; }
            set { _FinalGB = value; }
        }
        public decimal EP
        {
            get { return _EP; }
            set { _EP = value; }
        }
        public int SNO
        {
            get { return _SNO; }
            set { _SNO = value; }
        }
        public int RasterCount
        {
            get { return _RasterCount; }
            set { _RasterCount = value; }
        }
        public DataTable dtDimensions
        {
            get { return _dtDimensions; }
            set { _dtDimensions = value; }
        }

        private int _ParentID;
        public int ParentID
        {
            get { return _ParentID; }
            set { _ParentID = value; }
        }
        public int A
        {
            get { return _A; }
            set { _A = value; }
        }
        public int B
        {
            get { return _B; }
            set { _B = value; }
        }
        public int L
        {
            get { return _L; }
            set { _L = value; }
        }

        private DataTable _dtCopyPosition;
        public DataTable dtCopyPosition
        {
            get { return _dtCopyPosition; }
            set { _dtCopyPosition = value; }
        }       
        public decimal Discount
        {
            get { return _Discount; }
            set { _Discount = value; }
        }
        public string Dim
        {
            get { return _Dim; }
            set { _Dim = value; }
        }
        public DataTable dtArticle { get; set; }
        public double OZID = 0;
        public DataTable dtProjectArticles { get; set; }
        public string WGDescription { get; set; }
        public string WADescription { get; set; }

        public bool MontageEntry = false;

        public string LVRaster = string.Empty;

        public int RoundOffValue = 2;

        public DataRow drnewrow = null;
    }
}