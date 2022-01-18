using System.Collections.Generic;

namespace BlazorDSL {
    public class Attribute {
    }

    public class KeyValueAttribute : Attribute {
        public string Name { get; private set; }
        public object Value { get; private set; }

        public KeyValueAttribute(string name, object value) {
            Name = name;
            Value = value;
        }
    }

    public class EmptyAttribute : Attribute {
        private EmptyAttribute() {
        }

        private static EmptyAttribute _instance = new EmptyAttribute();
        public static EmptyAttribute Instance => _instance;
    }

    public class MultipleAttributes : Attribute {
        public Attribute[] Values { get; private set; }

        public MultipleAttributes(Attribute[] attributes) {
            this.Values = attributes;
        }
    }
}
