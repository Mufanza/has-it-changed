using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HasItChanged.UnitTests
{
    [TestClass]
    public sealed class ParsedArgsTests
    {
        [TestMethod]
        [DataRow("-s")]
        [DataRow("-silent")]
        public void IsSilent_argument_gets_parsed_correctly(string input)
        {
            // Arrange
            var args = new string[] { input };

            // Act
            var parsedArgs = new ParsedArgs(args);

            // Assert
            Assert.IsTrue(parsedArgs.IsSilent);
        }

        [TestMethod]
        [DataRow("-d")]
        [DataRow("-diff")]
        public void ShouldDiffBeDisplayed_argument_gets_parsed_correctly(string input)
        {
            // Arrange
            var args = new string[] { input };

            // Act
            var parsedArgs = new ParsedArgs(args);

            // Assert
            Assert.IsTrue(parsedArgs.ShouldDiffBeDisplayed);
        }

        [TestMethod]
        public void IsSilent_is_false_by_default()
        {
            var parsedArgs = new ParsedArgs(new string[0]);
            Assert.IsFalse(parsedArgs.IsSilent);
        }

        [TestMethod]
        public void ShouldDiffBeDisplayed_is_false_by_default()
        {
            var parsedArgs = new ParsedArgs(new string[0]);
            Assert.IsFalse(parsedArgs.ShouldDiffBeDisplayed);
        }

        [TestMethod]
        [DataRow(new string[] { "-c" }, DisplayName = "Missing value")]
        [DataRow(new string[] { "-c", "C:\\PathToNonexistingFile" }, DisplayName ="Path to nonexisting file")]
        public void PathToConfig_is_null_if_arguments_arent_passed_properly(string[] args)
        {
            // Act
            var parsedArgs = new ParsedArgs(args);

            // Assert
            Assert.IsNull(parsedArgs.PathToConfigFile);
        }
    }
}
