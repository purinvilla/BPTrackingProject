using BloodPressureMAUI.Services;
using BloodPressureMAUI.ViewModels;
using BloodPressureMAUI.Views;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microcharts.Maui;

namespace BloodPressureMAUI;

public static class MauiProgram {

	public static MauiApp CreateMauiApp() {
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMicrocharts()
            .ConfigureFonts(fonts => {
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		//builder.Services.AddTransient<AccountApiService>();
		builder.Services.AddSingleton<AccountApiService>();

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddSingleton<RegistrationPage>();
        builder.Services.AddSingleton<RegisterViewModel>();
		builder.Services.AddSingleton<DataEntryPage>();
        builder.Services.AddSingleton<DataEntryViewModel>();
        builder.Services.AddSingleton<DataViewViewModel>();
        builder.Services.AddSingleton<DataEditPage>();
        builder.Services.AddSingleton<DataEditViewModel>();
        builder.Services.AddSingleton<SettingsPage>();
        builder.Services.AddSingleton<SettingsViewModel>();
#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

}
