

using HasItChanged.Configuration;

namespace HasItChanged.UnitTests.Configuration
{
    [TestClass]
    public class ConfigurationReaderTests
    {
        #pragma warning disable CS8618 // Non-nullable field must contain a value... This is always set in Setup
        private ConfigReader configReader;
        #pragma warning restore CS8618

        private string pathToTestConfig = ConfigReader.DefaultPathToConfig;

        [TestInitialize]
        public void Setup()
        {
            this.configReader = new ConfigReader();
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (!string.IsNullOrWhiteSpace(this.pathToTestConfig))
            if (File.Exists(this.pathToTestConfig))
                File.Delete(this.pathToTestConfig);
        }

        [TestMethod]
        public void Default_configuration_is_returned_when_supplied_file_doesnt_exist()
        {
            // Act
            var actual = this.configReader.ReadConfiguration("nonexistent_config.json");

            // Assert
            Assert.IsTrue(Config.DefaultConfiguration.Equals(actual));
        }

        [TestMethod]
        public void Configuration_json_can_be_parsed_correctly()
        {
            // Arrange
            var expected = new Config()
            {
                FileExtensions = new[] { ".cs" },
                Root = @"C:\Repo\MyProject"
            };

            var contents = "{ \"FileExtensions\": [ \".cs\"], \"Root\": \"C:\\\\Repo\\\\MyProject\" }";
            File.WriteAllText(this.pathToTestConfig, contents);

            // Assume
            File.Exists(this.pathToTestConfig);

            // Act
            var actual = this.configReader.ReadConfiguration(this.pathToTestConfig);

            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }
    }
}
