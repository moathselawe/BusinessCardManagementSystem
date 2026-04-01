using Microsoft.AspNetCore.Identity;


namespace HireMind.Application.Services;

public class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<object> _hasher = new();

    public string Hash(string password)
    {
        return _hasher.HashPassword(null!, password);
    }

    public bool Verify(string password, string hashedPassword)
    {
        var result = _hasher.VerifyHashedPassword(null!, hashedPassword, password);
        return result != PasswordVerificationResult.Failed;
    }
}