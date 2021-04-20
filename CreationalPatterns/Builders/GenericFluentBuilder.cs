using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreationalPatterns.Builders
{
    public class GenericFluentBuilder
    {
        #region Model
        public interface ICard
        {
            public string Title { get; set; }
            public long Id { get; set; }
        }

        public class MonsterCard : ICard
        {
            public int Level { get; set; }
            public int Attack { get; set; }
            public int Defense { get; set; }
            public string Effect { get; set; }
            public long Id { get; set; }
            public string Title { get; set; }
        }

        public class TrapCard : ICard
        {
            public string Type { get; set; }
            public string Effect { get; set; }
            public long Id { get; set; }
            public string Title { get; set; }
        }
        #endregion

        public abstract class CardBuilderBase<T> where T : ICard
        {
            protected T card;
            public CardBuilderBase()
            {
                Reset();
            }

            public void Reset()
            {
                card = (T)Activator.CreateInstance(typeof(T));
            }

            public T Build()
            {
                return card;
            }
        }

        public abstract class CardBuilder<Self, T> : CardBuilderBase<T> where T : ICard where Self : CardBuilder<Self, T>
        {
            public Self Title(string title)
            {
                card.Title = title;
                return (Self)this;
            }

            public Self Id(long id)
            {
                card.Id = id;
                return (Self)this;
            }
        }

        public abstract class AbstractMonsterBuilder<Self> : CardBuilder<AbstractMonsterBuilder<Self>, MonsterCard> where Self : AbstractMonsterBuilder<Self>
        {
            public AbstractMonsterBuilder()
            {
                Reset();
                card = new MonsterCard();
            }

            public Self Level(int level)
            {
                card.Level = level;
                return (Self)this;
            }
            public Self Attack(int attack)
            {
                card.Attack = attack;
                return (Self)this;
            }
            public Self Defense(int defense)
            {
                card.Defense = defense;
                return (Self)this;
            }
            public Self Effect(string effect)
            {
                card.Effect = effect;
                return (Self)this;
            }
        }

        public class MonsterBuilder : AbstractMonsterBuilder<MonsterBuilder> { }

        public abstract class AbstractTrapBuilder<Self> : CardBuilder<Self, TrapCard> where Self : AbstractTrapBuilder<Self>
        {
            public AbstractTrapBuilder()
            {
                Reset();
                card = new TrapCard();
            }
            public Self Type(string type)
            {
                card.Type = type;
                return (Self)this;
            }
            public Self Effect(string effect)
            {
                card.Effect = effect;
                return (Self)this;
            }
        }

        public class TrapBuilder : AbstractTrapBuilder<TrapBuilder> { }

        [Test]
        public void Try()
        {
            TrapCard trap = new TrapBuilder().Title("Solemn Judgement")
                                             .Effect("When a monster(s) would be Summoned, OR a Spell/Trap Card is activated: Pay half your LP; negate the Summon or activation, and if you do, destroy that card.")
                                             .Build();

            MonsterCard monster = new MonsterBuilder().Title("Dark Magician Girl")
                                                      .Level(6)
                                                      .Attack(2000)
                                                      .Defense(1700)
                                                      .Effect("Gains 300 ATK for every 'Dark Magician' or 'Magician of Black Chaos' in the GY.")
                                                      .Build();              
        }
    }
}
