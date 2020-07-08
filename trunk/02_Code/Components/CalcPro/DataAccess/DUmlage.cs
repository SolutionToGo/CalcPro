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
    public class DUmlage
    {
        /// <summary>
        /// Code to distribute special cost to all positions based on business logiv
        /// </summary>
        /// <param name="ObjEUmlage"></param>
        /// <returns></returns>
        public EUmlage UpdateSpecialCost(EUmlage ObjEUmlage)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Upd_SpecialCost]";
                    cmd.Parameters.Add("@ProjectID", ObjEUmlage.ProjectID);
                    cmd.Parameters.Add("@dt", ObjEUmlage.dtSpecialCost);
                    cmd.Parameters.Add("@UmlageMode", ObjEUmlage.UmlageMode);
                    Object Objreturn = cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                {
                    throw new Exception("Fehler bei der Verteilung der Generellen Kosten");
                }
                else
                {
                    throw new Exception("Error while distributing special cost");
                }
            }
            finally
            {
                SQLCon.Close();
            }
            return ObjEUmlage;
        }

        /// <summary>
        /// Code to fetch special cost of a project
        /// </summary>
        /// <param name="ObjEUmlage"></param>
        /// <returns></returns>
        public EUmlage GetSpecialCost(EUmlage ObjEUmlage)
        {
            try
            {
                ObjEUmlage.dtSpecialCost = new DataTable();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_SpecialCost]";
                    cmd.Parameters.Add("@ProjectID", ObjEUmlage.ProjectID);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
                    }
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        ObjEUmlage.dtSpecialCost = ds.Tables[0];
                        if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                        {
                            int.TryParse(Convert.ToString(ds.Tables[1].Rows[0][0]), out ObjEUmlage.UmlageMode);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                {
                    throw new Exception("Fehler beim Laden der Generellen Kosten");
                }
                else
                {
                    throw new Exception("Error while retrieving special cost");
                }
            }
            finally
            {
                SQLCon.Close();
            }
            return ObjEUmlage;
        }

        /// <summary>
        /// Code to calculate estimated special cost
        /// </summary>
        /// <param name="ObjEUmlage"></param>
        /// <returns></returns>
        public EUmlage ShowUmlage(EUmlage ObjEUmlage)
        {
            try
            {
                ObjEUmlage.dtUmlage = new DataTable();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_ShowUmlage]";
                    cmd.Parameters.Add("@ProjectId", ObjEUmlage.ProjectID);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ObjEUmlage.dtUmlage);
                    }
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                {
                    throw new Exception("Fehler beim Laden der Umlage");
                }
                else
                {
                    throw new Exception("Error while retrieving umlage");
                }
            }
            finally
            {
                SQLCon.Close();
            }
            return ObjEUmlage;
        }

        /// <summary>
        /// Code to save special cost in database
        /// </summary>
        /// <param name="ObjEUmlage"></param>
        /// <returns></returns>
        public EUmlage SaveSpecialCost(EUmlage ObjEUmlage)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Ins_SpecialCost]";
                    cmd.Parameters.Add("@ProjectID", ObjEUmlage.ProjectID);
                    cmd.Parameters.Add("@dt", ObjEUmlage.dtSpecialCost);
                    cmd.Parameters.Add("@UmlageMode", ObjEUmlage.UmlageMode);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        da.Fill(dt);

                    if(dt != null && dt.Rows.Count > 0)
                    {
                        decimal DValue = 0;
                        if (decimal.TryParse(Convert.ToString(dt.Rows[0]["UmlageFactor"]), out DValue))
                            ObjEUmlage.UmlageFactor = DValue;
                        if (decimal.TryParse(Convert.ToString(dt.Rows[0]["UmlageValue"]), out DValue))
                            ObjEUmlage.UmlageValue = DValue;
                    }
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                    throw new Exception("Fehler beim Speichern der Generalkosten");
                else
                    throw new Exception("Error while saving special cost");
            }
            finally
            {
                SQLCon.Close();
            }
            return ObjEUmlage;
        }

    }
}
