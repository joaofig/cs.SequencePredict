using System;
using System.Collections.Generic;

namespace SequencePredictors.CPT
{
    public class LookupTable<T> where T : IEquatable<T>
    {
        private readonly Dictionary<int,PredictionTree<T>> lookup = new Dictionary<int, PredictionTree<T>>();

        public void Add(int sequenceId, PredictionTree<T> node)
        {
            lookup.Add(sequenceId, node);
        }

        public bool Contains(int sequenceId)
        {
            return lookup.ContainsKey(sequenceId);
        }

        public PredictionTree<T> Get(int sequenceId)
        {
            return lookup[sequenceId];
        }

        public T[] GetSequence(int sequenceId)
        {
            PredictionTree<T> node = lookup[sequenceId];
            List<T> list = new List<T>
            {
                node.Item
            };

            while (node.Parent != null)
            {
                node = node.Parent;
                list.Add(node.Item);
            }
            list.Reverse();
            return list.ToArray();
        }
    }
}
