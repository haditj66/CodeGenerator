using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ClangSharp;
using ConsoleApp2.MyClangWrapperClasses;
using ConsoleApp2.MyClangWrapperClasses.CXCursors;
using ConsoleApp2.MyClangWrapperClasses.CXCursors.MyCursorKinds;
using ConsoleApp2.Parsing;
using CPPParser;
using MyLibClangVisitors.ConsoleApp2;

namespace ConsoleApp2.CPPRefactoring
{
    public class CppRefactorer
    {


        public List<string> FilesInScopeToRefactor { get; private set; }
        private List<MyCXTranslationUnit> TranslationUnits { get; set; }
        private List<CppParser> Parsers { get; set; }


        public CppRefactorer(string filesInScopeToRefactor) : this(new List<string>() { filesInScopeToRefactor })
        {
        }

        public CppRefactorer(List<string> filesInScopeToRefactor)
        {
            //List<CXTranslationUnit> cxTranslationUnits = filesInScopeToRefactor.GetTranslationUnitsFromAllFiles();
            //TranslationUnits = cxTranslationUnits.Select((CXTranslationUnit tu) => { return new MyCXTranslationUnit(tu); }).ToList();


            this.FilesInScopeToRefactor = filesInScopeToRefactor;
            ReloadRefactorer();
        }

        /// <summary>
        /// do this when you changed alot of things and are in need of reloading translational units
        /// </summary>
        public void ReloadRefactorer()
        {
            Parsers = new List<CppParser>();
            TranslationUnits = new List<MyCXTranslationUnit>();
            //make a cppparser for each files inscope
            foreach (var fileInScope in this.FilesInScopeToRefactor)
            {
                CppParser cppParser = new CppParser(fileInScope);
                Parsers.Add(cppParser);
                TranslationUnits.Add(cppParser.MyCxTranslationUnit);
            }

        }

        /// <summary>
        /// this will get ALL files for the directories you provide
        /// </summary>
        /// <param name="directoriesForAllFiles">the directories that have all the files you want to include</param>
        public CppRefactorer(params DirectoryInfo[] directoriesForAllFiles)
        {
            FilesInScopeToRefactor = directoriesForAllFiles
                .SelectMany((DirectoryInfo d) => { return d.GetFiles().Select((FileInfo f) => { return f.FullName; }); }).ToList();

            foreach (var fileInScope in FilesInScopeToRefactor)
            {
                CppParser cppParser = new CppParser(fileInScope);
                Parsers.Add(cppParser);
                TranslationUnits.Add(cppParser.MyCxTranslationUnit);
            }

            //List<CXTranslationUnit> cxTranslationUnits = FilesInScopeToRefactor.GetTranslationUnitsFromAllFiles();
            //TranslationUnits = cxTranslationUnits.Select((CXTranslationUnit tu) => { return new MyCXTranslationUnit(tu); }).ToList();

            this.FilesInScopeToRefactor = FilesInScopeToRefactor;
        }


        private CppParser GetParserForFile(string ofThisFileInScope)
        {
            string fullPAthOffile = GetFullFilePathFromFileNameInScope(ofThisFileInScope);
            return Parsers.Where((CppParser cpppar) => { return cpppar.MyCxTranslationUnit.myCursor.MyCxSourceLocation.MyCxFilefileStart.FileFullName == fullPAthOffile; }).First();

        }

        public string GetFullFilePathFromFileNameInScope(string ofThisFileInScope)
        {
            //make sure file is just of the filename
            ofThisFileInScope = Path.GetFileName(ofThisFileInScope);

            //assert that the file is in scope that i want to change the file name of.
            Debug.Assert(FilesInScopeToRefactor
                    .Select((string FilePAth) => { return Path.GetFileName(FilePAth); })
                    .Any((string fileNAme) => { return fileNAme == ofThisFileInScope; })
                , "file needs to be in the scope of files you gave to the constructor of CPPRefactorer. make sure file is of correct name.cpp or name.h format.");

            //get full path of file
            return FilesInScopeToRefactor
                .Where((string FilePAth) => { return Path.GetFileName(FilePAth) == ofThisFileInScope; })
                .First();
        }


