namespace SubtitlesDownloader.App.Model;

class Attributes
{
  public string SubtitleId { get; set; }
  public string Language { get; set; }
  public int DownloadCount { get; set; }
  public int NewDownloadCount { get; set; }
  public bool HearingImpaired { get; set; }
  public bool Hd { get; set; }
  public double Fps { get; set; }
  public int Votes { get; set; }
  public double Ratings { get; set; }
  public bool FromTrusted { get; set; }
  public bool ForeignPartsOnly { get; set; }
  public DateTime UploadDate { get; set; }
  public bool AiTranslated { get; set; }
  public bool MachineTranslated { get; set; }
  public string Release { get; set; }
  public string Comments { get; set; }
  public int LegacySubtitleId { get; set; }
  public Uploader Uploader { get; set; }
  public FeatureDetails FeatureDetails { get; set; }
  public string Url { get; set; }
  public List<RelatedLink> RelatedLinks { get; set; }
  public List<File> Files { get; set; }
  public bool MoviehashMatch { get; set; }
}

class Datum
{
  public string Id { get; set; }
  public string Type { get; set; }
  public Attributes Attributes { get; set; }
}

class FeatureDetails
{
  public int FeatureId { get; set; }
  public string FeatureType { get; set; }
  public int Year { get; set; }
  public string Title { get; set; }
  public string MovieName { get; set; }
  public int ImdbId { get; set; }
  public int TmdbId { get; set; }
  public int SeasonNumber { get; set; }
  public int EpisodeNumber { get; set; }
  public int ParentImdbId { get; set; }
  public string ParentTitle { get; set; }
  public int ParentTmdbId { get; set; }
  public int ParentFeatureId { get; set; }
}

class File
{
  public int FileId { get; set; }
  public int CdNumber { get; set; }
  public string FileName { get; set; }
}

class RelatedLink
{
  public string Label { get; set; }
  public string Url { get; set; }
  public string ImgUrl { get; set; }
}

class SearchResponse
{
  public int TotalPages { get; set; }
  public int TotalCount { get; set; }
  public int PerPage { get; set; }
  public int Page { get; set; }
  public List<Datum> Data { get; set; }
}

class Uploader
{
  public int? UploaderId { get; set; }
  public string Name { get; set; }
  public string Rank { get; set; }
}

