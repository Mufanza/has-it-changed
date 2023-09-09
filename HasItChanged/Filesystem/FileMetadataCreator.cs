using HasItChanged.Filesystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HasItChanged.Filesystem
{
    public class FileMetadataCreator : IFileMetadataCreator
    {
        public FileMetadata CreateFileMetadata(FileInfo file)
        {
            var fileContents = File.ReadAllText(file.FullName, Encoding.UTF8);
            var fileHash = GetStableHashCode(fileContents);
            var metadata = new FileMetadata
            (
                byteSize: file.Length, 
                fileHash: fileHash
            );
            return metadata;
        }

        private string GetStableHashCode(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
