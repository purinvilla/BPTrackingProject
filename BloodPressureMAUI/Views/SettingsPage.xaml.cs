using BloodPressureMAUI.ViewModels;

namespace BloodPressureMAUI.Views;

public partial class SettingsPage : ContentPage {

    // Properties
    SettingsViewModel _settingsViewModel;

    // Constructor
    public SettingsPage(SettingsViewModel settingsViewModel) {
		InitializeComponent();
		BindingContext = settingsViewModel;
        _settingsViewModel = settingsViewModel;

    }

    protected override void OnAppearing() {
        base.OnAppearing();
        _settingsViewModel.OnAppearing();
    }

}
