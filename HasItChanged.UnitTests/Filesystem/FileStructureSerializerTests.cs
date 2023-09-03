using HasItChanged.Filesystem;

namespace HasItChanged.UnitTests.Filesystem
{
    [TestClass]
    public class FileStructureSerializerTests
    {
        private string pathToTestFileStructureFile = FileStructureSerializer.DefaultPathToFileStructure;

        [TestCleanup]
        public void Cleanup()
        {
            if (!string.IsNullOrWhiteSpace(this.pathToTestFileStructureFile))
                File.Delete(this.pathToTestFileStructureFile);
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
            FileStructureSerializer.SaveFileStructure(initialFileStructure);
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
