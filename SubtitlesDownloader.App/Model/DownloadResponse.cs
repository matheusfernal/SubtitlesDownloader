namespace SubtitlesDownloader.App.Model;

class DownloadResponse
{
  public string Link { get; set; }
  public string FileName { get; set; }
  public int Requests { get; set; }
  public int Remaining { get; set; }
  public string Message { get; set; }
  public string ResetTime { get; set; }
  public DateTime ResetTimeUtc { get; set; }
}
