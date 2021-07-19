//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using NUnit.Framework;

//namespace BehavioralPatterns.Observer
//{
//   
//    public class WeakEvent
//    {
//        public class Button
//        {
//            public event EventHandler Clicked;
//            public void Fire()
//            {
//                Clicked?.Invoke(this, EventArgs.Empty);
//            }
//        }

//        public class Window : IDisposable
//        {
//            private readonly Button _button;

//            public Window(Button button)
//            {
//                button.Clicked += ButtonClicked;
//                _button = button;
//            }

//            public void ButtonClicked(object sender, EventArgs args)
//            {
//                Console.WriteLine("Button Clicked (Window Handler)");
//            }

//            public void Dispose()
//            {
//                _button.Clicked -= ButtonClicked;
//            }

//            ~Window()
//            {
//                Console.WriteLine("Window Finalized");
//            }
//        }

//        [Test]
//        public void TryWeakObserver()
//        {
//            void FireGC()
//            {
//                Console.WriteLine("Starting GC");
//                GC.Collect();
//                GC.WaitForPendingFinalizers();
//                GC.Collect();
//                Console.WriteLine("Finish GC");
//            }

//            Button button = new Button();
//            Window window = new Window(button);
//            window = null;
//            FireGC();

//            button.Fire();
//        }
//    }
//}
