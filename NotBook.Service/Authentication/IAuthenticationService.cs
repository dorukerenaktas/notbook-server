using System.Security.Claims;

namespace NotBook.Service.Authentication
{
    public interface IAuthenticationService
    {
        string GenerateAccessToken(int userId, int universityId, out string refreshToken);

        string RefreshAccessToken(string accessToken, string refreshToken, out string newRefreshToken);

        int GetAuthenticatedUserId(ClaimsPrincipal user);
    }
}