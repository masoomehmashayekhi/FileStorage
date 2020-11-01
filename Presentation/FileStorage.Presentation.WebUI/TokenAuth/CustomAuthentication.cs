using FileStorage.Layers.L03_Services.Contracts;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileStorage.Presentation.WebUI.TokenAuth
{
    public class CustomAuthentication : ICustomAuthentication
    {
        private readonly IFileStorageRepo _repository;

        public static Dictionary<string, string> clients ;
        public CustomAuthentication(IFileStorageRepo repo)
        {
            _repository = repo;
            if (clients == null)
            {
                var clientsres = _repository.GetClientEntity().Result;
                clients = new Dictionary<string, string>();
                clientsres.ForEach(x => clients.TryAdd(x.Token.ToString().ToLower(), x.Id.ToString()));
            }
            
        }
       
        public string Authenticate(string token)
        {
            var x = clients.FirstOrDefault(t => t.Key == token.ToString().ToLower());
            if (x.Key != null)
            {
                return x.Value;
            }
            else
                return null;
        }
    }
}
