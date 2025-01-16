using ExpensoFinanceTracker.DataModel.Models;
namespace ExpensoFinanceTracker.Components.Pages
{
    public partial class LoginPage
    {
        private string? ErrorMessage;

        public Users Users { get; set; } = new();

        #region Login
        private async void HandleLogin()
        {
            if (await UserService.LoginPage(Users))
            {
                Nav.NavigateTo("/home");
            }
            else
            {
                ErrorMessage = "Invalid username or password.";
            }
        }
        #endregion

    }
}