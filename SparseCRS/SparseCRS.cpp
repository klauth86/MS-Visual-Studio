#include "SparseCRS.h"

SparseCRS::SparseCRS(int rows, int columns)
{
	Construct(rows, columns);
}

SparseCRS::SparseCRS(int dim)
{
	Construct(dim, dim);
}

SparseCRS::~SparseCRS()
{
	if (this->values != NULL)
	{
		delete this->values;
		delete this->columns;
	}

	delete this->rows;
}

void SparseCRS::Construct(int rows, int columns)
{
	if (rows < 1 || columns < 1) {
		throw "Matrix dimensions must be positive integers ...";
	}

	this->_rows = rows;
	this->_columns = columns;

	this->values = NULL;
	this->columns = NULL;
	this->rows = new std::vector<int>(rows + 1, 1);
}

int SparseCRS::Get(int row, int col) const
{
	this->Validate(row, col);

	int actual;

	for (int i = this->rows->at(row - 1) - 1; i < this->rows->at(row) - 1; i++)
	{
		actual = this->columns->at(i);

		if (actual == col)
		{
			return this->values->at(i);
		}
		else if (actual > col)
		{
			break;
		}
	}

	return 0;
}

SparseCRS& SparseCRS::Set(int val, int row, int col)
{
	this->Validate(row, col);

	int pos = this->rows->at(row - 1) - 1;
	int actual = -1;

	for (; pos < this->rows->at(row) - 1; pos++) {
		actual = this->columns->at(pos);

		if (actual == col)
		{
			break;
		}
		else if (actual > col)
		{
			break;
		}
	}

	if (actual != col)
	{
		if (val != 0)
		{
			this->Insert(pos, row, col, val);
		}
	}
	else if (val == 0)
	{
		this->Remove(pos, row);
	}
	else
	{
		this->values->at(pos) = val;
	}
	return *this;
}

std::vector<int> SparseCRS::Multiply(const std::vector<int> & x) const
{
	if (this->_columns != (int)x.size())
	{
		throw "Cannot multiply: Matrix column count and vector size don't match.";
	}

	std::vector<int> result(this->_rows, 0);

	for (int i = 1; i <= this->_columns; i++)
	{
		for (int j = 1; j <= this->_rows; j++) {
			result[j - 1] += this->Get(j, i) * x[i - 1];
		}
	}

	return result;
}

void SparseCRS::Validate(int row, int col) const
{
	if (row < 1 || col < 1 || row > this->_rows || col > this->_columns) {
		throw "Coordinations out of range.";
	}
}

void SparseCRS::Insert(int index, int row, int col, int val)
{
	if (this->values == NULL)
	{
		this->values = new std::vector<int>(1, val);
		this->columns = new std::vector<int>(1, col);

	}
	else {
		this->values->insert(this->values->begin() + index, val);
		this->columns->insert(this->columns->begin() + index, col);
	}

	for (int i = row; i <= this->_columns; i++)
	{
		this->rows->at(i) = this->rows->at(i) + 1;
	}
}

void SparseCRS::Remove(int index, int row)
{
	this->values->erase(this->values->begin() + index);
	this->columns->erase(this->columns->begin() + index);

	for (int i = row; i <= this->_rows; i++) {
		this->rows->at(i) = this->rows->at(i) - 1;
	}
}

