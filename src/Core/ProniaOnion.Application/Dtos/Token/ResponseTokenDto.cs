

namespace ProniaOnion.Application.Dtos.Token
{
    public record ResponseTokenDto(string AccessToken, DateTime ExpiresAt,string RefreshToken, DateTime RefreshTokenExpiresAt);
    
}
