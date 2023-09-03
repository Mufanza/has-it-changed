using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HasItChanged.Filesystem
{
    public class FileMetadata : IEquatable<FileMetadata>
    {
        public string Path { get; set; }
        public long ByteSize { get; set; }   
        public int FileHash { get; set; }

        public FileMetadata(string path, long byteSize, int fileHash)
        {
            this.Path = path;
            this.ByteSize = byteSize;
            this.FileHash = fileHash;
        }

        public static bool Equals(FileMetadata? lhs, FileMetadata? rhs)
        {
            if (lhs == null ^ rhs == null)
                return false;

            if (lhs == null)
                return true;

            return lhs.Equals(rhs);
        }

        public bool Equals(FileMetadata? other)
        {
            if (other == null)
                return false;

            if (!string.Equals(this.Path, other.Path))
                return false;
             if (this.ByteSize != other.ByteSize)
                return false;
             if (this.FileHash != other.FileHash)
                return false;
            
             return true;
        }

        public override int GetHashCode() => this.FileHash;
    }
}
