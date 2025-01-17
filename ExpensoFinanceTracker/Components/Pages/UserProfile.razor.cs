using ExpensoFinanceTracker.DataModel.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace ExpensoFinanceTracker.Components.Pages
{
    public partial class UserProfile
    {   
        private DateTime? StartDate { get; set; }
        private DateTime? EndDate { get; set; }
        private List<Transaction> Transactions = new();
        private List<Transaction> Filtered = new();
        private string Message = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                // Load all transactions but do not display them yet
                Transactions = await UserTxnService.GetAllTransaction();
            }
            catch (Exception ex)
            {
                Message = $"Error loading transactions: {ex.Message}";
            }
        }

        #region Filter Transactions to date range
        private async Task FilteredTxn()
        {
            try
            {
                if (!StartDate.HasValue || !EndDate.HasValue)
                {
                    Filtered = new List<Transaction>();
                    Message = "Please select a valid date range.";
                    return;
                }

                // Ensure EndDate includes the entire day
                var adjustedEndDate = EndDate.Value.Date.AddDays(1).AddTicks(-1);

                Filtered = Transactions
                    .Where(t => t.TxnDate >= StartDate.Value && t.TxnDate <= adjustedEndDate)
                    .ToList();

                Message = Filtered.Any() ? string.Empty : "No transactions found for the selected date range.";
            }
            catch (Exception ex)
            {
                Message = $"Error filtering transactions: {ex.Message}";
            }

            StateHasChanged();
        }
        #endregion

        #region Navigation to UserProfile
        private void HomePage()
        {
            Nav.NavigateTo("/home");
        }
        #endregion
    }

}

