namespace QBCustomer.Utils
{
    public class AppConfig
    {
        private static IConfigurationRoot _appSettings = null!;
        public static void Configure()
        {
            _appSettings = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }

        public static IConfigurationRoot AppSettings => _appSettings;
        public static string? ConnectionStrings => _appSettings.GetConnectionString("SmartBooks");
    }
}
