using HasItChanged.Configuration;
using HasItChanged.Filesystem.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HasItChanged.Filesystem
{
    public class FileStructureMapper
    {
        private readonly IFileMetadataCreator fileMetadataCreator;
        private readonly Config config;

        public FileStructureMapper(IFileMetadataCreator fileMetadataCreator, Config config)
        {
            this.fileMetadataCreator = fileMetadataCreator;
            this.config = config;
        }

        public async Task<FileStructure> MapFileStructure()
        {
            var rootDirectory = new DirectoryInfo(this.config.Root);

            var fileMap = new ConcurrentDictionary<string, ConcurrentDictionary<string, FileMetadata>>();
            await MapFilesAndSubDirectories(rootDirectory, fileMap);

            // Convert from ConcurrentDictionary to regular Dictionary
            var fileMapAsDict = fileMap.ToDictionary(kv => kv.Key, kv => kv.Value.ToDictionary(kvk => kvk.Key, kvk => kvk.Value));
            return new FileStructure(fileMapAsDict);
        }

        private async Task MapFilesAndSubDirectories(DirectoryInfo directory, ConcurrentDictionary<string, ConcurrentDictionary<string, FileMetadata>> map)
        {
            var subDirectories = directory.GetDirectories();

            var tasks = new Task[subDirectories.Length + 1];
            for (int i = 0; i < subDirectories.Length; i++)
                tasks[i] = MapFilesAndSubDirectories(subDirectories[i], map);
            tasks[subDirectories.Length] = Task.Run(() => MapFilesInDirectory(directory, map));

            await Task.WhenAll(tasks);
        }

        private void MapFilesInDirectory(DirectoryInfo directory, ConcurrentDictionary<string, ConcurrentDictionary<string, FileMetadata>> map)
        {
            var filesInDirectory = directory.GetFiles();

            if (this.config.FileExtensions.Length > 0)
                filesInDirectory = filesInDirectory
                    .Where(f => this.config.FileExtensions.Contains(f.Extension))
                    .ToArray();

            var resultMetadata = new ConcurrentDictionary<string, FileMetadata>();
            Parallel.ForEach(filesInDirectory, file => {
                
                if (file.FullName == config.PathToPastDataFile)
                    return;
                if (file.Name == Config.ConfigFilename)
                    return;

                var metadata = this.fileMetadataCreator.CreateFileMetadata(file);
                resultMetadata.TryAdd(file.Name, metadata);
            });

            if (!map.TryAdd(directory.FullName, resultMetadata))
                throw new ApplicationException($"Tried to map the {directory.FullName} directory more than once!");
        }
    }
}
