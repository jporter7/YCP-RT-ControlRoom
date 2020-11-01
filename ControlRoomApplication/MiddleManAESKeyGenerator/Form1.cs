using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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

        private void btnGenKey_Click(object sender, EventArgs e)
        {

            byte[] key;
            byte[] iv;

            // Generate new AES Key and IV
            using(Aes aes = Aes.Create())
            {
                aes.KeySize = 128;
                aes.GenerateKey();
                aes.GenerateIV();
                key = aes.Key;
                iv = aes.IV;
            }

            // Prepare save dialog
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Binary File|*.bin";
            sfd.Title = "Save Key";
            sfd.FileName = "AESKey.bin";

            // Save the AES key
            if(sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(sfd.FileName, key);
            }

            // Save the IV key
            sfd.FileName = "IV.bin";
            if(sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(sfd.FileName, iv);
            }
        }

        private void updateDateUpdated()
        {

        }
    }
}
