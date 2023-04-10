namespace SubtitlesDownloader.App.Model;

class LoginResponse
{
  public User User { get; set; }
  public string Token { get; set; }
  public int Status { get; set; }
}

class User {
  public int AllowedTransalations { get; set; }
  public int AllowedDownloads { get; set; }
  public int RemainingDownloads { get; set; }
  public string Level { get; set; }
  public int UserId { get; set; }
  public bool ExtInstalled { get; set; }
  public bool Vip { get; set; }

}