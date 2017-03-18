#include <iostream>
#include <thread>
#include <conio.h>
#include "ThreadSafeMap.h"

int main()
{
	ThreadSafeMap sp;

	std::thread t1([&sp]() {
		std::cout << std::this_thread::get_id() << "begins\n";
		try
		{
			sp["1"];
			std::this_thread::sleep_for(std::chrono::seconds(2));
			sp["2"].lock();
			std::cout << std::this_thread::get_id() << '\t' << sp["2"].getValue() << '\n';
		}
		catch (std::exception e)
		{
			std::cout << e.what() << '(' << std::this_thread::get_id() << ')' << '\n';
		}
		std::cout << std::this_thread::get_id() << "ends" << '\n';
	});
	
	std::thread t2([&sp]() {
		std::cout << std::this_thread::get_id() << "begins\n";
		try
		{
			sp["1"];
			sp["2"].lock();
			std::cout << std::this_thread::get_id() << '\t' << sp["2"].getValue() << '\n';
			sp["2"].setValue("22");
			std::cout << std::this_thread::get_id() << '\t' << sp["2"].getValue() << '\n';
			std::this_thread::sleep_for(std::chrono::seconds(2));
			sp["2"].unlock();

		}
		catch (std::exception e)
		{
			std::cout << e.what() << '(' << std::this_thread::get_id() << ')' <<'\n';
		}
		std::cout << std::this_thread::get_id() << "ends" << '\n';
	});

	t1.join();
	t2.join();
	std::cout << "That's all";
	_getch();
}