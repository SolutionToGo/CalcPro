using DataAccess;
using EL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BL
{     
    public class BUserInfo
    {
        DUserInfo ObjDUserInfo = new DUserInfo();
        
        /// <summary>
        ///  Code to send request to DL to add and edit user details
        /// </summary>
        /// <param name="ObjEUserInfo"></param>
        /// <returns></returns>
        public int SaveUserDetails(EUserInfo ObjEUserInfo)
        {
            try
            {
                int UserID = -1;
                XmlDocument Xdoc = new XmlDocument();
                string XPath = "/Nouns/UserInfo";
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "UserID", ObjEUserInfo.UserID.ToString());
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "RoleID", ObjEUserInfo.RoleID.ToString());
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "UserName", ObjEUserInfo.UserName.ToLower());
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "FirstName", ObjEUserInfo.FirstName);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "LastName", ObjEUserInfo.LastName);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "Password", ObjEUserInfo.Password);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "MobileNo", ObjEUserInfo.MobileNo);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "EmailID", ObjEUserInfo.EmailID);

                UserID = ObjDUserInfo.SaveUserDetails(Xdoc, ObjEUserInfo);
                return UserID;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        ///  Code to Get User list from DL
        /// </summary>
        /// <param name="ObjEUserInfo"></param>
        /// <returns></returns>
        public EUserInfo GetUser(EUserInfo ObjEUserInfo)
        {
            try
            {
                if (ObjEUserInfo != null)
                {
                    ObjEUserInfo.dsUserInfo = ObjDUserInfo.GetUser();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEUserInfo;
        }

        /// <summary>
        ///  Code to fetch User Roles from DL
        /// </summary>
        /// <param name="ObjEUserInfo"></param>
        /// <returns></returns>
        public EUserInfo GetUserRoles(EUserInfo ObjEUserInfo)
        {
            try
            {
                if (ObjEUserInfo != null)
                {
                    ObjEUserInfo.dsUserRole = ObjDUserInfo.GetUserRoles();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEUserInfo;
        }

        /// <summary>
        /// Code to get access features list from DL
        /// </summary>
        /// <param name="ObjEUserInfo"></param>
        /// <returns></returns>
        public EUserInfo GetFeatureData(EUserInfo ObjEUserInfo)
        {
            try
            {
                if (ObjEUserInfo != null)
                {
                    ObjEUserInfo = ObjDUserInfo.GetFeatureDetails(ObjEUserInfo);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEUserInfo;
        }

       /// <summary>
       /// Code to fetch user access levels from DL
       /// </summary>
       /// <param name="ObjEUserInfo"></param>
       /// <returns></returns>
        public EUserInfo GetAceesLevels(EUserInfo ObjEUserInfo)
        {
            try
            {
                if (ObjEUserInfo != null)
                {
                    ObjEUserInfo = ObjDUserInfo.GetAceesLevels(ObjEUserInfo);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEUserInfo;
        }

        /// <summary>
        /// Code to send request to DL while saving Role feature map
        /// </summary>
        /// <param name="ObjEUserInfo"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public EUserInfo SaveFeatureMap(EUserInfo ObjEUserInfo,DataTable dt)
        {
            try
            {
                if (ObjEUserInfo != null)
                {
                    ObjEUserInfo = ObjDUserInfo.SaveFeatureMap(ObjEUserInfo, dt);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEUserInfo;
        }

        /// <summary>
        /// Code to send request to DL while check userinformation while logging in.
        /// </summary>
        /// <param name="ObjEUserInfo"></param>
        /// <returns></returns>
        public EUserInfo CheckUserCredentials(EUserInfo ObjEUserInfo)
        {
            try
            {
                ObjEUserInfo = ObjDUserInfo.CheckUserCredentials(ObjEUserInfo);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEUserInfo;
        }

        /// <summary>
        ///  Code to send request to DL while changing the USer's Password
        /// </summary>
        /// <param name="ObjEUserInfo"></param>
        /// <returns></returns>
        public EUserInfo ResetPassword(EUserInfo ObjEUserInfo)
        {
            try
            {
                ObjEUserInfo = ObjDUserInfo.ResetPassword(ObjEUserInfo);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEUserInfo;
        }
    }
}
