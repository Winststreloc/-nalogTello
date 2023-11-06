using System.Security.Claims;
using AnalogTrello.Models;

namespace AnalogTrelloBE.Intefaces.IService;

public interface ITokenService
{
    Token GenerateTokens(User candidateForTokens);
    string GenerateAccessToken(IEnumerable<Claim> userClaims);
    string GenerateRefreshToken(IEnumerable<Claim> userClaims);
    bool ValidateRefreshToken(string refreshToken);
}