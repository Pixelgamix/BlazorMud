namespace BlazorMud.Contracts.Security
{
    public interface IPasswordHasher
    {
        string CreateHashedPassword(string password);
        bool IsSamePassword(string password, string hashedPassword);
    }
}
