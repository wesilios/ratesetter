using RateSetter.Sources.UserMatcherRules;

namespace RateSetter.Sources
{
    public class UserMatcher : IUserMatcher
    {
        private readonly IDistanceMatcher _distanceMatcher;
        private readonly INameAndAddressMatcher _nameAndAddressMatcher;
        private readonly IReferralCodeMatcher _referralCodeMatcher;

        public UserMatcher(
            IDistanceMatcher distanceMatcher,
            INameAndAddressMatcher nameAndAddressMatcher,
            IReferralCodeMatcher referralCodeMatcher)
        {
            _distanceMatcher = distanceMatcher;
            _nameAndAddressMatcher = nameAndAddressMatcher;
            _referralCodeMatcher = referralCodeMatcher;
        }

        public bool IsMatch(User newUser, User existingUser)
        {
            return _nameAndAddressMatcher.HasNameAddressMatched(newUser, existingUser)
                   || _distanceMatcher.IsInDistance(newUser.Address, existingUser.Address)
                   || _referralCodeMatcher.HasReferralCodeMatched(newUser.ReferralCode, existingUser.ReferralCode);
        }
    }
}