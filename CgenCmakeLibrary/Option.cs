using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
                    fullConstr += "@" + c.ValueConstricted;
                    //ValueList
                    //c.ValueConstricted.ForEach(vc => fullConstr += "@" + vc);

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

            Regex regex = new Regex(@"NAME (.*)\n.*DESCRIPTION (.*)\n");// POSSIBLEVALUES (.*)\nCONSTRICTS_LATER_OPTIONS ");
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
                regex = new Regex(@"POSSIBLEVALUES\s?(.*)\n.*CONSTRICTS_LATER_OPTIONS ");// POSSIBLEVALUES (.*)\nCONSTRICTS_LATER_OPTIONS ");
                match = regex.Match(contents);
                List<string> possibleValue = match.Groups[1].Value.Trim().Split(' ').ToList();
                foreach (var p in possibleValue)
                {
                    if (string.IsNullOrEmpty(p) == false)
                    {
                        nextOption.AddPossibleValue(p);
                    } 
                }

                //first delete all contents as thus far 
                contents = match.RemoveContentsMatchesSoFar(contents);
                contents = contents.Trim();

                //place a ";" at the end of contents if none exists.
                if (string.IsNullOrEmpty(contents) == false)
                {
                    if (contents.Last().ToString() != ";")
                    {  
                        if (contents.Last().ToString() == "\n")
                        {
                            contents = contents.Remove(contents.Length-1);
                            contents = contents.Trim();
                            if (string.IsNullOrEmpty(contents) == false)
                            {
                                contents += ";";
                            }
                               
                        }
                        else
                        {
                            contents += ";";
                        }
                    } 
                }

                //get all possible value constrictions
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
                    List<string> constValues = new List<string>();
                    foreach (Match mmm in matchpv)
                    {
                        string constrictValue = mmm.Groups[1].Value.Trim();
                        constValues.Add(constrictValue);
                        //constOpt.AddValueConstricted(constrictValue);
                        possibleconstricContent = mmm.RemoveContentsMatchesSoFar(possibleconstricContent);
                    }
                    possibleconstricContent = possibleconstricContent.Remove(possibleconstricContent.IndexOf(';'));
                    constValues.Add(possibleconstricContent);
                    //constOpt.AddValueConstricted(possibleconstricContent);

                    //create contrict options for each constValues found 
                    List<ConstrictedOptions> constrictions = new List<ConstrictedOptions>();
                    foreach (var item in constValues)
                    {
                        ConstrictedOptions cv = new ConstrictedOptions(constOpt.ForOption, constOpt.OptionConstricted, constOpt.WhenThisPossibleValueIsChosen);
                        cv.AddValueConstricted(item);
                        constrictions.Add(cv);
                    }


                    foreach (var item in constrictions)
                    {
                        possibleValueThatRestricts.AddConstrictedOption(item);
                    }
                    

                }


            }

                //set in alloptions
                AllOptions.Instance.SetOption(nextOption);

                return nextOption;
        }

        public void AddConstrictedOption(Option constrictsThisOption, string whenThisPossibleValueIsChosen, string thisPossibleValueIsConstricted)
        { 
            if (this.IsPossibleValueExists(whenThisPossibleValueIsChosen) == false )
            {
                throw new Exception();
            }

            var pv = GetPossibleValue(whenThisPossibleValueIsChosen);
            
            ConstrictedOptions copt = new ConstrictedOptions(this, constrictsThisOption, pv);
            copt.AddValueConstricted(thisPossibleValueIsConstricted);
            pv.AddConstrictedOption(copt);
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






    public class ConstrictedOptionsComparer : IEqualityComparer<ConstrictedOptions>
    {
        public bool Equals([AllowNull] ConstrictedOptions x, [AllowNull] ConstrictedOptions y)
        {
            return x.GetUniqieString() == y.GetUniqieString();
        }

        public int GetHashCode([DisallowNull] ConstrictedOptions obj)
        {
            return obj.GetUniqieString().GetHashCode();
        }
    }

    public class ConstrictedOptions : IEquatable<ConstrictedOptions>
    {
        public ConstrictedOptions(Option thisOption, Option constrictsThisOption, PossibleValue whenThisPossibleValueIsChosen)
        {
            ForOption = thisOption;
            OptionConstricted = constrictsThisOption;
            WhenThisPossibleValueIsChosen = whenThisPossibleValueIsChosen;
            //ValueConstricted = new List<string>();
        }

        public string GetUniqieString()
        {
            return this.ForOption.Name + this.OptionConstricted.Name + this.ValueConstricted;
        }


        public void AddValueConstricted(string valueConstricted) {
            //ValueList
            ValueConstricted = valueConstricted;
            //ValueConstricted.Add(valueConstricted);
            }

        //public void GetValueConstricted(string valueConstricted) { ValueConstricted.Add(valueConstricted); }
        public bool IsValueConstrictedExists(string valueConstricted) {
            //ValueList
            return ValueConstricted == valueConstricted;
            //return ValueConstricted.Exists(c => c== valueConstricted); 
        }

        public bool Equals([AllowNull] ConstrictedOptions other)
        {
            bool s1 = this.WhenThisPossibleValueIsChosen == other.WhenThisPossibleValueIsChosen;
            return s1;
        }

        public Option ForOption { get; }
        public PossibleValue WhenThisPossibleValueIsChosen { get; }
        public Option OptionConstricted{get; }
        public string ValueConstricted { get; protected set; }
    }





    public class PossibleValueEqual : IEqualityComparer<PossibleValue>
    {
        public bool Equals([AllowNull] PossibleValue x, [AllowNull] PossibleValue y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode([DisallowNull] PossibleValue obj)
        {
            return obj.Name.GetHashCode();
        }
    }

    public class PossibleValue // : IEqualityComparer<PossibleValue>// IEquatable<PossibleValue>, IEqualityComparer<PossibleValue>
    {

        public PossibleValue(string name, Option forOption)
        {
            Name = name;
            ForOption = forOption;
            MyConstrictedOptions = new List<ConstrictedOptions>();
        }

 
        public static bool operator==(PossibleValue thispos, PossibleValue other)
        {
            return thispos.Name == other.Name;
        }

        public static bool operator !=(PossibleValue thispos, PossibleValue other)
        {
            return thispos.Name != other.Name;
        }


        public void AddConstrictedOption(ConstrictedOptions optionToConstrict)
        {

            //check for duplicates
            if (!MyConstrictedOptions.Exists(con => con.GetUniqieString() == optionToConstrict.GetUniqieString()))
            {
                MyConstrictedOptions.Add(optionToConstrict);
            }
            return;
        }

        //public void AddConstrictedOption(Option optionToConstrict, string PossibleValueToConstrict)
        //{
        //    AddConstrictedOption(optionToConstrict.Name, PossibleValueToConstrict);
        //}

 
        public void AddConstrictedOption(Option constrictsThisOption, string thisPossibleValueIsConstricted) {

            string whenThisPossibleValueIsChosen = this.Name; 

            var pv = this;

            ConstrictedOptions copt = new ConstrictedOptions(ForOption, constrictsThisOption, pv);
            copt.AddValueConstricted(thisPossibleValueIsConstricted);
            
            //check for duplicates
            if (!MyConstrictedOptions.Exists(con => con.GetUniqieString() == copt.GetUniqieString()))
            {
                pv.AddConstrictedOption(copt);
            }
            return;
        }

        public bool IsConstrictedOptionExists(Option optionToConstricted, string possibleValueToConstrict)
        {
            bool exists = false;
            ConstrictedOptions newConstOpt = new ConstrictedOptions(ForOption, optionToConstricted, this);
            newConstOpt.AddValueConstricted( possibleValueToConstrict);

            foreach (var c in MyConstrictedOptions)
            {
                exists = (c.GetUniqieString() == newConstOpt.GetUniqieString());
                if (exists == true)
                {
                    break;
                }
            } 
            return exists;
        }

        public List<ConstrictedOptions> GetConstrictedOptionsForConstrictedOption(string optionToConstrictName)
        {
            return MyConstrictedOptions.Where(c => {

                return c.OptionConstricted.Name == optionToConstrictName;

            }).ToList();
        }



        public ConstrictedOptions GetConstrictedOption(string optionToConstrictName, string PossibleValueConstrited)
        { return MyConstrictedOptions.Where(c => {
            
            return c.OptionConstricted.Name == optionToConstrictName && c.ValueConstricted == PossibleValueConstrited;
        
        }).First(); }



        public string Name { get; }
        public Option ForOption { get; }

        public List<ConstrictedOptions> MyConstrictedOptions { get; }

    }






    public class ConstrictionInfo
    {
        public ConstrictionInfo()
        {
            PVsConstrictedNotAllowed = new List<string>();
        }
        public string ForOption;
        public string WhenPVSelected;
        public string ConstrictsOption;
        public List<string> PVsConstrictedNotAllowed;

        public static List<ConstrictionInfo2> Convert_ConstInfo1_To_ConstInfo2(List<ConstrictionInfo> constInfo, Option forConstrictedOption)
        {
            List<ConstrictionInfo2> ret = new List<ConstrictionInfo2>();

            foreach (var copvs in forConstrictedOption.MyPossibleValues)
            {
                 
                foreach (var coInfo in constInfo)
                {
                    ConstrictionInfo2 co2 = new ConstrictionInfo2();

                    if (coInfo.ConstrictsOption == forConstrictedOption.Name)
                    {
                        if (coInfo.PVsConstrictedNotAllowed.Exists(pvna => pvna == copvs.Name) )
                        {
                            co2.PVsConstrictedNotAllowed = copvs.Name;
                            co2.ConstrictsOption = coInfo.ConstrictsOption;

                            if (coInfo.PVsConstrictedNotAllowed.Exists(i => i == copvs.Name))
                            {
                                //if (co2.AllTheseOptions_WhenPVSelected.ContainsKey(coInfo.ForOption) == false)
                                //{
                                co2.AllTheseOptions.Add(coInfo.ForOption);
                                co2.WhenPVSelected.Add(coInfo.WhenPVSelected);
                                //}

                            }
                        }
                         
                    }

                    else
                    {
                        //this must have been a past selection, switch the constricted and foroption

                        co2.PVsConstrictedNotAllowed = copvs.Name;
                        co2.ConstrictsOption = coInfo.ForOption;
                        co2.PastSelectedOption = coInfo.ConstrictsOption;
                        co2.isPastSelectionRestricting = true;

                    }
                   



                    ret.Add(co2);
                }

               
            }

            return ret;

        }
    }


    public class ConstrictionInfo2
    {


        public ConstrictionInfo2()
        {
            AllTheseOptions = new List<string>();
            WhenPVSelected = new List<string>();
            isPastSelectionRestricting = false;
        }

        public List<string> AllTheseOptions;
        public List<string> WhenPVSelected; 
        public string ConstrictsOption;
        public string PVsConstrictedNotAllowed;

        public bool isPastSelectionRestricting;
        public string PastSelectedOption;

        public string GetDisplay()
        {
            string ret = "";
            ret = "for constricted option " + ConstrictsOption + " ,\nits possible value " + PVsConstrictedNotAllowed;
            if (isPastSelectionRestricting == false)
            {
                ret = ret + "\nis not allowed because the following options had the selected PVs \n";
                for (int i = 0; i < AllTheseOptions.Count; i++)
                {
                    ret = ret + AllTheseOptions[i] + ": " + WhenPVSelected[i] + "\n";
                }
                
            }
            else
            {
                ret = ret + "\nis not allowed because of the chosen PV, selected past option "+ PastSelectedOption + " chose.";
            }


            return ret;
        }
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
                Update(option);

                return;
            }
            else
            {
                Options.Add(option);
            }
        }



        private void Update(Option newOption)
        { 
                 

                //get the old options
            Option oldOption = GetOption(newOption.Name);


            //constricting values : union each 
            foreach (var pvold in oldOption.MyPossibleValues)
            {
                foreach (var pvnew in newOption.MyPossibleValues)
                {
                    List<ConstrictedOptions> CoList = new List<ConstrictedOptions>();
                    if (pvold == pvnew)
                    { 
                        CoList = pvold.MyConstrictedOptions.Union(pvnew.MyConstrictedOptions, new ConstrictedOptionsComparer()).ToList();
                        pvold.MyConstrictedOptions.Clear();
                        pvold.MyConstrictedOptions.AddRange(CoList);
                    }
                }
            }

            //union the possible values by adding any extra possible values, but not taking away any missing ones
            List<PossibleValue> newOptValueList = new List<PossibleValue>();
            newOptValueList = oldOption.MyPossibleValues
                .Union(newOption.MyPossibleValues, new PossibleValueEqual()).ToList();//
            oldOption.MyPossibleValues.Clear();
            oldOption.MyPossibleValues.AddRange(newOptValueList);
             

            //description: replace the description if it is empty and updating one is not.
            if (   (string.IsNullOrEmpty(oldOption.Description) == true)
                && (string.IsNullOrEmpty(newOption.Description) == false))
            {
                oldOption.Description = newOption.Description;
            }



 


            newOption = oldOption;
        }



        public static List<PossibleValue> GetAllowablePVs(Option forConstrictedOption, List<OptionsSelected> selectedOptionsSoFar, ref List<ConstrictionInfo> theConstrictionsOfThePVs)
        {
            //if there are no selected option's constrictedOptions that mention the forConstrictedOption, then return an "any"
            //or a all possible values
            //bool anyMention = false;
            //foreach (var selOpt in selectedOptionsSoFar)
            //{
            //    var pv = selOpt.option.MyPossibleValues.First(pvv => selOpt.possibleValueSelection == pvv.Name);

            //        foreach (var co in pv.MyConstrictedOptions)
            //        {
            //            anyMention = anyMention == true ? true : (co.OptionConstricted.Name == forConstrictedOption.Name);
            //            if (anyMention == true)
            //            {
            //                break;
            //            }

            //        }

            //}

            //if (anyMention == false || selectedOptionsSoFar.Count == 0)
            //{
            //    return forConstrictedOption.MyPossibleValues;
            //}


            List<string> UnallowablePVs_l = new List<string>();
            //go through the possible values of the forConstrictedOption first and check if any past selected options match any of its own restrictions. 
            foreach (var pv in forConstrictedOption.MyPossibleValues)
            {
                List<OptionsSelected> ForPastSelections = new List<OptionsSelected>();
                OptionsSelected thisConstSel = new OptionsSelected() { option = forConstrictedOption, possibleValueSelection = pv.Name };
                ForPastSelections.Add(thisConstSel);
                foreach (var so in selectedOptionsSoFar)
                {
                    List<ConstrictionInfo> cOpts = new List<ConstrictionInfo>();
                    var allowablePV = _GetAllowablePVs(so.option, ForPastSelections, ref cOpts);
                    //if the selected possible value is not in the allowable possible value, it must be a value that would have restricted the forConstrictedOption
                    if (allowablePV.Exists(av => so.possibleValueSelection == av.Name) == false)
                    {
                        var unall = new List<string>(); unall.Add(pv.Name);
                         UnallowablePVs_l = UnallowablePVs_l.Union(unall).ToList();

                        theConstrictionsOfThePVs = theConstrictionsOfThePVs != null ?  theConstrictionsOfThePVs.Union(cOpts).ToList() : null;

                    }
                }
            }


            List<string> PossbleValueOfConstric = forConstrictedOption.MyPossibleValues.Select(pv => pv.Name).ToList();

            List<ConstrictionInfo> cOptss = new List<ConstrictionInfo>(); 
            var allowablePVs = _GetAllowablePVs(forConstrictedOption, selectedOptionsSoFar, ref cOptss).Select(av => av.Name).ToList();
            theConstrictionsOfThePVs = theConstrictionsOfThePVs != null ? theConstrictionsOfThePVs.Union(cOptss).ToList() : null;

            List<string> UnallowablePVs = PossbleValueOfConstric.Except(allowablePVs).ToList(); 
            UnallowablePVs_l = UnallowablePVs_l.Union(UnallowablePVs).ToList();

            allowablePVs = PossbleValueOfConstric.Except(UnallowablePVs_l).ToList();
             

            return  forConstrictedOption.MyPossibleValues.Where(pv => allowablePVs.Exists(apv => apv == pv.Name)).ToList();
        }




        public static List<PossibleValue> _GetAllowablePVs(Option forConstrictedOption, List<OptionsSelected> selectedOptionsSoFar, ref List<ConstrictionInfo> theConstrictionsOfThePVs)
        {

            //go through each option selected and grab constricted options that point to the forConstrictedOption
            List<string> PossbleValueOfConstric = forConstrictedOption.MyPossibleValues.Select(pv => pv.Name).ToList();
            List<string> allowablePVs = new List<string>();
            List<string> UnallowablePVs = new List<string>();
            List<string> UnallowablePVs_l = new List<string>();
            foreach (var selOpt in selectedOptionsSoFar)
            {
                PossibleValue pv = selOpt.option.MyPossibleValues.First(pv => pv.Name == selOpt.possibleValueSelection);

                foreach (var co in pv.MyConstrictedOptions)
                {
                    if (co.OptionConstricted.Name == forConstrictedOption.Name)
                    { 

                        allowablePVs.Clear();
                        allowablePVs.AddRange(pv.MyConstrictedOptions
                           .Where(co => co.OptionConstricted.Name == forConstrictedOption.Name)
                           .Select(s => s.ValueConstricted).ToList());
                        allowablePVs.Distinct().ToList();

                        UnallowablePVs = PossbleValueOfConstric.Except(allowablePVs).ToList(); //.RemoveAll(av => allowablePVs.Exists(av));

                        ConstrictionInfo info = new ConstrictionInfo() { ForOption = selOpt.option.Name, WhenPVSelected = pv.Name, ConstrictsOption = forConstrictedOption.Name };
                        info.PVsConstrictedNotAllowed.AddRange(UnallowablePVs);
                        theConstrictionsOfThePVs.Add(info);

                        UnallowablePVs_l = UnallowablePVs_l.Union(UnallowablePVs).ToList();
                    }
                }
                 

            }

            allowablePVs = PossbleValueOfConstric.Except(UnallowablePVs_l).ToList();

            return forConstrictedOption.MyPossibleValues.Where(pv => allowablePVs.Exists(apv => apv == pv.Name)).ToList();
        }


        public bool OptionExists(string optionName) {return Options.Exists(s => s.Name == optionName); }

        public int GetOptionsLength() { return Options.Count; }
        public List<Option> Options { get; }
    
    }
    
}
