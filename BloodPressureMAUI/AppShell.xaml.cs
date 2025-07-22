using System.Globalization;
using BloodPressureMAUI.Resources.Strings;
using BloodPressureMAUI.Views;

namespace BloodPressureMAUI;

public partial class AppShell : Shell {

	public AppShell() {
		InitializeComponent();
        //AppResources.Culture = new CultureInfo("th-TH");
        Routing.RegisterRoute("registration", typeof(RegistrationPage));
        Routing.RegisterRoute("admin", typeof(AdminPage));
        Routing.RegisterRoute("aboutapp", typeof(AboutAppPage));
        Routing.RegisterRoute("editpage", typeof(DataEditPage));
    }

}
