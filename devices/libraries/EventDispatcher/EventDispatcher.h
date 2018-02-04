#ifndef EventDispatcher_h
#define EventDispatcher_h

#include "functional"
#include "vector"

namespace ART
{
	class EventDispatcher
	{

	public:

		template<typename Function>
		void addListener(Function && fn)
		{
			_functions.push_back(std::forward<Function>(fn));
		}

		void invoke_all()
		{
			for (auto && fn : _functions)
				fn();
		}

	private:

		std::vector<std::function<void()>> _functions;

	};
}

#endif