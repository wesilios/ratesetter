# RateSetter

## Contents

- [Requirement](#requirement)
- [Scenario](#scenario)
- [Documents](#documents)
    - [Project tree](#project-tree)
    - [Models](#models)
    - [Interface & Service](#interface--service)
    - [Extension & Library](#extension--library)
        - [String Extension](#string-extensions)
        - [Geolocation Library](#geolocation-library)

## Requirement

- .NET Core 3.1 or above
- .NET Standard 2.0

## Scenario

RateSetter needs to compare new member registrations against our existing members, using a
number of rules to reject new registrations. Currently we use the following (hypothetical!) rules:
1. Distance
   If the new user lives within 500 metres of an existing user their registration will be rejected.
2. Address
   If the new member’s name and address matches an existing member they will be rejected.
   Our users may include unusual characters in their address, so we need to allow for this when
   matching. For example, the address “Level 3,! 51_Pitt Street, Sydney NSW-2000” would be a
   match for “Level 3, 51 Pitt Street, Sydney, NSW 2000”. We store our users’ names in title
   case.
3. Referral Code
   A user can enter a referral code when registering. We need to ensure that no other user has
   entered the same code.
   Unfortunately, one of our affiliate partners has a buggy referral code generation method.
   Their code always results in 3 characters of the referral code being reversed, which we need
   to account for - for example, “ABC123” would match to (among others) both “ABC321” and
   “AB21C3”.

## Documents

Implementing logic setting so that the rule can be turn on/off or change rule

IsMatch logic:

```
New User is matched with Existing User when any of these rule is true
    NameAndAddressRule: If name and address matched return true => return User is match
    If NameAndAddressRule is false
        DistanceRule: Distance <= 500 meters return true => return User is match
            If DistanceRule is false
                ReferralCodeRule: Referral Code is matched return true => return User is match
    return false;
```

NameAndAddressRule:
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

DistanceRule:
```
In Distance Range
    If RuleSetting is ignore =>  return false
        If each of 2 coordinates from address is invald =>  return true
            If Distance between 2 Addresses gets from Geolocation result 
            is less than or equal to Distance Limit(From RuleSetting) =>  return true
return fasle
```

ReferralCodeRule:
```
Referral Code is match
    If RuleSetting is ignore =>  return false
        If number of characters (From RuleSetting) in existing Code is reversed
        match new Code => return true
return false
```

### Project tree

    .
    ├── RateSetter
    │   ├── Sources                             # Project Sources files
    │   │   ├── Extensions                      # Extensions
    │   │   │   ├── StringExtensions.cs         # String Extensions
    │   │   ├── Geolocations                    # Geolocation to get distance between two coordinates
    │   │   |   ├── Coordinates.cs              # Coodinate class
    │   │   |   ├── DistanceUnit.cs             # Distance Unit enum: Meters, Kilometers, Miles
    │   │   |   ├── ExtensionsMethods.cs        # Helper class to calculate Coordinate latitude and longitude to radian
    │   │   |   ├── Geolocation.cs              # Geolocation class to get distance between two coordinates
    │   │   ├── Settings                        # Extend setting for UserMatcher configuration
    │   │   │   ├── DistanceRule.cs             # Distance rule
    │   │   │   ├── NameAndAddressRule.cs       # Name And Address Rule
    │   │   │   ├── ReferralCodeRule.cs         # Referral Code Rule
    │   │   │   ├── UserMatcherSetting.cs       # User Matcher Setting
    │   │   ├── Address.cs                      
    │   │   ├── IUserMatcher.cs
    │   │   ├── User.cs
    │   │   ├── UserMatcher.cs
    │   ├── Tests                               # Unit Tests
    │   │   ├── GeolocationTests.cs             # Geolocation Tests
    │   │   ├── StringExtensionsTests.cs        # String Extention Tests
    │   │   ├── TestCases.cs                    # All Test Cases for Geolocation and User Matcher Test
    │   │   ├── UserMatcherTests.cs             # User Matcher Tests
    │   ├── RateSetter.csproj                   # .NET core project file
    ├── global.json                             # File allows you to define which .NET SDK version is used when you run .NET CLI commands.
    ├── RateSetter.sln                          # Solution file
    └── README.md                               # README

### Models

```c#
    public class Address
    {
        public string StreetAddress { get; set; }
        public string Suburb { get; set; }
        public string State { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
```

```c#
    public class User
    {
        public Address Address { get; set; }
        public string Name { get; set; }
        public string ReferralCode { get; set; }
    }
```

### Interface & Implement Class

Interface ```IUserMatcher```: define contract for class ```UserMatcher``` where implements logic for scenario

```c#
    public interface IUserMatcher
    {
        bool IsMatch(User newUser, User existingUser);
    }
```

class ```UserMatcher``` implemented ```IUserMatcher``` has 4 ```public``` functions:
    
1. ```IsMatch```: function must be implemented from ```IUserMatcher```, the result of this function is combination of 3 below.
2. ```HasNameAddressMatched```: check if new User's name and User's address are matched with existing User's
3. ```IsInDistance```: check if Coordinate of new User's Address is matched with existing User's.
4. ```HasReferralCodeMatched```: check if new User's Referral code is matched with existing User's.

```c#
    bool IsMatch(User newUser, User existingUser);
    bool HasNameAddressMatched(User newUser, User existingUser);
    bool IsInDistance(Address newAddress, Address existingAddress);
    bool HasReferralCodeMatched(string newReferralCode, string existingReferralCode);
```

### Extension & Library

#### String Extensions

Including 3 static function to modify ```string```:

1. ```ToTitleCase```: convert origin string to title case
2. ```TrimDuplicateSpaces```: remove all duplicate white spaces in string
3. ```TrimSpecialCharacters```: remove all characters that's not alphabet or numeric

#### Geolocation Library

##### Using Haversine formula:

```
a = sin²(Δφ/2) + cos φ1 ⋅ cos φ2 ⋅ sin²(Δλ/2)
c = 2 ⋅ atan2( √a, √(1−a) )
d = R ⋅ c
where φ is latitude, λ is longitude, R is earth’s radius (mean radius = 6,371km);
and that angles need to be in radians to pass to trig functions!
```

##### Class Coordinate

```c#
public class Coordinate
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
```

Validate its coordinates

```
-90 degree <= Latitude <= 90 degree
-180 degree <= Longitude <= 180 degree
```

```c#
public bool ValidateCoordinates()
{
    if (Latitude < -90 || Latitude > 90) return false;
    if (Longitude < -180 || Longitude > 180) return false;

    return true;
}
```

##### Extenstion Method

Helper class to convert Latitude and Longitude to Radian and Calculate Difference between 2 values

```c#
public static class ExtensionMethods
{
    public static double ToRadian(this double d)
    {
        return d * (Math.PI / 180);
    }

    public static double DiffRadian(this double val1, double val2)
    {
        return val2.ToRadian() - val1.ToRadian();
    }
}
```

##### Enum DistanceUnit

Enum class's used in Geolocation so it can return distance value in miles, kilometers or meteres

```c#
public enum DistanceUnit
{
    Miles,
    Kilometers,
    Meters,
}
```

##### Geolocation

Geolocation implemented Haversine formula to return Distance. Geolocation is static class.

Receive 2 Coordinates, decimalPlaces (default 6), distanceUnit(default Meters)

```c#
   double GetDistance(Coordinate originCoordinate, Coordinate destinationCoordinate,
            int decimalPlaces = 6, DistanceUnit distanceUnit = DistanceUnit.Meters);
```

