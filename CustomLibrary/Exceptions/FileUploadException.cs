using CustomLibrary.Helper;
using System;

namespace CustomLibrary.Exceptions
{
    public class FileUploadException : Exception
    {
        public FileUploadException(string message = null) : base(message ?? ResponseMessageExtensions.File.DefaultError)
        {

        }
    }
}
