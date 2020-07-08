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
using BL;
using EL;

namespace CalcPro
{
    public partial class frmAddRaster : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// This is form is to increase ot decrease ratser of a project.
        /// </summary>
        #region Varibales
        public int _ProjectID = 0;
        bool _isValidate = false;
        private string _NewRaster = string.Empty;
        private BGAEB objBGAEB = null;
        public string NewRaster
        {
            get { return _NewRaster; }
            set { _NewRaster = value; }
        }
        #endregion

        #region Constructors
        public frmAddRaster()
        {
            InitializeComponent();
        }
        public frmAddRaster(string _Raster)
        {
            InitializeComponent();
            txtOldRaster.Text = _Raster;
        }
        #endregion

        #region Events
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;            
            this.Close();
        }

        private void rgRasterNumbers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rgRasterNumbers.SelectedIndex == 0)
                {
                    AddRaster(1);
                }
                if (rgRasterNumbers.SelectedIndex == 1)
                {
                    if (objBGAEB == null)
                        objBGAEB = new BGAEB();
                    txtNewRaster.Text = _NewRaster = objBGAEB.GetOld_Raster(_ProjectID);;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtNewRaster.Text))
                {
                    if (Utility._IsGermany == true)
                    {
                        _isValidate = false;
                        throw new Exception("Bitte machen Sie gültige Angaben");
                    }
                    else
                    {
                        _isValidate = false;
                        throw new Exception("Please Enter Valid Value");
                    }
                }
                _NewRaster = txtNewRaster.Text;
                _isValidate = true;
                this.Close();
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void frmAddRaster_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.DialogResult != DialogResult.Cancel)
                {
                    if (_isValidate == false)
                        e.Cancel = true;
                }
                else
                    e.Cancel = false;
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void frmAddRaster_Load(object sender, EventArgs e)
        {
            try
            {
                if (objBGAEB == null)
                    objBGAEB = new BGAEB();
                string strTemp = objBGAEB.GetOld_Raster(_ProjectID);
                if (!string.IsNullOrEmpty(strTemp))
                    rgRasterNumbers.Properties.Items[0].Enabled = false;
                else
                    rgRasterNumbers.Properties.Items[1].Enabled = false;
                rgRasterNumbers.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void txtOldRaster_Enter(object sender, EventArgs e)
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


        #region Functions
        /// <summary>
        /// This function contains code to increase or decrease the existing raster size
        /// </summary>
        /// <param name="_Number"></param>
        private void AddRaster(int _Number)
        {
            try
            {
                string[] Levels = txtOldRaster.Text.Split('.');
                if (Levels != null)
                {
                    int Count = Levels.Length;
                    if (Count == 3)//99.1111.9
                    {
                        for (int i = 1; i <= _Number; i++)
                        {
                            Levels[0] = '9' + Levels[0];
                            Levels[1] = '1' + Levels[1];
                        }
                    }
                    else if (Count == 4)//99.99.1111.9
                    {
                        for (int i = 1; i <= _Number; i++)
                        {
                            Levels[0] = '9' + Levels[0];
                            Levels[1] = '9' + Levels[1];
                            Levels[2] = '1' + Levels[2];
                        }
                    }
                    else if (Count == 5)//99.99.99.1111.9
                    {
                        for (int i = 1; i <= _Number; i++)
                        {
                            Levels[0] = '9' + Levels[0];
                            Levels[1] = '9' + Levels[1];
                            Levels[2] = '9' + Levels[2];
                            Levels[3] = '1' + Levels[3];
                        }
                    }
                    else if (Count == 6)//99.99.99.99.1111.9
                    {
                        for (int i = 1; i <= _Number; i++)
                        {
                            Levels[0] = '9' + Levels[0];
                            Levels[1] = '9' + Levels[1];
                            Levels[2] = '9' + Levels[2];
                            Levels[3] = '9' + Levels[3];
                            Levels[4] = '1' + Levels[4];
                        }
                    }
                }
                String result = string.Join(".", Levels);
                txtNewRaster.Text = result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}