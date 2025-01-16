using System.Text.Json;

namespace ExpensoFinanceTracker.DataModel.Models
{
    public abstract class TransactionBase
    {
        protected static readonly string TxnFilePath = Path.Combine(@"D:\App Dev\ExpensoFinanceTracker", "Transaction.json");


        /// A list of users. If the file does not exist or is empty, returns an empty list.
        protected List<Transaction> LoadTransaction()
        {
            if (!File.Exists(TxnFilePath)) return new List<Transaction>();
            var json = File.ReadAllText(TxnFilePath);
            return JsonSerializer.Deserialize<List<Transaction>>(json) ?? new List<Transaction>();
        }

        /// Saves the list of users to a JSON file.
        protected void SaveTransactions(List<Transaction> transactions)
        {
            var json = JsonSerializer.Serialize(transactions);
            File.WriteAllText(TxnFilePath, json);
        }
    }
}
