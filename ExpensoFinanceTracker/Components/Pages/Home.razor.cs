using ExpensoFinanceTracker.DataModel.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;


namespace ExpensoFinanceTracker.Components.Pages
{
    public partial class Home : ComponentBase
    {
        private List<Transaction> Transactions = new();
        private List<Transaction> Filtered = new();
        private Transaction _txnModel = new();
        private string Message = string.Empty;
        private List<Transaction> TopTransactions = new();
        private bool ShowAddTransaction = false;
        private string _tabFilter = "All";
        private string _sortBy = "txnDate";
        private string _sortDirection = "ascending";

        protected override async Task OnInitializedAsync()
        {
            Transactions = await GetAllTransaction();
            Filtered = Transactions;
            TopTransactions = GetTopTransactions(Transactions);
            CalculateSummary();
            CalculateTransactionInsights();

        }
        private async Task<List<Transaction>> GetAllTransaction()
        {
            return await UserTxnService.GetAllTransaction();
        }
        private double IncomingFunds { get; set; }
        private double TotalExpenses { get; set; }
        private double OutstandingDebt { get; set; }
        private double Total { get; set; }

        private void CalculateSummary()
        {
            IncomingFunds = Filtered
                .Where(txn => txn.TxnType == "Credit" || txn.TxnType == "Debt")
                .Sum(txn => txn.TxnAmount);

            TotalExpenses = Filtered
                .Where(txn => txn.TxnType == "Debit")
                .Sum(txn => txn.TxnAmount);

            OutstandingDebt = Filtered
                .Where(txn => txn.TxnType == "Debt")
                .Sum(txn => txn.TxnAmount);

            Total = IncomingFunds - TotalExpenses;
        }
        private double HighestInflow { get; set; }
        private double LowestInflow { get; set; }
        private double HighestOutflow { get; set; }
        private double LowestOutflow { get; set; }
        private double HighestDebt { get; set; }
        private double LowestDebt { get; set; }

        private void CalculateTransactionInsights()
        {
            HighestInflow = Filtered.Where(t => t.TxnType == "Credit").Max(t => t.TxnAmount);
            LowestInflow = Filtered.Where(t => t.TxnType == "Credit").Min(t => t.TxnAmount);

            HighestOutflow = Filtered.Where(t => t.TxnType == "Debit").Max(t => t.TxnAmount);
            LowestOutflow = Filtered.Where(t => t.TxnType == "Debit").Min(t => t.TxnAmount);

            HighestDebt = Filtered.Where(t => t.TxnType == "Debt").Max(t => t.TxnAmount);
            LowestDebt = Filtered.Where(t => t.TxnType == "Debt").Min(t => t.TxnAmount);
        }

        #region  Search
        private string _search = string.Empty;