        /// <summary>
        /// change file name of one of the files that are in scope. you'll also change all mentions for files in scope #include of that file.
        /// </summary>
        /// <param name="ofThisFileInScope">should be one of the files that you put into CppRefactorer constructor. of form name.cpp or name.h</param>
        public void ChangeNameOfFile(string ofThisFileInScope, string toThisNewName, bool reloadOnFinish = true)
        {

            string FullPAthOffile = GetFullFilePathFromFileNameInScope(ofThisFileInScope);

            Debug.Assert(Path.GetExtension(toThisNewName) == Path.GetExtension(ofThisFileInScope), "new and old files need to have the same extensions");

            Action<string, string> ChangeJustFileName = (string FullPAthOffileofOld, string newNAmeOfFile) =>
            {
                File.Move(FullPAthOffileofOld,
                    Path.Combine(Path.GetDirectoryName(FullPAthOffileofOld), newNAmeOfFile));
                //change that name in the FilesInScopeToRefactor

                FilesInScopeToRefactor.Remove(FullPAthOffileofOld);
                FilesInScopeToRefactor.Add(Path.Combine(Path.GetDirectoryName(FullPAthOffileofOld), newNAmeOfFile));
            };



            //if the file is a .cpp file then you do not have to do anything to the #includes and just need to change the file name
            if ((Path.GetExtension(ofThisFileInScope) == ".cpp") || (Path.GetExtension(ofThisFileInScope) == ".c"))
            {
                ChangeJustFileName(FullPAthOffile, toThisNewName);

            }
            else
            {
                //it must be a .h so make change file name
                ChangeJustFileName(FullPAthOffile, toThisNewName);


                //but now I need to go through each file and change any #include "oldName" to #include "newName"
                foreach (var fileinscope in FilesInScopeToRefactor)
                {
                    List<string> oldcontents = File.ReadAllLines(fileinscope).ToList();
                    List<string> newcontents = new List<string>(oldcontents);

                    int lineIndex = 0;
                    bool fileChanged = false;
                    foreach (var line in oldcontents)
                    {
                        Match match = Regex.Match(line, @"^\s*#include\s*""(.*)""");
                        if (match.Success)
                        {
                            string filenameInThisInclude = Path.GetFileName(match.Groups[1].Value);
                            if (filenameInThisInclude == ofThisFileInScope)
                            {
                                //than this is the line that has an include that referenced the old file. change the name to the new one
                                //newcontents[lineIndex] = Regex.Replace(line, ofThisFileInScope, toThisNewName);
                                var m = Regex.Match(line, @"#include\s*""(.*)""");
                                string pat = string.Format(@"{0}", m.Groups[1].Value.SetEscapesOnString());
                                newcontents[lineIndex] = Regex.Replace(line, pat, toThisNewName);
                                fileChanged = true;
                            }
                        }

                        lineIndex++;
                    }

                    //write those changed contents bach into the file
                    if (fileChanged)
                    {
                        File.WriteAllLines(fileinscope, newcontents);
                    }

                }
            }
            //I need to reload the parser as files have changed 
            if (reloadOnFinish)
            {
                ReloadRefactorer();
            }

        }


        /// <summary>
        /// inserts namespace into all files in scope.
        /// </summary>
        /// <param name="nameOfNamespace"></param> 
        public void InsertNamespaceIntoAllFiles(string nameOfNamespace)
        {

            string namespaceOpen = @"namespace " + nameOfNamespace + "{";
            string namespaceCLose = @"}//" + nameOfNamespace;

            //get the first instance of any cursor found in the file 
            foreach (var TranslationUnit in TranslationUnits)
            {
                //vist the children of the root translation unit cursor
                MyAllVisitor allVisitor = new MyAllVisitor();
                clang.visitChildren(TranslationUnit.myCursor.CXCursor, allVisitor.Visit, new CXClientData(IntPtr.Zero));
                //first make sure there are any actual program entities in here as the file could just be empty
                if (allVisitor.AllCursorsVisited.Count != 0)
                {
                    MyCXCursor firstCursor = allVisitor.AllCursorsVisited[0];

                    //get the line location for the first cursor.
                    uint lineOfFirstEntity = allVisitor.AllCursorsVisited[0].MyCxSourceLocation.lineStart;

                    //insert the namespace before that line
                    List<string> constentsOfFile = File.ReadAllLines(firstCursor.MyCxSourceLocation.MyCxFilefileStart.FileFullName).ToList();

                    constentsOfFile.Insert((int)lineOfFirstEntity - 1, namespaceOpen);
                    constentsOfFile.Add(namespaceCLose);

                    File.WriteAllLines(firstCursor.MyCxSourceLocation.MyCxFilefileStart.FileFullName, constentsOfFile);
                }
            }

        }

