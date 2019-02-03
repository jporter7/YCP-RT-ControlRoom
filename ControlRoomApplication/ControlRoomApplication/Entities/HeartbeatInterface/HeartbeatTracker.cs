using System.Collections.Generic;

namespace ControlRoomApplication.Entities
{
    public class HeartbeatTracker
    {
        private static HeartbeatTracker SINGLETON = null;

        private List<HeartbeatInterface> Children;

        private HeartbeatTracker()
        {
            Children = new List<HeartbeatInterface>();
        }

        public void AddChild(HeartbeatInterface Child)
        {
            Children.Add(Child);
        }

        public void RemoveChild(HeartbeatInterface Child)
        {
            Children.Remove(Child);
        }

        public int GetNumberOfChildren()
        {
            return Children.Count;
        }

        public HeartbeatInterface GetInterfaceAt(int index)
        {
            return Children[index];
        }

        public static HeartbeatTracker GetInstance()
        {
            if (SINGLETON == null)
            {
                SINGLETON = new HeartbeatTracker();
            }

            return SINGLETON;
        }
    }
}