using HasItChanged.Configuration;
using HasItChanged.Filesystem;

namespace HasItChanged.UnitTests.Filesystem
{
    [TestClass]
    public class FileStructureSerializerTests
    {
        private string pathToTestFileStructureFile = FileStructureSerializer.DefaultPastFileStructureFilename;

        [TestCleanup]
        public void Cleanup()
        {
            if (!string.IsNullOrWhiteSpace(this.pathToTestFileStructureFile))
                File.Delete(this.pathToTestFileStructureFile);
        }

        [TestMethod]
        public void Reading_FileStructure_from_an_unexisting_file_returns_empty_FileStructure()
        {
            // Arrange
            var emptyStructure = FileStructureSerializer.ReadFileStructure(@"C:\Path_to_nonexisting_file.json");

            // Assert
            Assert.IsNull(emptyStructure);
        }

        [TestMethod]
        public void FileStructure_can_be_serialized_and_deserialized()
        {
            // Arrange
            var initialFileStructureDict = new Dictionary<string, Dictionary<string, FileMetadata>>()
            {
                {
                    "pathToFolder_A",
                    new Dictionary<string, FileMetadata>()
                    {
                        { "fileA1.txt", new FileMetadata(100, "abc12345") },
                        { "fileA2.txt", new FileMetadata(200, "dfg56789") }
                    }
                },
                {
                    "pathToFolder_B",
                    new Dictionary<string, FileMetadata>()
                    {
                        { "fileB.txt", new FileMetadata(500, "hij420420") }
                    }
                }
            };
            var initialFileStructure = new FileStructure(initialFileStructureDict);


            // Act
            FileStructureSerializer.SaveFileStructure(initialFileStructure, this.pathToTestFileStructureFile);
            var deserializedFileStructure = FileStructureSerializer.ReadFileStructure(this.pathToTestFileStructureFile);

            // Assert
            Assert.IsNotNull(deserializedFileStructure);
            Assert.IsTrue(FileStructure.Equals(initialFileStructure, deserializedFileStructure));
        }
    }
}
