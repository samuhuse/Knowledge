using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehavioralPatterns.Observer
{
    public class ObserverCollection
    {
        public class Market
        {
            public BindingList<float> Prices = new BindingList<float>();
        }

        [Test]
        public void Try()
        {
            void CheckList(object sender, ListChangedEventArgs eventArgs)
            {
                if (eventArgs.ListChangedType == ListChangedType.ItemAdded)
                {
                    Console.WriteLine("item added");
                    Console.WriteLine("Price is " + ((BindingList<float>)sender)[eventArgs.NewIndex]);
                }
                if (eventArgs.ListChangedType == ListChangedType.ItemDeleted)
                    Console.WriteLine("item removed");
                if (eventArgs.ListChangedType == ListChangedType.ItemChanged)
                {
                    Console.WriteLine("item changed"); 
                    Console.WriteLine("Price is " + ((BindingList<float>)sender)[eventArgs.NewIndex]);
                }                                
            }

            Market market = new Market();

            market.Prices.ListChanged += CheckList;
            market.Prices.Add(1);
            market.Prices.Add(2);
            market.Prices.Add(3);
            market.Prices.Add(4);
            market.Prices.Add(5);
            market.Prices.Remove(1);
            market.Prices[2] = 4;

        }

    }
}
