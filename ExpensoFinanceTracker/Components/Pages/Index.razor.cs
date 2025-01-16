using ExpensoFinanceTracker.DataModel.Models;
using Microsoft.AspNetCore.Components;

namespace ExpensoFinanceTracker.Components.Pages
{
    public partial class Index : ComponentBase
    {

        [CascadingParameter]
        private GlobalState _globalState { get; set; }

        protected override void OnInitialized()
        {

            Nav.NavigateTo("/loginpage");

        }
    }
}