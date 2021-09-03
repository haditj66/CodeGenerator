using CgenCmakeLibrary;
using CgenCmakeLibrary.FileHandlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System;

namespace CgenCmakeTests
{
    [TestClass]
    public class UnitTest1
    {


        //###########################
        //options  tests
        //###########################
        [TestMethod]
        public void create_option()
        {
            AllOptions.Instance.ClearAllOptions();

            Option op = new Option("MyOption");

            Assert.IsTrue(op.Name == "MyOption");

        }

        [TestMethod]
        public void no_duplicate_options()
        {
            AllOptions.Instance.ClearAllOptions();

            AllOptions.Instance.GetOption("MyOption");
            AllOptions.Instance.SetOption(new Option("MyOption"));
            AllOptions.Instance.SetOption(new Option("MyOption"));

            Assert.IsTrue(AllOptions.Instance.GetOptionsLength() == 1);

        }


        [TestMethod]
        public void update_option_extra_possiblevalue()
        {
            AllOptions.Instance.ClearAllOptions();
            Option opt = new Option("MyOption");
            opt.AddPossibleValue("pv1");
            AllOptions.Instance.SetOption(opt);

            Option optdup = new Option("MyOption");
            optdup.AddPossibleValue("pv2");
            optdup.Description = "hi";
            AllOptions.Instance.SetOption(optdup);

            Assert.IsTrue(AllOptions.Instance.GetOption("MyOption")
                .MyPossibleValues.Exists(pv=>pv.Name == "pv2"));

            Assert.IsTrue(AllOptions.Instance.GetOption("MyOption").Description == "hi");

        }

        [TestMethod]
        public void update_option_extra_possiblevalue2()
        {
            AllOptions.Instance.ClearAllOptions();
            Option opt = new Option("MyOption");
            opt.AddPossibleValue("pv1");
            opt.AddPossibleValue("pv2");
            AllOptions.Instance.SetOption(opt);

            Option optdup = new Option("MyOption");  
            optdup.Description = "hi";
            AllOptions.Instance.SetOption(optdup);

            Assert.IsTrue(AllOptions.Instance.GetOption("MyOption")
                .MyPossibleValues.Exists(pv => pv.Name == "pv2"));

            Assert.IsTrue(AllOptions.Instance.GetOption("MyOption").Description == "hi");

        }

        [TestMethod]
        public void update_option_extra_ConstricOption()
        {
            AllOptions.Instance.ClearAllOptions();

            Option optToConst = new Option("OptToConst");
            optToConst.AddPossibleValue("pvconst1");
            optToConst.AddPossibleValue("pvconst2");
            optToConst.AddPossibleValue("pvconst3");
            optToConst.AddPossibleValue("pvconst4");

            Option opt = new Option("MyOption");
            opt.AddPossibleValue("pv1");
            opt.AddPossibleValue("pv2");
            opt.AddConstrictedOption(optToConst,"pv1", "pvconst1");  
            AllOptions.Instance.SetOption(opt);

            Option optdup = new Option("MyOption");
            optdup.AddPossibleValue("pv2");
            optdup.AddPossibleValue("pv3");
            optdup.Description = "hi";
            optdup.AddConstrictedOption(optToConst, "pv2", "pvconst2");
            optdup.AddConstrictedOption(optToConst, "pv3", "pvconst3");
            AllOptions.Instance.SetOption(optdup);

            Assert.IsTrue(AllOptions.Instance.GetOption("MyOption")
                .GetPossibleValue("pv1").IsConstrictedOptionExists(optToConst, "pvconst1"));

            Assert.IsTrue(AllOptions.Instance.GetOption("MyOption")
    .GetPossibleValue("pv2").IsConstrictedOptionExists(optToConst, "pvconst2"));
             
            Assert.IsTrue(AllOptions.Instance.GetOption("MyOption")
    .GetPossibleValue("pv3").IsConstrictedOptionExists(optToConst, "pvconst3"));

            Assert.IsTrue(AllOptions.Instance.GetOption("MyOption").Description == "hi");

        }





        [TestMethod]
        public void get_allowable_values_of_constricted_option()
        {
            AllOptions.Instance.ClearAllOptions();

            Option optToConst = new Option("OptToConst");
            optToConst.AddPossibleValue("pvconst1");
            optToConst.AddPossibleValue("pvconst2");
            optToConst.AddPossibleValue("pvconst3");
            optToConst.AddPossibleValue("pvconst4");
            AllOptions.Instance.SetOption(optToConst);

            Option opt = new Option("MyOption");
            opt.AddPossibleValue("pv1");
            opt.AddPossibleValue("pv2");
            opt.AddConstrictedOption(optToConst, "pv1", "pvconst1");
            opt.AddConstrictedOption(optToConst, "pv1", "pvconst3");
            opt.AddConstrictedOption(optToConst, "pv2", "pvconst2");
            AllOptions.Instance.SetOption(opt);
             

            List<OptionsSelected> optSel = new List<OptionsSelected>();
            optSel.Add(new OptionsSelected() { option = opt, possibleValueSelection = "pv1" });

            List<ConstrictionInfo> cOpts = new List<ConstrictionInfo>();
            List<PossibleValue> allowablevalues = AllOptions.GetAllowablePVs(optToConst, optSel, ref cOpts);


            Assert.IsTrue(allowablevalues.Exists(av => av.Name == "pvconst1"));
            Assert.IsTrue(allowablevalues.Exists(av => av.Name == "pvconst3"));
            Assert.IsTrue(allowablevalues.Exists(av => av.Name == "pvconst2") == false);
        }



