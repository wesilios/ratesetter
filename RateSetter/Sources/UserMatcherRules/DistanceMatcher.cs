using System;
using RateSetter.Sources.Geolocations;

namespace RateSetter.Sources.UserMatcherRules
{
    public class DistanceMatcher : IDistanceMatcher
    {
        private readonly DistanceRule _distanceRule;

        public DistanceMatcher()
        {
            _distanceRule = new DistanceRule
            {
                IgnoreRule = false,
                DistanceUnit = DistanceUnit.Meters.ToString(),
                DecimalPlaces = 1,
                DistanceLimit = 500.0
            };
        }

        public DistanceMatcher(DistanceRule distanceRule)
        {
            _distanceRule = distanceRule;
        }

        public bool IsInDistance(Address newAddress, Address existingAddress)
        {
            if (_distanceRule.IgnoreRule) return false;

            var newAddressCoordinate = new Coordinate(newAddress.Latitude, newAddress.Longitude);
            var existingAddressCoordinate = new Coordinate(existingAddress.Latitude, existingAddress.Longitude);
            try
            {
                var distanceUnit =
                    Enum.TryParse<DistanceUnit>(_distanceRule.DistanceUnit, out var result)
                        ? result
                        : DistanceUnit.Meters;
                var distance = Geolocation.GetDistance(newAddressCoordinate, existingAddressCoordinate,
                    _distanceRule.DecimalPlaces, distanceUnit);
                return Math.Round(distance, MidpointRounding.ToZero) <= _distanceRule.DistanceLimit;
            }
            catch (ArgumentException)
            {
                return true;
            }
        }
    }
}