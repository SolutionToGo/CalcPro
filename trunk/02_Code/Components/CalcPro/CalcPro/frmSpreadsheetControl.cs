using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Export;
using System.Data.SqlClient;
using System.IO;
using BL;
using EL;
using DevExpress.XtraBars.Ribbon;

namespace CalcPro
{
    public partial class frmSpreadsheetControl : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        object SupplierID = null;
        object dtValidityDate = DateTime.Now;
        RibbonControl parent;
        public frmSpreadsheetControl()
        {
            InitializeComponent();
            cmbSupplier.EditValueChanged += repositoryItemLookUpEdit1_EditValueChanged;
            dtpValidityDate.EditValueChanged += repositoryItemButtonEdit1_EditValueChanged;
        }

        private void btnImportDataNorm_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                xtraOpenFileDialog1.Title = "Auswahl einer DATANORM Datei";
                xtraOpenFileDialog1.Filter = "Alle Dateien (*.*)|*.*";
                xtraOpenFileDialog1.FilterIndex = 2;
                xtraOpenFileDialog1.CheckFileExists = true;
                xtraOpenFileDialog1.CheckPathExists = true;
                xtraOpenFileDialog1.FileName = string.Empty;
                xtraOpenFileDialog1.RestoreDirectory = true;
                xtraOpenFileDialog1.ShowDialog();
                if (!string.IsNullOrEmpty(xtraOpenFileDialog1.FileName))
                {
                    IWorkbook workbook = spreadsheetControl1.Document;
                    bool isOSExists = false;
                    bool isILExists = false;
                    bool isVLExists = false;
                    foreach (Worksheet ws in workbook.Sheets)
                    {
                        if (ws.Name == "Blatt im Original")
                        {
                            var result = XtraMessageBox.Show("Das Blatt enthält Daten. Möchten Sie den Prozess fortsetzen?", "Bestätigung!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (Convert.ToString(result) == "No")
                            {
                                return;
                            }
                            ws.ConditionalFormattings.Clear();
                            ws.DataValidations.Clear();
                            isOSExists = true;
                        }
                        else if (ws.Name == "Import des Logs")
                        {
                            ws.ConditionalFormattings.Clear();
                            isILExists = true;
                        }
                        else if (ws.Name == "Log mit Validierungsangaben")
                        {
                            ws.ConditionalFormattings.Clear();
                            isVLExists = true;
                        }
                    }

                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    SplashScreenManager.Default.SetWaitFormDescription("Importieren...");
                    if (isOSExists)
                    {
                        workbook.Worksheets.Add("Sheet1");
                        workbook.Worksheets.Remove(workbook.Worksheets["Blatt im Original"]);
                    }
                    if (isILExists)
                        workbook.Worksheets.Remove(workbook.Worksheets["Import des Logs"]);

                    if (isVLExists)
                        workbook.Worksheets.Remove(workbook.Worksheets["Log mit Validierungsangaben"]);

                    DataTable dt = DNU.ProccessFile(xtraOpenFileDialog1.FileName, Application.UserAppDataPath,Path.GetDirectoryName(Application.ExecutablePath));
                    SplashScreenManager.Default.SetWaitFormDescription("Datenverarbeitung … ");
                    Worksheet sheet = workbook.Worksheets[0];
                    sheet.Name = "Blatt im Original";
                    workbook.BeginUpdate();
                    try
                    {
                        ExternalDataSourceOptions options = new ExternalDataSourceOptions() { ImportHeaders = true };
                        Table table = sheet.Tables.Add(dt, 0, 0, options);
                        table.Columns[0].Name = "Artikel_Id";
                        table.Columns[1].Name = "Artikelnummer";
                        table.Columns[2].Name = "Kurztext1";
                        table.Columns[3].Name = "Kurztext2";
                        table.Columns[4].Name = "WG";
                        table.Columns[5].Name = "WA";
                        table.Columns[6].Name = "WI";
                        table.Columns[7].Name = "A";
                        table.Columns[8].Name = "B";
                        table.Columns[9].Name = "L";
                        table.Columns[10].Name = "Preis";
                        sheet.DataValidations.Add(sheet["E:K"], DataValidationType.Custom, "=AND(ISNUMBER(E1))");
                        sheet.Range["N:N"].NumberFormat = "mmm/yyyy";
                        sheet.Columns.AutoFit(0, 10);
                    }
                    finally
                    {
                        workbook.EndUpdate();
                    }
                    btnSaveandConfirm.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    SplashScreenManager.CloseForm(false);
               }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show(ex.Message);
            }
        }
        
        private void frmSpreadsheetControl_Load(object sender, EventArgs e)
        {
            try
            {
                ESupplier ObjESupplier = new ESupplier();
                BSupplier ObjBSupplier = new BSupplier();
                ObjBSupplier.GetSupplier(ObjESupplier);
                cmbSupplier.DataSource = ObjESupplier.dtSupplier;
                cmbSupplier.DisplayMember = "FullName";
                cmbSupplier.ValueMember = "SupplierID";
            }
            catch (Exception ex) { Utility.ShowError(ex); }
        }

        private void btnSaveandConfirm_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (SupplierID == null)
                    return;
                if (dtValidityDate == null)
                    return;
                Worksheet worksheet = spreadsheetControl1.Document.Worksheets.ActiveWorksheet;
                Range range = worksheet.GetDataRange();
                int CCount = 11;
                int rCount = range.RowCount;
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                SplashScreenManager.Default.SetWaitFormDescription("Importieren...");
                DataTable dt = new DataTable();
                for (int j = 0; j < CCount; j++)
                {
                    if(Convert.ToString(worksheet.Rows[0][j].Value) == "Preis")
                        dt.Columns.Add(Convert.ToString(worksheet.Rows[0][j].Value), typeof(decimal));
                    else
                    dt.Columns.Add(Convert.ToString(worksheet.Rows[0][j].Value), typeof(string));
                }

                DataRow dr = null;
                for (int i = 1; i < rCount; i++)
                {
                    dr = dt.NewRow();
                    for (int j = 0; j < CCount; j++)
                    {
                        try
                        {
                            CellValue v = worksheet.Rows[i][j].Value;
                            dr[j] = Convert.ToString(v);
                        }
                        catch (Exception){}
                    }
                    dt.Rows.Add(dr);
                }
                if (dt.Rows.Count > 0)
                {
                    BSupplier ObjBSupplier = new BSupplier();
                    DataTable dtResults = ObjBSupplier.ImportDataNorm(SupplierID, dtValidityDate, dt);
                    if (dtResults.Rows.Count > 0)
                    {
                        IWorkbook workbook = spreadsheetControl1.Document;
                        bool isILExists = false;
                        bool isVLExists = false;
                        foreach (Worksheet ws in workbook.Sheets)
                        {
                            if (ws.Name == "Blatt im Original")
                            {
                                ws.ConditionalFormattings.Clear();
                            }
                            else if (ws.Name == "Import des Logs")
                            {
                                ws.ConditionalFormattings.Clear();
                                isILExists = true;
                            }
                            else if (ws.Name == "Log mit Validierungsangaben")
                            {
                                ws.ConditionalFormattings.Clear();
                                isVLExists = true;
                            }

                        }

                        if (isILExists)
                            workbook.Worksheets.Remove(workbook.Worksheets["Import des Logs"]);

                        if (isVLExists)
                            workbook.Worksheets.Remove(workbook.Worksheets["Log mit Validierungsangaben"]);

                        workbook.Worksheets.Add("Import des Logs");
                        Worksheet sheet = workbook.Worksheets["Import des Logs"];
                        workbook.BeginUpdate();
                        try
                        {
                            ExternalDataSourceOptions options = new ExternalDataSourceOptions() { ImportHeaders = true };
                            Table table = sheet.Tables.Add(dtResults, 0, 0, options);
                            table.Columns[0].Name = "Art_ID";
                            table.Columns[1].Name = "WG";
                            table.Columns[2].Name = "WA";
                            table.Columns[3].Name = "WI";
                            table.Columns[4].Name = "A";
                            table.Columns[5].Name = "B";
                            table.Columns[6].Name = "L";
                            table.Columns[7].Name = "Error";
                        }
                        finally
                        {
                            workbook.EndUpdate();
                        }

                        Range LogRange = sheet.GetUsedRange();
                        ApplyNotExistsArticleFormatingforImportLog(worksheet, LogRange, sheet);
                        workbook.Worksheets.ActiveWorksheet = workbook.Worksheets["Blatt im Original"];
                    }
                }
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Die Daten wurden erfolgreiche importiert und gespeichert.");
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show(ex.Message);
            }
        }

        private void repositoryItemLookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                SupplierID = (sender as LookUpEdit).EditValue;
            }
            catch (Exception ex){}
        }

        private void repositoryItemButtonEdit1_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                dtValidityDate = (sender as DevExpress.XtraEditors.DateEdit).DateTime;
                btnSaveandConfirm.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            catch (Exception ex){}
        }

        public void ApplyNotExistsArticleFormatingforValidationLog(Worksheet sheet, Range LogRange , Worksheet LogSheet)
        {
            try
            {
                string stRange = LogRange.GetReferenceA1();
                    ConditionalFormattingCollection conditionalFormattings = sheet.ConditionalFormattings;
                    FormulaExpressionConditionalFormatting cfRule =
                        conditionalFormattings.AddFormulaExpressionConditionalFormatting(sheet.GetDataRange(), "=$A1=VLOOKUP($A1,'Log mit Validierungsangaben'!$A:$A,1,0)");
                    cfRule.Formatting.Fill.BackgroundColor = Color.Red;
            }
            catch (Exception ex){}
        }

        public void ApplyNotExistsArticleFormatingforImportLog(Worksheet sheet, Range LogRange, Worksheet LogSheet)
        {
            try
            {
                string stRange = LogRange.GetReferenceA1();
                ConditionalFormattingCollection conditionalFormattings = sheet.ConditionalFormattings;
                FormulaExpressionConditionalFormatting cfRule =
                    conditionalFormattings.AddFormulaExpressionConditionalFormatting(sheet.GetDataRange(), "=$A1=VLOOKUP($A1,'Import des Logs'!$A:$A,1,0)");
                cfRule.Formatting.Fill.BackgroundColor = Color.Red;
            }
            catch (Exception ex) { }
        }

        private void btnValidateDataNorm_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                try
                {
                    if (dtValidityDate == null)
                        return;
                    Worksheet worksheet = spreadsheetControl1.Document.Worksheets.ActiveWorksheet;
                    Range range = worksheet.GetDataRange();
                    Range SelectedRange = worksheet.SelectedCell;
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    SplashScreenManager.Default.SetWaitFormDescription("Validieren...");
                    int CCount = 11;
                    int rCount = range.RowCount;
                    DataTable dt = new DataTable();
                    for (int j = 0; j < CCount; j++)
                    {
                        if (Convert.ToString(worksheet.Rows[0][j].Value) == "Preis")
                            dt.Columns.Add(Convert.ToString(worksheet.Rows[0][j].Value), typeof(decimal));
                        else
                            dt.Columns.Add(Convert.ToString(worksheet.Rows[0][j].Value), typeof(string));
                    }

                    DataRow dr = null;
                    for (int i = 1; i < rCount; i++)
                    {
                        dr = dt.NewRow();
                        for (int j = 0; j < CCount; j++)
                        {
                            try
                            {
                                CellValue v = worksheet.Rows[i][j].Value;
                                dr[j] = Convert.ToString(v);
                            }
                            catch (Exception) { }
                        }
                        dt.Rows.Add(dr);
                    }
                    if (dt.Rows.Count > 0)
                    {
                        BSupplier ObjBSupplier = new BSupplier();
                        DataSet dsResults = ObjBSupplier.ValidateDataNorm(SupplierID, dtValidityDate, dt);
                        if (dsResults.Tables[0].Rows.Count > 0)
                        {
                            IWorkbook workbook = spreadsheetControl1.Document;
                            bool isVLExists = false;
                            bool isOLExists = false;
                            bool isILExists = false;
                            foreach (Worksheet ws in workbook.Sheets)
                            {
                                if (ws.Name == "Blatt im Original")
                                {
                                    ws.ConditionalFormattings.Clear();
                                    isOLExists = true;
                                }
                                else if (ws.Name == "Log mit Validierungsangaben")
                                {
                                    ws.ConditionalFormattings.Clear();
                                    isVLExists = true;
                                }
                                else if (ws.Name == "Import des Logs")
                                {
                                    ws.ConditionalFormattings.Clear();
                                    isILExists = true;
                                }

                            }

                            if (isOLExists)
                            {
                                workbook.Worksheets.Add("Sheet1");
                                workbook.Worksheets.Remove(workbook.Worksheets["Blatt im Original"]);
                            }

                            if (isVLExists)
                                workbook.Worksheets.Remove(workbook.Worksheets["Log mit Validierungsangaben"]);

                            if (isILExists)
                                workbook.Worksheets.Remove(workbook.Worksheets["Import des Logs"]);

                            if(dsResults.Tables.Count > 1)
                            {
                                if (dsResults.Tables[1].Rows.Count > 0)
                                    btnSaveandConfirm.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                                else
                                    btnSaveandConfirm.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                            }
                            Worksheet sheet = workbook.Worksheets[0];
                            sheet.Name = "Blatt im Original";
                            workbook.BeginUpdate();
                            try
                            {
                                ExternalDataSourceOptions options = new ExternalDataSourceOptions() { ImportHeaders = true };
                                Table table = sheet.Tables.Add(dsResults.Tables[0], 0, 0, options);
                                table.Columns[0].Name = "Artikel_Id";
                                table.Columns[1].Name = "Artikelnummer";
                                table.Columns[2].Name = "Kurztext1";
                                table.Columns[3].Name = "Kurztext2";
                                table.Columns[4].Name = "WG";
                                table.Columns[5].Name = "WA";
                                table.Columns[6].Name = "WI";
                                table.Columns[7].Name = "A";
                                table.Columns[8].Name = "B";
                                table.Columns[9].Name = "L";
                                table.Columns[10].Name = "Preis";
                                table.Columns[11].Name = "New/Update";
                                table.Columns[12].Name = "OldPreis";
                                table.Columns[13].Name = "Validity Date";
                                table.Columns[14].Name = "Comments";
                                sheet.DataValidations.Add(sheet["E:K"], DataValidationType.Custom, "=AND(ISNUMBER(E1))");
                                sheet.Range["N:N"].NumberFormat = "mmm/yyyy";
                            }
                            finally
                            {
                                workbook.EndUpdate();
                            }

                            if (dsResults.Tables[1].Rows.Count > 0)
                            {
                                workbook.Worksheets.Add("Log mit Validierungsangaben");
                                Worksheet Vsheet = workbook.Worksheets["Log mit Validierungsangaben"];
                                workbook.BeginUpdate();
                                try
                                {
                                    ExternalDataSourceOptions options = new ExternalDataSourceOptions() { ImportHeaders = true };
                                    Table table = Vsheet.Tables.Add(dsResults.Tables[1], 0, 0, options);
                                    table.Columns[0].Name = "Art_Id";
                                    table.Columns[1].Name = "Comments";
                                }
                                finally
                                {
                                    workbook.EndUpdate();
                                }
                                sheet.Columns.AutoFit(0, 14);
                                workbook.Worksheets.ActiveWorksheet = workbook.Worksheets["Blatt im Original"];
                                Range LogRange = Vsheet.GetDataRange();
                                ApplyNotExistsArticleFormatingforValidationLog(sheet, LogRange, Vsheet);
                                string stRange = SelectedRange.GetReferenceA1();
                                spreadsheetControl1.SelectedCell = sheet.Cells[stRange];
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Die Angaben wurden validiert. Bitte prüfen Sie die Hinweise zur Validierung.");
                            }
                            else
                            {
                                string stRange = SelectedRange.GetReferenceA1();
                                spreadsheetControl1.SelectedCell = sheet.Cells[stRange];
                                sheet.Columns.AutoFit(0, 14);
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Die Angaben wurden validiert. Sämtliche Angaben sind korrekt und vollständig.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void frmSpreadsheetControl_ParentChanged(object sender, EventArgs e)
        {
            if (parent != null)
                parent.Merge -= new RibbonMergeEventHandler(parent_Merge);

            MdiClient parentClient = Parent as MdiClient;
            if (parentClient == null)
                return;
            RibbonForm form = parentClient.FindForm() as RibbonForm;
            if (form == null)
                return;
            parent = form.Ribbon;
            parent.Merge += new RibbonMergeEventHandler(parent_Merge);
        }

        void parent_Merge(object sender, RibbonMergeEventArgs e)
        {
            RibbonControl ribbon = sender as RibbonControl;
            ribbon.SelectedPage = ribbon.MergedPages[0];
        }

        private void btnOpen_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                xtraOpenFileDialog1.Title = "Auswahl XLSX Datei";
                xtraOpenFileDialog1.Filter = "Excel|*.xls|Excel 2010|*.xlsx";
                xtraOpenFileDialog1.FilterIndex = 2;
                xtraOpenFileDialog1.CheckFileExists = true;
                xtraOpenFileDialog1.CheckPathExists = true;
                xtraOpenFileDialog1.FileName = string.Empty;
                xtraOpenFileDialog1.RestoreDirectory = true;
                xtraOpenFileDialog1.ShowDialog();
                if (!string.IsNullOrEmpty(xtraOpenFileDialog1.FileName))
                {
                    IWorkbook workbook = spreadsheetControl1.Document;
                    bool isOSExists = false;
                    bool isILExists = false;
                    bool isVLExists = false;
                    foreach (Worksheet ws in workbook.Sheets)
                    {
                        if (ws.Name == "Blatt im Original")
                        {
                            var result = XtraMessageBox.Show("Das Blatt enthält Daten. Möchten Sie den Prozess fortsetzen?", "Bestätigung!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (Convert.ToString(result) == "No")
                            {
                                return;
                            }
                            ws.ConditionalFormattings.Clear();
                            ws.DataValidations.Clear();
                            isOSExists = true;
                        }
                        else if (ws.Name == "Import des Logs")
                        {
                            ws.ConditionalFormattings.Clear();
                            isILExists = true;
                        }
                        else if (ws.Name == "Log mit Validierungsangaben")
                        {
                            ws.ConditionalFormattings.Clear();
                            isVLExists = true;
                        }
                    }
                    if (isOSExists)
                    {
                        workbook.Worksheets.Add("Sheet1");
                        workbook.Worksheets.Remove(workbook.Worksheets["Blatt im Original"]);
                    }

                    if (isILExists)
                        workbook.Worksheets.Remove(workbook.Worksheets["Import des Logs"]);

                    if (isVLExists)
                        workbook.Worksheets.Remove(workbook.Worksheets["Log mit Validierungsangaben"]);

                    workbook.LoadDocument(xtraOpenFileDialog1.FileName, DocumentFormat.Xlsx);
                    btnSaveandConfirm.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void spreadsheetControl1_UnhandledException(object sender,DevExpress.XtraSpreadsheet.SpreadsheetUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            // Add your code to handle the exception.
            BeginInvoke(new MethodInvoker(delegate ()
            {
                XtraMessageBox.Show(e.Exception.Message, "Die Ausführungsanweisungen ist nicht umgesetzt.");
            }));
        }

        private void frmSpreadsheetControl_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                var dlgResult = XtraMessageBox.Show("Wollen Sie die Daten speichern?", "Frage", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(Convert.ToString(dlgResult) == "Yes")
                {
                    spreadsheetCommandBarButtonItem3.PerformClick();
                }

            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }
    }
}