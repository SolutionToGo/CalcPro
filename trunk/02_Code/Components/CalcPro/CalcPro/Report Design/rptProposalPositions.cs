using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;

namespace CalcPro.Report_Design
{
    public partial class rptProposalPositions : DevExpress.XtraReports.UI.XtraReport
    {
        public rptProposalPositions()
        {
            InitializeComponent();
        }

        private void txtTitleKurztext_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                XRRichText richText = (XRRichText)sender;
                richText.Font = new Font("Trebuchet MS", 9, FontStyle.Bold);
            }
            catch (Exception ex) { }
        }

        private void txtPositionKurztext_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                XRRichText richText = (XRRichText)sender;
                richText.Font = new Font("Trebuchet MS", 9, FontStyle.Regular);
            }
            catch (Exception ex) { }
        }

        private void txtCustomerNAme_PrintOnPage(object sender, PrintOnPageEventArgs e)
        {
            try
            {
                if(e.PageIndex >0 )
                {
                    txtCustomerAddress.Visible = false;
                    txtCustomerNAme.Visible = false;
                }
                   
           }
            catch (Exception ex){}
        }

        double GroupSum = 0;
        double AlternateSum = 0;
        bool IsNormalSubtitle = false;
        bool IsASubTitle = false;
        private void xrTableCell43_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            try
            {
                if (IsNormalSubtitle)
                {
                    if (GroupSum > 0)
                        e.Result = GroupSum;
                    else
                        e.Result =  string.Empty;
                }

                else
                {
                    if (IsASubTitle)
                    {
                        if (AlternateSum > 0)
                            e.Result = AlternateSum;
                        else
                            e.Result = string.Empty;
                    }
                    else
                        e.Result = "nur EP!";
                }
                    
                e.Handled = true;
            }
            catch (Exception ex) { }
        }

        private void xrTableCell43_SummaryRowChanged(object sender, EventArgs e)
        {
            try
            {
                string sttemp = Convert.ToString(this.GetCurrentColumnValue("FinalGB"));
                double dSum = 0;
                double.TryParse(sttemp, out dSum);
                string stPKZ = Convert.ToString(this.GetCurrentColumnValue("PositionKZ"));
                int i = 0;
                if (int.TryParse(Convert.ToString(this.GetCurrentColumnValue("DetailKZ")), out i) && i == 0 && stPKZ != "ZZ")
                {
                    if (stPKZ != "A")
                    {
                        GroupSum += dSum;
                        if (stPKZ != "E")
                            IsNormalSubtitle = true;
                    }
                    else
                        IsASubTitle = true;
                    AlternateSum += dSum;
                }
            }
            catch (Exception ex) { Utility.ShowError(ex); }
        }

        private void xrTableCell43_SummaryReset(object sender, EventArgs e)
        {
            GroupSum = 0;
            AlternateSum = 0;
            IsNormalSubtitle = false;
            IsASubTitle = false;
        }

        private void xrTableCell4_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            try
            {
                if (!IsNormalSubtitle && IsASubTitle && Convert.ToBoolean(this.Parameters["WithGP"].Value))
                    e.Result = "A";
                else
                    e.Result = "";
                e.Handled = true;
            }
            catch (Exception ex){}
        }
    }
}
