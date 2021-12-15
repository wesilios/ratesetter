using RateSetter.Sources.Settings;

namespace RateSetter.Sources.UserMatcherRules
{
    public class ReferralCodeMatcher : IReferralCodeMatcher
    {
        private readonly ReferralCodeRule _referralCodeRule;

        public ReferralCodeMatcher()
        {
            _referralCodeRule = new ReferralCodeRule
            {
                IgnoreRule = false,
                CharactersNumber = 3
            };
        }

        public ReferralCodeMatcher(ReferralCodeRule referralCodeRule)
        {
            _referralCodeRule = referralCodeRule;
        }

        public bool HasReferralCodeMatched(string newCode, string existingCode)
        {
            if (_referralCodeRule.IgnoreRule) return false;

            if (!newCode.Length.Equals(existingCode.Length)) return false;

            if (newCode.Equals(existingCode)) return false;

            var range = _referralCodeRule.CharactersNumber - 1;
            if (range >= existingCode.Length)
            {
                range = existingCode.Length - 1;
            }

            var startIndex = 0;
            while (startIndex + range < existingCode.Length)
            {
                var i = startIndex;
                var j = startIndex + range;
                var reversed = existingCode.ToCharArray();
                while (i < j)
                {
                    (reversed[i], reversed[j]) = (reversed[j], reversed[i]);
                    i++;
                    j--;
                }

                if (newCode.Equals(new string(reversed)))
                {
                    return true;
                }

                startIndex++;
            }

            return false;
        }
    }
}