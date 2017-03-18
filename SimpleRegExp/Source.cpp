#include <iostream> 
#include <conio.h>
#include "LogReader.h"

using std::cout;

const int BUFFER_SIZE = 256;

void main()
{
	char buf[BUFFER_SIZE];
	LogReader lr;
	if (lr.Open("C:\\Users\\kehel\\Downloads\\VK.txt"))
	{
		if (lr.SetFilter("*s*ad*"))
		{
			while (!lr.IsEOF())
			{
				if (lr.GetNextLine(buf, BUFFER_SIZE))
					std::cout << buf<<'\n';
			}
		}
		else
			std::cout << "Error! Can't set filter ..." << '\n';
	}
	else
		std::cout << "Error! Can't open file ..." << '\n';
	_getch();
}