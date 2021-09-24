namespace BlazorDSL {
    public class Attribute {
        public string Name { get; private set; }
        public object Value { get; private set; }

        public Attribute(string name, object value) {
            Name = name;
            Value = value;
        }
    }
}
