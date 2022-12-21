#pragma once
 
#include "AELoopObject.h" 
#include  "AEObjects.h"   

#include "AEIntegrationTesting.h"

 

class AELoopObject1Test : public AELoopObject 
{
	
public:

	AELoopObject1Test() {  
		
//		Subscribe<Button1>(SubToFunction(AELoopObject1Test, Button1_Callback));

	}
	  

	
public: int GetSomeVar()   const {return SomeVar;}
public: bool GetSomeVar2() const {return SomeVar2;} 
public: void SetSomeVar2(bool _SomeVar2) {SomeVar2 = _SomeVar2; }
	
private: int SomeVar = false; //public get protected set
private: bool SomeVar2 = false; //public get public set
public: void UserInitialize(int _SomeVar, bool _SomeVar2)
	{
		SomeVar = _SomeVar;
		SomeVar2 = _SomeVar2;
		
		
		userInitialized = true;
	}
private: bool userInitialized = false;
protected: void CheckIfConfiguredProperly() const override  
	{
		AELoopObject::CheckIfConfiguredProperly();
		if (userInitialized == false)
		{
			// you did not initialize one of your AEobjects
			AEAssertRuntime(userInitialized == true, "user did not call the UserInitailize function for this object"); 
		} 
	}

protected: 
	
	
	void StartAOLoopObject()  override
	{
		//		EventLoopCallBack_t pp = [](AEEventDiscriminator_t* evt, void* p) -> void {
		//			
		//			static_cast<AELoopObject1Test*>(p)->Button1_Callback(evt); 
		//		};
		
//		EventLoopCallBack_t pp = SubToFunction(AELoopObject1Test, Button1_Callback);
//		Subscribe<Button1>(SubToFunction(AELoopObject1Test, Button1_Callback));
		
		AELoopSubscribe(Button1, AELoopObject1Test, Button1_Callback);
	}
	
	void Update() override {
		AEITEST_CGENTest("randomTest", true, "random test")
		auto tt = averageSPB1->GetMyObservorType();
		uartDriver->Transmit(this->GetAO_ID(), "asvsv"); 

	} 
	
	
	void Button1_Callback(AEEventDiscriminator_t* evt)
	{
		AEPrint("AELoop1: button event has been recived!!!----------------------------------\n");
	}
	
	
	
};

