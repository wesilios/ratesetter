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
            var originCoordinate = TestCases.Coordinates.ValidOriginCoordinate;
            var destinationCoordinate = TestCases.Coordinates.ValidDestinationCoordinate;
            const double expectedResult = 5574840.5;

            var distance = Geolocation.GetDistance(originCoordinate, destinationCoordinate, 1);

            Assert.Equal(expectedResult, distance);
        }

        [Fact]
        public void GetDistanceInKilometers()
        {
            var originCoordinate = TestCases.Coordinates.ValidOriginCoordinate;
            var destinationCoordinate = TestCases.Coordinates.ValidDestinationCoordinate;
            const double expectedResult = 5574.8;

            var distance = Geolocation.GetDistance(originCoordinate, destinationCoordinate, 1, DistanceUnit.Kilometers);

            Assert.Equal(expectedResult, distance);
        }

        [Fact]
        public void GetDistanceThrowsArgumentExceptionWithInvalidOriginCoordinates()
        {
            var originCoordinate = TestCases.Coordinates.LatitudeAboveToMaximum;
            var destinationCoordinate = TestCases.Coordinates.ValidDestinationCoordinate;

            var ex = Assert.Throws<ArgumentException>(() =>
                Geolocation.GetDistance(originCoordinate, destinationCoordinate));

            Assert.Equal("Invalid origin coordinates.", ex.Message);
        }

        [Fact]
        public void GetDistanceThrowsArgumentExceptionWithInvalidDestinationCoordinates()
        {
            var originCoordinate = TestCases.Coordinates.ValidOriginCoordinate;
            var destinationCoordinate = TestCases.Coordinates.LatitudeAboveToMaximum;

            var ex = Assert.Throws<ArgumentException>(() =>
                Geolocation.GetDistance(originCoordinate, destinationCoordinate));

            Assert.Equal("Invalid destination coordinates.", ex.Message);
        }

        [Fact]
        public void ValidateCoordinatesLatitudeIsEqualToMinimumOrMaximum()
        {
            var coordinates = new List<Coordinate>
            {
                TestCases.Coordinates.LatitudeAboveToMaximum,
                TestCases.Coordinates.LatitudeAboveToMinimum
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
                TestCases.Coordinates.LongitudeAboveToMaximum,
                TestCases.Coordinates.LongitudeAboveToMinimum
            };

            foreach (var coordinate in coordinates)
            {
                Assert.False(coordinate.ValidateCoordinates());
            }
        }
    }
}