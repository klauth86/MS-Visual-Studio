#pragma once
#include <vector>

class SparseCRS
{
protected:
	int _rows, _columns;
	std::vector<int>* rows;
	std::vector<int>* columns;
	std::vector<int>* values;

	void Construct(int rows, int columns);
	void Validate(int row, int col) const;
	void Insert(int index, int row, int col, int val);
	void Remove(int index, int row);
public:
	SparseCRS(int rows, int columns);
	SparseCRS(int dim);
	~SparseCRS();
	int Get(int row, int col) const;
	SparseCRS& SparseCRS::Set(int val, int row, int col);
	std::vector<int> SparseCRS::Multiply(const std::vector<int> & x) const;
};

