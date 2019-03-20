using System.Collections.Generic;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Controllers.RadioTelescopeControllers;

namespace ControlRoomApplication.Entities
{
    public class ControlRoom
    {
        public List<RadioTelescopeControllerManagementThread> RTControllerManagementThreads { get; }

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

        public List<RadioTelescope.RadioTelescope> RadioTelescopes
        {
            get
            {
                List<RadioTelescope.RadioTelescope> RTList = new List<RadioTelescope.RadioTelescope>();

                foreach (RadioTelescopeControllerManagementThread rtmt in RTControllerManagementThreads)
                {
                    RTList.Add(rtmt.RTController.RadioTelescope);
                }

                return RTList;
            }
        }

        public ControlRoom()
        {
            RTControllerManagementThreads = new List<RadioTelescopeControllerManagementThread>();
        }
    }
}