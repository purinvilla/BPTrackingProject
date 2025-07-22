namespace BloodPressureMAUI;

public partial class App : Application {

	public App() {
		InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState) {
        Window window = new Window(new AppShell());

#if WINDOWS
        const int DefaultWidth = 512;
        const int DefaultHeight = 1024;

        // Change window size
        window.Width = DefaultWidth;
        window.Height = DefaultHeight;

        // Move to center of screen
        var disp = DeviceDisplay.Current.MainDisplayInfo;
        window.X = (disp.Width / disp.Density - window.Width) / 2;
        window.Y = (disp.Height / disp.Density - window.Height) / 2;
        //window.Activated += Window_Activated;
#endif

        return window;
    }

}
