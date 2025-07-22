using BloodPressureMAUI.Models;
using BloodPressureMAUI.Resources.Strings;
using BloodPressureMAUI.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BloodPressureMAUI.ViewModels;

public partial class LoginViewModel : BaseViewModel {

    // Properties
    AccountApiService _accountApiService;
    
    // Store user read from SecureStorage
    string? user;
    string? pwd;
    string? savePwd;

    [ObservableProperty]
    string userEmail;
    [ObservableProperty]
    string password;
    [ObservableProperty]
    bool savePasswordEnabled = false;

    // Constructor
    public LoginViewModel(AccountApiService accountApiService) {
        _accountApiService = accountApiService;
    }

    public async virtual void OnAppearing() {
        // Get saved username
        user = await SecureStorage.Default.GetAsync("user");
        UserEmail = (user == null) ? string.Empty : user;
        user = UserEmail; // if null set to string.Empty

        // Get saved password & preference
        pwd = await SecureStorage.Default.GetAsync("password");
        Password = (pwd == null) ? string.Empty : pwd;
        pwd = Password; // if null set to string.Empty

        savePwd = await SecureStorage.Default.GetAsync("savepwd");
        SavePasswordEnabled = (savePwd == null) ? false : bool.Parse(savePwd);
        savePwd = SavePasswordEnabled.ToString();

        // Get saved REST API
        string? apiHost = await SecureStorage.Default.GetAsync("savedapihost");
        string baseAddress = (apiHost == null) ? "http://localhost:8080" : apiHost;
        if (string.Compare(baseAddress, AccountApiService.baseAddress) != 0) { 
            AccountApiService.baseAddress = baseAddress;
            AccountApiService.httpClient = new() { BaseAddress = new Uri(baseAddress) };
        }
    }

    // Methods
    [RelayCommand]
	async Task Login() {
        if (UserEmail == String.Empty || Password == String.Empty) {
            //PopUpData popUpData = new PopUpData(AppResources.LoginFailed, AppResources.MissingField, AppResources.OKPrompt, "sad_face.svg");
            //await Shell.Current.Navigation.PushModalAsync(new PopUpPage { BindingContext = popUpData });
            await Shell.Current.DisplayAlert(AppResources.LoginFailed, AppResources.MissingField, AppResources.OKPrompt);

        } else {
            RegisteredUser account = new() {
                Email = UserEmail,
                Password = Password
            };

            // Get user role
            string role = await _accountApiService.LoginAccount(account);

            // If role not found, post error message
            if (role == null) {
                await Shell.Current.DisplayAlert(AppResources.LoginFailed, AppResources.InvalidCredentials, AppResources.OKPrompt);
            }
            
            // For administrators
            else if (role.Contains("ADMIN")) {
                // Only write when user changed
                if (string.Compare(user, UserEmail) != 0) {
                    await SecureStorage.Default.SetAsync("user", UserEmail);
                }
                await SecureStorage.Default.SetAsync("password", String.Empty);

                if (SavePasswordEnabled) {
                    // Only write when password changed
                    if (string.Compare(pwd, Password) != 0) {
                        await SecureStorage.Default.SetAsync("password", Password);
                    }
                } else {
                    if (pwd != string.Empty)
                        await SecureStorage.Default.SetAsync("password", String.Empty);
                }

                await Shell.Current.GoToAsync("admin");
            }
            
            // For regular users
            else {
                if (string.Compare(user, UserEmail) != 0) {
                    await SecureStorage.Default.SetAsync("user", UserEmail);
                }

                if (SavePasswordEnabled) {
                    // Only write when password changed
                    if (string.Compare(pwd, Password) != 0)
                        await SecureStorage.Default.SetAsync("password", Password);
                } else {
                    if (pwd != string.Empty)
                        await SecureStorage.Default.SetAsync("password", String.Empty);
                }

                await Shell.Current.GoToAsync("//user");
            }
        }
    }

    [RelayCommand]
    async Task Register() {
        await Shell.Current.GoToAsync("registration");
    }

    [RelayCommand]
    async Task AboutApp() {
        await Shell.Current.GoToAsync("aboutapp");
    }

    [RelayCommand]
    async Task CheckSavePWD() {
        // Only save change when status in secure storage is not the same as in program.
        if (bool.Parse(savePwd)!= SavePasswordEnabled) {
            await SecureStorage.Default.SetAsync(
                "savepwd", (SavePasswordEnabled == true) ? "True" : "False"
            );
        }
        
    }

}
