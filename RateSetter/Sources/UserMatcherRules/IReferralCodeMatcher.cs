namespace RateSetter.Sources.UserMatcherRules
{
    public interface IReferralCodeMatcher
    {
        bool HasReferralCodeMatched(string newCode, string existingCode);
    }
}