using CustomLibrary.Helper;

#nullable enable
namespace CustomLibrary.Exceptions
{
    public class FileSignatureErrorException : FileUploadException
    {
        public FileSignatureErrorException(string? message = null) : base(message ?? ResponseMessageExtensions.File.InvalidSignature)
        {

        }
    }
}
