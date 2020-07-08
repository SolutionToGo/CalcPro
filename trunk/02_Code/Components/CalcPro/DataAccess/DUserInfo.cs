using DAL;
using EL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DataAccess
{
    public class DUserInfo
    {
        /// <summary>
        /// Code to save User details while adding or editing from user master
        /// </summary>
        /// <param name="XmlDoc"></param>
        /// <param name="ObjEUserInfo"></param>
        /// <returns></returns>
        public int SaveUserDetails(XmlDocument XmlDoc, EUserInfo ObjEUserInfo)
        {
            int UserID = -1;
            DataSet ds = new DataSet();
            try
            {

                string innerxml = XmlDoc.InnerXml;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Ins_UserInfo]";
                    SqlParameter param = new SqlParameter("@XMLUserInfo", SqlDbType.Xml);
                    param.Value = innerxml;
                    cmd.Parameters.Add(param);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
                    }
                    string str = ds.Tables[0].Rows[0][0] == DBNull.Value ? "" : ds.Tables[0].Rows[0][0].ToString();
                    if (!string.IsNullOrEmpty(str))
                    {
                        if (int.TryParse(str, out UserID))
                        {
                            ObjEUserInfo.UserID = UserID;
                            ObjEUserInfo.dtUserInfo = ds.Tables[0];
                        }
                        else if (str.ToString().Contains("UNIQUE"))
                        {
                            if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                            {
                                throw new Exception("Der Nutzername existiert bereits!");
                            }
                            else
                            {
                                throw new Exception("UserName is already exists.!");
                            }
                        }
                        else
                            throw new Exception(str);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                SQLCon.Close();
            }
            return UserID;
        }

        /// <summary>
        /// Code to get user list from database
        /// </summary>
        /// <returns></returns>
        public DataSet GetUser()
        {
            DataSet dsUser = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_UserInfo]";
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsUser);
                    }
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                {
                    throw new Exception("Fehler beim Laden der Nutzer");
                }
                else
                {
                    throw new Exception("Error Occured While Retreiving Users");

                }
            }
            finally
            {
                SQLCon.Close();
            }
            return dsUser;
        }

        /// <summary>
        /// Code to fetch User Roles list for user master
        /// </summary>
        /// <returns></returns>
        public DataSet GetUserRoles()
        {
            DataSet dsUserRole = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_UserRoles]";
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsUserRole);
                    }
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                {
                    throw new Exception("Fehler beim Laden der Rollen");
                }
                else
                {
                    throw new Exception("Error Occured While Retreiving UserRoles");

                }
            }
            finally
            {
                SQLCon.Close();
            }
            return dsUserRole;
        }

        /// <summary>
        /// Code to fetch feature list from database
        /// </summary>
        /// <param name="ObjEUserInfo"></param>
        /// <returns></returns>
        public EUserInfo GetFeatureDetails(EUserInfo ObjEUserInfo)
        {
            DataSet dsFeature = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_Feature]";
                    cmd.Parameters.Add("@RoleID", ObjEUserInfo.RoleID);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsFeature);
                    }
                    if (dsFeature != null && dsFeature.Tables.Count > 0)
                    {
                        ObjEUserInfo.dtFeature = dsFeature.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                {
                    throw new Exception("Fehler beim Laden von Daten");
                }
                else
                {
                    throw new Exception("Error Occured While Retreiving Features");

                }
            }
            finally
            {
                SQLCon.Close();
            }
            return ObjEUserInfo;
        }

        /// <summary>
        /// Code to fetch access levels for selected role
        /// </summary>
        /// <param name="ObjEUserInfo"></param>
        /// <returns></returns>
        public EUserInfo GetAceesLevels(EUserInfo ObjEUserInfo)
        {
            DataSet dsAccessLevels = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_AccessLevels]";
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsAccessLevels);
                    }
                    if (dsAccessLevels != null && dsAccessLevels.Tables.Count > 0)
                    {
                        ObjEUserInfo.dtAccessLevels = dsAccessLevels.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                {
                    throw new Exception("Fehler beim Laden der Zugangsinformationen");
                }
                else
                {
                    throw new Exception("Error Occured While Retreiving Access Levels");

                }
            }
            finally
            {
                SQLCon.Close();
            }
            return ObjEUserInfo;
        }

        /// <summary>
        /// Code to save Role and feature mapping
        /// </summary>
        /// <param name="ObjEUserInfo"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public EUserInfo SaveFeatureMap(EUserInfo ObjEUserInfo,DataTable dt)
        {
            DataSet dsFeature = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Ins_RoleFeatureMap]";
                    cmd.Parameters.Add("@RoleID", ObjEUserInfo.RoleID);
                    cmd.Parameters.Add("@dtFeatureMap", dt);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsFeature);
                    }
                    if (dsFeature != null && dsFeature.Tables.Count > 0)
                    {
                        ObjEUserInfo.dtFeature = dsFeature.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                {
                    throw new Exception("Fehler bei der Zuordnung einer Berechtigung zu einer Rolle");
                }
                else
                {
                    throw new Exception("Error Occured While Mapping Features To Role");

                }
            }
            finally
            {
                SQLCon.Close();
            }
            return ObjEUserInfo;
        }

        /// <summary>
        /// Code to validate user details while logging in
        /// </summary>
        /// <param name="ObjEUserInfo"></param>
        /// <returns></returns>
        public EUserInfo CheckUserCredentials(EUserInfo ObjEUserInfo)
        {
            DataSet dsFeature = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn2();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_CheckUserCredentials]";
                    cmd.Parameters.Add("@UserName", ObjEUserInfo.UserName);
                    cmd.Parameters.Add("@Password", ObjEUserInfo.Password);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsFeature);
                    }
                    if (dsFeature != null && dsFeature.Tables.Count > 0)
                    {
                        string strUserID = dsFeature.Tables[0].Rows[0][0].ToString();
                        int UserID = 0;
                        if (int.TryParse(strUserID, out UserID))
                        {
                            ObjEUserInfo.dtUserDetails = dsFeature.Tables[0];
                            if (dsFeature.Tables.Count > 1)
                            {
                                ObjEUserInfo.dtFeature = dsFeature.Tables[1];
                                if (dsFeature.Tables.Count > 2)
                                {
                                    ObjEUserInfo.dtLVStatus = dsFeature.Tables[2];
                                    if (dsFeature.Tables.Count > 3)
                                    {
                                        ObjEUserInfo.dtPositionKZ = dsFeature.Tables[3];
                                    }
                                }
                            }
                        }
                        else
                        {
                            throw new Exception(strUserID);
                        }
                    }
                }
            }
            catch (Exception ex)
                 {
                throw new Exception("Der Login konnte nicht erfolgen");
            }
            finally { SQLCon.Close2(); }
            return ObjEUserInfo;
        }

        /// <summary>
        /// Code to Change the user password from Profile
        /// </summary>
        /// <param name="ObjEUserInfo"></param>
        /// <returns></returns>
        public EUserInfo ResetPassword(EUserInfo ObjEUserInfo)
        {
            DataSet dsFeature = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Upd_Password]";
                    cmd.Parameters.AddWithValue("@UserID", ObjEUserInfo.UserID);
                    cmd.Parameters.AddWithValue("@OldPassword", ObjEUserInfo.OldPassword);
                    cmd.Parameters.AddWithValue("@NewPassword", ObjEUserInfo.NewPassword);
                    cmd.Parameters.AddWithValue("@IsAdmin", ObjEUserInfo.IsAdmin);
                    object Objreturn = cmd.ExecuteScalar();
                    string str = Convert.ToString(Objreturn);
                    if (!string.IsNullOrEmpty(str))
                        throw new Exception(str);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Valid"))
                {
                    if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                    {
                        throw new Exception("Bitte geben Sie das gültige alte Passwort ein");
                    }
                    else
                    {
                        throw new Exception("Please Enter Valid Old Password");
                    }
                }                    
                else
                    throw new Exception("Fehler beim Zurücksetzen des Passworts");
            }
            finally
            {
                SQLCon.Close();
            }
            return ObjEUserInfo;
        }

        /// <summary>
        /// Code to save Auto save checkbox status in database
        /// </summary>
        /// <param name="ObjEUserInfo"></param>
        /// <returns></returns>
        public EUserInfo UpdateAutoSave(EUserInfo ObjEUserInfo)
        {
            DataSet dsFeature = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Upd_AutoSave]";
                    cmd.Parameters.AddWithValue("@UserID", ObjEUserInfo.UserID);
                    cmd.Parameters.AddWithValue("@AutoSave", ObjEUserInfo.AutoSaveMode);
                    object Objreturn = cmd.ExecuteScalar();
                    string str = Convert.ToString(Objreturn);
                    if (!string.IsNullOrEmpty(str))
                        throw new Exception(str);
                }
            }
            catch (Exception ex)
            {
                    throw new Exception("Error While updating AutoSave Mode");
            }
            return ObjEUserInfo;
        }
    }
}
