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
    public class DOTTO
    {
        /// <summary>
        /// Code to get save Organization details
        /// </summary>
        /// <param name="ObjEOTTO"></param>
        /// <returns></returns>
        public EOTTO SaveOTTODetails(XmlDocument XmlDoc, EOTTO ObjEOTTO)
        {
            int OTTOID = -1;
            DataSet dsOTTO = new DataSet();
            try
            {
                string innerxml = XmlDoc.InnerXml;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Ins_OTTODetails]";
                    SqlParameter param = new SqlParameter("@XMLOTTO", SqlDbType.Xml);
                    param.Value = innerxml;
                    cmd.Parameters.Add(param);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsOTTO);
                    }
                    if (dsOTTO != null && dsOTTO.Tables.Count > 0)
                    {
                        string str = Convert.ToString(dsOTTO.Tables[0].Rows[0][0]);
                        if (int.TryParse(str, out OTTOID))
                        {
                            ObjEOTTO.OTTOID = OTTOID;
                            if (dsOTTO.Tables.Count > 1)
                                ObjEOTTO.dtOTTO = dsOTTO.Tables[1];
                        }
                        else
                        {
                            if (str.Contains("duplicate"))
                            {
                                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                                    throw new Exception("Dieser Kurzname ist bereits vergeben");
                                else
                                    throw new Exception("ShortName is already exists.!");
                            }
                            else
                                throw new Exception("Fehler beim Speichern von Daten zu OTTO");
                        }
                    }
                }
            }
            catch (Exception ex){throw;}
            finally{SQLCon.Close();}
            return ObjEOTTO;
        }

        /// <summary>
        /// Code to get Organization details
        /// </summary>
        /// <param name="ObjEOTTO"></param>
        /// <returns></returns>
        public EOTTO GetOTTODetails(EOTTO ObjEOTTO)
        {
            DataSet dsOTTO = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_OTTODetails]";
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsOTTO);
                    }
                    if (dsOTTO != null && dsOTTO.Tables.Count > 0)
                    {
                        ObjEOTTO.dtOTTO = dsOTTO.Tables[0];
                        if (dsOTTO.Tables.Count > 1)
                            ObjEOTTO.dtContact = dsOTTO.Tables[1];
                    }
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                    throw new Exception("Fehler beim Laden der daten zu OTTO");
                else
                    throw new Exception("Error Occured While Retreiving OTTO details");
            }
            finally
            {
                SQLCon.Close();
            }
            return ObjEOTTO;
        }

        /// <summary>
        /// Code to save Organzition contacts
        /// </summary>
        /// <param name="ObjEOTTO"></param>
        /// <returns></returns>
        public EOTTO SaveOTTOContactDetails(XmlDocument XmlDoc, EOTTO ObjEOTTO)
        {
            int ContactID = -1;
            DataSet dsOTTO = new DataSet();
            try
            {

                string innerxml = XmlDoc.InnerXml;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Ins_OTTO_Contact]";
                    SqlParameter param = new SqlParameter("@XMLOTTOContact", SqlDbType.Xml);
                    param.Value = innerxml;
                    cmd.Parameters.Add(param);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsOTTO);
                    }
                    if (dsOTTO != null && dsOTTO.Tables.Count > 0)
                    {
                        if (int.TryParse(Convert.ToString(dsOTTO.Tables[0].Rows[0][0]), out ContactID))
                        {
                            ObjEOTTO.ContactID = ContactID;
                            if (dsOTTO.Tables.Count > 1)
                                ObjEOTTO.dtContact = dsOTTO.Tables[1];
                        }
                    }
                }
            }
            catch (Exception ex){throw;}
            finally{SQLCon.Close();}
            return ObjEOTTO;
        }

        /// <summary>
        /// Code to import customer data
        /// Not using now but we can use in future
        /// </summary>
        /// <param name="dt"></param>
        public void ImportCustomerData(DataTable dt)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[p_upd_customerdata]";
                    cmd.Parameters.AddWithValue("@dtCusotmerData", dt);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
