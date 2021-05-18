using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace CgenCmakeLibrary.FileHandlers
{

    public enum NextStatus
    {
        Empty,
        OptionFound,
        Done
    }

    public class NEXTFileParser : FileHandler
    {

        //from base
        //public bool IsFileContentsFilled(); 
        //public string GetContents();
        //public void RemoveContents(); 
        public NEXTFileParser(DirectoryInfo dir) : base(dir, "cgenCmakeConfigNEXT.txt")
        {
            mutex = new Mutex();
        }
         

        public NextStatus ParseNextFile()
        {
            mutex.WaitOne();
            if (IsFileContentsFilled() == false)
            {
                return NextStatus.Empty;
            }
            else
            {
                string contentsNext = GetContents();// File.ReadAllText(FullFilePath);
                if (contentsNext.Contains("---OPTIONS_DONE---"))
                {
                    RemoveContents();
                    return NextStatus.Done;
                }
                else
                {
                    NextOption = Option.Deserialize(contentsNext);
                    RemoveContents();
                    return NextStatus.OptionFound;
                }

            }
            mutex.ReleaseMutex();

            return NextStatus.Empty;
        }


        public Option NextOption { get; private set; }
        public Mutex mutex { get; private set; }


        //    string contents = File.ReadAllText(FullFilePath);


        //    Regex regex = new Regex(@"NAME (.*)\nDESCRIPTION (.*)\n");// POSSIBLEVALUES (.*)\nCONSTRICTS_LATER_OPTIONS ");
        //    Match match = regex.Match(contents);

        //    if (match.Success)
        //    {
        //        //group 0 is the whole thing

        //        //group 1 NEXTOPTION
        //        string nextOptionName = match.Groups[1].Value.Trim();
        //        Option nextOption = new Option(nextOptionName);


        //        //group 2 DESCRIPTION 
        //        string description = match.Groups[2].Value;
        //        nextOption.Description = description;
        //        contents = match.RemoveContentsMatchesSoFar(contents);

        //        //group 3 POSSIBLEVALUES  
        //        regex = new Regex(@"POSSIBLEVALUES (.*)\nCONSTRICTS_LATER_OPTIONS ");// POSSIBLEVALUES (.*)\nCONSTRICTS_LATER_OPTIONS ");
        //        match = regex.Match(contents);
        //        List<string> possibleValue = match.Groups[1].Value.Trim().Split(' ').ToList();
        //        possibleValue.ForEach(p=> nextOption.AddPossibleValue(p));
        //        contents = match.RemoveContentsMatchesSoFar(contents);

        //        //get all possible value constrictions
        //        //first delete all contents as thus far 
        //        regex = new Regex(@"(.*?);");
        //        var matchs = regex.Matches(contents);

        //        foreach (Match mm in matchs)
        //        {
        //            string constrictContents = mm.Groups[0].Value.Trim();
        //            Regex regexConst = new Regex(@"(.*?)@(.*?)@");//@(.*?);
        //            Match matchsConst = regexConst.Match(constrictContents);



        //            //group 1 this possible value will restrict 
        //            string possibleValueThatRestrictsName = matchsConst.Groups[1].Value.Trim();
        //            PossibleValue possibleValueThatRestricts = nextOption.GetPossibleValue(possibleValueThatRestrictsName);

        //            //group 2 option that possible value restricts 
        //            string optionConstrictedName = matchsConst.Groups[2].Value.Trim();
        //            //if option does not exist yet, create it
        //            Option optionToConstrict = AllOptions.Instance.GetOptionCreateIfNotExists(optionConstrictedName);
        //            ConstrictedOptions constOpt = new ConstrictedOptions(nextOption,optionToConstrict, possibleValueThatRestricts);

        //            //group 3 get all constrictedPossible values
        //            string possibleconstricContent = matchsConst.RemoveContentsMatchesSoFar(constrictContents);
        //            Regex regexpv = new Regex(@"(.*?)@");//@(.*?);
        //            var matchpv = regexpv.Matches(possibleconstricContent);
        //            foreach (Match mmm in matchpv)
        //            {
        //                string constrictValue = mmm.Groups[1].Value.Trim();
        //                constOpt.AddValueConstricted(constrictValue);
        //                possibleconstricContent = mmm.RemoveContentsMatchesSoFar(possibleconstricContent);
        //            }
        //            possibleconstricContent = possibleconstricContent.Remove(possibleconstricContent.IndexOf(';'));
        //            constOpt.AddValueConstricted(possibleconstricContent);

        //            possibleValueThatRestricts.AddConstrictedOption(constOpt);

        //        }



        //            //set in alloptions
        //            AllOptions.Instance.SetOption(nextOption);
        //        NextOption = nextOption;

        //    }

        //}



    }
}
