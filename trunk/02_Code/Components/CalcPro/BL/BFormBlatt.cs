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
    public class BFormBlatt
    {
        DFormBlatt ObjDFormBlatt = new DFormBlatt();

       /// <summary>
       ///  Code to get Formblatt 221 table 2 values
       /// </summary>
       /// <param name="ObjEFormBlatt"></param>
       /// <returns></returns>
        public EFormBlatt Get_tbl221_2(EFormBlatt ObjEFormBlatt)
        {
            try
            {
                ObjEFormBlatt = ObjDFormBlatt.Get_tbl221_2(ObjEFormBlatt);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEFormBlatt;
        }

        /// <summary>
        /// Code to get Cost types
        /// </summary>
        /// <param name="ObjEFormBlatt"></param>
        /// <returns></returns>
        public EFormBlatt Get_FormBlattTypes(EFormBlatt ObjEFormBlatt)
        {
            try
            {
                ObjEFormBlatt = ObjDFormBlatt.GetFormBlatttype(ObjEFormBlatt);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEFormBlatt;
        }

        /// <summary>
        /// Code to get Articles based on cost type mapping
        /// </summary>
        /// <param name="ObjEFormBlatt"></param>
        /// <returns></returns>
        public EFormBlatt Get_FormBlattArticles(EFormBlatt ObjEFormBlatt)
        {
            try
            {
                ObjEFormBlatt = ObjDFormBlatt.Get_FormBlattArticles(ObjEFormBlatt);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEFormBlatt;
        }

        /// <summary>
        /// Code to save Cost type mapping with articles
        /// </summary>
        /// <param name="ObjEFormBlatt"></param>
        /// <param name="_dt"></param>
        /// <returns></returns>
        public EFormBlatt Save_FormBlattArticles(EFormBlatt ObjEFormBlatt,DataTable _dt)
        {
            try
            {
                ObjEFormBlatt = ObjDFormBlatt.Save_FormBlattArticles(ObjEFormBlatt,_dt);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEFormBlatt;
        }

            /// <summary>
            /// Code to Get Form blatt articles mapping
            /// </summary>
            /// <param name="ObjEFormBlatt"></param>
            /// <returns></returns>
        public EFormBlatt GetFormBlattMapping(EFormBlatt ObjEFormBlatt)
        {
            try
            {
                ObjEFormBlatt = ObjDFormBlatt.GetFormBlattMapping(ObjEFormBlatt);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEFormBlatt;
        }
    }
}
