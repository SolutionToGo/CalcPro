using DataAccess;
using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BL
{
    public class BProposal
    {
        DProposal ObjDProposal = new DProposal();

        /// <summary>
        /// Code to save Text areas which are used in Title blatt template design
        /// </summary>
        /// <param name="ObjEProposal"></param>
        /// <returns></returns>
        public int SaveTextModule(EProposal ObjEProposal)
        {
            try
            {
                int TextID = -1;
                XmlDocument Xdoc = new XmlDocument();
                string XPath = "/Nouns/TextModule";
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "TextID", ObjEProposal.TextID.ToString());
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "TextModuleArea", ObjEProposal.TextModuleArea);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "Category", ObjEProposal.Category);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "Contents", ObjEProposal.Contents);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "IsSelect", ObjEProposal.IsSelect.ToString());
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "TextAreaID", ObjEProposal.TextAreaID.ToString());
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "CategoryID", ObjEProposal.TextCategoryID.ToString());

                TextID = ObjDProposal.SaveTextModuleDetails(Xdoc);
                if (TextID < 0)
                {
                    if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                    {
                        //  throw new Exception("Fehler beim Speichern der LV Angaben");
                    }
                    else
                    {
                        throw new Exception("Failed to Save Text Module");
                    }
                }
                return TextID;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Code to save Text Modules Category
        /// </summary>
        /// <param name="ObjEProposal"></param>
        /// <param name="_textID"></param>
        /// <returns></returns>
        public int SaveCategory(EProposal ObjEProposal,int _textID)
        {
            try
            {
                int CategoryID = -1;
                XmlDocument Xdoc = new XmlDocument();
                string XPath = "/Nouns/Category";
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "CategoryID", ObjEProposal.CategoryID.ToString());
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "CategoryName", ObjEProposal.CategoryName);
                Xdoc = XMLBuilder.XmlConstruct(Xdoc, XPath, "TextAreaID", _textID.ToString());

                CategoryID = ObjDProposal.SaveCategory(Xdoc);
                if (CategoryID < 0)
                {
                    if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                    {
                      //  throw new Exception("Fehler beim Speichern der Kundeninformation");
                    }
                    else
                    {
                        throw new Exception("Failed to Save Category");
                    }
                }
                return CategoryID;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Code to fetch text areas to show in a grid contol
        /// </summary>
        /// <param name="ObjEProposal"></param>
        public void GetTextModuleAreas(EProposal ObjEProposal)
        {
            try
            {
                if (ObjEProposal != null)
                {
                    ObjEProposal.dsTextModuleAreas = ObjDProposal.GetTextModuleAreas();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Code to get text areas categories
        /// </summary>
        /// <param name="ObjEProposal"></param>
        /// <param name="_textID"></param>
        public void GetCategories(EProposal ObjEProposal,int _textID)
        {
            try
            {
                if (ObjEProposal != null)
                {
                    ObjEProposal.dsCategory = ObjDProposal.GetCategories(_textID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Code to delete text areas
        /// </summary>
        /// <param name="ObjEProposal"></param>
        /// <param name="ID"></param>
        public void DeleteTextModuleAreas(EProposal ObjEProposal,int ID)
        {
            try
            {
                if (ObjEProposal != null)
                {
                    ObjEProposal.dsTextModuleAreas = ObjDProposal.DeleteTextModuleAreas(ObjEProposal,ID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
