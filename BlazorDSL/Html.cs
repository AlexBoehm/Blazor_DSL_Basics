namespace BlazorDSL {
    static class Html {
        public static Node div(params Node[] inner) => new TagNode("div", inner);
        public static Node h1(string text) => new TagNode("h1", new[] { Html.text(text) });
        public static Node text(string text) => new TextNode(text);
    }
}
