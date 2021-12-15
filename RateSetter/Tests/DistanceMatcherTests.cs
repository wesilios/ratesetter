using RateSetter.Sources;
using RateSetter.Sources.Geolocations;
using RateSetter.Sources.UserMatcherRules;
using Xunit;

namespace RateSetter.Tests
{
    public class DistanceMatcherTests
    {
        [Theory]
        [InlineData(10.77206, 106.698298)]
        [InlineData(10.771964, 106.697888)]
        public void IsInDistance_ExpectInDistance(decimal latitude, decimal longitude)
        {
            var newAddress = new Address
            {
                Latitude = latitude,
                Longitude = longitude
            };

            var existingAddress = new Address
            {
                StreetAddress = "Tower 1, 1 Alexander street",
                Suburb = "District One",
                State = "Paris",
                Latitude = 10.771525m,
                Longitude = 106.698359m
            };

            var distanceMatcher = new DistanceMatcher();

            var result = distanceMatcher.IsInDistance(newAddress, existingAddress);

            Assert.True(result);
        }

        [Theory]
        [InlineData(10.770094, 106.693742)]
        [InlineData(10.771558, 106.693440)]
        public void IsInDistance_ExpectNotInDistance(decimal latitude, decimal longitude)
        {
            var newAddress = new Address
            {
                Latitude = latitude,
                Longitude = longitude
            };

            var existingAddress = new Address
            {
                Latitude = 10.7715512m,
                Longitude = 106.6983801m
            };

            var distanceMatcher = new DistanceMatcher();

            var result = distanceMatcher.IsInDistance(newAddress, existingAddress);

            Assert.False(result);
        }

        [Fact]
        public void InDistance_IgnoreDistanceRule_ExpectNotMatch()
        {
            var newAddress = new Address
            {
                Latitude = 10.771525m,
                Longitude = 106.698359m
            };

            var existingAddress = new Address
            {
                Latitude = 10.771525m,
                Longitude = 106.698359m
            };
            
            var distanceRule = new DistanceRule
            {
                IgnoreRule = true,
                DistanceUnit = DistanceUnit.Meters.ToString(),
                DecimalPlaces = 1,
                DistanceLimit = 500.0
            };
            
            var distanceMatcher = new DistanceMatcher(distanceRule);
            
            var result = distanceMatcher.IsInDistance(newAddress, existingAddress);

            Assert.False(result);
        }

        [Theory]
        [InlineData(92.0000, -118.3977091)]
        [InlineData(90.0000, -188.3977091)]
        public void InDistance_newAddressCoordinateOutOfRange_ExpectMatch(decimal latitude, decimal longitude)
        {
            var newAddress = new Address
            {
                Latitude = latitude,
                Longitude = longitude
            };

            var existingAddress = new Address
            {
                Latitude = 10.771525m,
                Longitude = 106.698359m
            };
            
            var distanceMatcher = new DistanceMatcher();

            var result = distanceMatcher.IsInDistance(newAddress, existingAddress);

            Assert.True(result);
        }
        
        [Theory]
        [InlineData(92.0000, -118.3977091)]
        [InlineData(90.0000, -188.3977091)]
        public void InDistance_existingAddressCoordinateOutOfRange_ExpectMatch(decimal latitude, decimal longitude)
        {
            var newAddress = new Address
            {
                Latitude = 10.771525m,
                Longitude = 106.698359m
            };

            var existingAddress = new Address
            {
                Latitude = latitude,
                Longitude = longitude
            };
            
            var distanceMatcher = new DistanceMatcher();

            var result = distanceMatcher.IsInDistance(newAddress, existingAddress);

            Assert.True(result);
        }
    }
}