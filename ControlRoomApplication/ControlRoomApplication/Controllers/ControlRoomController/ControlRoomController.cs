using System;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Controllers.RadioTelescopeControllers;

namespace ControlRoomApplication.Controllers
{
    public class ControlRoomController
    {
        public ControlRoom CRoom { get; }

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ControlRoomController(ControlRoom controlRoom)
        {
            CRoom = controlRoom;
        }

        public bool AddRadioTelescopeController(RadioTelescopeController rtController)
        {
            if (CRoom.RadioTelescopes.Contains(rtController.RadioTelescope))
            {
                return false;
            }

            RadioTelescopeControllerManagementThread NewRTMT = new RadioTelescopeControllerManagementThread(rtController, CRoom.DBContext);
            CRoom.RTControllerManagementThreads.Add(NewRTMT);
            return NewRTMT.Start();
        }

        public bool AddRadioTelescopeControllerAndStart(RadioTelescopeController rtController)
        {
            if (AddRadioTelescopeController(rtController))
            {
                return CRoom.RTControllerManagementThreads[CRoom.RTControllerManagementThreads.Count - 1].Start();
            }
            else
            {
                return false;
            }
        }

        public bool RemoveRadioTelescopeControllerAt(int rtControllerIndex, bool waitForAnyTasks)
        {
            if ((rtControllerIndex < 0) || (rtControllerIndex >= CRoom.RTControllerManagementThreads.Count))
            {
                return false;
            }

            RadioTelescopeControllerManagementThread ToBeRemovedRTMT = CRoom.RTControllerManagementThreads[rtControllerIndex];

            if (ToBeRemovedRTMT.Busy && (!waitForAnyTasks))
            {
                ToBeRemovedRTMT.KillWithHardInterrupt();
            }
            else
            {
                ToBeRemovedRTMT.RequestToKill();
            }

            if (ToBeRemovedRTMT.WaitToJoin())
            {
                return CRoom.RTControllerManagementThreads.Remove(ToBeRemovedRTMT);
            }
            else
            {
                return false;
            }
        }

        public bool RemoveRadioTelescopeController(RadioTelescopeController rtController, bool waitForAnyTasks)
        {
            return RemoveRadioTelescopeControllerAt(CRoom.RadioTelescopeControllers.IndexOf(rtController), waitForAnyTasks);
        }
    }
}