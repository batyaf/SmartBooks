namespace SBClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.UseStaticFiles();

            
            app.MapGet("/", () => Results.Redirect("/page/Authentication.html"));

            app.Run();
        }
    }
}
