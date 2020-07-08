using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraWaitForm;

namespace CalcPro
{
    public partial class WaitForm1 : WaitForm
    {
        /// <summary>
        /// This form is to show progress bar throughout application
        /// </summary>
        public WaitForm1()
        {
            InitializeComponent();
            BackColor = Color.Fuchsia;
            TransparencyKey = BackColor;
        }

        #region Overrides

        public override void SetCaption(string caption)
        {
            base.SetCaption(caption);
            this.progressPanel1.Caption = caption;
        }
        public override void SetDescription(string description)
        {
            base.SetDescription(description);
            this.progressPanel1.Description = description;
        }
        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        #endregion

        public enum WaitFormCommand
        {

        }
    }
}