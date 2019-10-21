using System;
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

        public bool ContainsChild(HeartbeatInterface Child)
        {
            return Children.Contains(Child);
        }

        public void AddChild(HeartbeatInterface Child)
        {
            if (Children.Contains(Child))
            {
                throw new ArgumentException("Error trying to add a heartbeat child: this heartbeat-tracker already contains this instance.");
            }

            Children.Add(Child);
        }

        public void RemoveChild(HeartbeatInterface Child)
        {
            if (!Children.Contains(Child))
            {
                throw new ArgumentException("Error trying to remove a heartbeat child: this heartbeat-tracker does not contain this instance.");
            }

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
        /// <summary>
        /// only use this for tests
        /// </summary>
        public void clearHeartbeatTracker() {
            Children.Clear();
        }
    }
}