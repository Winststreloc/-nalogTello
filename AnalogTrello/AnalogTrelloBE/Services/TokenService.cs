using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using AnalogTrello.Models;
using AnalogTrelloBE.Helpers;
using AnalogTrelloBE.Interfaces.IService;
using AnalogTrelloBE.Models;
using Microsoft.IdentityModel.Tokens;

namespace AnalogTrelloBE.Services;

public class TokenService : ITokenService
{
    private const int AccessTokenExpiresMinutes = 2;
    private const int RefreshTokenExpiresDays = 1;

    public Token GenerateTokens(User candidateForTokens)
    {
        var claims = GetUserClaims(candidateForTokens);
        var accessToken = GenerateAccessToken(claims);
        var refreshToken = GenerateRefreshToken(claims);

        var token = new Token()
        {
            UserId = candidateForTokens.Id,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Username = candidateForTokens.UserName
        };

        return token;
    }

    private List<Claim> GetUserClaims(User candidateForTokens)
    {
        var claims = new List<Claim>
        {
            new Claim("Id", candidateForTokens.Id.ToString()),
            new Claim("Username", candidateForTokens.UserName),
        };
        return claims;
    }


    public string GenerateAccessToken(List<Claim> userClaims)
    {
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: userClaims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(AccessTokenExpiresMinutes)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public string GenerateRefreshToken(List<Claim> userClaims)
    {
        var id = userClaims.Where(claim => claim.Type == "Id");
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: userClaims.Where(claim => claim.Type == "Id"),
            expires: DateTime.UtcNow.Add(TimeSpan.FromDays(RefreshTokenExpiresDays)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public bool ValidateRefreshToken(string refreshToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters()
        {
            ValidateLifetime = false,
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidIssuer = AuthOptions.ISSUER,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            ValidAudience = AuthOptions.AUDIENCE,
        };

        SecurityToken validatedToken;

        try
        {
            IPrincipal principal = tokenHandler.ValidateToken(refreshToken, validationParameters, out validatedToken);
        }
        catch (SecurityTokenSignatureKeyNotFoundException)
        {
            return false;
        }

        return true;
    }
}