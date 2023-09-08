using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HasItChanged.Filesystem.Interfaces
{
    public interface IFileMetadataCreator
    {
        FileMetadata CreateFileMetadata(FileInfo file);
    }
}
