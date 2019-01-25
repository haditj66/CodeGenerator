using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeGenerator.IDESettingXMLs
{
    public class MyFilter : IEnumerable<MyFilter>
    {
        public MyFilter Parent { get; set; }
        public List<MyFilter> ChildrenFilters { get; set; }
        public string Name{ get; set; }
        public List<MyCLIncludeFile> CLIncludeFiles { get; set; }
        public List<MyCLCompileFile> CLCompileFiles { get; set; }

        public MyFilter this[int index]
        {
            get
            {
                return ChildrenFilters[index];
            }
            set
            {
                ChildrenFilters[index] = value;
            }
        }

        public MyFilter this[string includeName]
        {
            get
            {
                if (includeName == Name)
                {
                    return this;
                }

                foreach (var childFilter in ChildrenFilters)
                {
                    if (childFilter.Name == includeName)
                    {
                        return childFilter;
                    } 
                }
                return null;
            } 
        }

        public MyFilter(string name)
        {
            ChildrenFilters = new List<MyFilter>();
            Name = name;
        }

        private MyFilter(MyFilter parent, string name)
        {
            ChildrenFilters = new List<MyFilter>();
            Parent = parent;
            Name =  name; 
        } 

        public MyFilter AddChildFilter(string name)
        {
            //take out any full address paths
            string nameStripped = Path.GetFileName(name);
            MyFilter child = new MyFilter(this, nameStripped); 
            ChildrenFilters.Add(child);
            return child;
        }

        public MyFilter GetFilterFromAddress(string address)
        {
            //address needs to be of form name1\name2\name3\name4

            //first check if it still has a parent 
            string pattern = @"^([^\\.]+)\\";
            Regex regex = new Regex(pattern); 
             Match match = regex.Match(address);

            MyFilter finalChildFilter = null;
            if (match.Success)
            {
                //then it still has a parent. get rid of it and find the child instead
                string addressTunc = Regex.Replace(address, @"^([^\\.]+)\\", "");
                MyFilter Child = this[match.Groups[1].Value];

                //check if first name is the name of this filter 
                if (match.Groups[1].Value == Name)
                {
                    return this.GetFilterFromAddress(addressTunc);
                }
                if (Child == null)
                {
                    return null;
                } 
                finalChildFilter = Child.GetFilterFromAddress(addressTunc);
            }
            else
            {
                //else it must already be the last one
                finalChildFilter = this[address];
            }

            return finalChildFilter;
        }


        public string GetFullAddress()
        {
            //start with current name
            string address = "";
            address += Name;

            if (Parent != null)
            {
                address = Parent.GetFullAddress() + "\\" + address;
            }
            return address;
        }

        public MyFilter DoesAddressExist(string Address)
        {
            MyFilter filterToReturn = null;
            //first check if current filter matches address
            if (this.GetFullAddress() == Address)
            {
                return this;
            }


            //iterate through all children and check if that address matches exactly
            foreach (var childFilter in this.ChildrenFilters)
            {
                filterToReturn = childFilter.DoesAddressExist(Address);
                //check if not null
                if (filterToReturn != null)
                {
                    return filterToReturn;
                }
            } 
            return filterToReturn;
        }

        public IEnumerator<MyFilter> GetEnumerator()
        {
            yield return this;
            //now go through each childFilter and return that
            foreach (var child in ChildrenFilters)
            {
                foreach (var ch in child)
                {
                    yield return ch;
                }
            }

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
