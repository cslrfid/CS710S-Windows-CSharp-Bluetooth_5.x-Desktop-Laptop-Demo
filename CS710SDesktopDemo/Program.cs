using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS710SDesktopDemo
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());

            // Fix can not disconnect reader issue
            if (System.Windows.Forms.Application.MessageLoop)
            {
                // WinForms 應用程式
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                // 控制台應用程式
                System.Environment.Exit(0); // 非零值可用於自訂錯誤碼
            }
        }
    }
}
