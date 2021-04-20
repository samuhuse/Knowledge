#pragma warning disable CS1718

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructuralFeatures
{
    // Reference type that provides synthesized methods to provide value semantics for equality.
    // Records are immutable by default.

    public class Records
    {
        public record Person(string Name, int Age) { }

        public record Female : Person 
        {
            public List<Person> Childs { get; }

            public Female(string Name, int Age, List<Person> childs) : base(Name, Age) => Childs = childs;
         
        }

        [Test]
        public static void Try()
        {
            Person a = new("A", 0);
            Person b = new("B", 1);
            Person b1 = new("B", 1);

            Female a1 = new("A", 0, null);

            #region Comparison

            bool _;

            _ = a == b;  // false
            _ = a == a;  // true
            _ = b == b1; // true

            // To other type
            _ = a == a1; // false

            #endregion

            // Deconstruct the record into its component properties
            string name;
            int age;

            a.Deconstruct(out name, out age);

            #region Clone

            Person b2 = b with { };
            _ = b == b2; // true

            Person b3 = b with { Age = 2 };
            _ = b == b3; // false

            #endregion
        }
    }
}
