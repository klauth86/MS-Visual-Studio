#include "ThreadSafeMap.h"

ThreadSafeMap::ThreadSafeMap()
{
}

ThreadSafeMap::~ThreadSafeMap()
{
	for (auto el : data)
	{
		delete el.second;
		el.second = 0;
	}
}

ThreadSafeMapAtom& ThreadSafeMap::operator[](std::string key)
{
	std::lock_guard<std::mutex> guard(m);
	auto it = data.find(key);
	if (it != data.end())
	{
		return *(it->second);
	}
	else
	{
		data.insert(std::pair<std::string, ThreadSafeMapAtom*>(key, new ThreadSafeMapAtom("_EMPTY")));
		return *data[key];
	}
}
