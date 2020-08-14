using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraSplashScreen;
using DevExpress.LookAndFeel;

namespace CalcPro
{
    public partial class frmSplashScreen1 : SplashScreen
    {
        public frmSplashScreen1()
        {
            InitializeComponent();
            this.labelControl1.Text = "Copyright © 1998-" + DateTime.Now.Year.ToString();
            this.labelControl3.Text = "Version : V1.1";
            marqueeProgressBarControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            marqueeProgressBarControl1.LookAndFeel.SkinName = "CategisSkin";

        }

        #region Overrides

        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }
        
        #endregion

        public enum SplashScreenCommand
        {

        }

    }
}