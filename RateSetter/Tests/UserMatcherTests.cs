using RateSetter.Sources;
using Xunit;

namespace RateSetter.Tests
{
    public class UserMatcherTests
    {
        private static UserMatcher UserMatcher => new UserMatcher(TestCases.UserMatcherSettings.Default);

        [Fact]
        public void IsMatch_MatchNameAndAddress_ExpectMatch()
        {
            var existingUser = TestCases.Users.ExistingUser;

            var newUser = TestCases.Users.MatchedWithNameAndAddressUser;

            Assert.True(UserMatcher.IsMatch(newUser, existingUser));
        }
        
        [Fact]
        public void IsMatch_MatchInDistance_ExpectMatch()
        {
            var existingUser = TestCases.Users.ExistingUser;

            var newUser = TestCases.Users.MatchedWithInDistanceUser;

            Assert.True(UserMatcher.IsMatch(newUser, existingUser));
        }
        
        [Fact]
        public void IsMatch_MatchReferralCode_ExpectMatch()
        {
            var existingUser = TestCases.Users.ExistingUser;

            var newUser = TestCases.Users.MatchedWithReferralCodeUser;

            Assert.True(UserMatcher.IsMatch(newUser, existingUser));
        }

        [Fact]
        public void IsMatch_ExpectNotMatch()
        {
            var existingUser = TestCases.Users.ExistingUser;

            var newUsers = TestCases.Users.NotMatchedUsers;

            foreach (var newUser in newUsers)
            {
                Assert.True(UserMatcher.IsMatch(newUser, existingUser));
            }
        }

        [Fact]
        public void HasNameAddressMatched_ExpectMatch()
        {
            var newUsers = TestCases.Users.UsersHaveNameAddressMatched;

            var existingUser = TestCases.Users.ExistingUser;

            foreach (var newUser in newUsers)
            {
                Assert.True(UserMatcher.HasNameAddressMatched(newUser, existingUser));
            }
        }

        [Fact]
        public void HasNameAddressMatched_ExpectNotMatch()
        {
            var newUsers = TestCases.Users.UsersHaveNameAddressNotMatched;

            var existingUser = TestCases.Users.ExistingUser;

            foreach (var newUser in newUsers)
            {
                Assert.False(UserMatcher.HasNameAddressMatched(newUser, existingUser));
            }
        }

        [Theory]
        [InlineData("ABC123", "ABC123", 3)]
        [InlineData("CBA123", "ABC123", 3)]
        [InlineData("A1CB23", "ABC123", 3)]
        [InlineData("AB21C3", "ABC123", 3)]
        [InlineData("ABC321", "ABC123", 3)]
        [InlineData("ABCABC", "ABCABC", 3)]
        [InlineData("CBAABC", "ABCABC", 3)]
        [InlineData("ABCD123", "ABCD123", 3)]
        [InlineData("CBAD123", "ABCD123", 3)]
        [InlineData("ADCB123", "ABCD123", 3)]
        [InlineData("AB1DC23", "ABCD123", 3)]
        [InlineData("ABC21D3", "ABCD123", 3)]
        [InlineData("ABCD321", "ABCD123", 3)]
        [InlineData("ABCD123", "ABCD123", 4)]
        [InlineData("DCBA123", "ABCD123", 4)]
        [InlineData("A1DCB23", "ABCD123", 4)]
        [InlineData("AB21DC3", "ABCD123", 4)]
        [InlineData("ABC321D", "ABCD123", 4)]
        [InlineData("ABCD123", "ABCD123", 5)]
        [InlineData("1DCBA23", "ABCD123", 5)]
        [InlineData("A21DCB3", "ABCD123", 5)]
        [InlineData("AB321DC", "ABCD123", 5)]
        [InlineData("CBA", "ABC", 4)]
        [InlineData("DCBA", "ABCD", 5)]
        public void HasReferralCodeMatched_ExpectMatch(string newReferralCode, string existingReferralCode,
            int numberCharacterReversed)
        {
            var userMatcherSetting = TestCases.UserMatcherSettings.Default;

            userMatcherSetting.ReferralCodeRule.CharactersNumber = numberCharacterReversed;

            var userMatcher = new UserMatcher(userMatcherSetting);

            var result = userMatcher.HasReferralCodeMatched(newReferralCode, existingReferralCode);

            Assert.True(result);
        }

        [Theory]
        [InlineData("321CBA", "ABC123", 3)]
        [InlineData("21CBA3", "ABC123", 3)]
        [InlineData("31CB2A", "ABC123", 3)]
        [InlineData("3B21CA", "ABC123", 3)]
        [InlineData("A321CB", "ABC123", 3)]
        [InlineData("CBA321", "ABC123", 3)]
        [InlineData("ACB321", "ABC123", 3)]
        [InlineData("CAB321", "ABC123", 3)]
        [InlineData("ABCD123", "ABC123", 3)]
        [InlineData("321DCBA", "ABCD123", 3)]
        [InlineData("321DCBA", "ABCD123", 2)]
        [InlineData("321DCBA", "ABCD123", 5)]
        [InlineData("321DCBA", "ABCD123", 4)]
        public void HasReferralCodeMatched_ExpectNotMatch(string newReferralCode, string existingReferralCode,
            int numberCharacterReversed)
        {
            var userMatcherSetting = TestCases.UserMatcherSettings.Default;

            userMatcherSetting.ReferralCodeRule.CharactersNumber = numberCharacterReversed;

            var userMatcher = new UserMatcher(userMatcherSetting);
            
            var result = userMatcher.HasReferralCodeMatched(newReferralCode, existingReferralCode);

            Assert.False(result);
        }

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

            var existingAddress = TestCases.Users.ExistingUser.Address;

            var result = UserMatcher.IsInDistance(newAddress, existingAddress);

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

            var result = UserMatcher.IsInDistance(newAddress, existingAddress);

            Assert.False(result);
        }

        [Fact]
        public void IsMatch_MatchReferralCode_IgnoreReferralCodeRule_ExpectNotMatch()
        {
            var existingUser = TestCases.Users.ExistingUser;

            var newUser = TestCases.Users.MatchedUserButIgnoreReferralCodeRule;

            var userMatcherSetting = TestCases.UserMatcherSettings.IgnoreReferralCodeRule;

            var userMatcher = new UserMatcher(userMatcherSetting);

            Assert.False(userMatcher.IsMatch(newUser, existingUser));
        }

        [Fact]
        public void IsMatch_InDistance_IgnoreDistanceRule_ExpectNotMatch()
        {
            var existingUser = TestCases.Users.ExistingUser;

            var newUser = TestCases.Users.MatchedUserButIgnoreDistanceRule;

            var userMatcherSetting = TestCases.UserMatcherSettings.IgnoreDistanceRule;

            var userMatcher = new UserMatcher(userMatcherSetting);

            Assert.False(userMatcher.IsMatch(newUser, existingUser));
        }

        [Fact]
        public void IsMatch_NameAndAddress_IgnoreNameAndAddressRule_ExpectNotMatch()
        {
            var existingUser = TestCases.Users.ExistingUser;

            var newUser = TestCases.Users.MatchedUserButIgnoreNameAndAddressRule;

            var userMatcherSetting = TestCases.UserMatcherSettings.IgnoreNameAndAddressRule;

            var userMatcher = new UserMatcher(userMatcherSetting);

            Assert.False(userMatcher.IsMatch(newUser, existingUser));
        }
    }
}