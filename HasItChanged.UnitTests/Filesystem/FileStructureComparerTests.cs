using HasItChanged.Filesystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HasItChanged.UnitTests.Filesystem
{
    [TestClass]
    public sealed class FileStructureComparerTests
    {
        [TestMethod]
        public void AreFileStructuresEqual_returns_true_if_both_inputs_are_null()
        {
            // Arrange
            Dictionary<string, FileMetadata[]>? lhs = null;
            Dictionary<string, FileMetadata[]>? rhs = null;

            // Act
            var result = FileStructureComparer.AreFileStructuresEqual(lhs, rhs);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AreFileStructuresEqual_returns_false_if_one_input_is_null()
        {
            // Arrange
            Dictionary<string, FileMetadata[]>? lhs = null;
            Dictionary<string, FileMetadata[]>? rhs = new Dictionary<string, FileMetadata[]>();

            // Act
            var result = FileStructureComparer.AreFileStructuresEqual(lhs, rhs);
            var resultFlipped = FileStructureComparer.AreFileStructuresEqual(rhs, lhs);// flip left and right to make sure the order doesn't matter

            // Assert
            Assert.IsFalse(result);
            Assert.IsFalse(resultFlipped);
        }

        [TestMethod]
        public void AreFileStructuresEqual_returns_false_if_the_inputs_have_different_hash()
        {
            // Arrange
            Dictionary<string, FileMetadata[]> lhs = new Dictionary<string, FileMetadata[]>
            {
                {
                    "Folder1", new FileMetadata[]
                    {
                        new FileMetadata("file1.txt", 100, 12345),
                        new FileMetadata("file2.txt", 200, 67890)
                    }
                }
            };

            Dictionary<string, FileMetadata[]> rhs = new Dictionary<string, FileMetadata[]>
            {
                {
                    "Folder1", new FileMetadata[]
                    {
                        new FileMetadata("file1.txt", 100, 54321),// Different FileHash
                        new FileMetadata("file2.txt", 200, 67890)
                    }
                }
            };

            // Act
            var result = FileStructureComparer.AreFileStructuresEqual(lhs, rhs);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AreFileStructuresEqual_returns_false_if_the_inputs_have_different_sizes()
        {
            // Arrange
            Dictionary<string, FileMetadata[]> lhs = new Dictionary<string, FileMetadata[]>
            {
                {
                    "Folder1", new FileMetadata[]
                    {
                        new FileMetadata("file1.txt", 420, 12345),// Different size
                        new FileMetadata("file2.txt", 200, 67890)
                    }
                }
            };

            Dictionary<string, FileMetadata[]> rhs = new Dictionary<string, FileMetadata[]>
            {
                {
                    "Folder1", new FileMetadata[]
                    {
                        new FileMetadata("file1.txt", 100, 12345),
                        new FileMetadata("file2.txt", 200, 67890)
                    }
                }
            };

            // Act
            var result = FileStructureComparer.AreFileStructuresEqual(lhs, rhs);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AreFileStructuresEqual_returns_false_if_the_inputs_have_different_amount_of_files()
        {
            // Arrange
            Dictionary<string, FileMetadata[]> lhs = new Dictionary<string, FileMetadata[]>
            {
                {
                    "Folder1", new FileMetadata[]
                    {
                        new FileMetadata("file1.txt", 420, 12345),
                        new FileMetadata("file2.txt", 200, 67890),
                        new FileMetadata("file3.txt", 300, 77777)
                    }
                }
            };

            Dictionary<string, FileMetadata[]> rhs = new Dictionary<string, FileMetadata[]>
            {
                {
                    "Folder1", new FileMetadata[]
                    {
                        new FileMetadata("file1.txt", 420, 12345),
                        new FileMetadata("file2.txt", 200, 67890),
                    }
                }
            };

            // Act
            var result = FileStructureComparer.AreFileStructuresEqual(lhs, rhs);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AreFileStructuresEqual_returns_false_if_the_inputs_have_different_amount_of_folders()
        {
            // Arrange
            Dictionary<string, FileMetadata[]> lhs = new Dictionary<string, FileMetadata[]>
            {
                {
                    "Folder1", new FileMetadata[]
                    {
                        new FileMetadata("file1.txt", 420, 12345),
                        new FileMetadata("file2.txt", 200, 67890)
                    }
                },
                {
                    "Folder2", new FileMetadata[]
                    {
                        new FileMetadata("file3.txt", 123, 12345)
                    }
                }
            };

            Dictionary<string, FileMetadata[]> rhs = new Dictionary<string, FileMetadata[]>
            {
                {
                    "Folder1", new FileMetadata[]
                    {
                        new FileMetadata("file1.txt", 420, 12345),
                        new FileMetadata("file2.txt", 200, 67890),
                    }
                }
            };

            // Act
            var result = FileStructureComparer.AreFileStructuresEqual(lhs, rhs);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AreFileStructuresEqual_returns_true_if_the_inputs_are_same()
        {
            // Arrange
            Dictionary<string, FileMetadata[]> lhs = new Dictionary<string, FileMetadata[]>
            {
                {
                    "Folder1", new FileMetadata[]
                    {
                        new FileMetadata("file1.txt", 100, 12345),
                        new FileMetadata("file2.txt", 200, 67890)
                    }
                }
            };

            Dictionary<string, FileMetadata[]> rhs = new Dictionary<string, FileMetadata[]>
            {
                {
                    "Folder1", new FileMetadata[]
                    {
                        new FileMetadata("file1.txt", 100, 12345),
                        new FileMetadata("file2.txt", 200, 67890)
                    }
                }
            };

            // Act
            var result = FileStructureComparer.AreFileStructuresEqual(lhs, rhs);

            // Assert
            Assert.IsTrue(result);
        }

    }
}
