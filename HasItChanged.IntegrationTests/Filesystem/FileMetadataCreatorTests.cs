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
    public sealed class FileMetadataCreatorTests : FilesystemTests
    {
        [TestMethod]
        public void ReadFiles_creates_correct_Metadata()
        {
            // Arrange
            var target = new FileMetadataCreator();
             
            // Act
            var actualA = target.CreateFileMetadata(new FileInfo(this.filepathA));
            var actualB = target.CreateFileMetadata(new FileInfo(this.filepathB));
            var actualC = target.CreateFileMetadata(new FileInfo(this.subfilepathC));
            var actualD = target.CreateFileMetadata(new FileInfo(this.subfilepathD));

            // Assert
            Assert.AreEqual(actualA.FileHash, FilesystemTests.fileContentsA.GetHashCode());
            Assert.AreEqual(actualB.FileHash, FilesystemTests.fileContentsB.GetHashCode());
            Assert.AreEqual(actualC.FileHash, FilesystemTests.subfileContentsC.GetHashCode());
            Assert.AreEqual(actualD.FileHash, FilesystemTests.subfileContentsD.GetHashCode());
        }
    }
}
