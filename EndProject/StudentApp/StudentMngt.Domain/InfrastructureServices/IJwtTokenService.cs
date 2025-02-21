using System.Security.Claims;

namespace StudentMngt.Domain.InfrastructureServices;

public interface IJwtTokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);

    public (string accessToken, string refreshToken) GenerateTokens(IEnumerable<Claim> claims);

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);

}