using ASP_Meeting_2.Extensions;
using ASP_Meeting_2.MIddleware;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
string userSecret = "Qwerty1234";

//app.MapGet("/", () => "Hello World!");
//Inline-метод вбудовування компонентів Middleware
//Обробка помилок
app.Use(async (context, next) =>
{
    await next();
    switch (context.Response.StatusCode)
    {
        case 401:
            await context.Response.WriteAsync("Вам потрібно зареєструватися!");
            break;
        case 404:
            await context.Response.WriteAsync("Сторінку не знайдено!");
            break;
    }
});
//app.Use(UserAuthorization2);
//app.UseMiddleware<MyAuthorizationMiddleware>(userSecret);
app.UseDefaultFiles();
app.UseStaticFiles();

//app.UseWelcomePage();
//    .UseCors()
//    .UseAuthentication()
//    .UseAuthorization()
app.UseMyAuthorization(userSecret);
app.Run(async context =>
{
    string path = context.Request.Path;
    context.Response.ContentType = "text/plain;charset=utf-8";
    if (path == "/home")
    {
        context.Response.ContentType = "text/html;charset=utf-8";
        var fileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
        IFileInfo fileInfo = fileProvider.GetFileInfo("/html/home.html");
        //new PhysicalFileProvider();

        //await context.Response.SendFileAsync(fileInfo);
        await context.Response.SendFileAsync("html/home.html");
    }
    else
    if (path == "/about")
        await context.Response.WriteAsync("Про нашу компанію");
    else
        if (path == "/joinus")
        await context.Response.WriteAsync("Перелік наших вакансій");
    else
        if (path == "/services")
        await context.Response.WriteAsync("Наші послуги");
    else
    {
        context.Response.StatusCode = 404;
        //await context.Response.WriteAsync("Сторінку не знайдено!");
    }
});

app.Run();


async Task UserAuthorization (HttpContext context, RequestDelegate next) {
    //авторизація
    string? token = context.Request.Query["token"];
    if (string.IsNullOrWhiteSpace(token) || token != userSecret)
    {
        context.Response.StatusCode = 401;
        //await context.Response.WriteAsync("Вам потрібно зареєструватися!");
    }
    else

        await next(context);
}


async Task UserAuthorization2(HttpContext context, Func<Task> next)
{
    //авторизація
    string? token = context.Request.Query["token"];
    if (string.IsNullOrWhiteSpace(token) || token != userSecret)
    {
        context.Response.StatusCode = 401;
        //await context.Response.WriteAsync("Вам потрібно зареєструватися!");
    }
    else

        await next();
}
