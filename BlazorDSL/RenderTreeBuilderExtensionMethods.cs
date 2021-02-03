using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace BlazorDSL {
    public static class RenderTreeBuilderExtensionMethods {
        public static RenderTreeBuilder h1(this RenderTreeBuilder b, string text, [CallerLineNumber] int line = 0) {
            // eine Zeile benötigt mehrere Sequenz-Nummern
            b.OpenRegion(line);
            b.OpenElement(0, "h1");
            b.AddContent(1, text);
            b.CloseElement();
            b.CloseRegion();
            return b;
        }

        public static RenderTreeBuilder div(
            this RenderTreeBuilder b, 
            IEnumerable<KeyValuePair<string, object>> attributes, 
            Action<RenderTreeBuilder> inner, 
            [CallerLineNumber] int line = 0)
        {
            // eine Zeile benötigt mehrere Sequenz-Nummern
            b.OpenRegion(line);
            b.OpenElement(0, "div");
            b.AddMultipleAttributes(1, attributes);
            b.OpenRegion(2);
            inner(b);
            b.CloseRegion();
            b.CloseElement();
            b.CloseRegion();
            return b;
        }

        public static RenderTreeBuilder p(
            this RenderTreeBuilder b,
            string text,
            [CallerLineNumber] int line = 0
        ) {
            // eine Zeile benötigt mehrere Sequenz-Nummern
            b.OpenRegion(line);
            b.OpenElement(0, "p");
            b.AddContent(1, text);
            b.CloseElement();
            b.CloseRegion();
            return b;
        }

        public static RenderTreeBuilder button(
            this RenderTreeBuilder b,
            IEnumerable<KeyValuePair<string, object>> attributes,
            string text,
            [CallerLineNumber] int line = 0
        ) {
            // eine Zeile benötigt mehrere Sequenz-Nummern
            b.OpenRegion(line);
            b.OpenElement(0, "button");
            b.AddMultipleAttributes(1, attributes);
            b.AddContent(2, text);
            b.CloseElement();
            b.CloseRegion();
            return b;
        }
    }

    static class HTML {
        public static KeyValuePair<string, object> Attribute(string name, object value)
            => new KeyValuePair<string, object>(name, value);
    }
}
