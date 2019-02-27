using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CodeGenerator.IDESettingXMLs
{
    public abstract class SynchronizeProjectBase : MySettingsBase
    {
        protected const string SYNCEDSTR = "Synced";
        protected const string USERSTR = "User";
        public string PrefixAdditionalIncludesPathWith { get; protected set; }

        protected MyFilter Syncedfitler;
        protected MyFilter UserFilter;

        public SynchronizeProjectBase(params IDESetting[] xmlSettings) : base(xmlSettings)
        {
            PrefixAdditionalIncludesPathWith = "";

            Syncedfitler = new MyFilter(SYNCEDSTR);
            UserFilter = Syncedfitler.AddChildFilter(USERSTR);
            if (myFilters == null)
            {
                myFilters = new List<MyFilter>();
            } 
            myFilters.Add(UserFilter);
        }


        public override void Initiate()
        { 
            if (!IsAlreadyInitiated)
            {
                RemoveAllMentionsOfSyncedFilters();
                RemoveAllMentionsOfSyncedCComp();
                RemoveAllMentionsOfSyncedCInc();
                AddFilterSynced();

                myFilters = ConvertAllCurrentXMLFiltersAsMyFilters();
               StringIncludes = ConvertAllCurrentAdditionalIncludesAsStrings();

               AddAdditionalInclude("");
               AddAdditionalInclude(Path.Combine("Config"));
                //CLCompileFiles = ConvertAllCurrentCCompilessAsMyClCompiles();
                //CLIncludeFiles = ConvertAllCurrentCIncludesAsMyClIncludes();
                IsAlreadyInitiated = true;
            } 

        } 


        protected abstract void RemoveAllMentionsOfSyncedFilters();
        protected abstract void RemoveAllMentionsOfSyncedCComp();
        protected abstract void RemoveAllMentionsOfSyncedCInc();
        protected abstract void AddFilterSynced();




        public override void AddFilter(MyFilter filterToAdd)
        {
            //add to filters but also convert it to a XMLFilter and add it there to the settings as needed.
            //all filters will be added to the Application->Users filter so dont worry about having a check for if the filter already exists as the filter will be 
            //erased anyways when there is a new sync


            //all filters added to a synced project should have a base or Applications->Users
            foreach (var filterchildsAsWell in filterToAdd)
            {

                MyFilter filterToAddCopy = Extensions.restOfExtensions.DeepCopy<MyFilter>(filterchildsAsWell);
                 

                if (filterToAddCopy.Parent == null)
                {
                    MyFilter childToAdd = UserFilter.AddChildFilter(filterToAddCopy);
                    //convert that filter to an xmlClassFilter
                    AddMyFilterAsXMLFilter(childToAdd);
                }
                else
                {
                    //put user to the parent filter
                    MyFilter childToAdd = UserFilter.AddChildFilter(filterToAddCopy.GetTopParent());

                    AddMyFilterAsXMLFilter(filterToAddCopy);
                }

           
            }
             
            //myFilters.Add(userFilter);
             
            
        }

        public override void AddAdditionalInclude(string AdditionalInclude)
        {
            //if (!DoesAdditionalIncludesAlreadyExist(AdditionalInclude))
            //{


            //prefix all added additional inlcudes with the $PROJ_DIR$/..  if they are relative additionalinclude directories
            if (!Path.IsPathRooted(AdditionalInclude))
            {
                AdditionalInclude = Path.Combine(PrefixAdditionalIncludesPathWith, AdditionalInclude); 

            }
            // only add it if the additional include does not exist yet 
            if (!StringIncludes.Any(s => s == AdditionalInclude))
            {
                StringIncludes.Add(AdditionalInclude);

                AddSTRINGIncludeAsAdditionalIncludes(AdditionalInclude);
            }

            //}
        }

        public override void AddCLIncludeFile(MyCLIncludeFile CLIncludeFile)
        {
            //if (!DoesClIncludeExist(CLIncludeFile))
            //{
            //if (!CLIncludeFiles.Any(cl => cl.FullLocationName == CLIncludeFile.FullLocationName))
            //{
               // CLIncludeFiles.Add(CLIncludeFile);

                AddMyClIncludesAsCIncludes(CLIncludeFile);
                //}
            //}
        }

        public override void AddCLCompileFile(MyCLCompileFile CLCompileFile)
        {
            //if (!DoesCCompileExist(CLCompileFile))
            // {
            //if (!CLCompileFiles.Any(cl => cl.FullLocationName == CLCompileFile.FullLocationName))
            //{
               // CLCompileFiles.Add(CLCompileFile);

                AddMyCLCompileFileAsCLCompileFile(CLCompileFile);
            //}
            //}

        }
    }
}