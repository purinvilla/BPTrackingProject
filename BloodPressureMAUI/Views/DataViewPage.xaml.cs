using BloodPressureMAUI.ViewModels;

namespace BloodPressureMAUI.Views;

public partial class DataViewPage : ContentPage {

    // Properties
    protected DataViewViewModel _dataViewViewModel;

    // Constructor
    public DataViewPage(DataViewViewModel dataViewViewModel) {
		InitializeComponent();
        BindingContext = dataViewViewModel;
        _dataViewViewModel = dataViewViewModel;
    }

    // Update Data everytime Data View Page get accessed
    protected override void OnAppearing() {
        base.OnAppearing();
        _dataViewViewModel.OnAppearing();
    }

}
