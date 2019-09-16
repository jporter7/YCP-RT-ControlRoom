namespace ControlRoomApplication.Entities
{
    public static class HeartbeatTrackerContainer
    {
        static HeartbeatTracker HeartbeatTracker = HeartbeatTracker.GetInstance();

        public static bool IsTracking(HeartbeatInterface Child)
        {
            return HeartbeatTracker.ContainsChild(Child);
        }

        public static void StartTracking(HeartbeatInterface Child)
        {
            HeartbeatTracker.AddChild(Child);
        }

        public static void StopTracking(HeartbeatInterface Child)
        {
            HeartbeatTracker.RemoveChild(Child);
        }

        public static void StartHeartbeatRoutine()
        {
            for (int i = 0; i < HeartbeatTracker.GetNumberOfChildren(); i++)
            {
                HeartbeatTracker.GetInterfaceAt(i).BringUpHeartbeatThread();
            }
        }

        public static bool ChildrenAreAlive()
        {
            for (int i = 0; i < HeartbeatTracker.GetNumberOfChildren(); i++)
            {
                if (!HeartbeatTracker.GetInterfaceAt(i).IsConsideredAlive())
                {
                    return false;
                }
            }

            return true;
        }

        public static void SafelyKillHeartbeatComponents()
        {
            for (int i = 0; i < HeartbeatTracker.GetNumberOfChildren(); i++)
            {
                HeartbeatTracker.GetInterfaceAt(i).BringDownDueToMiscommunication();
            }
        }

        public static int GetNumberOfChildren()
        {
            return HeartbeatTracker.GetNumberOfChildren();
        }

        public static void clearChildren() {
            HeartbeatTracker.clearHeartbeatTracker();
        }
    }
}