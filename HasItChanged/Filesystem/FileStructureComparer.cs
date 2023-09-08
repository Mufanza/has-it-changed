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
    }
}
