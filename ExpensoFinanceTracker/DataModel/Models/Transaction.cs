using System.ComponentModel.DataAnnotations;

namespace ExpensoFinanceTracker.DataModel.Models
{
    public class Transaction
    {
        public Guid TxnId { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Please provide the task name.")]
        public string TxnName { get; set; }

        public string TxnDescription { get; set; }

        public string TxnType { get; set; }

        public double TxnAmount { get; set; }

        public string TxnTags { get; set; }

        public DateTime TxnDate { get; set; }

        public DateTime? DueDate { get; set; }
    }
}
