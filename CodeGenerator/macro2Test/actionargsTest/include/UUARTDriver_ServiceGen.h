//############################################### 
//this is an autogenerated file using cgen's macro2 command. Dont modify this file here unless it is in user sections 
//###############################################



//Transmit----------------------------------------------

//Macro2 - AERTOS/UtilityService.cgenMM::TopLevel
	 
//Macro2 - AERTOS/UtilityCTOR.cgenMM::UtilityNameCTORSection
//Macro2: UtilityNameCTORSection
	 #define UUartDriver_Transmit_CTOR ActionReq1.SetServiceFunc([](UUartDriver* p, char const* TransmitArg1){p->_Transmit( TransmitArg1); });
	  
	#define UUartDriver_Transmit_Service \
	inline void Transmit(int idOfAO, char const* TransmitArg1)\
	{\
		this->RunRequest1(idOfAO,  TransmitArg1); \
	}\
	\
	inline Token* Transmit_WithWait(int idOfAO, char const* TransmitArg1)\
	{\
		return this->RunRequestWithWait1(idOfAO,  TransmitArg1); \
	}\
	\
	inline Action1_RETURNTYPE_t Transmit_WaitForRequestToFinish1(Token* token)\
	{\
		return this->WaitForRequestToFinish(token  ); \
	}\
	\
	inline void Transmit_DoneWithRequest(Token* tokenOfRequest)\
	{\
		return this->DoneWithRequest1(tokenOfRequest); \
	}
	
	#define UUartDriver_Transmit void _Transmit(int idOfAO, char const* TransmitArg1)
	#define UUartDriver_Transmit_Update bool _TransmitUpdate(ActionRequestObjectArgTDU1<char const*, bool, 10, UUartDriver>* actReq, char const* TransmitArg1)   
//Alldefines ------------------
#define UUartDriver_CTOR \
UUartDriver_Transmit_CTOR
#define UUartDriver_Service \
UUartDriver_Transmit_Service

//Transmit----------------------------------------------

//Macro2 - AERTOS/UtilityService.cgenMM::TopLevel
	 
//Macro2 - AERTOS/UtilityCTOR.cgenMM::UtilityNameCTORSection
//Macro2: UtilityNameCTORSection
	 #define UUartDriverTDU_Transmit_CTOR ActionReq1.SetServiceFunc([](UUartDriverTDU* p, char const* TransmitArg1){p->_Transmit( TransmitArg1); });
	  
	#define UUartDriverTDU_Transmit_Service \
	inline void Transmit(int idOfAO, char const* TransmitArg1)\
	{\
		this->RunRequest1(idOfAO,  TransmitArg1); \
	}\
	\
	inline Token* Transmit_WithWait(int idOfAO, char const* TransmitArg1)\
	{\
		return this->RunRequestWithWait1(idOfAO,  TransmitArg1); \
	}\
	\
	inline Action1_RETURNTYPE_t Transmit_WaitForRequestToFinish1(Token* token)\
	{\
		return this->WaitForRequestToFinish(token  ); \
	}\
	\
	inline void Transmit_DoneWithRequest(Token* tokenOfRequest)\
	{\
		return this->DoneWithRequest1(tokenOfRequest); \
	}
	
	#define UUartDriverTDU_Transmit void _Transmit(int idOfAO, char const* TransmitArg1)
	#define UUartDriverTDU_Transmit_Update bool _TransmitUpdate(ActionRequestObjectArgTDU1<char const*, bool, 10, UUartDriverTDU>* actReq, char const* TransmitArg1)   

//TransmitTDU----------------------------------------------

//Macro2 - AERTOS/UtilityService.cgenMM::TopLevel
	 
//Macro2 - AERTOS/UtilityUpdateCTOR.cgenMM::UtilityNameCTORSection
//Macro2 - AERTOS/UtilityCTOR.cgenMM::UtilityNameCTORSection
//Macro2: UtilityNameCTORSection
	 #define UUartDriverTDU_TransmitTDU_CTOR ActionReq4.SetServiceFunc([](UUartDriverTDU* p, char* TransmitTDUArg1){p->_TransmitTDU( TransmitTDUArg1); });ActionReq4.SetUpdateFunc([](UUartDriverTDU* p, char* TransmitTDUArg1) {return p->_TransmitTDUUpdate(&p->ActionReq4,  TransmitTDUArg1); });
	  
	#define UUartDriverTDU_TransmitTDU_Service \
	inline void TransmitTDU(int idOfAO, char* TransmitTDUArg1)\
	{\
		this->RunRequest4(idOfAO,  TransmitTDUArg1); \
	}\
	\
	inline Token* TransmitTDU_WithWait(int idOfAO, char* TransmitTDUArg1)\
	{\
		return this->RunRequestWithWait4(idOfAO,  TransmitTDUArg1); \
	}\
	\
	inline Action4_RETURNTYPE_t TransmitTDU_WaitForRequestToFinish4(Token* token)\
	{\
		return this->WaitForRequestToFinish(token  ); \
	}\
	\
	inline void TransmitTDU_DoneWithRequest(Token* tokenOfRequest)\
	{\
		return this->DoneWithRequest4(tokenOfRequest); \
	}
	
	#define UUartDriverTDU_TransmitTDU void _TransmitTDU(int idOfAO, char* TransmitTDUArg1)
	#define UUartDriverTDU_TransmitTDU_Update bool _TransmitTDUUpdate(ActionRequestObjectArgTDU4<char*,int8_t,10 ,UUartDriverTDU>* actReq, char* TransmitTDUArg1)   
//Alldefines ------------------
#define UUartDriverTDU_CTOR \
UUartDriverTDU_Transmit_CTOR\
UUartDriverTDU_TransmitTDU_CTOR
#define UUartDriverTDU_Service \
UUartDriverTDU_Transmit_Service\
UUartDriverTDU_TransmitTDU_Service