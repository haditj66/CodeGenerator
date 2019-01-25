#pragma once


//For config files -------------------------------------
#define VIRTUAL
#define OVERRIDE
#define BASE
#define TYPE char*
#define VARNAME char*
#define VARTYPE char*
#define DEFINEPARAM
#define STATIC

//helper macros
//to create a public define
#define PublicCreation(name,type,value, multiInstance) publicDefVec->push_back((IDefine*)(new Defines<type>(name, value, multiInstance)));
 