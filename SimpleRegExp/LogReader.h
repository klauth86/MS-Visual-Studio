#pragma once

#ifdef _MSC_VER
#define _CRT_SECURE_NO_WARNINGS
#endif

#include <cstdio>
#include <string.h>

class LogReader
{
public:
	LogReader();
	~LogReader();
	bool Open(const char* filenameParam);
	bool SetFilter(const char* filterParam);
	bool GetNextLine(char* buf, const int bufsize);
	bool IsEOF();
	void Close();
private:
	FILE* fin;
	const char* filter;
	bool GetFilterResult(const char* p);
};

