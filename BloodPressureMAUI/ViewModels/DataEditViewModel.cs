using BloodPressureMAUI.Models;
using BloodPressureMAUI.Resources.Strings;
using BloodPressureMAUI.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BloodPressureMAUI.ViewModels;

[QueryProperty(nameof(Bpdata), "bpdata")]
public partial class DataEditViewModel : BaseViewModel {
    
    // Properties
    AccountApiService _accountApiService;

    // To accept QueryProperty:
    // 2nd parameter has to match property name (i.e. bpdata)
    // 1st parameter has to match public method (i.e. Bpdata)
    // Ref: https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/shell/navigation?view=net-maui-9.0#pass-single-use-object-based-navigation-data
    BPData bpdata;
    public BPData Bpdata {
        get { return bpdata; }
        set {
            bpdata = value;
            OnPropertyChanged();
        }
    }

    [ObservableProperty]
    string systolic;
    [ObservableProperty]
    string diastolic;
    [ObservableProperty]
    string heartrate;

    // Constructor
    public DataEditViewModel(AccountApiService accountApiService) {
        _accountApiService = accountApiService;
    }

    public virtual void OnAppearing() {
        Systolic = bpdata.Systolic.ToString();
        Diastolic = bpdata.Diastolic.ToString();
        Heartrate = bpdata.Heartrate.ToString();
    }

    [RelayCommand]
    async Task UpdateData() {
        if (Systolic == String.Empty || Diastolic == String.Empty || Heartrate == String.Empty) {
            await Shell.Current.DisplayAlert(AppResources.DataEntryFailed, AppResources.MissingField, AppResources.OKPrompt);

        } else {
            BPData updateData = new BPData(
                DateTime.Now, int.Parse(Systolic), int.Parse(Diastolic), int.Parse(Heartrate)
            );
            
            await _accountApiService.UpdateData(updateData, bpdata.Id);
            await Shell.Current.GoToAsync("..");
        }
    }

}
