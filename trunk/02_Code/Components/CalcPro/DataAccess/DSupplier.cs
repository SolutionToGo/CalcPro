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
    public class DSupplier
    {
        /// <summary>
        /// Code to add or edit supplier details from supplier master to database
        /// </summary>
        /// <param name="ObjEsupplier"></param>
        /// <returns></returns>
        public ESupplier SaveSupplierDetails(XmlDocument XmlDoc, ESupplier ObjESupplier)
        {
            int SupplierID = -1;
            DataSet ds = new DataSet();
            try
            {

                string innerxml = XmlDoc.InnerXml;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Ins_Supplier]";
                    SqlParameter param = new SqlParameter("@XMLSupplier", SqlDbType.Xml);
                    param.Value = innerxml;
                    cmd.Parameters.Add(param);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
                    }
                    string str = ds.Tables[0].Rows[0][0] == DBNull.Value ? "" : ds.Tables[0].Rows[0][0].ToString();
                    if (!string.IsNullOrEmpty(str))
                    {
                        if (int.TryParse(str, out SupplierID))
                        {
                            ObjESupplier.SupplierID = SupplierID;
                            ObjESupplier.dtSupplier = ds.Tables[1];
                        }
                        else if (str.ToString().Contains("duplicate"))
                        {
                            if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                                throw new Exception("Dieser Kurzname ist bereits vergeben");
                            else
                                throw new Exception("ShortName is already exists.!");
                        }
                        else
                            throw new Exception(str);
                    }
                }
            }
            catch (Exception ex) { throw; }
            finally { SQLCon.Close(); }
            return ObjESupplier;
        }

        /// <summary>
        /// Code to fetch supplier list from database
        /// </summary>
        /// <param name="ObjEsupplier"></param>
        /// <returns></returns>
        public DataSet GetSupplier()
        {
            DataSet dsSupplier = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_Supplier]";
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsSupplier);
                    }
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                    throw new Exception("Fehler beim Laden des Kunden");
                else
                    throw new Exception("Error Occured While Retreiving Customer");
            }
            finally
            {
                SQLCon.Close();
            }
            return dsSupplier;
        }

        /// <summary>
        /// Code to Add or Edit supplier contact details from Supplier master to database
        /// </summary>
        /// <param name="ObjEsupplier"></param>
        /// <returns></returns>
        public ESupplier SavedsSupplierContactDetails(XmlDocument XmlDoc,ESupplier ObjESupplier)
        {
            DataSet ds = new DataSet();
            int ContactPersonID = -1;
            try
            {
                string innerxml = XmlDoc.InnerXml;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Ins_Supplier_Contact]";
                    SqlParameter param = new SqlParameter("@XMLSupplierContact", SqlDbType.Xml);
                    param.Value = innerxml;
                    cmd.Parameters.Add(param);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
                    }
                    string str = ds.Tables[0].Rows[0][0] == DBNull.Value ? "" : ds.Tables[0].Rows[0][0].ToString();
                    if (!string.IsNullOrEmpty(str))
                    {
                        if (int.TryParse(str, out ContactPersonID))
                        {
                            ObjESupplier.ContactPersonID = ContactPersonID;
                            ObjESupplier.dtContact = ds.Tables[1];
                        }
                        else
                        {
                            throw new Exception(str);
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
            return ObjESupplier;
        }

        /// <summary>
        /// Code to Add or Edit Supplier address details from Supplier master to database
        /// </summary>
        /// <param name="ObjEsupplier"></param>
        /// <returns></returns>
        public ESupplier SaveSupplierAddressDetails(XmlDocument XmlDoc, ESupplier ObjESupplier)
        {
            DataSet ds = new DataSet();
            int AddressID = -1;
            try
            {
                string innerxml = XmlDoc.InnerXml;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Ins_Supplier_Address]";
                    SqlParameter param = new SqlParameter("@XMLSupplierAddress", SqlDbType.Xml);
                    param.Value = innerxml;
                    cmd.Parameters.Add(param);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
                    }
                    string str = ds.Tables[0].Rows[0][0] == DBNull.Value ? "" : ds.Tables[0].Rows[0][0].ToString();
                    if (!string.IsNullOrEmpty(str))
                    {
                        if (int.TryParse(str, out AddressID))
                        {
                            ObjESupplier.AddressID = AddressID;
                            ObjESupplier.dtAddress = ds.Tables[1];
                        }
                        else if (str.ToString().Contains("DefaultAddress"))
                        {
                            if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                            {
                                throw new Exception("Diese Standardadresse existiert bereits");
                            }
                            else
                            {
                                throw new Exception("Default Address is already exists.!");
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
            return ObjESupplier;
        }

        /// <summary>
        /// Code to save Supplier articles from Supplier master to database
        /// </summary>
        /// <param name="ObjEsupplier"></param>
        /// <returns></returns>
        public ESupplier SaveArticle(ESupplier ObjESupplier)
        {
             DataSet ds = new DataSet();
            int iArtcleID = -1;
            try
            {
                using ( SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Ins_WGWA]";
                    cmd.Parameters.AddWithValue("@WGWAID", ObjESupplier.WGWAID);
                    cmd.Parameters.AddWithValue("@SupplierID", ObjESupplier.SupplierID);
                    cmd.Parameters.AddWithValue("@WG", ObjESupplier.WG);
                    cmd.Parameters.AddWithValue("@WA", ObjESupplier.WA);
                    cmd.Parameters.AddWithValue("@WGDescription", ObjESupplier.WGDescription);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
                    }
                    string str = ds.Tables[0].Rows[0][0] == DBNull.Value ? "" : ds.Tables[0].Rows[0][0].ToString();
                    if (!string.IsNullOrEmpty(str))
                    {
                        if (int.TryParse(str, out iArtcleID))
                        {
                            ObjESupplier.WGWAID = iArtcleID;
                            ObjESupplier.dtArticle = ds.Tables[1];
                        }
                        else if (str.ToString().Contains("UNIQUE"))
                        {
                            if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                            {
                                throw new Exception("Der Artikel existiert bereits.");
                            }
                            else
                            {
                                throw new Exception("Article already exists..!");
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
            return ObjESupplier;
        }

        /// <summary>
        ///  Code to save Supplier and Article mapping form Price comparision module
        /// </summary>
        /// <param name="ObjESupplier"></param>
        /// <returns></returns>
        public ESupplier SaveArticleFromProposal(ESupplier ObjESupplier)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Ins_WGWAInUpdateSupplierProposal]";
                    cmd.Parameters.AddWithValue("@SupplierID", ObjESupplier.SupplierID);
                    cmd.Parameters.AddWithValue("@SupplierProposalID", ObjESupplier. SupplierProposalID);
                    cmd.ExecuteNonQuery();
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
            return ObjESupplier;
        }

        /// <summary>
        /// Code fetch positions from database for not saved supplier propsoal by passing ProjectID, LVSection, Articles as imput parameters
        /// </summary>
        /// <param name="ObjESupplier"></param>
        /// <returns></returns>
        public ESupplier GetPositionsforsupplierProposal(ESupplier ObjESupplier)
        {
            DataSet dsPositions = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_PositionsForSsupplierProposal]";
                    cmd.Parameters.AddWithValue("@ProjectID", ObjESupplier.ProjectID);
                    cmd.Parameters.AddWithValue("@LVSection", ObjESupplier.LVSection);
                    cmd.Parameters.AddWithValue("@LVSectionID", ObjESupplier.LVSectionID);
                    cmd.Parameters.AddWithValue("@dtArticleID", ObjESupplier.dtArticleID);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsPositions);
                    }
                    if (dsPositions != null && dsPositions.Tables.Count > 0)
                    {
                        ObjESupplier.dtNewPositions = dsPositions.Tables[0];
                        if (dsPositions.Tables.Count > 1)
                        {
                            ObjESupplier.dtSupplier_SP = dsPositions.Tables[1];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                {
                    throw new Exception("Fehler beim Laden der Daten");
                }
                else
                {
                    throw new Exception("Error Occured While Retreiving records");

                }
            }
            finally
            {
                SQLCon.Close();
            }
            return ObjESupplier;
        }

        /// <summary>
        /// Code to fetch positions from database based on supplier proposalID
        /// </summary>
        /// <param name="ObjESupplier"></param>
        /// <returns></returns>
        public ESupplier GetPositionsByProposalID(ESupplier ObjESupplier)
        {
            DataSet dsPositions = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_PositionsByProposalID]";
                    cmd.Parameters.AddWithValue("@ProjectID", ObjESupplier.ProjectID);
                    cmd.Parameters.AddWithValue("@LVSection", ObjESupplier.LVSection);
                    cmd.Parameters.AddWithValue("@LVSectionID", ObjESupplier.LVSectionID);
                    cmd.Parameters.AddWithValue("@SupplierProposalID", ObjESupplier.ProposalID);
                    cmd.Parameters.AddWithValue("@dtArticleID", ObjESupplier.dtArticleID);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsPositions);
                    }
                    if (dsPositions != null && dsPositions.Tables.Count > 0)
                    {
                        ObjESupplier.dtNewPositions = dsPositions.Tables[0];
                        if (dsPositions.Tables.Count > 1)
                        {
                            ObjESupplier.dtDeletedPositions = dsPositions.Tables[1];
                            if (dsPositions.Tables.Count > 2)
                            {
                                ObjESupplier.dtProposedPositions = dsPositions.Tables[2];
                                if (dsPositions.Tables.Count > 3)
                                {
                                    ObjESupplier.dtSupplier_SP = dsPositions.Tables[3];
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                    throw new Exception("Fehler beim Laden der Daten");
                else
                    throw new Exception("Error Occured While Retreiving records");
            }
            finally
            {
                SQLCon.Close();
            }
            return ObjESupplier;
        }

        /// <summary>
        /// Code to fetch project LV sections using projectid for supplier proposal module
        /// </summary>
        /// <param name="ObjEsupplier"></param>
        /// <returns></returns>
        public ESupplier GetLVSectionforProposal(ESupplier ObjEsupplier)
        {
            try
            {
                ObjEsupplier.dtLVSection = new DataTable();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_LVSectionForProposal]";
                    cmd.Parameters.AddWithValue("@ProjectID", ObjEsupplier.ProjectID);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ObjEsupplier.dtLVSection);
                    }
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                    throw new Exception("Fehler beim Laden der Daten");
                else
                    throw new Exception("Error Occured While Retreiving records");
            }
            finally{SQLCon.Close();}
            return ObjEsupplier;
        }

        /// <summary>
        /// Code to save Supplier proposal
        /// </summary>
        /// <param name="ObjEsupplier"></param>
        /// <returns></returns>
        public ESupplier SaveSupplierProposal(ESupplier ObjESupplier)
        {
            DataSet ds = new DataSet();
            int ProposalID = -1;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Ins_SupplierProposal_1]";
                    cmd.Parameters.AddWithValue("@SupplierProposalID", ObjESupplier.ProposalID);
                    cmd.Parameters.AddWithValue("@ProjectID", ObjESupplier.ProjectID);
                    cmd.Parameters.AddWithValue("@LVSection", ObjESupplier.LVSection);
                    cmd.Parameters.AddWithValue("@dtArticleID", ObjESupplier.dtArticleID);
                    cmd.Parameters.AddWithValue("@dtPositionID", ObjESupplier.dtPositionsToDB);
                    cmd.Parameters.AddWithValue("@dtSupplierID", ObjESupplier.dtSupplierToDB);
                    cmd.Parameters.AddWithValue("@dtDeletedPositions", ObjESupplier. dtDeletedPositionsToDB);
                    cmd.Parameters.AddWithValue("@LVSectionID", ObjESupplier.LVSectionID);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
                    }
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        string str = ds.Tables[0].Rows[0][0] == DBNull.Value ? "" : ds.Tables[0].Rows[0][0].ToString();
                        if (!string.IsNullOrEmpty(str))
                        {
                            if (int.TryParse(str, out ProposalID))
                                ObjESupplier.ProposalID = ProposalID;
                            else
                                throw new Exception(str);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Cannot"))
                {
                    if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                        throw new Exception("Die Preisanfrage ist auf 8 Lieferanten begrenzt");
                    else
                        throw new Exception("Cannot send proposal to more than 8 suppliers");
                }
                else
                {
                    if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                        throw new Exception("Fehler beim Laden der Daten");
                    else
                        throw new Exception("Error occured while saving suplier proposal");
                }
            }
            finally
            {
                SQLCon.Close();
            }
            return ObjESupplier;
        }

        /// <summary>
        /// Code to fetch saved adn not saved supplier proposal Numbers
        /// </summary>
        /// <param name="ObjEsupplier"></param>
        /// <returns></returns>
        public ESupplier GetProposalNumber(ESupplier ObjESupplier)
        {
            DataSet dsWGWA = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_SupplierProposal_1]";
                    cmd.Parameters.AddWithValue("@ProjectID", ObjESupplier.ProjectID);
                    cmd.Parameters.AddWithValue("@LVSection", ObjESupplier.LVSection);
                    cmd.Parameters.AddWithValue("@LVSectionID", ObjESupplier.LVSectionID);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsWGWA);
                    }
                    if(dsWGWA != null)
                        ObjESupplier.dsProposalNumbers = dsWGWA;
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                {
                    throw new Exception("Fehler beim Laden der Daten");
                }
                else
                {
                    throw new Exception("Error Occured While Retreiving records");
                }                    
            }
            finally
            {
                SQLCon.Close();
            }
            return ObjESupplier;
        }

        /// <summary>
        /// Code to fetch not saved articles for supplier proposal module while merging articles with proposals
        /// </summary>
        /// <param name="ObjEsupplier"></param>
        /// <returns></returns>
        public ESupplier GetArticlesForProposal(ESupplier ObjESupplier)
        {
            DataSet dsArticle = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_ArticleForProposal]";
                    cmd.Parameters.AddWithValue("@ProjectID", ObjESupplier.ProjectID);
                    cmd.Parameters.AddWithValue("@LVSectionID", ObjESupplier.LVSectionID);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsArticle);
                    }
                    if (dsArticle != null && dsArticle.Tables.Count > 0)
                    {
                        ObjESupplier.dtProposalArticles = dsArticle.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                {
                    throw new Exception("Fehler beim Laden der Daten");
                }
                else
                {
                    throw new Exception("Error Occured While Retreiving records");
                }
            }
            finally
            {
                SQLCon.Close();
            }
            return ObjESupplier;
        }

        /// <summary>
        ///  Code to fetch Supplier proposals for proce comparision module
        /// </summary>
        /// <param name="ObjEsupplier"></param>
        /// <returns></returns>
        public ESupplier GetUpdateSupplierProposal(ESupplier ObjESupplier)
        {
            DataSet dsWGWA = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_UpdateSupplierProposal]";
                    cmd.Parameters.AddWithValue("@ProjectID", ObjESupplier.ProjectID);
                    cmd.Parameters.AddWithValue("@LVSsection", ObjESupplier.LVSection);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsWGWA);
                    }
                    if (dsWGWA != null && dsWGWA.Tables.Count > 0)
                    {
                        ObjESupplier.dsProposal = dsWGWA;
                    }
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                {
                    throw new Exception("Fehler beim Laden der Daten");
                }
                else
                {
                    throw new Exception("Error Occured While Retreiving records");
                }
            }
            finally
            {
                SQLCon.Close();
            }
            return ObjESupplier;
        }

        /// <summary>
        /// Code to fetch positions based on supplier proposalID for price comparision module
        /// </summary>
        /// <param name="ObjESupplier"></param>
        /// <param name="_IsCalculate"></param>
        /// <returns></returns>
        public ESupplier GetProposalPostions(ESupplier ObjESupplier)
        {
            DataSet dsPositions = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_PositionsForProposal]";
                    cmd.Parameters.AddWithValue("@SupplierProposalID", ObjESupplier.SupplierProposalID);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsPositions);
                    }
                    if (dsPositions != null && dsPositions.Tables.Count > 0)
                    {
                        ObjESupplier.dtPositions = dsPositions.Tables[0];
                        if(dsPositions.Tables.Count > 1)
                            ObjESupplier.dtSuppliers = dsPositions.Tables[1];
                    }
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                {
                    throw new Exception("Fehler beim Laden der LV Position");
                }
                else
                {
                    throw new Exception("Error Occured While Retreiving Positions");
                }
            }
            finally
            {
                SQLCon.Close();
            }
            return ObjESupplier;
        }

        /// <summary>
        /// Code to update price to postions from price comparision module
        /// </summary>
        /// <param name="ObjESupplier"></param>
        /// <returns></returns>
        public ESupplier UpdateSupplierPrice(ESupplier ObjESupplier)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[p_Upd_SupplierPrice]";
                    cmd.Parameters.AddWithValue("@dtPositons", ObjESupplier.dtUpdateSupplierPrice);
                    cmd.Parameters.AddWithValue("@ProjectID", ObjESupplier.ProjectID);
                    object ObjReturn = cmd.ExecuteScalar();
                    if(ObjReturn != null)
                    {
                        string str =Convert.ToString(ObjReturn);
                        if (!string.IsNullOrEmpty(str))
                        {
                            if(str.Contains("Zero"))
                            {
                                throw new Exception("Einige LV Positionen haben keinen Preiseintrag");
                            }
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
            return ObjESupplier;
        }

        /// <summary>
        /// Code to save deleted positions from supplier proposal module
        /// </summary>
        /// <param name="ObjEsupplier"></param>
        /// <returns></returns>
        public ESupplier SaveDeletePosition(ESupplier ObjESupplier)
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Ins_DeletePosition]";
                    cmd.Parameters.AddWithValue("@PositionID", ObjESupplier.PositionID);
                    cmd.Parameters.AddWithValue("@ProjectID", ObjESupplier.ProjectID);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
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
            return ObjESupplier;
        }

        /// <summary>
        /// Code to update proposal values to positions from price comparision module
        /// </summary>
        /// <param name="ObjESupplier"></param>
        /// <returns></returns>
        public ESupplier SaveProposaleValues(ESupplier ObjESupplier)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Ins_ProposalValues]";
                    cmd.Parameters.AddWithValue("@PostionID", ObjESupplier.PositionID);
                    cmd.Parameters.AddWithValue("@SupplierProposalID", ObjESupplier.SupplierProposalID);
                    cmd.Parameters.AddWithValue("@SupplierPrice", ObjESupplier.SupplierPrice);
                    cmd.Parameters.AddWithValue("@Multi1", ObjESupplier.Multi1);
                    cmd.Parameters.AddWithValue("@Multi2", ObjESupplier.Multi2);
                    cmd.Parameters.AddWithValue("@Multi3", ObjESupplier.Multi3);
                    cmd.Parameters.AddWithValue("@Multi4", ObjESupplier.Multi4);
                    cmd.Parameters.AddWithValue("@Fabrikate", ObjESupplier.Fabrikate);
                    cmd.Parameters.AddWithValue("@SupplierName", ObjESupplier.SupplierName);
                    cmd.Parameters.AddWithValue("@IsSingle", ObjESupplier.IsSingle);
                    cmd.Parameters.AddWithValue("@dtPostionID", ObjESupplier.PID);
                    cmd.Parameters.AddWithValue("@ProposalSupplierID", ObjESupplier.PSupplierID);
                    object Objreturn = cmd.ExecuteScalar();
                    if (Objreturn != null)
                    {
                        int iValue = 0;
                        if (!int.TryParse(Convert.ToString(Objreturn), out iValue))
                            throw new Exception(Objreturn.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                {
                    throw ex;
                    //throw new Exception("Fehler beim Speichern des Listenpreises");
                }
                else
                {
                    throw ex;
                    //throw new Exception("Error While Saving the ListPrice");
                }
            }
            finally
            {
                SQLCon.Close();
            }
            return ObjESupplier;
        }

        /// <summary>
        /// Code to save Supplier price to proposal positions from supplier proposal module
        /// </summary>
        /// <param name="ObjESupplier"></param>
        /// <returns></returns>
        public ESupplier SaveSupplierPrice(ESupplier ObjESupplier)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Ins_ProposalValues_LP]";
                    cmd.Parameters.AddWithValue("@PostionID", ObjESupplier.PositionID);
                    cmd.Parameters.AddWithValue("@SupplierProposalID", ObjESupplier.SupplierProposalID);
                    cmd.Parameters.AddWithValue("@SupplierPrice", ObjESupplier.SupplierPrice);
                    cmd.Parameters.AddWithValue("@SupplierName", ObjESupplier.SupplierName);
                    cmd.Parameters.AddWithValue("@ProposalSupplierID", ObjESupplier.PSupplierID);
                    object Objreturn = cmd.ExecuteScalar();
                    if (Objreturn != null)
                    {
                        int iValue = 0;
                        if (!int.TryParse(Convert.ToString(Objreturn), out iValue))
                            throw new Exception(Objreturn.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                {
                    throw ex;
                    //throw new Exception("Fehler beim Speichern des Listenpreises");
                }
                else
                {
                    throw ex;
                    //throw new Exception("Error While Saving the ListPrice");
                }
            }
            finally
            {
                SQLCon.Close();
            }
            return ObjESupplier;
        }

        /// <summary>
        /// Code to save Supplier selection to position
        /// </summary>
        /// <param name="ObjESupplier"></param>
        /// <returns></returns>
        public ESupplier SaveSelection(ESupplier ObjESupplier)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Ins_SaveSelection]";
                    cmd.Parameters.AddWithValue("@PositionID", ObjESupplier.PositionID);
                    cmd.Parameters.AddWithValue("@SupplierProposalID", ObjESupplier.SupplierProposalID);
                    cmd.Parameters.AddWithValue("@SelectedSupplier", ObjESupplier.SelectedColumn);
                    cmd.Parameters.AddWithValue("@NotSelectedSupplier", ObjESupplier.UncheckedColumn);
                    cmd.Parameters.AddWithValue("@IsSelected", ObjESupplier.IsSelected);
                    cmd.Parameters.AddWithValue("@NotSelectedPSupplierID", ObjESupplier.NotSelectedPSupplierID);
                    cmd.Parameters.AddWithValue("@SelectedPSupplierID", ObjESupplier. SelectedPSupplierID);
                    cmd.ExecuteNonQuery();
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
            return ObjESupplier;
        }

        /// <summary>
        /// Code to save supplier selection for all positions
        /// </summary>
        /// <param name="ObjESupplier"></param>
        /// <returns></returns>
        public ESupplier SaveBulkSelection(ESupplier ObjESupplier)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Upd_SaveBulkSelection]";
                    cmd.Parameters.AddWithValue("@SupplierProposalID", ObjESupplier.SupplierProposalID);
                    cmd.Parameters.AddWithValue("@ProposalSupplierID", ObjESupplier.PSupplierID);
                    cmd.Parameters.AddWithValue("@IsSelected", ObjESupplier.IsSelected);
                    cmd.ExecuteNonQuery();
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
            return ObjESupplier;
        }

        /// <summary>
        /// Code to fetch Supplier details along with EMails
        /// </summary>
        /// <param name="ObjEsupplier"></param>
        /// <param name="ProposalID"></param>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public ESupplier GetSupplierMail(ESupplier ObjESupplier,int ProposalID,int ProjectID)
        {
            DataSet dsMail = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_SupplierMailID]";
                    cmd.Parameters.AddWithValue("@ProposalID", ProposalID);
                    cmd.Parameters.AddWithValue("@ProjectID", ProjectID);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsMail);
                    }
                    if (dsMail != null && dsMail.Tables.Count > 0)
                    {
                        ObjESupplier.dtSupplierMail = dsMail.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Fehler beim Laden der Daten");
            }
            finally
            {
                SQLCon.Close();
            }
            return ObjESupplier;
        }

        /// <summary>
        /// Code to chcek supplier and article mapping
        /// </summary>
        /// <param name="ObjESupplier"></param>
        /// <returns></returns>
        public ESupplier CheckSupplierArticle(ESupplier ObjESupplier)
        {
            DataSet dsCheckSupplier = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Chk_SupplierArticle]";
                    cmd.Parameters.AddWithValue("@SupplierPorposalID", ObjESupplier.SupplierProposalID);
                    cmd.Parameters.AddWithValue("@SupplierID", ObjESupplier.SupplierID);
                    cmd.Parameters.AddWithValue("@WG", ObjESupplier.WG);
                    cmd.Parameters.AddWithValue("@WA", ObjESupplier.WA);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsCheckSupplier);
                    }
                    if(dsCheckSupplier != null)
                    {
                        if (dsCheckSupplier.Tables.Count > 0 && dsCheckSupplier.Tables[0].Rows.Count > 0)
                        {
                            ObjESupplier.strArticleExists = Convert.ToString(dsCheckSupplier.Tables[0].Rows[0][0]);
                            if (dsCheckSupplier.Tables.Count > 1 && dsCheckSupplier.Tables[1].Rows.Count > 0)
                            {
                                ObjESupplier.strSupplierExists = Convert.ToString(dsCheckSupplier.Tables[1].Rows[0][0]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Fehler beim Laden der Daten");
            }
            finally
            {
                SQLCon.Close();
            }
            return ObjESupplier;
        }

        /// <summary>
        /// Code to add new supplier to existing proposal
        /// </summary>
        /// <param name="ObjESupplier"></param>
        /// <returns></returns>
        public ESupplier UpdateSupplierProposal(ESupplier ObjESupplier)
        {
            DataSet dsWGWA = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Upd_SupplierProposal]";
                    cmd.Parameters.AddWithValue("@SupplierProposalID", ObjESupplier.SupplierProposalID);
                    cmd.Parameters.AddWithValue("@SupplierID", ObjESupplier.SupplierID);
                    cmd.Parameters.AddWithValue("@ProjectID", ObjESupplier.ProjectID);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsWGWA);
                    }
                    if (dsWGWA != null && dsWGWA.Tables.Count > 0)
                    {
                        int IValue = 0;
                        if (int.TryParse(Convert.ToString(dsWGWA.Tables[0].Rows[0][0]), out IValue))
                            ObjESupplier.dsProposal = dsWGWA;
                        else
                            throw new Exception(Convert.ToString(dsWGWA.Tables[0].Rows[0][0]));
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Cannot"))
                {
                    if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                        throw new Exception("Die Preisanfrage ist auf 8 Lieferanten begrenzt");
                    else
                        throw new Exception("Cannot send proposal to more than 8 suppliers");
                }
                else
                {
                    if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                        throw new Exception("Fehler beim Laden der Daten");
                    else
                        throw new Exception("Error Occured While Retreiving records");
                }
            }
            finally
            {
                SQLCon.Close();
            }
            return ObjESupplier;
        }

        /// <summary>
        /// Code to Delete Supplier and article mapping
        /// </summary>
        /// <param name="ObjESupplier"></param>
        /// <returns></returns>
        public ESupplier DeleteSupplierArticleMap(ESupplier ObjESupplier)
        {
            DataSet dsArticle = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Del_ArticleSupplierMap]";
                    cmd.Parameters.AddWithValue("@WGWAID", ObjESupplier.WGWAID);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsArticle);
                    }
                    if(dsArticle != null && dsArticle.Tables.Count > 0)
                    {
                        ObjESupplier.dtArticle = dsArticle.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                    throw new Exception("Fehler beim Löschen des Artikels");
                else
                    throw new Exception("Error while Deleting Article");
            }
            return ObjESupplier;
        }

        /// <summary>
        /// Code to Update proposal date to supplier proposal after sending to Supplier
        /// </summary>
        /// <param name="ObjESupplier"></param>
        /// <returns></returns>
        public ESupplier UpdateProposalDate(ESupplier ObjESupplier)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Upd_SupplierproposalDate]";
                    cmd.Parameters.AddWithValue("@SupplierProposalID", ObjESupplier.SupplierProposalID);
                    cmd.ExecuteScalar();
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
            return ObjESupplier;
        }

        /// <summary>
        /// Code to Delete positions from existsing proposal
        /// </summary>
        /// <param name="SupplierProposalID"></param>
        /// <param name="PositionID"></param>
        public void DeleteProposalPositions(int SupplierProposalID, int PositionID)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Del_ProposalPosition]";
                    cmd.Parameters.AddWithValue("@SupplierProposalID", SupplierProposalID);
                    cmd.Parameters.AddWithValue("@PositionID", PositionID);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                    throw new Exception("Fehler beim Löschen des Position");
                else
                    throw new Exception("Error while Deleting Position");
            }
        }

        /// <summary>
        /// Code to delete suppliers from supplier proposal
        /// </summary>
        /// <param name="ObjESupplier"></param>
        /// <returns></returns>
        public ESupplier DeleteSuipplierProposal(ESupplier ObjESupplier)
        {
            DataSet dsArticle = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Del_SupplierProposal]";
                    cmd.Parameters.AddWithValue("@SupplierProposalID", ObjESupplier.SupplierProposalID);
                    cmd.Parameters.AddWithValue("@ProposalName", ObjESupplier.ProposalName);
                    cmd.Parameters.AddWithValue("@ProposalSupplierID", ObjESupplier.ProposalSupplierID);
                    object ObjReturn = cmd.ExecuteScalar();
                    int ivalue = 0;
                    if (!int.TryParse(Convert.ToString(ObjReturn), out ivalue))
                        throw new Exception(Convert.ToString(ObjReturn));
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                    throw new Exception("Fehler beim Löschen des Supplier Proposal");
                else
                    throw new Exception("Error while Deleting Supplier Proposal");
            }
            return ObjESupplier;
        }

        /// <summary>
        /// Code to fetch suppliers based on articles while adding new supplier to existing proposal
        /// </summary>
        /// <param name="ObjESupplier"></param>
        /// <returns></returns>
        public ESupplier GetSuppliersForProposal(ESupplier ObjESupplier)
        {
            try
            {
                DataSet dsSupplier = new DataSet();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_SuppliersForProposal]";
                    cmd.Parameters.Add("@ProjectID", ObjESupplier.ProjectID);
                    cmd.Parameters.Add("@SupplierProposalID", ObjESupplier.SupplierProposalID);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsSupplier);
                    }
                    if(dsSupplier != null && dsSupplier.Tables.Count > 0)
                    {
                        ObjESupplier.dtSupplierForproposal = dsSupplier.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                    throw new Exception("Fehler beim Laden des Kunden");
                else
                    throw new Exception("Error Occured While Retreiving Suppliers");
            }
            finally
            {
                SQLCon.Close();
            }
            return ObjESupplier;
        }

        /// <summary>
        /// Code to fetch suppliers while merging new article with existing proposal
        /// </summary>
        /// <param name="ObjESupplier"></param>
        /// <returns></returns>
        public ESupplier GetSuppliersForProposalMerge(ESupplier ObjESupplier)
        {
            try
            {
                DataSet dsSupplier = new DataSet();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_SuppliersForProposalMerge]";
                    cmd.Parameters.Add("@ProjectID", ObjESupplier.ProjectID);
                    cmd.Parameters.Add("@dtArticles", ObjESupplier.dtArticlesMerge);
                    cmd.Parameters.Add("@SupplierProposalID", ObjESupplier.SupplierProposalID);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsSupplier);
                    }
                    if (dsSupplier != null && dsSupplier.Tables.Count > 0)
                    {
                        ObjESupplier.dtSupplierForproposal = dsSupplier.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                    throw new Exception("Fehler beim Laden des Kunden");
                else
                    throw new Exception("Error Occured While Retreiving Suppliers");
            }
            finally
            {
                SQLCon.Close();
            }
            return ObjESupplier;
        }

        /// <summary>
        /// Code to save new article with existsing proposal
        /// </summary>
        /// <param name="ObjESupplier"></param>
        /// <returns></returns>
        public ESupplier SupplierProposalMerge(ESupplier ObjESupplier)
        {
            DataSet ds = new DataSet();
            int ProposalID = -1;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Ins_ProposalMerge]";
                    cmd.Parameters.AddWithValue("@SupplierProposalID", ObjESupplier.SupplierProposalID);
                    cmd.Parameters.AddWithValue("@ProjectID", ObjESupplier.ProjectID);
                    cmd.Parameters.AddWithValue("@LVSection", ObjESupplier.LVSection);
                    cmd.Parameters.AddWithValue("@dtArticleID", ObjESupplier.dtArticlesMerge);
                    cmd.Parameters.AddWithValue("@dtSupplierID", ObjESupplier.dtSupplierToDB);
                    cmd.Parameters.AddWithValue("@LVSectionID", ObjESupplier.LVSectionID);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
                    }
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        string str = ds.Tables[0].Rows[0][0] == DBNull.Value ? "" : ds.Tables[0].Rows[0][0].ToString();
                        if (!string.IsNullOrEmpty(str))
                        {
                            if (int.TryParse(str, out ProposalID))
                                ObjESupplier.SupplierProposalID = ProposalID;
                            else
                                throw new Exception(str);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Cannot"))
                {
                    if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                        throw new Exception("Die Preisanfrage ist auf 8 Lieferanten begrenzt");
                    else
                        throw new Exception("Cannot send proposal to more than 8 suppliers");
                }
                else
                {
                    if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                        throw new Exception("Fehler beim Laden der Daten");
                    else
                        throw new Exception("Error occured while saving suplier proposal");
                }
            }
            finally
            {
                SQLCon.Close();
            }
            return ObjESupplier;
        }

        /// <summary>
        /// Code to get import Datanorm file to database
        /// </summary>
        /// <param name="SupplierID"></param>
        /// <param name="ValidityDate"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataTable ImportDataNorm(object SupplierID, object ValidityDate, DataTable dt)
        {
            DataTable dtResult = new DataTable();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Imp_DataNorm]";
                    cmd.Parameters.Add("@SupplierID", SupplierID);
                    cmd.Parameters.Add("@ValidityDate", ValidityDate);
                    cmd.Parameters.Add("@dtDataNorm", dt);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResult);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //throw new Exception("Error Occured While importing Datanorm File");
            }
            return dtResult;
        }

        /// <summary>
        /// Code to validate datanorm before importing into database
        /// </summary>
        /// <param name="SupplierID"></param>
        /// <param name="ValidityDate"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataSet ValidateDataNorm(object SupplierID, object ValidityDate, DataTable dt)
        {
            DataSet dsResult = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_ValidateDataNorm]";
                    cmd.Parameters.Add("@ValidityDate", ValidityDate);
                    cmd.Parameters.Add("@dtDataNorm", dt);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsResult);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsResult;
        }
    }
}
