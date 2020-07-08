using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using DAL;

namespace CalcPro.Report_Design
{
    public partial class rptInvoicewithLangText : DevExpress.XtraReports.UI.XtraReport
    {
        public rptInvoicewithLangText()
        {
            InitializeComponent();
        }       
        double totalUnits = 0;
        int i = 0;

        private void TopMargin_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (i++ == 0)
            {
                foreach (XRControl cont in (((TopMarginBand)sender).Controls))
                {
                    cont.PrintOnPage += new PrintOnPageEventHandler(cont_PrintOnPage);
                }
            }
        }

        void cont_PrintOnPage(object sender, PrintOnPageEventArgs e)
        {
            e.Cancel = e.PageIndex == 0;
        }        

        private void xrLabel42_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = totalUnits.ToString("n2");
            e.Handled = true;
        }

        private void xrLabel42_SummaryReset(object sender, EventArgs e)
        {
           // totalUnits = 0;
        }

        private void xrLabel42_SummaryRowChanged(object sender, EventArgs e)
        {
            try
            {
                if (DetailReport.GetCurrentColumnValue("FinalGB") != DBNull.Value)
                    totalUnits += Convert.ToDouble(DetailReport.GetCurrentColumnValue("FinalGB"));
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }      
        }

        private void tbDiscount_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            tbDiscount_PrintOnPage(null,null);
        }

        private void tbDiscount_PrintOnPage(object sender, PrintOnPageEventArgs e)
        {
            Double dValue = 0;
            try
            {
                if (DetailReport.GetCurrentColumnValue("Discount") != DBNull.Value)
                {
                    dValue = Convert.ToDouble(DetailReport.GetCurrentColumnValue("Discount"));
                }
                if (dValue <= 0)
                {
                    tbDiscount.Visible = false;
                    lblDiscount.Visible = false;
                    lblDiscountEuro.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }
             
       

    }
}
