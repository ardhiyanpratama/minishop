using CustomLibrary.Helper;

namespace CustomLibrary.Exceptions
{
    public class FileExtensionsProhibitedException : FileUploadException
    {
        public FileExtensionsProhibitedException(string message = null) : base(message ?? ResponseMessageExtensions.File.ExtensionNotAllowed)
        {

        }
    }
}
