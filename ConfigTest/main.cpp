 

#include "tinyxml2.h"
#include <iostream>

using namespace tinyxml2;

int main()
{ 

	//example from https://shilohjames.wordpress.com/2014/04/27/tinyxml2-tutorial/
	XMLDocument doc; 

	XMLNode* root = doc.NewElement("Root");
	doc.InsertFirstChild(root);

	XMLElement * pElement = doc.NewElement("IntValue");
	pElement->SetText(10);
	root->InsertEndChild(pElement);

	pElement = doc.NewElement("FloatValue");
	pElement->SetText(0.5f); 
	root->InsertEndChild(pElement);

	pElement = doc.NewElement("Date");
	pElement->SetAttribute("day", 26);
	pElement->SetAttribute("month", "April");
	pElement->SetAttribute("year", 2014);
	pElement->SetAttribute("dateFormat", "26/04/2014"); 
	root->InsertEndChild(pElement);

	auto confElement = doc.NewElement("Config");
	root->InsertEndChild(confElement);
	auto defElement = doc.NewElement("Defines"); 
	defElement->SetText(7);
	confElement->InsertEndChild(defElement);


	XMLError eResult = doc.SaveFile("SavedData.xml");
}