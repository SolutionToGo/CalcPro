using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace OTTOPro
{
    public partial class frmAdhocPosition : DevExpress.XtraEditors.XtraForm
    {
        private string _SurchargePer;
        private string _SurchargeFrom;
        private string _SurchargeTo;

        public string SurchargePer
        {
            get { return _SurchargePer; }
            set { _SurchargePer = value; }
        }
        public string SurchargeFrom
        {
            get { return _SurchargeFrom; }
            set { _SurchargeFrom = value; }
        }
        public string SurchargeTo
        {
            get { return _SurchargeTo; }
            set { _SurchargeTo = value; }
        }

        public frmAdhocPosition()
        {
            InitializeComponent();
        }
        public frmAdhocPosition(string tvalue)
        {
            InitializeComponent();
            txtMaxOZ.Text = tvalue;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _SurchargePer = txtSurchargePer.Text;
            _SurchargeFrom = txtSurchargeFrom.Text;
            _SurchargeTo = txtSurchargeTo.Text;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAdhocPosition_Load(object sender, EventArgs e)
        {
            txtSurchargePer.Text = _SurchargePer;
            txtSurchargeFrom.Text = _SurchargeFrom;
            txtSurchargeTo.Text = _SurchargeTo;
        }
    }
}