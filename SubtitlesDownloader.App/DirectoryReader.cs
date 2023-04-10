namespace SubtitlesDownloader.App;

class DirectoryReader
{
  public IEnumerable<string> ReadFiles(string path)
  {
    foreach (var file in Directory.GetFiles(path))
    {
      yield return file;
    }
  }
}
