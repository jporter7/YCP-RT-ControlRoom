using ControlRoomApplication.Controllers;
using ControlRoomApplication.Controllers.BlkHeadUcontroler;
using ControlRoomApplication.Database;
using ControlRoomApplication.Entities;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;


namespace ControlRoomApplication.Main
{
    public class Program
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [STAThread]
        public static void Main(string[] args)
        {
            MainForm mainForm = new MainForm();
            ((log4net.Repository.Hierarchy.Hierarchy)log4net.LogManager.GetLoggerRepository()).Root.AddAppender(mainForm);
            Application.Run(mainForm);
        }
    }
}