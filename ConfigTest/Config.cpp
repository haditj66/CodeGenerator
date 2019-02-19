#include "Config.h"
#include <algorithm> 


//int Config::InstanceOfConfig = -1;
std::vector<Config*> Config::ConfigsCreatedSoFar;

void Config::PrintDefines(Config& TopLevelConfig)
{


	//set as toplevel
	TopLevelConfig.isTopLevel = true;
	//ACTUALL DONT also set any configs that match the top level type as top level
	for each (Config* config in ConfigsCreatedSoFar)
	{
		/*
		if (config->IsEqual(TopLevelConfig))
		{
			config->isTopLevel = true;
		}*/



		//----- set the xml values ------
		{
			config->XmlConfigElement = xmlDoc.NewElement("Config");
			config->XmlConfigElement->SetAttribute("Prefix", config->Prefix.c_str());
			config->XmlConfigElement->SetAttribute("myInstanceNum", config->myInstanceNum);
			config->XmlConfigElement->SetAttribute("Major", config->Major);
			config->XmlConfigElement->SetAttribute("ConfTypePrefix", config->ConfTypePrefix.c_str());
			config->XmlConfigElement->SetAttribute("isTopLevel", config->isTopLevel);
			config->XmlConfigElement->SetAttribute("FileNameString", config->FileNameString.c_str());
			config->XmlConfigElement->SetAttribute("ClassName", config->ClassName.c_str());

		}
	}


	//set the depends for the top level
	//look for all non top level libraries that dont have parent dependers
		for each(auto lib in ConfigsCreatedSoFar)
		{
			if (lib->isTopLevel == false && lib->ParentDepender == nullptr && lib->ClassName != "GlobalBuildConfig")
			{
				//then add this library as a depend to the top level library
				TopLevelConfig.DependConfigs.push_back(lib);
			}
		} 


	//I need to assert that no config is having a circular dependency to another
	bool anyCicularDepends = false;
	for each (Config* confDepender in Config::ConfigsCreatedSoFar)
	{
		// now go through all its dependents, if a dependent is the same as the depender, it is a circular dependence
		for each (Config*  confDependent in confDepender->DependConfigs)
		{ 
			// SAME IN THIS CASE JUST MEANS SAME PREFIX!!
			if (confDependent->Prefix == confDepender->Prefix)
			{
				anyCicularDepends = true;
				break;
			}
		}
	}
	assert(!anyCicularDepends);




	//delete the configuration file.
	DeleteConfigfile();

	//put the top header in
	PrintHeader(Header::Top);


	//go through that configs enum types and print out those types
	PrintHeader(Header::Types);
	for each (Config* config in ConfigsCreatedSoFar)
	{
		//also set all files that are static as top level 
		if (config->ClassName == "GlobalBuildConfig")
		{
			config->isTopLevel = true;
		}

		std::string strDefToPrint;
		for each (EnumElement* enumEle in config->enumElementList)
		{
			strDefToPrint = "";
			strDefToPrint.append("#define ");
			strDefToPrint.append(enumEle->Name);
			strDefToPrint.append("  ");
			strDefToPrint.append(std::to_string(enumEle->Value));
			strDefToPrint.append("\n");
			SendToConfigFile(strDefToPrint);
		}

	}


	//lambda for printing
	auto PrintLamb = [](IDefine* var, Config* config)->void
	{

		// if var is static and the instance is greater the 0, you dont have to print it.
		if (var->IsStatic == true && config->myInstanceNum > 0)
		{

		}
		else
		{
			std::string strDefToPrint;
			strDefToPrint.append("#define ");
			strDefToPrint.append(var->GetFullName());

			//print the value now
			strDefToPrint.append("  ");

			//assert that ValueAsString is not empty
			if(	var->ValueAsString.empty())
			{ 
				config->Problem(var->GetBaseName().append(" for library ")
					.append(config->ClassName)
					.append(" has not been defined. either define it or add a default define in its library ")
					.append(config->ClassName));
			}
			strDefToPrint.append(var->ValueAsString);

			strDefToPrint.append("\n");

			SendToFile(strDefToPrint);
		}

	};


	//lambda for printing top levels
	auto PrintTopLevelLamb = [](IDefine* var, Config* config)->void
	{


		//only do this once, afterwards only nonstatic defines get printed again.
		if (var->IsStatic == false && config->myInstanceNum > 0)
		{
			std::string strDefToPrint;
			strDefToPrint.append("#define ");
			strDefToPrint.append(var->DefineName);
			//print instancenum value
			strDefToPrint.append("_");
			strDefToPrint.append(std::to_string(var->InstanceOfConfig));

			//print the value now
			strDefToPrint.append("  ");

			//assert that ValueAsString is not empty
			assert(!var->ValueAsString.empty());
			strDefToPrint.append(var->ValueAsString);

			strDefToPrint.append("\n");

			SendToConfigFile(strDefToPrint);
		}
		// if var is static and the instance is greater the 0, you dont have to print it.
		else if ((var->IsStatic == true && config->myInstanceNum > 0))
		{

		}
		else
		{
			std::string strDefToPrint;
			strDefToPrint.append("#define ");
			strDefToPrint.append(var->DefineName);

			//print the value now
			strDefToPrint.append("  ");

			//assert that ValueAsString is not empty
			assert(!var->ValueAsString.empty());
			strDefToPrint.append(var->ValueAsString);

			strDefToPrint.append("\n");

			SendToConfigFile(strDefToPrint);
		}

	};


	//TODO
	//reorder any types that are top level types?


	//go through each config created and print out their defines
	PrintHeader(Header::Defines);
	for each (Config* config in Config::ConfigsCreatedSoFar)
	{
		//go through each public define and print out their defines
		for each (IDefine* var in config->publicDefineList)
		{
			if (config->isTopLevel) { PrintTopLevelLamb(var, config); }
			else { PrintLamb(var, config); }
		}
		//for private
		for each (IDefine* var in config->privateDefineList)
		{
			if (config->isTopLevel) { PrintTopLevelLamb(var, config); }
			else { PrintLamb(var, config); }
		}
		// for template
		for each (IDefine* var in config->templateDefineList)
		{
			if (config->isTopLevel) { PrintTopLevelLamb(var, config); }
			else { PrintLamb(var, config); }
		}
		// for static
		//for each (IDefine* var in config->staticDefineList)
		//{
		//	PrintLamb(var);
		//}
	}





	//finally send to the xml file.
	ConfigToXml();

}


