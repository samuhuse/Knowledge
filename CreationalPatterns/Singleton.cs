using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Text;

namespace CreationalPatterns
{
    public class Singleton
    {
        private Singleton() { }

        private static Singleton _instance;
        public static Singleton Instance { get { if (_instance is null) { _instance = new Singleton(); } return _instance; } }

        //static Singleton()
        //{
        //    _instance = new Singleton();
        //}

        [Test]
        public void Try() 
        {
            Singleton singleton = Singleton.Instance;
            Singleton singleton2 = Singleton.Instance;


        }
    }
}
