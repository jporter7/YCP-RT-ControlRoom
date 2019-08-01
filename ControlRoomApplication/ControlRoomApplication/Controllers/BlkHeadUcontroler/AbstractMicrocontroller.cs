using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ControlRoomApplication.Controllers.BlkHeadUcontroler
{

    /// <summary>
    /// Abstract Class for the Microcontroller
    /// </summary>
    /// 
    public abstract class AbstractMicrocontroller
    {

        public AbstractMicrocontroller() { }

        //FourierTransform Class
        /// <summary>
        /// 
        /// </summary>
        /// 



        /// <summary>
        /// Start listetning for TCP Connection
        /// </summary>
        ///  
        public abstract bool BringUp();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        protected void interpretData(dynamic data)
        {
            double threshold = 0;
            try
            {
                if (data.type == "temp")
                {
                    threshold = 80;
                }
                else if (data.type == "acc")
                {
                    threshold = 1.65;
                }
                else
                {
                    Console.WriteLine("Datatype not found");
                    return;
                }
                foreach (dynamic element in data.data)
                {
                    if (element.val > threshold)
                    {
                        Console.WriteLine(element.val);
                        Console.WriteLine(element.time - DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                        

                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e + "line 229");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// 




    }
}
