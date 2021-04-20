using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreationalPatterns.Builders
{
    public class FunctionalBuilder
    {

        #region Model

        public class Alien
    {
        public long id { get; set; }
        public string Race { get; set; }
    }

    #endregion

        public abstract class FunctionalBuilderbase<TSubject, TSelf> 
            where TSelf : FunctionalBuilderbase<TSubject, TSelf> 
            where TSubject : new()
        {
            protected List<Func<TSubject, TSubject>> actions = new List<Func<TSubject, TSubject>>();

            public TSelf AddAction(Action<TSubject> action)
            {
                actions.Add(a => { action(a); return a; });
                return (TSelf)this;
            }

            public virtual TSubject Build()
            {
                throw new NotImplementedException();
            }
        }

        public class AlienBuilder : FunctionalBuilderbase<Alien, AlienBuilder>
        {
            Alien _alien;

            public AlienBuilder SetId(long id)
            {
                return AddAction(a => a.id = id);
            }

            public override Alien Build()
            {
                return actions.Aggregate(new Alien(), (a, f) => f(a));
            }
        }

        [Test]
        public static void Try()
        {
            var builder = new AlienBuilder();
            Alien alien = builder.SetId(1000).AddAction((a) => a.Race = "Grey").Build();
        }
    }
}
