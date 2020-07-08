using DataAccess;
using EL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BL
{
    public class BSupplier
    {
        DSupplier ObjDSupplier = new DSupplier();

        /// <summary>
        /// Code to add or edit supplier details from supplier master to database
        /// </summary>
        /// <param name="ObjEsupplier"></param>
        /// <returns></returns>
        public ESupplier SaveSupplierDetails(ESupplier ObjEsupplier)
        {
            try
            {
                XmlDocument Xdoc = new XmlDocument();
                string XPath = "/Nouns/Supplier";
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "SupplierID", ObjEsupplier.SupplierID.ToString());
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "FullName", ObjEsupplier.SupplierFullName);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "ShortName", ObjEsupplier.SupplierShortName);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "PaymentCondition", ObjEsupplier.PaymentCondition);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "Commentary", ObjEsupplier.Commentary);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "EmailID", ObjEsupplier.SupplierEmailID);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "Street", ObjEsupplier.SupplierStreet);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "Telephone", ObjEsupplier.SupplierTelephone);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "Fax", ObjEsupplier.SupplierFax);

                ObjEsupplier = ObjDSupplier.SaveSupplierDetails(Xdoc, ObjEsupplier);
                return ObjEsupplier;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Code to fetch supplier list from database
        /// </summary>
        /// <param name="ObjEsupplier"></param>
        /// <returns></returns>
        public ESupplier GetSupplier(ESupplier ObjEsupplier)
        {
            try
            {
                if (ObjEsupplier != null)
                {
                    DataSet ds = ObjDSupplier.GetSupplier();
                    ObjEsupplier.dtSupplier = ds.Tables[0];
                    ObjEsupplier.dtContact = ds.Tables[1];
                    ObjEsupplier.dtAddress = ds.Tables[2];
                    ObjEsupplier.dtArticle = ds.Tables[3];
                }
            }
            catch (Exception ex){throw;}
            return ObjEsupplier;
        }

        /// <summary>
        /// Code to Add or Edit supplier contact details from Supplier master to database
        /// </summary>
        /// <param name="ObjEsupplier"></param>
        /// <returns></returns>
        public ESupplier SaveSupplierContactDetails(ESupplier ObjEsupplier)
        {
            try
            {
                XmlDocument Xdoc = new XmlDocument();
                string XPath = "/Nouns/SupplierContact";
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "SupplierID", Convert.ToString(ObjEsupplier.SupplierID));
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "ContactPersonID", Convert.ToString(ObjEsupplier.ContactPersonID));
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "ContactName", ObjEsupplier.ContactName);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "Salutation", ObjEsupplier.Salutation);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "Designation", ObjEsupplier.Designation);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "EmailID", ObjEsupplier.ContEmailID);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "Telephone", ObjEsupplier.ContTelephone);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "FAX", ObjEsupplier.ContFax);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "DefaultContact", Convert.ToString(ObjEsupplier.DefaultContact));
                ObjEsupplier = ObjDSupplier.SavedsSupplierContactDetails(Xdoc, ObjEsupplier);
                return ObjEsupplier;
            }
            catch (Exception ex){throw;}
        }

        /// <summary>
        /// Code to Add or Edit Supplier address details from Supplier master to database
        /// </summary>
        /// <param name="ObjEsupplier"></param>
        /// <returns></returns>
        public ESupplier SaveSupplierAddressDetails(ESupplier ObjEsupplier)
        {
            try
            {
                XmlDocument Xdoc = new XmlDocument();
                string XPath = "/Nouns/SupplierAddress";
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "SupplierID", Convert.ToString(ObjEsupplier.SupplierID));
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "AddressID", Convert.ToString(ObjEsupplier.AddressID));
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "ShortName", ObjEsupplier.AddressShortName);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "StreetNo", ObjEsupplier.StreetNo);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "PostalCode", ObjEsupplier.AddrPostalCode);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "City", ObjEsupplier.AddrCity);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "Country", ObjEsupplier.AddrCountry);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "DefaultAddress", Convert.ToString(ObjEsupplier.DefaultAddress));
                ObjEsupplier = ObjDSupplier.SaveSupplierAddressDetails(Xdoc, ObjEsupplier);
                return ObjEsupplier;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Code to save Supplier articles from Supplier master to database
        /// </summary>
        /// <param name="ObjEsupplier"></param>
        /// <returns></returns>
        public ESupplier SaveArticle(ESupplier ObjEsupplier)
        {
            try
            {
                ObjEsupplier = ObjDSupplier.SaveArticle(ObjEsupplier);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEsupplier;
        }

        /// <summary>
        ///  Code to save Supplier and Article mapping form Price comparision module
        /// </summary>
        /// <param name="ObjESupplier"></param>
        /// <returns></returns>
        public ESupplier SaveArticleFromProposal(ESupplier ObjESupplier)
        {
            try
            {
                ObjESupplier = ObjDSupplier.SaveArticleFromProposal(ObjESupplier);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjESupplier;
        }

        /// <summary>
        /// Code fetch positions from database for not saved supplier propsoal by passing ProjectID, LVSection, Articles as imput parameters
        /// </summary>
        /// <param name="ObjESupplier"></param>
        /// <returns></returns>
        public ESupplier GetPositionsforsupplierProposal(ESupplier ObjESupplier)
        {
            try
            {
                if (ObjDSupplier == null)
                    ObjDSupplier = new DSupplier();
                ObjDSupplier.GetPositionsforsupplierProposal(ObjESupplier);
            }
            catch (Exception ex){throw ex;}
            return ObjESupplier;
        }

        /// <summary>
        /// Code to fetch project LV sections using projectid for supplier proposal module
        /// </summary>
        /// <param name="ObjEsupplier"></param>
        /// <returns></returns>
        public ESupplier GetLVSectionForProposal(ESupplier ObjEsupplier)
        {
            try
            {
                ObjDSupplier.GetLVSectionforProposal(ObjEsupplier);
            }
            catch (Exception ex){throw ex;}
            return ObjEsupplier;
        }

        /// <summary>
        /// Code to save Supplier proposal
        /// </summary>
        /// <param name="ObjEsupplier"></param>
        /// <returns></returns>
        public ESupplier SaveSupplierProposal(ESupplier ObjEsupplier)
        {
            try
            {
                ObjDSupplier.SaveSupplierProposal(ObjEsupplier);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEsupplier;
        }

        /// <summary>
        /// Code to fetch saved adn not saved supplier proposal Numbers
        /// </summary>
        /// <param name="ObjEsupplier"></param>
        /// <returns></returns>
        public ESupplier GetProposalNumber(ESupplier ObjEsupplier)
        {
            try
            {
                ObjDSupplier.GetProposalNumber(ObjEsupplier);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEsupplier;
        }

        /// <summary>
        /// Code to fetch positions from database based on supplier proposalID
        /// </summary>
        /// <param name="ObjESupplier"></param>
        /// <returns></returns>
        public ESupplier GetPositionsByProposalID(ESupplier ObjESupplier)
        {
            try
            {
                ObjDSupplier.GetPositionsByProposalID(ObjESupplier);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ObjESupplier;
        }

        /// <summary>
        /// Code to fetch not saved articles for supplier proposal module while merging articles with proposals
        /// </summary>
        /// <param name="ObjEsupplier"></param>
        /// <returns></returns>
        public ESupplier GetArticlesForProposal(ESupplier ObjEsupplier)
        {
            try
            {
                ObjEsupplier = ObjDSupplier.GetArticlesForProposal(ObjEsupplier);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEsupplier;
        }

        /// <summary>
        ///  Code to fetch Supplier proposals for proce comparision module
        /// </summary>
        /// <param name="ObjEsupplier"></param>
        /// <returns></returns>
        public ESupplier GetUpdateSupplierProposal(ESupplier ObjEsupplier)
        {
            try
            {
                ObjEsupplier = ObjDSupplier.GetUpdateSupplierProposal(ObjEsupplier);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEsupplier;
        }

        /// <summary>
        /// Code to save deleted positions from supplier proposal module
        /// </summary>
        /// <param name="ObjEsupplier"></param>
        /// <returns></returns>
        public ESupplier SaveDeletePosition(ESupplier ObjEsupplier)
        {
            try
            {
                ObjEsupplier = ObjDSupplier.SaveDeletePosition(ObjEsupplier);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEsupplier;
        }

        /// <summary>
        /// Code to fetch positions based on supplier proposalID for price comparision module
        /// </summary>
        /// <param name="ObjESupplier"></param>
        /// <param name="_IsCalculate"></param>
        /// <returns></returns>
        public ESupplier GetProposalPostions(ESupplier ObjESupplier, bool _IsCalculate = true)
        {
            try
            {
                ObjESupplier = ObjDSupplier.GetProposalPostions(ObjESupplier);
                ObjESupplier.dtPositions.Columns["Cheapest"].Caption = "Niedrigstpreis";
                DataTable dtTemp = ObjESupplier.dtPositions.Clone();
                ChangeCultureInfo(dtTemp);
                foreach (DataColumn dc in dtTemp.Columns)
                {
                    if (dc.ColumnName.Contains("Check"))
                    {
                        dc.DataType = System.Type.GetType("System.Boolean");
                        dc.Caption = "";
                    }
                    else if (dtTemp.Columns[dc.ColumnName + "Check"] != null)
                    {
                        dc.DataType = System.Type.GetType("System.Decimal");
                    }
                    else if (dc.ColumnName.Contains("Multi"))
                    {
                        dc.DataType = System.Type.GetType("System.Decimal");
                    }
                }
                foreach (DataRow dr in ObjESupplier.dtPositions.Rows)
                {
                    dtTemp.ImportRow(dr);
                }
                ObjESupplier.dtPositions = new DataTable();
                ChangeCultureInfo(ObjESupplier.dtPositions);
                ObjESupplier.dtPositions = dtTemp.Copy();
                ObjESupplier.dtPositions.Columns.Add("SID", typeof(int));
                int iindex = 0;
                foreach (DataRow dr in ObjESupplier.dtPositions.Rows)
                {
                    iindex++;
                    dr["SID"] = iindex;
                }
                if (_IsCalculate)
                    ObjESupplier = CalculateCheapestValues(ObjESupplier);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjESupplier;
        }

        /// <summary>
        /// Code to update price to postions from price comparision module
        /// </summary>
        /// <param name="ObjESupplier"></param>
        /// <returns></returns>
        public ESupplier UpdateSupplierPrice(ESupplier ObjESupplier)
        {
            try
            {
                ObjESupplier = ObjDSupplier.UpdateSupplierPrice(ObjESupplier);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjESupplier;
        }

       /// <summary>
       /// Code to update proposal values to positions from price comparision module
       /// </summary>
       /// <param name="ObjESupplier"></param>
       /// <returns></returns>
        public ESupplier SaveProposaleValues(ESupplier ObjESupplier)
        {
            try
            {
                ObjESupplier = ObjDSupplier.SaveProposaleValues(ObjESupplier);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjESupplier;
        }

        /// <summary>
        /// Code to save Supplier price to proposal positions from supplier proposal module
        /// </summary>
        /// <param name="ObjESupplier"></param>
        /// <returns></returns>
        public ESupplier SaveSupplierPrice(ESupplier ObjESupplier)
        {
            try
            {
                ObjESupplier = ObjDSupplier.SaveSupplierPrice(ObjESupplier);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjESupplier;
        }

        /// <summary>
        /// Code to save Supplier selection to position
        /// </summary>
        /// <param name="ObjESupplier"></param>
        /// <returns></returns>
        public ESupplier SaveSelection(ESupplier ObjESupplier)
        {
            try
            {
                ObjESupplier = ObjDSupplier.SaveSelection(ObjESupplier);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjESupplier;
        }

        /// <summary>
        /// Code to save supplier selection for all positions
        /// </summary>
        /// <param name="ObjESupplier"></param>
        /// <returns></returns>
        public ESupplier SaveBulkSelection(ESupplier ObjESupplier)
        {
            try
            {
                ObjESupplier = ObjDSupplier.SaveBulkSelection(ObjESupplier);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjESupplier;
        }

        /// <summary>
        /// Code to change the view of positions
        /// </summary>
        /// <param name="ObjESupplier"></param>
        /// <param name="ViewMode"></param>
        /// <returns></returns>
        public ESupplier ChangeProposalView(ESupplier ObjESupplier, int ViewMode)
        {
            try
            {
                if (ViewMode == 0)
                {
                    ObjESupplier = GetProposalPostions(ObjESupplier);
                }
                else
                {
                    ObjESupplier.dtPositionscopy = new DataTable();
                    ObjESupplier.dtPositionscopy = ObjESupplier.dtPositions.Copy();
                    //ObjESupplier = GetProposalPostions(ObjESupplier, false);
                    List<string> TableColumnNAmes = new List<string>();
                    foreach (DataColumn dc in ObjESupplier.dtPositionscopy.Columns)
                    {
                        if (ObjESupplier.dtPositionscopy.Columns[dc.ColumnName + "Check"] != null)
                        {
                            TableColumnNAmes.Add(dc.ColumnName);
                        }
                    }

                    foreach (DataRow drTemp in ObjESupplier.dtPositionscopy.Rows)
                    {
                        foreach (string s in TableColumnNAmes)
                        {
                            decimal DValue = 0;
                            string strValue = drTemp[s] == DBNull.Value ? "" : drTemp[s].ToString();
                            if (decimal.TryParse(strValue, out DValue))
                            {
                                if (ViewMode == 1 || ViewMode == 3)
                                {
                                    string strMulti1 = drTemp[s + "Multi1"] == DBNull.Value ? "" : drTemp[s + "Multi1"].ToString();
                                    string strMulti2 = drTemp[s + "Multi2"] == DBNull.Value ? "" : drTemp[s + "Multi2"].ToString();
                                    string strMulti3 = drTemp[s + "Multi3"] == DBNull.Value ? "" : drTemp[s + "Multi3"].ToString();
                                    string strMulti4 = drTemp[s + "Multi4"] == DBNull.Value ? "" : drTemp[s + "Multi4"].ToString();
                                    decimal dMulti1 = 1;
                                    decimal dMulti2 = 1;
                                    decimal dMulti3 = 1;
                                    decimal dMulti4 = 1;

                                    if (decimal.TryParse(strMulti1, out dMulti1))
                                        DValue = DValue * dMulti1;
                                    if (decimal.TryParse(strMulti2, out dMulti2))
                                        DValue = DValue * dMulti2;
                                    if (decimal.TryParse(strMulti3, out dMulti3))
                                        DValue = DValue * dMulti3;
                                    if (decimal.TryParse(strMulti4, out dMulti4))
                                        DValue = DValue * dMulti4;
                                }
                                if (ViewMode == 2 || ViewMode == 3)
                                {
                                    decimal dMenge = 0;
                                    string strMenge = drTemp["Menge"] == DBNull.Value ? "" : drTemp["Menge"].ToString();
                                    if (decimal.TryParse(strMenge, out dMenge))
                                        DValue = DValue * dMenge;
                                }
                            }
                            drTemp[s] = Math.Round(DValue, 3);
                        }
                    }
                    ObjESupplier = CalculateCheapestValuesCopy(ObjESupplier);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjESupplier;
        }

        /// <summary>
        /// Code to calculate Cheapest supplier value for each position in selected proposal
        /// </summary>
        /// <param name="ObjESupplier"></param>
        /// <returns></returns>
        private ESupplier CalculateCheapestValues(ESupplier ObjESupplier)
        {
            try
            {
                List<string> TableColumnNAmes = new List<string>();
                foreach (DataColumn dc in ObjESupplier.dtPositions.Columns)
                {
                    if (ObjESupplier.dtPositions.Columns[dc.ColumnName + "Check"] != null)
                    {
                        TableColumnNAmes.Add(dc.ColumnName);
                    }
                }

                foreach (DataRow drTemp in ObjESupplier.dtPositions.Rows)
                {
                    List<double> SupplierPirces = new List<double>();
                    foreach (string s in TableColumnNAmes)
                    {
                        double DValue = drTemp[s] == DBNull.Value ? 0 : Math.Round(Convert.ToDouble(drTemp[s]), 3);
                        if (DValue > 0)
                            SupplierPirces.Add(DValue);
                    }
                    if (SupplierPirces != null && SupplierPirces.Count() > 0)
                    {
                        var i = Math.Round(SupplierPirces.Min(), 3);
                        drTemp["Cheapest"] = i;
                    }
                    else
                        drTemp["Cheapest"] = 0;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjESupplier;
        }

        /// <summary>
        /// Code to calculate Cheapest supplier value for each position in selected proposal while changing the proposal view
        /// </summary>
        /// <param name="ObjESupplier"></param>
        /// <returns></returns>
        private ESupplier CalculateCheapestValuesCopy(ESupplier ObjESupplier)
        {
            try
            {
                List<string> TableColumnNAmes = new List<string>();
                foreach (DataColumn dc in ObjESupplier.dtPositionscopy.Columns)
                {
                    if (ObjESupplier.dtPositionscopy.Columns[dc.ColumnName + "Check"] != null)
                    {
                        TableColumnNAmes.Add(dc.ColumnName);
                    }
                }

                foreach (DataRow drTemp in ObjESupplier.dtPositionscopy.Rows)
                {
                    List<double> SupplierPirces = new List<double>();
                    foreach (string s in TableColumnNAmes)
                    {
                        double DValue = drTemp[s] == DBNull.Value ? 0 : Math.Round(Convert.ToDouble(drTemp[s]), 3);
                        if (DValue > 0)
                            SupplierPirces.Add(DValue);
                    }
                    if (SupplierPirces != null && SupplierPirces.Count() > 0)
                    {
                        var i = Math.Round(SupplierPirces.Min(), 3);
                        drTemp["Cheapest"] = i;
                    }
                    else
                        drTemp["Cheapest"] = 0;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjESupplier;
        }

        /// <summary>
        /// Code to change the cutlure of a datatable
        /// </summary>
        /// <param name="table"></param>
        private void ChangeCultureInfo(DataTable table)
        {
            CultureInfo myCultureInfo = new CultureInfo("en-gb");
            table.Locale = myCultureInfo;
        }

        /// <summary>
        /// Code to fetch Supplier details along with EMails
        /// </summary>
        /// <param name="ObjEsupplier"></param>
        /// <param name="ProposalID"></param>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public ESupplier GetSupplierMail(ESupplier ObjEsupplier, int ProposalID, int ProjectID)
        {
            try
            {
                ObjEsupplier = ObjDSupplier.GetSupplierMail(ObjEsupplier, ProposalID, ProjectID);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEsupplier;
        }

        /// <summary>
        /// Code to chcek supplier and article mapping
        /// </summary>
        /// <param name="ObjESupplier"></param>
        /// <returns></returns>
        public ESupplier CheckSupplierArticle(ESupplier ObjESupplier)
        {
            try
            {
                ObjESupplier = ObjDSupplier.CheckSupplierArticle(ObjESupplier);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjESupplier;
        }

        /// <summary>
        /// Code to add new supplier to existing proposal
        /// </summary>
        /// <param name="ObjESupplier"></param>
        /// <returns></returns>
        public ESupplier UpdateSupplierProposal(ESupplier ObjESupplier)
        {
            try
            {
                if (ObjDSupplier == null)
                    ObjDSupplier = new DSupplier();
                ObjESupplier = ObjDSupplier.UpdateSupplierProposal(ObjESupplier);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjESupplier;
        }

        /// <summary>
        /// Code to Delete Supplier and article mapping
        /// </summary>
        /// <param name="ObjESupplier"></param>
        /// <returns></returns>
        public ESupplier DeleteSupplierArticleMap(ESupplier ObjESupplier)
        {
            try
            {
                if (ObjDSupplier == null)
                    ObjDSupplier = new DSupplier();
                ObjESupplier = ObjDSupplier.DeleteSupplierArticleMap(ObjESupplier);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjESupplier;
        }

        /// <summary>
        /// Code to Update proposal date to supplier proposal after sending to Supplier
        /// </summary>
        /// <param name="ObjESupplier"></param>
        /// <returns></returns>
        public ESupplier UpdateProposalDate(ESupplier ObjESupplier)
        {
            try
            {
                if (ObjDSupplier == null)
                    ObjDSupplier = new DSupplier();
                ObjESupplier = ObjDSupplier.UpdateProposalDate(ObjESupplier);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjESupplier;
        }

        /// <summary>
        /// Code to delete suppliers from supplier proposal
        /// </summary>
        /// <param name="ObjESupplier"></param>
        /// <returns></returns>
        public ESupplier DeleteSuipplierProposal(ESupplier ObjESupplier)
        {
            try
            {
                ObjDSupplier.DeleteSuipplierProposal(ObjESupplier);
            }
            catch (Exception ex){ throw; }
            return ObjESupplier;
        }

        /// <summary>
        /// Code to fetch suppliers based on articles while adding new supplier to existing proposal
        /// </summary>
        /// <param name="ObjESupplier"></param>
        /// <returns></returns>
        public ESupplier GetSuppliersForProposal(ESupplier ObjESupplier)
        {
            try
            {
                if (ObjDSupplier == null)
                    ObjDSupplier = new DSupplier();
                ObjDSupplier.GetSuppliersForProposal(ObjESupplier);
            }
            catch (Exception ex){throw ex;}
            return ObjESupplier;
        }

        /// <summary>
        /// Code to fetch suppliers while merging new article with existing proposal
        /// </summary>
        /// <param name="ObjESupplier"></param>
        /// <returns></returns>
        public ESupplier GetSuppliersForProposalMerge(ESupplier ObjESupplier)
        {
            try
            {
                if (ObjDSupplier == null)
                    ObjDSupplier = new DSupplier();
                ObjDSupplier.GetSuppliersForProposalMerge(ObjESupplier);
            }
            catch (Exception ex) { throw ex; }
            return ObjESupplier;
        }

        /// <summary>
        /// Code to save new article with existsing proposal
        /// </summary>
        /// <param name="ObjESupplier"></param>
        /// <returns></returns>
        public ESupplier SupplierProposalMerge(ESupplier ObjESupplier)
        {
            try
            {
                if (ObjDSupplier == null)
                    ObjDSupplier = new DSupplier();
                ObjDSupplier.SupplierProposalMerge(ObjESupplier);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ObjESupplier;
        }

        /// <summary>
        /// Code to get import Datanorm file to database
        /// </summary>
        /// <param name="SupplierID"></param>
        /// <param name="ValidityDate"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataTable ImportDataNorm(object SupplierID, object ValidityDate, DataTable dt)
        {
            DataTable dtreturn = null;
            try
            {
                if (ObjDSupplier == null)
                    ObjDSupplier = new DSupplier();
                dtreturn = ObjDSupplier.ImportDataNorm(SupplierID, ValidityDate, dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtreturn;
        }

        /// <summary>
        /// Code to validate datanorm before importing into database
        /// </summary>
        /// <param name="SupplierID"></param>
        /// <param name="ValidityDate"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataSet ValidateDataNorm(object SupplierID, object ValidityDate, DataTable dt)
        {
            DataSet dtreturn = null;
            try
            {
                if (ObjDSupplier == null)
                    ObjDSupplier = new DSupplier();
                dtreturn = ObjDSupplier.ValidateDataNorm(SupplierID, ValidityDate, dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtreturn;
        }
    }
}

