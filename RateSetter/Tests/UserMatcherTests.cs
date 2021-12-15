using System.Collections.Generic;
using RateSetter.Sources;
using RateSetter.Sources.Geolocations;
using RateSetter.Sources.Settings;
using RateSetter.Sources.UserMatcherRules;
using Xunit;

namespace RateSetter.Tests
{
    public class UserMatcherTests
    {
        [Fact]
        public void IsMatch_MatchNameAndAddress_ExpectMatch()
        {
            var existingUser = new User
            {
                Name = "John",
                ReferralCode = "ABCD1234",
                Address = new Address
                {
                    StreetAddress = "Tower 1, 1 Alexander street",
                    Suburb = "District One",
                    State = "Paris",
                    Latitude = 10.771525m,
                    Longitude = 106.698359m
                }
            };

            var newUser = new User
            {
                Name = " John ", // Match name
                ReferralCode = "4312DCBA", // Not Match Referral Code
                Address = new Address
                {
                    StreetAddress = "@!$%^!@!#Tower_1,_1_Alexander_street", // Match StreetAddress
                    Suburb = "@!$%^!@!#District+One?", // Match Suburb
                    State = "@!$%^!@!#[Paris]@!$%^!@!#", // Match State
                    Latitude = 10.770094m, // Not Match Latitude
                    Longitude = 106.693742m // Not Match Longitude
                }
            };

            var userMatcher = UserMatcher();

            Assert.True(userMatcher.IsMatch(newUser, existingUser));
        }

        [Fact]
        public void IsMatch_MatchInDistance_ExpectMatch()
        {
            var existingUser = new User
            {
                Name = "John",
                ReferralCode = "ABCD1234",
                Address = new Address
                {
                    StreetAddress = "Tower 1, 1 Alexander street",
                    Suburb = "District One",
                    State = "Paris",
                    Latitude = 10.771525m,
                    Longitude = 106.698359m
                }
            };

            var newUser = new User
            {
                Name = " Jane ", // Not Match name
                ReferralCode = "4312DCBA", // Not Match Referral Code
                Address = new Address
                {
                    StreetAddress = "@!$%^!@!#Tower_1,_132_Alexander_street", // Not Match StreetAddress
                    Suburb = "@!$%^!@!#District+Two?", // Not Match Suburb
                    State = "@!$%^!@!#[New York]@!$%^!@!#", // Not Match State
                    Latitude = 10.77206m, // Match Latitude
                    Longitude = 106.698298m // Match Longitude
                }
            };

            var userMatcher = UserMatcher();

            Assert.True(userMatcher.IsMatch(newUser, existingUser));
        }

        [Fact]
        public void IsMatch_MatchReferralCode_ExpectMatch()
        {
            var existingUser = new User
            {
                Name = "John",
                ReferralCode = "ABCD1234",
                Address = new Address
                {
                    StreetAddress = "Tower 1, 1 Alexander street",
                    Suburb = "District One",
                    State = "Paris",
                    Latitude = 10.771525m,
                    Longitude = 106.698359m
                }
            };

            var newUser = new User
            {
                Name = " Jane ", // Not Match name
                ReferralCode = "ADCB1234", // Match Referral Code
                Address = new Address
                {
                    StreetAddress = "@!$%^!@!#Tower_1,_1_Alexander_street", // Not Match StreetAddress
                    Suburb = "@!$%^!@!#District+One?", // Not Match Suburb
                    State = "@!$%^!@!#[Paris]@!$%^!@!#", // Not Match State
                    Latitude = 10.770094m, // Not Match Latitude
                    Longitude = 106.693742m // Not Match Longitude
                }
            };

            var userMatcher = UserMatcher();

            Assert.True(userMatcher.IsMatch(newUser, existingUser));
        }

        [Fact]
        public void IsMatch_ExpectNotMatch()
        {
            var existingUser = new User
            {
                Name = "John",
                ReferralCode = "ABCD1234",
                Address = new Address
                {
                    StreetAddress = "Tower 1, 1 Alexander street",
                    Suburb = "District One",
                    State = "Paris",
                    Latitude = 10.771525m,
                    Longitude = 106.698359m
                }
            };

            var newUsers = new List<User>
            {
                // Name not match
                new User
                {
                    Name = " John H.",
                    ReferralCode = "ADCB1234",
                    Address = new Address
                    {
                        StreetAddress = "@!$%^!@!#Tower_1,_1_Alexander_street",
                        Suburb = "@!$%^!@!#District+One?",
                        State = "@!$%^!@!#[Paris]@!$%^!@!#",
                        Latitude = 10.77206m,
                        Longitude = 106.698298m
                    }
                },
                // Address not match
                new User
                {
                    Name = " John ",
                    ReferralCode = "ADCB1234",
                    Address = new Address
                    {
                        StreetAddress = "@!$%^!@!#Tower_1,_1_Alexander_HM_street",
                        Suburb = "@!$%^!@!#District+Two?",
                        State = "@!$%^!@!#[New York]@!$%^!@!#",
                        Latitude = 10.77206m,
                        Longitude = 106.698298m
                    }
                },
                // Referral Code not match
                new User
                {
                    Name = " John ",
                    ReferralCode = "ADCBE1234",
                    Address = new Address
                    {
                        StreetAddress = "@!$%^!@!#Tower_1,_1_Alexander_street",
                        Suburb = "@!$%^!@!#District+One?",
                        State = "@!$%^!@!#[Paris]@!$%^!@!#",
                        Latitude = 10.77206m,
                        Longitude = 106.698298m
                    }
                },
                // Distance is not in range
                new User
                {
                    Name = " John ",
                    ReferralCode = "ADCB1234",
                    Address = new Address
                    {
                        StreetAddress = "@!$%^!@!#Tower_1,_1_Alexander_street",
                        Suburb = "@!$%^!@!#District+One?",
                        State = "@!$%^!@!#[Paris]@!$%^!@!#",
                        Latitude = 10.770094m,
                        Longitude = 106.693742m
                    }
                }
            };

            foreach (var newUser in newUsers)
            {
                var userMatcher = UserMatcher();

                Assert.True(userMatcher.IsMatch(newUser, existingUser));
            }
        }


