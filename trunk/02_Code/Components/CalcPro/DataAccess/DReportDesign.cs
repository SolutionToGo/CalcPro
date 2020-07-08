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
    public class DReportDesign
    {
        /// <summary>
        /// Code to Save report settings
        /// </summary>
        /// <param name="ObjEObject"></param>
        public DataSet SaveReportSetting(EReportDesign ObjEReportDesign)
        {
            DataSet dsReportSetting = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Ins_ReportSetting]";
                    cmd.Parameters.AddWithValue("@LVPosition", ObjEReportDesign.LVPosition);
                    cmd.Parameters.AddWithValue("@Fabrikat", ObjEReportDesign.Fabrikat);
                    cmd.Parameters.AddWithValue("@ArticleNr", ObjEReportDesign.ArticlNr);
                    cmd.Parameters.AddWithValue("@LieferantMA", ObjEReportDesign.Lieferant);
                    cmd.Parameters.AddWithValue("@LangText", ObjEReportDesign.LangText);
                    cmd.Parameters.AddWithValue("@KurzText", ObjEReportDesign.KurzText);
                    cmd.Parameters.AddWithValue("@Sender", ObjEReportDesign.Sender);
                    cmd.Parameters.AddWithValue("@Menge", ObjEReportDesign.Menge);
                    cmd.Parameters.AddWithValue("@GB", ObjEReportDesign.GB);
                    cmd.Parameters.AddWithValue("@EP", ObjEReportDesign.EP);
                    cmd.Parameters.AddWithValue("@Prices", ObjEReportDesign.Prices);
                    cmd.Parameters.AddWithValue("@MAMO", ObjEReportDesign.MAMO);
                    cmd.Parameters.AddWithValue("@KurzAndLangText", ObjEReportDesign.KurzAnLangText);
                    cmd.Parameters.AddWithValue("@HPos", ObjEReportDesign.HPos);
                    cmd.Parameters.AddWithValue("@ABPos", ObjEReportDesign.ABPos);
                    cmd.Parameters.AddWithValue("@BAPos", ObjEReportDesign.BAPos);
                    cmd.Parameters.AddWithValue("@VRPos", ObjEReportDesign.VRPos);
                    cmd.Parameters.AddWithValue("@UBPos", ObjEReportDesign.UBPos);
                    cmd.Parameters.AddWithValue("@NonePos", ObjEReportDesign.NonePos);
                    cmd.Parameters.AddWithValue("@WithDetKZ", ObjEReportDesign.WithDetailKZ);
                    cmd.Parameters.AddWithValue("@WithTitlePrices", ObjEReportDesign.WithTitlePrices);
                    cmd.Parameters.AddWithValue("@OnheMengeZeroPositions", ObjEReportDesign.OnheMengeZeroPositions);
                    cmd.Parameters.AddWithValue("@OnheMontagePrice", ObjEReportDesign.OnheMontagePrice);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsReportSetting);
                    }
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                {
                    throw new Exception("Fehler beim Speichern der Daten");
                }
                else
                {
                    throw new Exception("Error while saving the data");

                }
            }
            finally
            {
                SQLCon.Close();
            }
            return dsReportSetting;
        }

        /// <summary>
        /// Code to fetch report settings from database
        /// </summary>
        /// <param name="ObjEObject"></param>
        /// <returns></returns>
        public DataSet GetReportSettings(EReportDesign ObjEReportDesign)
        {
            DataSet dsReportSetting = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_ReportSetting]";

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsReportSetting);
                    }
                    if (dsReportSetting != null && dsReportSetting.Tables.Count > 0)
                    {
                        ObjEReportDesign.dtReportSettings = dsReportSetting.Tables[0];
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
                    throw new Exception("Error Occured While Retreiving the data");

                }
            }
            finally
            {
                SQLCon.Close();
            }
            return dsReportSetting;
        }

        /// <summary>
        /// Code to fetch PRoject, Organization and customer details at once
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public DataTable GetProjectCustomerDetails(int ProjectID)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Rpt_ProjectAndCustomerAndOTTODetails]";
                    cmd.Parameters.AddWithValue("@ProjectID", ProjectID);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        da.Fill(dt);
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
            return dt;
        }
    }
}
