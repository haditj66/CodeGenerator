using CgenCmakeLibrary;
using CgenCmakeLibrary.FileHandlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text.RegularExpressions;

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
            var option = AllOptions.Instance.GetOption("AERTOS_ENVIRONMENT");
            Assert.IsTrue(option.IsPossibleValueExists("RTOS_MICRO"));
            Assert.IsTrue(option.IsPossibleValueExists("RTOS_PC"));

            var possiblevalue = option.GetPossibleValue("RTOS_MICRO");
            Assert.IsTrue(possiblevalue.IsConstrictedOptionExists("Build_System"));

            var constricOpt = possiblevalue.GetConstrictedOption("Build_System");
            Assert.IsTrue(constricOpt.IsValueConstrictedExists("VSGDBCmake_Ninja"));
            Assert.IsTrue(constricOpt.IsValueConstrictedExists("AmentCmake_colcon"));

        }


        [TestMethod]
        public void serialize_option()
        {
            AllOptions.Instance.ClearAllOptions();

            //cgenCmakeConfigNEXT.txt
            NEXTFileParser nEXTFileParser = new NEXTFileParser(new System.IO.DirectoryInfo(@"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\CgenCmakeTests\TestFiles"));




            Option aertosenv = AllOptions.Instance.GetOptionCreateIfNotExists("AERTOS_ENVIRONMENT");
            aertosenv.Description = "what environment is this AE application set in";
            aertosenv.AddPossibleValue("RTOS_MICRO");
            aertosenv.AddPossibleValue("RTOS_PC");

            var pos1 = aertosenv.GetPossibleValue("RTOS_MICRO");
            var pos2 = aertosenv.GetPossibleValue("RTOS_PC");

            pos1.AddConstrictedOption("Build_System");
            var const1 = pos1.GetConstrictedOption("Build_System");
            const1.AddValueConstricted("VSGDBCmake_Ninja"); const1.AddValueConstricted("AmentCmake_colcon");

            pos2.AddConstrictedOption("Build_System");
            var const2 = pos2.GetConstrictedOption("Build_System");
            const2.AddValueConstricted("mingw");

            //nEXTFileParser.ParseFile();
            string ser = aertosenv.Serialize();
            string strCont = nEXTFileParser.GetContents();
            strCont = strCont.Replace("\r", "");

            Assert.IsTrue(strCont == ser);

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
