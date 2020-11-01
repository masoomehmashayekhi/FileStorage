using FileStorage.Layers.L03_Services.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace FileStorage.Presentation.WebUI.TokenAuth
{
    public class TokenAuthenticationHandler : AuthenticationHandler<TokenAuthenticationOptions>
    {
        private readonly ICustomAuthentication authentication;

        public IServiceProvider ServiceProvider { get; set; }
        

        public TokenAuthenticationHandler(IOptionsMonitor<TokenAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IServiceProvider serviceProvider
            ,ICustomAuthentication authentication)
            : base(options, logger, encoder, clock)
        {
            ServiceProvider = serviceProvider;
            this.authentication = authentication;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var headers = Request.Headers;
            if (!headers.ContainsKey("X-Auth-Token"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Token is not sent"));
            }
            var token = headers["X-Auth-Token"];

            if (string.IsNullOrEmpty(token))
            {
                return Task.FromResult(AuthenticateResult.Fail("Token is null"));
            }
            try
            {
                string authResult = authentication.Authenticate(token);
                if (string.IsNullOrEmpty(authResult))
                {
                    return Task.FromResult(AuthenticateResult.Fail($"Balancer not authorize token : for token={token}"));
                }

                var claims = new List<Claim> { new Claim("token", token), new Claim("clientid", authResult) };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var ticket = new AuthenticationTicket(new GenericPrincipal(identity, null), this.Scheme.Name);
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            catch(Exception ex)
            {
                return Task.FromResult(AuthenticateResult.Fail(ex.Message));
            }
        }
    }
}
