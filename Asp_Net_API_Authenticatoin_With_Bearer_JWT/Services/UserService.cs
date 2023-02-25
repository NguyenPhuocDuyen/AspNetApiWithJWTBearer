using Asp_Net_API_Authenticatoin_With_Bearer_JWT.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Asp_Net_API_Authenticatoin_With_Bearer_JWT.Services
{
    public class UserService : IUserService
    {
        private List<User> _users = new List<User>
        {
            new User { Id = 1, Username = "user1", FirstName = "John", LastName = "Doe", Password = "password1" },
            new User { Id = 2, Username = "user2", FirstName = "Jane", LastName = "Doe", Password = "password2" }
        };

        public User Authenticate(string username, string password)
        {
            var user = _users.SingleOrDefault(x => x.Username == username && x.Password == password);

            // return null if user not found or password is incorrect
            if (user == null)
                return null;

            // authentication successful, return user information
            return user;
        }

        public IEnumerable<User> GetUsers()
        {
            return _users;
        }
    }
}
