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
    public sealed class FileStructureMapperTests : FilesystemTests
    {
        [TestMethod]
        public async Task ReadFiles_maps_filestructure_correctly()
        {
            // Arrange
            var target = new FileStructureMapper(new FileMetadataCreator(), this.config);

            // Act
            var fileStructure = await target.MapFileStructure();

            // Assert
            // We expect 2 folders - one for the Root folder and one for the subfolder
            Assert.AreEqual(2, fileStructure.Count);

            var rootFolder = fileStructure.SingleOrDefault(kv => kv.Key == new DirectoryInfo(this.config.Root).FullName);
            var subFolder = fileStructure.SingleOrDefault(kv => kv.Key == new DirectoryInfo(Path.Combine(this.config.Root, this.subfolderPath)).FullName);

            Assert.IsNotNull(rootFolder);
            Assert.IsNotNull(subFolder);
            Assert.AreEqual(2, rootFolder.Value.Length);// file A and file B
            Assert.AreEqual(2, subFolder.Value.Length);// subfile C and subfile D
        }

        [TestMethod]
        public async Task ReadFiles_maps_filestructure_correctly_while_ignoring_unspecified_filetypes()
        {
            // Arrange
            this.config.FileExtensions = new string[] { ".txt", ".cs" };
            var target = new FileStructureMapper(new FileMetadataCreator(), this.config);

            // Act
            var fileStructure = await target.MapFileStructure();

            // Assert
            // We expect 2 folders - one for the Root folder and one for the subfolder
            Assert.AreEqual(2, fileStructure.Count);

            var rootFolder = fileStructure.SingleOrDefault(kv => kv.Key == new DirectoryInfo(this.config.Root).FullName);
            var subFolder = fileStructure.SingleOrDefault(kv => kv.Key == new DirectoryInfo(Path.Combine(this.config.Root, this.subfolderPath)).FullName);

            Assert.IsNotNull(rootFolder);
            Assert.IsNotNull(subFolder);
            Assert.AreEqual(2, rootFolder.Value.Length);// file A and file B
            Assert.AreEqual(1, subFolder.Value.Length);// subfile C (file D is missing, as that one is neither .txt nor .cs)
        }
    }
}
