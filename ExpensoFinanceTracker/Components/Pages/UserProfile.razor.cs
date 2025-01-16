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

        private string ImportMessage = string.Empty;
        private IBrowserFile? UploadedFile;

        private async Task OnFileSelected(InputFileChangeEventArgs e)
        {
            UploadedFile = e.File;
            ImportMessage = $"Selected file: {UploadedFile.Name}";
        }

        private async Task HandleImport()
        {
            if (UploadedFile == null)
            {
                ImportMessage = "Please select a file to import.";
                return;
            }

            try
            {
                using var stream = UploadedFile.OpenReadStream();
                using var reader = new StreamReader(stream);
                var content = await reader.ReadToEndAsync();

                // Process CSV content here
                ImportMessage = "File imported successfully!";
            }
            catch (Exception ex)
            {
                ImportMessage = $"Error importing file: {ex.Message}";
            }
        }
        #region Navigation to UserProfile
        private void HomePage()
        {
            Nav.NavigateTo("/home");
        }
        #endregion
    }

}

