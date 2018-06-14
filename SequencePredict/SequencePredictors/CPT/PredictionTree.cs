using System;
using System.Collections.Generic;
using System.Linq;

namespace SequencePredictors.CPT
{
    public class PredictionTree<T> where T : IEquatable<T>
    {
        private PredictionTree<T> parent = null;
        private List<PredictionTree<T>> children = new List<PredictionTree<T>>();
        private readonly T item = default(T);

        public PredictionTree(T item, PredictionTree<T> parent)
        {
            this.item = item;
            this.parent = parent;
        }

        public T Item
        {
            get { return item; }
        }

        public PredictionTree<T> Parent
        {
            get { return parent; }
        }

        public PredictionTree<T> Add(T item)
        {
            var child = new PredictionTree<T>(item, this);

            children.Add(child);
            return child;
        }

        public PredictionTree<T> Add(Sequence<T> sequence)
        {
            if(!sequence.Any())
                throw new InvalidOperationException("Sequence cannot be empty.");

            var head = sequence.Head;
            var tail = sequence.Tail;

            PredictionTree<T> node = null;

            if(Contains(head))
            {
                node = Get(head);
            }
            else
            {
                node = Add(head);
            }

            if(tail != null && tail.Any())
                return node.Add(tail);
            else
                return node;
        }

        public bool Contains(T item)
        {
            return children.Any(x => x.Item.Equals(item));
        }

        public PredictionTree<T> Get(T item)
        {
            return children.SingleOrDefault(x => x.Item.Equals(item));
        }
    }
}
