namespace server.src.Config;

public static class EnvValidator
{
    public static string GetRequired(string key)
    {
        var value = Environment.GetEnvironmentVariable(key);
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException($"Environment variable '{key}' is required and cannot be empty.");
        }
        return value;
    }

    public static int GetRequiredInt(string key)
    {
        var value = GetRequired(key);
        if (!int.TryParse(value, out var result))
        {
            throw new InvalidOperationException($"Environment variable '{key}' must be a valid integer.");
        }
        return result;
    }

    public static void ValidateAll()
    {
        GetRequired("DB_HOST");
        GetRequiredInt("DB_PORT");
        GetRequired("DB_DATABASE");
        GetRequired("DB_USERNAME");
        GetRequired("DB_PASSWORD");
        GetRequired("JWT_SECRET");
        GetRequired("JWT_ISSUER");
        GetRequired("JWT_AUDIENCE");
        GetRequiredInt("ACCESS_TOKEN_EXPIRATION_MINUTES");
        GetRequiredInt("REFRESH_TOKEN_EXPIRATION_DAYS");
    }
}
