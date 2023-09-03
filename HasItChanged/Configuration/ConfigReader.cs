using System.Text.Json;

namespace HasItChanged.Configuration
{
    public static class ConfigReader
    {
        public static readonly string DefaultPathToConfig = Path.Combine(Environment.CurrentDirectory, "HasItChanged_Config.json");

        /// <summary>
        /// Reads the HasItChanged_Config.json file
        /// The file is expected to reside at the same location as where this app is ran from
        /// If not found, a default instance of Configuration will be returned.
        /// </summary>
        /// <returns>
        /// An instance of the Configuration class
        /// </returns>
        public static Config ReadConfiguration(string? pathToConfig = null)
        {
            if (string.IsNullOrWhiteSpace(pathToConfig))
                pathToConfig = DefaultPathToConfig;

            // If the file doesn't exist, return a default instance
            if (!File.Exists(pathToConfig))
                return Config.DefaultConfiguration;
            
            // Read the JSON content from the specified file
            var jsonContent = File.ReadAllText(pathToConfig);

            try
            {
                // Deserialize the JSON content into a Configuration object
                var configuration = JsonSerializer.Deserialize<Config>(jsonContent);

                if (configuration == null)
                {
                    Console.WriteLine($"Configuration file at '{pathToConfig}' could not be parsed");
                    return Config.DefaultConfiguration;
                }

                return configuration;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Configuration file at '{pathToConfig}' was not in a correct format! {ex.Message}");
                return Config.DefaultConfiguration;
            }
        }
    }
}
