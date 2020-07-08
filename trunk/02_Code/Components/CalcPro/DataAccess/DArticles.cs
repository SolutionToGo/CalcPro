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
   public class DArticles
    {
        /// <summary>
        /// Code to add or edit an article from article master
        /// </summary>
        /// <param name="ObjEArticle"></param>
        /// <returns></returns>
        public EArticles SaveArticle(XmlDocument xml, EArticles ObjEArticle)
            {
                DataSet dsArticles = new DataSet();
                try
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        string innerxml = xml.InnerXml;
                        cmd.Connection = SQLCon.Sqlconn();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "[P_Ins_Article]";
                        SqlParameter param = new SqlParameter("@XMLArticle", SqlDbType.Xml);
                        param.Value = innerxml;
                        cmd.Parameters.Add(param);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dsArticles);
                        }
                        if(dsArticles != null && dsArticles.Tables.Count > 0)
                        {
                            int iValue = 0;
                            int iValue1 = 0;
                            string str = dsArticles.Tables[0].Rows[0][0] == DBNull.Value ? "" : dsArticles.Tables[0].Rows[0][0].ToString();
                            if(int.TryParse(str,out iValue))
                            {
                                ObjEArticle.WGID = iValue;
                                string str1 = dsArticles.Tables[0].Rows[0][1] == DBNull.Value ? "" : dsArticles.Tables[0].Rows[0][1].ToString();
                                if (int.TryParse(str1, out iValue1))
                                {
                                    ObjEArticle.WIID = iValue1;
                                    ObjEArticle.dtWG = dsArticles.Tables[1];
                                    ObjEArticle.dtWI = dsArticles.Tables[2];
                                }
                            }
                            else if (str.Contains("UNIQUE"))
                            {
                                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                                {
                                    throw new Exception("Der Artikel existiert bereits.");
                                }
                                else
                                {
                                    throw new Exception("Article Already Exists");
                                }
                            }
                             else
                                throw new Exception("Fehler beim Speichern des Artikels");
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
                return ObjEArticle;
            }

       /// <summary>
       /// Code to getch article data from base i.e. Article data, dimensiions, typ and Rabatt mappings
       /// </summary>
       /// <param name="ObjEArticle"></param>
       /// <returns></returns>
       public EArticles GetArticle(EArticles ObjEArticle)
       {
           DataSet dsArticles = new DataSet();
           try
           {
               using (SqlCommand cmd = new SqlCommand())
               {
                   cmd.Connection = SQLCon.Sqlconn();
                   cmd.CommandType = CommandType.StoredProcedure;
                   cmd.CommandText = "[P_Get_article]";
                   using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                   {
                       da.Fill(dsArticles);
                   }
                   ObjEArticle.dtWG = dsArticles.Tables[0];
                   ObjEArticle.dtWI = dsArticles.Tables[1];
                   ObjEArticle.dtDimenstions = dsArticles.Tables[2];
               }
           }
           catch (Exception ex)
           {
               if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                   throw new Exception("Fehler bei der Datenaktualisierung für Artikel");
               else
                   throw new Exception("Error While Retrieving the Articles");
           }
           finally
           {
               SQLCon.Close();
           }
           return ObjEArticle;
       }

        /// <summary>
        /// Code to Add dimensions of article
        /// </summary>
        /// <param name="ObjEArticle"></param>
        /// <returns></returns>
        public EArticles SaveDimension(XmlDocument xml, EArticles ObjEArticle)
       {
           DataSet dsDimensions = new DataSet();
           try
           {
               using (SqlCommand cmd = new SqlCommand())
               {
                   string innerxml = xml.InnerXml;
                   cmd.Connection = SQLCon.Sqlconn();
                   cmd.CommandType = CommandType.StoredProcedure;
                   cmd.CommandText = "[P_Ins_Dimension]";
                   SqlParameter param = new SqlParameter("@XMlDimension", SqlDbType.Xml);
                   param.Value = innerxml;
                   cmd.Parameters.Add(param);
                   using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                   {
                       da.Fill(dsDimensions);
                   }
                   if (dsDimensions != null && dsDimensions.Tables.Count > 0)
                   {
                       int iValue = 0;
                       string str = dsDimensions.Tables[0].Rows[0][0] == DBNull.Value ? "" : dsDimensions.Tables[0].Rows[0][0].ToString();
                       if (int.TryParse(str, out iValue))
                       {
                           ObjEArticle.DimensionID = iValue;
                           ObjEArticle.dtDimenstions = dsDimensions.Tables[1];
                       }
                       else if(str.Contains("UNIQUE"))
                       {
                           if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                           {
                               throw new Exception("Diese Maße gibt es bereits");
                           }
                           else
                           {
                               throw new Exception("Dimension Already Exists");
                           }
                       }
                       
                           else
                           throw new Exception("Fehler beim Speichern der Maße");
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
           return ObjEArticle;
       }

    /// <summary>
    /// Code to add ot edit typ from Typ master
    /// </summary>
    /// <param name="xml"></param>
    /// <param name="ObjEArticle"></param>
    /// <returns></returns>
       public EArticles SaveTyp(XmlDocument xml, EArticles ObjEArticle)
       {
           DataSet dsType = new DataSet();
           try
           {
               using (SqlCommand cmd = new SqlCommand())
               {
                   string innerxml = xml.InnerXml;
                   cmd.Connection = SQLCon.Sqlconn();
                   cmd.CommandType = CommandType.StoredProcedure;
                   cmd.CommandText = "[P_Ins_Typ]";
                   SqlParameter param = new SqlParameter("@XMLTyp", SqlDbType.Xml);
                   param.Value = innerxml;
                   cmd.Parameters.Add(param);
                   using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                   {
                       da.Fill(dsType);
                   }
                   if (dsType != null && dsType.Tables.Count > 0)
                   {
                       int iValue = 0;
                       string str = dsType.Tables[0].Rows[0][0] == DBNull.Value ? "" : dsType.Tables[0].Rows[0][0].ToString();
                       if (int.TryParse(str, out iValue))
                       {
                           ObjEArticle.TypID = iValue;
                           ObjEArticle.dtTyp = dsType.Tables[1];
                       }
                       else if (str.Contains("UNIQUE"))
                       {
                           if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                           {
                               throw new Exception("Dieser Datensatz existiert bereits.");
                           }
                           else
                           {
                               throw new Exception("Data Already Exists");
                           }
                       }                        
                           else
                           throw new Exception("Fehler beim Speichern von TYP");
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
           return ObjEArticle;
       }

        /// <summary>
        /// Code to fetch list of Typ 
        /// </summary>
        /// <param name="ObjEArticle"></param>
        /// <returns></returns>
        public EArticles GetTyp(EArticles ObjEArticle)
       {
           DataSet dsTyp = new DataSet();
           try
           {
               using (SqlCommand cmd = new SqlCommand())
               {
                   cmd.Connection = SQLCon.Sqlconn();
                   cmd.CommandType = CommandType.StoredProcedure;
                   cmd.CommandText = "[P_Get_Typ]";
                   using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                   {
                       da.Fill(dsTyp);
                   }
                   ObjEArticle.dtWG = dsTyp.Tables[0];
                   ObjEArticle.dtWI = dsTyp.Tables[1];
                   ObjEArticle.dtSupplier = dsTyp.Tables[2];
                   ObjEArticle.dtTyp = dsTyp.Tables[3];
               }
           }
           catch (Exception ex)
           {
               if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                   throw new Exception("Fehler bei der Datenaktualisierung für Typ");
               else
                   throw new Exception("Error While Retrieving the Typ");
           }
           finally
           {
               SQLCon.Close();
           }
           return ObjEArticle;
       }

        /// <summary>
        /// Code to add or edit rabatt from rabatt master
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="ObjEArticle"></param>
        /// <returns></returns>
       public EArticles SaveRabatt(XmlDocument xml, EArticles ObjEArticle)
       {
           DataSet dsRabatt = new DataSet();
           try
           {
               using (SqlCommand cmd = new SqlCommand())
               {
                   string innerxml = xml.InnerXml;
                   cmd.Connection = SQLCon.Sqlconn();
                   cmd.CommandType = CommandType.StoredProcedure;
                   cmd.CommandText = "[P_Ins_Rabatt]";
                   SqlParameter param = new SqlParameter("@XMLRabatt", SqlDbType.Xml);
                   param.Value = innerxml;
                   cmd.Parameters.Add(param);
                   using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                   {
                       da.Fill(dsRabatt);
                   }
                   if (dsRabatt != null && dsRabatt.Tables.Count > 0)
                   {
                       int iValue = 0;
                       string str = dsRabatt.Tables[0].Rows[0][0] == DBNull.Value ? "" : dsRabatt.Tables[0].Rows[0][0].ToString();
                       if (int.TryParse(str, out iValue))
                       {
                           ObjEArticle.RabattID = iValue;
                           if (dsRabatt.Tables.Count > 1)
                               ObjEArticle.dtRabatt = dsRabatt.Tables[1];
                       }
                       else if (str.Contains("UC_RabattTypMap"))
                       {
                           if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                           {
                               throw new Exception("Dieser TYP existiert bereits.");
                           }
                           else
                           {
                               throw new Exception("Typ OR Rabattgruppe Already Exists With Given ValidityDate");
                           }
                       }
                       else if (str.Contains("UC_Rabatt"))
                       {
                           if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                           {
                               throw new Exception("Diese Rabattgruppe existiert bereits");
                           }
                           else
                           {
                               throw new Exception("Rabatt Already Exists");
                           }
                       }                       
                           else
                           throw new Exception("Fehler beim Speichern der Rabattgruppe");
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
           return ObjEArticle;
       }

        /// <summary>
        /// Code to Fetch Rabatt list along with Typ mapping from database
        /// </summary>
        /// <param name="ObjEArticle"></param>
        /// <returns></returns>
        public EArticles GetRabatt(EArticles ObjEArticle)
       {
           DataSet dsRabatt = new DataSet();
           try
           {
               using (SqlCommand cmd = new SqlCommand())
               {
                   cmd.Connection = SQLCon.Sqlconn();
                   cmd.CommandType = CommandType.StoredProcedure;
                   cmd.CommandText = "[P_Get_Rabatt]";
                   using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                   {
                       da.Fill(dsRabatt);
                   }
                   if (dsRabatt != null && dsRabatt.Tables.Count > 0)
                       ObjEArticle.dtRabatt = dsRabatt.Tables[0];
               }
           }
           catch (Exception ex)
           {
               if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                   throw new Exception("Fehler bei der Datenaktualisierung für Rabattgruppen");
               else
                   throw new Exception("Error While Retrieving the Rabatt");
           }
           finally
           {
               SQLCon.Close();
           }
           return ObjEArticle;
       }

        /// <summary>
        /// Code to Save new set of Dimesions with new validity date from Article master
        /// </summary>
        /// <param name="ObjEArticle"></param>
        /// <returns></returns>
       public EArticles SaveDimensionCopy(EArticles ObjEArticle)
       {
           DataSet dsDimesions = new DataSet();
           try
           {
               using (SqlCommand cmd = new SqlCommand())
               {
                   cmd.Connection = SQLCon.Sqlconn();
                   cmd.CommandType = CommandType.StoredProcedure;
                   cmd.CommandText = "[P_Ins_DimensionCopy]";
                   cmd.Parameters.AddWithValue("@WIID", ObjEArticle.WIID);
                   cmd.Parameters.AddWithValue("@ValidityDate", ObjEArticle.ValidityDate);
                   cmd.Parameters.AddWithValue("@dtDimension", ObjEArticle.dtDim);
                   cmd.Parameters.AddWithValue("@IsCopy", ObjEArticle.IsCopy);
                   using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                   {
                       da.Fill(dsDimesions);
                   }
                   if (dsDimesions != null && dsDimesions.Tables.Count > 0 && dsDimesions.Tables[0].Rows.Count > 0)
                   {
                       string str = Convert.ToString(dsDimesions.Tables[0].Rows[0][0]);
                       int IValue = 0;
                       if (!int.TryParse(str, out IValue))
                           throw new Exception(str);
                       else
                           ObjEArticle.dtDimenstions = dsDimesions.Tables[0];
                   }
               }
           }
           catch (Exception ex)
           {
               if (ex.Message.Contains("UNIQUE"))
               {
                   if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                   {
                       throw new Exception("Das angegebene Gültigkeitsdatum besteht bereits");
                   }
                   else
                   {
                       throw new Exception("Selected Validity Date Already Exists with Dimensions");
                   }
               }                   
               else
                   throw new Exception("Error While Saving the Dimension");
           }
           finally
           {
               SQLCon.Close();
           }
           return ObjEArticle;
       }

        /// <summary>
        /// Code to import article data
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="ObjEArticle"></param>
        /// <returns></returns>
        public EArticles ImportArticleData(EArticles ObjEArticle)
       {
           try
           {
               string STR = string.Empty;
               DataSet dsDimesions = new DataSet();
               using (SqlCommand cmd = new SqlCommand())
               {
                   cmd.Connection = SQLCon.Sqlconn();
                   cmd.CommandType = CommandType.StoredProcedure;
                   cmd.CommandText = "[P_Imp_ArticleData]";
                   cmd.Parameters.AddWithValue("@dtArticle", ObjEArticle.dtArticleImport);
                   cmd.Parameters.AddWithValue("@dtDimensions", ObjEArticle.dtDimensionImport);
                   object ObjReturn = cmd.ExecuteScalar();
                   STR = Convert.ToString(ObjReturn);
               }
           }
           catch (Exception ex)
           {
               if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
               {
                   throw new Exception("Fehler beim Datenimport");
               }
               else
               {
                   throw new Exception("Error While Importing");
               }               
           }
           finally
           {
               SQLCon.Close();
           }
           return ObjEArticle;
       }

        /// <summary>
        /// Code to save Article mapping with accesories
        /// </summary>
        /// <param name="ObjEArticle"></param>
        /// <returns></returns>
       public EArticles SaveArticleMapping(EArticles ObjEArticle)
       {
           try
           {
                DataSet dsAcces = new DataSet();
               using (SqlCommand cmd = new SqlCommand())
               {
                   cmd.Connection = SQLCon.Sqlconn();
                   cmd.CommandType = CommandType.StoredProcedure;
                   cmd.CommandText = "[P_Ins_Accessories]";
                   cmd.Parameters.AddWithValue("@ParentID", ObjEArticle.ParentID);
                   cmd.Parameters.AddWithValue("@ChildWG", ObjEArticle. WG);
                   cmd.Parameters.AddWithValue("@ChildWA", ObjEArticle.WA);
                   cmd.Parameters.AddWithValue("@ChildWI", ObjEArticle.WI);
                   cmd.Parameters.AddWithValue("@UserID", ObjEArticle.UserID);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsAcces);
                    }
                    if(dsAcces != null && dsAcces.Tables.Count > 0  && dsAcces.Tables[0].Rows.Count > 0)
                    {
                        int IVAlue = 0;
                        if (int.TryParse(Convert.ToString(dsAcces.Tables[0].Rows[0][0]), out IVAlue))
                        {
                            ObjEArticle.AccessoriesID = IVAlue;
                            if (dsAcces.Tables.Count > 1)
                                ObjEArticle.dtAccessories = dsAcces.Tables[1];
                        }
                        else
                            throw new Exception(Convert.ToString(dsAcces.Tables[0].Rows[0][0]));
                    }
               }
           }
           catch (Exception ex)
           {
                if (ex.Message.Contains("Artikel"))
                    throw ex;
                else
                {
                    if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                        throw new Exception("Fehler beim Speichern von Zubehörangaben");
                    else
                        throw new Exception("Error while saving the accessories");
                }
           }
           finally
           {
               SQLCon.Close();
           }
           return ObjEArticle;
       }

        /// <summary>
        /// Code to get list of accessories
        /// </summary>
        /// <param name="ObjEArticle"></param>
        /// <returns></returns>
        public EArticles GetAccessories(EArticles ObjEArticle)
       {
           DataSet dsAccessories = new DataSet();
           try
           {
               using (SqlCommand cmd = new SqlCommand())
               {
                   cmd.Connection = SQLCon.Sqlconn();
                   cmd.CommandType = CommandType.StoredProcedure;
                   cmd.CommandText = "[P_Get_AccessorieS]";
                   cmd.Parameters.Add("@ParentID", ObjEArticle.ParentID);
                   using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                   {
                       da.Fill(dsAccessories);
                   }
                   if (dsAccessories != null && dsAccessories.Tables.Count > 0)
                       ObjEArticle.dtAccessories = dsAccessories.Tables[0];
               }
           }
           catch (Exception ex)
           {
               if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                   throw new Exception("Fehler bei der Datenaktualisierung für Zubehör");
               else
                   throw new Exception("Error While Retrieving the Accessories");
           }
           finally
           {
               SQLCon.Close();
           }
           return ObjEArticle;
       }

        /// <summary>
        /// Code to get accessories list while adding accessories to positions
        /// </summary>
        /// <param name="ObjEArticle"></param>
        /// <returns></returns>
       public EArticles GetAccessoriesForLVs(EArticles ObjEArticle)
       {
           DataSet dsAccessories = new DataSet();
           try
           {
               using (SqlCommand cmd = new SqlCommand())
               {
                   cmd.Connection = SQLCon.Sqlconn();
                   cmd.CommandType = CommandType.StoredProcedure;
                   cmd.CommandText = "[P_Get_AccessoriesForLVs]";
                   cmd.Parameters.Add("@WG", ObjEArticle.WG);
                   cmd.Parameters.Add("@WA", ObjEArticle.WA);
                   cmd.Parameters.Add("@WI", ObjEArticle.WI);
                   using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                   {
                       da.Fill(dsAccessories);
                   }
                   if (dsAccessories != null && dsAccessories.Tables.Count > 0)
                       ObjEArticle.dtAccessories = dsAccessories.Tables[0];
               }
           }
           catch (Exception ex)
           {
               if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                   throw new Exception("Fehler bei der Datenaktualisierung für Zubehör");
               else
                   throw new Exception("Error While Retrieving the Accessories");
           }
           finally
           {
               SQLCon.Close();
           }
           return ObjEArticle;
       }

        /// <summary>
        /// Code to get article details of based on accessory
        /// </summary>
        /// <param name="ObjEArticle"></param>
        /// <returns></returns>
        public EArticles GetArticleDetailsForAccessories(EArticles ObjEArticle)
       {
           DataSet dsArticleDetails = new DataSet();
           try
           {
               using (SqlCommand cmd = new SqlCommand())
               {
                   cmd.Connection = SQLCon.Sqlconn();
                   cmd.CommandType = CommandType.StoredProcedure;
                   cmd.CommandText = "[P_Get_ArticleForDetailKz]";
                   cmd.Parameters.Add("@WG", ObjEArticle.WG);
                   cmd.Parameters.Add("@WA", ObjEArticle.WA);
                   cmd.Parameters.Add("@WI", ObjEArticle.WI);
                   cmd.Parameters.Add("@A", ObjEArticle.A);
                   cmd.Parameters.Add("@B", ObjEArticle.B);
                   cmd.Parameters.Add("@L", ObjEArticle.L);
                   cmd.Parameters.Add("@dtSubmitDate", ObjEArticle.ValidityDate);
                   using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                   {
                       da.Fill(dsArticleDetails);
                   }
                   if (dsArticleDetails != null && dsArticleDetails.Tables.Count > 0)
                       ObjEArticle.dtArticleDetails = dsArticleDetails.Tables[0];
               }
           }
           catch (Exception ex)
           {
               if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                   throw new Exception("Fehler beim Laden von Artikeldaten");
               else
                   throw new Exception("Error While Retrieving article details");
           }
           finally
           {
               SQLCon.Close();
           }
           return ObjEArticle;
       }

        /// <summary>
        /// Code to fetch article details by Typ
        /// </summary>
        /// <param name="ObjEArticle"></param>
        /// <returns></returns>
       public EArticles GetArticleBytyp(EArticles ObjEArticle)
       {
           DataSet dsAccessories = new DataSet();
           try
           {
               using (SqlCommand cmd = new SqlCommand())
               {
                   cmd.Connection = SQLCon.Sqlconn();
                   cmd.CommandType = CommandType.StoredProcedure;
                   cmd.CommandText = "[P_Get_ArticleByType_Mapping]";
                   cmd.Parameters.Add("@Typ", ObjEArticle.Typ);
                   using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                   {
                       da.Fill(dsAccessories);
                   }
                   if (dsAccessories != null && dsAccessories.Tables.Count > 0)
                   {
                       ObjEArticle.dtArticleDetails = dsAccessories.Tables[0];
                       if (dsAccessories.Tables.Count > 1)
                           ObjEArticle.dtDimenstions = dsAccessories.Tables[1];
                   }
               }
           }
           catch (Exception ex)
           {
               if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                   throw new Exception("Fehler bei der Datenaktualisierung für Typ");
               else
                   throw new Exception("Error While Retrieving ArticleDetails");
           }
           finally
           {
               SQLCon.Close();
           }
           return ObjEArticle;
       }

        /// <summary>
        /// Code to fetch article details by WG
        /// </summary>
        /// <param name="ObjEArticle"></param>
        /// <returns></returns>
        public EArticles GetArticleByWG(EArticles ObjEArticle)
       {
           DataSet dsArticleDetails = new DataSet();
           try
           {
               using (SqlCommand cmd = new SqlCommand())
               {
                   cmd.Connection = SQLCon.Sqlconn();
                   cmd.CommandType = CommandType.StoredProcedure;
                   cmd.CommandText = "[P_Get_ArticleByWG_Mapping]";
                   cmd.Parameters.Add("@WG", ObjEArticle.WG);
                   cmd.Parameters.Add("@WA", ObjEArticle.WA);
                   cmd.Parameters.Add("@WI", ObjEArticle.WI);
                   using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                   {
                       da.Fill(dsArticleDetails);
                   }
                   if (dsArticleDetails != null && dsArticleDetails.Tables.Count > 0)
                   {
                       ObjEArticle.dtArticleDetails = dsArticleDetails.Tables[0];
                       ObjEArticle.dtDimenstions = dsArticleDetails.Tables[1];
                   }
               }
           }
           catch (Exception ex)
           {
               if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                   throw new Exception("Fehler bei der Datenaktualisierung für Typ");
               else
                   throw new Exception("Error While Retrieving ArticleDetails");
           }
           finally
           {
               SQLCon.Close();
           }
           return ObjEArticle;
       }

        /// <summary>
        /// Code to get mapped Typs based on article ID
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns></returns>
       public DataTable GetMultipleTyp(int _ID)
       {
           DataTable dtTyp = new DataTable();
           try
           {
               using (SqlCommand cmd = new SqlCommand())
               {
                   cmd.Connection = SQLCon.Sqlconn();
                   cmd.CommandType = CommandType.StoredProcedure;
                   cmd.CommandText = "[P_Get_MultipleTyp]";
                   cmd.Parameters.Add("@WIID", _ID);
                   using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                   {
                       da.Fill(dtTyp);
                   }                   
               }
           }
           catch (Exception ex)
           {
               if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                   throw new Exception("Fehler bei der Datenaktualisierung für Typ");
               else
                   throw new Exception("Error While Retrieving Typ");
           }
           finally
           {
               SQLCon.Close();
           }
           return dtTyp;
       }

        /// <summary>
        /// Code to get available validity dates based on article ID
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns></returns>
        public DataTable GetValidityDates(int _ID)
       {
           DataTable dtDates = new DataTable();
           try
           {
               using (SqlCommand cmd = new SqlCommand())
               {
                   cmd.Connection = SQLCon.Sqlconn();
                   cmd.CommandType = CommandType.StoredProcedure;
                   cmd.CommandText = "[P_Get_DimensionValidiyDates]";
                   cmd.Parameters.Add("@WIID", _ID);
                   using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                   {
                       da.Fill(dtDates);
                   }
               }
           }
           catch (Exception ex)
           {
               if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                   throw new Exception("Fehler bei der Datenaktualisierung für Datum");
               else
                   throw new Exception("Error While Retrieving ValidityDate");
           }
           finally
           {
               SQLCon.Close();
           }
           return dtDates;
       }

        /// <summary>
        /// Code to get validity dates of dimensions
        /// </summary>
        /// <param name="_ID"></param>
        /// <param name="_date"></param>
        /// <returns></returns>
       public DataTable GetValidityDatesDimensions(int _ID,DateTime _date)
       {
           DataTable dtDates = new DataTable();
           try
           {
               using (SqlCommand cmd = new SqlCommand())
               {
                   cmd.Connection = SQLCon.Sqlconn();
                   cmd.CommandType = CommandType.StoredProcedure;
                   cmd.CommandText = "[P_Get_DimensionsByDate]";
                   cmd.Parameters.Add("@WIID", _ID);
                   cmd.Parameters.Add("@ValidityDate", _date);
                   using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                   {
                       da.Fill(dtDates);
                   }
               }
           }
           catch (Exception ex)
           {
               if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                   throw new Exception("Fehler bei der Datenaktualisierung für Datum");
               else
                   throw new Exception("Error While Retrieving ValidityDate");
           }
           finally
           {
               SQLCon.Close();
           }
           return dtDates;
       }

        /// <summary>
        /// Code to get mapped Typs based on Rabatt ID
        /// </summary>
        /// <param name="ObjEArticle"></param>
        /// <returns></returns>
       public EArticles GetTypByRabatt(EArticles ObjEArticle)
       {
           try
           {
               ObjEArticle.dtTypID = new DataTable();
               using (SqlCommand cmd = new SqlCommand())
               {
                   cmd.Connection = SQLCon.Sqlconn();
                   cmd.CommandType = CommandType.StoredProcedure;
                   cmd.CommandText = "[P_Get_TypByRabatt]";
                   cmd.Parameters.Add("@RabattID", ObjEArticle.RID);
                   using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                   {
                       da.Fill(ObjEArticle.dtTypID);
                   }
               }
           }
           catch (Exception ex)
           {
               throw new Exception("Error While Retrieving Types");
           }
           finally
           {
               SQLCon.Close();
           }
           return ObjEArticle;
       }
       
        /// <summary>
        /// Code to save Rabatt typ mapping
        /// </summary>
        /// <param name="ObjEArticle"></param>
        /// <returns></returns>
       public EArticles SaveTypRabattMapping(EArticles ObjEArticle)
       {
           DataSet dsRabatt = new DataSet();
           try
           {
               using (SqlCommand cmd = new SqlCommand())
               {
                   cmd.Connection = SQLCon.Sqlconn();
                   cmd.CommandType = CommandType.StoredProcedure;
                   cmd.CommandText = "[P_Ins_RabattTypMapping]";
                   cmd.Parameters.Add("@RabattID", ObjEArticle.RabattID);
                   cmd.Parameters.Add("@TypID", ObjEArticle.TypID);
                   using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                   {
                       da.Fill(dsRabatt);
                   }
                   if (dsRabatt != null && dsRabatt.Tables.Count > 0)
                   {
                       int iValue = 0;
                       string str = Convert.ToString(dsRabatt.Tables[0].Rows[0][0]);
                       if (int.TryParse(str, out iValue))
                       {
                           ObjEArticle.RabattTypID = iValue;
                           if (dsRabatt.Tables.Count > 1)
                               ObjEArticle.dtTypID = dsRabatt.Tables[1];
                       }
                       else if (str.Contains("UC_RabattTypMap"))
                       {
                           if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                               throw new Exception("Dieser TYP existiert bereits.");
                           else
                               throw new Exception("Typ OR Rabattgruppe Already Exists With Given ValidityDate");
                       }
                       else
                           throw new Exception("Fehler beim Speichern der Rabattgruppe");
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
           return ObjEArticle;
       }

        /// <summary>
        /// Code to get Typ list while mapping Rabbat
        /// </summary>
        /// <param name="ObjEArticle"></param>
        /// <returns></returns>
       public EArticles GetTypForRabatt(EArticles ObjEArticle)
       {
           DataSet dsTyp = new DataSet();
           try
           {
               using (SqlCommand cmd = new SqlCommand())
               {
                   cmd.Connection = SQLCon.Sqlconn();
                   cmd.CommandType = CommandType.StoredProcedure;
                   cmd.CommandText = "[P_Get_TypForRabatt]";
                   using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                   {
                       da.Fill(dsTyp);
                   }
                   if (dsTyp != null && dsTyp.Tables.Count > 0)
                       ObjEArticle.dtTyp = dsTyp.Tables[0];
               }
           }
           catch (Exception ex)
           {
               if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                   throw new Exception("Fehler bei der Datenaktualisierung für Typ");
               else
                   throw new Exception("Error While Retrieving the Typ");
           }
           finally
           {
               SQLCon.Close();
           }
           return ObjEArticle;
       }

        /// <summary>
        /// Code to update the rabatt values
        /// </summary>
        /// <param name="ObjEArticle"></param>
        /// <returns></returns>
       public EArticles UpdateRabatt(EArticles ObjEArticle)
       {
           DataSet dsRabatt = new DataSet();
           try
           {
               using (SqlCommand cmd = new SqlCommand())
               {
                   cmd.Connection = SQLCon.Sqlconn();
                   cmd.CommandType = CommandType.StoredProcedure;
                   cmd.CommandText = "[P_Upd_Rabatt]";
                   cmd.Parameters.Add("@RabattID", ObjEArticle.RabattID);
                   cmd.Parameters.Add("@Rabatt", ObjEArticle.Rabatt);
                   cmd.Parameters.Add("@ValidityDate", ObjEArticle.ValidityDate);
                   cmd.Parameters.Add("@Multi1", ObjEArticle.Multi1);
                   cmd.Parameters.Add("@Multi2", ObjEArticle.Multi2);
                   cmd.Parameters.Add("@Multi3", ObjEArticle.Multi3);
                   cmd.Parameters.Add("@Multi4", ObjEArticle.Multi4);
                   cmd.Parameters.Add("@Flag", ObjEArticle.Flag);
                   using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                   {
                       da.Fill(dsRabatt);
                   }
                   if (dsRabatt != null && dsRabatt.Tables.Count > 0)
                   {
                       int iValue = 0;
                       string str = Convert.ToString(dsRabatt.Tables[0].Rows[0][0]);
                       if (int.TryParse(str, out iValue))
                       {
                           ObjEArticle.RabattID = iValue;
                           if (dsRabatt.Tables.Count > 1)
                               ObjEArticle.dtRabatt = dsRabatt.Tables[1];
                       }
                       else if (str.Contains("UC_RabattTypMap"))
                       {
                           if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                               throw new Exception("Dieser TYP existiert bereits.");
                           else
                               throw new Exception("Typ OR Rabattgruppe Already Exists With Given ValidityDate");
                       }
                       else if (str.Contains("UC_Rabatt"))
                       {
                           if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                               throw new Exception("Diese Rabattgruppe existiert bereits");
                           else
                               throw new Exception("Rabatt Already Exists");
                       }
                       else
                           throw new Exception("Fehler beim Speichern der Rabattgruppe");
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
           return ObjEArticle;
       }

        /// <summary>
        /// Code to get typ by Article ID
        /// </summary>
        /// <param name="ObjEArticle"></param>
        /// <returns></returns>
       public EArticles GetTypByWIID(EArticles ObjEArticle)
       {
           DataSet dsTyp = new DataSet();
           try
           {
               using (SqlCommand cmd = new SqlCommand())
               {
                   cmd.Connection = SQLCon.Sqlconn();
                   cmd.CommandType = CommandType.StoredProcedure;
                   cmd.CommandText = "[P_Get_GetTypByWIID]";
                   cmd.Parameters.Add("@WIID", ObjEArticle.WIID);
                   using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                   {
                       da.Fill(dsTyp);
                   }
                   if (dsTyp != null && dsTyp.Tables.Count > 0)
                   {
                       int IValue = 0;
                       if (dsTyp.Tables[0].Rows.Count > 0 && int.TryParse(Convert.ToString(dsTyp.Tables[0].Rows[0][0]),out IValue))
                           ObjEArticle.RTID = IValue;
                       else
                           ObjEArticle.RTID = 0;

                       if (dsTyp.Tables.Count > 1)
                           ObjEArticle.dtTyp = dsTyp.Tables[1];
                   }
               }
           }
           catch (Exception ex)
           {
               if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                   throw new Exception("Fehler bei der Datenaktualisierung für Typ");
               else
                   throw new Exception("Error While Retrieving the Typ");
           }
           finally
           {
               SQLCon.Close();
           }
           return ObjEArticle;
       }

        /// <summary>
        /// Code to delete a dimension from article
        /// </summary>
        /// <param name="ObjEArticle"></param>
        /// <returns></returns>
        public EArticles DeleteDimension(EArticles ObjEArticle)
        {
            DataSet dsDimension = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Del_Dimension]";
                    cmd.Parameters.Add("@DID", ObjEArticle.DimensionID);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsDimension);
                    }
                    if (dsDimension != null && dsDimension.Tables.Count > 0 && dsDimension.Tables[0].Rows.Count > 0)
                    {
                        string st = Convert.ToString(dsDimension.Tables[0].Rows[0][0]);
                        if (!string.IsNullOrEmpty(st))
                            throw new Exception(st);
                        if(dsDimension.Tables.Count > 1)
                        {
                            ObjEArticle.dtDimenstions = dsDimension.Tables[1];
                        }
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
            return ObjEArticle;
        }

        /// <summary>
        /// Code to delete Article
        /// </summary>
        /// <param name="ObjEArticle"></param>
        /// <returns></returns>
        public EArticles DeleteWI(EArticles ObjEArticle)
        {
            DataSet dsWI = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Del_WI]";
                    cmd.Parameters.Add("@WIID", ObjEArticle.WIID);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsWI);
                    }
                    if (dsWI != null && dsWI.Tables.Count > 0 && dsWI.Tables[0].Rows.Count > 0)
                    {
                        string st = Convert.ToString(dsWI.Tables[0].Rows[0][0]);
                        if (!string.IsNullOrEmpty(st))
                            throw new Exception(st);
                        if (dsWI.Tables.Count > 1)
                            ObjEArticle.dtWI = dsWI.Tables[1];
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
            return ObjEArticle;
        }

        /// <summary>
        /// Code to delete Article
        /// </summary>
        /// <param name="ObjEArticle"></param>
        /// <returns></returns>
        public EArticles DeleteWG(EArticles ObjEArticle)
        {
            DataSet dsRabatt = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Del_WG]";
                    cmd.Parameters.Add("@WGID", ObjEArticle.WGID);
                    object objreturn = cmd.ExecuteScalar();
                    string st = Convert.ToString(objreturn);
                    if (!string.IsNullOrEmpty(st))
                        throw new Exception(st);
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
            return ObjEArticle;
        }

        /// <summary>
        /// Code to delete typ
        /// </summary>
        /// <param name="ObjEArticle"></param>
        /// <returns></returns>
        public EArticles DeleteTyp(EArticles ObjEArticle)
        {
            DataSet dsRabatt = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Del_Typ]";
                    cmd.Parameters.Add("@TypID", ObjEArticle.TypID);
                    object objreturn = cmd.ExecuteScalar();
                    string st = Convert.ToString(objreturn);
                    if (!string.IsNullOrEmpty(st))
                        throw new Exception(st);
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
            return ObjEArticle;
        }

        /// <summary>
        /// Code to update dimension values
        /// </summary>
        /// <param name="ObjEArticle"></param>
        /// <returns></returns>
        public EArticles UpdateDimension(EArticles ObjEArticle)
        {
            DataSet dsDimension = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Upd_Dimension]";
                    cmd.Parameters.Add("@DimensionID", ObjEArticle.DimensionID);
                    cmd.Parameters.Add("@Minutes", ObjEArticle.Minuten);
                    cmd.Parameters.Add("@ListPrice", ObjEArticle.ListPrice);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsDimension);
                    }
                    if(dsDimension != null && dsDimension.Tables.Count > 0 && dsDimension.Tables[0].Rows.Count > 0)
                    {
                        ObjEArticle.dtDimenstions = dsDimension.Tables[0];
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
            return ObjEArticle;
        }

        /// <summary>
        /// Code to delete rabatta typ mapping
        /// </summary>
        /// <param name="ObjEArticle"></param>
        /// <returns></returns>
        public EArticles DeleteRabattTypMap(EArticles ObjEArticle)
        {
            DataSet dsRabatt = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Del_TypRabattMap]";
                    cmd.Parameters.Add("@RabattTypMapID", ObjEArticle.RabattTypID);
                    object objreturn = cmd.ExecuteScalar();
                    string st = Convert.ToString(objreturn);
                    if (!string.IsNullOrEmpty(st))
                        throw new Exception(st);
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
            return ObjEArticle;
        }

        /// <summary>
        /// Code to fetch dimensions validity dates
        /// </summary>
        /// <param name="ObjEArticle"></param>
        /// <returns></returns>
        public EArticles GetDimensionValidityDates(EArticles ObjEArticle)
        {
            DataSet dsArticleDetails = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_DimensionValidityDates]";
                    cmd.Parameters.Add("@WG", ObjEArticle.WG);
                    cmd.Parameters.Add("@WA", ObjEArticle.WA);
                    cmd.Parameters.Add("@WI", ObjEArticle.WI);
                    cmd.Parameters.Add("@A", ObjEArticle.A);
                    cmd.Parameters.Add("@B", ObjEArticle.B);
                    cmd.Parameters.Add("@L", ObjEArticle.L);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsArticleDetails);
                    }
                    if (dsArticleDetails != null && dsArticleDetails.Tables.Count > 0)
                    {
                        ObjEArticle.dtDimensionValidityDates = new DataTable();
                        ObjEArticle.dtDimensionValidityDates = dsArticleDetails.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                    throw new Exception("Fehler bei der Datenaktualisierung für Gültigkeitsdatum");
                else
                    throw new Exception("Error While Retrieving ArticleDetails");
            }
            finally
            {
                SQLCon.Close();
            }
            return ObjEArticle;
        }

        /// <summary>
        /// Code to fetch values based on article and dimension
        /// </summary>
        /// <param name="ObjEArticle"></param>
        /// <returns></returns>
        public EArticles GetValuesByDimension(EArticles ObjEArticle)
        {
            DataSet dsArticleDetails = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_ArticleByValidityDate]";
                    cmd.Parameters.Add("@DimensionID", ObjEArticle.DimensionID);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsArticleDetails);
                    }
                    if (dsArticleDetails != null && dsArticleDetails.Tables.Count > 0)
                    {
                        ObjEArticle.dtDimensionValues = new DataTable();
                        ObjEArticle.dtDimensionValues = dsArticleDetails.Tables[0];
                        if (dsArticleDetails.Tables.Count > 1)
                        {
                            ObjEArticle.dtMultiValues = new DataTable();
                            ObjEArticle.dtMultiValues = dsArticleDetails.Tables[1];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                    throw new Exception("Fehler bei der Datenaktualisierung für Gültigkeitsdatum");
                else
                    throw new Exception("Error While Retrieving ArticleDetails");
            }
            finally
            {
                SQLCon.Close();
            }
            return ObjEArticle;
        }

        /// <summary>
        /// Code to get Articles for accessories mapping
        /// </summary>
        /// <param name="ObjEArticle"></param>
        /// <returns></returns>
        public EArticles GetArticleForAccessories(EArticles ObjEArticle)
        {
            DataSet dsArticles = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_ArticlesForMapping]";
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsArticles);
                    }
                    ObjEArticle.dtArt = new DataTable();
                    ObjEArticle.dtArt = dsArticles.Tables[0];
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                    throw new Exception("Fehler bei der Datenaktualisierung für Artikel");
                else
                    throw new Exception("Error While Retrieving Articles");
            }
            finally
            {
                SQLCon.Close();
            }
            return ObjEArticle;
        }

        /// <summary>
        /// Code to delete accessory mapping
        /// </summary>
        /// <param name="ObjEArticle"></param>
        /// <returns></returns>
        public EArticles DeleteAccessory(EArticles ObjEArticle)
        {
            DataSet dsWI = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Del_Accessories]";
                    cmd.Parameters.Add("@AccessoryID", ObjEArticle.AccessoriesID);
                    object ObjReturn = cmd.ExecuteScalar();
                    int iv = 0;
                    if (!int.TryParse(Convert.ToString(ObjReturn), out iv))
                        throw new Exception("Error while deleting Accessory");
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
            return ObjEArticle;
        }

        /// <summary>
        /// Code to get dimensions while mapping accessories
        /// </summary>
        /// <param name="ObjEArticle"></param>
        /// <returns></returns>
        public EArticles GetAccessoryDimension(EArticles ObjEArticle)
        {
            DataSet dsArticles = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_DimensionByAccessory]";
                    cmd.Parameters.Add("@WG", ObjEArticle.ChildWG);
                    cmd.Parameters.Add("@WA", ObjEArticle.ChildWA);
                    cmd.Parameters.Add("@WI", ObjEArticle.ChildWI);
                    cmd.Parameters.Add("@A", ObjEArticle.A);
                    cmd.Parameters.Add("@B", ObjEArticle.B);
                    cmd.Parameters.Add("@L", ObjEArticle.L);
                    cmd.Parameters.Add("@dtSubmitDate", ObjEArticle.ValidityDate);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsArticles);
                    }
                    ObjEArticle.dtArt = new DataTable();
                    ObjEArticle.dtArt = dsArticles.Tables[0];
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                    throw new Exception("Fehler bei der Datenaktualisierung für Artikel");
                else
                    throw new Exception("Error While Retrieving Dimensions");
            }
            finally
            {
                SQLCon.Close();
            }
            return ObjEArticle;
        }

    }
}
