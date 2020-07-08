using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DAL;
using System.Data;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;

namespace CalcPro.Report_Design
{
    public partial class rptProposalwithoutMAMO : DevExpress.XtraReports.UI.XtraReport
    {
        int _PID=0;
        DataTable dtValue;
        string _Type = string.Empty;
        string _LVSection = string.Empty;
        bool IswithGB = false;
        public rptProposalwithoutMAMO()
        {
            InitializeComponent();
        }

        public rptProposalwithoutMAMO(int _ProID, DataTable dt, string type, string LVSection, bool IsReportWithGB = true)
        {
            InitializeComponent();
            _PID=_ProID;
            dtValue = dt;
            _Type = type;
            _LVSection = LVSection;
            IswithGB = IsReportWithGB;
        }

        private void TopMargin_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                foreach (XRControl cont in (((TopMarginBand)sender).Controls))
                {
                    cont.PrintOnPage += new PrintOnPageEventHandler(cont_PrintOnPage);
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        void cont_PrintOnPage(object sender, PrintOnPageEventArgs e)
        {
            try
            {
                if (e.PageIndex > 0)
                {
                    xrLabel10.Visible = false;
                    xrLabel11.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        double _Discount = 0;

        private void rptProposalCommon_DataSourceDemanded(object sender, EventArgs e)
        {
            try
            {
               
                dsDiscountCalculation ds = new dsDiscountCalculation();
                this.p_Rpt_QuerCalculation_DiscountPositionTableAdapter.Connection.ConnectionString = SQLCon.ConnectionString();
                this.p_Rpt_QuerCalculation_DiscountPositionTableAdapter.ClearBeforeFill = true;
                this.p_Rpt_QuerCalculation_DiscountPositionTableAdapter.Fill(ds.P_Rpt_QuerCalculation_DiscountPosition, dtValue, _PID, _Type, _LVSection);

                dsProposalCommon _dsCommon = new dsProposalCommon();
                this.p_Rpt_PositionForProposalPriceForCommonTableAdapter.Connection.ConnectionString = SQLCon.ConnectionString();
                this.p_Rpt_PositionForProposalPriceForCommonTableAdapter.ClearBeforeFill = true;
                this.p_Rpt_PositionForProposalPriceForCommonTableAdapter.Fill(_dsCommon.P_Rpt_PositionForProposalPriceForCommon, dtValue, _PID, _Type, _LVSection);

                dsProposalCommonTotalSummery _dsTotalSum = new dsProposalCommonTotalSummery();
                this.p_Rpt_GetTotalSummeryTableAdapter.Connection.ConnectionString = SQLCon.ConnectionString();
                this.p_Rpt_GetTotalSummeryTableAdapter.ClearBeforeFill = true;
                this.p_Rpt_GetTotalSummeryTableAdapter.Fill(_dsTotalSum.P_Rpt_GetTotalSummery, dtValue, _PID, _Type, _LVSection);

                DataTable dtdiscount = new DataTable();
                dtdiscount = ds.P_Rpt_QuerCalculation_DiscountPosition;
                if (dtdiscount != null)
                {
                    decimal _result = 0;
                    if (decimal.TryParse(Convert.ToString(dtdiscount.Rows[0][0]), out _result))
                    {
                        if (_result != 0 && !IswithGB)
                        {
                            tbDiscount.Text = '-'+ _result.ToString("n2");
                            _Discount = Convert.ToDouble(_result);
                        }
                        else
                        {
                            tbDiscount.Text = string.Empty;
                            lblDiscount.Visible = false;
                            lblDiscountEuro.Visible = false;
                            _Discount = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void xrTableCell13_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            double DVat = 0;
            try
            {
                if (double.TryParse(txtVat.Text, out DVat))
                {
                    double DGBValue = 0;
                    if (double.TryParse(Convert.ToString(xrLblGB.Summary.GetResult()), out DGBValue))
                    {
                        double _result = 0;
                        if (!IswithGB)
                            _result = Convert.ToDouble(((DGBValue - _Discount) * DVat) / 100);
                        xrLblTotalVat.Text = _result.ToString("n2");
                    }
                }
            }
            catch (Exception ex) { }
        }

        private void xrLabelFinalResult_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            double DVat = 0;
            try
            {
                if (double.TryParse(txtVat.Text, out DVat))
                {
                    double DGBValue = 0;
                    if (double.TryParse(Convert.ToString(xrLblGB.Summary.GetResult()), out DGBValue))
                    {
                        double _result = 0;
                        if (!IswithGB)
                            _result = Convert.ToDouble(((DGBValue - _Discount) * DVat) / 100);
                        xrLabelFinalResult.Text = ((DGBValue + _result) - _Discount).ToString("n2");
                    }
                }
            }
            catch (Exception ex) { }
        }

        #region 'Font Settings'
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

        private void xrRichText1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                XRRichText richText = (XRRichText)sender;
                richText.Font = new Font("Trebuchet MS", 10, FontStyle.Bold);
            }
            catch (Exception ex) { }
        }

        private void xrRichText5_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                XRRichText richText = (XRRichText)sender;
                richText.Font = new Font("Trebuchet MS", 10, FontStyle.Regular);
            }
            catch (Exception ex) { }
        }
        #endregion

        #region 'group Summary Calculation'
        double GroupSum = 0;
        double AlternateSum = 0;
        bool IsNormalSubtitle = false;
        private void lblGroupSum_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            try
            {
                if (!IsNormalSubtitle)
                    e.Result = AlternateSum;
                else
                    e.Result = GroupSum;
                e.Handled = true;
            }
            catch (Exception){}
        }

        private void lblGroupSum_SummaryReset(object sender, EventArgs e)
        {
            GroupSum = 0;
            AlternateSum = 0;
            IsNormalSubtitle = false;
        }

        private void lblGroupSum_SummaryRowChanged(object sender, EventArgs e)
        {
            try
            {
                string stTemp = Convert.ToString(DetailReport.GetCurrentColumnValue("FinalGB"));
                double dSum = 0;
                double.TryParse(stTemp, out dSum);
                string stPKZ = Convert.ToString(DetailReport.GetCurrentColumnValue("PositionKZ"));
                int i = 0;
                if (int.TryParse(Convert.ToString(DetailReport.GetCurrentColumnValue("DetailKZ")), out i) && i == 0)
                {
                    if (stPKZ != "A")
                    {
                        if (stPKZ != "E")
                        {
                            GroupSum += dSum;
                            IsNormalSubtitle = true;
                        }
                    }
                    AlternateSum += dSum;
                }
            }
            catch (Exception ex) { }
        }
        #endregion

        private void xrLabel15_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            try
            {
                if (!IsNormalSubtitle)
                    e.Result = "A";
                else
                    e.Result = "";
                e.Handled = true;
            }
            catch (Exception) { }
        }
    }
}
