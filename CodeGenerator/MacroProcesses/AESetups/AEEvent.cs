using System.Collections.Generic;

namespace CgenMin.MacroProcesses
{
    public abstract class AEEvent : AOWritableToAOClassContents
    {
        public int EventPoolSize { get; }
        public string[] EventDefinition { get; protected set; }

        public static int NumOfEventsCreatedSoFar { get { return numOfEventsCreatedSoFar; }  }

        static List<AEEvent> AllAEEvents = new List<AEEvent>();
        static int numOfEventsCreatedSoFar = 0;
        int eventID = 0;
        public AEEvent(string fromLibrary, string ClassName, int eventPoolSize, params string[] eventDefinition) : base(fromLibrary, ClassName, ClassName + "_", AOTypeEnum.Event)
        {
            EventPoolSize = eventPoolSize;
            EventDefinition = eventDefinition;
            numOfEventsCreatedSoFar++;
            eventID = numOfEventsCreatedSoFar;
            AllAEEvents.Add(this);
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

            ret += $"#define Event{eventID.ToString()} {ClassName}"; ret += "\n";
            ret += $"#define Event{eventID.ToString()}Size {EventPoolSize.ToString()}"; ret += "\n";

            return ret;
        }


        bool onlyWriteFilesOnce = false;
        protected override List<RelativeDirPathWrite> _WriteTheContentedToFiles()
        {
            //only do this once and only at the end after all event instances have been created
            if (onlyWriteFilesOnce == false)
            {

                //first grab all the contents of the events
                List<string> Allcontents = new List<string>();
                foreach (var item in AllAEEvents)
                {
                    string ArgSection = string.Join("\n", item.EventDefinition);
                    Allcontents.Add(AEInitializing.TheMacro2Session.GenerateFileOut("AERTOS/AEEvent",
                        new MacroVar() { MacroName = "ClassName", VariableValue = $"{ClassName}" },
                        new MacroVar() { MacroName = "ArgSection", VariableValue = $"{ArgSection}" }
                        ));
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


}