        public void ReplaceDefinesWithDefineValueFile(string file, string configContents)
        {
            string fullPAthOffile = GetFullFilePathFromFileNameInScope(file);
            string fileContents = File.ReadAllText(fullPAthOffile);
            string[] fileToReturn = fileContents.Split('\n');



            //go through each line and look for the define name
            int index = 0;
            foreach (var line in fileContents.Split('\n'))
            { 
                //go through each line in configContents and look for a #define line
                foreach (var lineconfStr in configContents.Split('\n'))
                {
                    //first make sure that that line is a define
                    //get the name and value of the defines in the configContents
                    Match m = Regex.Match(lineconfStr, @"#define(\b.+?\b)\s+(.+)$", RegexOptions.Multiline);
                    if (m.Success)
                    { 
                        string pattern = @"\b" + m.Groups[1].Value.Trim() + @"\b";
                        Match mm = Regex.Match(line, pattern);
                        if (mm.Success)
                        {
                            fileToReturn[index] = Regex.Replace(fileToReturn[index], pattern, m.Groups[2].Value.Trim());
                        }
                        

                    }
                }
                index++;
            }

            File.WriteAllLines(fullPAthOffile, fileToReturn);

        }




        // dealing with code insertion of a  file ************************************************
        //*******************************************************************************************

        #region MyRegion Insertion helpers*************************************************************
        //*******************************************************************************
        private void InsertStringAfterLine(string ofThisFileInScope, uint line, string strToInsert)
        {
            string fullPAthOffile = GetFullFilePathFromFileNameInScope(ofThisFileInScope);

            List<string> contents = File.ReadAllLines(fullPAthOffile).ToList();

            contents.Insert((int)line, strToInsert);
            File.WriteAllText(fullPAthOffile, string.Join("\n", contents));
        }

        private void InsertStringAtEndOfLine(string ofThisFileInScope, uint line, string strToInsert)
        {
            string fullPAthOffile = GetFullFilePathFromFileNameInScope(ofThisFileInScope);

            List<string> contents = File.ReadAllLines(fullPAthOffile).ToList();

            contents[(int)line - 1] += strToInsert;
            File.WriteAllText(fullPAthOffile, string.Join("\n", contents));
        }

        private void RemoveStringAtLine(string ofThisFileInScope, uint line)
        {
            string fullPAthOffile = GetFullFilePathFromFileNameInScope(ofThisFileInScope);

            List<string> contents = File.ReadAllLines(fullPAthOffile).ToList();

            contents.RemoveAt((int)line - 1);
            File.WriteAllText(fullPAthOffile, string.Join("\n", contents));
        }

        enum StartOrEndLocation
        {
            Start,
            End
        }


        private void InsertStringINSIDECXsourceLocation(string strToInsert, MyCXSourceLocation sourceLocation, StartOrEndLocation startOrEndOfSource)
        {
            int atline;
            int atcolumn;
            if (startOrEndOfSource == StartOrEndLocation.Start)
            {
                atline = (int)sourceLocation.lineStart - 1;//minus 1 to account for array indexing
                atcolumn = (int)sourceLocation.columnStart - 2;
            }
            else
            {
                atline = (int)sourceLocation.lineEnd - 1;
                atcolumn = (int)sourceLocation.columnEnd - 2;
            }

            List<string> contents = File.ReadAllLines(sourceLocation.MyCxFilefileStart.FileFullName).ToList();

            //go to the line of the contents 
            contents[atline] = contents[atline].Insert(atcolumn, strToInsert);
            File.WriteAllText(sourceLocation.MyCxFilefileStart.FileFullName, string.Join("\n", contents));
        }


        private void RemoveStringOfCXsourceLocation(MyCXSourceLocation sourceLocation)
        {
            int Fromline = (int)sourceLocation.lineStart - 1;//minus 1 to account for array indexing
            int Fromcolumn = (int)sourceLocation.columnStart - 1;

            int Toline = (int)sourceLocation.lineEnd - 1;
            int Tocolumn = (int)sourceLocation.columnEnd - 1;


            RemoveStringFromLineColumnToLineColumn(sourceLocation.MyCxFilefileStart.FileFullName, Fromline, Fromcolumn, Toline, Tocolumn);

        }

