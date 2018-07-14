using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencePredictors.CPT
{
    public class CountTable<T> where T : IEquatable<T>
    {
        private readonly Dictionary<T, double>  countTable      = new Dictionary<T, double>();
        private readonly HashSet<int>           visitedSets     = new HashSet<int>();
        private readonly InvertedIndex<T>       invertedIndex   = null;
        private readonly LookupTable<T>         lookupTable     = null;

        public CountTable(InvertedIndex<T> index, LookupTable<T> table)
        {
            invertedIndex = index;
            lookupTable = table;
        }

        public void Update(T[] itemArray, double weight)
        {
            HashSet<int>    intersectionSet = invertedIndex.GetIntersection(itemArray);
            HashSet<T>      itemSet         = new HashSet<T>(itemArray);

            foreach(int id in intersectionSet)
            {
                if(!visitedSets.Contains(id))
                {
                    T[]         branch      = lookupTable.GetSequence(id);
                    HashSet<T>  alreadySeen = new HashSet<T>();
                    int         index       = 0;

                    for(index = 0; index < branch.Length && alreadySeen.Count != itemSet.Count; index++)
                    {
                        if(!itemSet.Contains(branch[index]))
                        {
                            alreadySeen.Add(branch[index]);
                        }
                    }

                    int consequentEndPosition = index;
                    for(index = consequentEndPosition; index < branch.Length; index++)
                    {
                        T item = branch[index];
                        double oldValue = 0.0;
                        if(countTable.ContainsKey(item))
                            oldValue = countTable[item];
                        double curValue = 1.0 / (double)intersectionSet.Count;

                        countTable[item] = oldValue + curValue * weight;
                    }
                    visitedSets.Add(id);
                }
            }
        }

        public T[] GetBestSequence()
        {
            double  maxValue0   = double.MinValue;
            double  maxValue1   = double.MinValue;
            T       maxItem     = default(T);

            foreach(T key in countTable.Keys)
            {
                int     count       = invertedIndex.Get(key).Count;
                double  lift        = countTable[key] / count;
                double  confidence  = countTable[key];
                double  score       = confidence;

                if(score > maxValue0)
                {
                    maxValue1   = maxValue0;
                    maxItem     = key;
                    maxValue0   = score;
                }
                else if(score > maxValue1)
                {
                    maxValue1 = score;
                }
            }

            double  diff        = 1.0 - (maxValue1 / maxValue0);
            List<T> predicted   = new List<T>();

            if(!maxItem.Equals(default(T)))
            {
                if(diff >= 0.0 || maxValue1 < 0)
                {
                    predicted.Add(maxItem);
                }
                else if(diff == 0.0 && maxValue1 > 0.0)
                {
                    double  highestScore    = 0.0;
                    T       newBestItem     = default(T);

                    foreach(T key in countTable.Keys)
                    {
                        double value = countTable[key];

                        if(maxValue0 == value && invertedIndex.ContainsKey(key))
                        {
                            double lift = value / invertedIndex.Get(key).Count;
                            double score = lift;

                            if(score > highestScore)
                            {
                                highestScore = score;
                                newBestItem = key;
                            }
                        }
                    }
                    predicted.Add(newBestItem);
                }
            }
            return predicted.ToArray();
        }
    }
}
