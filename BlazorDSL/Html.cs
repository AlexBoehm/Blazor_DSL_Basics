namespace BlazorDSL {
    static class Html {
        #region Tags
        public static Node div(Attribute[] attributes, params Node[] inner)
            => new TagNode("div", attributes, inner);

        public static Node div(params Node[] inner) => new TagNode("div", inner);
        public static Node h1(string text) => new TagNode("h1", Html.text(text));
        public static Node text(string text) => new TextNode(text);
        public static Node p(string text) => new TagNode("p", Html.text(text));
        public static Node button(string text) => new TagNode("button", Html.text(text));
        public static Node button(Attribute[] attributes, string text)
            => new TagNode("button", attributes, Html.text(text));
        #endregion

        #region Attributs
        public static Attribute[] attrs(params Attribute[] attributes)
            => attributes;

        public static Attribute className(string className)
            => new Attribute("class", className);


        #endregion
    }
}
