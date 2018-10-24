namespace ControlRoomApplication.Entities
{
    public class Coordinate
    {
        private readonly double _latitude;
        private readonly double _longitude;

        public Coordinate(double latitude, double longitude)
        {
            _latitude = latitude;
            _longitude = longitude;
        }
        
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
