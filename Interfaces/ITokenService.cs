using Domain.Entites;

namespace Trade.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
