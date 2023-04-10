using System.Buffers.Text;
using System;
using System.Net.Http.Json;
using System.Text.Json;
using System.Web;
using JsonStringCaseConverter;
using SubtitlesDownloader.App.Model;
using HttpClientToCurl;

namespace SubtitlesDownloader.App;

class OSClient : IDisposable
{
  private HttpClient HttpClient { get; }
  private JsonSerializerOptions JsonOptions { get; }

  public OSClient(string apiKey)
  {
    HttpClient = new HttpClient();
    HttpClient.BaseAddress = new Uri("https://api.opensubtitles.com");
    HttpClient.DefaultRequestHeaders.Add("Api-Key", apiKey);

    JsonOptions = new JsonSerializerOptions
    {
      PropertyNamingPolicy = new JsonStringCaseNamingPolicy(StringCases.SnakeCase)
    };
  }

  public void Dispose()
  {
    HttpClient.Dispose();
  }

  public async Task<LoginResponse?> LoginAsync(string username, string password)
  {
    var loginContent = new { username, password };
    var loginResponse = await HttpClient.PostAsJsonAsync("api/v1/login", loginContent, JsonOptions);

    if (loginResponse.IsSuccessStatusCode)
    {
      var loginResponseObj = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>(JsonOptions);
      return loginResponseObj;
    }

    return null;
  }

  public async Task<SearchResponse?> SearchSubtitlesAsync(string movieHash, string fileName)
  {
    var query = HttpUtility.ParseQueryString(string.Empty);
    query["languages"] = "pt-BR";
    query["moviehash"] = movieHash;
    query["query"] = fileName;

    var response = await HttpClient.GetAsync($"api/v1/subtitles?{query.ToString()}");
    if (response.IsSuccessStatusCode)
    {
      var responseObj = await response.Content.ReadFromJsonAsync<SearchResponse>(JsonOptions);
      return responseObj;
    }

    return null;
  }

  public async Task<DownloadResponse?> DownloadAsync(int fileId, string token)
  {
    // TODO: Pass the login token when not dev
    // var downloadContent = new { fileId };
    // var downloadResponse = await HttpClient.PostAsJsonAsync("api/v1/download", downloadContent, JsonOptions);
    
    var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/download");
    request.Headers.Add("Authorization", $"Bearer {token}");
    request.Content = new StringContent("{\n \"file_id\": " + fileId + "\n}", null, "application/json");
    var downloadResponse = await HttpClient.SendAsync(request);

    HttpClient.GenerateCurlInConsole(request);

    if (downloadResponse.IsSuccessStatusCode)
    {
      var downloadResponseObj = await downloadResponse.Content.ReadFromJsonAsync<DownloadResponse>(JsonOptions);
      return downloadResponseObj;
    }

    return null;
  }
}
