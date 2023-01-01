//generated file: C:/CodeGenerator/CodeGenerator/macro2Test/CGENTest\conf\AEConfig.h
//**********************************************************************
//this is an auto-generated file using the template file located in the directory of C:\CodeGenerator\CodeGenerator\bin\Debug\..\..\FileTemplates\Files
//ONLY WRITE CODE IN THE UserCode_Section BLOCKS
//If you write code anywhere else,  it will be overwritten. modify the actual template file if needing to modify code outside usersection blocks.

//############################################### 
//this is an autogenerated file using cgen's macro2 command. Dont modify this file here unless it is in user sections 
//###############################################

#pragma once 
 
 
#ifdef  BOARD_USED__STM32F411RE
#include "stm32f4xx_hal.h"
#endif //  BOARD_USED__STM32F411RE

//UserCode_Sectiondefines


//
//#define Event1 I2C_RXCpltEVT
//#define Event1Size 10
 
//DONT DO THIS!!!!!
//#define AnyOtherNeededIncludes2 LoopObjeect1Test 

 
//UserCode_Sectiondefines_end







//####################################
//AE Settings
//####################################

#define AEconfigTICK_RATE_HZ 1000
#define AEconfigMINIMAL_STACK_SIZE 928
#define AEconfigTIMER_TASK_STACK_DEPTH 1500
#define AEconfigTOTAL_HEAP_SIZE 85000
#define AOPRIORITYLOWEST 5
#define AOPRIORITYMEDIUM 10
#define AOPRIORITYHIGHEST 29
#define NUMOFACTIVEOBJECTS 14
#define HIGHEST_NUM_OF_EVT_INSTANCES 2
#define MAXSPB_CHAIN_POOLSIZE 5
#define MAXNUMOFINTERPRETORS 3
#define MAXNUMOFOBSERVERINFLUENCES 2
#define MAXNUMOFOBSERVORS 8
#define MAXNUMOFSENSORS 4
#define MAXNUMBEROF_FILTERS 2
#define MAXNUMBEROF_FILTERS_From_A_Sensor 2
#define MaxNumOfAELoops 3
#define MAXNUMBEROFINPUTSIGNALS_TO_A_SPB 3
#define MAXNUMBEROFOUTPUTSIGNALS_TO_A_SPB 3
#define MAXNUM_OF_SUBSCRIBERS_To_A_SPB 3
#define MAXNUM_OF_AE_SUBSCRIPTIONS_To_SPBs 3
#define TOTALMAXNUMBEROFOUTPUTSIGNALS_TO_ALL_SPBs 25
#define MAXNUMOFTDUSSETTOTheSameSPB 3
#define configAE_USE_TDUs_AsService 1
#define configAE_USE_U_AsService 1
#define configAE_USE_DDSM_AsService 0
#define DontCheckForHardDeadlinesInSPBsForEverySetInput
#define AEDontCheckForCorrectActionRequestTemplate
#define Target_stm32f4
#define SPB_OF_FILTER1_SUBSCRIBED
#define SPB_OF_FILTER2_SUBSCRIBED








//####################################
//AO defines
//####################################

#define Event1 I2C_RXCpltEVT
#define Event1Size 10

#define Signal1 Button1

#define Signal2 Button2

#define Signal3 Button3

#define ADCPERIPHERAL1 ADCPeripheral<1, PortB, PIN0, PortA, PIN2> 
#define ADCPERIPHERAL1_Name_CH1 ADCPERIPHERAL_inst1_ch1
#define ADCPERIPHERAL1_Name_CH2 ADCPERIPHERAL_inst1_ch2


#define PWMPERIPHERAL1 PWMPeripheral<1, PortD, PIN13> 
#define PWMPERIPHERAL1_Name PWMPERIPHERAL_inst1

#define UARTPERIPHERAL1 UARTPeripheral<115200, 2, PortD, PIN5, PortD, PIN6> 
#define UARTPERIPHERAL1_Name UARTPERIPHERAL_inst1

#define I2CPERIPHERAL1 I2CPeripheral<1, PortB, PIN6, PortB, PIN7, 10000> 
#define I2CPERIPHERAL1_Name I2CPERIPHERAL_inst1

#define GPIOPERIPHERAL1 GPIOPeripheral< PortD, PIN12> 
#define GPIOPERIPHERAL1_Name GPIOPERIPHERAL_inst1

#define GPIOInputPERIPHERAL1 GPIOInputPeripheral< PortD, PIN0> 
#define GPIOInputPERIPHERAL1_Name GPIOInputPERIPHERAL_inst1

#define SPIPERIPHERAL1 SPIPeripheral<1, false, PortA, PIN5, PortA, PIN6, PortA, PIN7, PortA, PIN4> 
#define SPIPERIPHERAL1_Name SPIPERIPHERAL_inst1





#define ClockType1 AEClock<AEObservorSensor, AEObservorInterpretorBaseDUMMY, 3, 0, 1, 0, 0, 0,0, 0, 0,0, 0, 0,0, 0, 0,0, 0, 0,0, 0, 0> 
#define ClockName1 clock1 

#define SensorName1 sensor1

#define SensorName2 sensor2

#define SensorName3 sensor3

#define AOInclude1 AverageSPB
#define TemplateToAO1 template<bool isSubscribable, uint32_t CHANNELCOUNTBUFFER1, TEMPLATESPB_Filters>
#define ClassNameOfAO1 AverageSPB
#define TypeOfAO1_1 AverageSPB< false,  10,  1, Filter<DerivativeFilter, 2>>
#define InstanceNameOfAO1_1 averageSPB1

#define TypeOfAO1_2 AverageSPB< false,  10,  1, Filter<DerivativeFilter, 2>>
#define InstanceNameOfAO1_2 averageSPB2

#define TypeOfAO1_3 AverageSPB< false,  10,  1, Filter<DerivativeFilter, 2>>
#define InstanceNameOfAO1_3 averageSPB3

#define AOInclude2 AdderSPB
#define TemplateToAO2 template< uint16_t NUMOFINPUTSIGNALS, bool isSubscribable, uint32_t CHANNELCOUNTBUFFER, TEMPLATESPB_Filters>
#define ClassNameOfAO2 AdderSPB
#define TypeOfAO2_1 AdderSPB< 3,  false, 10>
#define InstanceNameOfAO2_1 adderSPB

#define AOInclude3 UARTDriver
#define TemplateToAO3 
#define ClassNameOfAO3 UARTDriver
#define TypeOfAO3_1 UARTDriver
#define InstanceNameOfAO3_1 uartDriver

#define AOInclude4 UARTDriverTDU
#define TemplateToAO4 
#define ClassNameOfAO4 UARTDriverTDU
#define TypeOfAO4_1 UARTDriverTDU
#define InstanceNameOfAO4_1 uartDriverTDU

#define AnyOtherNeededIncludes1 DerivativeFilter 












//####################################
//AE hal defines
//####################################

