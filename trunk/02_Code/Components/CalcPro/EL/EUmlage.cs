using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EL
{
    public class EUmlage
    {
        /// <summary>
        ///  It Contains entities of  special cost module
        /// </summary>

        private int _ProjectID = -1;
        private DataTable _dtSpecialCost = null;
        private DataTable _dtUmlage = null;
        private decimal _UmlageFactor = 0;
        private decimal _UmlageValue = 0;

        public int ProjectID
        {
            get { return _ProjectID; }
            set { _ProjectID = value; }
        }
        public DataTable dtSpecialCost
        {
            get { return _dtSpecialCost; }
            set { _dtSpecialCost = value; }
        }
        public DataTable dtUmlage
        {
            get { return _dtUmlage; }
            set { _dtUmlage = value; }
        }
        public decimal UmlageFactor
        {
            get { return _UmlageFactor; }
            set { _UmlageFactor = value; }
        }
        public decimal UmlageValue
        {
            get { return _UmlageValue; }
            set { _UmlageValue = value; }
        }
        public int UmlageMode = 0;
    }
}
