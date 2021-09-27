﻿using Microsoft.AspNetCore.Components;
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
        public static AttributeBase[] attrs(params AttributeBase[] attributes)
            => attributes;

        public static AttributeBase attribute(string key, string value)
            => new Attribute(key, value);

        public static AttributeBase parameter(string name, object value)
            => new Attribute(name, value);

        public static AttributeBase emptyAttribute()
            => EmptyAttribute.Instance;

        #endregion        

        #region templateParameter

        public static AttributeBase templateParameter(string key, params Node[] template)
            => new Attribute(
                 key,
                (RenderFragment)(
                    (RenderTreeBuilder builder) => {
                        Renderer.Render(builder, new ArrayNode(template));
                    }
                )
            );

        public static AttributeBase templateParameter(string key, Func<Node> template)
            => new Attribute(
                 key,
                (RenderFragment)(
                    (RenderTreeBuilder builder) => {
                        Renderer.Render(builder, template());
                    }
                )
            );

        public static AttributeBase templateParameter<TContext>(string key, Func<TContext, Node> template)
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

        public static Node Component<TComponent>(params AttributeBase[] parameters)
            => new ComponentNode(typeof(TComponent), parameters);

        public static Node Component<TComponent>(AttributeBase[] parameters, params Node[] childContent)
            => new ComponentNode(
                typeof(TComponent),
                parameters
                    .Concat(
                        new AttributeBase[] {
                            templateParameter("ChildContent", childContent)
                        }
                    ).ToArray()
                );
        #endregion

        public static Node fragment(RenderFragment fragment)
            => new RenderFragmentNode(fragment);
    }
}