        [Fact]
        public void IsMatch_MatchReferralCode_IgnoreReferralCodeRule_ExpectNotMatch()
        {
            var existingUser = new User
            {
                Name = "John",
                ReferralCode = "ABCD1234",
                Address = new Address
                {
                    StreetAddress = "Tower 1, 1 Alexander street",
                    Suburb = "District One",
                    State = "Paris",
                    Latitude = 10.771525m,
                    Longitude = 106.698359m
                }
            };

            var newUser = new User
            {
                Name = " Jane ",
                ReferralCode = "ABCD1234",
                Address = new Address
                {
                    StreetAddress = "@!$%^!@!#Tower_1,_12_Alexander_street",
                    Suburb = "@!$%^!@!#District+Two?",
                    State = "@!$%^!@!#[NewYork]@!$%^!@!#",
                    Latitude = 10.770094m,
                    Longitude = 106.693742m
                }
            };

            var referralCodeRule = new ReferralCodeRule
            {
                IgnoreRule = true,
                CharactersNumber = 3
            };

            var userMatcher = UserMatcher(null, null, referralCodeRule);

            Assert.False(userMatcher.IsMatch(newUser, existingUser));
        }

        [Fact]
        public void IsMatch_InDistance_IgnoreDistanceRule_ExpectNotMatch()
        {
            var existingUser = new User
            {
                Name = "John",
                ReferralCode = "ABCD1234",
                Address = new Address
                {
                    StreetAddress = "Tower 1, 1 Alexander street",
                    Suburb = "District One",
                    State = "Paris",
                    Latitude = 10.771525m,
                    Longitude = 106.698359m
                }
            };

            var newUser = new User
            {
                Name = "John",
                ReferralCode = "ABCDE1234",
                Address = new Address
                {
                    StreetAddress = "@!$%^!@!#Tower_1,_12_Alexander_street",
                    Suburb = "@!$%^!@!#District+Two?",
                    State = "@!$%^!@!#[NewYork]@!$%^!@!#",
                    Latitude = 10.771525m,
                    Longitude = 106.698359m
                }
            };

            var distanceRule = new DistanceRule
            {
                IgnoreRule = true,
                DistanceUnit = DistanceUnit.Meters.ToString(),
                DecimalPlaces = 1,
                DistanceLimit = 500.0
            };
            
            var userMatcher = UserMatcher(distanceRule);
            
            Assert.False(userMatcher.IsMatch(newUser, existingUser));
        }

        [Fact]
        public void IsMatch_NameAndAddress_IgnoreNameAndAddressRule_ExpectNotMatch()
        {
            var existingUser = new User
            {
                Name = "John",
                ReferralCode = "ABCD1234",
                Address = new Address
                {
                    StreetAddress = "Tower 1, 1 Alexander street",
                    Suburb = "District One",
                    State = "Paris",
                    Latitude = 10.771525m,
                    Longitude = 106.698359m
                }
            };

            var newUser = new User
            {
                Name = "John",
                ReferralCode = "ADCBE1234",
                Address = new Address
                {
                    StreetAddress = "Tower 1, 1 Alexander street",
                    Suburb = "District One",
                    State = "Paris",
                    Latitude = 10.770094m,
                    Longitude = 106.693742m
                }
            };

            var nameAndAddressRule = new NameAndAddressRule
            {
                IgnoreRule = true,
            };

            var userMatcher = UserMatcher(null, nameAndAddressRule);

            Assert.False(userMatcher.IsMatch(newUser, existingUser));
        }

        private static UserMatcher UserMatcher(DistanceRule distanceRule = null,
            NameAndAddressRule nameAndAddressRule = null, ReferralCodeRule referralCodeRule = null)
        {
            var distanceMatcher = distanceRule is null
                ? new DistanceMatcher()
                : new DistanceMatcher(distanceRule);

            var nameAndAddressMatcher = nameAndAddressRule is null
                ? new NameAndAddressMatcher()
                : new NameAndAddressMatcher(nameAndAddressRule);

            var referralCodeMatcher = referralCodeRule is null
                ? new ReferralCodeMatcher()
                : new ReferralCodeMatcher(referralCodeRule);
            return new UserMatcher(distanceMatcher, nameAndAddressMatcher, referralCodeMatcher);
        }
    }
}