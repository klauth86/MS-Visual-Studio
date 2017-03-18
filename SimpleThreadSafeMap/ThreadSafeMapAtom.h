#pragma once
#include <string>
#include <mutex>
#include <condition_variable>
#include <thread>
#include <iostream>


class ThreadSafeMapAtom
{
public:
	explicit ThreadSafeMapAtom(std::string);
	~ThreadSafeMapAtom();
	ThreadSafeMapAtom(ThreadSafeMapAtom const & other) = delete;
	ThreadSafeMapAtom& operator=(ThreadSafeMapAtom const & other) = delete;

	std::string getValue();
	void setValue(std::string);

	void lock();
	void unlock();

private:
	std::string Value;
	std::mutex m;
	std::condition_variable cv_isFree;
	std::thread::id master;
};

