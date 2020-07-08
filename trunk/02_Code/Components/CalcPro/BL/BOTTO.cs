using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EL;
using System.Xml;
using DataAccess;
using System.Data;

namespace BL
{
    public class BOTTO
    {
        DOTTO ObjDOTTO = new DOTTO();

        /// <summary>
        /// Code to get save Organization details
        /// </summary>
        /// <param name="ObjEOTTO"></param>
        /// <returns></returns>
        public EOTTO SaveOTTODetails(EOTTO ObjEOTTO)
        {
            try
            {
                XmlDocument Xdoc = new XmlDocument();
                string XPath = "/Nouns/OTTO";
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "OttoID", ObjEOTTO.OTTOID.ToString());
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "ShortName", ObjEOTTO.ShortName);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "FullName", ObjEOTTO.FullName);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "IsBranch", ObjEOTTO.IsBranch.ToString());
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "Street", ObjEOTTO.Street);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "PostalCode", ObjEOTTO.PostalCode);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "City", ObjEOTTO.City);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "Country", ObjEOTTO.Country);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "ILN", ObjEOTTO.ILN);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "BankName", ObjEOTTO.BankName);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "BankPostalCode", ObjEOTTO.BankPostalCode);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "BankAccNo", ObjEOTTO.BankAccNo);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "DVNr", ObjEOTTO.DVNr);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "TenderNo", ObjEOTTO.TenderNo);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "DebtorNo", ObjEOTTO.DebtorNo);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "CountryType", ObjEOTTO.CountryType);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "Industry", ObjEOTTO.Industry);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "ArtBevBew", ObjEOTTO.ArtBevBew);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "ArtNU", ObjEOTTO.ArtNU);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "BGBez", ObjEOTTO.BGBez);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "BGDatum", ObjEOTTO.BGDatum);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "BGNr", ObjEOTTO.BGNr);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "Telefon", ObjEOTTO.Telefon);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "Telefax", ObjEOTTO.Telefax);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "Website", ObjEOTTO.Website);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "HotLine", ObjEOTTO.HotLine);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "IBAN", ObjEOTTO.IBAN);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "BIC", ObjEOTTO.BIC);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "USTIDNr", ObjEOTTO.USTIDNr);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "SeatofCompany", ObjEOTTO.SeatofCompany);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "ManagingDirector", ObjEOTTO.ManagingDirector);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "Complementary", ObjEOTTO.Complementary);
                ObjEOTTO = ObjDOTTO.SaveOTTODetails(Xdoc, ObjEOTTO);
            }
            catch (Exception ex){throw;}
            return ObjEOTTO;
        }

        /// <summary>
        /// Code to get Organization details
        /// </summary>
        /// <param name="ObjEOTTO"></param>
        /// <returns></returns>
        public EOTTO GetOTTODetails(EOTTO ObjEOTTO)
        {
            try
            {
                if (ObjEOTTO != null)
                {
                    ObjEOTTO = ObjDOTTO.GetOTTODetails(ObjEOTTO);
                }
            }
            catch (Exception ex){throw;}
            return ObjEOTTO;
        }

        /// <summary>
        /// Code to save Organzition contacts
        /// </summary>
        /// <param name="ObjEOTTO"></param>
        /// <returns></returns>
        public EOTTO SaveOTTOContactDetails(EOTTO ObjEOTTO)
        {
            try
            {
                XmlDocument Xdoc = new XmlDocument();
                string XPath = "/Nouns/OTTOContact";
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "OttoID", ObjEOTTO.OTTOID.ToString());
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "ContactID", ObjEOTTO.ContactID.ToString());
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "ContactPerson", ObjEOTTO.ContactPerson);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "Telephone", ObjEOTTO.Cont_Telephone);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "Fax", ObjEOTTO.Fax);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "EmailID", ObjEOTTO.EmailID);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "TaxNo", ObjEOTTO.TaxNo);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "DefaultContact", ObjEOTTO.DefaultContact.ToString());
                ObjEOTTO = ObjDOTTO.SaveOTTOContactDetails(Xdoc, ObjEOTTO);
            }
            catch (Exception ex){throw;}
            return ObjEOTTO;
        }

        /// <summary>
        /// Code to import customer data
        /// Not using now but we can use in future
        /// </summary>
        /// <param name="dt"></param>
        public void ImportCustomerData(DataTable dt)
        {
            try
            {
                ObjDOTTO.ImportCustomerData(dt);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
