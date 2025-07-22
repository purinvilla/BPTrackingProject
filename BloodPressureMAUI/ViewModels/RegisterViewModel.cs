using BloodPressureMAUI.Models;
using BloodPressureMAUI.Resources.Strings;
using BloodPressureMAUI.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BloodPressureMAUI.ViewModels;

public partial class RegisterViewModel : BaseViewModel {

    // Properties
    private AccountApiService _accountApiService;

    [ObservableProperty]
    string username;
    [ObservableProperty]
    string userEmail;
    [ObservableProperty]
    string password;
    [ObservableProperty]
    string confirmPassword;

    // Constructor
    public RegisterViewModel(AccountApiService accountApiService) {
        _accountApiService = accountApiService;
        Username = String.Empty;
        UserEmail = String.Empty;
        Password = String.Empty;
        ConfirmPassword = String.Empty;
    }

    // Methods
    [RelayCommand]
    async Task Register() {
        if (Username == String.Empty || UserEmail == String.Empty || Password == String.Empty || ConfirmPassword == String.Empty) {
            await Shell.Current.DisplayAlert(AppResources.RegistrationFailed, AppResources.MissingField, AppResources.OKPrompt);

        } else if (!string.Equals(Password, ConfirmPassword)) {
            await Shell.Current.DisplayAlert(AppResources.RegistrationFailed, AppResources.ConfirmPasswordFailed, AppResources.OKPrompt);

        } else {
            RegisteredUser newAccount = new() {
                Nickname = Username,
                Email = UserEmail,
                Password = Password
            };

            await _accountApiService.AddAccount(newAccount);
        }
    }

    [RelayCommand]
    async Task GoBack() {
        await Shell.Current.GoToAsync("..");
    }

}
