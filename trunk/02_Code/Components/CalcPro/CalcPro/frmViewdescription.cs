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

namespace CalcPro
{
    public partial class frmViewdescription : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// This form is for view the long description of a project.
        /// </summary>
        #region Local Variables and Properties
        private string _LongDescription = null;
        public bool _IsSave = false;
        private bool _ISUpdated = false;
        private bool _isFirstTime = true;
        private bool _isNewMode = false;
        frmProject frmParent = null;
        BPosition ObjBPosition = new BPosition();
        private int PositionID = 0;
        public string LongDescription
        {
            get { return _LongDescription; }
            set { _LongDescription = value; }
        }
        #endregion

        #region Constructors
        public frmViewdescription()
        {
            InitializeComponent();
            _isFirstTime = true;

        }

        public frmViewdescription(bool _Mode, frmProject _frmParent,int _PositionID)
        {
            InitializeComponent();
            _isFirstTime = true;
            _isNewMode = _Mode;
            frmParent = _frmParent;
            PositionID = _PositionID;
        }
        #endregion

        #region Events
        private void frmViewdescription_FormClosing(object sender, FormClosingEventArgs e)
        {
            string _dlgResult = "";
            if (_IsSave)
                _LongDescription = txtLongdescription.RtfText;
            else
            {
                if (_ISUpdated)
                {
                    if (!string.IsNullOrEmpty(_dlgResult))
                    {
                        if (_dlgResult.ToLower() == "yes")
                        {
                            _LongDescription = txtLongdescription.RtfText;
                        }
                        else if (_dlgResult.ToLower() == "cancel")
                        {
                            e.Cancel = true;
                        }
                    }
                }
            }
        }

        private void frmViewdescription_Load(object sender, EventArgs e)
        {
            try
            {
                if (_LongDescription != null && !_isNewMode)
                {
                    txtLongdescription.RtfText = _LongDescription;
                    txtLVPosition.EditValue = frmParent.tl.FocusedNode["Position_OZ"];
                    txtShortDescription.EditValue = Utility.GetPlaintext(Convert.ToString(frmParent.tl.FocusedNode["ShortDescription"]));
                }
                else
                {
                    btnPrevious.Enabled = false;
                    btnNext.Enabled = false;
                }
            }
            catch (Exception ex){Utility.ShowError(ex);}
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this._IsSave = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtLongdescription_TextChanged(object sender, EventArgs e)
        {
            if (!_isFirstTime)
            {
                _ISUpdated = true;    
            }
            _isFirstTime = false;
        }

        private void frmViewdescription_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.F1)
                    btnOK_Click(null, null);
                else if (e.KeyData == Keys.PageUp)
                {
                    btnPrevious_Click(null, null);
                    e.Handled = false;
                }
                else if (e.KeyData == Keys.PageDown)
                {
                    btnNext_Click(null, null);
                    e.Handled = false;
                }
            }
            catch (Exception ex){}
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            try
            {

                if (!_isNewMode)
                {
                    ObjBPosition.UpdateLongDescription(PositionID, txtLongdescription.RtfText);
                    frmParent.tl.MovePrev();
                    int ivalue = 0;
                    if (int.TryParse(Convert.ToString(frmParent.tl.FocusedNode["PositionID"]), out ivalue))
                    {
                        txtLongdescription.RtfText = ObjBPosition.GetLongDescription(ivalue);
                        txtLVPosition.EditValue = frmParent.tl.FocusedNode["Position_OZ"];
                        txtShortDescription.EditValue = Utility.GetPlaintext(Convert.ToString(frmParent.tl.FocusedNode["ShortDescription"]));
                        PositionID = ivalue;
                    }
                    else { throw new Exception("Error while retrieving lang text"); }
                }
            }
            catch (Exception ex){ Utility.ShowError(ex); }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_isNewMode)
                {
                    ObjBPosition.UpdateLongDescription(PositionID, txtLongdescription.RtfText);
                    frmParent.tl.MoveNext();
                    int ivalue = 0;
                    if (int.TryParse(Convert.ToString(frmParent.tl.FocusedNode["PositionID"]), out ivalue))
                    {
                        txtLongdescription.RtfText = ObjBPosition.GetLongDescription(ivalue);
                        txtLVPosition.EditValue = frmParent.tl.FocusedNode["Position_OZ"];
                        txtShortDescription.EditValue = Utility.GetPlaintext(Convert.ToString(frmParent.tl.FocusedNode["ShortDescription"]));
                        PositionID = ivalue;
                    }
                    else { throw new Exception("Error while retrieving lang text"); }
                }
            }
            catch (Exception ex) { Utility.ShowError(ex); }
        }
        #endregion
    }
}