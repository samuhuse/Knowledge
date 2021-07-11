using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BehavioralPatterns.Observer
{
    public class BidirectionalObserver
    {
        public class Book : INotifyPropertyChanged
        {
            private string _name;
            public string Name
            {
                get { return _name; }
                set
                {
                    if (_name == value) return;
                    _name = value;
                    OnPropertyChanged();
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)      
            {
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(propertyName));                  
            }
        }

        public class BookDisplay : INotifyPropertyChanged
        {
            private string _bookName;
            public string BookName
            {
                get { return _bookName; }
                set
                {
                    if (_bookName == value) return;
                    _bookName = value;
                    OnPropertyChanged();
                }
            }

            public BookDisplay(Book book)
            {
                _bookName = book.Name;
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public sealed class BidirectionalBinding : IDisposable
        {
            private bool disposed;

            public BidirectionalBinding(
              INotifyPropertyChanged first,
              Expression<Func<object>> firstProperty,
              INotifyPropertyChanged second,
              Expression<Func<object>> secondProperty)
            {
                if (firstProperty.Body is MemberExpression firstExpr
                    && secondProperty.Body is MemberExpression secondExpr)
                {
                    if (firstExpr.Member is PropertyInfo firstProp
                        && secondExpr.Member is PropertyInfo secondProp)
                    {
                        first.PropertyChanged += (sender, args) =>
                        {
                            if (!disposed)
                            {
                                secondProp.SetValue(second, firstProp.GetValue(first));
                            }
                        };
                        second.PropertyChanged += (sender, args) =>
                        {
                            if (!disposed)
                            {
                                firstProp.SetValue(first, secondProp.GetValue(second));
                            }
                        };
                    }
                }
            }

            public void Dispose()
            {
                disposed = true;
            }
        }

        [Test]
        public void Try()
        {
            Book book = new Book { Name = "Book" };
            BookDisplay bookDisplay = new BookDisplay(book);

            var binding = new BidirectionalBinding(
                    book,
                    () => book.Name,
                    bookDisplay,
                    () => bookDisplay.BookName);

            Console.WriteLine("Display: " + bookDisplay.BookName);
            Console.WriteLine("Book: " + book.Name);

            book.Name = "Design pattern";
            Console.WriteLine("Display: " + bookDisplay.BookName);
            Console.WriteLine("Book: " + book.Name);

            bookDisplay.BookName = "Pattern Design";
            Console.WriteLine("Display: " + bookDisplay.BookName);
            Console.WriteLine("Book: " + book.Name);
        }
    }
}
