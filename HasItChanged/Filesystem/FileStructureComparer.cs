using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HasItChanged.Filesystem
{
    public static class FileStructureComparer
    {
        public static bool AreFileStructuresEqual(Dictionary<string, FileMetadata[]>? lhs, Dictionary<string, FileMetadata[]>? rhs)
        {
            if (lhs == null ^ rhs == null)
                return false;

            if (lhs == null || rhs == null)
                return true;

            if (lhs.Count != rhs.Count)
                return false;

            foreach(var key in lhs.Keys)
            {
                if (!rhs.ContainsKey(key))
                    return false;

                if (lhs[key].Length != rhs[key].Length)
                    return false;

                for (int i = 0; i < lhs[key].Length; i++)
                if (!FileMetadata.Equals(lhs[key][i], rhs[key][i]))
                    return false;
            }

            return true;
        }

        public static void Diff(Dictionary<string, FileMetadata[]>? previous, Dictionary<string, FileMetadata[]>? current, TextWriter? logger)
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

#pragma warning disable CS8602 // Dereference of a possibly null reference.

            // Are there any new folders?
            var newFolders = current.Keys
                .Where(k => !previous.ContainsKey(k))
                .ToArray();
            if (newFolders.Length > 0)
            {
                logger?.WriteLine("New folders:");
                foreach(var folder in newFolders)
                    logger?.WriteLine("\n" + folder);
                logger?.WriteLine();
            }

            // Were there any deleted folders?
            var deletedFolders = previous.Keys
                .Where(k => !current.ContainsKey(k))
                .ToArray();
            if (deletedFolders.Length > 0)
            {
                logger?.WriteLine("Deleted folders:");
                foreach(var folder in deletedFolders)
                    logger?.WriteLine("\n" + folder);
                logger?.WriteLine();
            }

            // Enough about folders; now compare the files...
            var comparableFolders = current.Keys
                .Where(k => previous.ContainsKey(k))
                .ToArray();

            if (comparableFolders.Length == 0)// no files to compare (all changes were done to folders)
                return;

            bool beginChangedFilesSection = true;
            foreach(var folder in comparableFolders)
            {
                var newFiles = current[folder]
                    .Where(currentMetaData => !previous[folder].Any(previousMetaData => previousMetaData.Path == currentMetaData.Path))
                    .ToArray();
                var deletedFiles = current[folder]
                    .Where(currentMetaData => !previous[folder].Any(previousMetaData => previousMetaData.Path == currentMetaData.Path))
                    .ToArray();
                
                var comparableFiles_current = current[folder]
                    .Where(currentMetaData => previous[folder].Any(previousMetaData => previousMetaData.Path == currentMetaData.Path))
                    .ToArray();
                var comparableFiles_previous = previous[folder]
                    .Where(previousMetaData => current[folder].Any(currentMetaData => currentMetaData.Path == previousMetaData.Path))
                    .ToArray();

                List<FileMetadata> changedFiles = new List<FileMetadata>();
                foreach(var currentFile in comparableFiles_current) {
                    var matchingPreviousFile = comparableFiles_previous.Single(pf => pf.Path == currentFile.Path);
                    if (!FileMetadata.Equals(currentFile, matchingPreviousFile))
                        changedFiles.Add(currentFile);
                }

                if (newFiles.Length == 0 && deletedFiles.Length == 0 && comparableFiles_current.Length == 0)
                    continue;

                if (beginChangedFilesSection)
                {
                    beginChangedFilesSection = false;
                    logger?.WriteLine("Changes in folders:");
                    logger?.WriteLine();
                }

                if (newFiles.Length > 0)
                {    
                    logger?.WriteLine($"\tNew files in '{folder}':");
                    foreach(var newFile in newFiles)
                        logger?.WriteLine($"\t\t{newFile.Path.Remove(0, folder.Length)}:");
                    logger?.WriteLine();
                }

                if (deletedFiles.Length > 0)
                {    
                    logger?.WriteLine($"\tFiles deleted from '{folder}':");
                    foreach(var deletedFile in deletedFiles)
                        logger?.WriteLine($"\t\t{deletedFile.Path.Remove(0, folder.Length)}:");
                    logger?.WriteLine();
                }

                if (changedFiles.Count > 0)
                {    
                    logger?.WriteLine($"\tModified files in '{folder}':");
                    foreach(var modifiedFile in changedFiles)
                        logger?.WriteLine($"\t\t{modifiedFile.Path.Remove(0, folder.Length)}:");
                    logger?.WriteLine();
                }

                logger?.WriteLine();
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }
}
