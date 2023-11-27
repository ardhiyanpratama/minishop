using CustomLibrary.Helper;

namespace CustomLibrary.Exceptions
{
    public class FileSizeOverLimitException : FileUploadException
    {
        public FileSizeOverLimitException(string message = null) : base(message ?? ResponseMessageExtensions.File.SizeOverLimit)
        {

        }
    }
}
