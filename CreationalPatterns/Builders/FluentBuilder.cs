using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Text;

using static System.Console;

namespace CreationalPatterns.Builders
{
    public class FluentBuilder
    {
        #region Model

        public class HtmlElement
    {
        public string Name, Text;
        public List<HtmlElement> Elements = new List<HtmlElement>();
        private const int indentSize = 2;

        public HtmlElement() { }

        public HtmlElement(string name, string text) { Name = name; Text = text; }

        private string ToStringImpl(int indent)
        {
            var sb = new StringBuilder();
            var i = new string(' ', indentSize * indent);
            sb.Append($"{i}<{Name}>\n");
            if (!string.IsNullOrWhiteSpace(Text))
            {
                sb.Append(new string(' ', indentSize * (indent + 1)));
                sb.Append(Text);
                sb.Append("\n");
            }

            foreach (var e in Elements)
                sb.Append(e.ToStringImpl(indent + 1));

            sb.Append($"{i}</{Name}>\n");
            return sb.ToString();
        }

        public override string ToString()
        {
            return ToStringImpl(0);
        }
    }

    #endregion

        public class HtmlBuilder
        {
            private readonly string rootName;
            HtmlElement root = new HtmlElement();

            public HtmlBuilder(string rootName)
            {
                this.rootName = rootName;
                root.Name = rootName;
            }
            public HtmlBuilder AddChild(string childName, string childText)
            {
                var e = new HtmlElement(childName, childText);
                root.Elements.Add(e);
                return this;
            }
            public HtmlElement Build()
            {
                return root;
            }
            public override string ToString()
            {
                return root.ToString();
            }
            public void Clear()
            {
                root = new HtmlElement { Name = rootName };
            }
        }

        [Test]
        public static void Try()
        {
            var builder = new HtmlBuilder("ul");
            builder.AddChild("li", "hello");
            builder.AddChild("li", "world");
            WriteLine(builder.ToString());

            builder.Clear();
            builder.AddChild("li", "hello").AddChild("li", "world");
            WriteLine(builder.Build());
        }
    }
}
