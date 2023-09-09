using HasItChanged.Filesystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HasItChanged.UnitTests.Filesystem
{
    [TestClass]
    public sealed class FileStructureTests
    {
        [TestMethod]
        public void AreFileStructuresEqual_returns_true_if_both_inputs_are_null()
        {
            // Arrange
            FileStructure? lhs = null;
            FileStructure? rhs = null;

            // Act
            var result = FileStructure.Equals(lhs, rhs);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AreFileStructuresEqual_returns_false_if_one_input_is_null()
        {
            // Arrange
            FileStructure? lhs = null;
            FileStructure? rhs = FileStructure.Empty;

            // Act
            var result = FileStructure.Equals(lhs, rhs);
            var resultFlipped = FileStructure.Equals(rhs, lhs);// flip left and right to make sure the order doesn't matter

            // Assert
            Assert.IsFalse(result);
            Assert.IsFalse(resultFlipped);
        }

        [TestMethod]
        public void AreFileStructuresEqual_returns_false_if_the_inputs_have_different_hash()
        {
            // Arrange
            var lhsDict = new Dictionary<string, Dictionary<string, FileMetadata>>
            {
                {
                    "Folder1", 
                    new Dictionary<string, FileMetadata>
                    {
                        { "file1.txt", new FileMetadata(100, "abc12345") },
                        { "file2.txt", new FileMetadata(200, "def67890") }
                    }
                }
            };
            var lhs = new FileStructure(lhsDict);
            var rhsDict = new Dictionary<string, Dictionary<string, FileMetadata>>
            {
                {
                    "Folder1", 
                    new Dictionary<string, FileMetadata>
                    {
                        { "file1.txt", new FileMetadata(100, "aaa11111") },// different filehash
                        { "file2.txt", new FileMetadata(200, "def67890") }
                    }
                }
            };
            var rhs = new FileStructure(rhsDict);

            // Act
            var result = FileStructure.Equals(lhs, rhs);
            var resultFlipped = FileStructure.Equals(rhs, lhs);// flip lhs with rhs to make sure the order doesn't matter

            // Assert
            Assert.IsFalse(result);
            Assert.IsFalse(resultFlipped);
        }

        [TestMethod]
        public void AreFileStructuresEqual_returns_false_if_the_inputs_have_different_sizes()
        {
            // Arrange
            var lhsDict = new Dictionary<string, Dictionary<string, FileMetadata>>
            {
                {
                    "Folder1", 
                    new Dictionary<string, FileMetadata>
                    {
                        { "file1.txt", new FileMetadata(100, "abc12345") },
                        { "file2.txt", new FileMetadata(200, "def67890") }
                    }
                }
            };
            var lhs = new FileStructure(lhsDict);
            var rhsDict = new Dictionary<string, Dictionary<string, FileMetadata>>
            {
                {
                    "Folder1", 
                    new Dictionary<string, FileMetadata>
                    {
                        { "file1.txt", new FileMetadata(100, "abc12345") },
                        { "file2.txt", new FileMetadata(420, "def67890") }// diferent size
                    }
                }
            };
            var rhs = new FileStructure(rhsDict);

            // Act
            var result = FileStructure.Equals(lhs, rhs);
            var resultFlipped = FileStructure.Equals(rhs, lhs);// flip lhs with rhs to make sure the order doesn't matter

            // Assert
            Assert.IsFalse(result);
            Assert.IsFalse(resultFlipped);
        }

        [TestMethod]
        public void AreFileStructuresEqual_returns_false_if_the_inputs_have_different_amount_of_files()
        {
            // Arrange
            var lhsDict = new Dictionary<string, Dictionary<string, FileMetadata>>
            {
                {
                    "Folder1", 
                    new Dictionary<string, FileMetadata>
                    {
                        { "file1.txt", new FileMetadata(100, "abc12345") },
                        { "file2.txt", new FileMetadata(200, "def67890") }
                    }
                }
            };
            var lhs = new FileStructure(lhsDict);
            var rhsDict = new Dictionary<string, Dictionary<string, FileMetadata>>
            {
                {
                    "Folder1", 
                    new Dictionary<string, FileMetadata>
                    {
                        { "file1.txt", new FileMetadata(100, "abc12345") },
                        { "file2.txt", new FileMetadata(200, "def67890") },
                        { "file3.txt", new FileMetadata(300, "xyz77777") }
                    }
                }
            };
            var rhs = new FileStructure(rhsDict);

            // Act
            var result = FileStructure.Equals(lhs, rhs);
            var resultFlipped = FileStructure.Equals(rhs, lhs);// flip lhs with rhs to make sure the order doesn't matter

            // Assert
            Assert.IsFalse(result);
            Assert.IsFalse(resultFlipped);
        }

        [TestMethod]
        public void AreFileStructuresEqual_returns_false_if_the_inputs_have_different_amount_of_folders()
        {
            // Arrange
            var lhsDict = new Dictionary<string, Dictionary<string, FileMetadata>>
            {
                {
                    "Folder1", 
                    new Dictionary<string, FileMetadata>
                    {
                        { "file1.txt", new FileMetadata(100, "abc12345") },
                        { "file2.txt", new FileMetadata(200, "def67890") }
                    }
                },
                {
                    "Folder2", 
                    new Dictionary<string, FileMetadata> { }
                }
            };
            var lhs = new FileStructure(lhsDict);
            var rhsDict = new Dictionary<string, Dictionary<string, FileMetadata>>
            {
                {
                    "Folder1", 
                    new Dictionary<string, FileMetadata>
                    {
                        { "file1.txt", new FileMetadata(100, "abc12345") },
                        { "file2.txt", new FileMetadata(200, "def67890") }
                    }
                }
            };
            var rhs = new FileStructure(rhsDict);

            // Act
            var result = FileStructure.Equals(lhs, rhs);
            var resultFlipped = FileStructure.Equals(rhs, lhs);// flip lhs with rhs to make sure the order doesn't matter

            // Assert
            Assert.IsFalse(result);
            Assert.IsFalse(resultFlipped);
        }

        [TestMethod]
        public void AreFileStructuresEqual_returns_true_if_the_inputs_are_same()
        {
            // Arrange
            var lhsDict = new Dictionary<string, Dictionary<string, FileMetadata>>
            {
                {
                    "Folder1", 
                    new Dictionary<string, FileMetadata>
                    {
                        { "file1.txt", new FileMetadata(100, "abc12345") },
                        { "file2.txt", new FileMetadata(200, "def67890") }
                    }
                }
            };
            var lhs = new FileStructure(lhsDict);
            var rhsDict = new Dictionary<string, Dictionary<string, FileMetadata>>
            {
                {
                    "Folder1", 
                    new Dictionary<string, FileMetadata>
                    {
                        { "file1.txt", new FileMetadata(100, "abc12345") },
                        { "file2.txt", new FileMetadata(200, "def67890") }
                    }
                }
            };
            var rhs = new FileStructure(rhsDict);

            // Act
            var result = FileStructure.Equals(lhs, rhs);
            var resultFlipped = FileStructure.Equals(rhs, lhs);

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(resultFlipped);
        }

    }
}
