
using Isogeo.Models.API;

namespace Isogeo.Models
{
    public class ApiBearerToken
    {
        public string AccessToken { get; }

        public DateTimeOffset ExpirationDate { get; }

        public ApiBearerToken(Token token)
        {
            AccessToken = token.AccessToken;
            ExpirationDate = DateTimeOffset.UtcNow.AddSeconds(token.ExpiresIn - 20); // -20 secs : security because we don't have a precise expiration date given inside the token received
        }
    }
}
