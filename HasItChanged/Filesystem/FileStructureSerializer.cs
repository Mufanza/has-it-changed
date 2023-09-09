using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HasItChanged.Filesystem
{
    public static class FileStructureSerializer
    {
        public const string DefaultPastFileStructureFilename = "HasItChanged_FileStructure.json";

        public static void SaveFileStructure(FileStructure fileStructure, string filePath)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(fileStructure.GetEntireFileStructure, options);
            File.WriteAllText(filePath, jsonString);
        }

        public static FileStructure? ReadFileStructure(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            string jsonString = File.ReadAllText(filePath);
            try
            {
                var fileStructureDict = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, FileMetadata>>>(jsonString);
                if (fileStructureDict == null)
                    return null;
                
                return new FileStructure(fileStructureDict);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"There was a problem while deserializing previous file structure from '{filePath}': {ex.Message}");
                throw ex;
            }

        }
    }
}
