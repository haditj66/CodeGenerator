

	#####delimits a string to whatever delimiter you want.
function (ListToString result delim)
    list(GET ARGV 2 temp)
    math(EXPR N "${ARGC}-1")
    foreach(IDX RANGE 3 ${N})
        list(GET ARGV ${IDX} STR)
        set(temp "${temp}${delim}${STR}")
    endforeach()
    set(${result} "${temp}" PARENT_SCOPE)
endfunction(ListToString)


#NAME: name of the option I want to create
#Description: description of that option to create.
#POSSIBLEVALUES: possible values the option can be. example POSSIBLEVALUES SIMULATION REALITY
#CONSTRICTS_LATER_OPTIONS: what options that may pop up does this option restrict
    #MUST BE IN FORMAT OPTIONTYPE*NAMEOFCONSTRICTED*POSSIBLE*POSSIBLE
    #example SIMULATION*SW_HW_DRIVEN*HWIL*SWIL 
function(Cgen_Option )
    set(options)
    set(oneValueArgs NAME  DESCRIPTION  )
    set(multiValueArgs CONSTRICTS_LATER_OPTIONS POSSIBLEVALUES)
    cmake_parse_arguments(_arg "${options}" "${oneValueArgs}" "${multiValueArgs}" ${ARGN})

  
    # 1.    check if that option has already been set
    if(DEFINED ${_arg_NAME})
        return()
    endif ()
	


    # 2.     overwrite the file cgenCmakeConfigNEXT.txt with ...
		ListToString(possibleStr " " ${_arg_POSSIBLEVALUES})
    file(WRITE ${CGEN_NEXTFILE} "NAME ${_arg_NAME}
DESCRIPTION ${_arg_DESCRIPTION}
POSSIBLEVALUES ${possibleStr}
CONSTRICTS_LATER_OPTIONS ${_arg_CONSTRICTS_LATER_OPTIONS}
")

    # 3.    throw a fatal error to end the execution
    message(FATAL_ERROR "cgen: ${_arg_NAME} needs to be set now")


endfunction()






macro(Cgen_Start )
    set(options)
    set(oneValueArgs   )
    set(multiValueArgs )
    cmake_parse_arguments(_arg "${options}" "${oneValueArgs}" "${multiValueArgs}" ${ARGN})
	
    # 1.    first check that the cgenCmakeCache.cmake and cgenCmakeConfigNEXT.txt
    #       file exists in location ${CMAKE_SOURCE_DIR}/CGensaveFiles
    set(CGEN_DIRECTORY "${CMAKE_CURRENT_BINARY_DIR}/CGensaveFiles" )
    set(CGEN_CACHEFILE "${CGEN_DIRECTORY}/cgenCmakeCache.cmake"  )
    set(CGEN_NEXTFILE "${CGEN_DIRECTORY}/cgenCmakeConfigNEXT.txt"  )
	set(CGEN_STEP1FILE "${CGEN_DIRECTORY}/Dir_Step1.txt"  )
	
    if (NOT EXISTS ${CGEN_DIRECTORY})
        file(MAKE_DIRECTORY ${CGEN_DIRECTORY})
    endif ()
    if (NOT EXISTS ${CGEN_CACHEFILE})
        file(WRITE ${CGEN_CACHEFILE} "")
    endif ()
    if (NOT EXISTS ${CGEN_NEXTFILE})
        file(WRITE ${CGEN_NEXTFILE} "")
    endif ()
	if (NOT EXISTS ${CGEN_STEP1FILE})
        file(WRITE ${CGEN_STEP1FILE} "")
    endif ()

	#write to the step1 file the directories that the cgencmagui will need
	file(WRITE ${CGEN_STEP1FILE} "CMAKE_CURRENT_BINARY_DIR: ${CMAKE_CURRENT_BINARY_DIR}\nCMAKE_CURRENT_SOURCE_DIR: ${CMAKE_CURRENT_SOURCE_DIR}")
	
	     execute_process(COMMAND  cgen cmakegui
            WORKING_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}
            OUTPUT_VARIABLE outVar 
            ERROR_VARIABLE errorVar
			)
			
			
	#read fromt the options save file cgenCmakeCache.cmake
	include(${CGEN_CACHEFILE})
endmacro()








function(Cgen_End )
    set(options)
    set(oneValueArgs   )
    set(multiValueArgs )
    cmake_parse_arguments(_arg "${options}" "${oneValueArgs}" "${multiValueArgs}" ${ARGN})
	
	#write a done in the next file signalling that there are no more options to set
	    file(WRITE ${CGEN_NEXTFILE} "---OPTIONS_DONE---")

endfunction()