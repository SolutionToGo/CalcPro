using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DAL;
using System.Data;
using System.Windows.Forms;

namespace CalcPro.Report_Design
{
    public partial class rptFormBlatt_221_1 : DevExpress.XtraReports.UI.XtraReport
    {
        DataTable dtValue;
        int _PID = 0;
        public rptFormBlatt_221_1()
        {
            InitializeComponent();
        }

        public rptFormBlatt_221_1(int _id, DataTable dt)
        {
            InitializeComponent();
            _PID = _id;
            dtValue = dt;
        }

        private void rptFormBlatt_221_1_DataSourceDemanded(object sender, EventArgs e)
        {
            try
            {
                this.p_Rpt_Get_FormBlatt_221_2TableAdapter.Connection.ConnectionString = SQLCon.ConnectionString();
                this.p_Rpt_Get_FormBlatt_221_2TableAdapter.ClearBeforeFill = true;
                this.p_Rpt_Get_FormBlatt_221_2TableAdapter.Fill(dsFormBlatt_221_21.P_Rpt_Get_FormBlatt_221_2, _PID, dtValue);
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

            

    }
}
