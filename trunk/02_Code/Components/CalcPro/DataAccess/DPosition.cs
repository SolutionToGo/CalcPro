using DAL;
using EL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace DataAccess
{
    public class DPosition
    {
        /// <summary>
        ///  Code to Add or edit Positions
        /// </summary>
        /// <param name="ObjEPosition"></param>
        /// <param name="strRaster"></param>
        /// <param name="LVSprunge"></param>
        /// <param name="_IsCopy"></param>
        /// <returns></returns> 
        public EPosition SavePositionDetails(XmlDocument XmlDoc,int iProjectID, string LongDescription, double OZID,
            string OZ1, string OZ2, string OZ3, string OZ4, string OZ5, string OZ6, string stOZChar,bool MontageEntry,
            string stLVRaster, int Roundoffvalue, EPosition ObjEPosition)
        {
            int ProjectID = -1;
            DataTable dtPosition = new DataTable();
            try
            {
                string innerxml = XmlDoc.InnerXml;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Ins_Position]";
                    SqlParameter param = new SqlParameter("@XMLPositions", SqlDbType.Xml);
                    param.Value = innerxml;
                    cmd.Parameters.Add(param);
                    cmd.Parameters.AddWithValue("@ProjectID", iProjectID);
                    cmd.Parameters.AddWithValue("@LongDescription", LongDescription);
                    cmd.Parameters.AddWithValue("@OZID", OZID);
                    cmd.Parameters.AddWithValue("@OZChar", stOZChar);
                    cmd.Parameters.AddWithValue("@O1", OZ1);
                    cmd.Parameters.AddWithValue("@O2", OZ2);
                    cmd.Parameters.AddWithValue("@O3", OZ3);
                    cmd.Parameters.AddWithValue("@O4", OZ4);
                    cmd.Parameters.AddWithValue("@O5", OZ5);
                    cmd.Parameters.AddWithValue("@O6", OZ6);
                    cmd.Parameters.AddWithValue("@MontageEntry", MontageEntry);
                    cmd.Parameters.AddWithValue("@LVRaster", stLVRaster);
                    cmd.Parameters.AddWithValue("@RoundOff", Roundoffvalue);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtPosition);
                    }
                    if(dtPosition != null && dtPosition.Rows.Count > 0)
                    {
                        string str = Convert.ToString(dtPosition.Rows[0][0]);
                        if (!string.IsNullOrEmpty(str))
                        {
                            if (int.TryParse(str, out ProjectID))
                            {
                                ObjEPosition.PositionID = ProjectID;
                                ObjEPosition.drnewrow = dtPosition.Rows[0];
                            }
                            else if (str.Contains("UNIQUE"))
                            {
                                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                                    throw new Exception("Diese Ordnungskennzahl existiert bereits");
                                else
                                    throw new Exception("OZ Already Exists");
                            }
                            else if (str.Contains("Title"))
                            {
                                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                                    throw new Exception("Dieser Titel / Untertitel existiert nicht");
                                else
                                    throw new Exception(str);
                            }
                            else if (str.ToString().Contains("Surcharge"))
                            {
                                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                                    throw new Exception("Eine Zuschlagsposition kann nicht mehrfach für die gleichen Positionen angelegt werden");
                                else
                                    throw new Exception(str);
                            }
                            else if (str.Contains("Sum"))
                            {
                                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                                    throw new Exception("Eine Summe kann nicht mehrfach für die gleichen Positionen angelegt werden");
                                else
                                    throw new Exception(str);
                            }
                            else if (str.Contains("Base"))
                            {
                                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                                    throw new Exception("Es existiert keine Basis-Position für diese Detail KZ Position");
                                else
                                    throw new Exception(str);
                            }
                            else
                                throw new Exception(str);
                        }
                    }
                }
            }
            catch (Exception ex){throw;}
            finally{SQLCon.Close();}
            return ObjEPosition;
        }

        /// <summary>
        /// Code to fetch Position list from database
        /// </summary>
        /// <param name="ObjEPosition"></param>
        /// <param name="ProjectID"></param>
        public DataSet GetPsoitionList(int ProjectID)
        {
            DataSet dsPositionsList = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_PositionList]";
                    cmd.Parameters.AddWithValue("@ProjectID", ProjectID);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsPositionsList, "Positions");
                    }                   
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                {
                    throw new Exception("Fehler beim Laden der Positionsliste");
                }
                else
                {
                    throw new Exception("Error Occured While Retreiving Position List");

                }
            }
            finally
            {
                SQLCon.Close();
            }
            return dsPositionsList;
        }

        /// <summary>
        /// Code to fetch lang description of positons
        /// </summary>
        /// <param name="PositionID"></param>
        /// <returns></returns>
        public string GetLongDescription(int PositionID)
        {
            string LongDescription = string.Empty;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_LongDescription]";
                    cmd.Parameters.AddWithValue("@PositionID", PositionID);
                    Object ObjReturn = cmd.ExecuteScalar();
                    if (ObjReturn != null && ObjReturn != DBNull.Value)
                    {
                        LongDescription = ObjReturn.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                {
                    throw new Exception("Fehler beim Laden der Positionsliste");
                }
                else
                {
                    throw new Exception("Error Occured While Retreiving Lang Text");
                }
            }
            finally
            {
                SQLCon.Close();
            }
            return LongDescription;
        }

        /// <summary>
        /// Code to fetch next availbale LV section availble under selected project
        /// </summary>
        /// <param name="strLVSection"></param>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public string GetLVSection(string strLVSection, int ProjectID)
        {
            string strNewLVSection = string.Empty;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_LVSection]";
                    cmd.Parameters.AddWithValue("@LVSectionName", strLVSection);
                    cmd.Parameters.AddWithValue("@ProjectID", ProjectID);
                    object returnObj = cmd.ExecuteScalar();
                    if (returnObj != null)
                    {
                        strNewLVSection = Convert.ToString(returnObj);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return strNewLVSection;
        }

        /// <summary>
        /// Code to insert new LV section into database after user apporval
        /// </summary>
        /// <param name="strNewLVSection"></param>
        /// <param name="ProjectID"></param>
        /// <param name="ObjEProject"></param>
        public void InsertLVSection(string strNewLVSecction, int ProjectID,EProject ObjEProject)
        {
            try
            {
                DataSet dsLVSection = new DataSet();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Ins_LVSection]";
                    cmd.Parameters.AddWithValue("@LVSection", strNewLVSecction);
                    cmd.Parameters.AddWithValue("@ProjectID", ProjectID);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsLVSection);
                    }
                    if(dsLVSection != null && dsLVSection.Tables.Count > 0 && dsLVSection.Tables[0].Rows.Count > 0)
                    {
                        ObjEProject.LVSectionID = dsLVSection.Tables[0].Rows[0][0];
                        if(dsLVSection.Tables.Count > 1)
                            ObjEProject.dtLVSection = dsLVSection.Tables[1];
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Code to fetch Positions from database for bulk proccess module by filtering Titles, Subtitles and Positions
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="tType"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataSet GetPsoitionOZList(int ProjectID,string tType,DataTable dt)
        {
            DataSet dsPositionsOZList = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_PositionOZ]";
                    cmd.Parameters.AddWithValue("@ProjectID", ProjectID);
                    cmd.Parameters.AddWithValue("@Position_Type", tType);
                    cmd.Parameters.AddWithValue("@WG", "");
                    cmd.Parameters.AddWithValue("@WA", "");
                    cmd.Parameters.AddWithValue("@Position_OZ_Table", dt);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsPositionsOZList, "Positions");
                    }
                    if(dsPositionsOZList !=null && dsPositionsOZList.Tables.Count > 0 && dsPositionsOZList.Tables[0].Rows.Count > 0)
                    {
                        string strError = dsPositionsOZList.Tables[0].Rows[0][0].ToString();
                        if(strError.Contains("EXISTS"))
                        {
                            throw new Exception(strError);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if(ex.Message.Contains("EXISTS"))
                {
                    if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                    {
                        throw new Exception("Diese LV Position existiert nicht für das ausgewählte Projekt.");
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                    {
                        throw new Exception("Fehler beim Laden der Positionsliste");
                    }
                    else
                    {
                        throw new Exception("Error Occured While Retreiving Position OZ List");
                    }
                }
            }
            finally
            {
                SQLCon.Close();
            }
            return dsPositionsOZList;
        }

        /// <summary>
        /// Code to fetch Positions from database for bulk proccess module by filtering with article
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="tType"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataSet GetPsoitionOZByWGWA(int ProjectID, string tType, string WG, string WA)
        {
            DataSet dsPositionsOZList = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_PositionOZ]";
                    cmd.Parameters.AddWithValue("@ProjectID", ProjectID);
                    cmd.Parameters.AddWithValue("@Position_Type", tType);
                    cmd.Parameters.AddWithValue("@WG", WG);
                    cmd.Parameters.AddWithValue("@WA", WA);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsPositionsOZList, "Positions");
                    }
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                {
                    throw new Exception("Fehler beim Laden der Positionsliste");
                }
                else
                {
                    throw new Exception("Error Occured While Retreiving Position OZ List");

                }
            }
            finally
            {
                SQLCon.Close();
            }
            return dsPositionsOZList;
        }

        /// <summary>
        /// Code to update values to position from Bulk proccess Section A
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="tType"></param>
        /// <param name="MA_Selbstkosten"></param>
        /// <param name="MO_Selbstkosten"></param>
        /// <param name="MA_Verkaufspreis"></param>
        /// <param name="MO_Verkaufspreis"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataSet UpdateBulkProcess_ActionA(int ProjectID, string tType, decimal MA_Selbstkosten, decimal MO_Selbstkosten, decimal MA_Verkaufspreis, decimal MO_Verkaufspreis, DataTable dt)
        {
            DataSet dsPositionsOZList = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Upd_BulkProcess_ActionA]";
                    cmd.Parameters.AddWithValue("@ProjectID", ProjectID);
                    cmd.Parameters.AddWithValue("@Position_Type", tType);
                    cmd.Parameters.AddWithValue("@MA_selbstkostenMulti", MA_Selbstkosten);
                    cmd.Parameters.AddWithValue("@MO_selbstkostenMulti", MO_Selbstkosten);
                    cmd.Parameters.AddWithValue("@MA_verkaufspreis_Multi", MA_Verkaufspreis);
                    cmd.Parameters.AddWithValue("@MO_verkaufspreisMulti", MO_Verkaufspreis);
                    cmd.Parameters.AddWithValue("@Bulk_Process_ActionA_Table", dt);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsPositionsOZList, "Positions");
                    }
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                {
                    throw new Exception("Fehler beim Laden der Positionsliste");
                }
                else
                {
                    throw new Exception("Error Occured While Retreiving Position OZ List");

                }
            }
            finally
            {
                SQLCon.Close();
            }
            return dsPositionsOZList;
        }

        /// <summary>
        /// Code to update values to position from Bulk proccess Section B
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="tType"></param>
        /// <param name="MA_Selbstkosten"></param>
        /// <param name="MO_Selbstkosten"></param>
        /// <param name="MA_Verkaufspreis"></param>
        /// <param name="MO_Verkaufspreis"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataSet UpdateBulkProcess_ActionB(int ProjectID, string tType, decimal Menge, string MA, string MO, string PeriesText, string Fabricat, string Typ, string LieferantMA, string wg, string wa, string wi, string tLVSection, DataTable dt)
        {
            DataSet dsPositionsOZList = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Upd_BulkProcess_ActionB]";
                    cmd.Parameters.AddWithValue("@ProjectID", ProjectID);
                    cmd.Parameters.AddWithValue("@Position_Type", tType);
                    cmd.Parameters.AddWithValue("@Menge",Menge);
                    cmd.Parameters.AddWithValue("@MA", MA);
                    cmd.Parameters.AddWithValue("@MO", MO);
                    cmd.Parameters.AddWithValue("@PreisText", PeriesText);
                    cmd.Parameters.AddWithValue("@Fabricate", Fabricat);
                    cmd.Parameters.AddWithValue("@Type", Typ);
                    cmd.Parameters.AddWithValue("@LiefrantMA", LieferantMA);
                    cmd.Parameters.AddWithValue("@WG",wg);
                    cmd.Parameters.AddWithValue("@WA",wa);
                    cmd.Parameters.AddWithValue("@WI",wi);
                    cmd.Parameters.AddWithValue("@LVSection",tLVSection);
                    cmd.Parameters.AddWithValue("@Bulk_Process_ActionB_Table", dt);
                    object ObjReturn = cmd.ExecuteScalar();
                    string str = Convert.ToString(ObjReturn);
                    if (!string.IsNullOrEmpty(str))
                    {
                        int IValue = 0;
                        if (!int.TryParse(str, out IValue))
                            throw new Exception(str);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Typ") || ex.Message.Contains("Artikel") || ex.Message.Contains("LV Section"))
                    throw;
                else
                {
                    if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                        throw new Exception("Fehler beim Laden der Positionsliste");
                    else
                        throw new Exception("Error While updating the Action B");
                }
            }
            finally
            {
                SQLCon.Close();
            }
            return dsPositionsOZList;
        }

        /// <summary>
        /// Code to update long description of position
        /// </summary>
        /// <param name="PositionID"></param>
        /// <param name="strLongDescription"></param>
        public void UpdateLongDescription(int PositionID,string strLongDescription)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Upd_LongDescription]";
                    cmd.Parameters.AddWithValue("@PositionID", PositionID);
                    cmd.Parameters.AddWithValue("@LongDescription", strLongDescription);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Code to delete a Position from project
        /// </summary>
        /// <param name="PositionID"></param>
        /// <param name="_PosKZ"></param>
        public void DeletePosition(int PositionID,string _PosKZ)
        {
            string strError = string.Empty;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Del_Position]";
                    cmd.Parameters.AddWithValue("@PositionID", PositionID);
                    cmd.Parameters.AddWithValue("@PositionKZ", _PosKZ);
                    object ObjReturn = cmd.ExecuteScalar();
                    if(!string.IsNullOrEmpty(Convert.ToString(ObjReturn)))
                    {
                        throw new Exception(Convert.ToString(ObjReturn));
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Positions"))
                {
                    if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                        throw new Exception("Die ausgewählte Position enthält eine Detail KZ Position");
                    else
                        throw new Exception("Selected Position Is Having DetailKZ Positions");
                }
                else
                {
                    if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                        throw new Exception("Fehler beim Löschen der Positionen");
                    else
                        throw new Exception("Error while Deleting Position");
                }
            }
        }

        /// <summary>
        /// Code to fetch article details and dimensions list from database by  using typ
        /// </summary>
        /// <param name="ObjEPositon"></param>
        /// <returns></returns>
        public EPosition GetArticleByTyp(EPosition ObjEPositon)
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_ArticleByType]";
                    cmd.Parameters.AddWithValue("@Typ", ObjEPositon.Type); 
                    cmd.Parameters.AddWithValue("@dtSubmitDate", ObjEPositon.ValidityDate);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
                    }
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        ObjEPositon.dtArticle = ds.Tables[0];
                        ObjEPositon.WG = ds.Tables[0].Rows[0]["WG"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["WG"].ToString();
                        ObjEPositon.WA = ds.Tables[0].Rows[0]["WA"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["WA"].ToString();
                        ObjEPositon.WI = ds.Tables[0].Rows[0]["WI"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["WI"].ToString();
                        ObjEPositon.Fabricate = ds.Tables[0].Rows[0]["Fabrikate"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["Fabrikate"].ToString();
                        ObjEPositon.ME = ds.Tables[0].Rows[0]["Menegenheit"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["Menegenheit"].ToString();
                        ObjEPositon.Dim = ds.Tables[0].Rows[0]["Masseinheit"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["Masseinheit"].ToString();
                        ObjEPositon.Faktor = ds.Tables[0].Rows[0]["Factor"] == DBNull.Value ? 1 : Convert.ToDecimal(ds.Tables[0].Rows[0]["Factor"]);
                        ObjEPositon.LiefrantMA = ds.Tables[0].Rows[0]["FullName"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["FullName"].ToString();
                        ObjEPositon.Multi1MA = ds.Tables[0].Rows[0]["Multi1"] == DBNull.Value ? 1 : Convert.ToDecimal(ds.Tables[0].Rows[0]["Multi1"]);
                        ObjEPositon.Multi2MA = ds.Tables[0].Rows[0]["Multi2"] == DBNull.Value ? 1 : Convert.ToDecimal(ds.Tables[0].Rows[0]["Multi2"]);
                        ObjEPositon.Multi3MA = ds.Tables[0].Rows[0]["Multi3"] == DBNull.Value ? 1 : Convert.ToDecimal(ds.Tables[0].Rows[0]["Multi3"]);
                        ObjEPositon.Multi4MA = ds.Tables[0].Rows[0]["Multi4"] == DBNull.Value ? 1 : Convert.ToDecimal(ds.Tables[0].Rows[0]["Multi4"]);
                        if (ds.Tables.Count > 1)
                            ObjEPositon.dtDimensions = ds.Tables[1];
                    }
                }
            }
            catch (Exception ex){throw;}
            return ObjEPositon;
        }

        /// <summary>
        /// Code to fetch article details and dimensions list from database by  using Article Numbers
        /// </summary>
        /// <param name="ObjEPositon"></param>
        /// <returns></returns>
        public EPosition GetArticleByWI(EPosition ObjEPositon)
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_ArticleByWG]";
                    cmd.Parameters.AddWithValue("@WG", ObjEPositon.WG);
                    cmd.Parameters.AddWithValue("@WA", ObjEPositon.WA);
                    cmd.Parameters.AddWithValue("@WI", ObjEPositon.WI);
                    cmd.Parameters.AddWithValue("@dtSubmitDate", ObjEPositon.ValidityDate);
                    cmd.Parameters.AddWithValue("@ProjectID", ObjEPositon.ProjectID);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
                    }
                    if (ds != null &&  ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        ObjEPositon.dtArticle = ds.Tables[0];
                        ObjEPositon.Type = ds.Tables[0].Rows[0]["Typ"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["Typ"].ToString();
                        ObjEPositon.Fabricate = ds.Tables[0].Rows[0]["Fabrikate"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["Fabrikate"].ToString();
                        ObjEPositon.ME = ds.Tables[0].Rows[0]["Menegenheit"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["Menegenheit"].ToString();
                        ObjEPositon.Dim = ds.Tables[0].Rows[0]["Masseinheit"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["Masseinheit"].ToString();
                        ObjEPositon.LiefrantMA = ds.Tables[0].Rows[0]["FullName"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["FullName"].ToString();
                        ObjEPositon.Multi1MA = ds.Tables[0].Rows[0]["Multi1"] == DBNull.Value ? 1 : Convert.ToDecimal(ds.Tables[0].Rows[0]["Multi1"]);
                        ObjEPositon.Multi2MA = ds.Tables[0].Rows[0]["Multi2"] == DBNull.Value ? 1 : Convert.ToDecimal(ds.Tables[0].Rows[0]["Multi2"]);
                        ObjEPositon.Multi3MA = ds.Tables[0].Rows[0]["Multi3"] == DBNull.Value ? 1 : Convert.ToDecimal(ds.Tables[0].Rows[0]["Multi3"]);
                        ObjEPositon.Multi4MA = ds.Tables[0].Rows[0]["Multi4"] == DBNull.Value ? 1 : Convert.ToDecimal(ds.Tables[0].Rows[0]["Multi4"]);
                        ObjEPositon.Faktor = ds.Tables[0].Rows[0]["Factor"] == DBNull.Value ? 1 : Convert.ToDecimal(ds.Tables[0].Rows[0]["Factor"]);

                        if (ds.Tables.Count > 1)
                            ObjEPositon.dtDimensions = ds.Tables[1];
                    }
                    else
                    {
                        ObjEPositon.dtArticle.Rows.Clear();
                        ObjEPositon.dtDimensions.Rows.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEPositon;
        }

        /// <summary>
        /// Code to fetch article details and dimensions list from database by  using WG WA combination
        /// </summary>
        /// <param name="ObjEPositon"></param>
        /// <returns></returns>
        public EPosition GetArticleByWGWA(EPosition ObjEPositon)
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_WGWAComb]";
                    cmd.Parameters.AddWithValue("@WG", ObjEPositon.WG);
                    cmd.Parameters.AddWithValue("@WA", ObjEPositon.WA);
                    cmd.Parameters.AddWithValue("@ProjectID", ObjEPositon.ProjectID);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
                    }
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        ObjEPositon.dtArticle = ds.Tables[0];
                    else
                        ObjEPositon.dtArticle.Rows.Clear();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEPositon;
        }

        /// <summary>
        /// Code to fetch article Prices from database by  using Article number and Dimenstions
        /// </summary>
        /// <param name="ObjEPositon"></param>
        /// <returns></returns>
        public EPosition GetArticleByDimension(EPosition ObjEPositon)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_ArticleByDimension]";
                    cmd.Parameters.AddWithValue("@WG", ObjEPositon.WG);
                    cmd.Parameters.AddWithValue("@WA", ObjEPositon.WA);
                    cmd.Parameters.AddWithValue("@WI", ObjEPositon.WI);
                    cmd.Parameters.AddWithValue("@A", ObjEPositon.Dim1);
                    cmd.Parameters.AddWithValue("@B", ObjEPositon.Dim2);
                    cmd.Parameters.AddWithValue("@L", ObjEPositon.Dim3);
                    cmd.Parameters.AddWithValue("@dtSubmitDate", ObjEPositon.ValidityDate);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        ObjEPositon.dtDimensions = dt.Copy();
                        ObjEPositon.LPMA = dt.Rows[0]["ListPrice"] == DBNull.Value ? 1 : Convert.ToDecimal(dt.Rows[0]["ListPrice"]);
                        ObjEPositon.Mins = dt.Rows[0]["Minuten"] == DBNull.Value ? 1 : Convert.ToDecimal(dt.Rows[0]["Minuten"]);
                        ObjEPositon.Faktor = dt.Rows[0]["Factor"] == DBNull.Value ? 1 : Convert.ToDecimal(dt.Rows[0]["Factor"]);
                        DateTime dtdate = DateTime.Now;
                        if (DateTime.TryParse(Convert.ToString(dt.Rows[0]["ValidityDate"]), out dtdate))
                            ObjEPositon.ValidityDate = dtdate;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEPositon;
        }

        /// <summary>
        /// Code to fetch position list from database for Copy LVs module
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public DataSet GetOldPositionList(int ProjectID)
        {
            DataSet dsPositionsList = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_PositionList_Copy]";
                    cmd.Parameters.AddWithValue("@ProjectID", ProjectID);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsPositionsList, "Positions");
                    }
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                    throw new Exception("Fehler beim Laden der Positionsliste");
                else
                    throw new Exception("Error Occured While Retreiving Position List");
            }
            finally
            {
                SQLCon.Close();
            }
            return dsPositionsList;
        }

        /// <summary>
        /// Code to Copy a Position from Copy LVs modules
        /// </summary>
        /// <param name="ObjEPosition"></param>
        /// <param name="stOZChar"></param>
        /// <returns></returns>
        public int CopyPosition(EPosition ObjEPosition,string stOZChar)
        {
            int ProjectID = -1;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                     cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Ins_CopyPosition]";
                    cmd.Parameters.AddWithValue("@ProjectID", ObjEPosition.ProjectID);
                    cmd.Parameters.AddWithValue("@ParentID", ObjEPosition.ParentID);
                    cmd.Parameters.AddWithValue("@PositionOZ", ObjEPosition.Position_OZ);
                    cmd.Parameters.AddWithValue("@SNO", ObjEPosition.SNO);
                    cmd.Parameters.AddWithValue("@dtCopyPosition", ObjEPosition.dtCopyPosition);
                    cmd.Parameters.AddWithValue("@OZID", ObjEPosition.OZID);
                    cmd.Parameters.AddWithValue("@OZChar", stOZChar);
                    object returnObj = cmd.ExecuteScalar();
                    if (!int.TryParse(Convert.ToString(returnObj), out ProjectID))
                        throw new Exception(Convert.ToString(returnObj));
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("UNIQUE"))
                {
                    if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                        throw new Exception("Diese Ordnungskennzahl existiert bereits");
                    else
                        throw new Exception("OZ Already Exists");
                }
                else
                {
                    if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                        throw new Exception("Fehler beim Verschieben der Position");
                    else
                        throw new Exception("Error While Moving the position");
                }
            }
            finally
            {
                SQLCon.Close();
            }
            return ProjectID;
        }

        /// <summary>
        /// Code to fetch corresponding dimensions by Dimension A
        /// </summary>
        /// <param name="ObjEPositon"></param>
        /// <param name="_DimType"></param>
        /// <returns></returns>
        public EPosition GetArticleByA(EPosition ObjEPositon,string _DimType)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_ArticleByA]";
                    cmd.Parameters.AddWithValue("@WG", ObjEPositon.WG);
                    cmd.Parameters.AddWithValue("@WA", ObjEPositon.WA);
                    cmd.Parameters.AddWithValue("@WI", ObjEPositon.WI);                    
                    cmd.Parameters.AddWithValue("@dtSubmitDate", ObjEPositon.ValidityDate);
                    cmd.Parameters.AddWithValue("@A", ObjEPositon.Dim1);
                    cmd.Parameters.AddWithValue("@DimType", _DimType);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string value = string.Empty;
                        ObjEPositon.Dim1 = dt.Rows[0]["A"] == DBNull.Value ? "" : dt.Rows[0]["A"].ToString();
                        ObjEPositon.Dim2 = dt.Rows[0]["B"] == DBNull.Value ? "" : dt.Rows[0]["B"].ToString();
                        ObjEPositon.Dim3 = dt.Rows[0]["L"] == DBNull.Value ? "" : dt.Rows[0]["L"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEPositon;
        }

        /// <summary>
        /// Code to fetch corresponding dimensions by Dimension B
        /// </summary>
        /// <param name="ObjEPositon"></param>
        /// <param name="_DimType"></param>
        /// <returns></returns>
        public EPosition GetArticleByB(EPosition ObjEPositon, string _DimType)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_ArticleByA]";
                    cmd.Parameters.AddWithValue("@WG", ObjEPositon.WG);
                    cmd.Parameters.AddWithValue("@WA", ObjEPositon.WA);
                    cmd.Parameters.AddWithValue("@WI", ObjEPositon.WI);
                    cmd.Parameters.AddWithValue("@dtSubmitDate", ObjEPositon.ValidityDate);
                    cmd.Parameters.AddWithValue("@A", ObjEPositon.Dim1);
                    cmd.Parameters.AddWithValue("@B", ObjEPositon.Dim2);
                    cmd.Parameters.AddWithValue("@DimType", _DimType);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        ObjEPositon.Dim1 = dt.Rows[0]["A"] == DBNull.Value ? "" : dt.Rows[0]["A"].ToString();
                        ObjEPositon.Dim2 = dt.Rows[0]["B"] == DBNull.Value ? "" : dt.Rows[0]["B"].ToString();
                        ObjEPositon.Dim3 = dt.Rows[0]["L"] == DBNull.Value ? "" : dt.Rows[0]["L"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEPositon;
        }

        /// <summary>
        /// Code to get Project specific Articles
        /// </summary>
        /// <param name="ObjEPositon"></param>
        /// <returns></returns>
        public EPosition GetProjectArticles(EPosition ObjEPositon)
        {
            try
            {
                ObjEPositon.dtProjectArticles = new DataTable();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_ProjectArticles]";
                    cmd.Parameters.AddWithValue("@ProjectID", ObjEPositon.ProjectID);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ObjEPositon.dtProjectArticles);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEPositon;
        }

        /// <summary>
        /// Code to Save Project Specific Articles
        /// </summary>
        /// <param name="ObjEPosition"></param>
        /// <returns></returns>
        public EPosition SaveProjectArticle(EPosition ObjEPosition)
        {
            try
            {
                int WGID = 0;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Ins_ProjectArticle]";
                    cmd.Parameters.AddWithValue("@ProjectID", ObjEPosition.ProjectID);
                    cmd.Parameters.AddWithValue("@WG", ObjEPosition.WG);
                    cmd.Parameters.AddWithValue("@WA", ObjEPosition.WA);
                    cmd.Parameters.AddWithValue("@WGDescription", ObjEPosition.WGDescription);
                    cmd.Parameters.AddWithValue("@WADescription", ObjEPosition.WADescription);
                    cmd.Parameters.AddWithValue("@UserID", ObjEPosition.UserID);
                    object returnObj = cmd.ExecuteScalar();
                    if (!int.TryParse(Convert.ToString(returnObj), out WGID))
                        throw new Exception(Convert.ToString(returnObj));
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("UNIQUE"))
                {
                    if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                        throw new Exception("Article already exists");
                    else
                        throw new Exception("Article already exists");
                }
                else
                {
                    if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                        throw new Exception("Error While saving article");
                    else
                        throw new Exception("Error While saving article");
                }
            }
            finally
            {
                SQLCon.Close();
            }
            return ObjEPosition;
        }

        /// <summary>
        /// Code to save Position Specific Articles
        /// </summary>
        /// <param name="ObjEPosition"></param>
        /// <returns></returns>
        public EPosition SavePositionArticle(EPosition ObjEPosition)
        {
            try
            {
                int WGID = 0;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Ins_PositionArticle]";
                    cmd.Parameters.AddWithValue("@ProjectID", ObjEPosition.ProjectID);
                    cmd.Parameters.AddWithValue("@WG", ObjEPosition.WG);
                    cmd.Parameters.AddWithValue("@WA", ObjEPosition.WA);
                    cmd.Parameters.AddWithValue("@WI", ObjEPosition.WI);
                    cmd.Parameters.AddWithValue("@UserID", ObjEPosition.UserID);
                    object returnObj = cmd.ExecuteScalar();
                    if (!int.TryParse(Convert.ToString(returnObj), out WGID))
                        throw new Exception(Convert.ToString(returnObj));
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("UNIQUE"))
                {
                    if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                        throw new Exception("Article already exists");
                    else
                        throw new Exception("Article already exists");
                }
                else
                {
                    if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                        throw new Exception("Error While saving article");
                    else
                        throw new Exception("Error While saving article");
                }
            }
            finally
            {
                SQLCon.Close();
            }
            return ObjEPosition;
        }

    }
}
    