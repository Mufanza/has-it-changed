using HasItChanged.Filesystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HasItChanged.Configuration
{
    public class Config : IEquatable<Config>
    {
        public const string ConfigFilename = "HasItChanged_Config.json";
        public static Config DefaultConfiguration => new Config();

        /// <summary>
        /// Specifies which file types should be considered by the checker.
        /// Files not mentioned here will not be checked for changes
        /// Leave this empty to check ALL files for changes
        /// </summary>
        public string[] FileExtensions { get; set; } = new string[0];
        
        /// <summary>
        /// Root of the folder where the checker should start checking for filechanges
        /// All subfolders of this folder will be searched as well
        /// If not filled, the CurrentDirectory will be set as root
        /// </summary>
        public string Root
        { 
            get => this._root ?? Environment.CurrentDirectory;
            set => this._root = value;
        }
        private string? _root = null;

        public string PathToPastDataFile {
            get => Path.Combine(this.Root, this._pastFileStructureFilename ?? FileStructureSerializer.DefaultPastFileStructureFilename);
            set => this._pastFileStructureFilename = value;
        }
        private string? _pastFileStructureFilename = null;

        public bool Equals(Config? other)
        {
            if (other == null)
                return false;

            if (!string.Equals(this.Root, other.Root))
                return false;

            if (this.FileExtensions == null)
            {
                if (other.FileExtensions != null)
                    return false;
            }
            else
            {
                if (other.FileExtensions == null)
                    return false;
                if (!Enumerable.SequenceEqual(this.FileExtensions, other.FileExtensions))
                    return false;
            }

            return true;
        }

        public string PrettyPrint()
        {
            var sb = new StringBuilder();

            sb.AppendLine("---------");
            sb.AppendLine($"root folder: {this.Root}");
            if (this.FileExtensions == null || this.FileExtensions.Length == 0)
                sb.AppendLine("file types: all");
            else
                sb.AppendLine($"file types: {this.FileExtensions.Aggregate((p,n) => p + ", " + n)}");

            sb.AppendLine($"past data file: {this.PathToPastDataFile}");
            sb.AppendLine("---------");
            return sb.ToString();
        }
    }
}
