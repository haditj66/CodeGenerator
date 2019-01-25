#pragma once

#include <string>
#include "tinyxml2.h"


using namespace tinyxml2;

enum Privacy
{
	Public,
	Private,
	Template 
}; 

class IDefine
{
public:
	IDefine(); 
	std::string GetBaseName();
	std::string GetFullName();
	virtual std::string GetTypeStr() = 0;
	friend class Config; 

	XMLElement* GetDefineXMLEle();
	

protected:
	XMLElement* defineXMLEle;

	void SetPrefix(std::string prefix); //make protected so only friends can set this.
	void SetPrivacy(Privacy privacy);//make protected so only friends can set this.
	void SetMajor(int major);//make protected so only friends can set this.
	
	virtual void InitializeXMLEle() = 0;

	std::string ValueAsString;
	Privacy MyPrivacy;
	std::string PrefixName;
	std::string DefineName;
	int Major;
	std::string ConfTypePrefix;
	bool IsStatic;
	int InstanceOfConfig;

};