Config::Config()
{
	

	ConfTypePrefix = "";
	//InstanceOfConfig++; //each time a config is initialized, the instance prefix goes up by one.
						//Major = major;
	AlreadyInitialized = false; 
	//std::string s= name[name.end()];

	//myInstanceNum = InstanceOfConfig;
	Prefix = "";
	//auto spublicDefineList = InitListPublicOVERRIDE(); 
}

void Config::Init()
{
	if (AlreadyInitialized == false)
	{
		AlreadyInitialized = true;

		InitListPublicOVERRIDE(&publicDefineList);
		InitListPrivateOVERRIDE(&privateDefineList);
		InitListTemplateOVERRIDE(&templateDefineList);
		//InitListStaticOVERRIDE(&staticDefineList); 


		FileNameString = GetFileNameOVERRIDE();
		
		static std::vector<std::string> enumsAlreadyObtained;
		std::vector<IDefine*> allDefines; allDefines.insert(allDefines.end(), publicDefineList.begin(), publicDefineList.end());
		allDefines.insert(allDefines.end(), privateDefineList.begin(), privateDefineList.end());
		allDefines.insert(allDefines.end(), templateDefineList.begin(), templateDefineList.end());
		// get any enums that are in this file. by going through the list of public and private dfine lists and checking their type.
		for each (IDefine* define in allDefines)
		{
			if (((define->GetTypeStr()).find("enum ") != -1))
			{
				//than it must be a enum type so grab the enum type names and put it in the enum 
				//first go in the file and read everything line by line. 
				std::ifstream infile(FileNameString);
				std::string line;
				bool enumlineFound = false;
				bool openBracketFound = false;
				int indexOfEnum = 0;
				while (std::getline(infile, line))
				{

					//if open bracket was found than the next line should be a enum value type
					if (openBracketFound == true)
					{
						//first check that the closed bracket is not next
						if (line.find("}") != -1)
						{
							// if closing bracket was found, stop looking and reset
							enumlineFound = false;
							openBracketFound = false;
							indexOfEnum = 0;

							break;
						}
						else
						{
							//else its an enum type. remove any commas and white spaces
							auto isWhiteSpace = [](char ele)->bool {return (std::string(1, ele) == " ") || (std::string(1, ele) == "\t") || (std::string(1, ele) == "\n") || (std::string(1, ele) == ","); };
							line.erase(std::remove_if(line.begin(), line.end(), isWhiteSpace), line.end());

							//prefix the name with the define Name
							std::string enumDefineName = define->DefineName;

							enumDefineName.append("_");
							enumDefineName.append(line);

							//push into enumlist
							enumElementList.push_back(new EnumElement(indexOfEnum, enumDefineName, &xmlDoc));
							indexOfEnum++;
						}
					}


					//if enum line was found already, 
					if (enumlineFound == true)
					{
						//look for the { next
						if (line.find("{") != -1)
						{
							openBracketFound = true;
						}
					}

					//check if that line contains the name of a enum type
					if (line.find("enum ") != -1) //(define->GetTypeStr()) != -1 )
					{
						//also check if I got that enum already
						bool gotItAlready = false;
						for (int i = 0; i < enumsAlreadyObtained.size(); i++)
						{
							if (enumsAlreadyObtained[i] == line)
							{
								gotItAlready = true;
								break;
							}
						}
						if (gotItAlready == false)
						{
							enumsAlreadyObtained.push_back(line);
							enumlineFound = true;
						}

					}
				}

			}
		}




		isTopLevel = false;

		//get name of class
		std::string name = typeid(*this).name();
		int pos = name.find("class", 0); name.replace(pos, 6, "");
		int pos2 = name.find("<", 0);
		//make sure there is a template
		if (pos2 != -1)
		{
			int pos3 = name.find(">", 0);  name.replace(pos2, pos3 - pos2 + 1, "");
		}
		ClassName = name;


		//setting the major and adding the MAJOR define
		int major;
		if (name != "GlobalBuildConfig")
		{
			 

			//get the Major of the class. make sure that there is a major, a single number at the end of the class.

			std::string s = std::string(1, name.back());
			assert((s == "0" || s == "1" || s == "2" || s == "3" || s == "4" || s == "5" || s == "6" || s == "7" || s == "8" || s == "9"));

			//now see if there was a second digit
			std::string ss = std::string(1, name[name.length() - 2]);
			if (ss == "0" || ss == "1" || ss == "2" || ss == "3" || ss == "4" || ss == "5" || ss == "6" || ss == "7" || ss == "8" || ss == "9")
			{
				ss.append(s);
				major = std::stoi(ss, nullptr, 10);
			}
			else {
				major = std::stoi(s, nullptr, 10);
			}

			//add a "major" define to the privatedefinelist
			Defines<int>* defineMajor = new Defines<int>("MAJOR", Major, true);
			defineMajor->SetValue(major);
			privateDefineList.push_back((IDefine*)defineMajor);

		}
		else {
			major = 0;
			name = "GlobalConfigBuild";
		}
		Major = major;
		Prefix = name;


		//get InstanceOfConfig
		//go through each config to see how many match its prefix, major, and conftypeprefix. these are all the exact same type
		auto lamb = [this](Config* conf)->bool {
			return (
				conf->Major == this->Major &&
				conf->Prefix == this->Prefix &&
				conf->ConfTypePrefix == this->ConfTypePrefix
				); };
		int instances = std::count_if(ConfigsCreatedSoFar.begin(), ConfigsCreatedSoFar.end(), lamb);
		myInstanceNum = instances;

		//add this config to the list so far
		AddConfigSoFar(this);


		SendLibraryFileNameToIncludeList();


		auto initDefinesLambda = [this](IDefine* define, Privacy privacy) {
			define->SetMajor(Major);
			define->SetPrefix(Prefix);
			define->SetPrivacy(privacy);
			define->ConfTypePrefix = ConfTypePrefix;
			define->InstanceOfConfig = myInstanceNum;
			define->InitializeXMLEle();
		};

		//go through and set the prefix in the defines 
		std::map<std::string, IDefine*>::iterator it;
		for (auto define : this->publicDefineList)//(it = publicDefineList.begin(); it != publicDefineList.end(); it++)
		{
			//auto define = it->second;
			initDefinesLambda(define, Privacy::Public);
		}
		//go through the same for the private list
		for (auto define : privateDefineList)//(it = publicDefineList.begin(); it != publicDefineList.end(); it++)
		{
			//auto define = it->second;
			initDefinesLambda(define, Privacy::Private);
		}
		//go through the same for template list
		for (auto define : templateDefineList)//(it = publicDefineList.begin(); it != publicDefineList.end(); it++)
		{
			//auto define = it->second;
			initDefinesLambda(define, Privacy::Template);
		}
		//go through the same for static list
		//for (auto define : staticDefineList)//(it = publicDefineList.begin(); it != publicDefineList.end(); it++)
		//{
			//auto define = it->second;
			//initDefinesLambda(define, Privacy::Static);
		//}






	}
}

