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

        public FreeControlForm()
        {
            InitializeComponent();
            Increment = 1;
            CurrentAppointment = new Appointment();
            CurrentAppointment.StartTime = DateTime.Now.AddSeconds(30);
            CurrentAppointment.EndTime = DateTime.Now.AddMinutes(15);
            CurrentAppointment.Status = AppointmentStatusEnum.REQUESTED;
            CurrentAppointment.Type = AppointmentTypeEnum.FREE_CONTROL;
            CurrentAppointment.SpectraCyberConfig = new SpectraCyberConfig(SpectraCyberModeTypeEnum.CONTINUUM);
            CurrentAppointment.TelescopeId = 1;
            CurrentAppointment.UserId = 1;
            TargetCoordinate = new Coordinate(0, 0);
            CurrentAppointment.Coordinates.Add(TargetCoordinate);
            DatabaseOperations.AddAppointment(CurrentAppointment);
        }

        private void PosDecButton_Click(object sender, EventArgs e)
        {
            TargetCoordinate = new Coordinate(TargetCoordinate.RightAscension, TargetCoordinate.Declination + Increment);
            CurrentAppointment.Coordinates.Add(TargetCoordinate);
            DatabaseOperations.AddAppointment(CurrentAppointment);
        }

        private void NegDecButton_Click(object sender, EventArgs e)
        {
            TargetCoordinate = new Coordinate(TargetCoordinate.RightAscension, TargetCoordinate.Declination - Increment);
            CurrentAppointment.Coordinates.Add(TargetCoordinate);
            DatabaseOperations.AddAppointment(CurrentAppointment);
        }

        private void NegRAButton_Click(object sender, EventArgs e)
        {
            TargetCoordinate = new Coordinate(TargetCoordinate.RightAscension - Increment, TargetCoordinate.Declination);
            CurrentAppointment.Coordinates.Add(TargetCoordinate);
            DatabaseOperations.AddAppointment(CurrentAppointment);
        }

        private void PosRAButton_Click(object sender, EventArgs e)
        {
            TargetCoordinate = new Coordinate(TargetCoordinate.RightAscension + Increment, TargetCoordinate.Declination);
            CurrentAppointment.Coordinates.Add(TargetCoordinate);
            DatabaseOperations.AddAppointment(CurrentAppointment);
        }

        private void CalibrateButton_Click(object sender, EventArgs e)
        {
            CurrentAppointment.Orientation = new Entities.Orientation(0, 0);
            DatabaseOperations.AddAppointment(CurrentAppointment);
        }
    }
}
