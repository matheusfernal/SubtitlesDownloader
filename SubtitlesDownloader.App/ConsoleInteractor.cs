namespace SubtitlesDownloader.App
{
  class ConsoleInteractor
  {
    public void WriteLineWithFileName(string textBefore, string fileName, string textAfter)
    {
      WriteLineWithTextInColor(textBefore, fileName, textAfter, ConsoleColor.Green); 
    }

    public void WriteLineWithSubtitleName(string textBefore, string subtitleName, string textAfter)
    {
      WriteLineWithTextInColor(textBefore, subtitleName, textAfter, ConsoleColor.Cyan);
    }

    public void WriteLineWithSubtitleFile(string textBefore, string subtitleFile, string textAfter)
    {
      WriteLineWithTextInColor(textBefore, subtitleFile, textAfter, ConsoleColor.Yellow);
    }

    public string AskInput()
    {
      Console.Write(">> ");
      var input = Console.ReadLine();

      return input;
    }

    private void WriteLineWithTextInColor(string textBefore, string textInColor, string textAfter, ConsoleColor color)
    {
      Console.Write(textBefore);
      Console.ForegroundColor = color;
      Console.Write(textInColor);
      Console.ResetColor();
      Console.Write(textAfter);
      Console.WriteLine();
    }
  }
}