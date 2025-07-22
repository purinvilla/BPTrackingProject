using BloodPressureMAUI.Resources.Strings;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BloodPressureMAUI.ViewModels;

public partial class SettingsViewModel : BaseViewModel {

    // General properties
    [ObservableProperty]
    string apiHostLocation;
    [ObservableProperty]
    string savedPhoneNumbers = string.Empty;

    // SMS alarm values
    [ObservableProperty]
    int systolicAlarmUpper = 160;
    [ObservableProperty]
    int systolicAlarmLower = 90;
    [ObservableProperty]
    int diastolicAlarmUpper = 100;
    [ObservableProperty]
    int diastolicAlarmLower = 60;

    // Store values read from SecureStorage
    string? _savedApiHost;
    string? _systolicAlarmUpper;
    string? _systolicAlarmLower;
    string? _diastolicAlarmUpper;
    string? _diastolicAlarmLower;
    string? _savedPhoneNumbers;

    // Constructor
    public SettingsViewModel() { }

    public async virtual void OnAppearing() {
        // Get saved API host
        _savedApiHost = await SecureStorage.Default.GetAsync("savedapihost");
        ApiHostLocation = (_savedApiHost == null) ? "http://localhost:8080" : _savedApiHost;
        
        // Get systolic alarm values
        _systolicAlarmUpper = await SecureStorage.Default.GetAsync("systolicAlarmUpper");
        SystolicAlarmUpper = (_systolicAlarmUpper == null) ? 160 : int.Parse(_systolicAlarmUpper);

        _systolicAlarmLower = await SecureStorage.Default.GetAsync("systolicAlarmLower");
        SystolicAlarmLower = (_systolicAlarmLower == null) ? 90 : int.Parse(_systolicAlarmLower);

        // Get diastolic alarm values
        _diastolicAlarmUpper = await SecureStorage.Default.GetAsync("diastolicAlarmUpper");
        DiastolicAlarmUpper = (_diastolicAlarmUpper == null) ? 100 : int.Parse(_diastolicAlarmUpper);

        _diastolicAlarmLower = await SecureStorage.Default.GetAsync("diastolicAlarmLower");
        DiastolicAlarmLower = (_diastolicAlarmLower == null) ? 60 : int.Parse(_diastolicAlarmLower);

        // Get saved phone numbers
        _savedPhoneNumbers = await SecureStorage.Default.GetAsync("savedPhoneNumbers");
        SavedPhoneNumbers = (_savedPhoneNumbers == null) ? string.Empty : _savedPhoneNumbers;
    }

    // Methods
    [RelayCommand]
    async Task SaveSettings() {
        // Post error if API Host Name field left empty
        if (ApiHostLocation == String.Empty) {
            await Shell.Current.DisplayAlert(AppResources.SettingsNotSaved, AppResources.MissingField, AppResources.OKPrompt);
        }
        // Post error if alarm ranges are inverted
        else if ((SystolicAlarmUpper < SystolicAlarmLower) || (DiastolicAlarmUpper < DiastolicAlarmLower)) {
            await Shell.Current.DisplayAlert(AppResources.SettingsNotSaved, AppResources.InvalidAlarmRange, AppResources.OKPrompt);
        }
        // No errors
        else {
            if (_savedApiHost != ApiHostLocation)
                await SecureStorage.Default.SetAsync("savedapihost", ApiHostLocation);

            // Get systolic alarm value
            await SecureStorage.Default.SetAsync("systolicAlarmUpper", SystolicAlarmUpper.ToString());
            await SecureStorage.Default.SetAsync("systolicAlarmLower", SystolicAlarmLower.ToString());
            await SecureStorage.Default.SetAsync("diastolicAlarmUpper", DiastolicAlarmUpper.ToString());
            await SecureStorage.Default.SetAsync("diastolicAlarmLower", DiastolicAlarmLower.ToString());

            if (SavedPhoneNumbers == null)
                SavedPhoneNumbers = string.Empty;

            // Get saved phone numbers
            await SecureStorage.Default.SetAsync("savedPhoneNumbers", SavedPhoneNumbers);

            await Shell.Current.DisplayAlert(AppResources.SettingsSaved, AppResources.SettingsSavedMessage, AppResources.OKPrompt);
        }
    }

}
