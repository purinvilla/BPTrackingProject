using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BloodPressureMAUI.ViewModels;

public partial class BaseViewModel : ObservableObject {

    [ObservableProperty]
    string title;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotLoading))]
    bool isLoading = false;

    public bool IsNotLoading => !IsLoading;

}
