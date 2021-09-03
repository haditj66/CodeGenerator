using CgenCompileLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompileKeywords
{
    public abstract class CompileKeywordContainerBase<TCompileKeywordUnits>
        where TCompileKeywordUnits : CompileKeywordUnitBase, new()
    {
        public string Keyword { get; protected set; }
        public List<TCompileKeywordUnits> AllKeywords { get; protected set; }

        public List<TCompileKeywordUnits> AllCompileKeywordUnits { get; protected set; }


        protected CompileKeywordContainerBase()
        {
            AllKeywords = new List<TCompileKeywordUnits>();
            AllCompileKeywordUnits = new List<TCompileKeywordUnits>();
        }


        public static TCompileKeyword FactoryCompileKeyword<TCompileKeyword>()
            where TCompileKeyword : CompileKeywordContainerBase<TCompileKeywordUnits>, new()
        {
            TCompileKeyword ck = new TCompileKeyword();
            ck.Keyword = ck.GetKeywordName();



            return ck;
        }

        /// <summary>
        /// find all instances of a keyword from a PostCompileFile
        /// </summary>
        /// <param name="fromThisFile"></param>
        /// <returns>true if any keywords were found.</returns>
        public bool FindInstanceOfUnit(PostCompileFile fromThisFile)
        {
            bool anyfound = false;

            TCompileKeywordUnits theCompileKeywordUnit = new TCompileKeywordUnits();
            //go through the source file (not intermediary as it is too long) and look for any keywords used by cgen 
            var lines = fromThisFile.SourceFileContents;
            int lineNum = 1;
            foreach (var line in lines)
            {
                if (line.Contains(this.Keyword))
                {
                    theCompileKeywordUnit.AtLineNumber = lineNum;
                    theCompileKeywordUnit.KeywordExistsAtSource = true;
                    bool foundAtIntermediary = CheckIfkeywordFromSourceExistsAtIntermediaryFile(theCompileKeywordUnit, fromThisFile);
                    if (foundAtIntermediary == true)
                    {
                        theCompileKeywordUnit.KeywordExistsAtIntermediary = true;
                        theCompileKeywordUnit.ForPostCompileFile = fromThisFile;
                        anyfound = true;
                        //add that keywordUnit to the container
                        AllCompileKeywordUnits.Add(theCompileKeywordUnit);
                        break;
                    }
                }

                lineNum++;
            }

            return anyfound;
        }

        private bool CheckIfkeywordFromSourceExistsAtIntermediaryFile(TCompileKeywordUnits compileKeywordUnitToCheck, PostCompileFile fromThisFile)
        {
            //the line it was found at in the source needs to match the line according to the following naming convention
            // keyword_linenumber
            string keywordNameConvention = Keyword + "_" + compileKeywordUnitToCheck.AtLineNumber;
            bool anyfound = false;


            var lines = fromThisFile.IntermediaryFileContents;
            int lineNum = 1;
            foreach (var line in lines)
            {
                if (line.Contains(keywordNameConvention))
                {
                    anyfound = true;
                    break;
                }

            }

            return anyfound;
        }

        protected abstract string GetKeywordName();

    }




    public class WeakKeywordsContainer : CompileKeywordContainerBase<WeakKeywordUnit>
    {


        protected override string GetKeywordName()
        {
            return "cgenweak";
        }
    }


    public class WeakKeywordUnit : CompileKeywordUnitBase
    {

    }

    public class CompileKeywordUnitBase
    {
        public bool KeywordExistsAtIntermediary { get; set; }
        public bool KeywordExistsAtSource { get; set; }
        public bool KeywordFound { get { return KeywordExistsAtIntermediary && KeywordExistsAtSource; } }
        public int AtLineNumber { get; set; }
        public PostCompileFile ForPostCompileFile { get; set; }
    }

}
