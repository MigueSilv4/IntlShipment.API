using IntlShipment.Helpers;
using IntlShipment.Models;

namespace IntlShipment.Services
{
    public interface IAuthService
    {
        Task<Response<string>> Login(string email, string password);
        Task<Response<User>> Register(User user);
    }
}
