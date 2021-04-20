using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreationalPatterns.Factories
{
    
    public class AbstractFactory
    {
        #region Client
        public interface IPerson
        {
            public string Name { get; set; }
            public string Surname { get; set; }
        }

        public abstract class PersonFactory
        {
            public abstract IPerson CreatePerson();
        }
        #endregion

        public class AdultPerson : IPerson
        {
            public string JobDescription { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
        }

        public class ChildPerson : IPerson
        {
            public string SudyDescription { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
        }

        public class AdultPersonFactory : PersonFactory
        {
            public override IPerson CreatePerson()
            {
                return new AdultPerson { Name = "Samuele", Surname = "Lombardi", JobDescription = "Software Creator" };
            }
        }

        public class ChildPersonFactory : PersonFactory
        {
            public override IPerson CreatePerson()
            {
                return new ChildPerson { Name = "Samuele", Surname = "Lombardi", SudyDescription = "Telecomunication" };
            }
        }

        public class Client
        {
            private readonly PersonFactory _factory;

            public Client(PersonFactory factory)
            {
                _factory = factory;
            }

            public IPerson GetPerson()
            {
                return _factory.CreatePerson();
            }
        }

        [Test]
        public void Try()
        {
            Client adultclient = new Client(new AdultPersonFactory());
            AdultPerson adult = (AdultPerson)adultclient.GetPerson();

            Client childClient = new Client(new ChildPersonFactory());
            ChildPerson child = (ChildPerson)childClient.GetPerson();
        }
    }
}
