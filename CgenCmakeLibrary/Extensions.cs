using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CgenCmakeLibrary
{
    public static class Extensions
    {
        public static string RemoveContentsMatchesSoFar(this Match match, string contents)
        {
            int indexOFSoFar = contents.IndexOf(match.Groups[0].Value);
            return contents.Remove(indexOFSoFar, match.Groups[0].Value.Length);
        }
    }
}
