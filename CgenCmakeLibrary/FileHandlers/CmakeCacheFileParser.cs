using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CgenCmakeLibrary.FileHandlers
{
    public class CmakeCacheFileParser : FileHandler
    {
        //from base
        //public bool IsFileContentsFilled(); 
        //public string GetContents();
        //public void RemoveContents();

        public CmakeCacheFileParser(DirectoryInfo dir) : base(dir, "cgenCmakeCache.cmake")
        {

        }

        public void WriteOptionsToFile(List<OptionsSelected> optionsToWrite)
        {
            //first remove all contents
            RemoveContents();

            string allOptions = "add_compile_definitions(CGEN_ALLOPTIONS=\"";
            foreach (var opt in optionsToWrite)
            {
                //File.AppendAllText(FullFilePath, "set(" + opt.option.Name + " " + opt.possibleValueSelection + ")\nadd_compile_definitions(" + opt.option.Name + " \"${" + opt.option.Name + "}\")\n");
                File.AppendAllText(FullFilePath, "set(" + opt.option.Name + " " + opt.possibleValueSelection + ")\nadd_compile_definitions(" + opt.option.Name + "__${" + opt.option.Name + "})\n");

                //if this is the INTEGRATION_TESTS option, do some extra stuff

                if (opt.option.Name.Contains("INTEGRATION_TESTS_FOR_") )
                { File.AppendAllText(FullFilePath, "add_compile_definitions(INTEGRATION_TEST_CHOSEN_SPECIFIC=\"" + opt.possibleValueSelection + "\")\n");
                }


                if (opt.option.Name == "INTEGRATION_TESTS")
                {
                    File.AppendAllText(FullFilePath, "add_compile_definitions(INTEGRATION_TEST_CHOSEN=\"" + opt.possibleValueSelection + "\")\n");
                    //File.AppendAllText(FullFilePath, "add_compile_definitions(AEITEST_"+ opt.possibleValueSelection + "=_AEITEST(testName, thingToAssert, AssertionMessage) )\n");
                    string pathToIntegrationTestFile = Path.Combine(DirectoryOfFile.FullName, "IntegrationTestMacros.h");
                    File.WriteAllText(pathToIntegrationTestFile, "#pragma once \n");
                    foreach (var pv in opt.option.MyPossibleValues)
                    {
                        if (pv.Name == opt.possibleValueSelection)
                        {
                            //write the macro that includes the implementation 
                            File.AppendAllText(pathToIntegrationTestFile, "#if BUILD_TESTS == TRUE\n");
                            File.AppendAllText(pathToIntegrationTestFile, "//** <h4>integration test enders: will end the test in various ways</h4>*// \n");
                            File.AppendAllText(pathToIntegrationTestFile, "//** the following will end the test after a certain time in milli has passed*// \n");
                            File.AppendAllText(pathToIntegrationTestFile, "#define AEITEST_END_TestsAfterTimer_" + pv.Name + "(timeInMilliBeforeEnd) _AEITEST_END_TestsAfterTimer(timeInMilliBeforeEnd)\n");
                            File.AppendAllText(pathToIntegrationTestFile, "#define AEITEST_" + pv.Name + "(testName, thingToAssert, AssertionMessage) _AEITEST(testName, thingToAssert, AssertionMessage) \n");
                            File.AppendAllText(pathToIntegrationTestFile, "#define AEITEST_EndTestsIfFalseAssertion_" + pv.Name + "(testName, thingToAssert, AssertionMessage) _AEITEST_EndTestsIfFalseAssertion(testName, thingToAssert, AssertionMessage) \n");
                            File.AppendAllText(pathToIntegrationTestFile, "#define AEITEST_END_" + pv.Name + " _AEITEST_END\n");
                            File.AppendAllText(pathToIntegrationTestFile, "#define AEITEST_EXPECT_TEST_TO_RUN_" + pv.Name + "(testName) _AEITEST_EXPECT_TEST_TO_RUN(testName)\n");
                            File.AppendAllText(pathToIntegrationTestFile, "#else\n");
                            File.AppendAllText(pathToIntegrationTestFile, "#define AEITEST_END_TestsAfterTimer_" + pv.Name + "(timeInMilliBeforeEnd) \n");
                            File.AppendAllText(pathToIntegrationTestFile, "#define AEITEST_" + pv.Name + "(testName, thingToAssert, AssertionMessage) \n");
                            File.AppendAllText(pathToIntegrationTestFile, "#define AEITEST_EndTestsIfFalseAssertion_" + pv.Name + "(testName, thingToAssert, AssertionMessage) \n");
                            File.AppendAllText(pathToIntegrationTestFile, "#define AEITEST_END_" + pv.Name + " \n");
                            File.AppendAllText(pathToIntegrationTestFile, "#define AEITEST_EXPECT_TEST_TO_RUN_" + pv.Name + "(testName) \n");
                            File.AppendAllText(pathToIntegrationTestFile, "#endif\n");
                        }
                        else
                        {
                            File.AppendAllText(pathToIntegrationTestFile, "#define AEITEST_" + pv.Name + "(testName, thingToAssert, AssertionMessage) \n");
                            File.AppendAllText(pathToIntegrationTestFile, "#define AEITEST_END_" + pv.Name + " \n");
                        }
                    }

                }


                allOptions += "@" + opt.option.Name + "::" + opt.possibleValueSelection + " ";
            }

            allOptions += "\")\n";
            File.AppendAllText(FullFilePath, allOptions);
        }

        public List<OptionsSelected> LoadOptionsSelected()
        {
            //first remove all contents
            string Contents = GetContents();

            List<OptionsSelected> retOptSel = new List<OptionsSelected>();

            using (var reader = new StringReader(Contents))
            {
                for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    if (string.IsNullOrEmpty(line) == false)
                    {
                        Regex regex = new Regex(@"set\((.*) (.*)\)");
                        Match match = regex.Match(line);
                        if (match.Success)
                        {
                            OptionsSelected optSel = new OptionsSelected();
                            string optionName = match.Groups[1].Value.Trim();
                            optSel.option = AllOptions.Instance.GetOptionCreateIfNotExists(optionName);
                            optSel.possibleValueSelection = match.Groups[2].Value.Trim();

                            retOptSel.Add(optSel);
                        }

                    }
                }
            }
            return retOptSel;

        }


    }
}


