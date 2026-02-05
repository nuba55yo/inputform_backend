using System.Globalization;
using System.Text.RegularExpressions;

public static class InputformValidate
{
    private static readonly Regex EmailRegex =
        new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    public static bool TryValidate(
        InputformRequest req,
        out DateOnly birthDay,
        out string error)
    {
        birthDay = default;
        error = "";

        if (req is null)
        {
            error = "Request is null.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(req.first_name) ||
            string.IsNullOrWhiteSpace(req.last_name) ||
            string.IsNullOrWhiteSpace(req.email))
        {
            error = "Missing required fields.";
            return false;
        }

        if (!EmailRegex.IsMatch(req.email))
        {
            error = "Invalid email.";
            return false;
        }

        if (!DateOnly.TryParseExact(
                req.birth_day,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out birthDay))
        {
            error = "Invalid birth_day format.";
            return false;
        }

        if (req.profile is null)
        {
            error = "Profile file is required.";
            return false;
        }

        if (req.profile.Length == 0)
        {
            error = "Profile is empty.";
            return false;
        }

        if (req.profile.Length > 1_000_000)
        {
            error = "File too large.";
            return false;
        }

        return true;
    }
}
