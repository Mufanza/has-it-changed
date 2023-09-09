using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HasItChanged.Filesystem
{
    public class FileMetadata : IEquatable<FileMetadata>
    {
        public long ByteSize { get; set; }   
        public string FileHash { get; set; }

        public FileMetadata(long byteSize, string fileHash)
        {
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

             if (this.ByteSize != other.ByteSize)
                return false;
             if (this.FileHash != other.FileHash)
                return false;
            
             return true;
        }

        public override int GetHashCode() => this.FileHash.GetHashCode();
        public override string ToString() => $"{{ Size: {this.ByteSize}, Hash: {this.FileHash} }}";
    }
}
