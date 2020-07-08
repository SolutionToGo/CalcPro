using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EL
{
    public class EProposal
    {
        //To hold the Text Module details
        private int _TextID = -1;
        private string _TextModuleArea;
        private string _Category;
        private bool _IsSelect;
        private string _Contents;
        private int _TextAreaID = -1;
        private int _TextCategoryID = -1;
        private DataSet _dsTextModule;       


        //To Hold Category
        private int _CategoryID = -1;
        private string _CategoryName;
        private DataSet _dsCategory;


        //To hold Text module areas
        private DataSet _dsTextModuleAreas;


        // Text Module Properties
        public DataSet dsTextModule
        {
            get { return _dsTextModule; }
            set { _dsTextModule = value; }
        }
        public int TextID
        {
            get { return _TextID; }
            set { _TextID = value; }
        }
        public string TextModuleArea
        {
            get { return _TextModuleArea; }
            set { _TextModuleArea = value; }
        }
        public string Category
        {
            get { return _Category; }
            set { _Category = value; }
        }
        public bool IsSelect
        {
            get { return _IsSelect; }
            set { _IsSelect = value; }
        }

        public string Contents
        {
            get { return _Contents; }
            set { _Contents = value; }
        }

        public int TextAreaID
        {
            get { return _TextAreaID; }
            set { _TextAreaID = value; }
        }
        public int TextCategoryID
        {
            get { return _TextCategoryID; }
            set { _TextCategoryID = value; }
        }

        // Category Properties
        public DataSet dsCategory
        {
            get { return _dsCategory; }
            set { _dsCategory = value; }
        }
        public int CategoryID
        {
            get { return _CategoryID; }
            set { _CategoryID = value; }
        }
        public string CategoryName
        {
            get { return _CategoryName; }
            set { _CategoryName = value; }
        }

        public DataSet dsTextModuleAreas
        {
            get { return _dsTextModuleAreas; }
            set { _dsTextModuleAreas = value; }
        }


        


    }
}
