using BloodPressureMAUI.ViewModels;

namespace BloodPressureMAUI.Views;

public partial class DataEntryPage : ContentPage {

    // Properties
    protected DataEntryViewModel _dataEntryViewModel;

    // Constructor
    public DataEntryPage(DataEntryViewModel dataEntryViewModel) {
		InitializeComponent();
		BindingContext = dataEntryViewModel;
        _dataEntryViewModel = dataEntryViewModel;
    }

    // Update Data everytime DataEntryPage gets accessed
    protected override void OnAppearing() {
        base.OnAppearing();
        _dataEntryViewModel.OnAppearing();
    }

}
