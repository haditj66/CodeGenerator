################################################ 
#this is an autogenerated file using cgen's macro2 command. Dont modify this file here unless it is in user sections 
################################################




if(((${_arg_ONLY_CREATE_LIBRARY} STREQUAL 0 )))
Cgen_Start(CGEN_PROJECT_DIRECTORY "C:/CodeGenerator/CodeGenerator/macro2Test/CGENTest")

 
Cgen_Option(
        NAME INTEGRATION_TESTS
        DESCRIPTION "choose an integration test directory to build"
        POSSIBLEVALUES CGENTest 
        CONSTRICTS_LATER_OPTIONS
)

else()
 
Cgen_Start(CGEN_PROJECT_DIRECTORY "${CMAKE_CURRENT_LIST_DIR}")

endif()


if(EXISTS "${CMAKE_CURRENT_LIST_DIR}/AEConfigProjectUser.cmake")
include("${CMAKE_CURRENT_LIST_DIR}/AEConfigProjectUser.cmake")
endif()

CREATE_TARGET_INTEGRATIONEXE(NAME_OF_TARGET CGENTest
LOCATION_OF_TARGET "C:/CodeGenerator/CodeGenerator/macro2Test/CGENTest"
LibrariesToLinkTo AECoreLib 
LIST_OF_TESTS defaultTest 
) 




if(((${_arg_ONLY_CREATE_LIBRARY} STREQUAL 0 )))
 CGEN_END()
  else()
 Cgen_End_Session()
 endif()
 
 


