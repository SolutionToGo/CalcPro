using EL;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DProject
    {
        /// <summary>
        /// Log4net object for logging in data layer
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Business Logic for Saving the project details
        /// </summary>
        /// <param name="ObjEObject"></param>
        /// <returns></returns>
        public int SaveProjectDetails(EProject ObjEProject)
        {
            int ProjectID = -1;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "P_Ins_Project";
                    cmd.Parameters.AddWithValue("@ProjectID", ObjEProject.ProjectID);
                    cmd.Parameters.AddWithValue("@Location", ObjEProject.Location);
                    cmd.Parameters.AddWithValue("@ProjectNumber", ObjEProject.ProjectNumber);
                    cmd.Parameters.AddWithValue("@ComissionNumber", ObjEProject.CommissionNumber);
                    cmd.Parameters.AddWithValue("@CustomerID", ObjEProject.KundeNr);
                    cmd.Parameters.AddWithValue("@ProjectDescription", ObjEProject.ProjectDescription);
                    cmd.Parameters.AddWithValue("@PlannerID", ObjEProject.PlannedID);
                    cmd.Parameters.AddWithValue("@LV_Raster", ObjEProject.LVRaster);
                    cmd.Parameters.AddWithValue("@LV_Raster_GAEB", ObjEProject.LVRaster_GAEB);
                    cmd.Parameters.AddWithValue("@LV_Sprung", ObjEProject.LVSprunge);
                    cmd.Parameters.AddWithValue("@Intern_X", ObjEProject.InternX);
                    cmd.Parameters.AddWithValue("@Intern_S", ObjEProject.InternS);
                    cmd.Parameters.AddWithValue("@Vat", ObjEProject.Vat);
                    cmd.Parameters.AddWithValue("@Submit_Location", ObjEProject.Submitlocation);
                    cmd.Parameters.AddWithValue("@Submit_Date", ObjEProject.SubmitDate);
                    cmd.Parameters.AddWithValue("@Submit_Time", ObjEProject.SubmitTime);
                    cmd.Parameters.AddWithValue("@Estimated_LV", ObjEProject.EstimatedLvs);
                    cmd.Parameters.AddWithValue("@Round_Off", ObjEProject.RoundingPrice);
                    cmd.Parameters.AddWithValue("@Remarks", ObjEProject.Remarks);
                    cmd.Parameters.AddWithValue("@Project_Discount", ObjEProject.Discount);
                    cmd.Parameters.AddWithValue("@Lock_LVHierarchy", ObjEProject.LockHierarchy);
                    cmd.Parameters.AddWithValue("@UserID", ObjEProject.UserID);
                    cmd.Parameters.AddWithValue("@CustomerName", ObjEProject.KundeName);
                    cmd.Parameters.AddWithValue("@PlannerName", ObjEProject.PlannerName);
                    cmd.Parameters.AddWithValue("@ProjectStartDate", ObjEProject.ProjectStartDate);
                    cmd.Parameters.AddWithValue("@ProjectEndDate", ObjEProject.ProjectEndDate);
                    cmd.Parameters.AddWithValue("@IsCummulated", ObjEProject.IsCumulated);
                    cmd.Parameters.AddWithValue("@ISRasterChange", ObjEProject.IsRasterChange);
                    cmd.Parameters.AddWithValue("@ProjectName", ObjEProject.ProjectName);
                    cmd.Parameters.AddWithValue("@ShowVK", ObjEProject.ShowVK);
                    cmd.Parameters.AddWithValue("@dtQuerCalc", ObjEProject.dtQuerCalc);
                    object returnObj = cmd.ExecuteScalar();
                    if (returnObj != null)
                    {
                        if (returnObj.ToString().Contains("ProjectNumber"))
                        {
                            if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                                throw new Exception("Diese Projektnummer existiert bereits");
                            else
                                throw new Exception("Project Nr Already Exists");
                        }
                        else if (returnObj.ToString().Contains("ComissionNumber"))
                        {
                            if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                                throw new Exception("Die eingegebene Kommissions-Nummer existiert bereits");
                            else
                                throw new Exception("ComissionNumber Already Exists");
                        }
                        else if (returnObj.ToString().Contains("RasterError"))
                        {
                            if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                                throw new Exception("Diese Funktion ist nicht möglich mit dem alten Raster");
                            else
                                throw new Exception("Operation not Possible with Old Raster");
                        }
                        else if (!int.TryParse(returnObj.ToString(), out ProjectID))
                        {
                            if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                                throw new Exception("Fehler beim Speichern der Projektangaben");
                            else
                                throw new Exception("Error Occured While Saving the Project Details");
                        }
                    }
                    ObjEProject.IsRasterChange = false;
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
            return ProjectID;
        }

        /// <summary>
        /// /// Business Logic for retreiving the project list to show on load project screen
        /// </summary>
        /// <param name="ObjEObject"></param>
        public DataTable GetProjectList()
        {
            DataTable dtProjectList = new DataTable();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "P_Get_ProjectList";
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtProjectList);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { SQLCon.Close(); }
            return dtProjectList;
        }

        /// <summary>
        /// /// Business Logic for retreiving the existing project details to show on project screen
        /// </summary>
        /// <param name="ObjEProject"></param>
        public DataSet GetProjectDetails(EProject ObjEProject)
        {
            DataSet dsProjectList = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProjectID", ObjEProject.ProjectID);
                    cmd.CommandText = "[P_Get_ProjectDetails]";
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsProjectList, "ProjectDetails");
                    }
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                {
                    throw new Exception("Fehler beim Laden der generiert werden");
                }
                else
                {
                    throw new Exception("Error Occured While Retreiving ProjectList");

                }
            }
            finally{SQLCon.Close();}
            return dsProjectList;
        }

        /// <summary>
        /// Code to fetch list of project numbers for Copy LV Positions from another project module
        /// </summary>
        /// <param name="ObjEProject"></param>
        /// <returns></returns>
        public EProject GetProjectNumber(EProject ObjEProject)
        {
            DataSet dsProjectNo = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_ProjectNumber]";
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsProjectNo);
                    }
                    if (dsProjectNo != null && dsProjectNo.Tables.Count > 0)
                    {
                        ObjEProject.dtProjecNumber = dsProjectNo.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                {
                    throw new Exception("Fehler beim Laden der generiert werden");
                }
                else
                {
                    throw new Exception("Error Occured While Retreiving Projects");

                }
            }
            finally
            {
                SQLCon.Close();
            }
            return ObjEProject;
        }

        /// <summary>
        /// Code to update the status of project
        /// </summary>
        /// <param name="ObjEProject"></param>
        /// <returns></returns>
        public EProject UpdateStatus(EProject ObjEProject)
        {
            DataSet dsProjectNo = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Upd_ProjectStatus]";
                    cmd.Parameters.AddWithValue("@ProjectID", ObjEProject.ProjectID);
                    cmd.Parameters.AddWithValue("@Status", ObjEProject.Status);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsProjectNo);
                    }                    
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                {
                    throw new Exception("Fehler beim Speichern des Projektstatus");
                }
                else
                {
                    throw new Exception("Error Occured While Saving the Project status");

                }
            }
            finally
            {
                SQLCon.Close();
            }
            return ObjEProject;
        }

        /// <summary>
        ///  Code to compare article prices with in project and acrossthe projects
        /// </summary>
        /// <param name="ObjEProject"></param>
        /// <param name="_wg"></param>
        /// <param name="_wa"></param>
        /// <param name="_wi"></param>
        /// <param name="_a"></param>
        /// <param name="_b"></param>
        /// <param name="_l"></param>
        /// <param name="_typ"></param>
        /// <param name="_Type"></param>
        /// <param name="_PosID"></param>
        /// <returns></returns>
        public EProject GetComparePrice(EProject ObjEProject, string _wg, string _wa, string _wi, string _a, string _b, string _l, string _typ, string _Type, int _PosID)
        {
            DataSet dsComnparePrice = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_ComparePrice]";
                    cmd.Parameters.AddWithValue("@ProjectID", ObjEProject.ProjectID);
                    cmd.Parameters.AddWithValue("@CustomerID", ObjEProject.KundeNr);
                    cmd.Parameters.AddWithValue("@WG", _wg);
                    cmd.Parameters.AddWithValue("@WA", _wa);
                    cmd.Parameters.AddWithValue("@WI", _wi);
                    cmd.Parameters.AddWithValue("@A", _a);
                    cmd.Parameters.AddWithValue("@B", _b);
                    cmd.Parameters.AddWithValue("@L", _l);
                    cmd.Parameters.AddWithValue("@TYP", _typ);
                    cmd.Parameters.AddWithValue("@TYPE", _Type);
                    cmd.Parameters.AddWithValue("@PositionID", _PosID);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsComnparePrice);
                    }
                    if (dsComnparePrice.Tables.Count > 0)
                    {
                        ObjEProject.dsComaparePrice = dsComnparePrice;
                    }
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                {
                    throw new Exception("Fehler beim Laden der generiert werden");
                }
                else
                {
                    throw new Exception("Error Occured While Retreiving Projects");

                }
            }
            finally
            {
                SQLCon.Close();
            }
            return ObjEProject;
        }

        /// <summary>
        /// Code to save Discount positions
        /// </summary>
        /// <param name="ObjEProject"></param>
        /// <returns></returns>
        public EProject SaveDiscount(EProject ObjEProject)
        {
            DataSet dsDiscount = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Ins_Discount]";
                    cmd.Parameters.AddWithValue("@ProjectID", ObjEProject.ProjectID);
                    cmd.Parameters.AddWithValue("@FromOZ", ObjEProject.FromOZ);
                    cmd.Parameters.AddWithValue("@ToOZ", ObjEProject.ToOZ);
                    cmd.Parameters.AddWithValue("@Discount", ObjEProject.Discount);
                    cmd.Parameters.AddWithValue("@UserID", ObjEProject.UserID);
                    cmd.Parameters.AddWithValue("@DiscountPosID", ObjEProject.DiscountPosID);
                    cmd.Parameters.AddWithValue("@dtDiscountList", ObjEProject.dtDiscountList);
                    cmd.Parameters.AddWithValue("@ShortDescription", ObjEProject.ShortDescription);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsDiscount);
                    }
                    if(dsDiscount != null && dsDiscount.Tables.Count > 0)
                    {
                        int IValue = 0;
                        string str = Convert.ToString(dsDiscount.Tables[0].Rows[0][0]);
                        if(int.TryParse(str,out IValue))
                        {
                            ObjEProject.DiscountID = IValue;
                            if (dsDiscount.Tables.Count > 1)
                                ObjEProject.dtDiscount = dsDiscount.Tables[1];
                        }
                        else
                            throw new Exception(str);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("valid"))
                    throw new Exception("Bitte geben Sie einen gültigen Titel oder Untertitel ein");
                else if (ex.Message.Contains("already"))
                    throw new Exception("Für einige ausgewählte Titel/Untertitel wurde bereits ein Nachlass zugewiesen.");
                else
                {
                    if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                        throw new Exception("Fehler beim Speichern des Nachlasses");
                    else
                        throw new Exception("Error Occured While Saving Discount");
                }
            }
            finally
            {
                SQLCon.Close();
            }
            return ObjEProject;
        }

        /// <summary>
        /// Code to delete discount positions
        /// </summary>
        /// <param name="ObjEProject"></param>
        /// <returns></returns> 
        public EProject DeleteDiscount(EProject ObjEProject)
        {
            DataSet dsDiscount = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Del_Discunt]";
                    cmd.Parameters.AddWithValue("@DiscountID", ObjEProject.DiscountID);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsDiscount);
                    }
                    if (dsDiscount != null && dsDiscount.Tables.Count > 0)
                        ObjEProject.dtDiscount = dsDiscount.Tables[0];
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                    throw new Exception("Fehler beim Löschen des Nachlasses");
                else
                    throw new Exception("Error Occured While Deleting Discount");
            }
            finally
            {
                SQLCon.Close();
            }
            return ObjEProject;
        }

        /// <summary>
        /// Code to save the Cover sheets and template path
        /// </summary>
        /// <param name="strPath"></param>
        /// <param name="TemplatePath"></param>
        public void SavePath(string strPath,string TemplatePath)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Ins_Path]";
                    cmd.Parameters.AddWithValue("@Path", strPath);
                    cmd.Parameters.AddWithValue("@TempaltePath", TemplatePath);
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
        }

        /// <summary>
        /// Code to get Cover sheets and template paths
        /// </summary>
        /// <param name="ObjEProject"></param>
        /// <returns></returns>
        public EProject GetPath(EProject ObjEProject)
        {
            DataSet dsPath = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_Path]";
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsPath);
                    }
                    if(dsPath != null && dsPath.Tables.Count > 0 && dsPath.Tables[0].Rows.Count > 0)
                    {
                        ObjEProject.CoverSheetPath = Convert.ToString(dsPath.Tables[0].Rows[0][0]);
                        if(dsPath.Tables.Count > 1 && dsPath.Tables[1].Rows.Count > 0)
                            ObjEProject.TemplatePath = Convert.ToString(dsPath.Tables[1].Rows[0][0]);
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
            return ObjEProject;
        }

        /// <summary>
        /// Code to delete a project
        /// </summary>
        /// <param name="ProjectID"></param>
        public void DeleteProject(int ProjectID)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Del_Project]";
                    cmd.Parameters.AddWithValue("@ProjectID", ProjectID);
                    object returnObj = cmd.ExecuteScalar();
                    string strReturn = Convert.ToString(returnObj);
                    if (!string.IsNullOrEmpty(strReturn))
                        throw new Exception("Fehler beim Löschen des Projektes");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Code to Copy a project
        /// </summary>
        /// <param name="ObjEProject"></param>
        /// <returns></returns>
        public EProject CopyProject(EProject ObjEProject)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Ins_CopyProject]";
                    cmd.Parameters.AddWithValue("@ProjectID", ObjEProject.ProjectID);
                    cmd.Parameters.AddWithValue("@ProjectNumber", ObjEProject.ProjectNumber);
                    cmd.Parameters.AddWithValue("@UserId", ObjEProject.UserID);
                    object returnObj = cmd.ExecuteScalar();
                    string strReturn = Convert.ToString(returnObj);
                    int Ivalue = 0;
                    if (int.TryParse(strReturn, out Ivalue))
                        ObjEProject.ProjectID = Ivalue;
                    else
                    {
                        if (returnObj.ToString().Contains("ProjectNumber"))
                        {
                            if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                                throw new Exception("Diese Projektnummer existiert bereits");
                            else
                                throw new Exception("Project Nr Already Exists");
                        }
                        else
                            throw new Exception("Fehler beim Kopieren des Projektes");
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEProject;
        }

        /// <summary>
        /// Code to fatch data from data to transfer to CalcPro projects
        /// </summary>
        /// <param name="ObjEProject"></param>
        /// <returns></returns>
        public EProject GetCockpitData(EProject ObjEProject)
        {
            try
            {
                DataSet dsCockpitData = new DataSet();
                ObjEProject.dtCockpitData = new DataTable();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_CockpitData]";
                    cmd.Parameters.AddWithValue("@ProjectID", ObjEProject.ProjectID);
                    cmd.Parameters.AddWithValue("@dtCockpitData", ObjEProject.dtTemplateData);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsCockpitData);
                    }
                    if(dsCockpitData != null && dsCockpitData.Tables.Count > 0)
                    {
                        ObjEProject.dtCockpitData = dsCockpitData.Tables[0];
                        if (dsCockpitData.Tables.Count > 1 && dsCockpitData.Tables[1].Rows.Count > 0)
                        {
                            decimal DValue = 0;
                            if (decimal.TryParse(Convert.ToString(dsCockpitData.Tables[1].Rows[0]["SValue"]), out DValue))
                                ObjEProject.SValue = DValue;
                            if (decimal.TryParse(Convert.ToString(dsCockpitData.Tables[1].Rows[0]["XValue"]), out DValue))
                                ObjEProject.XValue = DValue;
                            if (decimal.TryParse(Convert.ToString(dsCockpitData.Tables[1].Rows[0]["UmlageCost"]), out DValue))
                                ObjEProject.UmlageCost = DValue;
                            if (decimal.TryParse(Convert.ToString(dsCockpitData.Tables[1].Rows[0]["RevenueTotal"]), out DValue))
                                ObjEProject.RevenueTotal = DValue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEProject;
        }

        /// <summary>
        /// Code to Insert data to OTTO Projects
        /// </summary>
        /// <param name="ObjEProject"></param>
        /// <returns></returns>
        public string InssertCockpitData(EProject ObjEProject)
        {
            string strError = string.Empty;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.CockpitConnection();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Ins_InsertCalcProData]";
                    cmd.Parameters.Add("@dtData", ObjEProject.dtCockpitData);
                    cmd.Parameters.Add("@ProjectNumber", ObjEProject.ProjectNumber);
                    cmd.Parameters.Add("@SValue", ObjEProject.SValue);
                    cmd.Parameters.Add("@XValue", ObjEProject.XValue);
                    cmd.Parameters.Add("@UmlageCost", ObjEProject.UmlageCost);
                    cmd.Parameters.Add("@RevenueTotal", ObjEProject.RevenueTotal);
                    cmd.Parameters.Add("@ProjectDescription", ObjEProject.ProjectDescription);
                    cmd.Parameters.Add("@ProjectStartDate", ObjEProject.ProjectStartDate);
                    cmd.Parameters.Add("@ProjectEndDate", ObjEProject.ProjectEndDate);
                    cmd.Parameters.Add("@UserName", ObjEProject.UserName);
                    object returnObj = cmd.ExecuteScalar();
                    strError = Convert.ToString(returnObj);
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
            return strError;
        }

        /// <summary>
        /// Code to update article setting of a project
        /// </summary>
        /// <param name="ObjEProject"></param>
        public void UpdateArticleSettings(EProject ObjEProject)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Upd_ArticleSettings]";
                    cmd.Parameters.AddWithValue("@projectID", ObjEProject.ProjectID);
                    cmd.Parameters.AddWithValue("@dtArticleSettings", ObjEProject.dtArticleSettings);
                    cmd.Parameters.AddWithValue("@UserID", ObjEProject.UserID);
                    object oReturn = cmd.ExecuteScalar();
                    int IValue = 0;
                    if (!int.TryParse(Convert.ToString(oReturn), out IValue))
                        throw new Exception(Convert.ToString(oReturn));
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
        }

        /// <summary>
        /// Code to save PRoject COmmentary
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="CommentaryDescription"></param>
        public void SaveProjectCommentary(int ProjectID, string CommentaryDescription)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Ins_ProjectCommentary]";
                    cmd.Parameters.AddWithValue("@ProjectID", ProjectID);
                    cmd.Parameters.AddWithValue("@CommentaryDescription", CommentaryDescription);
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
        }

        /// <summary>
        /// Code to fetch project commentary
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public string GetProjectCommentary(int ProjectID)
        {
            string st = string.Empty;
            try
            {
                using (SqlCommand cmd = new SqlCommand())   
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_ProjectCommentary]";
                    cmd.Parameters.AddWithValue("@ProjectID", ProjectID);
                    object ObjReturn = cmd.ExecuteScalar();
                    st = Convert.ToString(ObjReturn);
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
            return st;
        }

        /// <summary>
        /// Code to save angebot commentary
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="CommentaryDescription"></param>
        public void SaveAngebotCommentary(int ProjectID, string CommentaryDescription)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Ins_AngebotComentary]";
                    cmd.Parameters.AddWithValue("@ProjectID", ProjectID);
                    cmd.Parameters.AddWithValue("@CommentaryDescription", CommentaryDescription);
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
        }

        /// <summary>
        /// Code to fetch Angebot commentary
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public string GetAngebotCommentary(int ProjectID)
        {
            string st = string.Empty;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = SQLCon.Sqlconn();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[P_Get_AngebotComentary]";
                    cmd.Parameters.AddWithValue("@ProjectID", ProjectID);
                    object ObjReturn = cmd.ExecuteScalar();
                    st = Convert.ToString(ObjReturn);
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
            return st;
        }

        /// <summary>
        /// code to Get PRoject and customer details
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

        /// <summary>
        ///  Code to Close DBconnection on application exit
        /// </summary>
        public void CloseDBConnection()
        {
            SQLCon.Close();
        }
    }
}