        private async Task FilteredTxn()
        {
            try
            {
                if (String.IsNullOrWhiteSpace(Search))
                {
                    Filtered = await UserTxnService.GetAllTransaction();
                    Message = string.Empty;
                    return;
                }
                Filtered = await UserTxnService.SearchTransaction(Search);

                if (!Filtered.Any())
                {
                    Message = "No Match Transaction Found";
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }

        private string Search
        {
            get => _search;

            set
            {
                if (_search == value) return;
                _search = value;
                _ = OnSearchInputAsync(_search);
            }
        }
        private async Task OnSearchInputAsync(string search)
        {
            Search = search;
            await FilteredTxn();
            StateHasChanged();
        }
        #endregion

        

        private IEnumerable<Transaction> FilteredAndSortedTransactions()
        {
            var filtered = Filtered;

            if (_tabFilter != "All")
            {
                filtered = filtered.Where(txn => txn.TxnType == _tabFilter).ToList();
            }

            return _sortBy switch
            {
                "txnDate" => _sortDirection == "ascending" ? filtered.OrderBy(t => t.TxnDate) : filtered.OrderByDescending(t => t.TxnDate),
                _ => filtered
            };
        }

        private void TabFilter(string filter)
        {
            _tabFilter = filter;
            StateHasChanged();
        }

        

        #region SaveTransaction
        private Transaction transactionDto { get; set; } = new Transaction();
        private async Task SaveTransaction()
        {
            // Validation logic
            if (string.IsNullOrWhiteSpace(transactionDto.TxnName) ||
                transactionDto.TxnAmount <= 0 ||
                string.IsNullOrWhiteSpace(transactionDto.TxnType) ||
                string.IsNullOrWhiteSpace(transactionDto.TxnTags) ||
                (transactionDto.TxnType == "Debt" && transactionDto.DueDate == null))
            {
                Message = "Please fill all required fields.";
                return;
            }

            // Save transaction
            var response = UserTxnService.AddTxn(transactionDto);

            if (response != null)
            {
                ShowAddTransaction = false;
                Nav.NavigateTo("/home", forceLoad: true);
            }
            else
            {
                Message = "Transaction not completed!";
            }
        }

        private void ShowAddTransactionPopup()
        {
            transactionDto = new Transaction();
            ShowAddTransaction = true;
        }

        private void HideAddTransactionPopup()
        {
            ShowAddTransaction = false;
        }
        #endregion


        #region top 5 transactions
        private List<Transaction> GetTopTransactions(List<Transaction> transactions)
        {
            return transactions
                .OrderByDescending(t => t.TxnAmount)
                .Take(5)
                .ToList();
        }
        #endregion

        #region EditTransaction

        private bool ShowEditTransaction = false;
        private Transaction editTransactionDto { get; set; } = new Transaction();

        private async Task EditTransaction(Guid txnId)
        {
            // Find the transaction to edit
            var transaction = FilteredAndSortedTransactions().FirstOrDefault(t => t.TxnId == txnId);
            if (transaction != null)
            {
                // Populate the editTransactionDto with the selected transaction's data
                editTransactionDto = new Transaction
                {
                    TxnId = transaction.TxnId,
                    TxnName = transaction.TxnName,
                    TxnAmount = transaction.TxnAmount,
                    TxnType = transaction.TxnType,
                    TxnTags = transaction.TxnTags,
                    TxnDate = transaction.TxnDate,
                    DueDate = transaction.DueDate
                };

                // Show the edit popup
                ShowEditTransaction = true;
            }
        }
        private async Task SaveEditedTransaction()
        {
            // Validation logic for editing
            if (string.IsNullOrWhiteSpace(editTransactionDto.TxnName) ||
                editTransactionDto.TxnAmount <= 0 ||
                string.IsNullOrWhiteSpace(editTransactionDto.TxnType) ||
                string.IsNullOrWhiteSpace(editTransactionDto.TxnTags) ||
                (editTransactionDto.TxnType == "Debt" && editTransactionDto.DueDate == null))
            {
                Message = "Please fill all required fields.";
                return;
            }

            // Update transaction in the data source
            var response = await UserTxnService.UpdateTxn(editTransactionDto);

            if (response)
            {
                // Close the popup and refresh the UI
                ShowEditTransaction = false;
                StateHasChanged();
            }
            else
            {
                Message = "Failed to update transaction.";
            }
        }


        private void ShowEditTransactionPopup()
        {
            ShowEditTransaction = true;
        }
        
        private void HideEditTransactionPopup()
        {
            ShowEditTransaction = false;
        }
        
        #endregion




        #region Logout Popup
        private bool IslogOut = false;
        private void ShowLogoutConfirmation()
        {
            IslogOut = true;
        }

        private void HideLogoutConfirmation()
        {
            IslogOut = false;
        }

        private void Logout()
        {
            Nav.NavigateTo("/loginpage");
        }
        #endregion

        

        #region Delete Confirmantion

        private bool IsDelete = false;
        private void ShowDeleteConfirmation()
        {
            IsDelete = true;
        }

        private void HideDeleteConfirmation()
        {
            IsDelete = false;
        }

        private async Task DeleteTransaction(Guid txnid)
        {
            var response = await UserTxnService.DeleteTransaction(txnid);

            if (response)
            {
                Transactions = await GetAllTransaction();
                
                IsDelete = false; 
            }

        }
        #endregion

        #region Navigation to UserProfile
        private void UserProfile()
        {
            Nav.NavigateTo("/userprofile");
        }
        #endregion
    }

}