using BloodPressureMAUI.ViewModels;

namespace BloodPressureMAUI;

public partial class MainPage : ContentPage {

    // Properties
    LoginViewModel _loginViewModel;

    // Constructor
    public MainPage(LoginViewModel loginViewModel) {
		InitializeComponent();
		BindingContext = loginViewModel;
        _loginViewModel = loginViewModel;
    }

    protected override void OnAppearing() {
        base.OnAppearing();
        _loginViewModel.OnAppearing();
    }

}
