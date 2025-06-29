using System;
using System.Windows.Forms;

namespace MiniTranslate
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Ensure only one instance is running
            bool createdNew;
            using (var mutex = new System.Threading.Mutex(true, "MiniTranslateApp", out createdNew))
            {
                if (createdNew)
                {
                    Application.Run(new MainForm());
                }
                else
                {
                    MessageBox.Show("MiniTranslate is already running. Check the system tray.", 
                        "MiniTranslate", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
} 