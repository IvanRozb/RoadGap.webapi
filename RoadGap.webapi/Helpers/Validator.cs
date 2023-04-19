using System.Text.RegularExpressions;

namespace RoadGap.webapi.Helpers;

public static partial class Validator
{
    public static bool ValidateUserName(string userName)
    {
        var trimmedUserName = userName.Trim();
        return !string.IsNullOrEmpty(userName)
               && userName.Length <= 50
               && MyRegex().IsMatch(trimmedUserName);
    }

    public static bool ValidateEmail(string email)
    {
        var trimmedEmail = email.Trim();
        return !string.IsNullOrEmpty(trimmedEmail)
               && trimmedEmail.Length <= 320
               && MyRegex1().IsMatch(trimmedEmail);
    }

    public static bool ValidateUrl(string url)
    {
        var trimmedUrl = url.Trim();
        return !string.IsNullOrEmpty(trimmedUrl)
               && trimmedUrl.Length <= 255
               && MyRegex2().IsMatch(trimmedUrl);
    }

    // Regex pattern to match only alphabets and numbers
    [GeneratedRegex("^[a-zA-Z0-9]+$")]
    private static partial Regex MyRegex();

    // Regex pattern to match email format
    [GeneratedRegex("^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$")]
    private static partial Regex MyRegex1();

    // Regex pattern to match url format
    [GeneratedRegex(
        "^(http|https):\\/\\/[\\w\\-_]+(\\.[\\w\\-_]+)+([\\w\\-\\.,@?^=%&amp;:/~\\+#]*[\\w\\-\\@?^=%&amp;/~\\+#])?$")]
    private static partial Regex MyRegex2();
}