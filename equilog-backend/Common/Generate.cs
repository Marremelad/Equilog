namespace equilog_backend.Common;

// Static utility class for generating various types of tokens and identifiers.
public static class Generate
{
    // Generates a URL-safe password reset token using a GUID.
    public static string PasswordResetToken()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray())
            .Replace("/", "_")    // Replace URL-unsafe forward slash with underscore.
            .Replace("+", "-")    // Replace URL-unsafe plus sign with hyphen.
            .Replace("=", "");    // Remove Base64 padding characters for a cleaner token.
    }
}