using ASP_Meeting_2.MIddleware;

namespace ASP_Meeting_2.Extensions
{
    public static class MiddlewareExtentions
    {
        public static IApplicationBuilder UseMyAuthorization(this IApplicationBuilder app, string userSecret)
        {
            return app.UseMiddleware<MyAuthorizationMiddleware>(userSecret);
        }
    }
}
