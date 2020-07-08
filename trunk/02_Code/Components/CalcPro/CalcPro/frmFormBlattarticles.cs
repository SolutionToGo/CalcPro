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
    public partial class frmFormBlattarticles : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        ///  This form is to show and map articles to costtypes for formblatt reports
        /// </summary>
        #region Variables
        BFormBlatt ObjBFormBlatt = null;
        EFormBlatt ObjEFormBlatt = null;
        int _FormBlattTyPeID = 0;
        EPosition ObjEPosition = null;
        #endregion

        #region Constructor
        public frmFormBlattarticles(EPosition _ObjEPosition)
        {
            InitializeComponent();
            ObjEPosition = _ObjEPosition;
        }

        #endregion

        #region Events

        private void frmFormBlattarticles_Load(object sender, EventArgs e)
        {
            try
            {
                BindBlattType();
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void cmbFormBlatttypes_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (ObjEFormBlatt == null)
                    ObjEFormBlatt = new EFormBlatt();
                if (ObjBFormBlatt == null)
                    ObjBFormBlatt = new BFormBlatt();
                if (cmbFormBlatttypes.Text != string.Empty)
                {
                    if (int.TryParse(Convert.ToString(cmbFormBlatttypes.SelectedValue), out _FormBlattTyPeID))

                        if (_FormBlattTyPeID > 0)
                        {
                            gcFormBlattArticles.DataSource = null;
                            ObjEFormBlatt.LookUpID = _FormBlattTyPeID;
                            ObjEFormBlatt.ProjectID = ObjEPosition.ProjectID;
                            ObjBFormBlatt.Get_FormBlattArticles(ObjEFormBlatt);
                            if (ObjEFormBlatt.dtBlattArticles != null)
                            {
                                gcFormBlattArticles.DataSource = ObjEFormBlatt.dtBlattArticles;
                                gvFormBlattArticles.BestFitColumns();
                            }
                        }
                }

            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }

        private void btnSaveFormBlattArticles_Click(object sender, EventArgs e)
        {
            try
            {
                DataView dvArticles = null;
                DataTable Temp = ObjEFormBlatt.dtBlattArticles.Copy();
                Temp.Columns.Remove("WGDescription");
                Temp.Columns.Remove("WADescription");
                DataTable dt = Temp;
                if (dt != null)
                {
                    dvArticles = dt.DefaultView;
                    dvArticles.RowFilter = "IsAssigned = '" + true + "'";
                }
                if (ObjEFormBlatt == null)
                    ObjEFormBlatt = new EFormBlatt();
                if (ObjBFormBlatt == null)
                    ObjBFormBlatt = new BFormBlatt();
                ObjEFormBlatt = ObjBFormBlatt.Save_FormBlattArticles(ObjEFormBlatt, dvArticles.ToTable());
                if (Utility._IsGermany == true)
                    frmCalcPro.UpdateStatus("Artikelangaben für das Formblatt erfolgreich gespeichert");
                else
                    frmCalcPro.UpdateStatus("FormBlatt Articles Saved Successfully");
            }
            catch (Exception ex){Utility.ShowError(ex);}
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Functions

        /// <summary>
        /// Code to fetch cost types from database and Bind to combobox
        /// </summary>
        private void BindBlattType()
        {
            try
            {
                if (ObjEFormBlatt == null)
                    ObjEFormBlatt = new EFormBlatt();
                if (ObjBFormBlatt == null)
                    ObjBFormBlatt = new BFormBlatt();
                ObjBFormBlatt.Get_FormBlattTypes(ObjEFormBlatt);
                if (ObjEFormBlatt.dtBlattTypes != null)
                {
                    cmbFormBlatttypes.DataSource = ObjEFormBlatt.dtBlattTypes;
                    cmbFormBlatttypes.ValueMember = "LookupID";
                    cmbFormBlatttypes.DisplayMember = "Value";
                    cmbFormBlatttypes.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }
        #endregion
    }
}