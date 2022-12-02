using CgenMin.MacroProcesses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.MacroProcesses.AESetups
{
    public class AEConfig
    {


        public AEConfig(
            int aEconfigTICK_RATE_HZ = 1000,
            int aEconfigMINIMAL_STACK_SIZE = 928,
            int aEconfigTIMER_TASK_STACK_DEPTH = 1500,
            int aEconfigTOTAL_HEAP_SIZE = 56360,
            bool dontCheckForHardDeadlinesInSPBsForEverySetInput = true
            )
        {
            AEconfigTICK_RATE_HZ = aEconfigTICK_RATE_HZ;
            AEconfigMINIMAL_STACK_SIZE = aEconfigMINIMAL_STACK_SIZE;
            AEconfigTIMER_TASK_STACK_DEPTH = aEconfigTIMER_TASK_STACK_DEPTH;
            AEconfigTOTAL_HEAP_SIZE = aEconfigTOTAL_HEAP_SIZE;
            DontCheckForHardDeadlinesInSPBsForEverySetInput = dontCheckForHardDeadlinesInSPBsForEverySetInput;


        }




        public string GetFileDefineContents()
        {


            Type type = typeof(AEConfig);
            Dictionary<string, object> properties = new Dictionary<string, object>();
            var allprops = type.GetProperties();
            var allfields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            var allfieldsint = allfields.Where(a => a.FieldType == typeof(int));
            var allfieldsbool = allfields.Where(a => a.FieldType == typeof(bool));


            string ret = "";

            foreach (FieldInfo f in allfieldsint)
            {
                ret += $"#define {f.Name} {f.GetValue(this).ToString()}"; ret += "\n";
            }
            foreach (FieldInfo f in allfieldsbool)
            {
                if ((bool)f.GetValue(this))
                {
                    ret += $"#define {f.Name}"; ret += "\n";
                }

            }


            return ret;


        }

        public void GenerateFile(string RunningProjectDir, AEInitializing aein, string AODefines,  string AEHalDefines)
        {




            //these are objects that are considered AOs
            //--utilities (not the services! a utility is considered ONE AO despite how many services it has)
            //--spb
            //--aesensor
            //--AELoopObject
            //--simpleFSM 
            NUMOFACTIVEOBJECTS = 0;
            NUMOFACTIVEOBJECTS += AO.AllInstancesOfAO.Count(a => a.AOType == AOTypeEnum.UtilityService);
            NUMOFACTIVEOBJECTS += AO.AllInstancesOfAO.Count(a => a.AOType == AOTypeEnum.SPB);
            NUMOFACTIVEOBJECTS += AO.AllInstancesOfAO.Count(a => a.AOType == AOTypeEnum.Sensor);
            NUMOFACTIVEOBJECTS += AO.AllInstancesOfAO.Count(a => a.AOType == AOTypeEnum.LoopObject);
            NUMOFACTIVEOBJECTS += AO.AllInstancesOfAO.Count(a => a.AOType == AOTypeEnum.SimpleFSM);
            NUMOFACTIVEOBJECTS++;

            HIGHEST_NUM_OF_EVT_INSTANCES = AEEvent.NumOfEventsCreatedSoFar + 1;

            //--spb
            //--aesensor 
            MAXNUMOFOBSERVORS = 0;
            MAXNUMOFOBSERVORS += AO.AllInstancesOfAO.Count(a => a.AOType == AOTypeEnum.SPB);
            MAXNUMOFOBSERVORS += AO.AllInstancesOfAO.Count(a => a.AOType == AOTypeEnum.Sensor);
            MAXNUMOFOBSERVORS++;

            MAXNUMOFSENSORS = 0;
            MAXNUMOFSENSORS += AO.AllInstancesOfAO.Count(a => a.AOType == AOTypeEnum.SPB);

            MAXNUMOFSENSORS = 0;
            MAXNUMOFSENSORS += AO.AllInstancesOfAO.Count(a => a.AOType == AOTypeEnum.Sensor);
            MAXNUMOFSENSORS++;
             

            var tt = AOObserver.AllInstancesOfAO.Where(a => a.AOType == AOTypeEnum.Sensor ||
            a.AOType == AOTypeEnum.SPB).Cast<AOObserver>().ToList();
            MAXNUMBEROF_FILTERS = tt.Count == 0 ? 1 : tt.Max(a => a.FiltersIflowTo.Count);

            var ttt = AOObserver.AllInstancesOfAO.Where(a => a.AOType == AOTypeEnum.Sensor)
                .Cast<AOObserver>().ToList();
            MAXNUMBEROF_FILTERS_From_A_Sensor = ttt.Count == 0 ? 1 : ttt.Max(a => a.FiltersIflowTo.Count);


            MaxNumOfAELoops += AO.AllInstancesOfAO.Count(a => a.AOType == AOTypeEnum.LoopObject);
            MaxNumOfAELoops = MaxNumOfAELoops == 0 ? 1 : MaxNumOfAELoops;

            var allspb = AO.AllInstancesOfAO.Where(a => a.AOType == AOTypeEnum.SPB).Cast<AESPBBase>().ToList();
            MAXNUMBEROFINPUTSIGNALS_TO_A_SPB = allspb.Count == 0 ? 1 : allspb.Max(a => a.Channels.Count);

            MAXNUMBEROFOUTPUTSIGNALS_TO_A_SPB = MAXNUMBEROFINPUTSIGNALS_TO_A_SPB;

            MAXNUM_OF_SUBSCRIBERS_To_A_SPB = 3; //TODO this one.
            MAXNUM_OF_AE_SUBSCRIPTIONS_To_SPBs = 3; //TODO this one. 

            TOTALMAXNUMBEROFOUTPUTSIGNALS_TO_ALL_SPBs = 25;//not mentioned

            MAXNUMOFTDUSSETTOTheSameSPB = 3;//not mentioned

            SPB_OF_FILTER1_SUBSCRIBED = true; //TODO this one.
            SPB_OF_FILTER2_SUBSCRIBED = true; //TODO this one.
            SPB_OF_FILTER3_SUBSCRIBED = false; //TODO this one.
            SPB_OF_FILTER4_SUBSCRIBED = false; //TODO this one.
            SPB_OF_FILTER5_SUBSCRIBED = false; //TODO this one.

            configAE_USE_TDUs_AsService = 1;
            configAE_USE_U_AsService = 1;
            configAE_USE_DDSM_AsService = 0;




            string aeConfigOUT = aein.GenerateFileOut("AERTOS\\AEConfig",
                new MacroVar() { MacroName = "AODefines", VariableValue = AODefines },
                new MacroVar() { MacroName = "AESettingsDefines", VariableValue = GetFileDefineContents() },
                new MacroVar() { MacroName = "AEHalDefines", VariableValue = AEHalDefines }
                );

            Console.WriteLine($"generating AEConfig.h ");
            aein.WriteFileContentsToFullPath(aeConfigOUT, Path.Combine(RunningProjectDir, "conf", "AEConfig.h"), "h", true);

        }


        public bool DontCheckForHardDeadlinesInSPBsForEverySetInput = true;
        public bool AEDontCheckForCorrectActionRequestTemplate = true;
        public bool Target_stm32f4 = true;

        //RTOS configs
        public int AEconfigTICK_RATE_HZ;
        public int AEconfigMINIMAL_STACK_SIZE;
        public int AEconfigTIMER_TASK_STACK_DEPTH;
        public int AEconfigTOTAL_HEAP_SIZE;

        //AE configs
        private int AOPRIORITYLOWEST = 5;
        private int AOPRIORITYMEDIUM = 10;
        private int AOPRIORITYHIGHEST = 29;
        private int NUMOFACTIVEOBJECTS;
        private int HIGHEST_NUM_OF_EVT_INSTANCES;


        private int MAXSPB_CHAIN_POOLSIZE = 5;
        private int MAXNUMOFINTERPRETORS = 3;
        private int MAXNUMOFOBSERVERINFLUENCES = 2;
        private int MAXNUMOFOBSERVORS;
        private int MAXNUMOFSENSORS;


        private int MAXNUMBEROF_FILTERS;
        private int MAXNUMBEROF_FILTERS_From_A_Sensor;

        private int MaxNumOfAELoops;

        private int MAXNUMBEROFINPUTSIGNALS_TO_A_SPB;
        private int MAXNUMBEROFOUTPUTSIGNALS_TO_A_SPB;

        private int MAXNUM_OF_SUBSCRIBERS_To_A_SPB;
        private int MAXNUM_OF_AE_SUBSCRIPTIONS_To_SPBs;

        private int TOTALMAXNUMBEROFOUTPUTSIGNALS_TO_ALL_SPBs;

        private int MAXNUMOFTDUSSETTOTheSameSPB;
        private bool SPB_OF_FILTER1_SUBSCRIBED;
        private bool SPB_OF_FILTER2_SUBSCRIBED;
        private bool SPB_OF_FILTER3_SUBSCRIBED;
        private bool SPB_OF_FILTER4_SUBSCRIBED;
        private bool SPB_OF_FILTER5_SUBSCRIBED;

        private int configAE_USE_TDUs_AsService;
        private int configAE_USE_U_AsService;
        private int configAE_USE_DDSM_AsService;
    }
}




