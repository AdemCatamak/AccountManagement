using AccountManagement.ConfigSection.ConfigModels;
using Microsoft.Extensions.Configuration;

namespace AccountManagement.ConfigSection
{
    public static class AppConfigs
    {
        public class ConfigKeys
        {
            public const string DbConfig = "DbConfig";
            public const string AppUrls = "AspNetCoreUrls";
            public const string RabbitMQConfig = "RabbitMQConfig";
        }

        private static IConfiguration _configuration;
        public static IConfiguration Configuration => _configuration ??= GetConfig();

        private static IConfiguration GetConfig()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            PrepareConfig(configurationBuilder);
            IConfigurationRoot configurationRoot = configurationBuilder.Build();
            return configurationRoot;
        }

        public static void PrepareConfig(IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.AddJsonFile("appsettings.json");
        }

        public static DbConfigModel GetDbConfigModel()
        {
            var dbConfigModel = Configuration.GetSection(ConfigKeys.DbConfig)
                                             .Get<DbConfigModel>();

            return dbConfigModel;
        }

        public static DbOption SelectedDbOption()
        {
            DbConfigModel dbConfigModel = GetDbConfigModel();

            DbOption dbOption = dbConfigModel.SelectedDbOption();

            return dbOption;
        }

        public static RabbitMQConfigModel GetRabbitMQConfigModel()
        {
            var rabbitMqConfigModel = Configuration.GetSection(ConfigKeys.RabbitMQConfig)
                                                   .Get<RabbitMQConfigModel>();

            return rabbitMqConfigModel;
        }

        public static string[] AppUrls()
        {
            string[] urls = Configuration.GetSection(ConfigKeys.AppUrls).Get<string[]>();
            return urls;
        }
    }
}