using RateSetter.Sources;
using RateSetter.Sources.UserMatcherRules;
using Xunit;

namespace RateSetter.Tests
{
    public class NameAndAddressMatcherTests
    {
        [Theory]
        [InlineData("John", "@!$%^!@!#Tower_1,_1_Alexander_street", "District One", "Paris")]
        [InlineData(" John ", "Tower 1, 1 Alexander street", "@!$%^!@!#District+One?", "Paris")]
        [InlineData(" John", "Tower 1, 1 Alexander street", "District One", "@!$%^!@!#[Paris]@!$%^!@!#")]
        [InlineData("John ", "Tower         1,        1 Alexander street", "District          One", "Paris           ")]
        public void HasNameAddressMatched_ExpectMatch(string name, string streetAddress, string suburb, string state)
        {
            var newUser = new User
            {
                Name = name,
                ReferralCode = "ABCD1234",
                Address = new Address
                {
                    StreetAddress = streetAddress,
                    Suburb = suburb,
                    State = state
                }
            };

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

            var nameAndAddressMatcher = new NameAndAddressMatcher();

            Assert.True(nameAndAddressMatcher.HasNameAddressMatched(newUser, existingUser));
        }

        [Theory]
        [InlineData("Jane", "Tower 1, 1 Alexander street", "District One", "Paris")]
        [InlineData("John", "Tower 1, 1 Alexander street", "District One", "New York")]
        public void HasNameAddressMatched_ExpectNotMatch(string name, string streetAddress, string suburb, string state)
        {
            var newUser = new User
            {
                Name = name,
                ReferralCode = "ABCD1234",
                Address = new Address
                {
                    StreetAddress = streetAddress,
                    Suburb = suburb,
                    State = state
                }
            };

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

            var nameAndAddressMatcher = new NameAndAddressMatcher();

            Assert.False(nameAndAddressMatcher.HasNameAddressMatched(newUser, existingUser));
        }

        [Fact]
        public void HasNameAddressMatched_IgnoreNameAndAddressRule_ExpectNotMatch()
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
            
            var nameAndAddressMatcher = new NameAndAddressMatcher(nameAndAddressRule);
            
            Assert.False(nameAndAddressMatcher.HasNameAddressMatched(newUser, existingUser));
        }
    }
}