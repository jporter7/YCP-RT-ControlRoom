using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Controllers;

namespace ControlRoomApplication.GUI
{
    public partial class AzimuthInputDialog : InputDialog
    {
        private double elevationHighLimit;
        private double elevationLowLimit;
        private double elevationPos;
        private double azimuthPos;
        private Timer timer;    // Timer is responsible for constantly updating the form. 
        private string[] values;
        private RadioTelescopeController rtController;

        public AzimuthInputDialog(RadioTelescopeController rtController)
        {
            timer = new Timer();
            timer.Interval = 5;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

            this.rtController = rtController;

            InitializeComponent();
        }

        public double getAzimuthPos()
        {
            return azimuthPos;
        }
        public double getElevationPos()
        {
            return elevationPos;
        }

        // Handle timer tick events 
        public void timer_Tick(Object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("TIMER TICK");

            // Update the limits 
            if (rtController.EnableSoftwareStops)
            {
                elevationHighLimit = rtController.RadioTelescope.maxElevationDegrees;
                elevationLowLimit = rtController.RadioTelescope.minElevationDegrees;
            }
            else
            {
                elevationHighLimit = SimulationConstants.LIMIT_HIGH_EL_DEGREES;
                elevationLowLimit = SimulationConstants.LIMIT_LOW_EL_DEGREES;
            }

            // Update prompt
            promptLabel.Text = "The Radio Telescope is currently set to be type " + rtController.RadioTelescope.teleType + "." +
                            " This script is best run with a telescope type of SLIP_RING.\n\n" +
                            "Please type an a custom orientation containing azimuth between 0 and 360 degrees," +
                                " and elevation between " + elevationLowLimit + " and " + elevationHighLimit +
                                " degrees. Format the entry as a comma-separated list in the format " +
                                "azimuth, elevation. Ex: 55,80";

            // Disable the OK button if the input is invalid 
            try
            {
                if (textBox.Text != "" && textBox.Text.Contains(','))
                {
                    values = textBox.Text.Split(',');

                    // Ensure the format is completely valid before proceeding 
                    if ((values[0] != "" && values[1] != "") && (values[0] != "-" && values[1] != "-"))
                    {
                        Double.TryParse(values[0], out azimuthPos);
                        Double.TryParse(values[1], out elevationPos);

                        if ((azimuthPos > 360 || azimuthPos < 0) || (elevationPos >= elevationHighLimit || elevationPos <= elevationLowLimit) || values.Length != 2)
                        {
                            okButton.Enabled = false;
                        }
                        else
                        {
                            okButton.Enabled = true;
                        }
                    }
                    else
                    {
                        okButton.Enabled = false;
                    }
                }
                else
                {
                    okButton.Enabled = false;
                }
            }
            catch (IndexOutOfRangeException ex) 
            { 
                
            }          
        }
    }
}
