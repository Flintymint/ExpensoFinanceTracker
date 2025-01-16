using ExpensoFinanceTracker.DataAccess.Services.Interface;
using ExpensoFinanceTracker.DataModel.Models;

namespace ExpensoFinanceTracker.DataAccess.Services
{
    public class UserTxnService : TransactionBase, IUserTxnService
    {
        private List<Transaction> _transactions;

        public UserTxnService()
        {
            _transactions = LoadTransaction();

            // Add default admin user if the file is empt
        }
        
        
        // Simulated New transaction logic
        public async Task AddTxn(Transaction transaction)
        {
            _transactions.Add(new Transaction { TxnName = transaction.TxnName, TxnAmount = transaction.TxnAmount,
                TxnDescription = transaction.TxnDescription, TxnType = transaction.TxnType, TxnTags = transaction.TxnTags, 
                TxnDate = transaction.TxnDate, DueDate = transaction.DueDate });
            SaveTransactions(_transactions);
        }

        public async Task<bool> UpdateTxn(Transaction upttransaction)
        {
            try
            {
                var existingTxn = _transactions.FirstOrDefault(t => t.TxnId == upttransaction.TxnId);
                if (existingTxn != null)
                {
                    existingTxn.TxnName = upttransaction.TxnName;
                    existingTxn.TxnAmount = upttransaction.TxnAmount;
                    existingTxn.TxnType = upttransaction.TxnType;
                    existingTxn.TxnTags = upttransaction.TxnTags;
                    existingTxn.DueDate = upttransaction.DueDate;
                    SaveTransactions(_transactions);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;   
            }
            
        }

        public async Task<Transaction> GetTxnById(Guid txnId)
        {
            var transaction = _transactions.FirstOrDefault(t => t.TxnId == txnId);
            return await Task.FromResult(transaction);
        }

        public async Task<bool> DeleteTransaction(Guid txnid)
        {
            var transaction = _transactions.FirstOrDefault(t => t.TxnId == txnid);
            if (transaction == null)
            {
                return false;
            }
                
            _transactions.Remove(transaction);
            SaveTransactions(_transactions);
            return true;
        }

        // Simulated retrieval logic
        public async Task<List<Transaction>> GetAllTransaction()
        {
           return _transactions;
        }

        // Simulated Search txn logic
        public async Task<List<Transaction>> SearchTransaction(string searchName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchName))
                    throw new ArgumentNullException("Name Cannot be Null or Empty", nameof(searchName));

                return await Task.FromResult(
                    _transactions.Where(t => t.TxnName.Contains(searchName, StringComparison.OrdinalIgnoreCase)).ToList());

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while searching for the user.", ex);
            }
        }
    }
}
