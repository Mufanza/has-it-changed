using HasItChanged.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HasItChanged
{
    /// <summary>
    /// Reads and parses the args string array (that was supplied to the Main method)
    /// </summary>
    public class ParsedArgs
    {
        public bool IsSilent { get; private set; }
        public bool ShouldDiffBeDisplayed { get; private set; }
        public string? PathToConfigFile { get; private set; }

        private StringBuilder parsedArgsLogs = new StringBuilder();
        public ParsedArgs(string[] args) 
        {
            for(int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-s" || args[i] == "-silent")
                {
                    this.IsSilent = true;
                    continue;
                }

                if (args[i] == "-d" || args[i] == "-diff")
                {
                    this.ShouldDiffBeDisplayed = true;
                    continue;
                }

                if (args[i] == "-c" || args[i] == "-config")
                {
                    this.PathToConfigFile = ParsePathToConfigFile(args, i);
                    if (!string.IsNullOrWhiteSpace(this.PathToConfigFile))
                        i++;// move index by +1 (because that's the value of the -c argument, not a new argument)
                    continue;
                }

                this.parsedArgsLogs.AppendLine($"The supplied argument {args[i]} was not reckognized and will be ignored");
            }
        }

        private string? ParsePathToConfigFile(string[] args, int i)
        {
            try
            {
                if (i+1 >= args.Length)
                {
                    this.parsedArgsLogs.AppendLine($"{args[i]} argument was specified but it must be followed by an actual path to conflict");
                    return null;
                }

                var pathToConfig = args[i+1];
                if (!File.Exists(pathToConfig))
                {
                    this.parsedArgsLogs.AppendLine($"Path to config was specified with the {args[i]} argument as '{pathToConfig}'; but no such file exists. The {args[i]} argument will be ignored");
                    return null;
                }

                if (new FileInfo(pathToConfig).Name != Config.ConfigFilename)
                {
                    this.parsedArgsLogs.AppendLine($"Path to config was specified with the {args[i]} argument as '{pathToConfig}'; but the specified file must named: '{Config.ConfigFilename}'");
                    return null;
                }

                return pathToConfig;
            }
            catch (Exception)
            {
                this.parsedArgsLogs.AppendLine($"Path to config was specified incorrectly; the {args[i]} argument will be ignored");
                return null;
            }
        }

        public string GetArgsParsingLogs() => this.parsedArgsLogs.ToString();
    }
}
