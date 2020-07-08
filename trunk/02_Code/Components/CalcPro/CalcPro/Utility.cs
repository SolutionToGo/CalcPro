using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using EL;
using GKSRVUtility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace CalcPro
{
    class Utility
    {
        /// <summary>
        /// This is a utility class to hold static functions nad variables
        /// </summary>

        #region Functions
        /// <summary>
        /// Code to validate controls on any form
        /// </summary>
        /// <param name="requiredFields"></param>
        /// <returns></returns>
        public static bool ValidateRequiredFields(List<Control> requiredFields)
        {
            
            bool IsValid = true;
            Control ctrlToFocus = null;
            StringBuilder sb = new StringBuilder();

             if(Utility._IsGermany == true)
                 sb.AppendLine("Bitte machen Sie folgene Pflichtangaben");
                    else
                        sb.AppendLine("Please enter the following Values");


            
            sb.AppendLine();
            foreach (Control ctrl in requiredFields)
            {
                if (ctrl is TextEdit && ctrl.Text.Trim() == string.Empty)
                {
                    if (ctrl.Tag == "LV-Status")
                    {
                        return IsValid;
                    }
                    IsValid = false;
                    sb.AppendLine(" * " + ctrl.Tag);
                    if (ctrlToFocus == null)
                        ctrlToFocus = ctrl;
                }
                else if (ctrl is System.Windows.Forms.ComboBox && ((System.Windows.Forms.ComboBox)ctrl).SelectedItem == null)
                {
                    IsValid = false;
                    sb.AppendLine(" * " + ctrl.Tag);
                    if (ctrlToFocus == null)
                        ctrlToFocus = ctrl;
                }
                else if (ctrl is System.Windows.Forms.ComboBox && ((System.Windows.Forms.ComboBox)ctrl).SelectedText.ToString() == "Select")
                {
                    IsValid = false;
                    sb.AppendLine(" * " + ctrl.Tag);
                    if (ctrlToFocus == null)
                        ctrlToFocus = ctrl;
                }
            }
            if (!IsValid)
            {
                XtraMessageBox.Show(sb.ToString(), "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ctrlToFocus.Focus();
            }
            return IsValid;
        }

        /// <summary>
        /// Code to show error message to user
        /// </summary>
        /// <param name="ex"></param>
        public static void ShowError(Exception ex)
        {
            try
            {
                XtraMessageBox.Show(ex.Message, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex1){}
        }

        /// <summary>
        /// Code to show success message to user
        /// </summary>
        /// <param name="Status"></param>
        public static void ShowSucces(string Status)
        {
            XtraMessageBox.Show(Status, "abgeschlossen", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        /// <summary>
        ///  Code to convert a xml file to GAEB abd Vice versa
        /// </summary>
        /// <param name="strInputFile"></param>
        /// <param name="strOututFile"></param>
        public static void ProcesssFile(string strInputFile, string strOututFile)
        {
            try
            {
                string appPath = Path.GetDirectoryName(Application.ExecutablePath);
                string ProductFilePath = appPath + ConfigurationManager.AppSettings["ProductFilePath"].ToString();
                string ClientFilePath = appPath + ConfigurationManager.AppSettings["ClientFilePath"].ToString();
                string LicenseKey = ConfigurationManager.AppSettings["LicenseKey"].ToString();
                GKSRVApp ObjGKSRV = new GKSRVApp();
                ObjGKSRV.ProcessFile(strInputFile, strOututFile, ProductFilePath, ClientFilePath, LicenseKey);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Code to transaform Position OZ to its ratser format
        /// </summary>
        /// <param name="strOZ"></param>
        /// <param name="strRaster"></param>
        /// <returns></returns>
        public static string PrepareOZ(string strOZ, string strRaster)
        {
            string str = string.Empty;
            try
            {
                string[] strPOZ = strOZ.Split('.');
                string[] strPRaster = strRaster.Split('.');
                int Count = -1;
                int i = -1;
                Count = strPOZ.Count();
                while (Count > 0)
                {
                    i = i + 1;
                    Count = Count - 1;
                    string OZ = string.Empty;
                    int OZLength = 0;
                    int RasterLength = 0;

                    OZ = strPOZ[i].Trim();
                    RasterLength = strPRaster[i].Length;
                    
                    OZLength = OZ.Trim().Length;

                    if(Count == 0)
                    {
                       if(RasterLength == 1 && OZLength > 0)
                       {
                           str = str + string.Concat(Enumerable.Repeat("0", RasterLength - OZLength)) + OZ;
                       }
                       else if (OZLength > 0)
                       {
                           str = str + string.Concat(Enumerable.Repeat("0", RasterLength - OZLength)) + OZ + ".";
                       }                                  
                    }
                    else
                    {
                        if (OZ=="")
                        {
                            str = str + string.Concat(Enumerable.Repeat(" ", RasterLength - OZLength)) + OZ + ".";
                        }
                        else
                        {
                            str = str + string.Concat(Enumerable.Repeat("0", RasterLength - OZLength)) + OZ + ".";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Bitte geben Sie eine gültige Ordnungskennzahl ein");
            }
            return str;
        }

       /// <summary>
       /// Code to set focus on grid control's row based on column name and keyvalue
       /// </summary>
       /// <param name="view"></param>
       /// <param name="_id"></param>
       /// <param name="_IdValue"></param>
        public static void Setfocus(GridView view, string _id, int _IdValue)
        {
            try
            {
                if (_IdValue > -1)
                {
                    int rowHandle = view.LocateByValue(_id, _IdValue);
                    if (rowHandle != DevExpress.XtraGrid.GridControl.InvalidRowHandle)
                        view.FocusedRowHandle = rowHandle;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Code to generate encypted text from a plain text
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string EncryptInner(string input)
        {
            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(input)));
        }

        /// <summary>
        /// Code to generate encypted text from a plain text
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private byte[] Encrypt(byte[] input)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes("CategisOTTO", new byte[] { 0x43, 0x87, 0x23, 0x72, 0x45, 0x56, 0x68, 0x14, 0x62, 0x84 });
            MemoryStream ms = new MemoryStream();
            Aes aes = new AesManaged();
            aes.Key = pdb.GetBytes(aes.KeySize / 8);
            aes.IV = pdb.GetBytes(aes.BlockSize / 8);
            CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(input, 0, input.Length);
            cs.Close();
            return ms.ToArray();
        }

        /// <summary>
        /// Code to generate Plain text from encrypted text
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Decrypt(string input)
        {
            return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(input)));
        }

        /// <summary>
        /// Code to generate Plain text from encrypted text
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static byte[] Decrypt(byte[] input)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes("CategisOTTO", new byte[] { 0x43, 0x87, 0x23, 0x72, 0x45, 0x56, 0x68, 0x14, 0x62, 0x84 });
            MemoryStream ms = new MemoryStream();
            Aes aes = new AesManaged();
            aes.Key = pdb.GetBytes(aes.KeySize / 8);
            aes.IV = pdb.GetBytes(aes.BlockSize / 8);
            CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(input, 0, input.Length);
            cs.Close();
            return ms.ToArray();
        }

        /// <summary>
        ///  Global method exposed to generate encrypted text form plain text
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Encrypt(string input)
        {
            Utility sec = new Utility();
            return sec.EncryptInner(input);
        }

        /// <summary>
        /// Code to convert plaintext to richtext format
        /// </summary>
        /// <param name="Plaintext"></param>
        /// <returns></returns>
        public static string GetRTFFormat(string Plaintext)
        {
            string st = string.Empty;
            try
            {
                using (RichTextBox rtf = new RichTextBox())
                {
                    rtf.Text = Plaintext;
                    st = rtf.Rtf;
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                    throw new Exception("Fehler beim Datenimport des Langtext");
                else
                    throw new Exception("Error while importing langtext");
            }
            return st;
        }

        /// <summary>
        /// Code to convert RTF text to plain text
        /// </summary>
        /// <param name="RTFText"></param>
        /// <returns></returns>
        public static string GetPlaintext(string RTFText)
        {

            string st = string.Empty;
            try
            {
                using (RichTextBox rtf = new RichTextBox())
                {
                    rtf.Rtf = RTFText;
                    st = rtf.Text;
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                    throw new Exception("Fehler beim Datenimport des Langtext");
                else
                    throw new Exception("Error while generating plain text");
            }
            return st;
        }

        /// <summary>
        /// Code to create dataset of positions from XML file while  importing GAEB
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="strLVSection"></param>
        /// <param name="Raster"></param>
        /// <param name="ObjEGAEB"></param>
        /// <returns></returns>
        public static DataSet CreateDatasetSchema(string strFilePath, string strLVSection, string Raster, EGAEB ObjEGAEB)
        {
            DataSet dsXmlData = new DataSet("Generic");
            try
            {
                string stFromOZ = string.Empty;
                string stToOZ = string.Empty;

                DataTable dtV = new DataTable("LVPos");
                dtV.Columns.Add("Art", typeof(string));
                dtV.Columns.Add("OZ", typeof(string));
                dtV.Columns.Add("Menge", typeof(decimal));
                dtV.Columns.Add("Einheit", typeof(string));
                dtV.Columns.Add("Kurztext", typeof(string));
                dtV.Columns.Add("Langtext", typeof(string));
                dtV.Columns.Add("EP", typeof(decimal));
                dtV.Columns.Add("GB", typeof(decimal));
                dtV.Columns.Add("Nachlass", typeof(string));
                dtV.Columns.Add("ParentOz", typeof(string));
                dtV.Columns.Add("Title", typeof(string));
                dtV.Columns.Add("SNO", typeof(int));
                dtV.Columns.Add("BezugBeschr", typeof(string));
                dtV.Columns.Add("BezugAusfNr", typeof(string));
                dtV.Columns.Add("Bedarf", typeof(string));
                dtV.Columns.Add("Nr", typeof(string));
                dtV.Columns.Add("BezugOZ", typeof(string));
                dtV.Columns.Add("OZID", typeof(decimal));
                dtV.Columns.Add("OZ1", typeof(string));
                dtV.Columns.Add("OZ2", typeof(string));
                dtV.Columns.Add("OZ3", typeof(string));
                dtV.Columns.Add("OZ4", typeof(string));
                dtV.Columns.Add("OZ5", typeof(string));
                dtV.Columns.Add("OZ6", typeof(string));
                dtV.Columns.Add("Bezuschl", typeof(string));
                dtV.Columns.Add("FromOZ", typeof(string));
                dtV.Columns.Add("ToOZ", typeof(string));
                dtV.Columns.Add("SurchargeSno", typeof(int));
                dsXmlData.Tables.Add(dtV);

                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(strFilePath);
                IDataObject obj = Clipboard.GetDataObject();
                XmlNodeList xnProjectInfo = xDoc.GetElementsByTagName("PrjInfo");
                foreach (XmlNode xn in xnProjectInfo)
                {
                    XmlNode xnPDescription = xn.SelectSingleNode("Bez");
                    if (xnPDescription != null)
                        ObjEGAEB.ProjectDescription = xnPDescription.InnerText;
                    else
                        ObjEGAEB.ProjectDescription = string.Empty;
                }

                XmlNodeList xnAGInfo = xDoc.GetElementsByTagName("AG");
                foreach (XmlNode xn in xnProjectInfo)
                {
                    XmlNode xnCustomerName = xn.SelectSingleNode("Name1");
                    if (xnCustomerName != null)
                        ObjEGAEB.CustomerName = xnCustomerName.InnerText;
                    else
                        ObjEGAEB.CustomerName = string.Empty;
                }

                XmlNodeList xnLVPos = xDoc.GetElementsByTagName("LVPos");
                string strART = string.Empty;
                int iSNO = 0;
                DataRow drLVPos = null;
                foreach (XmlNode xnPos in xnLVPos)
                {
                    iSNO++;
                    drLVPos = dtV.NewRow();
                    XmlNode xnArt = xnPos.SelectSingleNode("Art");
                    if (xnArt != null)
                    {
                        drLVPos["Art"] = strART = xnArt.InnerText;
                    }

                    XmlNode xnOZ = xnPos.SelectSingleNode("OZ");
                    string stOZ = string.Empty;
                    if (xnOZ != null)
                    {
                        drLVPos["OZ"] = stOZ = PrepareOZ(xnOZ.InnerText, Raster);
                        decimal OZID = 0;
                        if (!string.IsNullOrEmpty(stOZ))
                        {
                            string[] strOZList = stOZ.Split('.');
                            if (strOZList.Count() > 1)
                            {
                                string strOZID = strOZList[strOZList.Count() - 2];
                                string strIndex = strOZList[strOZList.Count() - 1];
                                if (!decimal.TryParse(strOZID + "." + strIndex, out OZID))
                                    OZID = 0;
                            }
                        }
                        drLVPos["OZID"] = OZID;

                        string OZ1 = string.Empty, OZ2 = string.Empty, OZ3 = string.Empty, OZ4 = string.Empty, OZ5 = string.Empty, OZ6 = string.Empty;
                        if (!string.IsNullOrEmpty(stOZ))
                        {
                            string[] strOZList = stOZ.Split('.');
                            if (strOZList.Count() > 1)
                            {
                                string strOZID = strOZList[strOZList.Count() - 2];
                                string strIndex = strOZList[strOZList.Count() - 1];
                                char[] OZcharList = strOZID.ToCharArray();
                                int CharCount = OZcharList.Count();
                                if (CharCount > 0)
                                {
                                    OZ1 = Convert.ToString(OZcharList[0]);
                                    if (CharCount > 1)
                                    {
                                        OZ2 = Convert.ToString(OZcharList[1]);
                                        if (CharCount > 2)
                                        {
                                            OZ3 = Convert.ToString(OZcharList[2]);
                                            if (CharCount > 3)
                                            {
                                                OZ4 = Convert.ToString(OZcharList[3]);
                                                if (CharCount > 4)
                                                {
                                                    OZ5 = Convert.ToString(OZcharList[4]);
                                                    OZ6 = strIndex;
                                                }
                                                else
                                                    OZ5 = strIndex;
                                            }
                                            else
                                                OZ4 = strIndex;
                                        }
                                        else
                                            OZ3 = strIndex;
                                    }
                                    else
                                        OZ2 = strIndex;
                                }
                            }
                        }
                        drLVPos["OZ1"] = OZ1;
                        drLVPos["OZ2"] = OZ2;
                        drLVPos["OZ3"] = OZ3;
                        drLVPos["OZ4"] = OZ4;
                        drLVPos["OZ5"] = OZ5;
                        drLVPos["OZ6"] = OZ6;
                    }

                    XmlNode xnMenge = xnPos.SelectSingleNode("Menge");
                    if (xnMenge != null)
                    {
                        decimal dValue = 0;
                        if (decimal.TryParse(xnMenge.InnerText, out dValue))
                            drLVPos["Menge"] = dValue;

                    }
                    XmlNode xnEinheit = xnPos.SelectSingleNode("Einheit");
                    if (xnEinheit != null)
                        drLVPos["Einheit"] = xnEinheit.InnerText;

                    XmlNode xnBezugBeschr = xnPos.SelectSingleNode("BezugBeschr");
                    if (xnBezugBeschr != null)
                        drLVPos["BezugBeschr"] = xnBezugBeschr.InnerText;

                    XmlNode xnBezugAusfNr = xnPos.SelectSingleNode("BezugAusfNr");
                    if (xnBezugAusfNr != null)
                        drLVPos["BezugAusfNr"] = xnBezugAusfNr.InnerText;

                    XmlNode xnBedarf = xnPos.SelectSingleNode("Bedarf");
                    if (xnBedarf != null)
                        drLVPos["Bedarf"] = xnBedarf.InnerText;

                    XmlNode xnNr = xnPos.SelectSingleNode("Nr");
                    if (xnNr != null)
                        drLVPos["Nr"] = xnNr.InnerText;

                    XmlNode xnBezugOZ = xnPos.SelectSingleNode("BezugOZ");
                    if (xnBezugOZ != null)
                        drLVPos["BezugOZ"] = xnBezugOZ.InnerText;

                    XmlNode xnBezuschl = xnPos.SelectSingleNode("Bezuschl");
                    string stBezuschl = string.Empty;
                    if (xnBezuschl != null)
                        drLVPos["Bezuschl"] = stBezuschl = xnBezuschl.InnerText;

                    if (!string.IsNullOrEmpty(stBezuschl))
                    {
                        if (string.IsNullOrEmpty(stFromOZ))
                            stFromOZ = stToOZ = stOZ;
                        else
                            stToOZ = stOZ;
                    }

                    if(strART == "Z")
                    {
                        drLVPos["FromOZ"] = stFromOZ;
                        drLVPos["ToOZ"] = stToOZ;
                        stFromOZ = string.Empty;
                        stToOZ = string.Empty;
                        DataRow[] drfind  = dtV.Select("Bezuschl = 'J' AND ISNULL(SurchargeSno,0) = 0");
                        foreach (DataRow dr in drfind)
                            dr["SurchargeSno"] = iSNO;
                    }
                    try
                    {
                        XmlNode xnKurztext = xnPos.SelectSingleNode("Kurztext");
                        if (xnKurztext != null)
                        {
                            string stKurztext = xnKurztext.InnerText;
                            if (IsRtfText(xnKurztext.InnerText))
                            {
                                drLVPos["Kurztext"] = xnKurztext.InnerText;
                                if (strART.ToLower() == "ng")
                                    drLVPos["Title"] = GetPlaintext(xnKurztext.InnerText);
                            }
                            else if (!string.IsNullOrEmpty(stKurztext))
                            {
                                drLVPos["Kurztext"] = GetRTFFormat(xnKurztext.InnerText);
                                if (strART.ToLower() == "ng")
                                    drLVPos["Title"] = xnKurztext.InnerText;
                            }
                            else
                            {
                                drLVPos["Kurztext"] = string.Empty;
                                if (strART.ToLower() == "ng")
                                    drLVPos["Title"] = string.Empty;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Utility.ShowError(ex);
                    }

                    try
                    {
                        XmlNode xnLangtext = xnPos.SelectSingleNode("Langtext");
                        if (xnLangtext != null)
                        {
                            XmlNodeList xnlist = xnPos.SelectNodes("ExtDat/ExtDatName");
                            if (xnlist != null && xnlist.Count > 0)
                            {
                                string strTemp = string.Empty;
                                using (RichTextBox txt = new RichTextBox())
                                {
                                    if (IsRtfText(xnLangtext.InnerText))
                                        strTemp = GetPlaintext(xnLangtext.InnerText);
                                    else
                                        strTemp = xnLangtext.InnerText;

                                    if (!string.IsNullOrEmpty(strTemp))
                                    {
                                        Clipboard.SetText(strTemp);
                                        txt.Paste();
                                        Clipboard.Clear();
                                    }
                                    foreach (XmlNode xnExtdat in xnlist)
                                    {
                                        string strImage = xnExtdat.InnerText;
                                        Image img = Image.FromFile(strImage);
                                        Clipboard.SetImage(img);
                                        txt.Paste();
                                        Clipboard.Clear();
                                    }
                                    drLVPos["Langtext"] = txt.Rtf;
                                    if (string.IsNullOrEmpty(Utility.GetPlaintext(Convert.ToString(drLVPos["Kurztext"]))))
                                    {
                                        if (strTemp.Length > 80)
                                        {
                                            drLVPos["Kurztext"] = Utility.GetRTFFormat(strTemp.Substring(0, 81));
                                            if (strART.ToLower() == "ng")
                                                drLVPos["Title"] = strTemp.Substring(0, 80);
                                        }
                                        else
                                        {
                                            drLVPos["Kurztext"] = Utility.GetRTFFormat(strTemp);
                                            drLVPos["Title"] = strTemp;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (IsRtfText(xnLangtext.InnerText))
                                {
                                    drLVPos["Langtext"] = xnLangtext.InnerText;
                                    string stTemp = Utility.GetPlaintext(xnLangtext.InnerText);
                                    if (string.IsNullOrEmpty(Utility.GetPlaintext(Convert.ToString(drLVPos["Kurztext"]))))
                                    {
                                        if (stTemp.Length > 80)
                                        {
                                            drLVPos["Kurztext"] = Utility.GetRTFFormat(stTemp.Substring(0, 81));
                                            if (strART.ToLower() == "ng")
                                                drLVPos["Title"] = stTemp.Substring(0, 80);
                                        }
                                        else
                                        {
                                            drLVPos["Kurztext"] = Utility.GetRTFFormat(stTemp);
                                            if (strART.ToLower() == "ng")
                                                drLVPos["Title"] = stTemp;
                                        }
                                    }
                                }
                                else
                                {
                                    drLVPos["Langtext"] = GetRTFFormat(xnLangtext.InnerText);
                                    if (string.IsNullOrEmpty(Utility.GetPlaintext(Convert.ToString(drLVPos["Kurztext"]))))
                                    {
                                        if (xnLangtext.InnerText.Length > 80)
                                        {
                                            drLVPos["Kurztext"] = Utility.GetRTFFormat(xnLangtext.InnerText.Substring(0, 81));
                                            if (strART.ToLower() == "ng")
                                                drLVPos["Title"] = xnLangtext.InnerText.Substring(0, 80);
                                        }
                                        else
                                        {
                                            drLVPos["Kurztext"] = Utility.GetRTFFormat(xnLangtext.InnerText);
                                            if (strART.ToLower() == "ng")
                                                drLVPos["Title"] = xnLangtext.InnerText;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Utility.ShowError(ex);
                    }

                    XmlNode xnEP = xnPos.SelectSingleNode("EP");
                    if (xnEP != null)
                    {
                        decimal dValue = 0;
                        if (decimal.TryParse(xnEP.InnerText, out dValue))
                            drLVPos["EP"] = dValue;
                        else
                            drLVPos["EP"] = 0;
                    }
                    XmlNode xnGB = xnPos.SelectSingleNode("GB");
                    if (xnGB != null)
                    {
                        decimal dValueDB = 0;
                        if (decimal.TryParse(xnGB.InnerText, out dValueDB))
                            drLVPos["GB"] = dValueDB;
                        else
                            drLVPos["GB"] = 0;
                    }
                    XmlNode xnNachlass = xnPos.SelectSingleNode("Nachlass");
                    if (strLVSection != string.Empty)
                        drLVPos["Nachlass"] = strLVSection;
                    else if (xnNachlass != null)
                    {
                        string str = xnNachlass.InnerText;
                        if (string.IsNullOrEmpty(str))
                            drLVPos["Nachlass"] = "HA";
                        else
                            drLVPos["Nachlass"] = str;
                    }

                    drLVPos["SNO"] = iSNO.ToString();
                    dtV.Rows.Add(drLVPos);
                }
                Clipboard.SetDataObject(obj);
                foreach (DataRow dr in dsXmlData.Tables[0].Rows)
                {
                    string strPositionOZ = dr["OZ"].ToString();
                    if (strPositionOZ != string.Empty)
                    {
                        dr["ParentOz"] = GetParentOZ(strPositionOZ);
                    }
                    else
                    {
                        string strSNO = dr["SNO"].ToString();
                        string MaxValue = dsXmlData.Tables[0].Compute("MIN(SNO)", "SNO >'" + strSNO + "'AND OZ <> ''").ToString();
                        if (MaxValue != string.Empty)
                        {
                            DataRow[] drNextPosition = dsXmlData.Tables[0].Select("SNO=" + MaxValue);
                            dr["ParentOz"] = GetParentOZ(drNextPosition[0]["OZ"].ToString());
                        }
                        else
                        {
                            string Value = dsXmlData.Tables[0].Compute("MAX(SNO)", "SNO <'" + strSNO + "'AND OZ <> ''").ToString();
                            DataRow[] drNextPosition = dsXmlData.Tables[0].Select("SNO=" + Value);
                            dr["ParentOz"] = GetParentOZ(drNextPosition[0]["OZ"].ToString()); 
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                {
                    throw new Exception("Beim Import der GAEB-Datei ist ein Fehler aufgetreten!");
                }
                else
                {
                    throw new Exception("Error While Importing GAEB File");
                }
            }
            return dsXmlData;
        }

        /// <summary>
        /// Code to extract raster from xml file while importing
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <returns></returns>
        public static string GetRaster(string strFilePath)
        {
            StringBuilder strRaster = new StringBuilder();
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(strFilePath);
                XmlNodeList xnLVInfo = xDoc.SelectNodes("/Generic/LVInfo");
                string strNew = string.Empty;
                foreach (XmlNode xn in xnLVInfo)
                {
                    XmlNode xnTyp1 = xn.SelectSingleNode("LVGliedTyp1");
                    if (xnTyp1 != null)
                    {
                        XmlNode xnLng1 = xn.SelectSingleNode("LVGliedLen1");
                        strNew = new string('9', Convert.ToInt32(xnLng1.InnerText));
                        strRaster.Append(strNew + ".");
                    }

                    XmlNode xnTyp2 = xn.SelectSingleNode("LVGliedTyp2");
                    if (xnTyp2 != null && xnTyp2.InnerText.ToLower() != "")
                    {
                        strNew = string.Empty;
                        XmlNode xnLng2 = xn.SelectSingleNode("LVGliedLen2");
                        if (xnTyp2.InnerText.ToLower() == "lvstufe")
                            strNew = new string('9', Convert.ToInt32(xnLng2.InnerText));
                        else if (xnTyp2.InnerText.ToLower() == "position")
                            strNew = new string('1', Convert.ToInt32(xnLng2.InnerText));
                        if (strNew != string.Empty)
                            strRaster.Append(strNew + ".");
                    }

                    XmlNode xnTyp3 = xn.SelectSingleNode("LVGliedTyp3");
                    if (xnTyp3 != null && xnTyp3.InnerText.ToLower() != "")
                    {
                        strNew = string.Empty;
                        XmlNode xnLng3 = xn.SelectSingleNode("LVGliedLen3");
                        if (xnTyp3.InnerText.ToLower() == "lvstufe")
                            strNew = new string('9', Convert.ToInt32(xnLng3.InnerText));
                        else if (xnTyp3.InnerText.ToLower() == "position")
                            strNew = new string('1', Convert.ToInt32(xnLng3.InnerText));
                        if (strNew != string.Empty)
                            strRaster.Append(strNew + ".");
                    }

                    XmlNode xnTyp4 = xn.SelectSingleNode("LVGliedTyp4");
                    if (xnTyp4 != null && xnTyp4.InnerText.ToLower() != "")
                    {
                        strNew = string.Empty;
                        XmlNode xnLng4 = xn.SelectSingleNode("LVGliedLen4");
                        if (xnTyp4.InnerText.ToLower() == "lvstufe")
                            strNew = new string('9', Convert.ToInt32(xnLng4.InnerText));
                        else if (xnTyp4.InnerText.ToLower() == "position")
                            strNew = new string('1', Convert.ToInt32(xnLng4.InnerText));
                        if (strNew != string.Empty)
                            strRaster.Append(strNew + ".");
                    }

                    XmlNode xnTyp5 = xn.SelectSingleNode("LVGliedTyp5");
                    if (xnTyp5 != null && xnTyp5.InnerText.ToLower() != "")
                    {
                        strNew = string.Empty;
                        XmlNode xnLng5 = xn.SelectSingleNode("LVGliedLen5");
                        if (xnTyp5.InnerText.ToLower() == "lvstufe")
                            strNew = new string('9', Convert.ToInt32(xnLng5.InnerText));
                        else if (xnTyp5.InnerText.ToLower() == "position")
                            strNew = new string('1', Convert.ToInt32(xnLng5.InnerText));
                        if (strNew != string.Empty)
                            strRaster.Append(strNew + ".");
                    }
                    strRaster.Append("9");
                }
            }
            catch (Exception ex)
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString() == "de-DE")
                {
                    throw new Exception("Beim Import der GAEB-Datei ist ein Fehler aufgetreten!");
                }
                else
                {
                    throw new Exception("Error While Importing GAEB File");
                }
            }
            return strRaster.ToString();
        }

        /// <summary>
        /// Code to check weather file open or not
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool fileIsOpen(string path)
        {
            System.IO.FileStream a = null;
            try
            {
                a = System.IO.File.Open(path,
                System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None);
                return false;
            }
            catch (System.IO.IOException ex)
            {
                return true;
            }
            finally
            {
                if (a != null)
                {
                    a.Close();
                    a.Dispose();
                }
            }
        }

        /// <summary>
        /// Code to read excel file while transfering the data from CalcPro to OTTO Projects
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileExt"></param>
        /// <returns></returns>
        public static DataTable ReadExcel(string fileName, string fileExt)
        {
            string conn = string.Empty;
            DataTable dtexcel = new DataTable();
            if (fileExt.CompareTo(".xls") == 0)
                conn = @"provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=1';"; //for below excel 2007  
            else
                conn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties='Excel 12.0;HDR=NO';"; //for above excel 2007  
            using (OleDbConnection con = new OleDbConnection(conn))
            {
                try
                {
                    OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [Sheet1$]", con); //here we read data from sheet1  
                    oleAdpt.Fill(dtexcel); //fill excel data into dataTable  
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return dtexcel;
        }

        /// <summary>
        /// Code to Check input string is RTFtext or not
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsRtfText(string text)
        {
            bool _ISRTF = false;
            try
            {
                if (text.TrimStart().StartsWith("{\\rtf1", StringComparison.Ordinal))
                {
                    if (IsValidRtf(text))
                        _ISRTF = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _ISRTF;
        }

        /// <summary>
        /// Code to check Input string is RTF or not
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static bool IsValidRtf(string text)
        {
            try
            {
                using (RichTextBox rtf = new RichTextBox())
                {
                    rtf.Rtf = text;
                }
            }
            catch (ArgumentException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

       /// <summary>
       /// Code to extract ParentOZ from Position OZ
       /// </summary>
       /// <param name="strPositionOZ"></param>
       /// <returns></returns>
        private static string GetParentOZ(string strPositionOZ)
        {
            string[] strOZ = strPositionOZ.Split('.');
            StringBuilder strParentOZ = new StringBuilder();
            try
            {
                if (strOZ != null && strOZ.Count() > 2)
                {
                    if (strOZ.Count() > 2)
                    {
                        strParentOZ.Append(strOZ[0] + ".");
                        if (strOZ.Count() > 3)
                        {
                            if (!string.IsNullOrEmpty(strOZ[1].Trim()))
                                strParentOZ.Append(strOZ[1] + ".");
                            if (strOZ.Count() > 4)
                            {
                                if (!string.IsNullOrEmpty(strOZ[2].Trim()))
                                    strParentOZ.Append(strOZ[2] + ".");
                                if (strOZ.Count() > 5 && !string.IsNullOrEmpty(strOZ[3].Trim()))
                                {
                                    strParentOZ.Append(strOZ[3] + ".");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return strParentOZ.ToString();
        }

        /// <summary>
        /// Code to get Position KZ description based on position KZ Character
        /// </summary>
        /// <param name="stPKZ"></param>
        /// <returns></returns>
        public static string GetKZDescription(string stPKZ)
        {
            string stDescription = string.Empty;
            try
            {
                if (stPKZ == "N")
                    stDescription = "N-Normalposition";
                else if (stPKZ == "ZS")
                    stDescription = "ZS-Sum Position";
                else if (stPKZ == "Z")
                    stDescription = "Z-Zuschlagsposition";
                else if (stPKZ == "E")
                    stDescription = "E-Bedarfspos. o. GB";
                else if (stPKZ == "A")
                    stDescription = "A-Alternativposition";
                else if (stPKZ == "M")
                    stDescription = "M-Bedarfspos. m. GB";
                else if (stPKZ == "P")
                    stDescription = "P-Pauschalposition";
                else if (stPKZ == "H")
                    stDescription = "H-Hinweistext";
                else if (stPKZ == "NG")
                    stDescription = "NG-LV-Normalgruppe";
                else if (stPKZ == "K")
                    stDescription = "K-Kombiposition";
                else if (stPKZ == "G")
                    stDescription = "G-Grundposition";
                else if (stPKZ == "B")
                    stDescription = "B-Bezugsposition";
                else if (stPKZ == "W")
                    stDescription = "W-Wiederholungsposition";
                else if (stPKZ == "L")
                    stDescription = "L-Leitbeschreibung";
                else if (stPKZ == "S")
                    stDescription = "S-Stundenlohnarbeiten";
                else if (stPKZ == "AB")
                    stDescription = "AB-Ausführungsbeschreibung";
                else if (stPKZ == "BA")
                    stDescription = "BA-Block einer Ausführungsbeschreibung";
                else if (stPKZ == "UB")
                    stDescription = "UB-Unterbeschreibung";
                else if (stPKZ == "LO")
                    stDescription = "LO-Los";
                else if (stPKZ == "GG")
                    stDescription = "GG-LV-Grundgruppe";
                else if (stPKZ == "AG")
                    stDescription = "AG-LV-Alternativgruppe";
                else if (stPKZ == "VR")
                    stDescription = "VR-Vertragliche Regelung";
                else if (stPKZ == "ZZ")
                    stDescription = "ZZ-Zuschlagsposition";
            }
            catch (Exception ex){}
            return stDescription;
        }

        /// <summary>
        /// Code to get Position KZ Character based on description
        /// </summary>
        /// <param name="stDescription"></param>
        /// <returns></returns>
        public static string GetPosKZ(string stDescription)
        {
            string stPosKZ = string.Empty;
            try
            {
                string[] stPList = stDescription.Split('-');
                if (stPList != null && stPList.Count() > 0)
                    stPosKZ = stPList[0];
            }
            catch (Exception ex){}
            return stPosKZ;
        }

        /// <summary>
        /// Code to Set Combobox edit value
        /// </summary>
        /// <param name="cntrl"></param>
        /// <param name="stDisplayValue"></param>
        public static void SetCheckedComboexitValue(CheckedComboBoxEdit cntrl, string stDisplayValue)
        {
            try
            {
                DataTable dt = (DataTable)cntrl.Properties.DataSource;
                DataTable dttemp = dt.Copy();
                DataView dv = dttemp.DefaultView;
                dv.RowFilter = cntrl.Properties.DisplayMember +"= '" + stDisplayValue.Trim() + "'";
                dttemp = dv.ToTable();
                object obj = dttemp.Rows[0][cntrl.Properties.ValueMember];
                if (obj != null)
                    cntrl.SetEditValue(obj);
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// Code to Set lookup edit value
        /// </summary>
        /// <param name="cntrl"></param>
        /// <param name="stDisplayValue"></param>
        public static void SetLookupEditValue(LookUpEdit cntrl, string stDisplayValue)
        {
            try
            {
                DataRowView dr = (DataRowView)cntrl.Properties.GetDataSourceRowByDisplayValue(stDisplayValue);
                if (dr != null)
                    cntrl.EditValue = dr[cntrl.Properties.ValueMember];
            }
            catch (Exception ex) { }
        }

        #endregion

        #region Variables
        public static bool _IsGermany = false;
        public static int UserID;
        public static string UserName;
        public static string FirstName;
        public static string LastName;
        public static bool IsOTP;
        public static int RoleID;
        public static bool AutoSave;

        public static string LVDetailsAccess = string.Empty;
        public static string CalcAccess = string.Empty;
        public static string ArticleDataAccess = string.Empty;
        public static string CustomerDataAccess = string.Empty;
        public static string OTTODataAccess = string.Empty;
        public static string SupplierDataAccess = string.Empty;
        public static string KomissionDataAccess = string.Empty;
        public static string DeliveryAccess = string.Empty;
        public static string InvoiceAccess = string.Empty;
        public static string LVsectionAddAccess = string.Empty;
        public static string LVSectionEditAccess = string.Empty;
        public static string UserDataAccess = string.Empty;
        public static string ProjectDataAccess = string.Empty;
        public static string GeneralTextModuleAccess = string.Empty;
        public static string CalculationTextModuleAccess = string.Empty;
        public static string CalculationTextModuleAccessEdit = string.Empty;
        public static string InvoiceTextModuleAccess = string.Empty;
        public static string InvoiceTextModuleAccessEdit = string.Empty;
        public static string FormBlattArticleMappingAccess = string.Empty;
        public static bool Isclose = false;
        public static string DBVersion = string.Empty;
        //public static string Appversion = "9.5.9 - PROD";
        public static string Appversion = "9.5.9 - UAT";
        //public static string Appversion = "9.5.9 - QA";
        public static string VersionDate = " (18.06.2020)";
        public static DataTable dtLVStatus = null;
        public static DataTable dtPositionKZ = null;
        #endregion
    }
}