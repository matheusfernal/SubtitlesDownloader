using Microsoft.Extensions.Configuration;

namespace SubtitlesDownloader.App
{
  class Program
  {
    static async Task Main(string[] args)
    {
      var builder = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", true, true);
      var config = builder.Build();

      var apiKey = config["OpenSubtitles:ApiKey"];
      var username = config["OpenSubtitles:UserName"];
      var password = config["OpenSubtitles:Password"];

      var console = new ConsoleInteractor();

      Console.WriteLine("Paste the path of a directory to scan: ");
      var path = console.AskInput();
      var files = new DirectoryReader().ReadFiles(path);

      using var osClient = new OSClient(apiKey);
      var login = await osClient.LoginAsync(username, password);

      foreach (var filePath in files)
      {
        var movieHash = Hashing.ToHexadecimal(Hashing.ComputeMovieHash(filePath));
        var fileName = Path.GetFileName(filePath);
        var searchResponse = await osClient.SearchSubtitlesAsync(movieHash, fileName);

        Console.WriteLine();
        console.WriteLineWithFileName("File: ", fileName, "");

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
            var choice = console.AskInput();
            fileDownloader.DownloadFileAndSaveFileAsync(downloadResponse, filePath, choice.ToLower() == "y");
            Console.WriteLine("Subtitle file saved");
          }
          else
          {
            Console.WriteLine("Multiple subtitles found");
            Console.WriteLine();
            var option = 1;
            foreach (var subtitleAttributes in searchResponse.Data.Select(d => d.Attributes))
            {
              console.WriteLineWithSubtitleName($"{option++}: ", subtitleAttributes.Release, "");
              Console.WriteLine(subtitleAttributes.Comments);
              Console.WriteLine();
            }
            Console.WriteLine("Chose one typing the number (s to skip, a to all)");
            var choice = console.AskInput();
            
            if (int.TryParse(choice, out var index) && --index >= 0 && index < searchResponse.Data.Count)
            {
              var singleFile = searchResponse.Data[index].Attributes.Files.First(); //TODO: Consider others
              Console.WriteLine();
              console.WriteLineWithSubtitleFile("", singleFile.FileName, " will be downloaded");
              var singleDownloadResponse = await osClient.DownloadAsync(singleFile.FileId, login.Token);
              Console.WriteLine("Do you want to rename the subtitle (same name of video file)? (y/n)");
              choice = console.AskInput();
              Console.WriteLine();
              fileDownloader.DownloadFileAndSaveFileAsync(singleDownloadResponse, filePath, choice.ToLower() == "y");
              Console.WriteLine("Subtitle file saved");
            }
            else if (choice == "a")
            {
              foreach (var file in searchResponse.Data.Select(a => a.Attributes.Files.First()))
              {
                console.WriteLineWithSubtitleFile("", file.FileName, " will be downloaded");
                var downloadResponse = await osClient.DownloadAsync(file.FileId, login.Token);
                fileDownloader.DownloadFileAndSaveFileAsync(downloadResponse, filePath);
                Console.WriteLine("Subtitle file saved");
              }
            }
          }
        }
        Console.WriteLine();
        Console.WriteLine("-------------------");
      }
    }
  }
}
