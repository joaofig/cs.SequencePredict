using SequencePredictors;
using SequencePredictors.CPT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencePredict
{
    class Program
    {
        static void Main(string[] args)
        {
            Sequence<int> s1 = new Sequence<int>(1, new int[] {1, 2, 3, 4, 6});
            Sequence<int> s2 = new Sequence<int>(2, new int[] {4, 3, 2, 5});
            Sequence<int> s3 = new Sequence<int>(3, new int[] {5, 1, 4, 3, 2});
            Sequence<int> s4 = new Sequence<int>(4, new int[] {5, 7, 1, 4, 2, 3});


            CompactPredictionTree<int> cpt = new CompactPredictionTree<int>();

            cpt.Add(s1);
            cpt.Add(s2);
            cpt.Add(s3);
            cpt.Add(s4);

            var result = cpt.Predict(new int[] { 1, 4 }, 2);
        }
    }
}
