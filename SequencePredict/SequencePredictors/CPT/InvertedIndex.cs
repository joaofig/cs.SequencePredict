using System;
using System.Collections.Generic;

namespace SequencePredictors.CPT
{
    /// <summary>
    /// Implements the inverted index component of the compact prediction tree.
    /// </summary>
    /// <typeparam name="T">The sequence element type.</typeparam>
    public class InvertedIndex<T> where T : IEquatable<T>
    {
        private readonly Dictionary<T,HashSet<int>> index = new Dictionary<T, HashSet<int>>();

        /// <summary>
        /// Adds an item sequence to the inverted index.
        /// </summary>
        /// <param name="sequence">Item sequence</param>
        public void Add(Sequence<T> sequence)
        {
            foreach(var item in sequence)
                Add(item, sequence.Id);
        }

        /// <summary>
        /// Gets the set containing the intersection between the entries in the inverted index and the input sequence.
        /// </summary>
        /// <param name="sequence">Item sequence</param>
        /// <returns>Set containing the intersections.</returns>
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

        /// <summary>
        /// Returns true if the inverted index contains the given item.
        /// </summary>
        /// <param name="key">Item to test.</param>
        /// <returns>true if the inverted index contains this key.</returns>
        public bool ContainsKey(T key)
        {
            return index.ContainsKey(key);
        }

        /// <summary>
        /// Returns the set of sequence indexes that contain the given key.
        /// </summary>
        /// <param name="key">Retrieve the inclusion set for this item</param>
        /// <returns>An inclusion set containing all the sequence indexes that contain this item.</returns>
        public HashSet<int> Get(T key)
        {
            return index[key];
        }

        //---------------------------------------------------------------------

        private void Add(T item, int sequenceId)
        {
            if(!index.ContainsKey(item))
                index[item] = new HashSet<int>();
            index[item].Add(sequenceId);
        }
    }
}
