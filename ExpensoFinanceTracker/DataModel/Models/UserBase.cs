using System.Text.Json;
namespace ExpensoFinanceTracker.DataModel.Models
{
    public abstract class UserBase
    {
        protected static readonly string FilePath = Path.Combine(@"D:\App Dev\ExpensoFinanceTracker", "users.json");

        /// A list of users. If the file does not exist or is empty, returns an empty list.
        protected List<Users> LoadUsers()
        {
            if (!File.Exists(FilePath)) return new List<Users>();
            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Users>>(json) ?? new List<Users>();
        }

        /// Saves the list of users to a JSON file.
        protected void SaveUsers(List<Users> users)
        {
            var json = JsonSerializer.Serialize(users);
            File.WriteAllText(FilePath, json);
        }

    }
}
