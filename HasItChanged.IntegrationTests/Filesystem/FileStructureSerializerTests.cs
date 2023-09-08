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
            var initialFileStructure = new Dictionary<string, FileMetadata[]>()
            {
                {
                    "pathToFolder_A",
                    new FileMetadata[] 
                    {
                        new FileMetadata(@"pathToFolder_A\fileA1.txt", 100, 12345),
                        new FileMetadata(@"pathToFolder_A\fileA2.txt", 200, 56789)
                    }
                },
                {
                    "pathToFolder_B",
                    new FileMetadata[] 
                    {
                        new FileMetadata(@"pathToFolder_B\fileB.txt", 500, 420420)
                    }
                }
            };


            // Act
            FileStructureSerializer.SaveFileStructure(initialFileStructure, new Config().PathToPastDataFile);
            var deserializedFileStructure = FileStructureSerializer.ReadFileStructure(this.pathToTestFileStructureFile);

            // Assert
            Assert.IsNotNull(deserializedFileStructure);
            Assert.AreEqual(initialFileStructure.Keys.Count, deserializedFileStructure.Keys.Count);
            foreach(var key in initialFileStructure.Keys)
            {
                Assert.AreEqual(initialFileStructure[key].Length, deserializedFileStructure[key].Length);
                for(int i = 0; i < initialFileStructure[key].Length; i++)
                    Assert.IsTrue(initialFileStructure[key][i].Equals(deserializedFileStructure[key][i]));
            }
        }
    }
}
