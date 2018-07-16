using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SequencePredictors
{
    public class Sequence<T> : IEnumerable<T> where T : IEquatable<T>
    {
        private readonly List<T> list;
        private readonly int id = 0;

        public Sequence(int id, IEnumerable<T> sequence)
        {
            this.id = id;
            list = new List<T>(sequence);
        }

        public int Id
        {
            get { return id; }
        }

        public void Add(T item)
        {
            list.Add(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)list).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)list).GetEnumerator();
        }

        public T Head
        {
            get
            {
                if(list.Count > 0)
                    return list[0];
                else
                    return default(T);
            }
        }

        public Sequence<T> Tail
        {
            get
            {
            if(list.Count > 1)
                return new Sequence<T>(id, list.Skip(1));
            else
                return null;
            }
        }

        public IEnumerable<T> GetLast(int n)
        {
            if(n > list.Count)
                return list;
            else
                return list.Skip(list.Count - n);
        }
    }
}
