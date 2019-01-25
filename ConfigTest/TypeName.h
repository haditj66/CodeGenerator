#pragma once

#include <stdint.h>
// default implementation
template <class T>
class TypeName
{
public:
	static const char* Get()
	{
		return typeid(T).name();
	}
};

 

 
template <>
struct TypeName<int>
{
	static const char* Get()
	{
		return "int";
	}
};

template <>
struct TypeName<uint32_t>
{
	static const char* Get()
	{
		return "uint32_t";
	}
};


template <>
struct TypeName<uint16_t>
{
	static const char* Get()
	{
		return "uint16_t";
	}
};


template <>
struct TypeName<uint8_t>
{
	static const char* Get()
	{
		return "uint8_t";
	}
};

template <>
struct TypeName<float>
{
	static const char* Get()
	{
		return "float";
	}
};


template <>
struct TypeName<double>
{
	static const char* Get()
	{
		return "double";
	}
};

template <>
struct TypeName<char>
{
	static const char* Get()
	{
		return "char";
	}
};

template <>
struct TypeName<std::string>
{
	static const char* Get()
	{
		return "string";
	}
};