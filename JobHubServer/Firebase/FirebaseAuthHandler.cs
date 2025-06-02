using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace JobHubServer.Firebase
{
    internal class FirebaseAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public FirebaseAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string authorization = Request.Headers.Authorization.ToString();

            if (string.IsNullOrEmpty(authorization))
            {
                return AuthenticateResult.NoResult();
            }

            if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                string idToken = authorization["Bearer ".Length..].Trim();

                try
                {
                    FirebaseToken firebaseToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
                    ClaimsPrincipal principal = new();
                    principal.AddIdentity(new ClaimsIdentity(ToClaims(firebaseToken), "firebase"));
                    AuthenticationTicket ticket = new(principal, JwtBearerDefaults.AuthenticationScheme);
                    return AuthenticateResult.Success(ticket);
                }
                catch (Exception e)
                {
                    return AuthenticateResult.Fail(e);
                }
            }

            return AuthenticateResult.NoResult();
        }

        private static IEnumerable<Claim> ToClaims(FirebaseToken firebaseToken)
        {
            foreach (var item in firebaseToken.Claims)
            {
                yield return new Claim(item.Key, item.Value.ToString() ?? "");
            }
        }
    }
}