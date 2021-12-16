using RateSetter.Sources.UserMatcherRules;
using Xunit;

namespace RateSetter.Tests
{
    public class ReferralCodeMatcherTests
    {
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
        [InlineData("ABC", "ABC", 4)]
        [InlineData("CBA", "ABC", 4)]
        [InlineData("DCBA", "ABCD", 5)]
        public void HasReferralCodeMatched_ExpectMatch(string newReferralCode, string existingReferralCode,
            int numberCharacterReversed)
        {
            var referralCodeRule = new ReferralCodeRule
            {
                IgnoreRule = false,
                CharactersNumber = numberCharacterReversed
            };

            var referralCodeMatcher = new ReferralCodeMatcher(referralCodeRule);

            var result = referralCodeMatcher.HasReferralCodeMatched(newReferralCode, existingReferralCode);

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
            var referralCodeRule = new ReferralCodeRule
            {
                IgnoreRule = false,
                CharactersNumber = numberCharacterReversed
            };

            var referralCodeMatcher = new ReferralCodeMatcher(referralCodeRule);
            
            var result = referralCodeMatcher.HasReferralCodeMatched(newReferralCode, existingReferralCode);

            Assert.False(result);
        }

        [Theory]
        [InlineData("CBA456", "ABC123", 3)]
        [InlineData("DEF456", "ABC123", 3)]
        [InlineData("CBA   ", "ABC123", 3)]
        [InlineData("DEF123", "ABC123", 3)]
        [InlineData("BAC456", "ABC123", 2)]
        [InlineData("BAD123", "ABC123", 2)]
        public void HasReferralCodeMatched_EachCharacterNotTheSame_ExpectNotMatch(string newReferralCode,
            string existingReferralCode, int numberCharacterReversed)
        {
            var referralCodeRule = new ReferralCodeRule
            {
                IgnoreRule = false,
                CharactersNumber = numberCharacterReversed
            };
            
            var referralCodeMatcher = new ReferralCodeMatcher(referralCodeRule);
            
            var result = referralCodeMatcher.HasReferralCodeMatched(newReferralCode, existingReferralCode);

            Assert.False(result);
        }

        [Theory]
        [InlineData("A", "A", 1)]
        [InlineData("AB", "AB", 2)]
        [InlineData("BA", "AB", 2)]
        [InlineData("ABC", "ABC", 3)]
        [InlineData("CBA", "ABC", 3)]
        [InlineData("ABC123", "ABC123", 3)]
        [InlineData("CBA123", "ABC123", 3)]
        [InlineData("A1CB23", "ABC123", 3)]
        [InlineData("AB21C3", "ABC123", 3)]
        [InlineData("ABC321", "ABC123", 3)]
        public void MatchReferralCode_IgnoreReferralCodeRule_ExpectNotMatch(string newReferralCode,
            string existingReferralCode, int numberCharacterReversed)
        {
            var referralCodeRule = new ReferralCodeRule
            {
                IgnoreRule = true,
                CharactersNumber = numberCharacterReversed
            };
            
            var referralCodeMatcher = new ReferralCodeMatcher(referralCodeRule);
            var result = referralCodeMatcher.HasReferralCodeMatched(newReferralCode, existingReferralCode);

            Assert.False(result);
        }
    }
}