using System.Collections.Generic;

namespace BlazorDSL {
    public class AttributeBase {        
    }

    public class Attribute : AttributeBase {
        public string Name { get; private set; }
        public object Value { get; private set; }

        public Attribute(string name, object value) {
            Name = name;
            Value = value;
        }
    }

    public class EmptyAttribute : AttributeBase {
        private EmptyAttribute() {
        }

        private static EmptyAttribute _instance = new EmptyAttribute();
        public static EmptyAttribute Instance => _instance;
    }

    public class MultipleAttributes : AttributeBase {
        public AttributeBase[] Values { get; private set; }

        public MultipleAttributes(AttributeBase[] attributes) {
            this.Values = attributes;
        }
    }

    public class BindAttribute : AttributeBase {
        public string UpdatesAttributeName { get; private set; } // i.e. checked

        public Attribute[] Attributes { get; private set; }

        public BindAttribute(Attribute[] attributes, string updatedAttributeName) {
            this.Attributes = attributes;
            this.UpdatesAttributeName = UpdatesAttributeName;
        }
    }
}
