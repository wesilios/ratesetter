namespace RateSetter.Sources.UserMatcherRules
{
    public interface INameAndAddressMatcher
    {
        bool HasNameAddressMatched(User newUser, User existingUser);
    }
}