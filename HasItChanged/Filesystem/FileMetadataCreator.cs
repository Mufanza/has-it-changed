using HasItChanged.Filesystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HasItChanged.Filesystem
{
    public class FileMetadataCreator : IFileMetadataCreator
    {
        public FileMetadata CreateFileMetadata(FileInfo file)
        {
            var fileContents = File.ReadAllText(file.FullName, Encoding.UTF8);
            var fileHash = fileContents.GetHashCode();
            var metadata = new FileMetadata
            (
                byteSize: file.Length, 
                fileHash: fileHash
            );
            return metadata;
        }
    }
}
