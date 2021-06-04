﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorDSL {
    static class Html {
        #region Tags
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

        public static Node empty() => EmptyNode.Instance;
        
        public static Node Tags(params Node[] nodes)
            => new ArrayNode(nodes.ToArray());

        #endregion

        #region Attributes
        public static Attribute[] attrs(params Attribute[] attributes)
            => attributes;

        public static Attribute parameter(string name, object value)
            => new Attribute(name, value);

        public static Attribute className(string className)
            => new Attribute("class", className);

        public static Attribute onClick(object sender, Action callback)
            => new Attribute(
                "onclick",
                EventCallback.Factory.Create<MouseEventArgs>(sender, callback)
            );

        #endregion

        #region Components

        public static Node Component<TComponent>()
            => new ComponentNode(typeof(TComponent));

        public static Node Component<TComponent>(params Attribute[] parameters)
            => new ComponentNode(typeof(TComponent), parameters);

        #endregion


    }
}
