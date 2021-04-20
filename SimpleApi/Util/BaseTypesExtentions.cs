using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleApi.Util
{
    public static class BaseTypesExtentions
    {
        public static int GetPermutation(this int i)
        {
            return Enumerable.Range(1, i < 1 ? 1 : i).Aggregate((f, x) => f + x);
        }
    }
}
