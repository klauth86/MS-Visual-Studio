#include "ThreadSafeMapAtom.h"

ThreadSafeMapAtom::ThreadSafeMapAtom(std::string value):Value(value)
{
}

ThreadSafeMapAtom::~ThreadSafeMapAtom()
{
}

std::string ThreadSafeMapAtom::getValue()
{
	std::this_thread::get_id() == master ?  1 : throw std::exception("You must lock element before using getValue ...");
	return Value;
}

void ThreadSafeMapAtom::setValue(std::string value)
{
	std::this_thread::get_id() == master ? 1 : throw std::exception("You must lock element before using setValue ...");
	Value = value;
}

void ThreadSafeMapAtom::lock()
{
	std::unique_lock<std::mutex> ul(m);
	cv_isFree.wait(ul, [this]() {return master == std::thread::id(); });
	master = std::this_thread::get_id();
}

void ThreadSafeMapAtom::unlock()
{
	std::this_thread::get_id() == master ? 1 : throw std::exception("You must lock element before using unlock ...");
	master=std::thread::id();
	cv_isFree.notify_one();
}