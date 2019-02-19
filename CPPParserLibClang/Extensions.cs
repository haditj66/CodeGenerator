using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CPPParserLibClang.CPPRefactoring;

namespace CPPParserLibClang
{
    public static class Extensions
    {

        public static bool isStartsWithHashtagInclude(this string lineToCheckIfisInclude)
        {

            //if start of line starts with a // then it is not a include
            if (Regex.IsMatch(lineToCheckIfisInclude, @"^\s*//") )
            {
                return false;
            }
            //if line starts with this pattern, it is a inlcude
            if (Regex.IsMatch(lineToCheckIfisInclude, @"^\s*#include"))
            {
                return true;
            }

            return false;
        }

        public static bool IsAnEmptyLine(this string lineToCheck)
        {
            if (Regex.IsMatch(lineToCheck, @"^\s*$"))
            {
                return true;
            }

            return false;
        }

        public static bool IsPragmaOnce(this string lineToCheck)
        {
            //if start of line starts with a // then it is not a include
            if (Regex.IsMatch(lineToCheck, @"^\s*//"))
            {
                return false;
            }
            //if line starts with this pattern, it is a inlcude
            if (Regex.IsMatch(lineToCheck, @"^\s*#pragma once"))
            {
                return true;
            }

            return false;
        }

        public static bool IsComment(this string lineToCheck)
        {
            //if start of line starts with a // then it is not a include
            if (Regex.IsMatch(lineToCheck, @"^\s*//"))
            {
                return true;
            } 

            return false;
        }


        public static MyCXCursor GetHeaderGaurdLocation(this string lineToCheck)
        {
            //if start of line starts with a // then it is not a include
            if (Regex.IsMatch(lineToCheck, @"^\s*//"))
            {
                return null;
            }

            return null;
        }
    }
}
