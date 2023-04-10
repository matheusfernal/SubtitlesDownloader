using System.Buffers.Text;
using System;
using System.Net.Http.Json;
using System.Text.Json;
using System.Web;
using JsonStringCaseConverter;
using SubtitlesDownloader.App.Model;
using RestSharp;
using RestSharp.Serializers.Json;

namespace SubtitlesDownloader.App;

class OSClient : IDisposable
{
  private readonly RestClient _client;

  public OSClient(string apiKey)
  {
    var options = new RestClientOptions("https://api.opensubtitles.com");
    _client = new RestClient(options,
      configureSerialization: s => s.UseSystemTextJson(new JsonSerializerOptions
      {
        PropertyNamingPolicy = new JsonStringCaseNamingPolicy(StringCases.SnakeCase)
      })
    );
    _client.AddDefaultHeader("Api-Key", apiKey);
  }

  public void Dispose()
  {
    _client?.Dispose();
    GC.SuppressFinalize(this);
  }

  public async Task<LoginResponse?> LoginAsync(string username, string password)
  {
    var loginContent = new { username, password };

    var loginResponse = await _client.PostJsonAsync<object, LoginResponse>("api/v1/login", loginContent);
    return loginResponse;
  }

  public async Task<SearchResponse?> SearchSubtitlesAsync(string movieHash, string fileName)
  {
    var args = new
    {
      languages = "pt-Br",
      movieHash,
      query = fileName
    };

    var searchResponse = await _client.GetJsonAsync<SearchResponse>("api/v1/subtitles", args);
    return searchResponse;
  }

  public async Task<DownloadResponse?> DownloadAsync(int fileId, string token)
  {
    var downloadContent = new { fileId };

    var request = new RestRequest("api/v1/download");
    request.AddHeader("Authorization", $"Bearer {token}");
    request.AddJsonBody(downloadContent);

    var downloadResponse = await _client.PostAsync<DownloadResponse>(request);
    return downloadResponse;
  }
}
