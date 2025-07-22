using BloodPressureMAUI.ViewModels;

namespace BloodPressureMAUI.Views;

public partial class DataEditPage : ContentPage {

    protected DataEditViewModel _dataEditViewModel;
    public DataEditPage(DataEditViewModel dataEditViewModel) {
		InitializeComponent();
		BindingContext = dataEditViewModel;
        _dataEditViewModel = dataEditViewModel;
    }

    protected override void OnAppearing() {
        base.OnAppearing();
        _dataEditViewModel.OnAppearing();
    }

}
