
namespace PasswordManager.Server.Utilities
{
    public class CacheService
    {

        public static string GetUserSessionCacheKey(Guid cookieId) => $"us_{cookieId.ToString()}";

        public static string GetAdfsCacheKey(Guid cookieId) => $"adfstoken_{cookieId.ToString()}";

        public static string GetPasswordCacheKey(Guid cookieId) => $"pw_${cookieId.ToString()}";
    }
}
