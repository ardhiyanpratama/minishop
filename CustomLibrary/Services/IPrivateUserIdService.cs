using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomLibrary.Services
{
    public interface IPrivateUserIdService
    {
        Task<string> GetUserId();
    }
}
