#pragma once

#include "Defines.h"
#include "EnumElement.h"
#include <initializer_list>
#include "IDefine.h" 
#include <list>
#include <vector>
#include <map>
#include <assert.h>
//#include "CGKeywordDefines.h"
#include <iostream>
#include <fstream>
#include "tinyxml2.h"



std::string const BASE_DIRECTORY = "C:\\Users\\Hadi\\OneDrive\\Documents\\VisualStudioprojects\\Projects\\c# apps\\CodeGenerator\\CodeGenerator\\ConfigTest\\";

#define STRINGIFY(MACRO) #MACRO 

// public defines
#define PUBLIC_DEF_START protected: std::string GetFileNameOVERRIDE() override { return __FILE__; } \
void InitListPublicOVERRIDE(std::vector<IDefine*>* publicDefVec) override {
#define END } 
#define PUBLIC_DEF_DECLARE(name, type) public: \
Defines<type>* name;
#define PUBLIC_DEF_CREATION1(name,type,value, Static) name = new Defines<type>(STRINGIFY(name), value, Static); \
publicDefVec->push_back((IDefine*)(name));
#define PUBLIC_DEF_CREATION2(name,type) name = new Defines<type>(STRINGIFY(name), true); \
publicDefVec->push_back((IDefine*)(name));

//private defines
#define PRIVATE_DEF_START protected: \
void InitListPrivateOVERRIDE(std::vector<IDefine*>* privateDefVec) override{  
#define PRIVATE_DEF_CREATION1(name,type,value, Static) privateDefVec->push_back((IDefine*)(new Defines<type>(STRINGIFY(name), value, Static)));
#define PRIVATE_DEF_CREATION2(name,type) privateDefVec->push_back((IDefine*)(new Defines<type>(STRINGIFY(name), true)));



//template defines
#define TEMPLATE_DEF_START protected: \
void InitListTemplateOVERRIDE(std::vector<IDefine*>* templateDefVec) override { 
//#define TEMPLATE_DEF_DECLARE(name, type) protected: \
//Defines<type>* name;
#define TEMPLATE_DEF_CREATION(name,type) templateDefVec->push_back((IDefine*)(new Defines<type>(STRINGIFY(name), name, true))); \
ConfTypePrefix.append(STRINGIFY(name)); ConfTypePrefix.append(std::to_string(name));



//Config Associations 
#define DEPENDS_ON0() () : Config() {
#define DEPENDS_ON1(type,name) (type name) : Config() {\
name->Init();\
name->ParentDepender = this;\
DependConfigs.push_back(name);
#define DEPENDS_ON2(type,name, type2,name2) (type name,type2 name2) : Config() {\
name->Init(); \
name2->Init(); \
name->ParentDepender = this;name2->ParentDepender = this;\
DependConfigs.push_back(name); \
DependConfigs.push_back(name2);
#define DEPENDS_ON3(type,name, type2, name2, type3, name3) (type name,type2 name2, type3 name3) : Config() {\
name->Init(); name2->Init(); name3->Init(); \
name->ParentDepender = this;name2->ParentDepender = this;name3->ParentDepender = this;\
DependConfigs.push_back(name); DependConfigs.push_back(name2); DependConfigs.push_back(name3);
#define DEPENDS_ON4(type,name,type2,name2,type3,name3,type4,name4) (type name,type2 name2, type3 name3,type4 name4) : Config() { \
name->Init(); name2->Init(); name3->Init(); name4->Init(); \
name->ParentDepender = this;name2->ParentDepender = this;name3->ParentDepender = this;name4->ParentDepender = this;\
DependConfigs.push_back(name); DependConfigs.push_back(name2); DependConfigs.push_back(name3); DependConfigs.push_back(name4);
#define DEPENDS_ON5(type,name,type2,name2,type3,name3,type4,name4,type5,name5) (type name,type2 name2, type3 name3,type4 name4,type5 name5) : Config() { \
name->Init(); name2->Init(); name3->Init(); name4->Init(); name5->Init();\
name->ParentDepender = this;name2->ParentDepender = this;name3->ParentDepender = this;name4->ParentDepender = this;name5->ParentDepender = this;\
DependConfigs.push_back(name); DependConfigs.push_back(name2); DependConfigs.push_back(name3); DependConfigs.push_back(name4);DependConfigs.push_back(name5);
#define DEPENDS_ON6(type,name,type2,name2,type3,name3,type4,name4,type5,name5,type6,name6) (type name,type2 name2, type3 name3,type4 name4,type5 name5,type6 name6) : Config() { \
name->Init(); name2->Init(); name3->Init(); name4->Init(); name5->Init();name6->Init();\
name->ParentDepender = this;name2->ParentDepender = this;name3->ParentDepender = this;name4->ParentDepender = this;name5->ParentDepender = this;name6->ParentDepender = this;\
DependConfigs.push_back(name); DependConfigs.push_back(name2); DependConfigs.push_back(name3); DependConfigs.push_back(name4);DependConfigs.push_back(name5);DependConfigs.push_back(name6);
#define DEPENDS_ON7(type,name,type2,name2,type3,name3,type4,name4,type5,name5,type6,name6,type7,name7) (type name,type2 name2, type3 name3,type4 name4,type5 name5,type6 name6,type7 name7) : Config() { \
name->Init(); name2->Init(); name3->Init(); name4->Init(); name5->Init();name6->Init();name7->Init();\
name->ParentDepender = this;name2->ParentDepender = this;name3->ParentDepender = this;name4->ParentDepender = this;name5->ParentDepender = this;name6->ParentDepender = this;name7->ParentDepender = this;\
DependConfigs.push_back(name); DependConfigs.push_back(name2); DependConfigs.push_back(name3); DependConfigs.push_back(name4);DependConfigs.push_back(name5);DependConfigs.push_back(name6);DependConfigs.push_back(name7);
#define DEPENDS_ON8(type,name,type2,name2,type3,name3,type4,name4,type5,name5,type6,name6,type7,name7,type8,name8) (type name,type2 name2, type3 name3,type4 name4,type5 name5,type6 name6,type7 name7,type8 name8) : Config() { \
name->Init(); name2->Init(); name3->Init(); name4->Init(); name5->Init();name6->Init();name7->Init();name8->Init();\
name->ParentDepender = this;name2->ParentDepender = this;name3->ParentDepender = this;name4->ParentDepender = this;name5->ParentDepender = this;name6->ParentDepender = this;name7->ParentDepender = this;name8->ParentDepender = this;\
DependConfigs.push_back(name); DependConfigs.push_back(name2); DependConfigs.push_back(name3); DependConfigs.push_back(name4);DependConfigs.push_back(name5);DependConfigs.push_back(name6);DependConfigs.push_back(name7);DependConfigs.push_back(name8);
#define DEPENDS_ON9(type,name,type2,name2,type3,name3,type4,name4,type5,name5,type6,name6,type7,name7,type8,name8,type9,name9) (type name,type2 name2, type3 name3,type4 name4,type5 name5,type6 name6,type7 name7,type8 name8,type9 name9) : Config() { \
name->Init(); name2->Init(); name3->Init(); name4->Init(); name5->Init();name6->Init();name7->Init();name8->Init();name9->Init();\
name->ParentDepender = this;name2->ParentDepender = this;name3->ParentDepender = this;name4->ParentDepender = this;name5->ParentDepender = this;name6->ParentDepender = this;name7->ParentDepender = this;name8->ParentDepender = this;name9->ParentDepender = this;\
DependConfigs.push_back(name); DependConfigs.push_back(name2); DependConfigs.push_back(name3); DependConfigs.push_back(name4);DependConfigs.push_back(name5);DependConfigs.push_back(name6);DependConfigs.push_back(name7);DependConfigs.push_back(name8);DependConfigs.push_back(name9);
#define DEPENDS_ON10(type, name,type2,name2,type3,name3,type4,name4,type5,name5,type6,name6,type7,name7,type8,name8,type9,name9,type10,name10) (type name,type2 name2, type3 name3,type4 name4,type5 name5,type6 name6,type7 name7,type8 name8,type9 name9,type10 name10) : Config() { \
name->Init(); name2->Init(); name3->Init(); name4->Init(); name5->Init();name6->Init();name7->Init();name8->Init();name9->Init();name10->Init();\
name->ParentDepender = this;name2->ParentDepender = this;name3->ParentDepender = this;name4->ParentDepender = this;name5->ParentDepender = this;name6->ParentDepender = this;name7->ParentDepender = this;name8->ParentDepender = this;name9->ParentDepender = this;name10->ParentDepender = this;\
DependConfigs.push_back(name); DependConfigs.push_back(name2); DependConfigs.push_back(name3); DependConfigs.push_back(name4);DependConfigs.push_back(name5);DependConfigs.push_back(name6);DependConfigs.push_back(name7);DependConfigs.push_back(name8);DependConfigs.push_back(name9);DependConfigs.push_back(name10);

/*
//static defines
//these must be public
#define STATIC_DEF_DECLARE(name, type) public: \
static Defines<type>* name;
#define STATIC_DEF_START protected: \
void InitListStaticOVERRIDE(std::vector<IDefine*>* staticDefVec) override {  
#define STATIC_DEF_CREATION(name,type,value) name = new Defines<type>(STRINGIFY(name), value, false); \
staticDefVec->push_back((IDefine*)(name)); 
*/

using namespace tinyxml2;

class Config
{
public:
	Config* ParentDepender;

	static void PrintDefines(Config& TopLevelConfig);

	Config();//(char* prefix, int major);
	//Config() { ConfTypePrefix = ""; }
	void Init(); 
	bool IsEqual(const Config& configToCompare ) const;


	template<class T> 
	void SetPublicDefineValue(std::string name, T valueToChangeTo);

	
	
	static XMLDocument xmlDoc;
protected:

	
	XMLElement* XmlConfigElement;
	static std::vector<Config*> ConfigsCreatedSoFar;
	std::vector<IDefine*> privateDefineList;
	std::vector<IDefine*> publicDefineList;
	std::vector<IDefine*> templateDefineList;
	std::vector<EnumElement*> enumElementList;
	std::vector<Config*> DependConfigs;
	  
	std::string Prefix;
	int myInstanceNum;
	int Major;
	std::string ConfTypePrefix;
	bool isTopLevel;
	std::string FileNameString;
	std::string ClassName;


	static void AddConfigSoFar(Config* configToAdd);

	static void SendToFile(std::string stringToSend);
	static void SendToConfigFile(std::string stringToSend);
	static void DeleteConfigfile();
	static void ConfigToXml();
	void SendLibraryFileNameToIncludeList();

	virtual void InitListPublicOVERRIDE(std::vector<IDefine*>* publicDefVec) = 0;
	virtual void InitListPrivateOVERRIDE(std::vector<IDefine*>* privateDefVec) = 0;
	virtual void InitListTemplateOVERRIDE(std::vector<IDefine*>* templateDefVec) = 0; 



	virtual std::string GetFileNameOVERRIDE() = 0; 

	//virtual void InitListStaticOVERRIDE(std::vector<IDefine*>* staticDefVec) = 0;


	enum class Header { Defines, Types, Top };
	static void PrintHeader(Header theHeader);
	  
	bool AlreadyInitialized ;
	
};
  
template<class T>
inline void Config::SetPublicDefineValue(std::string name, T valueToChangeTo)
{
	//assert that it even exists
	bool exists = false;
	int index = 0;
	for (auto define : this->publicDefineList)
	{ 
		if (define->DefineName == name)
		{
			exists = true; 
		}
	} 
	assert(exists);//(publicDefineList.find(name) != publicDefineList.end());

	Defines<T>* g = static_cast<Defines<T>*>((publicDefineList[index]));//static_cast<Defines<T>*>((publicDefineList.find(name)->second));
	g->SetValue(valueToChangeTo);
}

 
