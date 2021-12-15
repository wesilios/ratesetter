namespace RateSetter.Sources
{
    public interface IUserMatcher
    {
        bool IsMatch(User newUser, User existingUser);
    }
}