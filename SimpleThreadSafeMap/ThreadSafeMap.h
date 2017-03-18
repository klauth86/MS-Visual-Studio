#pragma once
#include "ThreadSafeMapAtom.h"
#include <map>

class ThreadSafeMap
{
private:
		std::mutex m;
		std::map<std::string, ThreadSafeMapAtom*> data;
public:
	ThreadSafeMap();
	~ThreadSafeMap();
	ThreadSafeMap(ThreadSafeMap const & other) = delete;
	ThreadSafeMap& operator=(ThreadSafeMap const & other) = delete;

	ThreadSafeMapAtom& operator[](std::string key);
};

