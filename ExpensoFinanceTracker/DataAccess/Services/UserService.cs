using ExpensoFinanceTracker.DataAccess.Services.Interface;
using ExpensoFinanceTracker.DataModel.Models;
using ExpensoFinanceTracker.DataModel.Models.Seeding;

namespace ExpensoFinanceTracker.DataAccess.Services
{
    public class UserService : UserBase, IUserService
    {
        private List<Users> _users;

        public UserService() 
        {
            _users = LoadUsers();

            if (!_users.Any())
            {
                _users.Add(new Users { Username = Seeding.SeedUsername, Password = Seeding.SeedPassword });
                SaveUsers(_users);
            }
        }

        // Simulated authentication login logic
        public async Task<bool> LoginPage(Users user)
        {
            if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
            {
                return false;
            }
            return _users.Any(u => u.Username == user.Username && u.Password == u.Password);
        }


        

        
    }
}
