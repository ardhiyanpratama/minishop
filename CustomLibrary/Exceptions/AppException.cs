using System;

namespace CustomLibrary.Exceptions
{
    public class AppException : Exception
    {

        public AppException(string? message, string? note = null, long? data = null) : base(message)
        {
            ErrorNote = note;
            ErrorData = data;
        }

        public string? ErrorNote { get; private set; }
        public long? ErrorData { get; private set; }
    }
}
