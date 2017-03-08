using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleQueue
{
    class MyQueue<T>
    {
        private readonly TimeSpan _timeout = TimeSpan.FromMilliseconds(10);
        private readonly object _locker = new object();
        private readonly object _lockerForCheck = new object();

        private Queue<T> _data= new Queue<T>();

        public void Enqueue(T param)
        {
            lock (_locker)
            {
                _data.Enqueue(param);
            }
        }

        public T Dequeue()
        {
            lock(_lockerForCheck)
            {
                while (_data.Count == 0)
                    ;

                lock (_locker)
                {
                    return _data.Dequeue();
                }
            }
        }
    }
}
