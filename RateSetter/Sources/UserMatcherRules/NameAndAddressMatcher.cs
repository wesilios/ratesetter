using RateSetter.Sources.Extensions;

namespace RateSetter.Sources.UserMatcherRules
{
    public class NameAndAddressMatcher : INameAndAddressMatcher
    {
        private readonly NameAndAddressRule _nameAndAddressRule;

        public NameAndAddressMatcher()
        {
            _nameAndAddressRule = new NameAndAddressRule
            {
                IgnoreRule = false,
            };
        }

        public NameAndAddressMatcher(NameAndAddressRule nameAndAddressRule)
        {
            _nameAndAddressRule = nameAndAddressRule;
        }

        public bool HasNameAddressMatched(User newUser, User existingUser)
        {
            if (_nameAndAddressRule.IgnoreRule) return false;

            if (!newUser.Name.Trim().ToTitleCase().Equals(existingUser.Name.Trim().ToTitleCase()))
            {
                return false;
            }

            var fullNewAddress = $"{newUser.Address.StreetAddress} {newUser.Address.Suburb} {newUser.Address.State}";
            var fullExistingAddress =
                $"{existingUser.Address.StreetAddress} {existingUser.Address.Suburb} {existingUser.Address.State}";

            fullNewAddress = fullNewAddress.TrimSpecialCharacters().TrimDuplicateSpaces();
            fullExistingAddress = fullExistingAddress.TrimSpecialCharacters().TrimDuplicateSpaces();
            return fullNewAddress.Equals(fullExistingAddress);
        }
    }
}