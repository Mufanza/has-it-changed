using HasItChanged.Configuration;
using HasItChanged.Filesystem;

class Program
{
    /// <returns>
    /// 1 if there were any changes
    /// 0 if there were no changes
    /// -1 if something went wrong
    /// </returns>
    static async Task<int> Main(string[] args)
    {
        try
        {
            // Read the configuration
            var config = ConfigReader.ReadConfiguration();

            // Try read previous file structure
            var previousFileStructure = FileStructureSerializer.ReadFileStructure();
            var currentFileStructure = await new FileStructureMapper(new FileMetadataCreator(), config).MapFileStructure();
            FileStructureSerializer.SaveFileStructure(currentFileStructure);

            if (FileStructureComparer.AreFileStructuresEqual(previousFileStructure, currentFileStructure))
                return 0;
            else
                return 1;
        }
        catch (Exception)
        {
            return -1;
        }
    }
}