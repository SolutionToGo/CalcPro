using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using OTTOPro.dsDeliveryNotesTableAdapters;
using System.Data;
using System.Windows.Forms;

namespace OTTOPro
{
    public partial class rptDeliveryNotes : DevExpress.XtraReports.UI.XtraReport
    {
        int _Custid;
        int _Projectid;

        public rptDeliveryNotes()
        {
            InitializeComponent();
        }

        public rptDeliveryNotes(int _Prjid, int _Cutid)
        {
            InitializeComponent();
            _Projectid = _Prjid;
            _Custid = _Cutid;
        }

        private void rptDeliveryNotes_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            PositionTableAdapter _tdPosition = new PositionTableAdapter();
            _tdPosition.FillByProjectID(dsDeliveryNotes1.Position, _Projectid);

            ProjectTableAdapter _tdProject = new ProjectTableAdapter();
            _tdProject.FillByProjectID(dsDeliveryNotes1.Project,_Projectid);

            CustomerTableAdapter _tdCustomer = new CustomerTableAdapter();
            _tdCustomer.FillByCustmerID(dsDeliveryNotes1.Customer, _Custid);

        }

        private void xrTableCell6_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((XRLabel)sender).Tag = GetCurrentRow();
        }

        private void xrTableCell6_PreviewClick(object sender, PreviewMouseEventArgs e)
        {
            // Create a new Detail Report instance. 
            rptSubReport detailReport = new rptSubReport();

            // Obtain the current category's ID and Name from the e.Brick.Value property, 
            // which stores an object assigned the label's Tag property. 
            detailReport.ProjectID.Value = (int)((DataRowView)e.Brick.Value).Row["ProjectID"];
           // detailReport.positio.Value = ((DataRowView)e.Brick.Value).Row["Position_OZ"].ToString();

            // Show the detail report in a new modal window. 
            ReportPrintTool pt = new ReportPrintTool(detailReport);
            pt.ShowPreviewDialog();
        }

        private void xrTableCell6_PreviewMouseMove(object sender, PreviewMouseEventArgs e)
        {
            e.PreviewControl.Cursor = Cursors.Hand;
        }

//******************
    }
}
