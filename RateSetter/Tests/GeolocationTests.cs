using System;
using System.Collections.Generic;
using RateSetter.Sources.Geolocations;
using Xunit;

namespace RateSetter.Tests
{
    public class GeolocationTests
    {
        [Fact]
        public void GetDistanceInMeters()
        {
            var originCoordinate = new Coordinate { Latitude = 51.5007, Longitude = 0.1246 };
            var destinationCoordinate = new Coordinate { Latitude = 40.6892, Longitude = 74.0445 };
            const double expectedResult = 5574840.5;

            var distance = Geolocation.GetDistance(originCoordinate, destinationCoordinate, 1);

            Assert.Equal(expectedResult, distance);
        }

        [Fact]
        public void GetDistanceInKilometers()
        {
            var originCoordinate = new Coordinate { Latitude = 51.5007, Longitude = 0.1246 };
            var destinationCoordinate = new Coordinate { Latitude = 40.6892, Longitude = 74.0445 };
            const double expectedResult = 5574.8;

            var distance = Geolocation.GetDistance(originCoordinate, destinationCoordinate, 1, DistanceUnit.Kilometers);

            Assert.Equal(expectedResult, distance);
        }

        [Fact]
        public void GetDistanceInMiles()
        {
            var originCoordinate = new Coordinate { Latitude = 51.5007, Longitude = 0.1246 };
            var destinationCoordinate = new Coordinate { Latitude = 40.6892, Longitude = 74.0445 };
            const double expectedResult = 3464.3;

            var distance = Geolocation.GetDistance(originCoordinate, destinationCoordinate, 1, DistanceUnit.Miles);

            Assert.Equal(expectedResult, distance);
        }

        [Fact]
        public void GetDistanceThrowsArgumentExceptionWithInvalidOriginCoordinates()
        {
            var originCoordinate = new Coordinate { Latitude = 92.0000, Longitude = -118.3977091 };
            var destinationCoordinate = new Coordinate { Latitude = 40.6892, Longitude = 74.0445 };

            var ex = Assert.Throws<ArgumentException>(() =>
                Geolocation.GetDistance(originCoordinate, destinationCoordinate));

            Assert.Equal("Invalid origin coordinates.", ex.Message);
        }

        [Fact]
        public void GetDistanceThrowsArgumentExceptionWithInvalidDestinationCoordinates()
        {
            var originCoordinate = new Coordinate { Latitude = 51.5007, Longitude = 0.1246 };
            var destinationCoordinate = new Coordinate { Latitude = 92.0000, Longitude = -118.3977091 };

            var ex = Assert.Throws<ArgumentException>(() =>
                Geolocation.GetDistance(originCoordinate, destinationCoordinate));

            Assert.Equal("Invalid destination coordinates.", ex.Message);
        }

        [Fact]
        public void ValidateCoordinatesLatitudeIsEqualToMinimumOrMaximum()
        {
            var coordinates = new List<Coordinate>
            {
                new Coordinate { Latitude = 92.0000, Longitude = -118.3977091 },
                new Coordinate { Latitude = -91.000, Longitude = -118.3977091 }
            };

            foreach (var coordinate in coordinates)
            {
                Assert.False(coordinate.ValidateCoordinates());
            }
        }

        [Fact]
        public void ValidateCoordinatesLongitudeIsEqualToMinimumOrMaximum()
        {
            var coordinates = new List<Coordinate>
            {
                new Coordinate { Latitude = 34.0675918, Longitude = 189.0000 },
                new Coordinate { Latitude = 34.0675918, Longitude = -187.0000 }
            };

            foreach (var coordinate in coordinates)
            {
                Assert.False(coordinate.ValidateCoordinates());
            }
        }
    }
}