using System.Collections.Generic;
using System.Linq;

namespace BlazorDSL {
    public static partial class Html {
        public static Node div(Attribute[] attributes, params Node[] inner)
            => new TagNode("div", attributes, inner);

        public static Node div(params Node[] inner) => new TagNode("div", inner);
        public static Node div(IEnumerable<Node> inner) => new TagNode("div", inner.ToArray());
        public static Node h1(string text) => new TagNode("h1", Html.text(text));
        public static Node text(string text) => new TextNode(text);
        public static Node p(string text) => new TagNode("p", Html.text(text));
        public static Node button(string text) => new TagNode("button", Html.text(text));
        public static Node button(Attribute[] attributes, string text)
            => new TagNode("button", attributes, Html.text(text));
        public static Node a(Attribute[] attributes, params Node[] inner)
            => new TagNode("a", attributes, inner);
    }
}
