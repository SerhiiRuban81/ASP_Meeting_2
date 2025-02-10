namespace ASP_Meeting_2.MIddleware
{
    public class MyAuthorizationMiddleware
    {
        private readonly RequestDelegate next;
        private readonly string userSecret;

        public MyAuthorizationMiddleware(RequestDelegate next, string userSecret)
        {
            this.next = next;
            this.userSecret = userSecret;
        }

        //public void Invoke(HttpContext context) { }

        public async Task InvokeAsync(HttpContext context)
        {
            string? token = context.Request.Query["token"];
            if (string.IsNullOrWhiteSpace(token) || token != userSecret)
            {
                context.Response.StatusCode = 401;
                //await context.Response.WriteAsync("Вам потрібно зареєструватися!");
            }
            else

                await next(context);
        }
    }
}
