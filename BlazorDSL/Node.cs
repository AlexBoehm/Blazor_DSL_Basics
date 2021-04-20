using System;

namespace BlazorDSL {
    public abstract class Node {
    }

    public class TextNode : Node {
        public string Text { get; private set; }

        public TextNode(string text) {
            Text = text;
        }
    }

    public class TagNode : Node {
        public string Tag { get; private set; }
        public Node[] Inner { get; private set; }
        public Attribute[] Attributes { get; private set; }

        public TagNode(string tag) {
            Tag = tag;
            Inner = _emptyNodes;
            Attributes = _emptyAttributes;
        }

        public TagNode(string tag, Node[] inner) {
            Tag = tag;
            Inner = inner;
            Attributes = _emptyAttributes;
        }

        protected static Node[] _emptyNodes = new Node[0];
        protected static Attribute[] _emptyAttributes = new Attribute[0];
    }

    public class Attribute {
        public string Name { get; set; }
        public object Value { get; set; }

        public Attribute(string key, object value) {
            this.Name = key;
            this.Value = value;
        }
    }
}
