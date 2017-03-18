#include "LogReader.h"

LogReader::LogReader()
{
}

LogReader::~LogReader()
{
	Close();
}

bool LogReader::Open(char const * filenameParam)
{	
	Close();
	fin = fopen(filenameParam, "r");
	return (fin != NULL);
}

void LogReader::Close()
{
	if (fin != NULL)
		fclose(fin);
}

bool LogReader::SetFilter(const char * filterParam)
{
	if (filterParam == NULL)
		return false;

	filter = filterParam;
	return true;
}

bool LogReader::GetFilterResult(const char* inputCharsParam)
{
	const char* pf= filter;

	int j = 0;
	int z = -1;

	while (*inputCharsParam)
	{
		if (*(pf + j) == '?')
		{
			j++;
		}
		else if (*(pf + j) == '*')
		{
			j++;
			z = j;
			continue;
		}
		else if (*(pf + j) == *inputCharsParam)
		{
			j++;
		}
		else
		{
			if (z == -1)
				return false;

			for (int k = j; k > z; k--)
				inputCharsParam--;

			j = z;
		}
		inputCharsParam++;
	}
	return !(*(pf + j));
}

bool LogReader::IsEOF()
{
	return fin == NULL || feof(fin);
}

bool LogReader::GetNextLine(char * buf, const int bufsize)
{
	if (fin == NULL || buf == NULL)
		return false;

	char* bufferPos = buf;
	fgets(buf, bufsize, fin);

	auto t= strchr(buf, '\n');
	if (t != NULL)
		*t = '\0';

	if (filter == NULL)
		return true;

	return GetFilterResult(bufferPos);
}
