using CustomLibrary.Helper;
using System;

namespace CustomLibrary.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base(ResponseMessageExtensions.Database.DataNotFound)
        {

        }

        public NotFoundException(string dataName) : base($"{dataName} Tidak Ditemukan")
        {

        }
    }
}
