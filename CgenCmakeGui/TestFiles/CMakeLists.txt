#-----------------------------------------
# Root 
# Targets: FreeRTOS  BSP 
#-----------------------------------------
cmake_minimum_required(VERSION 3.15)
 
project(AERTOS LANGUAGES C CXX ASM)


include("cmake/CgenCmakeConfigFunctions.cmake")


Cgen_Start()


###################################
#User Options
###################################   

 

Cgen_Option(
        NAME AERTOS_ENVIRONMENT
        DESCRIPTION "what environment is this AE application set in"
	POSSIBLEVALUES RTOS_MICRO RTOS_PC
	CONSTRICTS_LATER_OPTIONS RTOS_MICRO@Build_System@VSGDBCmake_Ninja@AmentCmake_colcon;RTOS_PC@Build_System@mingw;
)

#AERTOS_ENVIRONMENT
#POSSIBLE_VALUES RTOS_MICRO ROS_MICRO RTOS_PC ROS_PC AECorePC
set(AERTOS_ENVIRONMENT RTOS_PC)
add_compile_definitions(AERTOS_ENVIRONMENT=${AERTOS_ENVIRONMENT})

#RTOS_USED
#POSSIBLE_VALUES FREERTOS NONE
set(RTOS_USED FREERTOS)
add_compile_definitions(RTOS_USED=${RTOS_USED})

#OS_USED
#POSSIBLE_VALUES Windows Ubuntu NONE
set(OS_USED Windows)
add_compile_definitions(OS_USED=${OS_USED})

#SIM_REAL
#POSSIBLE_VALUES Reality Simulated
set(SIM_REAL Reality)
add_compile_definitions(SIM_REAL=${SIM_REAL})

#SWIL_HWIL_DRIVEN
#POSSIBLE_VALUES SWIL HWIL
set(SWIL_HWIL_DRIVEN SWIL)
add_compile_definitions(SWIL_HWIL_DRIVEN=${SWIL_HWIL_DRIVEN})

#BOARD_USED
#POSSIBLE_VALUES RPI4B JetsonNano X64 STM32F4RE11
set(BOARD_USED X64)
add_compile_definitions(BOARD_USED=${BOARD_USED})





Cgen_End()




