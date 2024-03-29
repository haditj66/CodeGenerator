//############################################### 
//this is an autogenerated file using cgen's macro2 command. Dont modify this file here unless it is in user sections 
//###############################################


#define UARTDriver_Transmit_CTOR ActionReq1.SetServiceFunc([](UARTDriver* p, char const* msg){p->_Transmit( msg); });


//Transmit----------------------------------------------

	  
	  
	#define UARTDriver_Transmit_Service \
	inline void Transmit(int idOfAO, char const* msg)\
	{\
		this->RunRequest1(idOfAO,  msg); \
	}\
	\
	inline Token* Transmit_WithWait(int idOfAO, char const* msg)\
	{\
		return this->RunRequestWithWait1(idOfAO,  msg); \
	}\
	\
	inline Action1_RETURNTYPE_t Transmit_WaitForRequestToFinish(Token* token)\
	{\
		return this->WaitForRequestToFinish1(token  ); \
	}\
	\
	inline void Transmit_DoneWithRequest(Token* tokenOfRequest)\
	{\
		return this->DoneWithRequest1(tokenOfRequest); \
	}
	
	#define UARTDriver_Transmit void _Transmit(int idOfAO, char const* msg)
	#define UARTDriver_Transmit_Update bool _TransmitUpdate(ActionRequestObjectArgTDU-2* actReq, char const* msg)   
//Alldefines ------------------
#define UARTDriver_CTOR \
UARTDriver_Transmit_CTOR 
#define UARTDriver_Service \
UARTDriver_Transmit_Service 
