using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StructuralFeatures.Reflection
{
    [Serializable]
    public class ReflectionBasics
    {
        public class DummyType
        {
            public int MyInteger { get; set; }
            public string MyString { get; set; }

            [Obsolete]
            public void Do() { }
            public string GetString(string message) { return message; }
            public int GetInteger(int x) { return x; }
            public void GetInteger(out int x) { x = 5; }
            public static string GetStringStatic(string message) { return message; }

            public DummyType(string message)
            {

            }

            public class InnerDummyClass
            {
                public void Do() { }
            }
        }

        [Test]
        public void TryConstructor()
        {
            string aa = typeof(DummyType).Assembly.GetName().Name;

            DummyType dummy = typeof(DummyType)
                              .GetConstructor(new [] {typeof(string)})
                              .Invoke(new object[] {""}) as DummyType;

            dummy = Activator.CreateInstance(typeof(DummyType), "") as DummyType;

            DummyType.InnerDummyClass innerDummy = typeof(DummyType).GetTypeInfo()
                                                   .GetNestedTypes().First()
                                                   .GetConstructor(Array.Empty<Type>())
                                                   .Invoke(Array.Empty<object>()) as DummyType.InnerDummyClass;
        }

        [Test]
        public void TryMethods()
        {
            Type type = typeof(DummyType);

            List<MethodInfo> methods = type.GetMethods().ToList();
            methods.ForEach(m => Console.WriteLine(m.Name));

            string? message = type.GetMethod("GetString").Invoke(new DummyType(""), new object[] { "message" }) as string;
            message = methods.Where(m => m.IsStatic).FirstOrDefault()?.Invoke(null, new object[] { "message" }) as string;

            object[] x = new object[1];
            type.GetMethod("GetInteger", new Type[] { typeof(int).MakeByRefType() }).Invoke(new DummyType(""), x);
            Console.WriteLine(x[0]);
        }

        [Test]
        public void TryPropreties()
        {
            Type type = typeof(DummyType);

            List<PropertyInfo> proprieties = type.GetProperties().ToList();
            proprieties.ForEach(m => Console.WriteLine(m.Name));

            DummyType dummy = new DummyType("");

            type.GetProperty("MyInteger").SetValue(dummy,100);
            int? integer = (int)type.GetProperty("MyInteger").GetValue(dummy);
        }

    }
}
