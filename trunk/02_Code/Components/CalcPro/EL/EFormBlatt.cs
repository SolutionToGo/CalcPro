using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EL
{
    public class EFormBlatt
    {
        private int _ProjectID = -1;
        private int _LookUpID= -1;
        private DataTable _dtBlatt221_1 = null;
        private DataTable _dtProjectDetails = null;
        private DataTable _dtBlatt221_2 = null;
        private DataTable _dtBlattTypes = null;
        private DataTable _dtBlattArticles = null;

        public int ProjectID
        {
            get { return _ProjectID; }
            set { _ProjectID = value; }
        }
        public int LookUpID
        {
            get { return _LookUpID; }
            set { _LookUpID = value; }
        }
        public DataTable dtBlatt221_1
        {
            get { return _dtBlatt221_1; }
            set { _dtBlatt221_1 = value; }
        }

        public DataTable dtProjectDetails
        {
            get { return _dtProjectDetails; }
            set { _dtProjectDetails = value; }
        }

        public DataTable dtBlatt221_2
        {
            get { return _dtBlatt221_2; }
            set { _dtBlatt221_2 = value; }
        }

        public DataTable dtBlattTypes
        {
            get { return _dtBlattTypes; }
            set { _dtBlattTypes = value; }
        }

        public DataTable dtBlattArticles
        {
            get { return _dtBlattArticles; }
            set { _dtBlattArticles = value; }
        }
    }
}
