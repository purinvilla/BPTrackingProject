using BloodPressureMAUI.Models;
using BloodPressureMAUI.Resources.Strings;
using BloodPressureMAUI.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkiaSharp;
using System.Collections.ObjectModel;
using Microcharts;
using Entry = Microcharts.ChartEntry;
using System.Text.RegularExpressions;

namespace BloodPressureMAUI.ViewModels;

public partial class DataViewViewModel : BaseViewModel {

    // Properties
    AccountApiService _accountApiService;

    public ObservableCollection<BPData> BPRecords { get; private set; } = new();

    [ObservableProperty]
    DateTime begindate;
    [ObservableProperty]
    DateTime enddate;
    [ObservableProperty]
    string monthlyViewText = "-30";
    [ObservableProperty]
    bool singleDayModeEnabled = true;
    [ObservableProperty]
    bool nextDayEnabled = false;

    // Data variables
    [ObservableProperty]
    DateTime datetime;
    [ObservableProperty]
    int systolic;
    [ObservableProperty]
    int diastolic;
    [ObservableProperty]
    int heartrate;

    // Entry list for chart plotting
    List<Entry> sysEntries = new List<Entry> { };
    List<Entry> diaEntries = new List<Entry> { };

    // Charts
    [ObservableProperty]
    LineChart syschart;
    [ObservableProperty]
    LineChart diachart;
    [ObservableProperty]
    bool showChart = false;

    // Constructor
    public DataViewViewModel(AccountApiService accountApiService) {
        _accountApiService = accountApiService;
        Begindate = DateTime.Now;
        Enddate = Begindate;
    }

    // Methods
    // Update data when viewing page
    public async virtual void OnAppearing() {
        await GetBPData();
    }

    // Format yyyy-MM-dd to MM-dd-yy
    public string dtFormat(string input) {
        var regexTest = new Regex(@"(?<year1>\d{2})(?<year2>\d{2})-(?<month>\d{2})-(?<date>\d{2})");

        var match = regexTest.Match(input);
        string output = string.Empty;
        
        if (match.Success) {
            var yearValue = match.Groups["year2"];
            var monthValue = match.Groups["month"];
            var dateValue = match.Groups["date"];
            output = $"{monthValue}-{dateValue}-{yearValue}";
        }
        
        return output;
    }

