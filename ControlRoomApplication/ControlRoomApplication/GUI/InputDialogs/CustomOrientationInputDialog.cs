using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Entities;

namespace ControlRoomApplication.GUI
{
    public partial class CustomOrientationInputDialog : InputDialog
    {
        private double elevationHighLimit;
        private double elevationLowLimit;
        private double elevationPos;
        private double azimuthPos;
        private string[] values;
        private Regex rx;

        public CustomOrientationInputDialog(ref bool EnableSoftwareStops, ref string teleType, ref double maxElevationDegrees, ref double minElevationDegrees)
        {
            InitializeComponent();

            rx = new Regex(@"[+-]?([0-9]+\.?[0-9]*|\.[0-9]+),[+-]?([0-9]+\.?[0-9]*|\.[0-9]+)");

            okButton.Enabled = false;
            invalidInputLabel.Visible = false;

            textBox.TextChanged += new EventHandler(TextBox_TextChanged);

            if (EnableSoftwareStops)
            {
                elevationHighLimit = maxElevationDegrees;
                elevationLowLimit = minElevationDegrees;
            }
            else
            {
                elevationHighLimit = SimulationConstants.LIMIT_HIGH_EL_DEGREES;
                elevationLowLimit = SimulationConstants.LIMIT_LOW_EL_DEGREES;
            }

            promptLabel.Text = "The Radio Telescope is currently set to be type " + teleType + "." +
                            " This script is best run with a telescope type of SLIP_RING.\n\n" +
                            "Please type an a custom orientation containing azimuth between 0 and 360 degrees," +
                                " and elevation between " + elevationLowLimit + " and " + elevationHighLimit +
                                " degrees. Format the entry as a comma-separated list in the format " +
                                "azimuth, elevation. Ex: 55,80";
        }

        public double GetAzimuthPos()
        {
            return azimuthPos;
        }
        public double GetElevationPos()
        {
            return elevationPos;
        }
        
        public void SetPrompt(string text)
        {
            this.promptLabel.Text = text;
        }

        public void TextBox_TextChanged(Object sender, EventArgs e)
        {
            MatchCollection matches = rx.Matches(textBox.Text);

            System.Diagnostics.Debug.WriteLine("elevationHighLimit: " + elevationHighLimit);
            System.Diagnostics.Debug.WriteLine("elevationLowLimit: " + elevationLowLimit);

            if (matches.Count > 0)
            {
                values = textBox.Text.Split(',');

                Double.TryParse(values[0], out azimuthPos);
                Double.TryParse(values[1], out elevationPos);

                if ((azimuthPos > 360 || azimuthPos < 0) || (elevationPos >= elevationHighLimit || elevationPos <= elevationLowLimit) || values.Length != 2)
                {
                    okButton.Enabled = false;
                    invalidInputLabel.Visible = true;
                }
                else
                {
                    okButton.Enabled = true;
                    invalidInputLabel.Visible = false;
                }
            }
            else
            {
                okButton.Enabled = false;
                invalidInputLabel.Visible = true;
            }
        }
    }
}
