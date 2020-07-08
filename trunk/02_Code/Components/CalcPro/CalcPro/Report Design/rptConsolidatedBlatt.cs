using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace CalcPro.Report_Design
{
    public partial class rptConsolidatedBlatt : DevExpress.XtraReports.UI.XtraReport
    {
        public rptConsolidatedBlatt()
        {
            InitializeComponent();
        }

        double totalUnits = 0;
        private void xrLabel16_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = totalUnits;
            e.Handled = true;
        }

        private void xrLabel16_SummaryReset(object sender, EventArgs e)
        {
            totalUnits = 0;
        }


        private void xrLabel16_SummaryRowChanged(object sender, EventArgs e)
        {
            try
            {
                if (DetailReport.GetCurrentColumnValue("Quantity") != DBNull.Value)
                    totalUnits += Convert.ToDouble(DetailReport.GetCurrentColumnValue("Quantity"));
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

    }
}