bool Config::IsEqual(const Config& configToCompare) const
{
	//if major is the same, ClassName is the same, and conftypeprefix is the same.
	if (configToCompare.ConfTypePrefix == this->ConfTypePrefix && configToCompare.Major == this->Major && configToCompare.ClassName == this->ClassName)
	{
		return true;
	}

	return false;
}

void Config::Problem(std::string msg)
{
	std::string p = "PROBLEM:: ";
	std::cout << std::endl;
	std::cout << p.append(msg) << std::endl;
	std::cout << std::endl;

}

void Config::AddConfigSoFar(Config* configToAdd)
{
	ConfigsCreatedSoFar.push_back(configToAdd);
}

void Config::SendToFile(std::string stringToSend)
{
	std::cout << stringToSend;
}

void Config::SendToConfigFile(std::string stringToSend)
{
	std::fstream file;
	file.open(CONFIGURATIONFILE_DIRECTORY, std::ios::app);

	file.write(stringToSend.c_str(), sizeof(char) * stringToSend.length());
	file.close();
}

void Config::DeleteConfigfile()
{

	std::fstream file;
	file.open(CONFIGURATIONFILE_DIRECTORY, std::ofstream::out | std::ofstream::trunc);

	file.close();
}

XMLDocument Config::xmlDoc;

