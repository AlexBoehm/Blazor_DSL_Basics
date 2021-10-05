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

    public class BindAttribute : AttributeBase {
        public BindAttribute(ValueWithSetter value) {

        }
    }
}
