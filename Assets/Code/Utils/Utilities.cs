using System.Collections.Generic;
using UnityEngine;

namespace Code.Utils
{
    public static class Utilities
    {
        public static void Shuffle<T>(List<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = Random.Range(0, n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}