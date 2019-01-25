#pragma once

#include <string>
#include "tinyxml2.h"

using namespace tinyxml2;

struct EnumElement
{
	int Value;
	std::string Name;
	XMLElement* enumXMLEle;

	EnumElement(int value, std::string name, XMLDocument* const xmlDoc) : Value(value), Name(name) { 
		enumXMLEle = xmlDoc->NewElement("EnumDefinition");
		enumXMLEle->SetAttribute("Name", name.c_str());
		enumXMLEle->SetAttribute("Value", value);
	};
};