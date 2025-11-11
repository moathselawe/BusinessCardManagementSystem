namespace BCMS.Domain.Entities;

public class BusinessCard: BaseAuditableEntity
{
    public string ArabicName { get; private set; } = null!;
    public string EnglishName { get; private set; } = null!;
    public DateTime DateOfBirth { get; private set; }
    public string Email { get; private set; } = null!;
    public string Phone { get; private set; } = null!;
    public string? Logo { get; private set; }
    public string Address { get; private set; } = null!;

    public static BusinessCard Create(string arabicName, string englishName, DateTime dateOfBirth, string email, string phone, string logo, string address)
    {
        return new BusinessCard()
        {
            ArabicName = arabicName,
            EnglishName = englishName,
            DateOfBirth = dateOfBirth,
            Email = email,
            Phone = phone,
            Logo = logo,
            Address = address,
            CreatedDate = DateTime.Now

        };
    }

    public static BusinessCard Update(Guid id, string arabicName, string englishName, DateTime dateOfBirth, string email, string phone, string logo, string address)
    {
        return new BusinessCard()
        {
            Id = id,
            ArabicName = arabicName,
            EnglishName = englishName,
            DateOfBirth = dateOfBirth,
            Email = email,
            Phone = phone,
            Logo = logo,
            Address = address,
            LastModifiedDate = DateTime.Now
        };
    }
}

