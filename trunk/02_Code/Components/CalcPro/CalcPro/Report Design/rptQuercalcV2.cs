using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using DAL;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;

namespace CalcPro.Report_Design
{
    public partial class rptQuercalcV2 : DevExpress.XtraReports.UI.XtraReport
    {
        DataTable dtValue;
        int _PID = 0;
        string _Type = string.Empty;
        string _LVSection = string.Empty;
        public rptQuercalcV2()
        {
            InitializeComponent();
        }
        public rptQuercalcV2(int _id, DataTable dt, string type, string LVSection)
        {
            InitializeComponent();
            _PID = _id;
            dtValue = dt;
            _Type = type;
            _LVSection = LVSection;
        }

        double _Discount = 0;

        private void rptQuercalcV2_DataSourceDemanded(object sender, EventArgs e)
        {
            try
            {
                this.p_Rpt_QuerCalculationTableAdapter.Connection.ConnectionString = SQLCon.ConnectionString();
                this.p_Rpt_QuerCalculationTableAdapter.ClearBeforeFill = true;
                this.p_Rpt_QuerCalculationTableAdapter.Fill(dsQuerKalculation1.P_Rpt_QuerCalculation, dtValue, _PID, _Type, _LVSection);

                this.p_Rpt_QuerCalculation_SurchargePositionTableAdapter.Connection.ConnectionString = SQLCon.ConnectionString();
                this.p_Rpt_QuerCalculation_SurchargePositionTableAdapter.ClearBeforeFill = true;
                this.p_Rpt_QuerCalculation_SurchargePositionTableAdapter.Fill(dsSurchargeCalculation1.P_Rpt_QuerCalculation_SurchargePosition, dtValue, _PID, _Type, _LVSection);

                DataTable dt = new DataTable();
                dt = dsSurchargeCalculation1.P_Rpt_QuerCalculation_SurchargePosition;
                if (dt != null)
                {
                    decimal _result = 0;
                    if (decimal.TryParse(Convert.ToString(dt.Rows[0][0]), out _result))
                    {
                        if (_result == 0)
                            tblSurchargeresult.Text = string.Empty;
                        else
                            tblSurchargeresult.Text = _result.ToString("n2");
                    }
                }

                dsDiscountCalculation ds = new dsDiscountCalculation();
                this.p_Rpt_QuerCalculation_DiscountPositionTableAdapter.Connection.ConnectionString = SQLCon.ConnectionString();
                this.p_Rpt_QuerCalculation_DiscountPositionTableAdapter.ClearBeforeFill = true;
                this.p_Rpt_QuerCalculation_DiscountPositionTableAdapter.Fill(ds.P_Rpt_QuerCalculation_DiscountPosition, dtValue, _PID, _Type, _LVSection);

                DataTable dtdiscount = new DataTable();
                dtdiscount = ds.P_Rpt_QuerCalculation_DiscountPosition;
                if (dtdiscount != null)
                {
                    decimal _result = 0;
                    if (decimal.TryParse(Convert.ToString(dtdiscount.Rows[0][0]), out _result))
                    {
                        if (_result > 0)
                        {
                            tbDiscountResult.Text = _result.ToString("n2");
                            _Discount = Convert.ToDouble(_result);
                        }
                        else
                        {
                            tbDiscountResult.Text = string.Empty;
                            _Discount = 0;
                        }
                    }
                }

            }
            catch (Exception ex){}
        }
        double value = 0;
        private void xrTableCell60_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                if (double.TryParse(Convert.ToString(xrTableCell36.Summary.GetResult()), out value))
                {
                    xrTableCell60.Text = value.ToString("n2");
                }
                if (DetailReport2.GetCurrentColumnValue("Total") != DBNull.Value)
                {
                    value = Convert.ToDouble(xrTableCell36.Summary.GetResult()) + Convert.ToDouble(DetailReport2.GetCurrentColumnValue("Total"));
                    xrTableCell60.Text = (value - _Discount).ToString("n2");
                }
            }
            catch (Exception ex){}
        }

        private void xrKommentarKalkulator_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                XRRichText richText = (XRRichText)sender;
                using (RichEditDocumentServer docServer = new RichEditDocumentServer())
                {
                    docServer.RtfText = richText.Rtf;
                    docServer.Document.DefaultCharacterProperties.FontName = "Trebuchet MS";
                    docServer.Document.DefaultCharacterProperties.FontSize = 10;
                    docServer.Document.DefaultParagraphProperties.Alignment = ParagraphAlignment.Justify;
                    richText.Rtf = docServer.RtfText;
                }
            }
            catch (Exception ex) { }
        }

        private void rptQuercalcV2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            
        }
    }
}