void Config::ConfigToXml()
{
	 

	XMLNode* root = xmlDoc.NewElement("Root");
	xmlDoc.InsertFirstChild(root);

	//go through each configuration and insert them into the config list.
	XMLElement* configsEle = xmlDoc.NewElement("Configs");


	root->InsertEndChild(configsEle);

	for each (Config* config  in ConfigsCreatedSoFar)
	{

		// I should ignore all configs with instances above 0 and defines to the 0 one
		if (config->myInstanceNum == 0)
		{ 

			//copy all nonstatic defines to the config from same type configs 
			for each (Config* conf2 in ConfigsCreatedSoFar)
			{
				if (conf2->IsEqual(*config) && conf2->myInstanceNum > 0)
				{
					//go through public list
					for each (IDefine* pubDef in conf2->publicDefineList)
					{
						if (pubDef->IsStatic == false)
						{
							//than copy that over to the instance 0 config
							pubDef->DefineName.append("_").append(std::to_string(conf2->myInstanceNum));
							config->publicDefineList.push_back(pubDef);
						}
					}
					//go through private list
					for each (IDefine* privDef in conf2->privateDefineList)
					{
						if (privDef->IsStatic == false)
						{
							//than copy that over to the instance 0 config
							privDef->DefineName.append("_").append(std::to_string(conf2->myInstanceNum));
							config->privateDefineList.push_back(privDef);
						}
					}

				}
			} 


			std::vector<IDefine*> allDefines; allDefines.insert(allDefines.end(), config->publicDefineList.begin(), config->publicDefineList.end());
			allDefines.insert(allDefines.end(), config->privateDefineList.begin(), config->privateDefineList.end());
			allDefines.insert(allDefines.end(), config->templateDefineList.begin(), config->templateDefineList.end());


			configsEle->InsertEndChild(config->XmlConfigElement);


			XMLElement* enumsEle = xmlDoc.NewElement("EnumsDefinintions");
			XMLElement* DefinesEle = xmlDoc.NewElement("Defines");
			XMLElement* LibraryDependends = xmlDoc.NewElement("Depends");

			config->XmlConfigElement->InsertEndChild(enumsEle);
			//go through each enum element and insert it into the configxmlelement
			for each (EnumElement* enumEle in config->enumElementList)
			{
				enumsEle->InsertEndChild(enumEle->enumXMLEle);
			}

			config->XmlConfigElement->InsertEndChild(DefinesEle);
			for each (IDefine* define in allDefines)
			{
				DefinesEle->InsertEndChild(define->GetDefineXMLEle());
			}

			//for library depends
			config->XmlConfigElement->InsertEndChild(LibraryDependends);
			for each (Config* configDepend in config->DependConfigs)
			{
				XMLElement* XMLEle = Config::xmlDoc.NewElement("Depend");
				XMLEle->SetAttribute("NameOfDepend", configDepend->ClassName.c_str());
				XMLEle->SetAttribute("TypePrefOfDepend", configDepend->ConfTypePrefix.c_str());
				XMLEle->SetAttribute("ModeOfDepend", configDepend->Major);
				LibraryDependends->InsertEndChild(XMLEle);
			}

		}
	}


	//send to xml file.
	std::string t = BASE_DIRECTORY;
	XMLError eResult = xmlDoc.SaveFile(t.append("\\SavedData.xml").c_str());

}

