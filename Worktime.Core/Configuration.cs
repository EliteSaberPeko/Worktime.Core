using System.Text.Json;

namespace Worktime.Core
{
    public static class Configuration
    {
        private const string ConfigurationFileName = "config.json";
        private static bool IsConfigurationFileExist() => File.Exists(ConfigurationFileName);
        public static bool GetConnectionString(out string connectionString, out string error)
        {
            connectionString = string.Empty;
            error = string.Empty;
            if (!IsConfigurationFileExist())
            {
                error = $"Configuration file <{ConfigurationFileName}> is not exist!";
                return false;
            }
            using(var reader = new StreamReader(ConfigurationFileName))
            {
                string json = reader.ReadToEnd();
                if(string.IsNullOrWhiteSpace(json))
                {
                    error = $"Configuration file <{ConfigurationFileName}> is empty!";
                    return false;
                }
                try
                {
                    ConfigurationObject conf = JsonSerializer.Deserialize<ConfigurationObject>(json)!;
                    if(string.IsNullOrWhiteSpace(conf.ConnectionString))
                    {
                        error = $"ConnectionString is empty!";
                        return false;
                    }
                    connectionString = conf.ConnectionString;
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    return false;
                }
            }
            return true;
        }
    }
    public class ConfigurationObject
    {
        public string? ConnectionString { get; set; }
    }
}
