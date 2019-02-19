using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CppSharp.Parser;
using ClangParser = CppSharp.ClangParser;

namespace CPPParserLibClang
{
    class Program
    {

        static void Main(string[] args)
        {
            string file = @"c:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\Module1AA\rg.cpp";

            var parserOptions = new ParserOptions
            {
                LanguageVersion = LanguageVersion.CPP11,

                // Verbose here will make sure the parser outputs some extra debugging
                // information regarding include directories, which can be helpful when
                // tracking down parsing issues.
                Verbose = true
            };

            // This will setup the necessary system include paths and arguments for parsing.
            // It will probe into the registry (on Windows) and filesystem to find the paths
            // of the system toolchains and necessary include directories.
            parserOptions.Setup();

            // We create the Clang parser and parse the source code.
            var parser = new ClangParser();
            var parserResult = parser.ParseSourceFile(file, parserOptions);

            // If there was some kind of error parsing, then lets print some diagnostics.
            if (parserResult.Kind != ParserResultKind.Success)
            {
                if (parserResult.Kind == ParserResultKind.FileNotFound)
                    Console.Error.WriteLine($"{file} was not found.");

                for (uint i = 0; i < parserResult.DiagnosticsCount; i++)
                {
                    var diag = parserResult.GetDiagnostics(i);

                    Console.WriteLine("{0}({1},{2}): {3}: {4}",
                        diag.FileName, diag.LineNumber, diag.ColumnNumber,
                        diag.Level.ToString().ToLower(), diag.Message);
                }

                parserResult.Dispose(); 
            }

            // Now we can consume the output of the parser (syntax tree).

            // First we will convert the output, bindings for the native Clang AST,
            // to CppSharp's managed AST representation.
            var astContext = ClangParser.ConvertASTContext(parserOptions.ASTContext);

            // After its converted, we can dispose of the native AST bindings.
            parserResult.Dispose();

            // Now we can finally do what we please with the syntax tree.
            foreach (var sourceUnit in astContext.TranslationUnits)
            {
                Console.WriteLine(sourceUnit.FileName);
               //Console.WriteLine(sourceUnit.Classes.First().Fields.First().Name);
            }


            /*
            CXUnsavedFile cxf = new CXUnsavedFile();
            CXIndex index =  clang.createIndex(0, 0);
            clang.parseTranslationUnit(
                index, 
                @"test.hpp", null,0,
                out cxf, 0, 
                (int)CXTranslationUnit_Flags.CXTranslationUnit_None);*/

        }
    }
}
