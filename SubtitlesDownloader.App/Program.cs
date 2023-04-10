using System.Web;
using System.Net.Http.Json;
using System.Text.Json;
using JsonStringCaseConverter;
using SubtitlesDownloader.App.Model;

namespace SubtitlesDownloader.App
{
  class Program
  {
    private const string ApiKey = "";
    private const string Username = "";
    private const string Password = "";

    static async Task Main(string[] args)
    {
      Console.WriteLine("Paste the path of a directory to scan: ");
      Console.Write(">> ");
      var path = Console.ReadLine();
      var files = new DirectoryReader().ReadFiles(path);

      using var osClient = new OSClient(ApiKey);
      var login = await osClient.LoginAsync(Username, Password);

      foreach (var filePath in files)
      {
        var movieHash = Hashing.ToHexadecimal(Hashing.ComputeMovieHash(filePath));
        var fileName = Path.GetFileName(filePath);
        var searchResponse = await osClient.SearchSubtitlesAsync(movieHash, fileName);

        Console.WriteLine($"File: {fileName}");

        if (searchResponse.TotalCount > 0) 
        {
          var fileDownloader = new FileDownloader();
          if (searchResponse.Data.Where(d => d.Attributes.MoviehashMatch).Any())
          {
            Console.WriteLine("Found subtitle by moviehash");

            var firstFile = searchResponse.Data.Where(d => d.Attributes.MoviehashMatch).First().Attributes.Files.First();
            Console.WriteLine($"Downloading first subtitle from moviehash: {firstFile.FileName}"); //TODO: Consider others
            var downloadResponse = await osClient.DownloadAsync(firstFile.FileId, login.Token);
            Console.WriteLine("Do you want to rename the subtitle (same name of video file)? (y/n)");
            var choice = Console.ReadLine();
            fileDownloader.DownloadFileAndSaveFileAsync(downloadResponse, filePath, choice.ToLower() == "s");
            Console.WriteLine("Subtitle file saved");
          }
          else
          {
            Console.WriteLine("Multiple subtitles found");
            var option = 1;
            foreach (var subtitleAttributes in searchResponse.Data.Select(d => d.Attributes))
            {
              Console.WriteLine($"{option++}: {subtitleAttributes.Release}");
              Console.WriteLine(subtitleAttributes.Comments);
              Console.WriteLine("---");
            }
            Console.WriteLine("Chose one typing the number");
            Console.Write(">> ");
            var choice = Console.ReadLine();
            var index = int.Parse(choice) - 1;
            
            var file = searchResponse.Data[index].Attributes.Files.First(); //TODO: Consider others
            Console.WriteLine($"{file.FileName} will be downloaded");
            var downloadResponse = await osClient.DownloadAsync(file.FileId, login.Token);
            Console.WriteLine("Do you want to rename the subtitle (same name of video file)? (y/n)");
            choice = Console.ReadLine();
            fileDownloader.DownloadFileAndSaveFileAsync(downloadResponse, filePath, choice.ToLower() == "s");
            Console.WriteLine("Subtitle file saved");
          }
        }
        Console.WriteLine("===");
      }
    }
  }
}
