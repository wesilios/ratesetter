using System;
using RateSetter.Sources.Extensions;
using RateSetter.Sources.Geolocations;
using RateSetter.Sources.Settings;

namespace RateSetter.Sources
{
    public class UserMatcher : IUserMatcher
    {
        private readonly UserMatcherSetting _userMatcherSetting;

        public UserMatcher()
        {
            _userMatcherSetting = new UserMatcherSetting
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
        }

        public UserMatcher(UserMatcherSetting userMatcherSetting)
        {
            _userMatcherSetting = userMatcherSetting;
        }

        public bool IsMatch(User newUser, User existingUser)
        {
            return HasNameAddressMatched(newUser, existingUser)
                   || IsInDistance(newUser.Address, existingUser.Address)
                   || HasReferralCodeMatched(newUser.ReferralCode, existingUser.ReferralCode);
        }

        public bool HasNameAddressMatched(User newUser, User existingUser)
        {
            if (_userMatcherSetting.NameAndAddressRule.IgnoreRule) return false;

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

        public bool IsInDistance(Address newAddress, Address existingAddress)
        {
            if (_userMatcherSetting.DistanceRule.IgnoreRule) return false;

            var newAddressCoordinate = new Coordinate(newAddress.Latitude, newAddress.Longitude);
            var existingAddressCoordinate = new Coordinate(existingAddress.Latitude, existingAddress.Longitude);
            try
            {
                var distanceUnit =
                    Enum.TryParse<DistanceUnit>(_userMatcherSetting.DistanceRule.DistanceUnit, out var result)
                        ? result
                        : DistanceUnit.Meters;
                var distance = Geolocation.GetDistance(newAddressCoordinate, existingAddressCoordinate,
                    _userMatcherSetting.DistanceRule.DecimalPlaces, distanceUnit);
                return Math.Round(distance, MidpointRounding.ToZero) <= _userMatcherSetting.DistanceRule.DistanceLimit;
            }
            catch (ArgumentException)
            {
                return true;
            }
        }

        public bool HasReferralCodeMatched(string newReferralCode, string existingReferralCode)
        {
            if (_userMatcherSetting.ReferralCodeRule.IgnoreRule) return false;

            if (!newReferralCode.Length.Equals(existingReferralCode.Length))
            {
                return false;
            }

            if (newReferralCode.Equals(existingReferralCode))
            {
                return true;
            }

            var range = _userMatcherSetting.ReferralCodeRule.CharactersNumber - 1;
            if (range >= existingReferralCode.Length)
            {
                range = existingReferralCode.Length - 1;
            }

            var startIndex = 0;
            while (startIndex + range < existingReferralCode.Length)
            {
                var i = startIndex;
                var j = startIndex + range;
                var reversed = existingReferralCode.ToCharArray();
                while (i < j)
                {
                    (reversed[i], reversed[j]) = (reversed[j], reversed[i]);
                    i++;
                    j--;
                }

                if (newReferralCode.Equals(new string(reversed)))
                {
                    return true;
                }

                startIndex++;
            }

            return false;
        }
    }
}