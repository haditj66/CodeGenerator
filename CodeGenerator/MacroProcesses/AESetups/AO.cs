using CodeGenerator.MacroProcesses.AESetups;
using CodeGenerator.ProblemHandler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CgenMin.MacroProcesses
{


    public enum StyleOfSPB
    {
        // look at file AESPBObservor.cpp at funciton _RefreshCheckStyle() for where this implementation makes a difference. 
        EachSPBTask,  //each spb has its own task that it will use to execute its refresh
        ChainOfSPBsTask, // there is one "chain task" that will run all refreshs
                         //there are no tasks involved and so everything is run within the interrupt(although doesnt need to be an interrupt, can also just be from a normal tick() of a clock).
                         //Currently this doesnt look to be any different than the 
                         //ChainOfSPBsTask so maybe use this for now intead of that one. Remeber that if you do infact use this in an interrupt, it should be a VERY quick spb
        ChainOfSPBsFromInterrupt

    }
;

    public enum AOTypeEnum
    {
        Sensor,
        Filter,
        SPB, 
        UtilityService,
        Clock,
        Event,
        LoopObject,
        SimpleFSM,
        AEHal
    }


    public enum AEPriorities 
    {
        LowPriority = 0,
        MediumPriority = 1,
        HighPriority = 2
    }

    public enum SensorResolution
    {
        Resolution0Bit = 0,
        Resolution8Bit = 8,
        Resolution12Bit = 12,
        Resolution16Bit = 16,
        Resolution32Bit = 32,
        Resolution64Bit = 64

    };

    public enum AEClock_PrescalerEnum
    {
        PRESCALER1 = 1,
        PRESCALER2 = 2,
        PRESCALER4 = 4,
        PRESCALER8 = 8,
        PRESCALER16 = 16,
        PRESCALER32 = 32,
        PRESCALER64 = 64

    };

    public enum LinkTypeEnum
    {
        Copy,       // all data is copied from the linked AO to this spb's channel
        Reference   // all data is not copied but instead a reference is passed. do this if you dont intend on changing the data that is passed in.
    }

    [System.AttributeUsage(System.AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public class AEEXETest : System.Attribute
    {
        public AEConfig AEconfigToUse { get; }
        public AEEXETest(int aEconfigTICK_RATE_HZ = 1000,
            int aEconfigMINIMAL_STACK_SIZE = 928,
            int aEconfigTIMER_TASK_STACK_DEPTH = 1500,
            int aEconfigTOTAL_HEAP_SIZE = 75000,
            bool dontCheckForHardDeadlinesInSPBsForEverySetInput = true)
        { 
            AEconfigToUse = new AEConfig(
                aEconfigTICK_RATE_HZ  ,
              aEconfigMINIMAL_STACK_SIZE ,
              aEconfigTIMER_TASK_STACK_DEPTH ,
              aEconfigTOTAL_HEAP_SIZE ,
              dontCheckForHardDeadlinesInSPBsForEverySetInput)   ;
        }

    }




    public abstract class AO
    {
         
        private static  List<string> listOfAdditionalIncludes = new List<string>();
        protected string GetAdditionalIncludeInAEConfig(string fileNameWithoutTheExt)
        {
            //return only if this file has not been included yet
            var alreadyIncluded = listOfAdditionalIncludes.Where(a => a == fileNameWithoutTheExt).FirstOrDefault();

            if (alreadyIncluded == null)
            {
                listOfAdditionalIncludes.Add(fileNameWithoutTheExt);

                return $"#define AnyOtherNeededIncludes{listOfAdditionalIncludes.Count.ToString()} {fileNameWithoutTheExt} \n";
            }
            else
            {
                return "";
            }

        }


        public static List<AO> AllInstancesOfAO = new List<AO>(); 
        public AOTypeEnum AOType { get; protected set; }
        public string ClassName;
        public string InstanceName;

        public AO(string instanceName, AOTypeEnum aOType)
        {
            ClassName = GetType().Name;

            InstanceName = instanceName.Trim(); 
            AOType = aOType;

            if (this.ClassName != "UpdateEVT")
            {
                AllInstancesOfAO.Add(this);
            }
        }

        public static string GetStyleOfSPBStr(StyleOfSPB styleOfSPB)
        {
            string ret = 
                styleOfSPB == StyleOfSPB.ChainOfSPBsFromInterrupt ? "ChainOfSPBsFromInterrupt" :
                styleOfSPB == StyleOfSPB.ChainOfSPBsTask ? "ChainOfSPBsTask" :
                styleOfSPB == StyleOfSPB.EachSPBTask ? "EachSPBTask" : ""
                ;

            return ret; 
        }

        public static List<T> GetAllAOOfType<T>() where T : AO
        {
            if (typeof(T) == typeof(AESensor))
            {
                List<T> tt = AllInstancesOfAO.Where(a => a.AOType == AOTypeEnum.Sensor).Cast<T>().ToList();
                return tt;
            }
            if (typeof(T) == typeof(AEFilter))
            {
                List<T> tt = AllInstancesOfAO.Where(a => a.AOType == AOTypeEnum.Filter).Cast<T>().ToList();
                return tt;
            }
            if (typeof(T) == typeof(AESPBBase))
            {
                List<T> tt = AllInstancesOfAO.Where(a => a.AOType == AOTypeEnum.SPB).Cast<T>().ToList();
                return tt;
            }
            if (typeof(T) == typeof(AEUtilityService))
            {
                List<T> tt = AllInstancesOfAO.Where(a => a.AOType == AOTypeEnum.UtilityService).Cast<T>().ToList();
                return tt;
            }

            return null;
        }


        //the part that goes into the header of the main.cpp file. stuff like callback function declarations static void clockTimer1(TimerHandle_t xTimerHandle);
        public abstract string GenerateMainHeaderSection();
        //create instances of all AOs and initialize them
        public abstract string GenerateMainInitializeSection();
        //set AO to clocks section
        public abstract string GenerateMainClockSetupsSection();
        //link AOs together section
        public abstract string GenerateMainLinkSetupsSection();

        //bottom area of main file for defining callback functions.
        public abstract string GenerateFunctionDefinesSection();


        public static string All_GenerateMainHeaderSection()
        {
            string ret = "";
            foreach (var ao in AllInstancesOfAO)
            {
                ret += ao.GenerateMainHeaderSection();
            }

            return ret;
        } 
        public static string All_GenerateMainInitializeSection()
        {
            string ret = "";

            foreach (var ao in AllInstancesOfAO)
            {
                ret += ao.GenerateMainInitializeSection() + "\n";
            } 
            return ret;
        } 
        public static string All_GenerateMainClockSetupsSection()
        {
            string ret = "";
            foreach (var ao in AllInstancesOfAO)
            {
                ret += ao.GenerateMainClockSetupsSection();
            } 
            return ret;
        } 
        public static string All_GenerateMainLinkSetupsSection()
        {
            string ret = "";
            foreach (var ao in AllInstancesOfAO)
            {
                ret += ao.GenerateMainLinkSetupsSection();
            }
            return ret;
        }
         
        public static string All_GenerateFunctionDefinesSection()
        {
            string ret = "";
            foreach (var ao in AllInstancesOfAO)
            {
                ret += ao.GenerateFunctionDefinesSection();
            }
            return ret;
        }

        public static string All_GenerateAEConfigSection()
        {
            string ret = "";
            foreach (var ao in AllInstancesOfAO)
            {
                ret += ao.GenerateAEConfigSection();
            }
            return ret;
        }



        public  static int numOfAOSoFarAEConfigGenerated { get { return _numOfAOSoFarAEConfigGenerated; } protected set { _numOfAOSoFarAEConfigGenerated = value; } }
        protected static int _numOfAOSoFarAEConfigGenerated = 0;

        public bool IsGeneratedConfg { get { return isGeneratedConfg; } protected set { isGeneratedConfg = value; } }
        public bool isGeneratedConfg = false;

        //the part that goes in the AEConfig. the defines and such
        protected abstract string _GenerateAEConfigSection(int numOfAOOfThisSameTypeGeneratesAlready);

        public string GenerateAEConfigSection()
        {
            //if (this.AOType == AOType.SPB)
            //{
            //    var t = this.GetAllAOOfType<AESPBBase>();
            
            //var tt = (IPartOfAEDefines)this;
            var allSameType = AllInstancesOfAO.Where(d => d.ClassName == this.ClassName);// && tt.GetFullTemplateArgs() == ((IPartOfAEDefines)d).GetFullTemplateArgs());
            int numAlreadyCreated = 0;

            foreach (var same in allSameType)
            {
                if (same.IsGeneratedConfg == true)
                {
                    numAlreadyCreated++;
                }
            }

            //}
             

            if (numAlreadyCreated == 0)
            {
                //increment numOfAOSoFarAEConfigGenerated as this is the start of a new AOcponfig generated
                //only do this for spbs, utilityServices, or AOSM
                if (AOType == AOTypeEnum.SPB || AOType == AOTypeEnum.UtilityService)
                {
                     numOfAOSoFarAEConfigGenerated++; 
                }
                

                //ret += "#define AOInclude1 AverageSPB"; ret += "\n";
                //ret += "#define TemplateToAO1 template<bool isSubscribable,  uint32_t CHANNELCOUNTBUFFER1, TEMPLATESPB_Filters>"; ret += "\n";
                //ret += "#define ClassNameOfAO1 AverageSPB"; ret += "\n";
            }
            else
            {
                //ret += "#define TypeOfAO1_1 AverageSPB<false, 10>"; ret += "\n";
                //ret += "#define InstanceNameOfAO1_1 averageSPB"; ret += "\n";
            }



            isGeneratedConfg = true;
            return _GenerateAEConfigSection(numAlreadyCreated) + "\n";
           
             
        }
    }



    public interface IPartOfAEDefines
    {
        string GetFullTemplateType();
        string GetFullTemplateArgsValues();
    }

}
