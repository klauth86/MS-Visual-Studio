#include <iostream>
#include <conio.h>
#include <algorithm>
#include <math.h>

double Simpson(double(*f)(double), double a, double b, double step)
{
	double S = 0;
	double start = a;
	while (b > start + step)
	{
		S += f(start + step) + 3 * f((3 * start + step) / 3) + 3 * f((3 * start + 2 * step) / 3) + f(start);
		start += step;
	}
	S += f(b) + 3 * f((start + 2 * b) / 3) + 3 * f((2 * start + b) / 3) + f(start);
	S *= std::min(b - a, step);
	S *= 1.0 / 8;
	return S;
}

double f1(double x)
{
	return std::exp(x);
}

int main()
{
	double a = 0.0;
	double b = 2.0;
	std::cout << Simpson(f1, a, b, 4.0)<<'\n';
	std::cout << Simpson(f1, a, b, 1.0) << '\n';
	std::cout << Simpson(f1, a, b, 0.02) << '\n';
	std::cout << Simpson(f1, a, b, 0.01) << '\n';
	std::cout << std::exp(b) - std::exp(a);
	_getch();
}