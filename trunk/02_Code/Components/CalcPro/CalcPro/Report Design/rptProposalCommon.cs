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
    public partial class rptProposalCommon : DevExpress.XtraReports.UI.XtraReport
    {
        int _PID = 0;
        DataTable dtValue;
        string _Type = string.Empty;
        string _LVSection = string.Empty;
        bool ISWithGB = false;

        public rptProposalCommon()
        {
            InitializeComponent();
        }
        public rptProposalCommon(int _ProID, DataTable dt, string type, string LVSection, bool IsReportWithGB = true)
        {
            InitializeComponent();
            _PID = _ProID;
            dtValue = dt;
            _Type = type;
            _LVSection = LVSection;
            ISWithGB = IsReportWithGB;
            //xrPictureBox1.Image = null;
        }

        Double totalvat = 0;
        Double GBValue = 0;
        Double GB1 = 0;
        Double GBWithVat = 0;
        private void xrTableCell13_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Double dValue = 0;
            Double Value1 = 0;
            Double Value2 = 0;
            try
            {
                if (double.TryParse(Convert.ToString(txtVat.Text), out dValue))
                    totalvat = dValue;
                GBValue = value;
                double _result = 0;
                //if (!ISWithGB)
                _result = (GBValue * totalvat) / 100;
                xrLblTotalVat.Text = _result.ToString("n2");
                if (double.TryParse(xrLblGB.Text, out Value1))
                    GB1 = Value1;
                if (double.TryParse(xrLblTotalVat.Text, out Value2))
                    GBWithVat = Value2;
                double _resultVat = 0;
                //if (!ISWithGB)
                _resultVat = GB1 + GBWithVat;
                xrLabelFinalResult.Text = _resultVat.ToString("n2");
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
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
                        if (_result != 0)
                            //&& !ISWithGB)
                        {
                            tbDiscount.Text = '-' + _result.ToString("n2");
                            _Discount = Convert.ToDouble(_result);
                        }
                        else
                        {
                            tbDiscount.Text = string.Empty;
                            _Discount = 0;
                            lblDiscount.Visible = false;
                            lblDiscountEuro.Visible = false;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }
        double value = 0;
        private void xrLblGB_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                double dMA = 0;
                double dMO = 0;
                double.TryParse(Convert.ToString(xrLblMA.Summary.GetResult()), out dMA);
                double.TryParse(Convert.ToString(xrLblMO.Summary.GetResult()), out dMO);
                //if (!ISWithGB)
                value = dMA + dMO - _Discount;
                xrLblGB.Text = (value).ToString("n2");
                xrTableCell13_BeforePrint(null, null);
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        #region 'Font Setting Events'

        private void xrRichText4_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                XRRichText richText = (XRRichText)sender;
                richText.Font = new Font("Trebuchet MS", 10, FontStyle.Bold);
            }
            catch (Exception ex){}
        }

        private void xrRichText3_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                XRRichText richText = (XRRichText)sender;
                richText.Font = new Font("Trebuchet MS", 10, FontStyle.Regular);
            }
            catch (Exception ex) { Utility.ShowError(ex); }
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
            catch (Exception ex) { Utility.ShowError(ex); }
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
            catch (Exception ex){}
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
                string sttemp = Convert.ToString(DetailReport.GetCurrentColumnValue("FinalGB"));
                double dSum = 0;
                double.TryParse(sttemp, out dSum);
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
            catch (Exception ex) { Utility.ShowError(ex); }
        }
        #endregion

        private void xrLabel15_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            if (!IsNormalSubtitle)
                e.Result = "A";
            else
                e.Result = "";
            e.Handled = true;
        }
    }
}
