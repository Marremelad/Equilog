namespace equilog_backend.Common;

public static class Generate
{
    public static string PasswordResetCode()
    {
        return Guid.NewGuid().ToString();
    }
}