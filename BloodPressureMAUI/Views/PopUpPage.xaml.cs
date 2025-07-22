namespace BloodPressureMAUI.Views;

public partial class PopUpPage : ContentPage {
    
    // Page for custom alerts.
    // Data passed in from BindingContent.
    // Ref: https://learn.microsoft.com/en-us/dotnet/maui/user-interface/pages/navigationpage?view=net-maui-9.0
    public PopUpPage() {
		InitializeComponent();
    }

    private void Button_Clicked(object sender, EventArgs e) {
		Shell.Current.Navigation.PopAsync();
    }

}
