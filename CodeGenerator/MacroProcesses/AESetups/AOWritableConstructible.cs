using CodeGenerator.ProblemHandler;

namespace CgenMin.MacroProcesses
{


    public enum CppTypeEnum
    {
        float_t,
        uint32_t,
        uint16_t,
        uint8_t,
        int32_t,
        int16_t,
        int8_t,
        bool_t
    }

    public class CppFunctionArg
    {
        public CppFunctionArg(CppTypeEnum type, string name, bool isPublicSet = true)
        {
            Type = type;
            Name = name;
            _TypeStr = null;
            IsPublicSet = isPublicSet;
        }

        public CppFunctionArg(string type, string name, bool isPublicSet = true)
        {
            _TypeStr = type; 
            Name = name;
            IsPublicSet = isPublicSet;
        }

        public string TypeStr { get { return CppTypeEnumToStr(Type); } }
        private string _TypeStr;
        public CppTypeEnum Type { get; }
        public string Name { get; }
        public bool IsPublicSet { get; }

        public string GetPropertyFunctions()
        {
            string ret = "";

            //backing variable
            ret += $"protected: {TypeStr} {Name}; ";
            ret += "\n";

            //the Get
            ret += $"public: {TypeStr}  Get{Name}()   const " + "{" + $"return {Name}; " + "}";
            ret += "\n";
            //if set is protected, return nothing
            string publicORprotected = IsPublicSet == true ? "public" : "protected";
            if (IsPublicSet == true)
            {
                ret += $"{publicORprotected}:  void Set{Name}({TypeStr}  _{Name})" + "{" + $"{Name} = _{Name}; " + "}";
                ret += "\n";
            }

            return ret;
        }


        public string GetPropertyAllocating()
        {
            string ret = "";

            ret += $"{Name} =  _{Name};";
            return ret;
        }

        private string CppTypeEnumToStr(CppTypeEnum cppTypeEnum)
        {
            if (_TypeStr != null)
            {
                return _TypeStr;
            }

            switch (cppTypeEnum)
            {
                case CppTypeEnum.float_t:
                    return "float";
                    break;
                case CppTypeEnum.uint32_t:
                    return "uint32_t";
                    break;
                case CppTypeEnum.uint16_t:
                    return "uint16_t";
                    break;
                case CppTypeEnum.uint8_t:
                    return "uint8_t";
                    break;
                case CppTypeEnum.int32_t:
                    return "int32_t";
                    break;
                case CppTypeEnum.int16_t:
                    return "int16_t";
                    break;
                case CppTypeEnum.int8_t:
                    return "int8_t";
                    break;
                case CppTypeEnum.bool_t:
                    return "bool";
                    break;
                default:
                    ProblemHandle problem = new ProblemHandle();
                    problem.ThereisAProblem("you selected an invalid cpp type");
                    return "";
                    break;
            }
        }


    }


    public class CppFunctionArgs
    {

        public CppFunctionArg[] TheCppFunctionArgs { get; }
        public CppFunctionArgs(params CppFunctionArg[] cppFunctionArgs)
        {
            TheCppFunctionArgs = cppFunctionArgs;
        }


        public string GetContructorArgs()
        {
            string ret = "";
            if (TheCppFunctionArgs.Length > 0)
            {
                ret += $"{TheCppFunctionArgs[0].TypeStr} _{TheCppFunctionArgs[0].Name} ";
            }
            for (int i = 1; i < TheCppFunctionArgs.Length; i++)
            {
                ret += $", {TheCppFunctionArgs[i].TypeStr} _{TheCppFunctionArgs[i].Name} ";
            }

            return ret;
        }


        public string GetAllPropertyAllocating()
        {
            string ret = "";
            foreach (var item in TheCppFunctionArgs)
            {
                ret += item.GetPropertyAllocating();
                ret += "\n";
            }

            return ret;
        }
        public string GetAllPropertyFunctions()
        {
            string ret = "";
            foreach (var item in TheCppFunctionArgs)
            {
                ret += item.GetPropertyFunctions();
                ret += "\n";
            }

            return ret;
        }
    }





    public abstract class AOWritableConstructible : AOWritableToAOClassContents
    {
        public CppFunctionArgs TheCppFunctionArgs { get; }
        public AOWritableConstructible(string fromLibrary,  string instanceNameOfTDU,   AOTypeEnum aOTypeEnum, CppFunctionArgs cppFunctionArgs = null) 
            : base(fromLibrary,  instanceNameOfTDU, aOTypeEnum)
        { 
            TheCppFunctionArgs = cppFunctionArgs;
        }



        public string GetInitializationFunction()
        {

            string ret = TheCppFunctionArgs == null ? "" : 
                AEInitializing.TheMacro2Session.GenerateFileOut("AERTOS\\UserInitialize",
                new MacroVar() { MacroName = "ClassName", VariableValue = ClassName },
                new MacroVar() { MacroName = "CTORArgs", VariableValue = TheCppFunctionArgs.GetContructorArgs() },
                new MacroVar() { MacroName = "AllProperties", VariableValue = TheCppFunctionArgs.GetAllPropertyFunctions() },
                new MacroVar() { MacroName = "CTORArgsDefining", VariableValue = TheCppFunctionArgs.GetAllPropertyAllocating() }
                );

            return ret;
        }

    }

}
