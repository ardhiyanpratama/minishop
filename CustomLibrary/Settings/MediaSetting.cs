using System.Collections.Generic;

namespace CustomLibrary.Settings
{
#nullable enable
    public class MediaSetting
    {
        public string? FilePath { get; set; }
        public string[]? PermittedExtensions { get; set; }
        public string[]? ThumbnailForExtensions { get; set; }
        public Dictionary<string, int>? FileSizeLimits { get; set; }
        public long? GlobalSizeLimits { get; set; }
        public MultipleUpload? MultipleUpload { get; set; }
        public Dictionary<string, string>? CustomPaths { get; set; }
    }

    public class MultipleUpload
    {
        public long SizeLimit { get; set; }
        public int CountLimit { get; set; }
    }
}