void Config::SendLibraryFileNameToIncludeList()
{
	//todo: make sure the file is put in the allLibraryIncludes folder like this
			//naming may be a problem later on however as libraries with same names .h files will have conflict. for now just do this.


	//#if Using_ModAAConf1_H
	int u;
	//c:\\Users\\Hadi\\OneDrive\\Documents\\VisualStudioprojects\\Projects\\c# apps\\CodeGenerator\\CodeGenerator\\Module1AA\\ModAAConf1.h
//#endif // ModAAConf1_H


	//first make sure that the file does not already include this library
	std::fstream file;
	std::ifstream infile("C:\\Users\\Hadi\\OneDrive\\Documents\\VisualStudioprojects\\Projects\\c# apps\\CodeGenerator\\CodeGenerator\\ConfigTest\\AllLibraryIncludes.h");
	std::string line;
	bool includeFound = false;
	std::string macroString; macroString.append("USING_"); macroString.append(ClassName); macroString.append("_H");
	while (std::getline(infile, line))
	{
		if (line.find(macroString) != -1)
		{
			includeFound = true;
			break;
		}
	}

	//if not found then put it in alllibraries
	if (includeFound == false)
	{
		file.open("C:\\Users\\Hadi\\OneDrive\\Documents\\VisualStudioprojects\\Projects\\c# apps\\CodeGenerator\\CodeGenerator\\ConfigTest\\AllLibraryIncludes.h", std::ios::app);
		file.write("#if ", sizeof(char) * 4);
		file.write(macroString.c_str(), sizeof(char) * macroString.length());
		file.write("\n", sizeof(char) * 1);
		file.write("#include \"", sizeof(char) * 10);
		file.write(FileNameString.c_str(), sizeof(char) * FileNameString.length());
		file.write("\"\n", sizeof(char) * 2);
		file.write("#endif \n", sizeof(char) * 8);
		file.close();

		//also put it in the CGKeywordDefine.h file.
		file.open("C:\\Users\\Hadi\\OneDrive\\Documents\\VisualStudioprojects\\Projects\\c# apps\\CodeGenerator\\CodeGenerator\\ConfigTest\\CGKeywordDefine.h", std::ios::app);
		file.write("#define ", sizeof(char) * 8);
		file.write(macroString.c_str(), sizeof(char) * macroString.length());
		file.write(" ", sizeof(char) * 1);
		file.write("false \n", sizeof(char) * 7);
		file.close();
	} 

}



void Config::PrintHeader(Header theHeader)
{
	std::string header;
	if (theHeader == Config::Header::Defines)
	{
		header = "\n\n\/\/****************************************************\n\/\/defines for the local program. \n\/\/names of defines will be prefixName+suffixMajor_prefixInstance_defineName_instanceOfconfig\n\/\/****************************************************\n\n";
	}
	else if (theHeader == Config::Header::Types)
	{
		header = "\n\n\/\/****************************************************\n\/\/autogenerated enum types \n\/\/****************************************************\n\n";
	}
	else if (theHeader == Config::Header::Top)
	{
		header = "\/\/****************************************************\n\/\/This is an autogenerated configuration file\n\/\/****************************************************\r\n\/\/---------DO NOT EDIT THIS FILE ----------------\n\n\n";
	}


	std::fstream file;

	//put the headers in 
	file.open(CONFIGURATIONFILE_DIRECTORY, std::ios::app);


	file.write(header.c_str(), sizeof(char) * header.length());
	file.close();
}
