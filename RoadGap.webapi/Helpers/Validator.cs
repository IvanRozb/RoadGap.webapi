using System.Text.RegularExpressions;

namespace RoadGap.webapi.Helpers;

public static partial class Validator
{
    public static bool ValidateUserName(string userName)
    {
        return userName.Length <= 50 && MyRegex().IsMatch(userName);
    }

    public static bool ValidateEmail(string email)
    {
        return email.Length <= 320 && MyRegex1().IsMatch(email);
    }

    // Regex pattern to match only alphabets and numbers
    [GeneratedRegex("^[a-zA-Z0-9]*$")]
    private static partial Regex MyRegex();

    // Regex pattern to match email format
    [GeneratedRegex("^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$")]
    private static partial Regex MyRegex1();
}