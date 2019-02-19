using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPPParserLibClang.CPPRefactoring
{
    public class CppRefactorer
    {
        public string InsertNamespaceIntoFile(string ContentsOfFile,string nameOfNamespace)
        {

            string namespaceOpen = @"namespace " + nameOfNamespace + "{";
            string namespaceCLose = @"}//" + nameOfNamespace ;

            var nameOfNamespaceAsList = ContentsOfFile.Split('\n').ToList();  
                int lineNumber = 0;
                foreach (var line in nameOfNamespaceAsList) 
                {
                    //let only line that are either empty, #include, comment, or pragmaonce pass through
                    if (line.isStartsWithHashtagInclude()  || line.IsAnEmptyLine() || line.IsPragmaOnce() || line.IsComment())
                    { 

                    }
                    else
                    {
                        break;
                    }

                    lineNumber++;
                }

                nameOfNamespaceAsList.Insert(lineNumber, namespaceOpen);
                nameOfNamespaceAsList.Add(namespaceCLose);
                return String.Join("\n", nameOfNamespaceAsList.ToArray());
        }


    }
}
