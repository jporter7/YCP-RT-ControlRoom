using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiddleManAESKeyGenerator
{
    public partial class frmKeygen : Form
    {
        public frmKeygen()
        {
            InitializeComponent();
        }

        private void btnDocumentation_Click(object sender, EventArgs e)
        {
            // Launch documentation from regular Control Room location
            String filename = Directory.GetCurrentDirectory() + "..\\..\\..\\..\\ControlRoomApplication\\UIDoc.pdf";
            if (File.Exists(filename)) System.Diagnostics.Process.Start(filename);
            else
            {
                // If that doesn't exist, look for documentation in local directory
                filename = Directory.GetCurrentDirectory() + "\\UIDoc.pdf";
                if (File.Exists(filename)) System.Diagnostics.Process.Start(filename);
                else
                {
                    // TODO: Put website link with PDF
                }
            }
        }
    }
}
