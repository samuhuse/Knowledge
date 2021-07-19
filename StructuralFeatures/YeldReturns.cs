using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructuralFeatures
{
    public class YeldReturns
    {

        [Test]
        public void TryYeldMethod()
        {
            foreach (string item in GetFavouriteWords(new List<string>() { "hello", "word","my","name","is","samuele"}))
            {
                Console.WriteLine(item);
            }
        }

        private IEnumerable<string> GetFavouriteWords(IEnumerable<string> words)
        {
            foreach (string word in words)
            {
                if ("hello".Equals(word, StringComparison.OrdinalIgnoreCase)) yield return word;
                if ("word".Equals(word, StringComparison.OrdinalIgnoreCase)) yield return word;
                if ("samuele".Equals(word, StringComparison.OrdinalIgnoreCase)) yield return word;
            }
        }

    }
}
