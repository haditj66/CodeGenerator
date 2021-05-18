using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CgenCmakeLibrary.FileHandlers
{
    public class CmakeCacheFileParser : FileHandler
    {
        //from base
        //public bool IsFileContentsFilled(); 
        //public string GetContents();
        //public void RemoveContents();

        public CmakeCacheFileParser(DirectoryInfo dir) : base(dir, "cgenCmakeCache.cmake")
        {

        }

        public void WriteOptionsToFile(List<OptionsSelected> optionsToWrite)
        {
            //first remove all contents
            RemoveContents();

            foreach (var opt in optionsToWrite)
            {
                File.AppendAllText(FullFilePath, "set("+ opt.option.Name + " "+ opt.possibleValueSelection+ ")"); 
            }
        }

        public List<OptionsSelected> LoadOptionsSelected()
        {
            //first remove all contents
            string Contents = GetContents();

            List<OptionsSelected> retOptSel = new List<OptionsSelected>();

            using (var reader = new StringReader(Contents))
            {
                for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    if (string.IsNullOrEmpty(line) == false)
                    {
                        Regex regex = new Regex(@"set\((.*) (.*)\)");
                        Match match = regex.Match(line); 
                        if (match.Success)
                        {
                            OptionsSelected optSel = new OptionsSelected();
                            string optionName = match.Groups[1].Value.Trim();
                            optSel.option = AllOptions.Instance.GetOptionCreateIfNotExists(optionName);
                            optSel.possibleValueSelection = match.Groups[2].Value.Trim();
                            
                            retOptSel.Add(optSel);
                        }

                    }
                }
            }
            return retOptSel;
            
        }


    }
}


