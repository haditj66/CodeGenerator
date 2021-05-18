using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CgenCmakeLibrary
{




    public class Option
    {
        public Option(string name)
        {
            Name = name;
            Description = "";
            MyPossibleValues = new List<PossibleValue>();
        }

        public string Serialize()
        {
            string serializedContent = "";

            serializedContent += "NAME " +Name+"\n";
            serializedContent += "DESCRIPTION " + Description + "\n";
            serializedContent += "POSSIBLEVALUES";
            MyPossibleValues.ForEach(p => serializedContent += " " + p.Name);
            serializedContent += "\n";
            serializedContent += "CONSTRICTS_LATER_OPTIONS ";

            List<string> constr = new List<string>();

            foreach (var p in MyPossibleValues)
            {
                foreach (var c in p.MyConstrictedOptions)
                {
                    string fullConstr = "" + p.Name;

                    string constVal = "";

                    fullConstr += "@" + c.OptionConstricted.Name;
                    c.ValueConstricted.ForEach(vc => fullConstr += "@" + vc);
                 
                    fullConstr += ";";
                    constr.Add(fullConstr);
                }
            } 

            constr.ForEach(c => serializedContent += c);

            return serializedContent;
        }


        public static Option Deserialize(string contents)
        {

            Option nextOption = null;

            Regex regex = new Regex(@"NAME (.*)\nDESCRIPTION (.*)\n");// POSSIBLEVALUES (.*)\nCONSTRICTS_LATER_OPTIONS ");
            Match match = regex.Match(contents);

            if (match.Success)
            {
                //group 0 is the whole thing

                //group 1 NEXTOPTION
                string nextOptionName = match.Groups[1].Value.Trim();
                nextOption = new Option(nextOptionName);


                //group 2 DESCRIPTION 
                string description = match.Groups[2].Value;
                nextOption.Description = description;
                contents = match.RemoveContentsMatchesSoFar(contents);

                //group 3 POSSIBLEVALUES  
                regex = new Regex(@"POSSIBLEVALUES (.*)\nCONSTRICTS_LATER_OPTIONS ");// POSSIBLEVALUES (.*)\nCONSTRICTS_LATER_OPTIONS ");
                match = regex.Match(contents);
                List<string> possibleValue = match.Groups[1].Value.Trim().Split(' ').ToList();
                foreach (var p in possibleValue)
                {
                    if (string.IsNullOrEmpty(p) == false)
                    {
                        nextOption.AddPossibleValue(p);
                    } 
                }

                //possibleValue.ForEach(p => nextOption.AddPossibleValue(p));
                contents = match.RemoveContentsMatchesSoFar(contents);

                //get all possible value constrictions
                //first delete all contents as thus far 
                regex = new Regex(@"(.*?);");
                var matchs = regex.Matches(contents);

                foreach (Match mm in matchs)
                {
                    string constrictContents = mm.Groups[0].Value.Trim();
                    Regex regexConst = new Regex(@"(.*?)@(.*?)@");//@(.*?);
                    Match matchsConst = regexConst.Match(constrictContents);



                    //group 1 this possible value will restrict 
                    string possibleValueThatRestrictsName = matchsConst.Groups[1].Value.Trim();
                    PossibleValue possibleValueThatRestricts = nextOption.GetPossibleValue(possibleValueThatRestrictsName);

                    //group 2 option that possible value restricts 
                    string optionConstrictedName = matchsConst.Groups[2].Value.Trim();
                    //if option does not exist yet, create it
                    Option optionToConstrict = AllOptions.Instance.GetOptionCreateIfNotExists(optionConstrictedName);
                    ConstrictedOptions constOpt = new ConstrictedOptions(nextOption, optionToConstrict, possibleValueThatRestricts);

                    //group 3 get all constrictedPossible values
                    string possibleconstricContent = matchsConst.RemoveContentsMatchesSoFar(constrictContents);
                    Regex regexpv = new Regex(@"(.*?)@");//@(.*?);
                    var matchpv = regexpv.Matches(possibleconstricContent);
                    foreach (Match mmm in matchpv)
                    {
                        string constrictValue = mmm.Groups[1].Value.Trim();
                        constOpt.AddValueConstricted(constrictValue);
                        possibleconstricContent = mmm.RemoveContentsMatchesSoFar(possibleconstricContent);
                    }
                    possibleconstricContent = possibleconstricContent.Remove(possibleconstricContent.IndexOf(';'));
                    constOpt.AddValueConstricted(possibleconstricContent);

                    possibleValueThatRestricts.AddConstrictedOption(constOpt);

                }


            }

                //set in alloptions
                AllOptions.Instance.SetOption(nextOption);

                return nextOption;
        }


        public void AddPossibleValue(string possibleValue)
        {
            //first check if posible value already exists. no duplicaes
            bool exists = MyPossibleValues.Exists(p => p.Name == possibleValue);
            if (exists){return;}
            MyPossibleValues.Add(new PossibleValue(possibleValue, this));
        }

        public PossibleValue GetPossibleValue(string possibleValueName)
        {
            return MyPossibleValues.Where(opt => opt.Name == possibleValueName).First();
        }

        public bool IsPossibleValueExists(string possibleValueName)
        {
            return MyPossibleValues.Exists(opt => opt.Name == possibleValueName);
        }

        public string Description { get; set; }
        public string Name { get;}
        public List<PossibleValue> MyPossibleValues { get; }
    }








    public class ConstrictedOptions
    {
        public ConstrictedOptions(Option thisOption, Option constrictsThisOption, PossibleValue whenThisPossibleValueIsChosen)
        {
            ForOption = thisOption;
            OptionConstricted = constrictsThisOption;
            WhenThisPossibleValueIsChosen = whenThisPossibleValueIsChosen;
            ValueConstricted = new List<string>();
        }

        public void AddValueConstricted(string valueConstricted) {  ValueConstricted.Add(valueConstricted); }

        //public void GetValueConstricted(string valueConstricted) { ValueConstricted.Add(valueConstricted); }
        public bool IsValueConstrictedExists(string valueConstricted) { return ValueConstricted.Exists(c => c== valueConstricted); }

        public Option ForOption { get; }
        public PossibleValue WhenThisPossibleValueIsChosen { get; }
        public Option OptionConstricted{get; }
        public List<string> ValueConstricted { get; }
    }









    public class PossibleValue
    {

        public PossibleValue(string name, Option forOption)
        {
            Name = name;
            ForOption = forOption;
            MyConstrictedOptions = new List<ConstrictedOptions>();
        }


        public void AddConstrictedOption(ConstrictedOptions optionToConstrict)
        {

            //check for duplicates
            if (!MyConstrictedOptions.Exists(con => con.OptionConstricted.Name == optionToConstrict.OptionConstricted.Name))
            {  
                MyConstrictedOptions.Add(optionToConstrict);
            }
            return;
        }

        public void AddConstrictedOption(string optionToConstrict) {
             
            //check for duplicates
            if (!MyConstrictedOptions.Exists(con => con.OptionConstricted.Name == optionToConstrict))
            {
                var constOpt = new ConstrictedOptions(ForOption, AllOptions.Instance.GetOptionCreateIfNotExists(optionToConstrict), this);

                MyConstrictedOptions.Add(constOpt);
            }
            return;
        }

        public bool IsConstrictedOptionExists(string optionToConstrictName) { return MyConstrictedOptions.Exists(c => c.OptionConstricted.Name == optionToConstrictName); }
        public ConstrictedOptions GetConstrictedOption(string optionToConstrictName) { return MyConstrictedOptions.Where(c => c.OptionConstricted.Name == optionToConstrictName).First(); }

        public string Name { get; }
        public Option ForOption { get; }

        public List<ConstrictedOptions> MyConstrictedOptions { get; }

    }








    public class AllOptions
    {

        

        private static AllOptions instance = null; 
        private AllOptions()
        {
            Options = new List<Option>();
        } 
        public static AllOptions Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AllOptions();
                }
                return instance;
            }
        }
         
        public void ClearAllOptions() { Options.Clear(); }


        public Option GetOptionCreateIfNotExists(string optionName)
        {
            if (!OptionExists(optionName))
            {
                Option newOption = new Option(optionName);
                Options.Add(newOption);
                return newOption;
            }
            else
            {
                Option optionToGet = Options.Where<Option>(s => s.Name == optionName).First();
                return optionToGet;
            }
        }


        public Option GetOption(string optionName)
        { 
            if (!OptionExists(optionName))
            {
                //Option newOption = new Option(optionName);
                //Options.Add(newOption);
                return null;
            }
            else
            {
                Option optionToGet = Options.Where<Option>(s => s.Name == optionName).First();
                return optionToGet;
            }
        }
         

        public void SetOption(Option option)
        {
            bool isDup = Options.Exists(s => s.Name == option.Name);
            if (isDup)
            { 
                return;
            }
            else
            {
                Options.Add(option);
            }
        }

        public bool OptionExists(string optionName) {return Options.Exists(s => s.Name == optionName); }

        public int GetOptionsLength() { return Options.Count; }
        public List<Option> Options { get; }
    
    }
    
}
