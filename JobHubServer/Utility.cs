using static Google.Rpc.Context.AttributeContext.Types;

namespace JobHubServer
{
    public static class Utility
    {
        public static void VerifyUserRequest(HttpRequest request, out string? userId, out string? token)
        {
            userId = request.Headers["user_id"];
            token = request.Headers.Authorization.ToString()["Bearer ".Length..];
        }
    }
}
