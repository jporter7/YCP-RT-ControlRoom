namespace ControlRoomApplication.Entities
{
    public class CelestialBody
    {
        public CelestialBody()
        {

        }

        public CelestialBody(string name, string coordinates)
        {
            Name = name;
            Coordinates = coordinates;
        }

        public string Name { get; set; }
        public string Coordinates { get; set; }
    }
}