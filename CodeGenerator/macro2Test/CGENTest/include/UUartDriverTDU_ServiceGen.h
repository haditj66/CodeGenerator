//############################################### 
//this is an autogenerated file using cgen's macro2 command. Dont modify this file here unless it is in user sections 
//###############################################


#define UUartDriverTDU_Transmit_CTOR ActionReq1.SetServiceFunc([](UUartDriverTDU* p, char const* msg){p->_Transmit( msg); });


//Transmit----------------------------------------------

	  
	  
	#define UUartDriverTDU_Transmit_Service \
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
	
	#define UUartDriverTDU_Transmit void _Transmit(int idOfAO, char const* msg)
	#define UUartDriverTDU_Transmit_Update bool _TransmitUpdate(ActionRequestObjectArgTDU-2* actReq, char const* msg)   
#define UUartDriverTDU_TransmitTDU_CTOR ActionReq4.SetServiceFunc([](UUartDriverTDU* p, char* msg){p->_TransmitTDU( msg); });ActionReq4.SetUpdateFunc([](UUartDriverTDU* p, char* msg) {return p->_TransmitTDUUpdate(&p->ActionReq4,  msg); });


//TransmitTDU----------------------------------------------

	  
	  
	#define UUartDriverTDU_TransmitTDU_Service \
	inline void TransmitTDU(int idOfAO, char* msg)\
	{\
		this->RunRequest4(idOfAO,  msg); \
	}\
	\
	inline Token* TransmitTDU_WithWait(int idOfAO, char* msg)\
	{\
		return this->RunRequestWithWait4(idOfAO,  msg); \
	}\
	\
	inline Action4_RETURNTYPE_t TransmitTDU_WaitForRequestToFinish(Token* token)\
	{\
		return this->WaitForRequestToFinish4(token  ); \
	}\
	\
	inline void TransmitTDU_DoneWithRequest(Token* tokenOfRequest)\
	{\
		return this->DoneWithRequest4(tokenOfRequest); \
	}
	
	#define UUartDriverTDU_TransmitTDU void _TransmitTDU(int idOfAO, char* msg)
	#define UUartDriverTDU_TransmitTDU_Update bool _TransmitTDUUpdate(ActionRequestObjectArgTDU1* actReq, char* msg)   
//Alldefines ------------------
#define UUartDriverTDU_CTOR \
UUartDriverTDU_Transmit_CTOR \
UUartDriverTDU_TransmitTDU_CTOR 
#define UUartDriverTDU_Service \
UUartDriverTDU_Transmit_Service \
UUartDriverTDU_TransmitTDU_Service 
