using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace BuildinBlocks.Auth;

public class AuthOptions
{
    public const string ISSUER = "AnalogTrello";
    public const string AUDIENCE = "AnalogTrello-RESP-API";
    private const string KEY = "mysupersecret_secretkey!123";
    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }
}