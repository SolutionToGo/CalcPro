using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using OTTOPro.dsLVDetailsTableAdapters;
using System.Data;

namespace OTTOPro
{
    public partial class rptLVDetails : DevExpress.XtraReports.UI.XtraReport
    {
        int _PID;
        public rptLVDetails()
        {
            InitializeComponent();
        }
        public rptLVDetails(int _id)
        {
            InitializeComponent();
            _PID = _id;
        }

        private void rptLVDetails_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            PositionTableAdapter ta = new dsLVDetailsTableAdapters.PositionTableAdapter();
            ta.FillByPID(dsLVDetails1.Position, _PID);


            OTTOContactTableAdapter _Cta = new OTTOContactTableAdapter();
            _Cta.FillByContact(dsLVDetails1.OTTOContact,true);

        }

        double totalUnits = 0;
        private void xrLabel8_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = totalUnits;
            e.Handled = true;
        }

        private void xrLabel8_SummaryReset(object sender, EventArgs e)
        {
            totalUnits = 0;
        }

        private void xrLabel8_SummaryRowChanged(object sender, EventArgs e)
        {
            // Calculate a summary. 
            if (GetCurrentColumnValue("FinalGB") != DBNull.Value)
                totalUnits += Convert.ToDouble(GetCurrentColumnValue("FinalGB"));
        }

    }
}
