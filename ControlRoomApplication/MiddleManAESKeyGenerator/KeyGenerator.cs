using ControlRoomApplication.Controllers.Communications.Encryption;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
//using ControlRoomApplication.Controllers.Communications.Encryption;

namespace MiddleManAESKeyGenerator
{
    public partial class frmKeygen : Form
    {
        public frmKeygen()
        {
            InitializeComponent();
            retrieveDateUpdated();
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

        private void btnGenKey_Click(object sender, EventArgs e)
        {
            // Generate and assign new keys
            var keys = AES.getNewKeys();
            byte[] key = keys[0];
            byte[] iv = keys[1];

            // Prepare save dialog
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Binary File|*.bin";
            sfd.Title = "Save Key";
            sfd.FileName = "AESKey.bin";

            // Save the AES key
            if(sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(sfd.FileName, key);
                
                // Save the IV key
                sfd.FileName = "IV.bin";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(sfd.FileName, iv);
                    updateDateUpdated();
                }
            }

        }

        // Updates the date.cfg file with the lastest date the keys were updated
        private void updateDateUpdated()
        {
            DateTime date = DateTime.Now;
            File.WriteAllText("date.cfg", date.ToString());
            lblLastGenerated.Text = "Keys Last Generated:\n" + date.ToString("MMMM dd, yyyy");
        }

        // Retrieves the date the key was last generated
        private void retrieveDateUpdated()
        {
            try
            {
                String data = File.ReadAllText("date.cfg");

                // We attempt to parse the date first instead of just directly printing
                // the String to protect against the possibility of file corruption.
                // If the file were corrupted, we do not want it to give misleading
                // information.
                DateTime date;
                if (!DateTime.TryParse(data, out date)) throw new Exception();

                // If it makes it to this point, the String is good data and can be
                // printed to the user interface.
                lblLastGenerated.Text += "\n" + date.ToString("MMMM dd, yyyy");
            }

            // If the key was never generated, or the config file is corrupted, 
            // no date will appear.
            catch (Exception e)
            {
                lblLastGenerated.Text += "\nNEVER GENERATED";
            }
        }
    }
}
