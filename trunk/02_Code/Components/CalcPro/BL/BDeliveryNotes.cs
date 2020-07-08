using DataAccess;
using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class BDeliveryNotes
    {
        DDeliveryNotes ObjDdeliveryNotes = new DDeliveryNotes();

        /// <summary>
        ///  Code to fetch positions for delivery notes module
        /// </summary>
        /// <param name="ObjEDeliveryNotes"></param>
        /// <returns></returns>
        public EDeliveryNotes GetPositions(EDeliveryNotes ObjEDeliveryNotes)
        {
            try
            {
                ObjEDeliveryNotes = ObjDdeliveryNotes.GetPositions(ObjEDeliveryNotes);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEDeliveryNotes;
        }

        /// <summary>
        /// Code to save a delivery
        /// </summary>
        /// <param name="ObjEDeliveryNotes"></param>
        /// <returns></returns>
        public EDeliveryNotes SaveDelivery(EDeliveryNotes ObjEDeliveryNotes)
        {
            try
            {
                ObjEDeliveryNotes = ObjDdeliveryNotes.SaveDelivery(ObjEDeliveryNotes);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEDeliveryNotes;
        }

        /// <summary>
        /// Code to fetch non active deliveries from database
        /// </summary>
        /// <param name="ObjEDeliveryNotes"></param>
        /// <returns></returns>
        public EDeliveryNotes GetNonActiveDelivery(EDeliveryNotes ObjEDeliveryNotes)
        {
            try
            {
                ObjEDeliveryNotes = ObjDdeliveryNotes.GetNonActiveDelivery(ObjEDeliveryNotes);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEDeliveryNotes;
        }

        /// <summary>
        /// Code to get Saved blatt numbers from database
        /// </summary>
        /// <param name="ObjEDeliveryNotes"></param>
        /// <returns></returns>
        public EDeliveryNotes GetBlattNumbers(EDeliveryNotes ObjEDeliveryNotes)
        {
            try
            {
                ObjEDeliveryNotes = ObjDdeliveryNotes.GetBlattNumbers(ObjEDeliveryNotes);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEDeliveryNotes;
        }

        /// <summary>
        /// Code to get next available Blatt Number
        /// </summary>
        /// <param name="ObjEDeliveryNotes"></param>
        /// <returns></returns>
        public EDeliveryNotes GetNewBlattNumber(EDeliveryNotes ObjEDeliveryNotes)
        {
            try
            {
                ObjEDeliveryNotes = ObjDdeliveryNotes.GetNewBlattNumber(ObjEDeliveryNotes);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEDeliveryNotes;
        }

        /// <summary>
        /// Code to get Blatt details after saving
        /// </summary>
        /// <param name="ObjEDeliveryNotes"></param>
        /// <returns></returns>
        public EDeliveryNotes GetBlattDetails(EDeliveryNotes ObjEDeliveryNotes)
        {
            try
            {
                ObjEDeliveryNotes = ObjDdeliveryNotes.GetBlattDetails(ObjEDeliveryNotes);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEDeliveryNotes;
        }
    }
}
