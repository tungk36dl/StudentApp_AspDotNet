//using StudentMngt.Domain.InfrastructureServices;
//using StudentMngt.Infrastructure.DependencyInjection;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;


//namespace StudentMngt.Infrastructure;

//public class JwtTokenService : IJwtTokenService
//{

//    private readonly IOptions<JwtOption> _jwtOption;
//    private readonly IConfiguration _configuration;

//    public JwtTokenService(IConfiguration configuration, IOptions<JwtOption> jwtOption)
//    {
//        _jwtOption = jwtOption;
//        _configuration = configuration;
//    }
//    public string GenerateAccessToken(IEnumerable<Claim> claims)
//    {
//        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.Value.SecretKey));
//        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

//        var tokeOptions = new JwtSecurityToken(
//            //issuer: _configuration["JwtOption:Issuer"],
//            issuer: _jwtOption.Value.Issuer,
//            audience: _jwtOption.Value.Audience,
//            claims: claims,
//            expires: DateTime.Now.AddMinutes(_jwtOption.Value.ExpireMin),
//            signingCredentials: signinCredentials
//        );

//        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
//        return tokenString;
//    }


//}

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StudentMngt.Domain.InfrastructureServices;
using StudentMngt.Infrastructure.DependencyInjection;

namespace StudentMngt.Infrastructure;

public class JwtTokenService : IJwtTokenService
{
    private readonly IOptions<JwtOption> _jwtOption;
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration, IOptions<JwtOption> jwtOption)
    {
        _jwtOption = jwtOption;
        _configuration = configuration;
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.Value.SecretKey));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var tokeOptions = new JwtSecurityToken(
            //issuer: _configuration["JwtOption:Issuer"],
            issuer: _jwtOption.Value.Issuer,
            audience: _jwtOption.Value.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwtOption.Value.ExpireMin),
            signingCredentials: signinCredentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        return tokenString;
    }

    // Tạo cả Access Token và Refresh Token
    public (string accessToken, string refreshToken) GenerateTokens(IEnumerable<Claim> claims)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.Value.SecretKey));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var accessToken = new JwtSecurityToken(
            issuer: _jwtOption.Value.Issuer,
            audience: _jwtOption.Value.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtOption.Value.ExpireMin),
            signingCredentials: signinCredentials
        );

        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

        return (new JwtSecurityTokenHandler().WriteToken(accessToken), refreshToken);
    }

    // Lấy thông tin user từ Access Token hết hạn
    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false, // Cho phép kiểm tra ngay cả khi token hết hạn
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtOption.Value.Issuer,
            ValidAudience = _jwtOption.Value.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.Value.SecretKey))
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }

 

}
