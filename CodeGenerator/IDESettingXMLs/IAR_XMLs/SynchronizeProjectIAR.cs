using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CodeGenerator.IDESettingXMLs.IAR_XMLs.DEP;
using CodeGenerator.IDESettingXMLs.IAR_XMLs.EWD;
using CodeGenerator.IDESettingXMLs.IAR_XMLs.EWP;
using CodeGenerator.IDESettingXMLs.IAR_XMLs.EWT;
using Group = CodeGenerator.IDESettingXMLs.IAR_XMLs.EWT.Group;
using Option = CodeGenerator.IDESettingXMLs.IAR_XMLs.EWP.Option;

namespace CodeGenerator.IDESettingXMLs.IAR_XMLs
{
    public class SynchronizeProjectIAR : SynchronizeProjectBase
    {
        public static string FirstIARDir = "IAR";
        public static string SecondIARDir = "EWARM";

        public ProjectEWT EwtXml { get; }
        public ProjectEWP EwpXml { get; }
        public ProjectEWD EwdXml { get; }
        public ProjectDEP DepXml { get; }

        public SynchronizeProjectIAR(IDESetting ewtXML, IDESetting ewpXML, IDESetting ewdXML, IDESetting depXML) : base(ewtXML, ewpXML, ewdXML, depXML)
        {
            PrefixAdditionalIncludesPathWith = Path.Combine(@"$PROJ_DIR$", "..","..");
            EwtXml = ewtXML.RootOfSetting;
            EwpXml = ewpXML.RootOfSetting;
            EwdXml = ewdXML.RootOfSetting;
            DepXml = depXML.RootOfSetting;

        }

        #region Filter stuff



        protected override void AddMyFilterAsXMLFilter(MyFilter myfilter)
        {

            //go through the filter's parents to get the full path
            EWT.Group group;
            EWP.Group group2;
            List<string> ancestors = myfilter.GetAncestors();
            List<string> restOfAncestorsPath = GetGroupThatMatchesAncestorPath(out group, out group2, ancestors);

            if (group != null)
            {
                EWT.Group groupewt = EWT.Group.GetDefaultGroup();
                groupewt.Name = restOfAncestorsPath[0];
                EWP.Group groupewp = EWP.Group.GetDefaultGroup();
                groupewp.Name = restOfAncestorsPath[0]; 
                restOfAncestorsPath.RemoveAt(0);
                group.GroupNested.Add(groupewt);
                group2.GroupNested.Add(groupewp);

                //ewt
                //then add that group adn the rest of the groups
                EWT.Group parent = groupewt;
                foreach (var ancestor in restOfAncestorsPath)
                { 
                    EWT.Group childGroup = EWT.Group.GetDefaultGroup();
                    childGroup.Name = ancestor;
                    parent.GroupNested.Add(childGroup);

                    parent = childGroup;
                }   
                //EwtXml.Group.Add(group);


                //ewp 
                //then add that group adn the rest of the groups
                EWP.Group parent2 = groupewp;
                foreach (var ancestor in restOfAncestorsPath)
                {
                    EWP.Group childGroup2 = EWP.Group.GetDefaultGroup();
                    childGroup2.Name = ancestor;
                    parent2.GroupNested.Add(childGroup2);

                    parent2 = childGroup2;
                } 
               // EwpXml.Group.Add(group2);
            }
             
        }


