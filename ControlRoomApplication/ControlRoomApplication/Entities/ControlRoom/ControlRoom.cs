using System.Collections.Generic;
using System.Linq;
using ControlRoomApplication.Entities.RadioTelescope;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Controllers.RadioTelescopeControllers;
using ControlRoomApplication.Main;

namespace ControlRoomApplication.Entities
{
    public class ControlRoom
    {
        public List<RadioTelescopeControllerManagementThread> RTControllerManagementThreads { get; }
        public RTDbContext DBContext { get; }

        public List<RadioTelescopeController> RadioTelescopeControllers
        {
            get
            {
                List<RadioTelescopeController> rtControllers = new List<RadioTelescopeController>();

                foreach (RadioTelescopeControllerManagementThread rtmt in RTControllerManagementThreads)
                {
                    rtControllers.Add(rtmt.RTController);
                }

                return rtControllers;
            }
        }

        public List<AbstractRadioTelescope> RadioTelescopes
        {
            get
            {
                List<AbstractRadioTelescope> RTList = new List<AbstractRadioTelescope>();

                foreach (RadioTelescopeControllerManagementThread rtmt in RTControllerManagementThreads)
                {
                    RTList.Add(rtmt.RTController.RadioTelescope);
                }

                return RTList;
            }
        }

        public List<Appointment> Appointments
        {
            get
            {
                return DBContext.Appointments.ToList();
            }
            set
            {
                foreach (Appointment appt in value)
                {
                    DBContext.Appointments.Add(appt);
                    DBContext.SaveChanges();
                }
            }
        }

        public ControlRoom(RTDbContext dbContext)
        {
            RTControllerManagementThreads = new List<RadioTelescopeControllerManagementThread>();
            DBContext = dbContext;
        }
    }
}