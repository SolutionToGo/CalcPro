using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace CalcPro.Report_Design
{
    public partial class rptSampleBreakdown : DevExpress.XtraReports.UI.XtraReport
    {
        public rptSampleBreakdown()
        {
            InitializeComponent();
            this.sqlDataSource1.Connection.ConnectionString = DAL.SQLCon.ConnectionString();
            this.sqlDataSource2.Connection.ConnectionString = DAL.SQLCon.ConnectionString();
        }

        private void xrRichText1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                XRRichText richText = (XRRichText)sender;
                richText.Font = new Font("Trebuchet MS", 6, FontStyle.Bold);
            }
            catch (Exception ex) { }
        }

        private void xrRichText14_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                XRRichText richText = (XRRichText)sender;
                richText.Font = new Font("Trebuchet MS", 6, FontStyle.Regular);
            }
            catch (Exception ex) { }
        }
    }
}