        [TestMethod]
        public void get_allowable_values_of_constricted_option2()
        {
            SavedOptionsFileHandler saveoptFile = new SavedOptionsFileHandler(new DirectoryInfo(
                Environment.CurrentDirectory + "\\..\\..\\..\\..\\CgenCmakeGui\\TestFiles\\GetAllowedPVTest"));

            saveoptFile.LoadAllOptions();

            var aertosEnv_option = AllOptions.Instance.GetOption("AERTOS_ENVIRONMENT");
            var Build_System_option = AllOptions.Instance.GetOption("Build_System");
            var RTOS_USED_option = AllOptions.Instance.GetOption("RTOS_USED");
            var OS_USED_option = AllOptions.Instance.GetOption("OS_USED");
            var SIM_REAL_option = AllOptions.Instance.GetOption("SIM_REAL");
            var SWIL_HWIL_DRIVEN_option = AllOptions.Instance.GetOption("SWIL_HWIL_DRIVEN");
            var BOARD_USED_option = AllOptions.Instance.GetOption("BOARD_USED");
            var TOOLCHAIN_USED_option = AllOptions.Instance.GetOption("TOOLCHAIN_USED");

            List<OptionsSelected> selOpts = new List<OptionsSelected>();
            selOpts.Add(new OptionsSelected() { option = aertosEnv_option, possibleValueSelection = "RTOS_MICRO" }) ;

            List<ConstrictionInfo> cOpt = new List<ConstrictionInfo>();
            List<PossibleValue> allowablevalues = AllOptions.GetAllowablePVs(Build_System_option, selOpts, ref cOpt);

            Assert.IsTrue(allowablevalues.Count == 1);
            Assert.IsTrue(allowablevalues.Exists(av => av.Name == "VSGDBCmake_Ninja"));


            selOpts.Add(new OptionsSelected() { option = Build_System_option, possibleValueSelection = "VSGDBCmake_Ninja" });
            List<ConstrictionInfo> cOpts = new List<ConstrictionInfo>();
            allowablevalues = AllOptions.GetAllowablePVs(RTOS_USED_option, selOpts, ref cOpts);

            Assert.IsTrue(allowablevalues.Count == 2);
            Assert.IsTrue(allowablevalues.Exists(av => av.Name == "FREERTOS"));
            Assert.IsTrue(allowablevalues.Exists(av => av.Name == "NONE"));


            selOpts.Add(new OptionsSelected() { option = RTOS_USED_option, possibleValueSelection = "FREERTOS" });
            List<ConstrictionInfo> cOpts2 = new List<ConstrictionInfo>();
            allowablevalues = AllOptions.GetAllowablePVs(TOOLCHAIN_USED_option, selOpts, ref cOpts2);

            Assert.IsTrue(allowablevalues.Count == 1);
            Assert.IsTrue(allowablevalues.Exists(av => av.Name == "arm_non_eabi_id")); 

        }


        //###########################
        //NextFileParser tests
        //###########################
        [TestMethod]
        public void check_contents_are_full()
        {
            AllOptions.Instance.ClearAllOptions();

            //cgenCmakeConfigNEXT.txt
            NEXTFileParser nEXTFileParser = new NEXTFileParser(new System.IO.DirectoryInfo(@"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\CgenCmakeTests\TestFiles"));

            bool isfilled = nEXTFileParser.IsFileContentsFilled();

            Assert.IsTrue(isfilled);

        }

        [TestMethod]
        public void deserialize_Option()
        {

            AllOptions.Instance.ClearAllOptions();


            //cgenCmakeConfigNEXT.txt
            NEXTFileParser nEXTFileParser = new NEXTFileParser(new System.IO.DirectoryInfo(@"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\CgenCmakeTests\TestFiles"));




            string contents = nEXTFileParser.GetContents();

            Option.Deserialize(contents);



            Assert.IsTrue(AllOptions.Instance.OptionExists("AERTOS_ENVIRONMENT") == true);
            Assert.IsTrue(AllOptions.Instance.OptionExists("Build_System") == true);

            var buildOption = AllOptions.Instance.GetOption("Build_System");


            var option = AllOptions.Instance.GetOption("AERTOS_ENVIRONMENT");
            Assert.IsTrue(option.IsPossibleValueExists("RTOS_MICRO"));
            Assert.IsTrue(option.IsPossibleValueExists("RTOS_PC"));

            var possiblevalue = option.GetPossibleValue("RTOS_MICRO");
            Assert.IsTrue(possiblevalue.IsConstrictedOptionExists(buildOption, "VSGDBCmake_Ninja"));

            var constricOpt = possiblevalue.GetConstrictedOption("Build_System", "VSGDBCmake_Ninja");
            var constricOpt2 = possiblevalue.GetConstrictedOption("Build_System", "AmentCmake_colcon");
            Assert.IsTrue(constricOpt.IsValueConstrictedExists("VSGDBCmake_Ninja"));
            Assert.IsTrue(constricOpt2.IsValueConstrictedExists("AmentCmake_colcon"));

        }


