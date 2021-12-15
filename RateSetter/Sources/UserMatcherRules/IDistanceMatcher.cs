namespace RateSetter.Sources.UserMatcherRules
{
    public interface IDistanceMatcher
    {
        bool IsInDistance(Address newAddress, Address existingAddress);
    }
}