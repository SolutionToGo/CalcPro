using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;


namespace CalcPro.Report_Design
{
    public partial class rptDeliveryNotes : DevExpress.XtraReports.UI.XtraReport
    {
        private int _DeliveryNumberID = -1;
        public int DeliveryNumberID
        {
            get { return _DeliveryNumberID; }
            set { _DeliveryNumberID = value; }
        }
        public rptDeliveryNotes()
        {
            InitializeComponent();
        }

       
    }
}