        [TestMethod]
        public void serialize_option()
        {
            AllOptions.Instance.ClearAllOptions();

            //cgenCmakeConfigNEXT.txt
            NEXTFileParser nEXTFileParser = new NEXTFileParser(new System.IO.DirectoryInfo(@"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\CgenCmakeTests\TestFiles"));

            Option ConstOption = AllOptions.Instance.GetOptionCreateIfNotExists("Build_System");


            Option aertosenv = AllOptions.Instance.GetOptionCreateIfNotExists("AERTOS_ENVIRONMENT");
            aertosenv.Description = "what environment is this AE application set in";
            aertosenv.AddPossibleValue("RTOS_MICRO");
            aertosenv.AddPossibleValue("RTOS_PC");
             

            var pos1 = aertosenv.GetPossibleValue("RTOS_MICRO");
            var pos2 = aertosenv.GetPossibleValue("RTOS_PC");
            pos1.AddConstrictedOption(ConstOption, "AmentCmake_colcon");
            pos1.AddConstrictedOption(ConstOption, "VSGDBCmake_Ninja");
            pos2.AddConstrictedOption(ConstOption, "mingw");

            //pos1.SetConstrictedOption("Build_System");
            //var const1 = pos1.GetConstrictedOption("Build_System", "");
            //const1.AddValueConstricted("VSGDBCmake_Ninja"); const1.AddValueConstricted("AmentCmake_colcon");

            //pos2.SetConstrictedOption("Build_System");
            //var const2 = pos2.GetConstrictedOption("Build_System");
            //const2.AddValueConstricted("mingw");

            //nEXTFileParser.ParseFile();
            string ser = aertosenv.Serialize();
            string strCont = nEXTFileParser.GetContents();
            strCont = strCont.Replace("\r", "");

            Assert.IsTrue(ser == "NAME AERTOS_ENVIRONMENT\nDESCRIPTION what environment is this AE application set in\nPOSSIBLEVALUES RTOS_MICRO RTOS_PC\nCONSTRICTS_LATER_OPTIONS RTOS_MICRO@Build_System@AmentCmake_colcon;RTOS_MICRO@Build_System@VSGDBCmake_Ninja;RTOS_PC@Build_System@mingw;");

        }







        [TestMethod]
        public void grid_simple_point()
        {
            GridGenerator gg = new GridGenerator(10, 10, 1000, 1000);

            var loc = gg.GetLocation(2,2);
            var loc23 = gg.GetLocation(2, 3);

            Assert.IsTrue(loc.XLocation == 150);
            Assert.IsTrue(loc.YLocation == 150);
            Assert.IsTrue(loc23.XLocation == 250);
            Assert.IsTrue(loc23.YLocation == 150);


            var loc2 = gg.GetLocation(10, 10);

            Assert.IsTrue(loc2.XLocation == 950);
            Assert.IsTrue(loc2.YLocation == 950);


            GridGenerator gg2 = new GridGenerator(10, 10, 1800, 1000);
            var loc3 = gg2.GetLocation(1,1);

            Assert.IsTrue(loc3.XLocation == 90);
            Assert.IsTrue(loc3.YLocation == 50);
        }


        [TestMethod]
        public void grid_parent_point()
        {
            GridGenerator gg = new GridGenerator(10, 10, 1000, 1000);
            GridGenerator gg2 = gg.CreateGridFromGrid(1, 1,10,10);
            

            var loc = gg2.GetLocation(2, 2);

            Assert.IsTrue(loc.XLocation == 15);
            Assert.IsTrue(loc.YLocation == 15);

            GridGenerator gg3 = gg.CreateGridFromGrid(2, 2, 10, 10);
            var loc2 = gg3.GetLocation(1, 1);

            Assert.IsTrue(loc2.XLocation == 105);
            Assert.IsTrue(loc2.YLocation == 105);


        }


        [TestMethod]
        public void grid_multi_grids()
        {
            GridGenerator gg = new GridGenerator(10, 10, 1000, 1000);
            GridGenerator[,] grids = gg.CreateGridFromAllGrid(10, 10);

            var loc = grids[1, 1].GetLocation(2, 2); 

            Assert.IsTrue(loc.XLocation == 15);
            Assert.IsTrue(loc.YLocation == 15);

            var loc2 = grids[2, 2].GetLocation(1, 1);

            Assert.IsTrue(loc2.XLocation == 105);
            Assert.IsTrue(loc2.YLocation == 105);


        }



    }
}
