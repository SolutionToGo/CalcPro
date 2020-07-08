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
    public partial class frmResetPassword : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// This form is to reset the user password by himself
        /// </summary>

        #region Constructors
        public frmResetPassword()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtOldPassword.Text == txtNewPassword.Text)
                {
                    if (Utility._IsGermany == true)
                    {
                        throw new Exception("Altes und Neues Passwort dürfen nicht übereinstimmen");
                    }
                    else
                    {
                        throw new Exception("Old Password and New Password should not be same");
                    }
                }
                if (txtNewPassword.Text != txtConfirmPassword.Text)
                {
                    if (Utility._IsGermany == true)
                    {
                        throw new Exception("Die Passwortangaben stimmen nicht überein");
                    }
                    else
                    {
                        throw new Exception("Passwords do not match.");
                    }
                }
                EUserInfo ObjEUserInfo = new EUserInfo();
                BUserInfo ObjBUserInfo = new BUserInfo();
                ObjEUserInfo.UserID = Utility.UserID;
                ObjEUserInfo.OldPassword = Utility.Encrypt(txtOldPassword.Text);
                ObjEUserInfo.NewPassword = Utility.Encrypt(txtNewPassword.Text);
                ObjEUserInfo.IsAdmin = false;
                ObjEUserInfo = ObjBUserInfo.ResetPassword(ObjEUserInfo);
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

        private void frmResetPassword_Load(object sender, EventArgs e)
        {
            txtUserName.Text = Utility.UserName;
            txtUserName.ReadOnly = true;
        }

        private void txtUserName_Enter(object sender, EventArgs e)
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