/*
 
#define DontCheckForHardDeadlinesInSPBsForEverySetInput
#define AEDontCheckForCorrectActionRequestTemplate
 
//default on
#define Target_stm32f4 

//RTOS configs
#define AEconfigTICK_RATE_HZ 1000
#define AEconfigMINIMAL_STACK_SIZE 928
#define AEconfigTIMER_TASK_STACK_DEPTH 1500
#define AEconfigTOTAL_HEAP_SIZE 56360

//AE configs
#define AOPRIORITYLOWEST 5
#define AOPRIORITYMEDIUM 10
#define AOPRIORITYHIGHEST 29
#define NUMOFACTIVEOBJECTS 20
#define HIGHEST_NUM_OF_EVT_INSTANCES 25

//#define MAXSPB_CHAIN_POOLSIZE 5
#define MAXNUMOFOBSERVORS 30
#define MAXNUMOFINTERPRETORS 15
#define MAXNUMOFSPB 25
#define MAXNUMOFSENSORS 15
#define MAXNUMOFOBSERVERINFLUENCES 25

#define MAXNUMBEROF_FILTERS 5
#define MAXNUMBEROF_FILTERS_From_A_Sensor 2

#define MaxNumOfAELoops 2

#define MAXNUMBEROFINPUTSIGNALS_TO_A_SPB 10
#define MAXNUMBEROFOUTPUTSIGNALS_TO_A_SPB 4

#define MAXNUM_OF_SUBSCRIBERS_To_A_SPB 3
#define MAXNUM_OF_AE_SUBSCRIPTIONS_To_SPBs 3

#define TOTALMAXNUMBEROFOUTPUTSIGNALS_TO_ALL_SPBs 25

#define MAXNUMOFTDUSSETTOTheSameSPB 3
#define SPB_OF_FILTER1_SUBSCRIBED




//#define Config_Check_Build_RTTI
//#define Runtime_Build

//#ifdef TESTWITH_ACTIONREQUEST2
#define configAE_USE_TDUs_AsService  1
#define configAE_USE_U_AsService  1
//#endif
#define configAE_USE_DDSM_AsService  0

 */