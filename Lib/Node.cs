using Microsoft.AspNetCore.Components;
using System;

namespace BlazorDSL {
    public abstract class Node {
        protected static Node[] _emptyNodes = new Node[0];
        protected static Attribute[] _emptyAttributes = new Attribute[0];
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

        public TagNode(string tag, Attribute[] attributes, params Node[] inner) {
            Tag = tag;
            Inner = inner;
            Attributes = attributes;
        }

        public TagNode(string tag, params Node[] inner) {
            Tag = tag;
            Inner = inner;
            Attributes = _emptyAttributes;
        }
    }

    public class ArrayNode : Node {
        public Node[] Inner { get; private set; }

        public ArrayNode(Node[] inner) {
            this.Inner = inner;
        }
    }

    public class EmptyNode : Node {
        public static EmptyNode Instance { get; private set; } = new EmptyNode();
        private EmptyNode() {}
    }

    public class ComponentNode : Node {
        public Type Type { get; private set; }
        public Attribute[] Attributes { get; private set; }

        public ComponentNode(Type type) {
            Type = type;
            Attributes = _emptyAttributes;
        }

        public ComponentNode(Type type, Attribute[] attributes) {
            Type = type;
            Attributes = attributes;
        }
    }

    public class RenderFragmentNode : Node {
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public RenderFragment? Fragment { get; private set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

        public RenderFragmentNode(RenderFragment? fragment) {
            Fragment = fragment;
        }
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
