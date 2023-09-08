using HasItChanged.Configuration;

namespace HasItChanged.UnitTests.Configuration
{
    [TestClass]
    public class ConfigurationReaderTests
    {
        private string pathToTestConfig = ConfigReader.DefaultPathToConfig;

        [TestCleanup]
        public void Cleanup()
        {
            if (!string.IsNullOrWhiteSpace(this.pathToTestConfig))
                File.Delete(this.pathToTestConfig);
        }

        [TestMethod]
        public void Default_configuration_is_returned_when_supplied_file_doesnt_exist()
        {
            // Act
            var actual = ConfigReader.ReadConfiguration("nonexistent_config.json");

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
            Assert.IsTrue(File.Exists(this.pathToTestConfig));

            // Act
            var actual = ConfigReader.ReadConfiguration(this.pathToTestConfig);

            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }
    }
}
