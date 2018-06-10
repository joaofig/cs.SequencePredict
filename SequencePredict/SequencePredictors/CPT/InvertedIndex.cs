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
    }
}
