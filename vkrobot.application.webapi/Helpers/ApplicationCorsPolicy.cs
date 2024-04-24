using Microsoft.AspNetCore.Cors.Infrastructure;

namespace vkrobot.application.webapi.Helpers
{
    internal sealed class ApplicationCorsPolicy : CorsPolicy
    {
        public ApplicationCorsPolicy()
        {
            Origins.Clear();
            IsOriginAllowed = origin => true;
            Headers.Clear();
            Headers.Add("*");

            Methods.Clear();
            Methods.Add("*");

            SupportsCredentials = true;
        }
    }
}
