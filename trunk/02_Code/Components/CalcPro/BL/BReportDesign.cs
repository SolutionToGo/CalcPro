using DataAccess;
using EL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class BReportDesign
    {
        
        DReportDesign ObjDReportDesign = new DReportDesign();

        /// <summary>
        /// Code to Save report settings
        /// </summary>
        /// <param name="ObjEObject"></param>
        public void SaveReportSetting(EReportDesign ObjEObject)
        {
            try
            {
                if (ObjEObject != null)
                {
                    ObjDReportDesign.SaveReportSetting(ObjEObject);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Code to fetch report settings from database
        /// </summary>
        /// <param name="ObjEObject"></param>
        /// <returns></returns>
        public DataSet GetReportSettings(EReportDesign ObjEObject)
        {
            DataSet dsReportSetting = new DataSet();
            try
            {
                if (ObjEObject != null)
                {
                    dsReportSetting = ObjDReportDesign.GetReportSettings(ObjEObject);
                }
            }
            catch (Exception ex)
            {
                throw;
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
                dt = ObjDReportDesign.GetProjectCustomerDetails(ProjectID);
            }
            catch (Exception ex) { throw ex; }
            return dt;
        }

    }
}
