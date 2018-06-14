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
            CompactPredictionTree<int> cpt = new CompactPredictionTree<int>();

            cpt.Learn(new int[] {1, 2, 3, 4, 6});
            cpt.Learn(new int[] {4, 3, 2, 5});
            cpt.Learn(new int[] {5, 1, 4, 3, 2});
            cpt.Learn(new int[] {5, 7, 1, 4, 2, 3});

            var result = cpt.Predict(new int[] { 1, 4 }, 2);
        }
    }
}
