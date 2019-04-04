using ControlRoomApplication.Controllers;
using ControlRoomApplication.Database;
using ControlRoomApplication.Entities;
using System;
using System.Threading;
using System.Windows.Forms;

namespace ControlRoomApplication.Main
{
    public partial class FreeControlForm : Form
    {
        public Appointment CurrentAppointment { get; set; }
        public Coordinate TargetCoordinate { get; set; }
        public int Increment { get; set; }
        public CoordinateCalculationController CoordCalc { set; get; }
        public ControlRoom controlRoom { get; set; }
        public int rtId { get; set; }

        public FreeControlForm(ControlRoom new_controlRoom, int new_rtId)
        {
            InitializeComponent();
            //ControlRoom
            rtId = new_rtId;
            controlRoom = new_controlRoom;
            // Make coordCalc
            CoordCalc = controlRoom.RadioTelescopeControllers[rtId - 1].CoordinateController;
            // Set increment
            Increment = 1;
            // Add free control appt
            CurrentAppointment = new Appointment();
            CurrentAppointment.StartTime = DateTime.Now.AddSeconds(5);
            CurrentAppointment.EndTime = DateTime.Now.AddMinutes(15);
            CurrentAppointment.Status = AppointmentStatusEnum.REQUESTED;
            CurrentAppointment.Type = AppointmentTypeEnum.FREE_CONTROL;
            CurrentAppointment.SpectraCyberConfig = new SpectraCyberConfig(SpectraCyberModeTypeEnum.CONTINUUM);
            CurrentAppointment.TelescopeId = rtId;
            CurrentAppointment.UserId = 1;
            TargetCoordinate = new Coordinate(0, 0);
            CurrentAppointment.Coordinates.Add(TargetCoordinate);
            DatabaseOperations.AddAppointment(CurrentAppointment);
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
        }

        private void CoordMove()
        {
            CurrentAppointment = DatabaseOperations.GetUpdatedAppointment(CurrentAppointment.Id);
            CurrentAppointment.Coordinates.Add(TargetCoordinate);
            DatabaseOperations.UpdateAppointment(CurrentAppointment);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SetTargetRAText(TargetCoordinate.RightAscension.ToString());
            SetTargetDecText(TargetCoordinate.Declination.ToString());
            Entities.Orientation currentOrienation = controlRoom.RadioTelescopeControllers[rtId - 1].GetCurrentOrientation();
            Coordinate ConvertedPosition = CoordCalc.OrientationToCoordinate(currentOrienation, DateTime.Now);
            SetActualRAText(ConvertedPosition.RightAscension.ToString());
            SetActualDecText(ConvertedPosition.Declination.ToString());
        }

        delegate void SetTargetRATextCallback(string text);
        private void SetTargetRAText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (TargetRATextBox.InvokeRequired)
            {
                SetTargetRATextCallback d = new SetTargetRATextCallback(SetTargetRAText);
                Invoke(d, new object[] { text });
            }
            else
            {
                TargetRATextBox.Text = text;
            }
        }

        delegate void SetTargetDecTextCallback(string text);
        private void SetTargetDecText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (TargetDecTextBox.InvokeRequired)
            {
                SetTargetDecTextCallback d = new SetTargetDecTextCallback(SetTargetDecText);
                Invoke(d, new object[] { text });
            }
            else
            {
                TargetDecTextBox.Text = text;
            }
        }

        delegate void SetActualRATextCallback(string text);
        private void SetActualRAText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (ActualRATextBox.InvokeRequired)
            {
                SetActualRATextCallback d = new SetActualRATextCallback(SetActualRAText);
                Invoke(d, new object[] { text });
            }
            else
            {
                ActualRATextBox.Text = text;
            }
        }

        delegate void SetActualDecTextCallback(string text);
        private void SetActualDecText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (ActualDecTextBox.InvokeRequired)
            {
                SetActualDecTextCallback d = new SetActualDecTextCallback(SetActualDecText);
                Invoke(d, new object[] { text });
            }
            else
            {
                ActualDecTextBox.Text = text;
            }
        }
    }
}
