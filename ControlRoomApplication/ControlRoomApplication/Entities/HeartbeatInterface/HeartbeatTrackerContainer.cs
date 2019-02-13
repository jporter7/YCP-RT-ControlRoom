using System;

namespace ControlRoomApplication.Entities
{
    public static class HeartbeatTrackerContainer
    {
        static HeartbeatTracker HeartbeatTracker = HeartbeatTracker.GetInstance();

        internal static void StartTracking(HeartbeatInterface Child)
        {
            HeartbeatTracker.AddChild(Child);
        }

        internal static void StopTracking(HeartbeatInterface Child)
        {
            HeartbeatTracker.RemoveChild(Child);
        }

        internal static void StartHeartbeatRoutine()
        {
            for (int i = 0; i < HeartbeatTracker.GetNumberOfChildren(); i++)
            {
                HeartbeatTracker.GetInterfaceAt(i).BringUpHeartbeatThread();
            }
        }

        internal static bool ChildrenAreAlive()
        {
            for (int i = 0; i < HeartbeatTracker.GetNumberOfChildren(); i++)
            {
                if (!HeartbeatTracker.GetInterfaceAt(i).IsConsideredAlive())
                {
                    Console.WriteLine("Interface " + i.ToString() + " was DEAD.");
                    return false;
                }
            }

            return true;
        }

        internal static void SafelyKillHeartbeatComponents()
        {
            for (int i = 0; i < HeartbeatTracker.GetNumberOfChildren(); i++)
            {
                HeartbeatTracker.GetInterfaceAt(i).BringDownDueToMiscommunication();
            }
        }

        internal static int GetNumberOfChildren()
        {
            return HeartbeatTracker.GetNumberOfChildren();
        }
    }
}