    [RelayCommand]
    async Task GetBPData() {
        // Get date range (1-30 days)
        int totalDays = Math.Abs((Begindate.Date - Enddate.Date).Days) + 1;
        
        if (totalDays > 30) {
            await Shell.Current.DisplayAlert(AppResources.DataViewTitle, AppResources.DateTotalMax, AppResources.OKPrompt);
            Begindate = Enddate.AddDays(-29);
            totalDays = Math.Abs((Begindate.Date - Enddate.Date).Days) + 1;
        }
        
        // Reset chart
        ShowChart = false;
        sysEntries = new List<Entry> { };
        diaEntries = new List<Entry> { };

        if (IsLoading) return;
        
        try {
            IsLoading = true;

            // Clear existing on-screen records 
            if (BPRecords.Any()) BPRecords.Clear();

            var tempData = new List<BPData>();

            // Check for an enabled enddate
            if (totalDays == 1) {
                tempData = await _accountApiService.GetDataFromDate(Begindate);
                SingleDayModeEnabled = true;    // Turn on Prev and Next button
                MonthlyViewText = "-30";
            } else {
                tempData = await _accountApiService.GetDataFromPeriod(Begindate, Enddate);
            }

            // Add data to records and Chart entries
            foreach (var data in tempData) {
                BPRecords.Add(data);

                // Single day
                if (totalDays == 1) { 
                    sysEntries.Add(new Entry(data.Systolic) {
                        Label = data.Posttime.ToString("HH:mm"),
                        ValueLabel = data.Systolic.ToString(),
                        Color = SKColor.Parse("#3498db")    // Blue
                    });

                    diaEntries.Add(new Entry(data.Diastolic) {
                        Label = data.Posttime.ToString("HH:mm"),
                        ValueLabel = data.Diastolic.ToString(),
                        Color = SKColor.Parse("#b455b6")    // Purple
                    });

                }
                // Multiple days
                else {
                    sysEntries.Add(new Entry(data.Systolic) {
                        Label = data.Posttime.ToString("yyyy-MM-dd"),
                        ValueLabel = data.Systolic.ToString(),
                        Color = SKColor.Parse("#3498db")
                    });

                    diaEntries.Add(new Entry(data.Diastolic) {
                        Label = data.Posttime.ToString("yyyy-MM-dd"),
                        ValueLabel = data.Diastolic.ToString(),
                        Color = SKColor.Parse("#b455b6")
                    });
                }
            }

            //Only show charts when there is data
            if (sysEntries.Count > 0) {
                // Generate systolic chart
                Syschart = new LineChart() {
                    LineMode = LineMode.Straight,
                    LineSize = 8,
                    PointMode = PointMode.Square,
                    PointSize = 22,
                    LabelTextSize = 24,
                    LabelOrientation = Microcharts.Orientation.Horizontal,
                    ValueLabelTextSize = 24,
                    ValueLabelOrientation = Microcharts.Orientation.Horizontal,
                };

                // Generate diastolic chart
                Diachart = new LineChart() {
                    LineMode = LineMode.Straight,
                    LineSize = 8,
                    PointMode = PointMode.Square,
                    PointSize = 22,
                    LabelTextSize = 24,
                    LabelOrientation = Microcharts.Orientation.Horizontal,
                    ValueLabelTextSize = 24,
                    ValueLabelOrientation = Microcharts.Orientation.Horizontal,
                };

                // If single day, convert to hourly chart
                if (totalDays == 1) {
                    Syschart.Entries = sysEntries.OrderBy(x => x.Label).ToList();
                    Diachart.Entries = diaEntries.OrderBy(x => x.Label).ToList();
                
                }
                // If multiple days, convert to daily chart
                else {
                    // Fill in systolic chart
                    var newSysEntries = sysEntries
                        .OrderBy(x => x.Label)
                        .GroupBy(p => p.Label)
                        .Select(g => new { Label = g.Key, Value = g.Average(p => p.Value) });

                    sysEntries = new List<Entry> { };

                    foreach (var result in newSysEntries) {
                        int v = (int)Convert.ToDecimal(result.Value);

                        sysEntries.Add(new Entry(v) {
                            Label = dtFormat(result.Label),
                            ValueLabel = v.ToString(),
                            Color = SKColor.Parse("#3498db")
                        });
                    }
                    
                    // Set chart labels to vertical for decluttering
                    Syschart.Entries = sysEntries;
                    Syschart.LabelOrientation = Microcharts.Orientation.Vertical;

                    if (sysEntries.Count > 15) {
                        Syschart.ValueLabelOrientation = Microcharts.Orientation.Vertical;
                        Diachart.ValueLabelOrientation = Microcharts.Orientation.Vertical;
                    }

                    // Fill in diastolic chart
                    var newDiasEntries = diaEntries
                        .OrderBy(x => x.Label)
                        .GroupBy(p => p.Label)
                        .Select(g => new { Label = g.Key, Value = g.Average(p => p.Value) });

                    diaEntries = new List<Entry> { };

                    foreach (var result in newDiasEntries) {
                        int v = (int)Convert.ToDecimal(result.Value);

                        diaEntries.Add(new Entry(v) {
                            Label = dtFormat(result.Label),
                            ValueLabel = v.ToString(),
                            Color = SKColor.Parse("#b455b6")
                        });
                    }
                    
                    Diachart.Entries = diaEntries;
                    Diachart.LabelOrientation = Microcharts.Orientation.Vertical;
                }
                
                ShowChart = true;

            }
            // If no data to show
            else {
                ShowChart = false;
            }

        } catch {
            await Shell.Current.DisplayAlert(AppResources.DataViewFailed, AppResources.DataViewFailedMessage, AppResources.OKPrompt);

        } finally {
            IsLoading = false;
        }
    }

    [RelayCommand]
    async Task ViewMonthly() {
        if((Begindate.Date - Enddate.Date).Days == 0) {
            MonthlyViewText = "◀ -30";
        } else {
            Enddate = Begindate;
        }

        SingleDayModeEnabled = false;
        Begindate = Enddate.AddDays(-29);
        await GetBPData();
    }

    [RelayCommand]
    async Task GetPrevDay() {
        Begindate = Enddate.AddDays(-1);
        Enddate = Enddate.AddDays(-1);
        await GetBPData();
    }

    [RelayCommand]
    async Task GetNextDay() {
        if (Enddate.Date != DateTime.Now.Date) {
            Enddate = Enddate.AddDays(1);
            Begindate = Enddate;
            await GetBPData();
        }
    }

    [RelayCommand]
    async Task SetToCurrentDate() {
        Begindate = DateTime.Now;
        Enddate = Begindate;
        await GetBPData();
    }

    [RelayCommand]
    async Task DeleteBPData(int id) {
        await _accountApiService.DeleteData(id);
        IsLoading = false;
        await GetBPData();
    }

    [RelayCommand]
    async Task EditBPData(BPData bpdata) {
        // Ref: https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/shell/navigation?view=net-maui-9.0#pass-single-use-object-based-navigation-data
        var navigationParameter = new ShellNavigationQueryParameters {
            { "bpdata", bpdata }
        };

        await Shell.Current.GoToAsync($"editpage", navigationParameter);
    }

    [RelayCommand]
    async Task CheckBegindate() {
        // Cannot go past current date
        if ((Begindate.Date - DateTime.Now.Date).Days > 0)
            Begindate = DateTime.Now;

        NextDayEnabled = !((Enddate.Date - DateTime.Now.Date).Days == 0);
    }

    [RelayCommand]
    async Task CheckEnddate() {
        // Cannot go past current date
        if ((Enddate.Date - DateTime.Now.Date).Days > 0)
            Enddate = DateTime.Now;

        NextDayEnabled = !((Enddate.Date - DateTime.Now.Date).Days == 0);
    }

}
