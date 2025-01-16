using ExpensoFinanceTracker.DataModel.Models;

namespace ExpensoFinanceTracker.DataAccess.Services.Interface
{
    public interface IUserTxnService
    {
        /// Deletes a user by their username.
        Task<bool> DeleteTransaction(Guid txnid);

        /// Retrieves all users Transaction from the system.
        Task<List<Transaction>> GetAllTransaction();

        /// Searches for users Transaction based on their name.
        Task<List<Transaction>> SearchTransaction(string searchName);

        Task AddTxn(Transaction transaction);

        Task<bool> UpdateTxn(Transaction transaction); // New method for editing

        Task<Transaction> GetTxnById(Guid txnId); // For retrieving a specific transaction
    }
}
