﻿using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
