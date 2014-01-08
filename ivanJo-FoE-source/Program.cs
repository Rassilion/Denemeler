using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using ForgeBot.Diagnostics;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace ForgeBot
{
    using System;
    using System.Windows.Forms;

    internal static class Program
    {
        [DllImport("kernel32.dll")]
        internal static extern int AllocConsole();

        const string BsodFileName = "bsod";
        const string ButtonReloadFileName = "btnreload";

        private static MainForm mainForm;
        [STAThread]
        private static void Main(string []args)
        {
            Application.ThreadException +=
                new ThreadExceptionEventHandler(Application_ThreadException); 

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            bool autoLogin = false, autoStartBot = false, pressAutoReloadButton = false;
            foreach (var arg in args)
            {
                if (arg.ToLowerInvariant().Trim() == "autologin")
                    autoLogin = true;
                else if (arg.ToLowerInvariant().Trim() == "autostartbot")
                    autoStartBot = true;
                else if (arg.ToLowerInvariant().Trim() == "buttonautoreload")
                    pressAutoReloadButton = true;
            }

            Check4BSOD(ref autoLogin, ref autoStartBot,ref pressAutoReloadButton);
            bool restartApp = false ;
            using (var form = new MainForm(autoLogin, autoStartBot, pressAutoReloadButton))
            {
                form.ShowDialog();
                restartApp = form.PerformApplicationRestart;
                pressAutoReloadButton = form.KeepAutoReloadButton;
            }
            if (restartApp)
                RestartApplication(pressAutoReloadButton);

            //try
            //{
            //    Application.Run(new MainForm(autoLogin,autoStartBot));
            //}
            //catch (RestartApplicationNeededException restartNeededEx)
            //{
            //    RestartApplication();
            //}
            //catch (Exception ex)
            //{
            //    LogException(ex);
            //}
        }

        private static void Check4BSOD(ref bool autoLogin, ref bool autoStartBot,ref bool buttonAutoReload)
        {
            if (File.Exists(BsodFileName))
            {
                autoStartBot = true;
                autoLogin = true;
                try
                {
                    File.Delete(BsodFileName);
                }
                catch
                {
                }
            }
            if (File.Exists(ButtonReloadFileName))
            {
                buttonAutoReload = true;
                try
                {
                    File.Delete(ButtonReloadFileName);
                }
                catch
                {
                }
            }

        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            LogException(e.Exception);

            RestartApplication(true);
        }

        private static void LogException(Exception ex)
        {
            Logger.LogError("Unhandled exception in main program.",ex);
        }

        public static void RestartApplication(bool pressAutoReloadButton)
        {
            using (File.Create(BsodFileName))
            {}
            if(pressAutoReloadButton)
                using (File.Create(ButtonReloadFileName))
                { }
            Application.Restart();
        }

    }
}
