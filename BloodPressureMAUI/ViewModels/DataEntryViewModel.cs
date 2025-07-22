using BloodPressureMAUI.Models;
using BloodPressureMAUI.Resources.Strings;
using BloodPressureMAUI.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BloodPressureMAUI.ViewModels;

public partial class DataEntryViewModel : BaseViewModel {

    // General properties
    AccountApiService accountApiService;
    string[] _phoneNumbersList;
    string _userEmail;

    [ObservableProperty]
    string systolic;
    [ObservableProperty]
    string diastolic;
    [ObservableProperty]
    string heartrate;

    // SMS alarm values
    int _systolicAlarmUpper = 160;
    int _systolicAlarmLower = 90;
    int _diastolicAlarmUpper = 100;
    int _diastolicAlarmLower = 60;

    // Constructor
    public DataEntryViewModel(AccountApiService accountApiService) {
        this.accountApiService = accountApiService;
        Systolic = string.Empty;
        Diastolic = string.Empty;
        Heartrate = string.Empty;
    }

    public async virtual void OnAppearing() {
        // Get systolic alarm values from settings
        string? temp = await SecureStorage.Default.GetAsync("systolicAlarmUpper");
        _systolicAlarmUpper = (temp == null || temp == string.Empty) ? 160 : int.Parse(temp);
        
        temp = await SecureStorage.Default.GetAsync("systolicAlarmLower");
        _systolicAlarmLower = (temp == null || temp == string.Empty) ? 90 : int.Parse(temp);
        
        // Get diastolic alarm values from settings
        temp = await SecureStorage.Default.GetAsync("diastolicAlarmUpper");
        _diastolicAlarmUpper = (temp == null || temp == string.Empty) ? 100 : int.Parse(temp);

        temp = await SecureStorage.Default.GetAsync("diastolicAlarmLower");
        _diastolicAlarmLower = (temp == null || temp == string.Empty) ? 60 : int.Parse(temp);

        // Get authorized phone numbers from settings
        string? phones = await SecureStorage.Default.GetAsync("savedPhoneNumbers");
        _phoneNumbersList = (phones == null || phones == string.Empty) ? new string[] { } : phones.Split(',');

        // Get user
        string? user = await SecureStorage.Default.GetAsync("user");
        _userEmail = (user == null) ? string.Empty : user;
    }

    // Methods
    [RelayCommand]
    async Task AddData() {
        if (Systolic == String.Empty || Diastolic == String.Empty || Heartrate == String.Empty) {
            await Shell.Current.DisplayAlert(AppResources.DataEntryFailed, AppResources.MissingField, AppResources.OKPrompt);
        
        } else {
            int systolicValue = int.Parse(Systolic);
            int diastolicValue = int.Parse(Diastolic);

            BPData newData = new BPData(
                DateTime.Now, int.Parse(Systolic), int.Parse(Diastolic), int.Parse(Heartrate)
            );

            await accountApiService.AddData(newData);

            // Check successfully flag to clear input data
            if (newData.Systolic == -1) {
                Systolic = string.Empty;
                Diastolic = string.Empty;
                Heartrate = string.Empty;

                /* Sends SMS when blood pressure values are too high or low according to settings.
                 * The phone number for the Android emulator is 650-555-1212.
                 */
                if (_phoneNumbersList.Count() > 0 && Sms.Default.IsComposeSupported) {
                    string systolicText = string.Empty;
                    string diastolicText = string.Empty;
                    string alertText = string.Empty;

                    // Alarm for systolic
                    if (systolicValue >= _systolicAlarmUpper || systolicValue <= _systolicAlarmLower) { 
                        systolicText = string.Format(AppResources.SMSMessageSystolic, systolicValue, _systolicAlarmLower, _systolicAlarmUpper);
                        alertText = systolicText;
                    }

                    // Alarm for diastolic
                    if (diastolicValue >= _diastolicAlarmUpper || diastolicValue <= _diastolicAlarmLower) { 
                        diastolicText = string.Format(AppResources.SMSMessageDiastolic, diastolicValue, _diastolicAlarmLower, _diastolicAlarmUpper);
                        alertText = (alertText == string.Empty)? diastolicText : systolicText + "\n" + diastolicText;
                    }

                    // Send message
                    if (alertText != string.Empty) {
                        string fullText = string.Format(AppResources.SMSMessage + "\n" + alertText, _userEmail);
                        var message = new SmsMessage(fullText, _phoneNumbersList);
                        await Sms.Default.ComposeAsync(message);
                    }
                }
            }

        }
    }

}
