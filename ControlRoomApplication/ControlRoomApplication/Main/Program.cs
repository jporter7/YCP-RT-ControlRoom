using System;
using System.Windows.Forms;

namespace ControlRoomApplication.Main
{
    public class Program
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [STAThread]
        public static void Main(string[] args)
        {
            Application.Run(new MainForm());
        }
    }
}