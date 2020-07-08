using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EL
{
    public class EArticles
    {
        /// <summary>
        /// It contains variables and properties of Article module i.e/ Article data, Typ , Rabatt, Accessories
        /// </summary>
        private int _WGID = -1;
        private string _WG = string.Empty;
        private string _WA = string.Empty;
        private string _WGDescription;
        private string _WADescription;
        private int _WIID = -1;
        private string _WI = string.Empty;
        private string _WIDescription;
        private string _Fabrikate;
        private string _Masseinheit;
        private string _Typ;
        private string _Dimension;
        private string _Menegenheit;
        private string _Remarks;
        private string _TextKZ;
        private DateTime _ValidityDate;
        private decimal _Multi1 = 1;
        private decimal _Multi2 = 1;
        private decimal _Multi3 = 1;
        private decimal _Multi4 =1;
        private string _DataNormNumber;
        private int _CreatedBy;
        private int _LastUpdatedBy;
        private DataTable _dtWG;
        private DataTable _dtWI;
        private DataTable _dtDimenstions;

        private int _DimensionID;
	    private string _A;
	    private string _B;
	    private string _L;
	    private decimal _ListPrice = 0;
        private decimal _GMulti;
	    private decimal _Minuten = 0;

        private int _TypID = -1;
        private int _SupplierID;
        private DataTable _dtTyp;
        private DataTable _dtSupplier;

        private int _RabbatID = -1;
        private DataTable _dtRabatt;
        private string _Rabatt;
        private DataTable _dtTypID;
        private DataTable _dtID;

        private int _ParentID;
        private int _ChildID;
        private int _UserID;
        private int _AccessoriesID;

        public int WGID
        {
            get { return _WGID; }
            set { _WGID = value; }
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
        public string WADescription
        {
            get { return _WADescription; }
            set { _WADescription = value; }
        }
        public int WIID
        {
            get { return _WIID; }
            set { _WIID = value; }
        }
        public string WI
        {
            get { return _WI; }
            set { _WI = value; }
        }
        public string WIDescription
        {
            get { return _WIDescription; }
            set { _WIDescription = value; }
        }
        public string Fabrikate
        {
            get { return _Fabrikate; }
            set { _Fabrikate = value; }
        }
        public string Masseinheit
        {
            get { return _Masseinheit; }
            set { _Masseinheit = value; }
        }
        public string Dimension
        {
            get { return _Dimension; }
            set { _Dimension = value; }
        }
        public string Menegenheit
        {
            get { return _Menegenheit; }
            set { _Menegenheit = value; }
        }
        public string Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
        }
        public string TextKZ
        {
            get { return _TextKZ; }
            set { _TextKZ = value; }
        }
        public DateTime ValidityDate
        {
            get { return _ValidityDate; }
            set { _ValidityDate = value; }
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
        public string DataNormNumber
        {
            get { return _DataNormNumber; }
            set { _DataNormNumber = value; }
        }
        public int CreatedBy
        {
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        }
        public int LastUpdatedBy
        {
            get { return _LastUpdatedBy; }
            set { _LastUpdatedBy = value; }
        }
        public string Typ
        {
            get { return _Typ; }
            set { _Typ = value; }
        }
        public DataTable dtWG
        {
            get { return _dtWG; }
            set { _dtWG = value; }
        }

        public DataTable dtArt = null;
        public DataTable dtWI
        {
            get { return _dtWI; }
            set { _dtWI = value; }
        }
        public DataTable dtDimenstions
        {
            get { return _dtDimenstions; }
            set { _dtDimenstions = value; }
        }
        
        public int DimensionID
        {
            get { return _DimensionID; }
            set { _DimensionID = value; }
        }
        public string A
        {
            get { return _A; }
            set { _A = value; }
        }
        public string B
        {
            get { return _B; }
            set { _B = value; }
        }
        public string L
        {
            get { return _L; }
            set { _L = value; }
        }
        public decimal ListPrice
        {
            get { return _ListPrice; }
            set { _ListPrice = value; }
        }
        public decimal GMulti
        {
            get { return _GMulti; }
            set { _GMulti = value; }
        }
        public decimal Minuten
        {
            get { return _Minuten; }
            set { _Minuten = value; }
        }

        public int TypID
        {
            get { return _TypID; }
            set { _TypID = value; }
        }
        public int SupplierID
        {
            get { return _SupplierID; }
            set { _SupplierID = value; }
        }
        public DataTable dtTyp
        {
            get { return _dtTyp; }
            set { _dtTyp = value; }
        }
        public DataTable dtSupplier
        {
            get { return _dtSupplier; }
            set { _dtSupplier = value; }
        }
        public DataTable dtRabatt
        {
            get { return _dtRabatt; }
            set { _dtRabatt = value; }
        }
        public int RabattID
        {
            get { return _RabbatID; }
            set { _RabbatID = value; }
        }
        public string Rabatt
        {
            get { return _Rabatt; }
            set { _Rabatt = value; }
        }

        private DataTable _dtArticleImport = null;
        private DataTable _dtDimensionImport = null;
        public DataTable dtArticleImport
        {
            get { return _dtArticleImport; }
            set { _dtArticleImport = value; }
        }
        public DataTable dtDimensionImport
       {
           get { return _dtDimensionImport; }
           set { _dtDimensionImport = value; }
       }

        public int ParentID
        {
            get { return _ParentID; }
            set { _ParentID = value; }
        }
        public int ChildID
        {
            get { return _ChildID; }
            set { _ChildID = value; }
        }
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        public int AccessoriesID
        {
            get { return _AccessoriesID; }
            set { _AccessoriesID = value; }
        }
        
        private DataTable _dtAccessories;
        public DataTable dtAccessories
        {
            get { return _dtAccessories; }
            set { _dtAccessories = value; }
        }

        private DataTable _dtArticleDetails;
        public DataTable dtArticleDetails
        {
            get { return _dtArticleDetails;}
            set { _dtArticleDetails = value; }
        }

        public DataTable dtDimensionValidityDates = null;
        public DataTable dtDimensionValues = null;
        public DataTable dtMultiValues = null;

        private string _ChildWG;
        private string _ChildWA;
        private string _ChildWI;
        private string _ChildA;
        private string _ChildB;
        private string _ChildL;

        public string ChildWG
        {
            get { return _ChildWG; }
            set { _ChildWG = value; }
        }
        public string ChildWA
        {
            get { return _ChildWA; }
            set { _ChildWA = value; }
        }
        public string ChildWI
        {
            get { return _ChildWI; }
            set { _ChildWI = value; }
        }
        public string ChildA
        {
            get { return _ChildA; }
            set { _ChildA = value; }
        }
        public string ChildB
        {
            get { return _ChildB; }
            set { _ChildB = value; }
        }
        public string ChildL
        {
            get { return _ChildL; }
            set { _ChildL = value; }
        }
        public DataTable dtTypID
        {
            get { return _dtTypID; }
            set { _dtTypID = value; }
        }
        public DataTable dtID
        {
            get { return _dtID; }
            set { _dtID = value; }
        }
        public DataSet dsRabatt { get; set; }
        public int RID { get; set; }
        public int RabattTypID { get; set; }
        public bool IsContinue = false;
        public string Flag { get; set; }
        public int RTID { get; set; }
        public bool IsCopy { get; set; }
        public DataTable dtDim{get;set;}
    }
}
