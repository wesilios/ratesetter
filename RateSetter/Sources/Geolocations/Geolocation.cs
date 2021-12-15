using System;

namespace RateSetter.Sources.Geolocations
{
    public static class Geolocation
    {
        private const double EarthRadiusInMiles = 3959.0;
        private const double EarthRadiusInKilometers = 6371.0;
        private const double EarthRadiusInMeters = 6371000.0;

        public static double GetDistance(Coordinate originCoordinate, Coordinate destinationCoordinate,
            int decimalPlaces = 6, DistanceUnit distanceUnit = DistanceUnit.Meters)
        {
            if (!originCoordinate.ValidateCoordinates())
            {
                throw new ArgumentException("Invalid origin coordinates.");
            }

            if (!destinationCoordinate.ValidateCoordinates())
            {
                throw new ArgumentException("Invalid destination coordinates.");
            }

            return GetDistance(originCoordinate.Latitude, originCoordinate.Longitude, destinationCoordinate.Latitude,
                destinationCoordinate.Longitude, decimalPlaces, distanceUnit);
        }

        private static double GetDistance(
            double originLatitude,
            double originLongitude,
            double destinationLatitude,
            double destinationLongitude,
            int decimalPlaces = 2,
            DistanceUnit distanceUnit = DistanceUnit.Meters)
        {
            var distance = Math.Round(
                GetRadius(distanceUnit) * 2.0 * Math.Asin(Math.Min(1.0,
                    Math.Sqrt(Math.Pow(Math.Sin(originLatitude.DiffRadian(destinationLatitude) / 2.0), 2.0) +
                              Math.Cos(originLatitude.ToRadian()) * Math.Cos(destinationLatitude.ToRadian()) *
                              Math.Pow(Math.Sin(originLongitude.DiffRadian(destinationLongitude) / 2.0), 2.0)))),
                decimalPlaces);
            return distance;
        }

        private static double GetRadius(DistanceUnit unit)
        {
            return unit switch
            {
                DistanceUnit.Miles => EarthRadiusInMiles,
                DistanceUnit.Kilometers => EarthRadiusInKilometers,
                _ => EarthRadiusInMeters
            };
        }
    }
}