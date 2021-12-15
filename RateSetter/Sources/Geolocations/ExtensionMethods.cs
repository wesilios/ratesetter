using System;

namespace RateSetter.Sources.Geolocations
{
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
}