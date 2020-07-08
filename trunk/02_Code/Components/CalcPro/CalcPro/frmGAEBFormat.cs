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
using EL;

namespace CalcPro
{
    public partial class frmGAEBFormat : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// This form is to select GAEB file format while exporting a supplier proposal
        /// </summary>
        #region  Variables
        private EGAEB ObjEGAEB = null;
        #endregion

        #region Constructors

        public frmGAEBFormat()
        {
            InitializeComponent();
        }

        public frmGAEBFormat(EGAEB _ObjEGAEB)
        {
            ObjEGAEB = _ObjEGAEB;
            InitializeComponent();
        }
        #endregion

        #region Events

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtFilePath.Text = dlg.SelectedPath;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtFileName.Text))
                    throw new Exception("Bitte machen Sie eine Eingabe zum Dateinamen");
                if (!dxValidationProvider1.Validate())
                    return;
                ObjEGAEB.IsSave = true;
                ObjEGAEB.FileNAme = txtFileName.Text;
                ObjEGAEB.OutputPath = txtFilePath.Text;
                if (rgGAEBVersion.SelectedIndex == 0)
                    ObjEGAEB.FileFormat = "D83";
                else if (rgGAEBVersion.SelectedIndex == 1)
                    ObjEGAEB.FileFormat = "P83";
                else if (rgGAEBVersion.SelectedIndex == 2)
                    ObjEGAEB.FileFormat = "X83";
                ObjEGAEB.DeliveryDeadline = dtpDeliveryDeadline.DateTime;
                this.Close();
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ObjEGAEB.IsSave = false;
            this.Close();
        }

        private void frmGAEBFormat_Load(object sender, EventArgs e)
        {
            try
            {
                int IValue = 0;
                IValue = ObjEGAEB.LvRaster.Replace(".",string.Empty).Length;
                dtpDeliveryDeadline.DateTime = DateTime.Now;
                if (IValue > 9)
                {
                    rgGAEBVersion.Properties.Items[0].Enabled = false;
                    rgGAEBVersion.SelectedIndex = 1;
                }
                if (!ObjEGAEB.IsMail)
                {
                    txtProjectNumber.Text = ObjEGAEB.ProjectNumber;
                    txtFileName.Text = ObjEGAEB.ProjectNumber;
                    txtFilePath.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }
                else
                {
                    txtProjectNumber.Text = ObjEGAEB.ProjectNumber;
                    txtFileName.Text = ObjEGAEB.FileNAme;
                    txtFilePath.Text = ObjEGAEB.OutputPath;
                    btnBrowse.Enabled = false;
                    txtFilePath.Properties.ReadOnly = true;
                    txtFileName.Properties.ReadOnly = true;
                }

                dtpDeliveryDeadline.Properties.MinValue = DateTime.Now;
            }
            catch (Exception ex)
            {
                
            }
        }
        #endregion
    }
}