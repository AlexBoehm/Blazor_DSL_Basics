using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;


namespace BlazorDSL {
    public static partial class Html {
        #region Tags        

        public static Node empty() => EmptyNode.Instance;

        public static Node Tags(params Node[] nodes)
            => new ArrayNode(nodes.ToArray());

        public static Node text(string text)
            => new TextNode(text);

        #endregion

        #region Attributes
        public static Attribute[] attrs(params Attribute[] attributes)
            => attributes;

        public static Attribute attribute(string key, string value)
            => new Attribute(key, value);

        public static Attribute parameter(string name, object value)
            => new Attribute(name, value);

        #endregion        

        #region templateParameter

        public static Attribute templateParameter(string key, params Node[] template)
            => new Attribute(
                 key,
                (RenderFragment)(
                    (RenderTreeBuilder builder) => {
                        Renderer.Render(builder, new ArrayNode(template));
                    }
                )
            );

        public static Attribute templateParameter(string key, Func<Node> template)
            => new Attribute(
                 key,
                (RenderFragment)(
                    (RenderTreeBuilder builder) => {
                        Renderer.Render(builder, template());
                    }
                )
            );

        public static Attribute templateParameter<TContext>(string key, Func<TContext, Node> template)
            => new Attribute(
                key,
                (RenderFragment<TContext>)(
                    (TContext context) =>
                        (RenderTreeBuilder builder) => {
                            Renderer.Render(builder, template(context));
                        }
                    )
                );


        #endregion

        #region Components

        public static Node Component<TComponent>()
            => new ComponentNode(typeof(TComponent));

        public static Node Component<TComponent>(params Attribute[] parameters)
            => new ComponentNode(typeof(TComponent), parameters);

        public static Node Component<TComponent>(Attribute[] parameters, params Node[] childContent)
            => new ComponentNode(
                typeof(TComponent),
                parameters
                    .Concat(
                        new Attribute[] {
                            templateParameter("ChildContent", childContent)
                        }
                    ).ToArray()
                );
        #endregion

        public static Node fragment(RenderFragment fragment)
            => new RenderFragmentNode(fragment);
    }
}
