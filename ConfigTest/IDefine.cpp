#include "IDefine.h"
 


IDefine::IDefine()
{
}

std::string IDefine::GetBaseName()
{
	return DefineName;
}

std::string IDefine::GetFullName()
{
	std::string delimiter = "*";

	//prefixName_ConfTypePrefix_defineName_suffixMajor_instanceOfconfig
	std::string strDefToPrint;
	  

	auto KeepPrefixName = [this, &strDefToPrint, delimiter](void) ->void
	{
		strDefToPrint.append(PrefixName);
		strDefToPrint.append(delimiter);
	}; 
	auto KeepconftypePrefix = [this, &strDefToPrint, delimiter](void) ->void
	{
		//make sure conftypePrefix is not empty
		if (!ConfTypePrefix.empty())
		{
			strDefToPrint.append(ConfTypePrefix);
			strDefToPrint.append(delimiter);
		}
	}; 
	auto KeepDefineName = [this, &strDefToPrint, delimiter](void) ->void
	{
		strDefToPrint.append(DefineName);
		
	}; 
	auto KeepMajor = [this, &strDefToPrint, delimiter](void) ->void
	{
		strDefToPrint.append(delimiter);
		strDefToPrint.append(std::to_string(Major));
	};
	auto KeepInstanceOfconfig = [this, &strDefToPrint, delimiter](void) ->void
	{ 
		//the first instance does not need to be put 
		if (InstanceOfConfig != 0)
		{
			strDefToPrint.append(delimiter);
			strDefToPrint.append(std::to_string(InstanceOfConfig));
		}
	};




	// for public or private  and static = false
	//prefixName_ConfTypePrefix_defineName_suffixMajor_instanceOfconfig
	if (((MyPrivacy == Privacy::Public) || (MyPrivacy == Privacy::Private)) )
	{ 
		if ((IsStatic == false))
		{
			KeepPrefixName(); KeepconftypePrefix(); KeepDefineName();  KeepInstanceOfconfig();
		}
		
	} 
	// for public or private  and multiInstance = true 
	//prefixName_ConfTypePrefix_defineName_suffixMajor 
	if (((MyPrivacy == Privacy::Public) || (MyPrivacy == Privacy::Private))  )
	{ 
		if ((IsStatic == true))
		{
			KeepPrefixName(); KeepconftypePrefix(); KeepDefineName(); //KeepMajor(); 
		} 
	}   
	//for template
	//prefixName_defineName_suffixMajor 
	//still need definename and conftype.
	if (MyPrivacy == Privacy::Template)
	{
		KeepPrefixName(); KeepconftypePrefix(); KeepDefineName();// KeepMajor();
	}
	//for statics
	//prefixName_ConfTypePrefix_defineName_suffixMajor
	//these can only be public for now. they need to not have multiinstance
	//else if (MyPrivacy == Privacy::Static)
	//{
	//	KeepPrefixName(); KeepconftypePrefix(); KeepDefineName(); KeepMajor();
	//}


	return strDefToPrint;

}

XMLElement* IDefine::GetDefineXMLEle()
{
	InitializeXMLEle();
	return defineXMLEle;
}
 
void IDefine::SetPrefix(std::string prefix)
{
	PrefixName = prefix;
}

void IDefine::SetPrivacy(Privacy privacy)
{
	MyPrivacy = privacy;
}

void IDefine::SetMajor(int major)
{
	Major = major;
}



