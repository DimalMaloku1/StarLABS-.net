using Domain.Models;

namespace Application.Services.TokenService
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
