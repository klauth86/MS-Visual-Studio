#include "SparseCrs.h"
#include <iostream>
#include <conio.h>

const int dimen = 20;

int main()
{
	//SparseCRS sm(dimen, dimen);
	SparseCRS sm(dimen);

	std::vector<int> v;
	for (int i = 1; i <= dimen; i++)
	{
		//sm.Set(i, i, i);
		v.push_back(i);
		//if (i + 1 <= dimen)
		//	sm.Set(1, i, i + 1);
	}
	
	std::cout << "\nMATRIX OUTPUT:\n";

	for (int i = 1; i <= dimen; i++)
	{
		for (int j = 1; j <= dimen; j++)
		{
			if (i <= j)
				sm.Set(1, i, j);
			std::cout << sm.Get(i, j) << " , ";
		}
		std::cout << '\n';
	}

	auto smv = sm.Multiply(v);

	std::cout << "\nVECTOR OUTPUT:\n";
	for (int i = 1; i <= dimen; i++)
	{
		std::cout << v[i - 1] << " , ";
	}

	std::cout << "\nMATRIX VECTOR PRODUCT OUTPUT:\n";
	for (int i = 1; i <= dimen; i++)
	{
		std::cout << smv[i-1] << " , ";
	}

	_getch();
}