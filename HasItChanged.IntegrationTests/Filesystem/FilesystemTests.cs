using HasItChanged.Configuration;
using HasItChanged.Filesystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HasItChanged.IntegrationTests.Filesystem
{
    [TestClass]
    public class FilesystemTests
    {
        #pragma warning disable CS8618 // Non-nullable field - will never be null as we set them all in the Setup
        protected Config config;

        protected string subfolderPath;

        protected string filepathA;
        protected string filepathB;
        protected string subfilepathC;
        protected string subfilepathD;

        protected const string fileContentsA    = "Initial texts of test file A";
        protected const string fileContentsB    = "Default words in test file B";
        protected const string subfileContentsC = "First sentencte in subfile C";
        protected const string subfileContentsD = "Only characters in subfile D";
        #pragma warning restore CS8618

        [TestInitialize]
        public void Setup()
        {
            // Create directory structure
            this.config = new Config()
            {
                Root = Path.Combine(Environment.CurrentDirectory, "TestFiles")
            };
            Directory.CreateDirectory(this.config.Root);
            this.subfolderPath = Path.Combine(this.config.Root, "subFolder");
            Directory.CreateDirectory(subfolderPath);

            // Create files
            this.filepathA = Path.Combine(this.config.Root, "TestFile_A.txt");
            this.filepathB = Path.Combine(this.config.Root, "TestFile_B.cs");
            this.subfilepathC = Path.Combine(subfolderPath, "TestFile_C.txt");
            this.subfilepathD = Path.Combine(subfolderPath, "TestFile_D.md");

            File.WriteAllText(filepathA, fileContentsA);
            File.WriteAllText(filepathB, fileContentsB);
            File.WriteAllText(subfilepathC, subfileContentsC);
            File.WriteAllText(subfilepathD, subfileContentsD);
        }
    }
}
