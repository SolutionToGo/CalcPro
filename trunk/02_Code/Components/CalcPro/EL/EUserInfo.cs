using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EL
{
    public class EUserInfo
    {
        /// <summary>
        /// It contains Variables and Entities of User master, Roles and Access features 
        /// </summary>


        //To hold the UserInfo Details
        private int _UserID = -1;
        private string _UserName;
        private string _FirstName;
        private string _LastName;
        private string _Password;
        private string _PasswordSalt;
        private string _MobileNo;
        private string _EmailID;
        private DataSet _dsUserInfo;
        private DataTable _dtUserInfo;

        //To hold Feature Details
        private int _RoleFeatureMapID = -1;
        private int _FeatureID;
        private int _AccessLevelID;
        private DataSet _dsFeature;
        private DataTable _dtFeature;

        // Feature Properties
        public int RoleFeatureMapID
        {
            get { return _RoleFeatureMapID; }
            set { _RoleFeatureMapID = value; }
        }
        public int FeatureID
        {
            get { return _FeatureID; }
            set { _FeatureID = value; }
        }
        public int AccessLevelID
        {
            get { return _AccessLevelID; }
            set { _AccessLevelID = value; }
        }

        public DataSet dsFeature
        {
            get { return _dsFeature; }
            set { _dsFeature = value; }
        }

        public DataTable dtFeature
        {
            get { return _dtFeature; }
            set { _dtFeature = value; }
        }

        public DataTable dtLVStatus = null;
        public DataTable dtPositionKZ = null;

        //To hold the UserRole Details
        private int _RoleID = -1;
        private string _RoleName;
        private DataSet _dsUserRole;
        private DataTable _dtUserRole;
        private DataTable _dtAccessLevels;

        public DataTable dtAccessLevels
        {
            get { return _dtAccessLevels; }
            set { _dtAccessLevels = value; }
        }

        // UserInfo Properties
        public DataSet dsUserInfo
        {
            get { return _dsUserInfo; }
            set { _dsUserInfo = value; }
        }

        public DataTable dtUserInfo
        {
            get { return _dtUserInfo; }
            set { _dtUserInfo = value; }
        }

        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }
        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }
        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; }
        }
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }
        public string PasswordSalt
        {
            get { return _PasswordSalt; }
            set { _PasswordSalt = value; }
        }
        public string MobileNo
        {
            get { return _MobileNo; }
            set { _MobileNo = value; }
        }
        public string EmailID
        {
            get { return _EmailID; }
            set { _EmailID = value; }
        }

        // UserRole Properties
        public DataSet dsUserRole
        {
            get { return _dsUserRole; }
            set { _dsUserRole = value; }
        }

        public DataTable dtUserRole
        {
            get { return _dtUserRole; }
            set { _dtUserRole = value; }
        }

        public int RoleID
        {
            get { return _RoleID; }
            set { _RoleID = value; }
        }
        public string RoleName
        {
            get { return _RoleName; }
            set { _RoleName = value; }
        }

        private DataTable _dtUserDetails;
        public DataTable dtUserDetails
        {
            get { return _dtUserDetails; }
            set { _dtUserDetails = value; }
        }
        private bool _IsAdmin;
        private string _OldPassword = string.Empty;
        private string _NewPassword = string.Empty;

        public bool IsAdmin
        {
            get { return _IsAdmin; }
            set { _IsAdmin = value; }
        }
        public string OldPassword
        {
            get { return _OldPassword; }
            set { _OldPassword = value; }
        }
        public string NewPassword
        {
            get { return _NewPassword; }
            set { _NewPassword = value; }

        }
        public bool AutoSaveMode = false;
    }
}