        private List<string> GetGroupThatMatchesAncestorPath(out EWT.Group group, out EWP.Group group2, List<string> ancestorsFullPath)
        {

            List<string> ancestors = ancestorsFullPath;

            //go through all groups starting with first root ancestor and reate a nested group for each group not there
            group = EwtXml.Group.FirstOrDefault(g => g.Name == ancestors[0]);
            group2 = EwpXml.Group.FirstOrDefault(g => g.Name == ancestors[0]);
            if (group == null)
            {
                //create a root group and add it then
                group = EWT.Group.GetDefaultGroup();
                group.Name = ancestors[0];
                group2 = EWP.Group.GetDefaultGroup();
                group2.Name = ancestors[0];
                ancestors.RemoveAt(0);
                EwtXml.Group.Add(group);
                EwpXml.Group.Add(group2);
            }
            else
            {
                EWT.Group groupprev;
                EWP.Group group2prev;
                do
                {
                    groupprev = group;
                    group2prev = group2;
                    //are any of the children matching the next ancestor
                    ancestors.RemoveAt(0);
                    if (ancestors.Count == 0)
                    {
                        break;
                    }
                    group = group.GroupNested.FirstOrDefault(g => g.Name == ancestors[0]);
                    group2 = group2.GroupNested.FirstOrDefault(g => g.Name == ancestors[0]);
                } while (group != null);

                group = groupprev;
                group2 = group2prev;
            }

            return ancestors;
        }

        protected override List<MyFilter> ConvertAllCurrentXMLFiltersAsMyFilters()
        { 

            //go through each top level group and add them as a filter 
            List<MyFilter> topFilters = new List<MyFilter>();
            //for ewt
            foreach (var topGroup in EwtXml.Group)
            {
                MyFilter topFilterToAdd = new MyFilter(topGroup.Name);
                SetChildrenFiltersFromGroup(topFilterToAdd, topGroup);
                 
                 
                topFilters.Add(topFilterToAdd);
            }

            return topFilters;
        }


        private void SetChildrenFiltersFromGroup(MyFilter parentFilterToSetChildFor, EWT.Group parentGroup)
        {
             
            //go through the nested group to get child filters
            if (parentGroup.GroupNested.Count > 0)
            {
                foreach (var child1 in parentGroup.GroupNested)
                {
                    MyFilter childFilterToAdd1 = parentFilterToSetChildFor.AddChildFilter(child1.Name);

                    SetChildrenFiltersFromGroup(childFilterToAdd1, child1);
                     
                }

            } 

        }


        protected override void RemoveAllMentionsOfSyncedFilters()
        {
            //just remove the groups with that filter.

            //for ewt
            var applicationGroup =  EwtXml.Group.FirstOrDefault(g => g.Name == SYNCEDSTR);
            if (applicationGroup != null)
            {
                EwtXml.Group.Remove(applicationGroup);
            }

            //for ewp
            var applicationGroup2 = EwpXml.Group.FirstOrDefault(g => g.Name == SYNCEDSTR);
            if (applicationGroup2 != null)
            {
                EwpXml.Group.Remove(applicationGroup2);
            }

            //filters dont affect ewd

            //filters dont affect dep 
        }

        protected override void AddFilterSynced()
        {
            //for ewt
            EWT.Group groupewtSync = EWT.Group.GetDefaultGroup();
            groupewtSync.Name = SYNCEDSTR;
            EWT.Group groupewtUser = EWT.Group.GetDefaultGroup();
            groupewtUser.Name = USERSTR;
            groupewtSync.GroupNested.Add(groupewtUser);
            EwtXml.Group.Add(groupewtSync);


            //for ewp
            EWP.Group groupewpSync = EWP.Group.GetDefaultGroup();
            groupewpSync.Name = SYNCEDSTR;
            EWP.Group groupewpUser = EWP.Group.GetDefaultGroup();
            groupewpUser.Name = USERSTR;
            groupewpSync.GroupNested.Add(groupewpUser);
            EwpXml.Group.Add(groupewpSync);

        }



        #endregion



        #region additionalinclude stuff
        protected override void AddSTRINGIncludeAsAdditionalIncludes(string additionalIncludeDir)
        {
            List<Option> options = EwpXml.Configuration.Settings.SelectMany(s => s.Data.Option).ToList();
            Option ewpincludes = options.First(opt => opt.Name == "CCIncludePath2");
             
            ewpincludes.State.Add(additionalIncludeDir);
        }

