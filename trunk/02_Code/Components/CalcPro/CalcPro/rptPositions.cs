using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace OTTOPro
{
    public partial class rptPositions : DevExpress.XtraReports.UI.XtraReport
    {
        int _Id = 0;
        public rptPositions()
        {
            InitializeComponent();
        }
        public rptPositions(int _tID)
        {
            InitializeComponent();
            _Id = _tID;
        }

        private void rptPositions_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            this.positionTableAdapter.FillById(dsPositions1.Position, _Id);
        }

    }
}
