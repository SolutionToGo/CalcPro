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
    public partial class frmSaveDimension : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// This form is to Save list of dimensions with new validity date and new values
        /// </summary>
        #region Varibales
        public EArticles ObjEArticle = null;
        public BArticles ObjBArticle = null;
        public string strArticle = string.Empty;
        string _Formtype = string.Empty;
        string _ValidDate = string.Empty;
        int _Wiid = 0;
        bool Iscopy = false;
        #endregion

        #region Constructors
        public frmSaveDimension()
        {
            InitializeComponent();
        }

        public frmSaveDimension(string _type,string _validityDate)
        {
            InitializeComponent();
            _Formtype = _type;
            _ValidDate = _validityDate;
        }

        public frmSaveDimension(string _type,string _validityDate,int _id)
        {
            InitializeComponent();
            _Formtype = _type;
            _ValidDate = _validityDate;
            _Wiid = _id;
        }
        #endregion

        #region Events
        private void frmSaveDimension_Load(object sender, EventArgs e)
        {
            try
            {
                if(_Formtype=="Dimension")
                {
                    if (ObjEArticle == null)
                        ObjEArticle = new EArticles();
                    ObjEArticle.IsCopy = true;
                    lblArticle.Text = strArticle;
                    dateEditGultigkeit.DateTime = DateTime.Now;
                    BindDimensions(ObjEArticle.WIID);
                    dateEditGultigkeit.ReadOnly = false;
                    dateEditGultigkeit.Properties.MinValue = DateTime.Now;
                    gvDimensions.BestFitColumns();
                }
                else if(_Formtype=="ValidityDate")
                {
                    if (ObjEArticle == null)
                        ObjEArticle = new EArticles();
                    ObjEArticle.IsCopy = false;
                    DateTime _date= Convert.ToDateTime(_ValidDate);
                    if (DateTime.TryParse(_ValidDate, out _date))
                    {
                        dateEditGultigkeit.EditValue = _date;
                    }
                    BindDimensionsValidityDate(_Wiid, _date);
                    dateEditGultigkeit.ReadOnly = true;
                    lblArticle.Text = strArticle;
                    gvDimensions.BestFitColumns();
                }
                
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ObjBArticle == null)
                    ObjBArticle = new BArticles();
                ObjEArticle.ValidityDate = dateEditGultigkeit.DateTime;
                ObjEArticle.dtDim = ((DataView)gvDimensions.DataSource).ToTable();
                ObjEArticle = ObjBArticle.SaveDimensionCopy(ObjEArticle);
                MessageBox.Show("Maße / Artikeldaten mit neuem Gültigkeitsdatum wurden gespeichert : " + string.Format("{0:y}", dateEditGultigkeit.DateTime));
                this.Close();
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

        private void BindDimensionsValidityDate(int WIID,DateTime _Validity)
        {
            try
            {
                DataTable _dtData = new DataTable();
                if (ObjBArticle == null)
                    ObjBArticle = new BArticles();
                _dtData = ObjBArticle.GetValidityDatesDimensions(WIID, _Validity);
               if (_dtData !=null)
               {
                   gcDimensions.DataSource = _dtData;
                   gvDimensions.BestFitColumns();
               }                
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Functions

        /// <summary>
        /// Code to bind dimensions send of selected article
        /// </summary>
        /// <param name="WIID"></param>
        private void BindDimensions(int WIID)
        {
            try
            {
                DataView dvDimensions = ObjEArticle.dtDimenstions.DefaultView;
                dvDimensions.RowFilter = "WIID = '" + WIID + "'";
                gcDimensions.DataSource = dvDimensions.ToTable();
                gvDimensions.BestFitColumns();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}