using BloodPressureMAUI.ViewModels;

namespace BloodPressureMAUI.Views;

public partial class RegistrationPage : ContentPage {

	public RegistrationPage(RegisterViewModel registerViewModel) {
		InitializeComponent();
		BindingContext = registerViewModel;
	}

}
