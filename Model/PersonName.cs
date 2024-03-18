using EntityFrameworkCore.Projectables;

namespace slp.light.Model;

public record PersonName
{
    /// <summary>
    /// CZ: Titul před jménem.
    /// </summary>
    public required string TitleBefore { get; init; }

    /// <summary>
    /// CZ: Jméno.
    /// </summary>
    public required string FirstName { get; init; }

    /// <summary>
    /// CZ: Příjmení.
    /// </summary>
    public required string LastName { get; init; }

    /// <summary>
    /// CZ: Titul za jménem.
    /// </summary>
    public required string TitleAfter { get; init; }

    /// <summary>
    /// CZ: Celé jméno.
    /// </summary>
    [Projectable]
    public string FullName => string.IsNullOrWhiteSpace(TitleAfter)
        ? $"{TitleBefore} {FirstName} {LastName}".Trim()
        : $"{TitleBefore} {FirstName} {LastName}, {TitleAfter}".Trim();

    public static PersonName Parse(string fullName)
    {
        var fullNameParts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return fullNameParts switch
        {
            [var titleBefore, var firstName, var lastName, var titleAfter] => new PersonName
            {
                TitleBefore = titleBefore.Trim(),
                FirstName = firstName.Trim(),
                LastName = lastName.Trim(' ', ','),
                TitleAfter = titleAfter.Trim()
            },

            [var titleBefore, var firstName, var lastName] => new PersonName
            {
                TitleBefore = titleBefore.Trim(),
                FirstName = firstName.Trim(),
                LastName = lastName.Trim(),
                TitleAfter = ""
            },

            [var firstName, var lastName] => new PersonName
            {
                TitleBefore = "",
                FirstName = firstName.Trim(),
                LastName = lastName.Trim(),
                TitleAfter = ""
            },

            _ => new PersonName
            {
                TitleBefore = "",
                FirstName = "",
                LastName = fullName.Trim(),
                TitleAfter = ""
            },
        };
    }
}