using System.Security.Claims;
using AnalogTrelloBE.Models;

namespace AnalogTrelloBE.Interfaces.IService;

public interface ITokenService
{
    Token GenerateTokens(User candidateForTokens);
    string GenerateAccessToken(List<Claim> userClaims);
    string GenerateRefreshToken(List<Claim> userClaims);
    bool ValidateRefreshToken(string refreshToken);
}