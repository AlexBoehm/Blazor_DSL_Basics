namespace BlazorDSL {
    static class Html {
        public static Node div(params Node[] inner) => new TagNode("div", inner);
        public static Node h1(string text) => new TagNode("h1", Html.text(text));
        public static Node text(string text) => new TextNode(text);
        public static Node p(string text) => new TagNode("p", Html.text(text));
        public static Node button(string text) => new TagNode("button", Html.text(text));
    }
}
