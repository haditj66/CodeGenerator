################################################ 
#this is an autogenerated file using cgen's macro2 command. Dont modify this file here unless it is in user sections 
################################################



#Cgen_Start(CGEN_PROJECT_DIRECTORY "C:/CodeGenerator/CodeGenerator/macro2Test/CGENTest2")

MATH(EXPR _arg_ONLY_CREATE_LIBRARY "${_arg_ONLY_CREATE_LIBRARY}+1")
include("C:/CodeGenerator/CodeGenerator/macro2Test/CGENTest2/AEConfigProject.cmake")
MATH(EXPR _arg_ONLY_CREATE_LIBRARY "${_arg_ONLY_CREATE_LIBRARY}-1")

#CREATE_TARGET_INTEGRATIONEXE(NAME_OF_TARGET CGENTest2
#LOCATION_OF_TARGET "C:/CodeGenerator/CodeGenerator/macro2Test/CGENTest2"
#LibrariesToLinkTo AECoreLib CGENTest_lib
#ONLY_CREATE_LIBRARY TRUE
#)  
#Cgen_End_Session()
 

if(((${_arg_ONLY_CREATE_LIBRARY} STREQUAL 0 )))
Cgen_Start(CGEN_PROJECT_DIRECTORY "C:/CodeGenerator/CodeGenerator/macro2Test/CGENTest3")

 
Cgen_Option(
        NAME INTEGRATION_TESTS
        DESCRIPTION "choose an integration test directory to build"
        POSSIBLEVALUES CGENTest3 
        CONSTRICTS_LATER_OPTIONS
)

else()
 
Cgen_Start(CGEN_PROJECT_DIRECTORY "${CMAKE_CURRENT_LIST_DIR}")

endif()


if(EXISTS "${CMAKE_CURRENT_LIST_DIR}/AEConfigProjectUser.cmake")
include("${CMAKE_CURRENT_LIST_DIR}/AEConfigProjectUser.cmake")
endif()

CREATE_TARGET_INTEGRATIONEXE(NAME_OF_TARGET CGENTest3
LOCATION_OF_TARGET "C:/CodeGenerator/CodeGenerator/macro2Test/CGENTest3"
LibrariesToLinkTo AECoreLib CGENTest2_lib
AnyAdditionalIncludeDirs 
LIST_OF_TESTS defaultTest
) 




if(((${_arg_ONLY_CREATE_LIBRARY} STREQUAL 0 )))
 CGEN_END()
  else()
 Cgen_End_Session()
 endif()
 
 

