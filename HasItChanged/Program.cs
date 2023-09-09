using HasItChanged.Configuration;
using HasItChanged.Filesystem;
using System.Text;

public class Program
{
    /// <returns>
    /// 1 if there were any changes
    /// 0 if there were no changes
    /// -1 if something went wrong
    /// </returns>
    public static async Task<int> Main(string[] args)
    {
        var logger = IsSilent(args) ? null: Console.Out;

        try
        {
            // Read the configuration
            var config = ConfigReader.ReadConfiguration();
            logger?.WriteLine("running the has-it-changed utility tool");
            logger?.Write(config.PrettyPrint());

            // Try read previous file structure
            var previousFileStructure = FileStructureSerializer.ReadFileStructure(config.PathToPastDataFile);
            var currentFileStructure = await new FileStructureMapper(new FileMetadataCreator(), config).MapFileStructure();
            FileStructureSerializer.SaveFileStructure(currentFileStructure, config.PathToPastDataFile);

            if (previousFileStructure == null)
            {
                logger?.WriteLine("Data about previous file changes were not found: all files are considered to be new (changed)");
                return 1;
            }

            if (FileStructure.Equals(previousFileStructure, currentFileStructure))
            {
                logger?.WriteLine("No changes were detected in the specified files.");
                return 0;
            }
            else
            {
                logger?.WriteLine("Some changes were detected:");
                if (ShouldDiffBeDisplayed(args))
                {
                    logger?.WriteLine();
                    FileStructure.Diff(previousFileStructure, currentFileStructure, logger);
                }
                return 1;
            }
        }
        catch (Exception ex)
        {
            logger?.WriteLine($"An error has occured while comparing the changes: {ex}");
            return -1;
        }
    }

    private static bool IsSilent(string[] args) => args.Any(a => a == "-s" || a == "-silent");
    private static bool ShouldDiffBeDisplayed(string[] args) => args.Any(a => a == "-d" || a == "-diff");
}