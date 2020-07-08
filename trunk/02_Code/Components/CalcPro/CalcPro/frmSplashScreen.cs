using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraSplashScreen;

namespace CalcPro
{
    public partial class frmSplashScreen : SplashScreen
    {
        /// <summary>
        /// This from is to show progress while opening application
        /// </summary>

        #region Constructors
        public frmSplashScreen()
        {
            InitializeComponent();
        }

        #endregion

        #region Overrides

        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        #endregion

    }
}