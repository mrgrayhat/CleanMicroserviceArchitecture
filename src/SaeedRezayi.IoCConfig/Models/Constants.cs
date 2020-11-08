
namespace SaeedRezayi.IoCConfig.Models
{
    /// <summary>
    /// Message's Constant's
    /// </summary>
    public static class Constants
    {
        public const string TOKEN_VALIDATED = "Authentication Token Validated.";
        public const string AUTH_FAILED = "Authentication failed.";
        public const string CHALLENGE_ERROR = "OnChallenge error.";
        public const string NO_DATABASE_PROVIDER = "No Database Provider Found!";
        public const string TOKEN_FAILURE_MESSAGE = "RefreshTokenExpirationMinutes is less than AccessTokenExpirationMinutes. Obtaining new tokens using the refresh token should happen only if the access token has expired.";
        public const string SWAGGER_AUTH_MESSAGE = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"";

    }
}
