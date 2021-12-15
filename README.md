# RateSetter

## Contents

- [Requirement](#requirement)
- [Scenario](#scenario)
- [Project tree](#project-tree)
- [Document](#documents)

## Requirement

- .NET Core 3.1 or above
- .NET Standard 2.0

## Scenario

RateSetter needs to compare new member registrations against our existing members, using a number of rules to reject new
registrations. Currently we use the following (hypothetical!) rules:

1. Distance If the new user lives within 500 metres of an existing user their registration will be rejected.
2. Address If the new member’s name and address matches an existing member they will be rejected. Our users may include
   unusual characters in their address, so we need to allow for this when matching. For example, the address “Level 3,!
   51_Pitt Street, Sydney NSW-2000” would be a match for “Level 3, 51 Pitt Street, Sydney, NSW 2000”. We store our
   users’ names in title case.
3. Referral Code A user can enter a referral code when registering. We need to ensure that no other user has entered the
   same code. Unfortunately, one of our affiliate partners has a buggy referral code generation method. Their code
   always results in 3 characters of the referral code being reversed, which we need to account for - for example,
   “ABC123” would match to (among others) both “ABC321” and “AB21C3”.

### Project tree

    .
    ├── RateSetter
    │   ├── Sources                          # Project Sources files
    │   │   ├── Extensions                   # Extensions
    │   │   ├── Geolocations                 # Geolocation to get distance between two coordinates
    │   │   ├── UserMatcherRules             # All Matching Rules: interfaces, implementing and rule classes
    │   │   ├── Address.cs
    │   │   ├── IUserMatcher.cs
    │   │   ├── User.cs
    │   │   ├── UserMatcher.cs
    │   ├── Tests                            # Unit Tests
    │   │   ├── DistanceMatcherTests.cs      # DistanceMatcher Tests
    │   │   ├── GeolocationTests.cs          # Geolocation Tests
    │   │   ├── NameAndAddressMatcher.cs     # NameAndAddressMatcher Tests
    │   │   ├── ReferralCodeMatcherTests.cs  # ReferralCodeMatcher Tests
    │   │   ├── StringExtensionsTests.cs     # String Extention Tests
    │   │   ├── UserMatcherTests.cs          # User Matcher Tests
    │   ├── RateSetter.csproj                # .NET core project file
    ├── global.json                          # File allows you to define which .NET SDK version is used.
    ├── RateSetter.sln                       # Solution file
    └── README.md                            # README.md

## Documents

Implementing logic setting so that the rule can be turn on/off or change rule

### IsMatch logic:

```
New User is matched with Existing User when any of these rule is true
    NameAndAddressRule: If name and address matched return true => return User is match
    If NameAndAddressRule is false
        DistanceRule: Distance <= 500 meters return true => return User is match
            If DistanceRule is false
                ReferralCodeRule: Referral Code is matched return true => return User is match
return false;
```

#### NameAndAddressRule:

```
Name and Address is match
    If RuleSetting is ignore => return false
        If Name is not match => return false
            If Name Match is true
                If New Address and Old Address 
                after removing special characters and Trim start space and end space 
                are matched =>  return true
return fasle
```

#### DistanceRule:

```
In Distance Range
    If RuleSetting is ignore =>  return false
        If each of 2 coordinates from address is invald =>  return true
            If Distance between 2 Addresses gets from Geolocation result 
            is less than or equal to Distance Limit(From RuleSetting) =>  return true
return fasle
```

#### ReferralCodeRule:

```
Referral Code is match
    If RuleSetting is ignore =>  return false
        If number of characters (From RuleSetting) in existing Code is reversed
        match new Code => return true
return false
```
