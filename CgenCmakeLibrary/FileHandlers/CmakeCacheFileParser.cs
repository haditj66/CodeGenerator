using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

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

        public static string CompressString(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            var memoryStream = new MemoryStream();
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                gZipStream.Write(buffer, 0, buffer.Length);
            }

            memoryStream.Position = 0;

            var compressedData = new byte[memoryStream.Length];
            memoryStream.Read(compressedData, 0, compressedData.Length);

            var gZipBuffer = new byte[compressedData.Length + 4];
            Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
            return Convert.ToBase64String(gZipBuffer);
        }

        ///use this constructor if the directory for the original cgen directory is not the same as the current directory. In this case, create a guid string
        ///from the new directory, and append that to the new cgenCmakeCache file name. example cgenCmakeCache_guid.cmake
        public CmakeCacheFileParser(DirectoryInfo dir, string newDir) 
        {
            string nameAppended = newDir.Replace(@"/", ""); 
            nameAppended = nameAppended.Replace(@"\\", "");
            nameAppended = nameAppended.Replace(@":", "");
            //nameAppended = CompressString(nameAppended);  

            _init(dir, $"cgenCmakeCache_{nameAppended}.cmake");

            // I need to append an include() to this cgencache.cmake file to the original cgencache.cmake because now that one depends on this one.
            //Read the contents of the original cgencache file
            string originalcgencmake = Path.GetDirectoryName( FullFilePath ) + @"\\cgenCmakeCache.cmake";
            string contentsOriginal = File.ReadAllText(originalcgencmake); 
            string includeStatement = $"include(\"{Path.Combine(Path.GetDirectoryName(FullFilePath), this.FullFileName)}\")";
            includeStatement = includeStatement.Replace(@"\", @"/");

            if (contentsOriginal.Contains(includeStatement) == false)
            {
                contentsOriginal = includeStatement+ "\n\n" + contentsOriginal;
                File.WriteAllText(originalcgencmake, contentsOriginal);
            } 
        }

        public void WriteOptionsToFile(List<OptionsSelected> optionsToWrite)
        {
            //first remove all contents EXCEPT for the includes lines
            string contentsForIncludes = GetContents();
            var contentsForIncludesLines = contentsForIncludes.Split("\n");
            List<string> Allincludes = new List<string>();
            foreach (var includeline in contentsForIncludesLines)
            {
                if (includeline.Contains("include("))
                {
                    Allincludes.Add(includeline);
                }
                 
            } 
            RemoveContents();
            foreach (var includeLineFromBefore in Allincludes)
            {
                File.AppendAllText(FullFilePath, includeLineFromBefore + "\n\n");
            }
            


            string allOptions = "add_compile_definitions(CGEN_ALLOPTIONS=\"";
            foreach (var opt in optionsToWrite)
            {
                //File.AppendAllText(FullFilePath, "set(" + opt.option.Name + " " + opt.possibleValueSelection + ")\nadd_compile_definitions(" + opt.option.Name + " \"${" + opt.option.Name + "}\")\n");
                File.AppendAllText(FullFilePath, "set(" + opt.option.Name + " " + opt.possibleValueSelection + ")\nadd_compile_definitions(" + opt.option.Name + "__${" + opt.option.Name + "})\n");

                //if this is the INTEGRATION_TESTS option, do some extra stuff

                if (opt.option.Name.Contains("INTEGRATION_TESTS_FOR_") )
                { File.AppendAllText(FullFilePath, "add_compile_definitions(INTEGRATION_TEST_CHOSEN_SPECIFIC=\"" + opt.possibleValueSelection + "\")\n");
                }


                if (opt.option.Name == "INTEGRATION_TESTS")// || opt.option.Name.Contains("INTEGRATION_TESTS_FOR_"))
                {
                    var optsInteg = optionsToWrite.Where(o => o.option.Name == "INTEGRATION_TESTS").First();

                    File.AppendAllText(FullFilePath, "add_compile_definitions(INTEGRATION_TEST_CHOSEN=\"" + optsInteg.possibleValueSelection + "\")\n");
                    //File.AppendAllText(FullFilePath, "add_compile_definitions(AEITEST_"+ optsInteg.possibleValueSelection + "=_AEITEST(testName, thingToAssert, AssertionMessage) )\n");
                    string pathToIntegrationTestFile = Path.Combine(DirectoryOfFile.FullName, "IntegrationTestMacros.h");
                    File.WriteAllText(pathToIntegrationTestFile, "#pragma once \n");
                    foreach (var pv in optsInteg.option.MyPossibleValues)
                    {
                        if (pv.Name == optsInteg.possibleValueSelection)
                        {
                            //write the macro that includes the implementation 
                            //File.AppendAllText(pathToIntegrationTestFile, "#if BUILD_TESTS__TRUE\n");
                            //File.AppendAllText(pathToIntegrationTestFile, "//** <h4>integration test enders: will end the test in various ways</h4>*// \n");
                            //File.AppendAllText(pathToIntegrationTestFile, "//** the following will end the test after a certain time in milli has passed*// \n");
                            //File.AppendAllText(pathToIntegrationTestFile, "#define AEITEST_END_TestsAfterTimer_" + pv.Name + "(timeInMilliBeforeEnd) _AEITEST_END_TestsAfterTimer(timeInMilliBeforeEnd)\n");
                            //File.AppendAllText(pathToIntegrationTestFile, "#define AEITEST_" + pv.Name + "(testName, thingToAssert, AssertionMessage) _AEITEST(testName, thingToAssert, AssertionMessage,0,1) \n");
                            //File.AppendAllText(pathToIntegrationTestFile, "#define AEITEST_" + pv.Name + "_MUST_PASS_THIS_MANY(testName, thingToAssert, AssertionMessage, passedOnlyAfterThisManyPasses) _AEITEST(testName, thingToAssert, AssertionMessage, 0,passedOnlyAfterThisManyPasses) \n");
                            //File.AppendAllText(pathToIntegrationTestFile, "#define AEITEST_" + pv.Name + "_IgnoreFirstTests(testName, thingToAssert, AssertionMessage, ignoreFirstRunsNum) _AEITEST(testName, thingToAssert, AssertionMessage, ignoreFirstRunsNum,1) \n");
                            //File.AppendAllText(pathToIntegrationTestFile, "#define AEITEST_EndTestsIfFalseAssertion_" + pv.Name + "(testName, thingToAssert, AssertionMessage) _AEITEST_EndTestsIfFalseAssertion(testName, thingToAssert, AssertionMessage) \n");
                            //File.AppendAllText(pathToIntegrationTestFile, "#define AEITEST_END_" + pv.Name + " _AEITEST_END\n");
                            //File.AppendAllText(pathToIntegrationTestFile, "#define AEITEST_EXPECT_TEST_TO_RUN_" + pv.Name + "(testName) _AEITEST_EXPECT_TEST_TO_RUN(testName)\n");
                            //File.AppendAllText(pathToIntegrationTestFile, "#else\n");
                            //File.AppendAllText(pathToIntegrationTestFile, "#define AEITEST_END_TestsAfterTimer_" + pv.Name + "(timeInMilliBeforeEnd) \n");
                            //File.AppendAllText(pathToIntegrationTestFile, "#define AEITEST_" + pv.Name + "(testName, thingToAssert, AssertionMessage) \n");
                            //File.AppendAllText(pathToIntegrationTestFile, "#define AEITEST_" + pv.Name + "_MUST_PASS_THIS_MANY(testName, thingToAssert, AssertionMessage, passedOnlyAfterThisManyPasses) \n");
                            //File.AppendAllText(pathToIntegrationTestFile, "#define AEITEST_" + pv.Name + "_IgnoreFirstTests(testName, thingToAssert, AssertionMessage, ignoreFirstRunsNum) \n");
                            //File.AppendAllText(pathToIntegrationTestFile, "#define AEITEST_EndTestsIfFalseAssertion_" + pv.Name + "(testName, thingToAssert, AssertionMessage) \n");
                            //File.AppendAllText(pathToIntegrationTestFile, "#define AEITEST_END_" + pv.Name + " \n");
                            //File.AppendAllText(pathToIntegrationTestFile, "#define AEITEST_EXPECT_TEST_TO_RUN_" + pv.Name + "(testName) \n");
                            //File.AppendAllText(pathToIntegrationTestFile, "#endif\n");
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


