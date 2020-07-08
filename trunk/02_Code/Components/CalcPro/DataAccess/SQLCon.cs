using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class SQLCon
    {
        /// <summary>
        /// Log4net object for error and exception logging
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Sql connection object for login
        /// </summary>
        static SqlConnection ObjCon2 = new SqlConnection();

        /// <summary>
        /// Sql connection for other transactions after logging in
        /// </summary>
        static SqlConnection ObjCon = new SqlConnection();

        /// <summary>
        /// Sql connection for transfering data to CalcProjects
        /// </summary>
        static SqlConnection ObjCockpitConn = new SqlConnection();

        /// <summary>
        /// Sql connection for other transactions after logging in
        /// </summary>
        public static SqlConnection Sqlconn()
        {
            try
            {
                if (ObjCon.State == ConnectionState.Closed)
                {
                    Log.Info("Connection is Closed");
                    ObjCon.ConnectionString = ConfigurationManager.ConnectionStrings["CalcPro"].ToString();
                    ObjCon.Open();
                    Log.Info("Connection is Open");
                }
            }
            catch (Exception ex){ Log.Error(ex.Message, ex); }
            return ObjCon;
        }

        /// <summary>
        /// Connection string for Reports
        /// </summary>
        /// <returns></returns>
        public static string ConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["CalcPro"].ToString();
        }

        /// <summary>
        /// Sql connection for transfering data to CalcProjects
        /// </summary>
        public static SqlConnection CockpitConnection()
        {
            if (ObjCockpitConn.State == ConnectionState.Open)
            {
                return ObjCockpitConn;
            }
            else
            {
                ObjCockpitConn.ConnectionString = ConfigurationManager.AppSettings["CockpitConnection"].ToString();
                ObjCockpitConn.Open();
                return ObjCockpitConn;
            }
        }

        /// <summary>
        /// Closing database connection
        /// </summary>
        public static void Close()
        {
            try
            {
                //if (ObjCon.State == ConnectionState.Open)
                //{
                //    ObjCon.Close();
                //}
            }
            catch (Exception ex) { Log.Error(ex.Message, ex); }
        }

        /// <summary>
        /// Sql connection object for login
        /// </summary>
        public static SqlConnection Sqlconn2()
        {
            try
            {
                if (ObjCon2.State == ConnectionState.Closed)
                {
                    Log.Info("Connection is Closed");
                    ObjCon2.ConnectionString = ConfigurationManager.ConnectionStrings["CalcPro"].ToString();
                    ObjCon2.Open();
                    Log.Info("Connection is Open");
                }
            }
            catch (Exception ex) { Log.Error(ex.Message, ex); }
            return ObjCon2;
        }

        /// <summary>
        /// Closing database connection
        /// </summary>
        public static void Close2()
        {
            try
            {
            
            }
            catch (Exception ex) { Log.Error(ex.Message, ex); }
        }
    }
}
