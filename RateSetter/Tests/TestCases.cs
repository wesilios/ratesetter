using System.Collections.Generic;
using RateSetter.Sources;
using RateSetter.Sources.Geolocations;
using RateSetter.Sources.Settings;

namespace RateSetter.Tests
{
    public static class TestCases
    {
        public static class Coordinates
        {
            public static Coordinate ValidOriginCoordinate =>
                new Coordinate { Latitude = 51.5007, Longitude = 0.1246 };

            public static Coordinate ValidDestinationCoordinate =>
                new Coordinate { Latitude = 40.6892, Longitude = 74.0445 };

            public static Coordinate LatitudeAboveToMinimum =>
                new Coordinate { Latitude = -91.000, Longitude = -118.3977091 };

            public static Coordinate LatitudeAboveToMaximum =>
                new Coordinate { Latitude = 92.0000, Longitude = -118.3977091 };

            public static Coordinate LongitudeAboveToMinimum =>
                new Coordinate { Latitude = 34.0675918, Longitude = -187.0000 };

            public static Coordinate LongitudeAboveToMaximum =>
                new Coordinate { Latitude = 34.0675918, Longitude = 189.0000 };
        }

        public static class Users
        {
            public static User ExistingUser => new User
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

            public static IEnumerable<User> UsersHaveNameAddressMatched => new List<User>
            {
                new User
                {
                    Name = "John",
                    Address = new Address
                    {
                        StreetAddress = "@!$%^!@!#Tower_1,_1_Alexander_street",
                        Suburb = "District One",
                        State = "Paris",
                    }
                },
                new User
                {
                    Name = " John ",
                    Address = new Address
                    {
                        StreetAddress = "Tower 1, 1 Alexander street",
                        Suburb = "@!$%^!@!#District+One?",
                        State = "Paris",
                    }
                },
                new User
                {
                    Name = " John",
                    Address = new Address
                    {
                        StreetAddress = "Tower 1, 1 Alexander street",
                        Suburb = "District One",
                        State = "@!$%^!@!#[Paris]@!$%^!@!#",
                    }
                },
                new User
                {
                    Name = "John ",
                    Address = new Address
                    {
                        StreetAddress = "Tower         1,        1 Alexander street",
                        Suburb = "District          One",
                        State = "Paris           ",
                    }
                }
            };

            public static IEnumerable<User> UsersHaveNameAddressNotMatched => new List<User>
            {
                new User
                {
                    Name = "Jane",
                    Address = new Address
                    {
                        StreetAddress = "Tower 1, 1 Alexander street",
                        Suburb = "District One",
                        State = "Paris",
                    }
                },
                new User
                {
                    Name = "John",
                    Address = new Address
                    {
                        StreetAddress = "Tower 1, 1 Alexander street",
                        Suburb = "District One",
                        State = "New York",
                    }
                }
            };

            public static User MatchedWithNameAndAddressUser => new User
            {
                Name = " John ",                                                // Match name
                ReferralCode = "4312DCBA",                                      // Not Match Referral Code
                Address = new Address
                {
                    StreetAddress = "@!$%^!@!#Tower_1,_1_Alexander_street",     // Match StreetAddress
                    Suburb = "@!$%^!@!#District+One?",                          // Match Suburb
                    State = "@!$%^!@!#[Paris]@!$%^!@!#",                        // Match State
                    Latitude = 10.770094m,                                      // Not Match Latitude
                    Longitude = 106.693742m                                     // Not Match Longitude
                }
            };
            
            public static User MatchedWithInDistanceUser => new User
            {
                Name = " Jane ",                                                // Not Match name
                ReferralCode = "4312DCBA",                                      // Not Match Referral Code
                Address = new Address
                {
                    StreetAddress = "@!$%^!@!#Tower_1,_132_Alexander_street",   // Not Match StreetAddress
                    Suburb = "@!$%^!@!#District+Two?",                          // Not Match Suburb
                    State = "@!$%^!@!#[New York]@!$%^!@!#",                     // Not Match State
                    Latitude = 10.77206m,                                       // Match Latitude
                    Longitude = 106.698298m                                     // Match Longitude
                }
            };
            
            public static User MatchedWithReferralCodeUser => new User
            {
                Name = " Jane ",                                                // Not Match name
                ReferralCode = "ADCB1234",                                      // Match Referral Code
                Address = new Address
                {
                    StreetAddress = "@!$%^!@!#Tower_1,_1_Alexander_street",     // Not Match StreetAddress
                    Suburb = "@!$%^!@!#District+One?",                          // Not Match Suburb
                    State = "@!$%^!@!#[Paris]@!$%^!@!#",                        // Not Match State
                    Latitude = 10.770094m,                                      // Not Match Latitude
                    Longitude = 106.693742m                                     // Not Match Longitude
                }
            };
            
            public static IEnumerable<User> NotMatchedUsers => new List<User>
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
            
            public static User MatchedUserButIgnoreReferralCodeRule => new User
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
            
            public static User MatchedUserButIgnoreDistanceRule => new User
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

            public static User MatchedUserButIgnoreNameAndAddressRule => new User
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
        }

        public static class UserMatcherSettings
        {
            public static UserMatcherSetting Default => new UserMatcherSetting
            {
                NameAndAddressRule = new NameAndAddressRule
                {
                    IgnoreRule = false,
                },
                DistanceRule = new DistanceRule
                {
                    IgnoreRule = false,
                    DistanceUnit = DistanceUnit.Meters.ToString(),
                    DecimalPlaces = 1,
                    DistanceLimit = 500.0
                },
                ReferralCodeRule = new ReferralCodeRule
                {
                    IgnoreRule = false,
                    CharactersNumber = 3
                }
            };
            
            public static UserMatcherSetting IgnoreReferralCodeRule => new UserMatcherSetting
            {
                NameAndAddressRule = new NameAndAddressRule
                {
                    IgnoreRule = false,
                },
                DistanceRule = new DistanceRule
                {
                    IgnoreRule = false,
                    DistanceUnit = DistanceUnit.Meters.ToString(),
                    DecimalPlaces = 1,
                    DistanceLimit = 500.0
                },
                ReferralCodeRule = new ReferralCodeRule
                {
                    IgnoreRule = true,
                    CharactersNumber = 3
                }
            };
            
            public static UserMatcherSetting IgnoreDistanceRule => new UserMatcherSetting
            {
                NameAndAddressRule = new NameAndAddressRule
                {
                    IgnoreRule = false,
                },
                DistanceRule = new DistanceRule
                {
                    IgnoreRule = true,
                    DistanceUnit = DistanceUnit.Meters.ToString(),
                    DecimalPlaces = 1,
                    DistanceLimit = 500.0
                },
                ReferralCodeRule = new ReferralCodeRule
                {
                    IgnoreRule = false,
                    CharactersNumber = 3
                }
            };
            
            public static UserMatcherSetting IgnoreNameAndAddressRule => new UserMatcherSetting
            {
                NameAndAddressRule = new NameAndAddressRule
                {
                    IgnoreRule = true,
                },
                DistanceRule = new DistanceRule
                {
                    IgnoreRule = false,
                    DistanceUnit = DistanceUnit.Meters.ToString(),
                    DecimalPlaces = 1,
                    DistanceLimit = 500.0
                },
                ReferralCodeRule = new ReferralCodeRule
                {
                    IgnoreRule = false,
                    CharactersNumber = 3
                }
            };
        }
    }
}