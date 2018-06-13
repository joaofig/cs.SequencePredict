﻿using System;
using System.Collections.Generic;

namespace SequencePredictors.CPT
{
    public class LookupTable<T> where T : IEquatable<T>
    {
        private Dictionary<int,PredictionTree<T>> lookup = new Dictionary<int, PredictionTree<T>>();

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

        public T[] GetReversedConsequent(int sequenceId, HashSet<T> sequenceTail)
        {
            if(lookup.ContainsKey(sequenceId))
            {
                
                List<T> list = new List<T>();
                for(var node = lookup[sequenceId]; node != null; node = node.Parent)
                {
                    if(!sequenceTail.Contains(node.Item))
                        list.Add(node.Item);
                    else
                        break;
                }
                return list.ToArray();
            }
            else
                return null;
        }
    }
}
