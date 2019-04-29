using System;
using System.Threading;
using ControlRoomApplication.Entities;

namespace ControlRoomApplication.Controllers
{
    public abstract class AbstractHardwareCommunicationHandlerBase
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Thread CommunicationThread;
        protected Mutex CommunicationThreadMutex;
        protected bool KillCommunicationThreadFlag;
        public bool HasActiveConnection { get; protected set; }

        public AbstractHardwareCommunicationHandlerBase()
        {
            CommunicationThread = new Thread(new ThreadStart(CommunicationRoutine));
            CommunicationThreadMutex = new Mutex();
            KillCommunicationThreadFlag = false;
            HasActiveConnection = false;

            InitCommunicationThreadElements();
        }

        protected abstract void InitCommunicationThreadElements();

        public void StartCommunicationThread()
        {
            CommunicationThread.Start();
        }

        public void TerminateAndJoinCommunicationThread()
        {
            CommunicationThreadMutex.WaitOne();
            KillCommunicationThreadFlag = true;
            CommunicationThreadMutex.ReleaseMutex();
            CommunicationThread.Join();
        }

        protected abstract void CommunicationRoutine();
    }
}
