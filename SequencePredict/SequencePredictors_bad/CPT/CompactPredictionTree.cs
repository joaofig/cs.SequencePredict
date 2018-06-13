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

        public Sequence<T> Predict(IEnumerable<T> source, int tailSize)
        {
            HashSet<int> set = index.GetIntersection(source.TakeLast(tailSize));

            return null;
        }
    }
}
