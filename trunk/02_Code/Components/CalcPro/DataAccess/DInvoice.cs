using DAL;
using EL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class DInvoice
    {
        /// <summary>
        /// Code to get invoice next blatt numbers
        /// </summary>
        /// <param name="ObjEInvoice"></param>
        /// <returns></returns>
        public EInvoice GeTBlattNumbers(EInvoice ObjEInvoice)
        {
            try
            {
                DataSet dsInvoices = new DataSet();
                try
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = SQLCon.Sqlconn();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "[P_Get_BlattNumbers]";
                        cmd.Parameters.AddWithValue("@ProjectID", ObjEInvoice.ProjectID);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dsInvoices);
                        }
                    }
                    if (dsInvoices != null)
                    {
                        if(dsInvoices.Tables.Count > 0)
                            ObjEInvoice.dtBlattNumbers = dsInvoices.Tables[0];
                    }
                }
                catch (Exception ex)
                {
                    if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                        throw new Exception("Fehler beim Datenabruf zu den Aufmassen");
                    else
                        throw new Exception("Error While Retrieving the Delivery Notes");
                }
                finally
                {
                    SQLCon.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEInvoice;
        }

        /// <summary>
        /// Code to save invoice 
        /// </summary>
        /// <param name="ObjEInvoice"></param>
        /// <returns></returns>
        public EInvoice SaveInvoice(EInvoice ObjEInvoice)
        {
            DataSet dsInvoices = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Ins_Invoice]";
                    cmd.Parameters.AddWithValue("@InvoiceID", ObjEInvoice.InvoiceID);
                    cmd.Parameters.AddWithValue("@ProjectID", ObjEInvoice.ProjectID);
                    cmd.Parameters.AddWithValue("@InvoiceNumber", ObjEInvoice.InvoiceNumber);
                    cmd.Parameters.AddWithValue("@InvoiceDescription", ObjEInvoice.InvoiceDescription);
                    cmd.Parameters.AddWithValue("@InvoiceType", ObjEInvoice.InvoiceType);
                    cmd.Parameters.AddWithValue("@dtInvoice", ObjEInvoice.dtInvoice);
                    cmd.Parameters.AddWithValue("@IsFinalInvoice", ObjEInvoice.IsFinalInvoice);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsInvoices);
                    }
                }
                if (dsInvoices != null && dsInvoices.Tables.Count > 0)
                {
                    if (dsInvoices.Tables[0].Rows.Count > 0)
                    {
                        int iValue =  0;
                        string strError = dsInvoices.Tables[0].Rows[0][0].ToString();
                        if (!int.TryParse(strError, out iValue))
                        {
                            throw new Exception(strError);
                        }
                        else
                        {
                            ObjEInvoice.InvoiceID = iValue;
                            ObjEInvoice.dtBlattNumbers = dsInvoices.Tables[1];
                            if (dsInvoices.Tables.Count > 1)
                                ObjEInvoice.dtInvoices = dsInvoices.Tables[2];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Atleast"))
                    throw new Exception("Bitte wählen Sie mindestens ein Aufmass");
                else if(ex.Message.Contains("UNIQUE"))
                {
                    throw new Exception("Diese Rechnungsnummer wurde für dieses Projekt bereits vergeben");
                }
                else
                    throw new Exception("Error While Saving the Invoice");
            }
            finally
            {
                SQLCon.Close();
            }
            return ObjEInvoice;
        }

        /// <summary>
        /// Code to get list of invoices
        /// </summary>
        /// <param name="ObjEInvoice"></param>
        /// <returns></returns>
        public EInvoice GetInvoices(EInvoice ObjEInvoice)
        {
            try
            {
                DataSet dsInvoices = new DataSet();
                try
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = SQLCon.Sqlconn();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "[p_Get_Invoices]";
                        cmd.Parameters.AddWithValue("@ProjectID", ObjEInvoice.ProjectID);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dsInvoices);
                        }
                    }
                    if (dsInvoices != null)
                    {
                        if (dsInvoices.Tables.Count > 0)
                            ObjEInvoice.dtInvoices = dsInvoices.Tables[0];
                    }
                }
                catch (Exception ex)
                {
                    if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                        throw new Exception("Fehler bei der Aufbereitung des Aufmasses");
                    else
                        throw new Exception("Error While Retrieving the Delivery Notes");
                }
                finally
                {
                    SQLCon.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEInvoice;
        }
    }
}
