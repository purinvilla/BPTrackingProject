using BloodPressureMAUI.Resources.Strings;
using BloodPressureMAUI.Services;

namespace BloodPressureMAUI.Views;

public partial class LogoutPage : ContentPage {

    // Properties
    AccountApiService _accountApiService;

    // Constructor
    public LogoutPage(AccountApiService accountApiService) {
		InitializeComponent();
        _accountApiService = accountApiService;
    }

    // Remove authentication token and return to login page
    protected override async void OnAppearing() {
        base.OnAppearing();
        _accountApiService.authenticatedToken = "";
        await Shell.Current.DisplayAlert(AppResources.LogoutSuccess, AppResources.LogoutSuccessMessage, AppResources.OKPrompt);
        await Shell.Current.GoToAsync("//MainPage");
    }

}
