using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EL
{
    public class EMulti
    {
        private object _LVSection = string.Empty;
        private int _ProjectID = -1;
        private DataTable _dtArticles = null;
        private string _Type = string.Empty;

        public object LVSection
        {
            get { return _LVSection; }
            set { _LVSection = value; }
        }

        public int ProjectID
        {
            get { return _ProjectID; }
            set { _ProjectID = value; }
        }

        public DataTable dtArticles
        {
            get { return _dtArticles; }
            set { _dtArticles = value; }
        }

        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        public DataTable dtSoldMultis;
        public DataTable dtVoldMultis;
        public bool ISMaterial = false;

        public DataTable dtLVSectionID = new DataTable();
    }
}
