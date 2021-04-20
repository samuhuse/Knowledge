using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BehavioralFeatures.Dynamics
{
    public class DynamicObjects
    {
        public class DynamicXmlElement : DynamicObject
        {
            private readonly XElement _node;

            public DynamicXmlElement(XElement node)
            {
                _node = node;
            }

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                XElement element = _node.Element(binder.Name);
                if(element is not null) 
                {
                    result = new DynamicXmlElement(element);
                    return true;
                }

                XAttribute attribute = _node.Attribute(binder.Name);
                if (attribute is not null)
                {
                    result = attribute.Value;
                    return true;
                }

                result = null;
                return false;
            }
        }

        [Test]
        public void TryXmlDynamic()
        {
            string xml =
@"
<people>
<Person name ='samuele'/>
</people>
";
            var node = XElement.Parse(xml);
            dynamic dynamicNode = new DynamicXmlElement(node);

            Console.WriteLine(dynamicNode.Person.name);
        }

    }
}
