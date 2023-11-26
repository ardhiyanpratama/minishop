using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomLibrary.Helper
{
    public class ResponseBaseViewModel
    {
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }
    }
}
