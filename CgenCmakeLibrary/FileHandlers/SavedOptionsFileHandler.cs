using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CgenCmakeLibrary.FileHandlers
{
    public class SavedOptionsFileHandler : FileHandler
    {

        public SavedOptionsFileHandler(DirectoryInfo dir) : base(dir, "SavedOptions.txt")
        {

        }

        public void SaveAllOptions()
        {


            string contents = "";
            foreach (var opt in AllOptions.Instance.Options)
            {
                contents = contents + "\n\n";
                contents = contents + opt.Serialize();
            }
            RemoveContents();

            File.WriteAllText(FullFilePath, contents);
        }

        public void LoadAllOptions()
        {
            string contents = GetContents();

            List<Option> optionsDes = new List<Option>();

            int indexForOptCount = 0;
            string optionContent = "";
            bool OptBeginFound = false;

            using (StringReader m = new StringReader(contents))
            {
                string line = "";
                do
                {
                    line = m.ReadLine();
                    if (line != null)
                    {

                    
                    if (OptBeginFound)
                    {
                        optionContent += "\n" + line;

                        indexForOptCount++;
                        if (indexForOptCount == 3)
                        {
                            Option optDes = Option.Deserialize(optionContent);
                            optionsDes.Add(optDes);
                            OptBeginFound = false;
                        }

                    }

                    else if (line.Contains("NAME "))
                    {
                        //next four lines will be the option
                        optionContent = "";
                        optionContent = line;
                        indexForOptCount = 0;
                        OptBeginFound = true;
                    }
                    }
                } while (line != null);
            }

            AllOptions.Instance.ClearAllOptions();
                AllOptions.Instance.Options.AddRange(optionsDes);

            }

        }
    }
