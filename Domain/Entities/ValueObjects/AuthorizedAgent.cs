using System.Text.RegularExpressions;
using Domain.Exceptions;

namespace Domain.Entities.ValueObjects;

public class AuthorizedAgent : ValueObject
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public Identity Identity { get; set; }

    public AuthorizedAgent(string name, string surname, string email, Identity identity)
    {
        ValidateEmail(email);
        Name = name;
        Surname = surname;
        Email = email;
        Identity = identity;
    }

    private static void ValidateEmail(string email)
    {
        const string patternEmail = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        bool isEmail = Regex.IsMatch(email, patternEmail);
        if (!isEmail)
        {
            throw new InvalidEmailException(Messages.InvalidEmailException);
        }
    }
}