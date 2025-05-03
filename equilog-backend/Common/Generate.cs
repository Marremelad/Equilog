namespace equilog_backend.Common;

public static class Generate
{
    public static string PasswordResetCode()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray())
            .Replace("/", "_")
            .Replace("+", "-")
            .Replace("=", "");
    }
}