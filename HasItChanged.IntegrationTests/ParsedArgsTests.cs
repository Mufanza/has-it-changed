using HasItChanged.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HasItChanged.IntegrationTests
{
    [TestClass]
    public sealed class ParsedArgsTests
    {
        private string rootFolder => Path.Combine(Environment.CurrentDirectory, "TestRun");

        [TestInitialize]
        public void Setup()
        {
            Directory.CreateDirectory(rootFolder);
        }

        [TestMethod]
        [DataRow("-c")]
        [DataRow("-config")]
        public void PathToConfig_argument_gets_parsed_correctly(string input)
        {
            // Arrange
            var pathToConfigFile = Path.Combine(rootFolder, Config.ConfigFilename);
            File.WriteAllText(pathToConfigFile, "{ \"Root\": \"C:\\\\Repo\\\\MyTestProject\" }");
            var args = new string[] { input, pathToConfigFile };

            // Act
            var parsedArgs = new ParsedArgs(args);

            // Assert
            Assert.IsNotNull(parsedArgs.PathToConfigFile);
            Assert.AreEqual(pathToConfigFile, parsedArgs.PathToConfigFile);
        }

        [TestMethod]
        public void PathToCOnfig_is_null_when_config_file_is_named_incorrectly()
        {
            // Arrange
            var pathToConfigFile = Path.Combine(rootFolder, "WrongFilename.json");
            File.WriteAllText(pathToConfigFile, "{ \"Root\": \"C:\\\\Repo\\\\MyTestProject\" }");
            var args = new string[] { "-c", pathToConfigFile };

            // Act
            var parsedArgs = new ParsedArgs(args);

            // Assert
            Assert.IsNull(parsedArgs.PathToConfigFile);
        }
    }
}
