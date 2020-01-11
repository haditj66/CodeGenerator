#pragma once
#include <string>
#include <assert.h>

#include "Config.h"
#include "IDefine.h"
#include "TypeName.h"
#include <exception>

#define STRINGIFY(MACRO) #MACRO


template<class T>
class Defines : public IDefine
{
public:

	friend class Config;

	std::string namestringify = typeid(T).name();//STRINGIFY(T);
	

	Defines(std::string  defineName, T value, bool IsStatic);
	Defines(std::string  defineName, bool IsStatic);

	void SetValue(T value);
	T GetValue();
	void operator=(T  value) ; 

private: 
	 
	T Value;
	//TypeNameStr<T> TypeStr;
	TypeName<T> TypeStr;
	

	bool ValueAlreadySet;
	void SetValueAsString();

	 
	virtual std::string GetTypeStr() override
	{
		return namestringify;//TypeStr.Get();
	}


	// Inherited via IDefine
	virtual void InitializeXMLEle() override
	{ 
		defineXMLEle = Config::xmlDoc.NewElement("Define"); 

		defineXMLEle->SetAttribute("DefineName", this->DefineName.c_str());
		defineXMLEle->SetAttribute("Type", this->TypeStr.Get());
		defineXMLEle->SetAttribute("Value", this->ValueAsString.c_str());
		std::string privacyAsString = MyPrivacy == Privacy::Private ? "private" : MyPrivacy == Privacy::Public ? "public" : "template";
		defineXMLEle->SetAttribute("MyPrivacy", privacyAsString.c_str());
		defineXMLEle->SetAttribute("PrefixName", PrefixName.c_str());
		defineXMLEle->SetAttribute("IsStatic", IsStatic);
	}

};



 
template<class T>
inline void Defines<T>::SetValue(T value)
{
	// I need to check if this is static and if this has already been set. 
	//if it has already been set, it needs to fail an assertion.
	//as statics can only be set once.
	//THIS ASSERTION DOES NOT NEED TO BE MADE AS THERE CAN BE MANY CHANGES EVEN TO A STATIC
	//assert(!(IsStatic == true && ValueAlreadySet == true));
	 
	 
	//todo create an assertion that makes sure this config was initialized first. 
	if (this == nullptr)
	{ 
		std::cout << "PROBLEM:  you probably did not assert the config first before setting the define values." << std::endl; 
		//assert(false && "-----------------------------------------------PROBLEM:  ---------------------------------\n you probably did not assert the config first before setting the define values.\n------------------------------------------------------\n------------------");//(this != nullptr)
			
	}  
	else
	{
		
	}

	//only public members should show
	Value = value;

	//make as string as well
	SetValueAsString();

	ValueAlreadySet = true;

}

template <class T>
T Defines<T>::GetValue()
{
	return Value;
}

template<class T>
inline void Defines<T>::operator=(T value) 
{
	SetValue(value);
}

 
  


template<class T>
Defines<T>::Defines(std::string defineName, T value,  bool isStatic )
{
	DefineName = defineName;
	Value = value;
	IsStatic = isStatic;
	ValueAlreadySet = false;
	SetValueAsString(); 
}

template<class T>
inline Defines<T>::Defines(std::string defineName, bool isStatic)
{
	DefineName = defineName; 
	IsStatic = isStatic;
	ValueAlreadySet = false; 
}

template<class T>
inline void Defines<T>::SetValueAsString()
{
	//for anything else just do this
	ValueAsString = std::to_string((int)Value);
}
template<>
inline void Defines<int>::SetValueAsString()
{
	ValueAsString = std::to_string(Value);
}
template<>
inline void Defines<uint32_t>::SetValueAsString()
{
	ValueAsString = std::to_string(Value);
}
template<>
inline void Defines<float>::SetValueAsString()
{
	ValueAsString = std::to_string(Value);
}

template<>
inline void Defines<std::string>::SetValueAsString()
{
	ValueAsString = "";
	ValueAsString.append("\"");
	ValueAsString.append(Value);
	ValueAsString.append("\"");
}
template<>
inline void Defines<char>::SetValueAsString()
{
	ValueAsString = "";
	ValueAsString.append("\'");
	ValueAsString.append(std::to_string(Value));
	ValueAsString.append("\'");
}

template<>
inline void Defines<stringWithoutQuotations>::SetValueAsString()
{
	ValueAsString = "";
	//ValueAsString.append("\'");
	std::string s(Value);
	ValueAsString.append(s);
	//ValueAsString.append("\'");
}
 
 
