using Asp_Net_API_Authenticatoin_With_Bearer_JWT.Models;
using System.Collections.Generic;

namespace Asp_Net_API_Authenticatoin_With_Bearer_JWT.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetUsers();
    }
}
