using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using EL;

namespace BL
{
    public class BInvoice
    {
        DInvoice ObjDInvoice = new DInvoice();

        /// <summary>
        /// Code to get invoice next blatt numbers
        /// </summary>
        /// <param name="ObjEInvoice"></param>
        /// <returns></returns>
        public EInvoice GeTBlattNumbers(EInvoice ObjEInvoice)
        {
            try
            {
                ObjEInvoice = ObjDInvoice.GeTBlattNumbers(ObjEInvoice);
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
            try
            {
                ObjEInvoice = ObjDInvoice.SaveInvoice(ObjEInvoice);
            }
            catch (Exception ex)
            {
                throw;
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
                ObjEInvoice = ObjDInvoice.GetInvoices(ObjEInvoice);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEInvoice;
        }
    }
}
