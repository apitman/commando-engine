/*
 ***************************************************************************
 * Copyright 2009 Eric Barnes, Ken Hartsook, Andrew Pitman, & Jared Segal  *
 *                                                                         *
 * Licensed under the Apache License, Version 2.0 (the "License");         *
 * you may not use this file except in compliance with the License.        *
 * You may obtain a copy of the License at                                 *
 *                                                                         *
 * http://www.apache.org/licenses/LICENSE-2.0                              *
 *                                                                         *
 * Unless required by applicable law or agreed to in writing, software     *
 * distributed under the License is distributed on an "AS IS" BASIS,       *
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.*
 * See the License for the specific language governing permissions and     *
 * limitations under the License.                                          *
 ***************************************************************************
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commando
{

    class Pool<T> where T : new()
    {
        private List<T> stack_;

        private int tail_;

        public void initializePool(int capacity)
        {

            stack_ = new List<T>(capacity);
            for (int i = 0; i < capacity; i++)
            {
                stack_[tail_] = new T();
            }
            tail_ = capacity - 1;
        }

        public T pop()
        {
            if (tail_ < 0)
            {
                throw new OutOfMemoryException("Not enough objects in Pool");
            }
            T temp = stack_[tail_];
            stack_.RemoveAt(tail_);
            tail_--;
            return temp;
        }

        public void push(T item)
        {
            stack_.Add(item);
            tail_++;
        }
    }
}
