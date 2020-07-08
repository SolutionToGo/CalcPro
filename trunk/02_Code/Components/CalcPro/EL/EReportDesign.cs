using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EL
{
    public class EReportDesign
    {
        //Report Design 
        private string _Col1;
        private string _Col2;
        private string _Col3;

        //Report Settings
        private DataTable _dtReportSettings;
        private bool _LVPosition;
        private bool _ArticlNr;
        private bool _Fabrikat;
        private bool _Lieferant;
        private bool _Sender;
        private bool _Menge;
        private bool _GB;
        private bool _Prices;
        private bool _EP;
       // private bool _SeparatePrice;
        private bool _LangText;
        private bool _KurzText;
        private bool _MAMO;
        private bool _KurzAndLangText;

        private DataSet _dsReportDesign;
        public DataSet dsReportDesign
        {
            get { return _dsReportDesign; }
            set { _dsReportDesign = value; }
        }

        public string Col1
        {
            get { return _Col1; }
            set { _Col1 = value; }
        }
        public string Col2
        {
            get { return _Col2; }
            set { _Col2 = value; }
        }
        public string Col3
        {
            get { return _Col3; }
            set { _Col3 = value; }
        }


        //Report Settings
        public bool LVPosition
        {
            get { return _LVPosition; }
            set { _LVPosition = value; }
        }
        public bool ArticlNr
        {
            get { return _ArticlNr; }
            set { _ArticlNr = value; }
        }
        public bool Fabrikat
        {
            get { return _Fabrikat; }
            set { _Fabrikat = value; }
        }
        public bool Lieferant
        {
            get { return _Lieferant; }
            set { _Lieferant = value; }
        }
        public bool Sender
        {
            get { return _Sender; }
            set { _Sender = value; }
        }
        public bool Menge
        {
            get { return _Menge; }
            set { _Menge = value; }
        }
        public bool GB
        {
            get { return _GB; }
            set { _GB = value; }
        }
        public bool Prices
        {
            get { return _Prices; }
            set { _Prices = value; }
        }
        public bool EP
        {
            get { return _EP; }
            set { _EP = value; }
        }
        //public bool SeparatePrice
        //{
        //    get { return _SeparatePrice; }
        //    set { _SeparatePrice = value; }
        //}
        public bool LangText
        {
            get { return _LangText; }
            set { _LangText = value; }
        }
        public bool KurzText
        {
            get { return _KurzText; }
            set { _KurzText = value; }
        }
        public bool MAMO
        {
            get { return _MAMO; }
            set { _MAMO = value; }
        }

        public DataTable dtReportSettings
        {
            get { return _dtReportSettings; }
            set { _dtReportSettings = value; }
        }
        public bool KurzAnLangText
        {
            get { return _KurzAndLangText; }
            set { _KurzAndLangText = value; }
        }
        public bool HPos;
        public bool ABPos;
        public bool BAPos;
        public bool VRPos;
        public bool UBPos;
        public bool NonePos;
        public bool WithDetailKZ;
        public bool WithTitlePrices;
        public bool OnheMengeZeroPositions;
        public bool OnheMontagePrice;
    }
}
