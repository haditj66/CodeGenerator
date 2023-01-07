 
using System.Collections.Generic;
using System.Linq;

namespace CgenMin.MacroProcesses
{



    public abstract class AEEvent : AOWritableToAOClassContents
    {
        public bool IsASignal { get; }
        public string[] EventDefinition { get; protected set; }

        public int EventPoolSize { get; protected set; }

        public static int NumOfEventsCreatedSoFar { get { return numOfEventsCreatedSoFar; } }
        static int numOfEventsCreatedSoFar = 0;
        static int numOfSigsCreatedSoFar = 0;
        int eventID = 0;
        int sigID = 0;

        static List<AEEvent> AllAEEvents = new List<AEEvent>();
        public AEEvent( string ClassName, bool isASignal,  params string[] eventDefinition) 
            : base(AEInitializing.RunningProjectName,  ClassName + "_", AOTypeEnum.Event)
        {
            IsASignal = isASignal;
            EventDefinition = eventDefinition;

            //dont add the updateEvt
            if (this.ClassName != "UpdateEVT")
            {
                if (IsASignal)
                {
                    numOfSigsCreatedSoFar++;
                    sigID = numOfSigsCreatedSoFar;
                }
                else 
            {
                    numOfEventsCreatedSoFar++;
                    eventID = numOfEventsCreatedSoFar;
                    AllAEEvents.Add(this);
                }
            }


        }




        public override string GenerateFunctionDefinesSection()
        {
            return "";
        }

        public override string GenerateMainClockSetupsSection()
        {
            return "";
        }

        public override string GenerateMainHeaderSection()
        {
            return "";
        }

        public override string GenerateMainInitializeSection()
        {
            return "";
        }

        public override string GenerateMainLinkSetupsSection()
        {
            return "";
        }

        public override string GetFullTemplateArgs()
        {
            return "";
        }

        public override string GetFullTemplateArgsValues()
        {
            return "";
        }

        public override string GetFullTemplateType()
        {
            return "";
        }

        protected override string _GenerateAEConfigSection(int numOfAOOfThisSameTypeGeneratesAlready)
        {
            //#define Event1 EventTest1
            //#define Event1Size 10

            string ret = "";

            if (IsASignal == false)
            {
                ret += $"#define Event{eventID.ToString()} {ClassName}"; ret += "\n";
                ret += $"#define Event{eventID.ToString()}Size {EventPoolSize.ToString()}"; ret += "\n";
            }
            else
            {
//#define Signal1 DoneUploading_Sig
//#define Signal2 SomeOther_Sig
//#define Signal3 Button1_Sig //value for a null signal
//#define Signal4 Button2_Sig //value for a null signal
//#define Signal5 Button3_Sig //value for a null signal 
                ret += $"#define Signal{sigID.ToString()} {ClassName}"; ret += "\n";
            }

            return ret;
        }


        static bool onlyWriteFilesOnce = false;
        protected override List<RelativeDirPathWrite> _WriteTheContentedToFiles()
        {
            //only do this once and only at the end after all event instances have been created
            if (onlyWriteFilesOnce == false)
            {

                //first grab all the contents of the events
                List<string> Allcontents = new List<string>();
                foreach (var item in AllAEEvents)
                {
                    //dont do this for signals
                    if (IsASignal == false)
                    {
                        string ArgSection = string.Join("\n", item.EventDefinition);
                        Allcontents.Add(AEInitializing.TheMacro2Session.GenerateFileOut("AERTOS/AEEvent",
                            new MacroVar() { MacroName = "ClassName", VariableValue = $"{item.ClassName}" },
                            new MacroVar() { MacroName = "ArgSection", VariableValue = $"{ArgSection}" }
                            ));
                    } 
                }

                string AllEventContents = string.Join("\n", Allcontents);

                AllEventContents = AEInitializing.TheMacro2Session.GenerateFileOut("AERTOS/EventsForProject",
                    new MacroVar() { MacroName = "AllEvents", VariableValue = $"{AllEventContents}" }
                    );



                List<RelativeDirPathWrite> ret = new List<RelativeDirPathWrite>();
                //for (int i = 0; i < AllAEEvents.Count; i++)
                //{
                ret.Add(new RelativeDirPathWrite("EventsForProject.h", "h", "conf", AllEventContents, false));
                //}


                onlyWriteFilesOnce = true;
                return ret;
            }

            return null;

        }

    }





    public abstract class AEEventBase<TDerived> : AEEvent
        where TDerived : AEEventBase<TDerived>, new()
    {
        

        static TDerived _instance;

        protected AEEventBase(  string ClassName, bool isSignal, params string[] eventProperties) 
            : base(  ClassName, isSignal,  eventProperties)
        {
        }

         

        protected static void _Init(int eventPoolSize) 
            //: base(fromLibrary, ClassName, eventPoolSize, eventDefinition)
        {
            if (_instance == null)
            {
                _instance = new TDerived();
                _instance.EventPoolSize = eventPoolSize;
            } 
        }

        
        public static AEEvent Init(int eventPoolSize)
        {
            _Init(eventPoolSize);
            //_instance.EventPoolSize = eventPoolSize;
            return (AEEvent)_instance;

        }

        public static TDerived Instance
        {
            get
            { 
                if (_instance == null)
                {
                    ProblemHandle prob = new ProblemHandle();
                    prob.ThereisAProblem($"You never created an instance of  event {typeof(TDerived).ToString()} \n by calling the AEEventFactory() for it in the _GetEventsInLibrary() function for the AEProject");
                }
                return _instance;
            }
        } 

    }

    public abstract class AEEventEVT<TDerived> : AEEventBase<TDerived>
where TDerived : AEEventBase<TDerived>, new()
    {
        
        protected AEEventEVT(string ClassName, params string[] eventProperties)
         : base(  ClassName, false, eventProperties)
        {
            AO.atLeastOneEvt = true;
        }
    }

    public abstract class AEEventSignal<TDerived> : AEEventBase<TDerived>
where TDerived : AEEventBase<TDerived>, new()
    {
        protected AEEventSignal(  string ClassName, params string[] eventProperties)
         : base( ClassName, true, eventProperties)
        {
        }
    }



}
