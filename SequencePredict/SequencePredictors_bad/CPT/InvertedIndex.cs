using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePredictors.CPT
{
    public class InvertedIndex<T> where T : IEquatable<T>
    {
        private Dictionary<T,HashSet<int>> index = new Dictionary<T, HashSet<int>>();

        public void Add(T item, int sequenceId)
        {
            if(!index.ContainsKey(item))
                index[item] = new HashSet<int>();
            index[item].Add(sequenceId);
        }

        public void Add(Sequence<T> sequence)
        {
            foreach(var item in sequence)
                Add(item, sequence.Id);
        }

        public HashSet<int> GetIntersection(IEnumerable<T> sequence)
        {
            HashSet<int> result = new HashSet<int>();

            foreach(var item in sequence)
            {
                if(index.ContainsKey(item))
                {
                    if(result.Count == 0)
                        result = index[item];
                    else
                        result.IntersectWith(index[item]);
                }
            }
            return result;
        }
    }
}
