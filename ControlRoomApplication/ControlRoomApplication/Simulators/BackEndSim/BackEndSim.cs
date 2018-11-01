using ControlRoomApplication.Entities;
using System;
using System.Collections.ObjectModel;

namespace ControlRoomApplication.Simulators.BackEndSim
{
    class BackEndSim
    {
        public BackEndSim() { throw new NotImplementedException("TODO"); }
        
        public Collection<Appointment> RetrieveAppointments()
        {
            throw new NotImplementedException("TODO");
        }

        public void SendAppointmentSchedule(Schedule schedule)
        {
            throw new NotImplementedException("TODO");
        }

        public void AppointmentOverride(DateTime dateTime)
        {
            throw new NotImplementedException("TODO");
        }

        public void RadioTelescopeStatus(RadioTelescopeStatusEnum status)
        {
            throw new NotImplementedException("TODO");
        }

        public void SendRFData(RFData data)
        {
            throw new NotImplementedException("TODO");
        }

        public void SendCurrentOrientation(Orientation orientation)
        {
            throw new NotImplementedException("TODO");
        }
        
        public void RecieveCommand(Command command)
        {
            throw new NotImplementedException("TODO");
        }
    }
}
