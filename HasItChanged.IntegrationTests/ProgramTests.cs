using HasItChanged.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HasItChanged.IntegrationTests
{
    [TestClass]
    public class ProgramTests
    {
        private string rootFolder => Path.Combine(Environment.CurrentDirectory, "TestRun");
        private string subFolder => Path.Combine(rootFolder, "SubFolder");
        private string pathToPastData => Path.Combine(rootFolder, "HasItChanged_FileStructure.json");

        [TestInitialize]
        public void Setup()
        {
            Directory.CreateDirectory(rootFolder);
            Directory.CreateDirectory(subFolder);
            File.WriteAllText(Path.Combine(rootFolder, "SomeFile.txt"), "Some text in the root file");
            File.WriteAllText(Path.Combine(subFolder, "SomeSubFile.txt"), "Some text in the subfile");
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "HasItChanged_Config.json"),
                "{" +
                "\t\"FileExtensions\": [\".txt\"]," +
                $"\t\"Root\": \"{rootFolder.Replace(@"\", @"\\")}\"" +
                "}"
            );
        }

        [TestCleanup]
        public void Cleanup()
        {
            File.Delete(pathToPastData);
        }

        [TestMethod]
        public async Task Main_exits_with_1_if_no_past_data_was_found()
        {
            // Assume
            Assert.IsFalse(File.Exists(pathToPastData));

            // Act
            var exitCode = await Program.Main(new string[] { "-s" });

            // Assert
            Assert.AreEqual(1, exitCode);
        }

        [TestMethod]
        public async Task Past_data_file_gets_created_after_first_run()
        {
            // Assume
            Assert.IsFalse(File.Exists(pathToPastData));

            // Act
            await Program.Main(new string[] { "-s" });

            // Assert
            Assert.IsTrue(File.Exists(pathToPastData));
        }

        [TestMethod]
        public async Task Subsequent_run_returns_0_after_nothing_changes()
        {
            // Assume
            Assert.IsFalse(File.Exists(pathToPastData));

            // Act
            var firstRunExitCode  = await Program.Main(new string[] { "-s" });
            var secondRunExitCode = await Program.Main(new string[] { "-s" });

            // Assert
            Assert.AreEqual(1, firstRunExitCode);
            Assert.AreEqual(0, secondRunExitCode);
        }

        [TestMethod]
        public async Task Subsequent_run_returns_1_if_something_changes()
        {
            // Assume
            Assert.IsFalse(File.Exists(pathToPastData));

            // Act
            var firstRunExitCode  = await Program.Main(new string[] { "-s" });
            File.WriteAllText(Path.Combine(rootFolder, "SomeFile.txt"), "This text has changed during the test run");
            var secondRunExitCode = await Program.Main(new string[] { "-s" });

            // Assert
            Assert.AreEqual(1, firstRunExitCode);
            Assert.AreEqual(1, secondRunExitCode);
        }
    }
}
