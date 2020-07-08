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
using System.Diagnostics;

namespace CalcPro
{
    public partial class frmDocuwareLink : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// This form is for saving docuware links of position
        /// </summary>
        #region Variables and properties

        private bool _IsSave = false;
        private bool _ISUpdated = false;
        private bool _isFirstTime1 = true;
        private bool _isFirstTime2 = true;
        private bool _isFirstTime3 = true;
        private string _DocuwareLink1;
        private string _DocuwareLink2;
        private string _DocuwareLink3;

        public string DocuwareLink1
        {
            get { return _DocuwareLink1; }
            set { _DocuwareLink1 = value; }
        }
        public string DocuwareLink2
        {
            get { return _DocuwareLink2; }
            set { _DocuwareLink2 = value; }
        }
        public string DocuwareLink3
        {
            get { return _DocuwareLink3; }
            set { _DocuwareLink3 = value; }
        }
        public frmDocuwareLink()
        {
            InitializeComponent();
        }

        #endregion

        #region Events
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            DialogResult result = ofd.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtLink1.Text = ofd.FileName;
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            DialogResult result = ofd.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtLink2.Text = ofd.FileName;
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            DialogResult result = ofd.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtLink3.Text = ofd.FileName;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this._IsSave = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this._IsSave = false;
            this.Close();
        }

        private void frmDocuwareLink_FormClosing(object sender, FormClosingEventArgs e)
        {
            string _dlgResult = "";
            if (_IsSave)
            {
                _DocuwareLink1 = txtLink1.Text;
                _DocuwareLink2 = txtLink2.Text;
                _DocuwareLink3 = txtLink3.Text;
            }
            else
            {
                if (_ISUpdated)
                {
                    if(Utility._IsGermany == true)
                    {
                        _dlgResult = XtraMessageBox.Show("Änderungen speichern …!", "Bestätigung …?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question).ToString();
                    }
                    else
                    {
                        _dlgResult = XtraMessageBox.Show("Save changes...!", "Confirmation.. ?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question).ToString();

                    }
                    if (!string.IsNullOrEmpty(_dlgResult))
                    {
                        if (_dlgResult.ToLower() == "yes")
                        {
                            _DocuwareLink1 = txtLink1.Text;
                            _DocuwareLink2 = txtLink2.Text;
                            _DocuwareLink3 = txtLink3.Text;
                        }
                        else if (_dlgResult.ToLower() == "cancel")
                        {
                            e.Cancel = true;
                        }
                    }
                }
            }
        }

        private void textEdit1_TextChanged(object sender, EventArgs e)
        {
            if (!_isFirstTime1)
            {
                _ISUpdated = true;
            }
            _isFirstTime1 = false;
        }

        private void frmDocuwareLink_Load(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(_DocuwareLink1))
                    txtLink1.Text = _DocuwareLink1;
                if (!string.IsNullOrEmpty(_DocuwareLink2))
                    txtLink2.Text = _DocuwareLink2;
                if (!string.IsNullOrEmpty(_DocuwareLink3))
                    txtLink3.Text = _DocuwareLink3;
            }
            catch (Exception ex)
            {
                
            }
        }

        private void textEdit2_TextChanged(object sender, EventArgs e)
        {
            if (!_isFirstTime2)
            {
                _ISUpdated = true;
            }
            _isFirstTime2 = false;
        }

        private void textEdit3_TextChanged(object sender, EventArgs e)
        {
            if (!_isFirstTime3)
            {
                _ISUpdated = true;
            }
            _isFirstTime3 = false;
        }

        private void frmDocuwareLink_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if(e.KeyCode == Keys.Escape)
                {
                    btnCancel_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void btnOpenLink1_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtLink1.Text == "")
                {
                    MessageBox.Show("Es konnte keine Datei gefunden werden");
                    return;
                }
                else
                {
                    Process.Start(txtLink1.Text);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Die angegebene Datei konnte nicht gefunden werden");
                return;
            }
        }

        private void btnOpenLink2_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtLink2.Text == "")
                {
                    MessageBox.Show("Es konnte keine Datei gefunden werden");
                    return;
                }
                else
                {
                    Process.Start(txtLink2.Text);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Die angegebene Datei konnte nicht gefunden werden");
                return;
            }
        }

        private void btnOpenLink3_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtLink3.Text == "")
                {
                    MessageBox.Show("Es konnte keine Datei gefunden werden");
                    return;
                }
                else
                {
                    Process.Start(txtLink3.Text);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Die angegebene Datei konnte nicht gefunden werden");
                return;
            }
        }

        private void txtLink1_Enter(object sender, EventArgs e)
        {
            try
            {
                var edit = ((DevExpress.XtraEditors.TextEdit)sender);
                BeginInvoke(new MethodInvoker(() =>
                {
                    edit.SelectionStart = 0;
                    edit.SelectionLength = edit.Text.Length;
                }));
            }
            catch (Exception ex) { }
        }
        #endregion

    }
}