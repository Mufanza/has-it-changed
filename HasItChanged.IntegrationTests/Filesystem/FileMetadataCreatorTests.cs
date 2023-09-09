using HasItChanged.Configuration;
using HasItChanged.Filesystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HasItChanged.IntegrationTests.Filesystem
{
    [TestClass]
    public sealed class FileMetadataCreatorTests : FilesystemTests
    {
        [TestMethod]
        public void ReadFiles_creates_correct_distinct_hashes_for_Metadata()
        {
            // Arrange
            var target = new FileMetadataCreator();
             
            // Act
            var actualA = target.CreateFileMetadata(new FileInfo(this.filepathA));
            var actualB = target.CreateFileMetadata(new FileInfo(this.filepathB));
            var actualC = target.CreateFileMetadata(new FileInfo(this.subfilepathC));
            var actualD = target.CreateFileMetadata(new FileInfo(this.subfilepathD));

            Assert.IsFalse(string.IsNullOrWhiteSpace(actualA.FileHash));
            Assert.IsFalse(string.IsNullOrWhiteSpace(actualB.FileHash));
            Assert.IsFalse(string.IsNullOrWhiteSpace(actualC.FileHash));
            Assert.IsFalse(string.IsNullOrWhiteSpace(actualD.FileHash));
            Assert.AreEqual(4, new string[] { actualA.FileHash, actualB.FileHash, actualC.FileHash, actualD.FileHash }.Distinct().Count());
        }
    }
}
