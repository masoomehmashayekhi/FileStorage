using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileStorage.Presentation.WebUI.TokenAuth
{
    public interface ICustomAuthentication
    {
        public static IDictionary<string, string> clients { get; }
        public string Authenticate(string token);
    }
}
