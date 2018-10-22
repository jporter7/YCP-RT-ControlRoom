using System;

namespace ControlRoomApplication.Entities.Coordinate
{
    class Coordinate
    {
        readonly DateTime _ra;
        readonly DateTime _dec;

        public Coordinate()
        {

        }
        
        public DateTime RA { get; set; }
        public DateTime Dec { get; set; }
    }
}
