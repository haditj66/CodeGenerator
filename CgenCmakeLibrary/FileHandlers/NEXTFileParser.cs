using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CgenCmakeLibrary.FileHandlers
{

    public enum NextStatus
    {
        Empty,
        OptionFound,
        Reset,
        Done
    }

    public class NEXTFileParser : FileHandler
    {

        private Task _NextFileThread;
        private Action OptionDoneAction;
        private Action OptionFoundAction;
        private Action ResetGuiAction;
        private CancellationTokenSource ts;
        private CancellationToken ct;

        public void StartNextFileUpdater(Action optionDoneAction, Action optionFoundAction, Action resetGuiAction)
        {
            ResetGuiAction = resetGuiAction;
            OptionDoneAction = optionDoneAction;
            OptionFoundAction = optionFoundAction;

            //_NextFileThread = new Thread(this._NextFileUpdater);
            //_NextFileThread.Start();

            ts = new CancellationTokenSource();
            ct = ts.Token;
            _NextFileThread = Task.Factory.StartNew(() =>
            {
                this._NextFileUpdater();
            });
        }

        public void StopNextFileUpdater()
        {
            ts.Cancel();
            _NextFileThread.Wait();
            _NextFileThread.Dispose();
        }

        private void _NextFileUpdater()
        {


            while (true)
            {
                Thread.Sleep(500);

                if (ct.IsCancellationRequested)
                {
                    break;
                }



                NextStatus nextStatus = this.ParseNextFile();
                
                if (nextStatus != NextStatus.Empty)
                {

                    //if it is done then display a done on the output
                    if (nextStatus == NextStatus.Done)
                    {
                        OptionDoneAction();
                        //this.Dispatcher.Invoke(() =>
                        //{
                        //    guioutputScrollHandler.display("Options configuring Done", OutputLevel.Normal); 
                        //});

                    }
                    else if (nextStatus == NextStatus.Reset)
                    {
                        ResetGuiAction();
                    }
                    else if (nextStatus == NextStatus.OptionFound)
                    {

                        OptionFoundAction();

                        ////first update any options that are already saved of this next option. 
                        //if (AllOptions.Instance.OptionExists(this.NextOption.Name) == true)
                        //{
                        //    AllOptions.Instance.SetOption(this.NextOption);

                        //    this._SetNextOption(AllOptions.Instance.GetOption(this.NextOption.Name));

                        //    savedOptionsFileHandler.SaveAllOptions();
                        //}

                        ////make sure the previous option has been selected yet
                        //bool isPrevOptionSelectedYet = true;
                        //if (OptionsSelectedGuiBox.optionsSelected.Count > 0)
                        //{
                        //    isPrevOptionSelectedYet = !string.IsNullOrEmpty(OptionsSelectedGuiBox.optionsSelected[OptionsSelectedGuiBox.optionsSelected.Count - 1].possibleValueSelection);
                        //}

                        ////option must have been found 
                        //if (isPrevOptionSelectedYet == true)
                        //{
                        //    this.Dispatcher.Invoke(() =>
                        //    {  
                        //        OptionsSelectedGuiBox.AddOptionSelectedToGui(this.NextOption, true); 
                        //    });

                        //    ii++;
                        //}
                        //else
                        //{
                        //    this.Dispatcher.Invoke(() =>
                        //    {
                        //        guioutputScrollHandler.display("You need to select the previous option", OutputLevel.Problem);
                        //    });
                        //}



                    }
                }
            }
        }



    

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
                if (contentsNext.Contains("---RESET_GUI---"))
                {
                    RemoveContents();
                    return NextStatus.Reset;
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

        public void _SetNextOption(Option nextOption) { NextOption = nextOption; }


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
