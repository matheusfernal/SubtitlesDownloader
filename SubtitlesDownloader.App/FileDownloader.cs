using System.Net;
using SubtitlesDownloader.App.Model;

namespace SubtitlesDownloader.App;

class FileDownloader
{
  public void DownloadFileAndSaveFileAsync(DownloadResponse downloadResponse, string videoFilePath, bool renameSubtileAsVideoTitle = false)
  {
    var subtitleFileName = renameSubtileAsVideoTitle 
      ? RenameSubtilteFile(Path.GetFileName(videoFilePath), downloadResponse.FileName) 
      : downloadResponse.FileName;
    subtitleFileName = Path.Combine(Path.GetDirectoryName(videoFilePath), subtitleFileName); 

    using var webClient = new WebClient();
    webClient.DownloadFileAsync(new Uri(downloadResponse.Link), subtitleFileName);
  }

  private string RenameSubtilteFile(string videoFileName, string subtilteFileName)
  {
    var videoFileNameExtension = videoFileName.Split('.').Last();
    var videoFileNameWithoutExtension = videoFileName.Replace($".{videoFileNameExtension}", "");
    var subtitleFileExtension = subtilteFileName.Split('.').Last();

    return $"{videoFileNameWithoutExtension}.{subtitleFileExtension}"; 
  }
}
