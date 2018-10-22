using System;

namespace ControlRoomApplication.Entities.Coordinate
{
    class Coordinate
    {
        readonly double _latitude;
        readonly double _longitude;

        public Coordinate(double latitude, double longitude)
        {
            this._latitude = latitude;
            this._longitude = longitude;
        }
        
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
