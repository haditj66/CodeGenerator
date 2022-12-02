#pragma once
#include "AEUtilityAsService.h" 

#include "AEPublishSubscribeManager.h"

#include "ActionRequestForUARTTransmit.h"
 
#include "AE_TDUAsService.h"

#include  "ActionRequestForUARTTransmitTDU.h"


//Hadi:
//- DONE: change all ActionRequestObjectArg1 so that they work with funciton pointers now!
//- I will need to code generate all service helper functions and put them into a .h file I can include.
//		Have a define that will be put in the constructor and function section!  Do the same
//		thing for tdus! (2 days)
// - refactor spbs now QUICKLY! no more than (3 days)!!
// - code generate an intialization of projects quickly and only if it is easy enough to do so!!! no more than (4 days)!!!!
// - rebuild MOST IMPORTANT stuff into modules that I will use from now on! DONT DO STUFF LIKE THE ARDUINO STUFF!!! its not worth it anymore! (3 days!)
// - finally start on my next new big thing!! make sure it is something big I believe in. No small projects anymore!!
//	
//class UUartDriver : public AEService<10, ActionRequestForUARTTransmit>//AEUtilityAsService<10, ActionRequestForUARTTransmit>
class UUartDriver : public AEService<10, 
	ActionRequestObjectArg1<char const*, bool, 10, UUartDriver>> 
{
public:
	 
	
	
	UUartDriver()
	{ 
			ActionReq1.ServiceName = "Transmit"; 
		
		//UARTPERIPHERAL2_Instance->AE_UART_RxCpltCallback = UartdriverRXCallback;
		ActionReq1.SetServiceFunc([](UUartDriver* p, char const* arg1){p->_Transmit(arg1); });
		  
	}; 
	
	
//#include "UUartDriver_SERVICES.h"
	inline void Transmit(int idOfAO, char const* msg)
	{
		this->RunRequest1(idOfAO, msg); 
	}
	
	inline Token* Transmit_WithWait(int idOfAO, char const* msg)
	{
		return this->RunRequestWithWait1(idOfAO, msg); 
	}
	
	inline Action1_RETURNTYPE_t Transmit_WaitForRequestToFinish1(Token* token)
	{
		return this->WaitForRequestToFinish1(token  ); 
	}
	
	
	 
	
	
protected:
	
	void _Transmit(char const* msg)
	{
		AEStopTimer(1);
		auto tr = AEGetTimerDuration(1);
		AELogTimer(1, "UtilityRequestTimeToRun");
		AEITEST_AECoreTestEXE_MUST_PASS_THIS_MANY("actionRequestTime", tr < 500000, "actionRequest timer should be less than .5ms", 60);
		AEPrint(msg, 100);
	}

};



class UUartDriverTDU :   //public AEService<10, ActionRequestForUARTTransmit, AENullActionRequest, AENullActionRequest, ActionRequestObjectArgTDU1>//AETDUAsService<10, ActionRequestForUARTTransmitTDU>
	 public AEService<10, 
	ActionRequestObjectArg1<char const*, bool, 10, UUartDriverTDU>, 
	AENullActionRequest, 
	AENullActionRequest, 
	ActionRequestObjectArgTDU1<char*,int8_t,10 ,UUartDriverTDU>>
{
public: 

	//this tdu needs the Uuartdriver utility.  
	UUartDriverTDU()
	{ 
	
			ActionReq1.ServiceName = "Transmit";
		ActionReq4.ServiceName = "TransmitTDU";
		
		//UARTPERIPHERAL2_Instance->AE_UART_RxCpltCallback = UartdriverRXCallback;
		
		ActionReq1.SetServiceFunc([](UUartDriverTDU* p, char const* arg1){p->_Transmit(arg1); });
		
		ActionReq4.SetServiceFunc([](UUartDriverTDU* p, char* arg1){p->_TransmitTDU(arg1); });
		ActionReq4.SetUpdateFunc([](UUartDriverTDU* p, char* arg1) {return p->_TransmitTDUUpdate(&p->ActionReq4, arg1); });
		 
	}
	
	
protected:
	
	void _Transmit(char const* msg)
	{
		AEStopTimer(1);
		auto tr = AEGetTimerDuration(1);
		AELogTimer(1, "UtilityRequestTimeToRun");
		AEITEST_AECoreTestEXE_MUST_PASS_THIS_MANY("actionRequestTime", tr < 500000, "actionRequest timer should be less than .5ms", 60);
		AEPrint(msg, 100);
	}
	
	 
	char* arg1;
	int32_t timesToPrint;
	
		void _TransmitTDU(char* Argument1) 
		{
			AEStopTimer(1)
			float timel = AEGetTimerDuration(1);
			
			arg1 = Argument1;  
			timesToPrint = 0;
		} 
	
	
	bool _TransmitTDUUpdate(ActionRequestObjectArgTDU1<char*, int8_t, 10, UUartDriverTDU>* a, char* msg)   
		{ 
	
			
			//print it 10 times before the final print
			if (timesToPrint < 10)
			{
				 
				AEPrint("UART TDU print number: %d \n", timesToPrint);
				timesToPrint++;
				
				AEITEST_AECoreTestEXE_MUST_PASS_THIS_MANY("TDUENTERED", true, "TDU wait for event ran", 100);
				return false;
			}
			else
			{
				bool FlashWasCompletedSuccessfullyWithIntterupt =
				a->WaitForEvent<SomeOther_Sig>(
					[](void* s, AEEventDiscriminator_t* evt)->void {
						//SomeOther_Sig* sssc = (SomeOther_Sig*)evt->evt;
						AEPrint("Event SomeOther_Sig recieved");
						AEITEST_AECoreTestEXE_MUST_PASS_THIS_MANY("TDUWaitForEvent", true, "TDU wait for event ran", 10);
						return;
					}
					, 500);
				
	
				AEPrint(msg); 
				a->SetReturnArg(1); 
				return true;
			}
		
			return false;
			 
		}  
	  

};