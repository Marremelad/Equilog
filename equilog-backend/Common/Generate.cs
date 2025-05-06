namespace equilog_backend.Common;

public static class Generate
{
    public static string PasswordResetToken()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray())
            .Replace("/", "_")
            .Replace("+", "-")
            .Replace("=", "");
    }
}