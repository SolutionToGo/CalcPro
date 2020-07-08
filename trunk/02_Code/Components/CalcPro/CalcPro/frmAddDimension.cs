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
using BL;

namespace CalcPro
{
    public partial class frmAddDimension : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// This form is to add new dimensions to article
        /// </summary>

        #region Varibales
        List<Control> Requirefields = new List<Control>();
        BArticles ObjBArticle = null;
        private EArticles _ObjEArticle = null;
        bool _isValidate = false;

        #region PROPERTY SETTING

        public EArticles ObjEArticle
        {
            get
            {
                return _ObjEArticle;
            }
            set
            {
                _ObjEArticle = value;
            }
        }

        #endregion
        #endregion

        #region Constructors
        public frmAddDimension()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Utility.ValidateRequiredFields(Requirefields))
                    return;
                if (ObjBArticle == null)
                    ObjBArticle = new BArticles();
                ParseSDimensionDetails();
                ObjBArticle = new BArticles();
                ObjBArticle.SaveDimension(_ObjEArticle);
                _isValidate = true;
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void frmAddDimension_Load(object sender, EventArgs e)
        {
            try
            {
                Requirefields.Add(txtA);
                Requirefields.Add(txtB);
                Requirefields.Add(txtListenPrice);
                Requirefields.Add(txtMinuten);
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtA_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && (e.KeyChar) != '\b' && e.KeyChar != '.')
                e.Handled = true;
        }

        private void frmAddDimension_FormClosing(object sender, FormClosingEventArgs e)
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

        private void txtA_Enter(object sender, EventArgs e)
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
        /// Assigning Dimension values from controls to properties in Entity layer
        /// </summary>
        private void ParseSDimensionDetails()
        {
            try
            {
                decimal dValue = 0;
                _ObjEArticle.A = txtA.Text;
                _ObjEArticle.B = txtB.Text;
                _ObjEArticle.L = txtL.Text;
                if (decimal.TryParse(txtListenPrice.Text, out dValue))
                    _ObjEArticle.ListPrice = dValue;
                if (decimal.TryParse(txtMinuten.Text, out dValue))
                    _ObjEArticle.Minuten = dValue;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}