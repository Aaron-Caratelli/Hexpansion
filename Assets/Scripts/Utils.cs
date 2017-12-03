using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Hexpansion
{
    public static class Utils
    {
        public static void PrintStrings(ICollection<string> strings)
        {
            foreach (var s in strings)
                Debug.Log(s);
        }
    }

}
