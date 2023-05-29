using KatifiWebServer.Models.DTOModels;
using KatifiWebServer.Models.GoogleModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;

namespace KatifiWebServer.Services.GoogleServices
{
    public class GoogleService : IGoogleService
{
        private readonly string credentialsPath;
        private const string web_api_key = "AIzaSyD_GvcrrhfD04O5p0ac41yqqcTARttGyP8";
        private const string calendarAddress = "https://www.googleapis.com/calendar/v3/calendars"; // base address is 'https://www.googleapis.com'

        public GoogleService(IWebHostEnvironment environment)
        {
            credentialsPath = Path.Combine(environment.ContentRootPath, "Resources", "Credentials");
        }

        /*
        public async Task<IActionResult> RefreshToken()
        {
            return 
        }
        */

        public async Task<bool> CreateEvent(EventDTO newevent)
        {
            string? accesstoken = this.GetTokens().GetValueOrDefault("access_token", "").ToString();
            if (string.IsNullOrEmpty(accesstoken))
                return false;

            var googleevent = new GoogleEvent
            {
                Summary = newevent.Name,
                Description = $"Address: {newevent.Address.CountryCode}, {newevent.Address.City}. Fee: {newevent.Fee}. Organizer: {newevent.Organizer}.",
                Start = new EventDateTime { Date = newevent.Start.ToString("yyyy-MM-dd") },
                End = new EventDateTime { Date = newevent.End.ToString("yyyy-MM-dd") }
                //Datetime ha nem egész nap yyyy-MM-dd'T'HH:mm:ss.fffk
                //Date yyyy-MM-dd ha egész napos
            };         
            var model = JsonConvert.SerializeObject(googleevent, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            var content = new StringContent(model, Encoding.UTF8, "application/json");
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accesstoken);

                var eventResponse = await client.PostAsync($"{calendarAddress}/primary/events?alt=json&key={web_api_key}", content);

                if (!eventResponse.IsSuccessStatusCode)
                {
                    string debugReport = $"{eventResponse.StatusCode}-{eventResponse.ReasonPhrase}\nContent: {eventResponse.Content.ReadAsStringAsync()}\nRequest message: {eventResponse.RequestMessage}";
                    Debug.WriteLine("[HttpClient] Error: " + debugReport);
                    return false;
                }
                
                return true;
            }
            catch(Exception ex)
            {
                 Debug.WriteLine(ex.ToString());
            }

            return false;
        }

        public async Task GetGoogleTokens(string code)
        {
            var credentials = this.GetCredentials();
            if (credentials is null)
                return;

            string? clientId = credentials.GetValueOrDefault("client_id", string.Empty);
            string? clientSecret = credentials.GetValueOrDefault("client_secret", string.Empty);
            string? tokenUri = credentials.GetValueOrDefault("token_uri", string.Empty);

            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret) || string.IsNullOrEmpty(tokenUri))
                return;

            RestClient restClient = new RestClient(new Uri(tokenUri));
            RestRequest restRequest = new RestRequest();

            restRequest.AddQueryParameter("client_id", clientId);
            restRequest.AddQueryParameter("client_secret", clientSecret);
            restRequest.AddQueryParameter("code", code);
            restRequest.AddQueryParameter("grant_type", "authorization_code");
            restRequest.AddQueryParameter("redirect_uri", "https://zr4jwc3b-443.euw.devtunnels.ms/api/google/callback");

            var response = await restClient.PostAsync(restRequest);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                await File.WriteAllTextAsync(Path.Combine(credentialsPath, "google-tokens.json"), response.Content);
            }
        }

        public string? GetSignUri()
        {
            var credentials = this.GetCredentials();
            if (credentials is null)
                return null;

            string? clientId = credentials.GetValueOrDefault("client_id", string.Empty);
            if (string.IsNullOrEmpty(clientId))
                return null;

            var redirectUrl = "https://accounts.google.com/o/oauth2/v2/auth?" +
                "scope=https://www.googleapis.com/auth/calendar+https://www.googleapis.com/auth/calendar.events&" +
                "acces_type=offline&" +
                "include_granted_scopes=true&" +
                "response_type=code&" +
                "state=hukatifiapp&" +
                "redirect_uri=https://zr4jwc3b-443.euw.devtunnels.ms/api/google/callback&" +
                $"client_id={clientId}";

            return redirectUrl;
        }

        public Dictionary<string, string>? GetProjectData()
        {
            string path = Path.Combine(credentialsPath, "google-project.json");
            StreamReader reader = new StreamReader(path);
            var json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<Dictionary<string, string>?>(json);
        }

        public Dictionary<string, string>? GetCredentials()
        {
            string path = Path.Combine(credentialsPath, "google-credentials.json");
            StreamReader reader = new StreamReader(path);
            var json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<Dictionary<string, string>?>(json);
        }

        public Dictionary<string, object>? GetTokens()
        {
            string path = Path.Combine(credentialsPath, "google-tokens.json");
            StreamReader reader = new StreamReader(path);
            var json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<Dictionary<string, object>?>(json);
        }


    }
}
