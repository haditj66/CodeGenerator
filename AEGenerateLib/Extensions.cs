using CgenMin.MacroProcesses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.MacroProcesses
{
    public static class Extensions
    {
        public static string SetAEConfig<T>(this T aeAOdefinable, int numOfAOOfThisSameTypeGeneratesAlready) where T : AO, IPartOfAEDefines
        {

            string ret = "";
            if (numOfAOOfThisSameTypeGeneratesAlready == 0)
            {


                string templateType = string.IsNullOrWhiteSpace(aeAOdefinable.GetFullTemplateType()) ? "" : $"template<{aeAOdefinable.GetFullTemplateType()}>"; 



                ret += $"#define AOInclude{AO.numOfAOSoFarAEConfigGenerated} {aeAOdefinable.ClassName}"; ret += "\n";
                ret += $"#define TemplateToAO{AO.numOfAOSoFarAEConfigGenerated} {templateType}"; ret += "\n";
                ret += $"#define ClassNameOfAO{AO.numOfAOSoFarAEConfigGenerated} {aeAOdefinable.ClassName}"; ret += "\n";
            }



            string temptype = string.IsNullOrWhiteSpace(aeAOdefinable.GetFullTemplateArgsValues()) ? "" : $"<{aeAOdefinable.GetFullTemplateArgsValues()}>";
            ret += $"#define TypeOfAO{AO.numOfAOSoFarAEConfigGenerated}_{numOfAOOfThisSameTypeGeneratesAlready + 1} {aeAOdefinable.ClassName}{temptype}"; ret += "\n";
            ret += $"#define InstanceNameOfAO{AO.numOfAOSoFarAEConfigGenerated}_{numOfAOOfThisSameTypeGeneratesAlready + 1} {aeAOdefinable.InstanceName}"; ret += "\n";
            //}


            aeAOdefinable.isGeneratedConfg = true;

            return ret;
        }
    }
}
