using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SequencePredictors.CPT
{
    public class CompactPredictionTree<T> where T : IEquatable<T>
    {
        private PredictionTree<T>   tree            = null;
        private InvertedIndex<T>    invertedIndex   = new InvertedIndex<T>();
        private LookupTable<T>      lookupTable     = new LookupTable<T>();
        private int                 sampleCount     = 0;
        private int                 splitLength     = 0;
        private int                 minRecursion    = 1;
        private int                 maxRecursion    = 4;

        public CompactPredictionTree(int splitLength = 0)
        {
            this.splitLength = splitLength;
        }

        public void Learn(IEnumerable<T> sample)
        {
            Sequence<T>         sequence = new Sequence<T>(++sampleCount, splitLength == 0 ? sample : sample.TakeLast(splitLength));
            PredictionTree<T>   lastNode = null;

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

            invertedIndex.Add(sequence);
            lookupTable.Add(sequence.Id, lastNode);
        }

        public T[] Predict(IEnumerable<T> target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            T[] knownItems  = target.Where(x => invertedIndex.ContainsKey(x)).ToArray();        // Keep only the items that were seen by the inverted index
            T[] result      = new T[0];
            T[] itemArray   = knownItems;
            int initialSize = knownItems.Length;

            for(int i = minRecursion; i <= maxRecursion && result.Length == 0; i++)
            {
                CountTable<T> countTable = new CountTable<T>(invertedIndex, lookupTable);

                int minSize = itemArray.Length - 1;

                RecursiveDivider(itemArray, minSize, countTable, initialSize);

                result = countTable.GetBestSequence();
            }
            return result;
        }

        //

        private void RecursiveDivider(T[] itemArray, int minSize, CountTable<T> countTable, int initialArraySize)
        {
            int size = itemArray.Length;

            if(size > minSize)
            {
                double weight = (double)size / initialArraySize;

                countTable.Update(itemArray, weight);

                for(int n = 0; n < size; n++)
                {
                    T[] subSequence = itemArray.Exclude(n, 1).ToArray();

                    RecursiveDivider(subSequence, minSize, countTable, initialArraySize);
                }
            }
        }
    }
}
