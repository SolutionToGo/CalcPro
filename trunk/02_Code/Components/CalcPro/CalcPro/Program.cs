using BL;
using DevExpress.LookAndFeel;
using DevExpress.UserSkins;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;

namespace CalcPro
{
    static class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Assembly asm = typeof(DevExpress.UserSkins.CategisSkin).Assembly;
                DevExpress.Skins.SkinManager.Default.RegisterAssembly(asm);
                // Create a new object, representing the German culture.  
                BonusSkins.Register();
                //UserLookAndFeel.Default.SetSkinStyle("Office 2019 Colorful");
                UserLookAndFeel.Default.SetSkinStyle("CategisSkin");
                var culture = System.Globalization.CultureInfo.CurrentCulture;
                if (culture != null)
                {
                    if (culture.ToString() == "de-DE")
                        Utility._IsGermany = true;
                }
                Thread.CurrentThread.CurrentUICulture = culture;
                Thread.CurrentThread.CurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                log4net.Config.XmlConfigurator.Configure();

                AppDomain.CurrentDomain.FirstChanceException += (sender, e) =>
                {
                    System.Text.StringBuilder msg = new System.Text.StringBuilder();
                    msg.AppendLine("Version 9.5.9 - PROD  ");
                    msg.AppendLine(e.Exception.GetType().FullName);
                    msg.AppendLine(e.Exception.Message);
                    System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
                    msg.AppendLine(st.ToString());
                    msg.AppendLine();
                    Log.Error(msg);
                    Log.Debug(msg);
                    Log.Info(msg);
                };
                Application.Run(new frmNewLogin());
            }
            catch (Exception ex)
            {
                Utility.ShowError(ex);
            }
        }
    }
    public class SkinRegistration : Component
    {
        public SkinRegistration()
        {
            DevExpress.Skins.SkinManager.Default.RegisterAssembly(typeof(DevExpress.UserSkins.CategisSkin).Assembly);
        }
    }
}
