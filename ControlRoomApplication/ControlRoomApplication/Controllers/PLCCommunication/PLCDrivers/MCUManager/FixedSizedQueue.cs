using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager
{
    public class FixedSizedQueue<T> : ConcurrentQueue<T>
    {
        private readonly object syncObject = new object();

        public int Size { get; private set; }

        public FixedSizedQueue(int size)
        {
            Size = size;
        }

        public new void Enqueue(T obj)
        {
            base.Enqueue(obj);
            lock (syncObject)
            {
                while (base.Count > Size)
                {
                    T outObj;
                    base.TryDequeue(out outObj);
                }
            }
        }
        public MCUPositonStore GetAbsolutePosChange()
        {
            if (typeof(T) == typeof(MCUPositonStore))
            {
                var en = base.GetEnumerator();
                MCUPositonStore x, y, sum = new MCUPositonStore();
                try
                {
                    en.MoveNext();
                    x = en.Current as MCUPositonStore;
                    while (en.MoveNext())
                    {
                        y = en.Current as MCUPositonStore;
                        sum.SUMAbsolute(y, x);
                        x = y;
                    }
                }
                catch (Exception err)
                {
                    Console.WriteLine(err);
                }
                return sum;
            }
            else return new MCUPositonStore();
        }
    }
}
