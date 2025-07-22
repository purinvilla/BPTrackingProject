using System.Net.Http.Headers;
using System.Text;
using BloodPressureMAUI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BloodPressureMAUI.Resources.Strings;
using System.Text.Json.Nodes;

namespace BloodPressureMAUI.Services;

public class AccountApiService {

	// Properties
    public static string baseAddress = "http://localhost:8080";
    public static HttpClient httpClient = new() { BaseAddress = new Uri(baseAddress) };
    private static AuthenticationHeaderValue authValue;
    public string authenticatedToken = String.Empty;

    public string BaseAddress {
        get { return baseAddress; }
        set { baseAddress = value; }
    }
    public HttpClient HttpClient {
        get { return httpClient; }
        set { httpClient = value; }
    }

    // Constructor
    public AccountApiService() { }

    // Methods
    public async Task AddAccount(RegisteredUser account) {
        try {
            var data = System.Text.Json.JsonSerializer.Serialize(account);
            var postData = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("/register/user", postData);
            response.EnsureSuccessStatusCode();

            await Shell.Current.DisplayAlert(AppResources.RegistrationSuccess, AppResources.RegistationSuccessMessage, AppResources.OKPrompt);

        } catch {
            await Shell.Current.DisplayAlert(AppResources.RegistrationFailed, AppResources.RegistrationFailedMessage, AppResources.OKPrompt);
        }
    }

    public async Task<string?> LoginAccount(RegisteredUser account) {
        try {
            string authString = $"{account.Email}:{account.Password}";

            var utf8 = new UTF8Encoding();
            byte[] authByte = utf8.GetBytes(authString);
            string authToken = Convert.ToBase64String(authByte);

            // Add Authentication header to the request
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

            // Save AuthenticationHeaderValue for future use
            authValue = httpClient.DefaultRequestHeaders.Authorization;

            var response = await httpClient.GetAsync("/account");

            if (response.IsSuccessStatusCode) {
                var result = response.Content.ReadAsStringAsync().Result;
                JObject? s = JsonConvert.DeserializeObject(result) as JObject;
                authenticatedToken = authToken;

                await Shell.Current.DisplayAlert(AppResources.LoginSuccess, AppResources.LoginSuccessMessage, AppResources.OKPrompt);

                string role = (string)s["Role"];
                return role;
            }

        } catch {
            await Shell.Current.DisplayAlert(AppResources.LoginFailed, AppResources.LoginFailedMessage, AppResources.OKPrompt);
        }

        return null;
    }

    public async Task AddData(BPData bpData) {
        try {
            var data = System.Text.Json.JsonSerializer.Serialize(bpData);

            //Posttime will be added by Server
            JsonObject jsonObject = JsonNode.Parse(data).AsObject();
            jsonObject.Remove("Id");
            jsonObject.Remove("Posttime");
            data = jsonObject.ToJsonString();

            var postData = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("/account/data", postData);
            response.EnsureSuccessStatusCode();

            await Shell.Current.DisplayAlert(AppResources.DataEntrySuccess, AppResources.DataEntrySuccessMessage, AppResources.OKPrompt);

            // Set to -1 to clear the data after successfully record.
            bpData.Systolic = -1;

        } catch {
            await Shell.Current.DisplayAlert(AppResources.DataEntryFailed, AppResources.DataEntryFailedMessage, AppResources.OKPrompt);
        }
    }

    public async Task<List<BPData>?> GetData() {
        try {
            var response = await httpClient.GetStringAsync("/account/data");
            return JsonConvert.DeserializeObject<List<BPData>>(response);

        } catch {
            await Shell.Current.DisplayAlert(AppResources.DataNotFound, AppResources.DataNotFoundMessage, AppResources.OKPrompt);
        }

        return null;
    }

    public async Task<List<BPData>?> GetDataFromDate(DateTime beginDate) {
        try {
            String beginDateString = beginDate.ToString("yyyy-MM-dd");
            var response = await httpClient.GetStringAsync($"/account/data/date/{beginDateString}");
            return JsonConvert.DeserializeObject<List<BPData>>(response);

        } catch {
            await Shell.Current.DisplayAlert(AppResources.DataNotFound, AppResources.DataNotFoundMessage, AppResources.OKPrompt);
        }

        return null;
    }

    public async Task<List<BPData>?> GetDataFromPeriod(DateTime beginDate, DateTime endDate) {
        try {
            String beginDateString = beginDate.ToString("yyyy-MM-dd");
            String endDateString = endDate.ToString("yyyy-MM-dd");
            var response = await httpClient.GetStringAsync($"/account/data/date/{beginDateString}/{endDateString}");
            
            return JsonConvert.DeserializeObject<List<BPData>>(response);

        } catch {
            await Shell.Current.DisplayAlert(AppResources.DataNotFound, AppResources.DataNotFoundMessage, AppResources.OKPrompt);
        }

        return null;
    }

    public async Task UpdateData(BPData updatedData, int id) {
        try {
            var data = System.Text.Json.JsonSerializer.Serialize(updatedData);

            //Posttime will be added by Server
            JsonObject jsonObject = JsonNode.Parse(data).AsObject();
            jsonObject.Remove("Id");
            jsonObject.Remove("Posttime");
            data = jsonObject.ToJsonString();

            var putData = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync($"/account/data/{id}", putData);
            response.EnsureSuccessStatusCode();

            await Shell.Current.DisplayAlert(AppResources.UpdateSuccess, AppResources.UpdateSuccessMessage, AppResources.OKPrompt);

        } catch {
            await Shell.Current.DisplayAlert(AppResources.UpdateFailed, AppResources.UpdateFailedMessage, AppResources.OKPrompt);
        }
    }

    public async Task DeleteData(int id) {
        try {
            var response = await httpClient.DeleteAsync($"/account/data/{id}");
            response.EnsureSuccessStatusCode();

            await Shell.Current.DisplayAlert(AppResources.DeleteSuccess, AppResources.DeleteSuccessMessage, AppResources.OKPrompt);

        } catch {
            await Shell.Current.DisplayAlert(AppResources.DataNotFound, AppResources.DataNotFoundMessage, AppResources.OKPrompt);
        }
    }

}
