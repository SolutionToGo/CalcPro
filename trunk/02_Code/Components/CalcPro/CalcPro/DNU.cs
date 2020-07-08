using dcm.Trinity.Definition;
using dcm.Trinity.SDK;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CalcPro
{
    public static class DNU
    {
        public static string FileVerison = string.Empty;

        /// <summary>
        /// Code to proccess file data norm file
        /// </summary>
        /// <param name="SourceFilePath"></param>
        /// <param name="AppDataPath"></param>
        /// <param name="ApplicationFolderPath"></param>
        /// <returns></returns>
        public static DataTable ProccessFile(string SourceFilePath,string AppDataPath,string ApplicationFolderPath)
        {
            DataTable dtOutPut = new DataTable();
            try
            {
                DataSet ds = new DataSet();
                string FileName = Path.GetFileNameWithoutExtension(SourceFilePath);
                var resultFile = AppDataPath + "\\" + FileName + ".xml";
                //Validating Datanorm file
                var adapterType = InferAdapterFromFile(SourceFilePath);
                var factory = new AdapterFactory(ApplicationFolderPath);
                //var factory = new AdapterFactory(@"C:\Program Files (x86)\datacrossmedia\trinity6\moduls");
                var adapter = factory.CreateAdapter(adapterType);
                //Authenticate license
                AuthorizeAdapter(adapter);
                //Attaching Event Handlers
                AttachEventHandlers(adapter);
                //Proccessing file
                adapter.Run(SourceFilePath, resultFile);
                SplashScreenManager.Default.SetWaitFormDescription("Auslesen der XML Daten");
                ds.ReadXml(resultFile);
                DataTable dtArticle = new DataTable();
                DataTable dtPrice = new DataTable();
                foreach (DataTable dt in ds.Tables)
                {
                    if (dt.TableName == "Artikel")
                        dtArticle = dt;
                    else if (dt.TableName == "Artikelpreis")
                    {
                        dtPrice = dt.Clone();
                        CultureInfo myCultureInfo = new CultureInfo("en-gb");
                        dtPrice.Locale = myCultureInfo;
                        dtPrice.Columns["Preis"].DataType = System.Type.GetType("System.Decimal");
                        foreach (DataRow dr in dt.Rows)
                            dtPrice.ImportRow(dr);
                    }
                }

                var results = from table1 in dtArticle.AsEnumerable()
                              join table2 in dtPrice.AsEnumerable() on (int)table1["Artikel_Id"] equals (int)table2["Artikel_Id"]
                              select new
                              {
                                  ArticleID = table1["Artikel_Id"],
                                  Artikelnummer = table1["Artikelnummer"],
                                  Kurztext1 = table1["Kurztext1"],
                                  Kurztext2 = table1["Kurztext2"],
                                  Mengeneinheit = table1["Mengeneinheit"],
                                  Hauptwarengruppe = table1["Hauptwarengruppe"],
                                  Warengruppe = table1["Warengruppe"],
                                  Produktgruppe = table1["Produktgruppe"],
                                  Rabattgruppe = table1["Rabattgruppe"],
                                  Waehrung = table2["Waehrung"],
                                  Preis = table2["Preis"],
                              };

                dtOutPut.Columns.Add("Artikel_Id", typeof(int));
                dtOutPut.Columns.Add("Artikelnummer", typeof(string));
                dtOutPut.Columns.Add("Kurztext1", typeof(string));
                dtOutPut.Columns.Add("Kurztext2", typeof(string));
                dtOutPut.Columns.Add("WG", typeof(string));
                dtOutPut.Columns.Add("WA", typeof(string));
                dtOutPut.Columns.Add("WI", typeof(string));
                dtOutPut.Columns.Add("A", typeof(string));
                dtOutPut.Columns.Add("B", typeof(string));
                dtOutPut.Columns.Add("L", typeof(string));
                dtOutPut.Columns.Add("Preis", typeof(decimal));
                string[] Dimenstions = null;
                foreach (var item in results)
                {
                    Dimenstions = ExtractNumbers(Convert.ToString(item.Kurztext1));
                    dtOutPut.Rows.Add(item.ArticleID, item.Artikelnummer,item.Kurztext1, item.Kurztext2,
                        string.Empty, string.Empty, string.Empty,Dimenstions[0], Dimenstions[1], Dimenstions[2],item.Preis);
                }
            }
            catch (AdapterFailureException ex) { throw ex; }
            catch (Exception ex) { throw ex; }
            return dtOutPut;
        }

        /// <summary>
        ///  Code to identify DataNorm file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static TrinityAdapter InferAdapterFromFile(string path)
        {
            try
            {
                var normIdentifier = new dcm.NormIdentifyLib.NormIdentifier();
                var normInfo = normIdentifier.IdentifyNorm(path);
                if (normInfo.Norm != dcm.NormIdentifyLib.Norm.DATANORM)
                    throw new ArgumentException("Bei der ausgewählten Datei handelt es sich nicht um eine DATANORM Datei");
                switch (normInfo.Version)
                {
                    case "3":
                        FileVerison = "DN3";
                        return TrinityAdapter.ImportDN3;
                    case "4":
                        FileVerison = "DN4";
                        return TrinityAdapter.ImportDN4;
                    case "5":
                        FileVerison = "DN5";
                        return TrinityAdapter.ImportDN5;
                    default: throw new InvalidOperationException("Bei der ausgewählten Datei handelt es sich nicht um eine bekannte DATANORM Datei");
                }
            }
            catch (Exception ex){throw ex;}
        }

        /// <summary>
        /// Code to authorize the license
        /// </summary>
        /// <param name="adapter"></param>
        private static void AuthorizeAdapter(Adapter adapter)
        {
            try
            {
                var authorizer = new AdapterAuthorizer("betasystems+fz/TD22vq%X:VMbb58W@");
                authorizer.Authorize(adapter);
            }
            catch (Exception ex){throw ex;}
        }

        /// <summary>
        ///  Code to attach event handlers
        /// </summary>
        /// <param name="adapter"></param>
        private static void AttachEventHandlers(Adapter adapter)
        {
            try
            {
                adapter.ProgressChanged += (sender, e) =>
                SplashScreenManager.Default.SetWaitFormDescription(e.Current + " of " + e.Maximum);

                adapter.UserLog.LineWritten += (sender, e) =>
                SplashScreenManager.Default.SetWaitFormDescription("(at  "+ e.Timestamp + "): " + e.Text);
            }
            catch (Exception ex){throw ex;}
        }

        /// <summary>
        /// Code to extract dimensions from short description
        /// </summary>
        /// <param name="stText"></param>
        /// <returns></returns>
        private static string[] ExtractNumbers(string stText)
        {
            string[] stNumber = new string[3];
            try
            {
                string[] Temp = Regex.Split(stText, @"\D+");
                int i = -1;
                foreach(string s in Temp)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        i++;
                        stNumber[i] = s;
                    }
                    if (i == 2)
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return stNumber;
        }
    }
}