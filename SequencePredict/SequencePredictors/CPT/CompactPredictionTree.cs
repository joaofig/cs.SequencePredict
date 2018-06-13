using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SequencePredictors.CPT
{
    public class CompactPredictionTree<T> where T : IEquatable<T>
    {
        private PredictionTree<T>   tree    = null;
        private InvertedIndex<T>    index   = new InvertedIndex<T>();
        private LookupTable<T>      lookup  = new LookupTable<T>();

        public void Add(Sequence<T> sequence)
        {
            PredictionTree<T> lastNode = null;

            if(!sequence.Any())
                throw new InvalidOperationException("Sequence cannot be empty.");

            if(tree == null)
            {
                lastNode = tree = new PredictionTree<T>(sequence.Head, null);

                if(sequence.Tail.Any())
                    lastNode = tree.Add(sequence.Tail);
            }
            else
            {
                lastNode = tree.Add(sequence);
            }

            index.Add(sequence);
            lookup.Add(sequence.Id, lastNode);
        }

        public T Predict(IEnumerable<T> source, int tailSize)
        {
            T[]                 tail        = source.TakeLast(tailSize).ToArray();
            HashSet<int>        set         = index.GetIntersection(tail);
            Dictionary<T,int>   countTable  = new Dictionary<T, int>();
            HashSet<T>          tailHash    = new HashSet<T>(tail);

            foreach(int id in set)
            {
                T[] sequence = lookup.GetReversedConsequent(id, tailHash);

                for(int i = 0; i < sequence.Length; i++)
                {
                    if(!countTable.ContainsKey(sequence[i]))
                        countTable[sequence[i]] = 0;
                    countTable[sequence[i]] += 1;
                }
            }

            T result = default(T);
            int max = 0;
            foreach(var key in countTable.Keys)
            {
                if(countTable[key] > max)
                {
                    result = key;
                    max = countTable[key];
                }
            }
            return result;
        }
    }
}
