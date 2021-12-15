using System;

namespace RateSetter.Sources.Geolocations
{
    public class Coordinate
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Coordinate()
        {
        }

        public Coordinate(decimal latitude, decimal longitude)
        {
            Latitude = Convert.ToDouble(latitude);
            Longitude = Convert.ToDouble(longitude);
        }

        public bool ValidateCoordinates()
        {
            if (Latitude < -90 || Latitude > 90) return false;
            if (Longitude < -180 || Longitude > 180) return false;

            return true;
        }
    }
}