        private void RemoveStringFromENDofCXsourceToENDofCXsource(MyCXSourceLocation FromEndOfsourceLocation, MyCXSourceLocation ToEndOfsourceLocation)
        {
            int Fromline = (int)FromEndOfsourceLocation.lineEnd - 1;//minus 1 to account for array indexing
            int Fromcolumn = (int)FromEndOfsourceLocation.columnEnd - 1;

            int Toline = (int)ToEndOfsourceLocation.lineEnd - 1;
            int Tocolumn = (int)ToEndOfsourceLocation.columnEnd - 1;

            RemoveStringFromLineColumnToLineColumn(FromEndOfsourceLocation.MyCxFilefileStart.FileFullName, Fromline, Fromcolumn, Toline, Tocolumn);

        }

        private void RemoveStringFromBEGGININGofCXsourceToBEGGININGofCXsource(MyCXSourceLocation FromBEGGININGOfsourceLocation, MyCXSourceLocation ToBEGGININGOfsourceLocation)
        {
            int Fromline = (int)FromBEGGININGOfsourceLocation.lineStart - 1;//minus 1 to account for array indexing
            int Fromcolumn = (int)FromBEGGININGOfsourceLocation.columnStart - 1;

            int Toline = (int)ToBEGGININGOfsourceLocation.lineStart - 1;
            int Tocolumn = (int)ToBEGGININGOfsourceLocation.columnStart - 1;

            RemoveStringFromLineColumnToLineColumn(FromBEGGININGOfsourceLocation.MyCxFilefileStart.FileFullName, Fromline, Fromcolumn, Toline, Tocolumn);

        }

        private void RemoveStringFromLineColumnToLineColumn(string FullPathOfFile, int Fromline, int Fromcolumn, int Toline, int Tocolumn)
        {
            List<string> contents = File.ReadAllLines(FullPathOfFile).ToList();

            if (Fromline == Toline)
            {
                contents[Fromline] = contents[Fromline].Remove(Fromcolumn, Tocolumn - Fromcolumn);
                File.WriteAllText(FullPathOfFile, string.Join("\n", contents));
                return;
            }

            //if line extends more than one line
            //deal with first line
            contents[Fromline] = contents[Fromline].Remove(Fromcolumn, contents[Fromline].Length - Fromcolumn);
            Fromline++;
            while (Fromline != Toline)
            {
                //remove whole lines here
                contents[Fromline] = contents[Fromline].Remove(0, contents[Fromline].Length);
                Fromline++;
            }
            contents[Fromline] = contents[Fromline].Remove(0, Tocolumn);
            File.WriteAllText(FullPathOfFile, string.Join("\n", contents));

        }


        private void DeleteFirstOccurenceOfStrAfterSource(MyCXSourceLocation fromcxSource, string charToRemove)
        {
            int Fromline = (int)fromcxSource.lineStart - 1;//minus 1 to account for array indexing
            int Fromcolumn = (int)fromcxSource.columnStart - 1;

            int Toline = (int)fromcxSource.lineEnd - 1;
            int Tocolumn = (int)fromcxSource.columnEnd - 1;

            //get the substring contents After the source
            List<string> contents = File.ReadAllLines(fromcxSource.MyCxFilefileStart.FileFullName).ToList();
            List<string> contentsCopy = new List<string>(contents);
            contentsCopy.RemoveRange(0, Toline);
            contentsCopy[0] = contentsCopy[0].Remove(0, Tocolumn);
            string contentAfterTheSource = string.Join("\n", contentsCopy);

            //remove first occurence of that char adn replace it with " "
            int index = contentAfterTheSource.IndexOf(charToRemove);
            contentAfterTheSource = contentAfterTheSource.Remove(index, charToRemove.Length);
            StringBuilder b = new StringBuilder(contentAfterTheSource);
            b.Insert(index, " ", charToRemove.Length);
            contentAfterTheSource = b.ToString();

            contents.RemoveRange(Toline + 1, contents.Count - Toline - 1);
            contents[contents.Count - 1] = contents.Last().Remove(Tocolumn, contents.Last().Length - Tocolumn);
            string contentBeforeEndOfSource = string.Join("\n", (contents));
            string newContents = contentBeforeEndOfSource + contentAfterTheSource;

            File.WriteAllText(fromcxSource.MyCxFilefileStart.FileFullName, newContents);

        }

