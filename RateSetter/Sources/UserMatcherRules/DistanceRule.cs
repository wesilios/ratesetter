namespace RateSetter.Sources.UserMatcherRules
{
    public class DistanceRule
    {
        public bool IgnoreRule { get; set; }
        public string DistanceUnit { get; set; }
        public int DecimalPlaces { get; set; }
        public double DistanceLimit { get; set; }
    }
}