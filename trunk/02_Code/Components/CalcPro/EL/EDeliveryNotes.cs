using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EL
{
    public class EDeliveryNotes
    {
        private int _ProjectID = -1;
        private DataTable _dtPositions;
        private DataTable _dtDelivery;
        private bool _ISActiveDelivery;
        private DataTable _dtNonActivedelivery;
        private string _BlattName;
        private DataTable _dtBlattNumbers;
        private int _BlattID = -1;

        public int ProjectID
        {
            get { return _ProjectID; }
            set { _ProjectID = value; }
        }
        
        public DataTable dtPositions
        {
            get { return _dtPositions; }
            set { _dtPositions = value; }
        }

        public DataTable dtDelivery
        {
            get { return _dtDelivery; }
            set { _dtDelivery = value; }
        }

        public bool ISActiveDelivery
        {
            get { return _ISActiveDelivery; }
            set { _ISActiveDelivery = value; }
        }

        public DataTable dtNonActivedelivery
        {
            get { return _dtNonActivedelivery; }
            set { _dtNonActivedelivery = value; }
        }

        public string BlattName
        {
            get { return _BlattName; }
            set { _BlattName = value; }
        }

        public DataTable dtBlattNumbers
        {
            get { return _dtBlattNumbers; }
            set { _dtBlattNumbers = value; }
        }

        public int BlattID
        {
            get { return _BlattID; }
            set { _BlattID = value; }
        }
    }
}
