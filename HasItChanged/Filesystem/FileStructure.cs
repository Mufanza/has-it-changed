using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HasItChanged.Filesystem
{
    public class FileStructure : IEquatable<FileStructure>
    {
        private Dictionary<string, Dictionary<string, FileMetadata>> fileStructure;

        public static FileStructure Empty => new FileStructure(new Dictionary<string, Dictionary<string, FileMetadata>>());

        public FileStructure(Dictionary<string, Dictionary<string, FileMetadata>> fileStructure)
        {
            this.fileStructure = fileStructure;
        }

        public string[] GetAllFolders() => fileStructure.Keys.ToArray();
        public IReadOnlyDictionary<string, FileMetadata> GetAllFilesInFolder(string path) =>
            fileStructure[path];

        /// <summary>
        /// Returns the underlying dictionary of dictionaries
        /// </summary>
        public IReadOnlyDictionary<string, Dictionary<string, FileMetadata>> GetEntireFileStructure => fileStructure;

        public override int GetHashCode() => this.GetAllFolders().GetHashCode();
        public bool Equals(FileStructure? other) => Equals(this, other);
        public static bool Equals(FileStructure? lhs, FileStructure? rhs)
        {
            if (lhs == null ^ rhs == null)
                return false;

            if (lhs == null || rhs == null)
                return true;

            if (lhs.GetAllFolders().Length != rhs.GetAllFolders().Length)
                return false;

            // Are there any folders in lhs that aren't in rhs?
            if (lhs.GetAllFolders().Any(lf => !rhs.GetAllFolders().Any(rf => lf == rf)))
                return false;

            // Are there any folders in rhs that aren't in lhs?
            if (rhs.GetAllFolders().Any(rf => !lhs.GetAllFolders().Any(lf => lf == rf)))
                return false;

            foreach(var folder in lhs.GetAllFolders())
            {
                var lhsFiles = lhs.GetAllFilesInFolder(folder);
                var rhsFiles = rhs.GetAllFilesInFolder(folder);
                var lhsFilenames = lhsFiles.Keys.ToArray();
                var rhsFilenames = rhsFiles.Keys.ToArray();

                if (lhsFilenames.Length != rhsFilenames.Length)
                    return false;

                // Are there any files in lhs that aren't in rhs?
                if (lhsFilenames.Any(lf => !rhsFilenames.Any(rf => lf == rf)))
                    return false;

                // Are there any folders in rhs that aren't in lhs?
                if (rhsFilenames.Any(rf => !lhsFilenames.Any(lf => lf == rf)))
                    return false;

                foreach(var filename in lhsFilenames)
                    if (!FileMetadata.Equals(lhsFiles[filename], rhsFiles[filename]))
                        return false;
            }

            return true;
        }

        public static void Diff(FileStructure? previous, FileStructure? current)
        {
            var stringBuilder = new StringBuilder();
            var customWriter = new StringWriter(stringBuilder);
            Diff(previous, current, customWriter);
            var text = stringBuilder.ToString();
        }

        /// <summary>
        /// PrettyPrints the differences between two file structures into the standard output
        /// Doesn't change any state, just logs stuff for debugging purposes.
        /// </summary>
        public static void Diff(FileStructure? previous, FileStructure? current, TextWriter? logger)
        {
            if (previous == null && current == null)
            {
                logger?.WriteLine("Neither of the compared file structures exist (so technically there are no changes)");
                return;
            }

            if (previous == null && current != null)
            {
                logger?.WriteLine("Data about previous file structure doesn't exist");
                return;
            }
            else if (previous != null && current == null)
            {
                logger?.WriteLine("Data about current file structure doesn't exist (or wasn't supplied properly)");
                return;
            }

            if (current == null || previous == null)
                throw new ApplicationException("This case can never happen - the compiler is just too stupid to realize that and this supresses the warnings");

            // Are there any new folders?
            var newFolders = current.GetAllFolders()
                .Where(k => !previous.GetAllFolders().Contains(k))
                .ToArray();
            if (newFolders.Length > 0)
            {
                logger?.WriteLine("New folders:");
                foreach(var folder in newFolders)
                    logger?.WriteLine("\n" + folder);
                logger?.WriteLine();
            }

            // Were there any deleted folders?
            var deletedFolders = previous.GetAllFolders()
                .Where(k => !current.GetAllFolders().Contains(k))
                .ToArray();
            if (deletedFolders.Length > 0)
            {
                logger?.WriteLine("Deleted folders:");
                foreach(var folder in deletedFolders)
                    logger?.WriteLine("\n" + folder);
                logger?.WriteLine();
            }

            // Enough about folders; now compare the files...
            var comparableFolders = current.GetAllFolders()
                .Where(k => previous.GetAllFolders().Contains(k))
                .ToArray();

            if (comparableFolders.Length == 0)// no files to compare (all changes were done to folders)
                return;

            foreach(var folder in comparableFolders)
            {
                var newFiles = current.GetAllFilesInFolder(folder).Keys
                    .Where(currentFilename => !previous.GetAllFilesInFolder(folder).Keys.Any(previousFilename => previousFilename == currentFilename))
                    .ToArray();
                var deletedFiles = previous.GetAllFilesInFolder(folder).Keys
                    .Where(previousFilename => !current.GetAllFilesInFolder(folder).Keys.Any(currentFilename => currentFilename == previousFilename))
                    .ToArray();
                
                var comparableFilenames = current.GetAllFilesInFolder(folder).Keys
                    .Where(currentFilename => previous.GetAllFilesInFolder(folder).Keys.Any(previousFilename => previousFilename == currentFilename))
                    .ToArray();

                var changedFiles = new List<Tuple<string, FileMetadata, FileMetadata>>();
                foreach(var filename in comparableFilenames)
                {
                    var currentFile = current.GetAllFilesInFolder(folder)[filename];
                    var previousFile = previous.GetAllFilesInFolder(folder)[filename];
                    if (!FileMetadata.Equals(currentFile, previousFile))
                        changedFiles.Add(new Tuple<string, FileMetadata, FileMetadata>(filename, currentFile, previousFile));
                }

                if (newFiles.Length == 0 && deletedFiles.Length == 0 && changedFiles.Count == 0)
                    continue;

                if (newFiles.Length > 0)
                {    
                    logger?.WriteLine($"\tNew files in '{folder}':");
                    foreach(var newFile in newFiles)
                        logger?.WriteLine($"\t\t{newFile}:");
                    logger?.WriteLine();
                }

                if (deletedFiles.Length > 0)
                {    
                    logger?.WriteLine($"\tFiles deleted from '{folder}':");
                    foreach(var deletedFile in deletedFiles)
                        logger?.WriteLine($"\t\t{deletedFile}:");
                    logger?.WriteLine();
                }

                if (changedFiles.Count > 0)
                {    
                    logger?.WriteLine($"\tModified files in '{folder}':");
                    foreach(var modifiedFiles in changedFiles)
                    {
                        if (modifiedFiles.Item2.ByteSize != modifiedFiles.Item3.ByteSize)
                        {
                            var currentSizeKb = (int)(modifiedFiles.Item2.ByteSize / 1000);
                            var previousSizeKb = (int)(modifiedFiles.Item3.ByteSize / 1000);
                            logger?.WriteLine($"\t\t{modifiedFiles.Item1}: size from {previousSizeKb} to {currentSizeKb}");
                            continue;
                        }
                        var currentHash = modifiedFiles.Item2.FileHash;
                        var previousHash = modifiedFiles.Item3.FileHash;
                        logger?.WriteLine($"\t\t{modifiedFiles.Item1}: hash from {currentHash} to {previousHash}");
                    }
                    logger?.WriteLine();
                }

                logger?.WriteLine();
            }
        }
    }
}