        protected override List<string> ConvertAllCurrentAdditionalIncludesAsStrings()
        {

            //includes are in an  option element with name element CCIncludePath2
            List<Option> options = EwpXml.Configuration.Settings.SelectMany(s=>s.Data.Option).ToList();
            var ewpincludes = options.Where(opt => opt.Name == "CCIncludePath2")
                                     .SelectMany(optcc => optcc.State).ToList();
             
            return ewpincludes;
        }


        #endregion


        #region clinclude stuff
        protected override void AddMyClIncludesAsCIncludes(MyCLIncludeFile CLIncludeFile)
        {
            //first find the group that it belongs from the filter it belongs to. i need to insert synced and user filters
            EWT.Group group;
            EWP.Group group2;
            List<string> ancestors = CLIncludeFile.FilterIBelongTo.GetAncestors();
            ancestors.Reverse(); ancestors.Add(USERSTR); ancestors.Add(SYNCEDSTR); ancestors.Reverse();
            List<string> restOfAncestorsPath = GetGroupThatMatchesAncestorPath(out group, out group2, ancestors);

            //add the file to that group
            EWT.File fileewt = new EWT.File();
            EWP.File fileewp = new EWP.File();
            fileewt.Name = Path.Combine(PrefixAdditionalIncludesPathWith, CLIncludeFile.FullLocationName);
            fileewp.Name = Path.Combine(PrefixAdditionalIncludesPathWith, CLIncludeFile.FullLocationName);
            group.File.Add(fileewt);
            group2.File.Add(fileewp);
        }
         
        protected override void RemoveAllMentionsOfSyncedCInc()
        {
            //no need to change anything as the filter should have also removed the ccomps and ccincs
        }
        #endregion


        #region ccompile stuff
        protected override void AddMyCLCompileFileAsCLCompileFile(MyCLCompileFile CLCompileFile)
        {
            //first find the group that it belongs from the filter it belongs to. i need to insert synced and user filters
            EWT.Group group;
            EWP.Group group2;
            List<string> ancestors = CLCompileFile.FilterIBelongTo.GetAncestors();
            ancestors.Reverse(); ancestors.Add(USERSTR); ancestors.Add(SYNCEDSTR); ancestors.Reverse();
            List<string> restOfAncestorsPath = GetGroupThatMatchesAncestorPath(out group, out group2, ancestors);

            //add the file to that group
            EWT.File fileewt = new EWT.File();
            EWP.File fileewp = new EWP.File();
            fileewt.Name = Path.Combine(PrefixAdditionalIncludesPathWith, CLCompileFile.FullLocationName);
            fileewp.Name = Path.Combine(PrefixAdditionalIncludesPathWith, CLCompileFile.FullLocationName);
            group.File.Add(fileewt);
            group2.File.Add(fileewp);
             
        }
         

        protected override void RemoveAllMentionsOfSyncedCComp()
        {
            //no need to change anything as the filter should have also removed the ccomps and ccincs
        }
        #endregion




        public static SynchronizeProjectIAR CreateSynchronizeProjectIAR(string pathWithoutFileNameOfFileProject)
        {
            IDESetting ewtXML = new IDESetting(pathWithoutFileNameOfFileProject, ".ewt", typeof(CodeGenerator.IDESettingXMLs.IAR_XMLs.EWT.ProjectEWT));
            IDESetting ewpXML = new IDESetting(pathWithoutFileNameOfFileProject, ".ewp", typeof(CodeGenerator.IDESettingXMLs.IAR_XMLs.EWP.ProjectEWP));
            IDESetting ewdXML = new IDESetting(pathWithoutFileNameOfFileProject, ".ewd", typeof(CodeGenerator.IDESettingXMLs.IAR_XMLs.EWD.ProjectEWD));
            IDESetting depXML = new IDESetting(pathWithoutFileNameOfFileProject, ".dep", typeof(CodeGenerator.IDESettingXMLs.IAR_XMLs.DEP.ProjectDEP));

            SynchronizeProjectIAR synchronizeProjectiar = new SynchronizeProjectIAR(ewtXML, ewpXML, ewdXML, depXML);
            return synchronizeProjectiar;
        }







    }
}