        #endregion


        /// <summary>
        /// this will add a enum EnumConstantDecl to the enum name you specify with the EnumConstantDecl name you specify
        /// </summary>
        /// <param name="ofThisFileInScope"></param>
        /// <param name="toEnum">enum name from enum you want to add</param>
        /// <param name="enumvaluename">enum constant name value to add</param>
        /// <returns></returns>
        public bool AddEnumConstantDecl(string ofThisFileInScope, string toEnum, string enumvaluename)
        {
            //get of enum values cursors
            //List<MyCursorOfKindEnumConstantDecl> enumValuesCursor = toEnum.GetChildrenOfKind_EnumConstantDecl();

            CppParser cppParser = GetParserForFile(ofThisFileInScope);
            MyCursorOfKindEnumDecl toEnumCursor = cppParser.GetAllCursorsOfKind<MyCursorOfKindEnumDecl>()
                .Where((MyCursorOfKindEnumDecl cursorEnum) => { return cursorEnum.getCursorSpelling().ToString() == toEnum; }).FirstOrDefault();
            //check that that enum even exists
            if (toEnumCursor == null) { return false; }
            //else add the cursor name to the end source location of the enum
            var enumValuesCursor = toEnumCursor.GetChildrenOfKind_EnumConstantDecl();
            //if there are other
            if (enumValuesCursor.Count > 0)
            {//add a comma
                InsertStringINSIDECXsourceLocation("," + enumvaluename + "\n", toEnumCursor.MyCxSourceLocation, StartOrEndLocation.End);
            }
            else
            {//dont add the comma
                InsertStringINSIDECXsourceLocation(enumvaluename + "\n", toEnumCursor.MyCxSourceLocation, StartOrEndLocation.End);
            }


            //dont forget to reload the parser after EVERY write! 
            cppParser.ReloadParser();
            return true;

        }


        public bool RemoveEnumConstantDecl(string ofThisFileInScope, string toEnum, string enumvaluename)
        {
            CppParser cppParser = GetParserForFile(ofThisFileInScope);
            MyCursorOfKindEnumDecl toEnumCursor = cppParser.GetAllCursorsOfKind<MyCursorOfKindEnumDecl>()
                .Where((MyCursorOfKindEnumDecl cursorEnum) => { return cursorEnum.getCursorSpelling().ToString() == toEnum; }).FirstOrDefault();

            if (toEnumCursor == null) { return false; }

            var enumValueCursors = toEnumCursor.GetChildrenOfKind_EnumConstantDecl();
            var enumValueCursor = enumValueCursors.FirstOrDefault(enumconst => enumconst.getCursorSpelling() == enumvaluename);
            int indexofEnumIWant = enumValueCursors.FindIndex(en => clang.equalCursors(enumValueCursor.CXCursor, en.CXCursor) == 1);
            if (enumValueCursor == null) { return false; }

            if (enumValueCursors.Count == 0)
            {
                RemoveStringOfCXsourceLocation(enumValueCursor.MyCxSourceLocation);

            }
            else if (indexofEnumIWant == 0)
            {//if the enum I want is the first one, I need to account for the comma that comes after it. 
                DeleteFirstOccurenceOfStrAfterSource(enumValueCursor.MyCxSourceLocation, ",");
                RemoveStringOfCXsourceLocation(enumValueCursor.MyCxSourceLocation);
                //RemoveStringFromBEGGININGofCXsourceToBEGGININGofCXsource (enumValueCursors[indexofEnumIWant].MyCxSourceLocation, enumValueCursors[1].MyCxSourceLocation);

            }
            else
            {
                //there must be more than one enumValueCursors so I need to account for the comma before the last one. 
                DeleteFirstOccurenceOfStrAfterSource(enumValueCursors[indexofEnumIWant - 1].MyCxSourceLocation, ",");
                RemoveStringOfCXsourceLocation(enumValueCursors[indexofEnumIWant].MyCxSourceLocation);
                //RemoveStringFromENDofCXsourceToENDofCXsource(enumValueCursors[indexofEnumIWant-1].MyCxSourceLocation, enumValueCursors[indexofEnumIWant].MyCxSourceLocation);

            }


            cppParser.ReloadParser();
            return true;

        }


    }
}
