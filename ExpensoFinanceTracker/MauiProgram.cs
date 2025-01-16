using ExpensoFinanceTracker.DataAccess.Services;
using ExpensoFinanceTracker.DataAccess.Services.Interface;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;

namespace ExpensoFinanceTracker
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddMudServices();
            builder.Services.AddScoped<IUserTxnService, UserTxnService>();  
            builder.Services.AddScoped<IUserService, UserService>();


#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
