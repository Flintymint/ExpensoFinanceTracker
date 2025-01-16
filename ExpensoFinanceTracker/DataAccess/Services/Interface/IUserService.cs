using ExpensoFinanceTracker.DataModel.Models;

namespace ExpensoFinanceTracker.DataAccess.Services.Interface
{
    public interface IUserService
    {
        /// Logs in a user using their credentials.
        Task<bool> LoginPage(Users user);
        
    }
}
