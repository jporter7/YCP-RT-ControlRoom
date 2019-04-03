using ControlRoomApplication.Controllers;
using ControlRoomApplication.Database;
using ControlRoomApplication.Entities;
using System;
using System.Windows.Forms;

namespace ControlRoomApplication.Main
{
    public partial class FreeControlForm : Form
    {
        public Appointment CurrentAppointment { get; set; }
        public Coordinate TargetCoordinate { get; set; }
        public int Increment { get; set; }
        public CoordinateCalculationController CoordCalc { set; get; }

        public FreeControlForm(int telescope_id, Location telescope_location)
        {
            InitializeComponent();
            // Make coordCalc
            CoordCalc = new CoordinateCalculationController(telescope_location);
            // Set increment
            Increment = 1;
            // Add free control appt
            CurrentAppointment = new Appointment();
            CurrentAppointment.StartTime = DateTime.Now.AddSeconds(30);
            CurrentAppointment.EndTime = DateTime.Now.AddMinutes(15);
            CurrentAppointment.Status = AppointmentStatusEnum.REQUESTED;
            CurrentAppointment.Type = AppointmentTypeEnum.FREE_CONTROL;
            CurrentAppointment.SpectraCyberConfig = new SpectraCyberConfig(SpectraCyberModeTypeEnum.CONTINUUM);
            CurrentAppointment.TelescopeId = telescope_id;
            CurrentAppointment.UserId = 1;
            TargetCoordinate = new Coordinate(0, 0);
            CurrentAppointment.Coordinates.Add(TargetCoordinate);
            DatabaseOperations.AddAppointment(CurrentAppointment);

            UpdateText();
        }

        private void PosDecButton_Click(object sender, EventArgs e)
        {
            TargetCoordinate = new Coordinate(TargetCoordinate.RightAscension, TargetCoordinate.Declination + Increment);
            CoordMove();
        }

        private void NegDecButton_Click(object sender, EventArgs e)
        {
            TargetCoordinate = new Coordinate(TargetCoordinate.RightAscension, TargetCoordinate.Declination - Increment);
            CoordMove();
        }

        private void NegRAButton_Click(object sender, EventArgs e)
        {
            TargetCoordinate = new Coordinate(TargetCoordinate.RightAscension - Increment, TargetCoordinate.Declination);
            CoordMove();
        }

        private void PosRAButton_Click(object sender, EventArgs e)
        {
            TargetCoordinate = new Coordinate(TargetCoordinate.RightAscension + Increment, TargetCoordinate.Declination);
            CoordMove();
        }

        private void CalibrateButton_Click(object sender, EventArgs e)
        {
            CalibrateMove();
        }

        public void CalibrateMove()
        {
            CurrentAppointment = DatabaseOperations.GetUpdatedAppointment(CurrentAppointment.Id);
            CurrentAppointment.Orientation = new Entities.Orientation(0, 0);
            DatabaseOperations.UpdateAppointment(CurrentAppointment);
            TargetCoordinate = CoordCalc.OrientationToCoordinate(CurrentAppointment.Orientation, DateTime.Now);
            UpdateText();
        }

        private void CoordMove()
        {
            CurrentAppointment = DatabaseOperations.GetUpdatedAppointment(CurrentAppointment.Id);
            CurrentAppointment.Coordinates.Add(TargetCoordinate);
            DatabaseOperations.UpdateAppointment(CurrentAppointment);
            UpdateText();
        }

        private void UpdateText()
        {
            TargetRATextBox.Text = TargetCoordinate.RightAscension.ToString();
            TargetDecTextBox.Text = TargetCoordinate.Declination.ToString();
        }
    }
}
