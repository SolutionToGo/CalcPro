using DataAccess;
using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class BUmlage
    {
        DUmlage ObjDUmlage = new DUmlage();

        /// <summary>
        /// Code to update special cost to positions
        /// </summary>
        /// <param name="ObjEUmlage"></param>
        /// <returns></returns>
        public EUmlage UpdateSpecialCost(EUmlage ObjEUmlage)
        {
            try
            {
                ObjEUmlage = ObjDUmlage.UpdateSpecialCost(ObjEUmlage);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEUmlage;
        }

        /// <summary>
        /// Code to fetch special cost details from database
        /// </summary>
        /// <param name="ObjEUmlage"></param>
        /// <returns></returns>
        public EUmlage GetSpecialCost(EUmlage ObjEUmlage)
        {
            try
            {
                ObjEUmlage = ObjDUmlage.GetSpecialCost(ObjEUmlage);
            }
            catch (Exception ex)
            {
                throw;
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
                ObjEUmlage = ObjDUmlage.ShowUmlage(ObjEUmlage);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEUmlage;
        }

        /// <summary>
        ///  Code to save special cost on database
        /// </summary>
        /// <param name="ObjEUmlage"></param>
        /// <returns></returns>
        public EUmlage SaveSpecialCost(EUmlage ObjEUmlage)
        {
            try
            {
                ObjEUmlage = ObjDUmlage.SaveSpecialCost(ObjEUmlage);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEUmlage;
        }
    }
}
