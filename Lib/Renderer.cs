using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace BlazorDSL {
    public static class Renderer {
        public static void Render(RenderTreeBuilder builder, Node node) {
            Render(builder, node, 0);
        }

        private static int Render(RenderTreeBuilder builder, Node node, int sequenceNumber) {
            return node switch {
                TagNode n => RenderTagNode(builder, sequenceNumber, n),
                TextNode n => RenderTextNode(builder, sequenceNumber, n),
                ArrayNode n => RenderArrayNode(builder, sequenceNumber, n),
                EmptyNode _ => RenderEmptyNode(builder, sequenceNumber),
                ComponentNode n => RenderComponentNode(builder, sequenceNumber, n),
                RenderFragmentNode n => RenderFragmentNode(builder, sequenceNumber, n),
                _ => throw new Exception("Unexpected node of Type " + node.GetType().FullName)
            };
        }

        private static int RenderTagNode(RenderTreeBuilder builder, int sequenceNumber, TagNode n) {
            builder.OpenRegion(sequenceNumber);
            builder.OpenElement(0, n.Tag);

            AddAttributes(1, builder, n.Attributes);

            var nextSequenceInRegion = 2;

            for (int i = 0; i < n.Inner.Length; i++) {
                nextSequenceInRegion = Render(builder, n.Inner[i], nextSequenceInRegion);
            }

            builder.CloseElement();
            builder.CloseRegion();
            return sequenceNumber + 1;
        }

        private static int RenderTextNode(RenderTreeBuilder builder, int sequenceNumber, TextNode n) {
            builder.AddContent(sequenceNumber, n.Text);
            return sequenceNumber + 1;
        }

        private static int RenderArrayNode(RenderTreeBuilder builder, int sequenceNumber, ArrayNode n) {
            builder.OpenRegion(sequenceNumber);

            var nextSequenceInRegion = 0;

            for (int i = 0; i < n.Inner.Length; i++) {
                nextSequenceInRegion = Render(builder, n.Inner[i], nextSequenceInRegion);
            }

            builder.CloseRegion();

            return sequenceNumber + 1;
        }

        private static int RenderEmptyNode(RenderTreeBuilder builder, int sequenceNumber) {
            // Nichts zu rendern, die Sequenznummer muss jedoch erhöht werden
            return sequenceNumber + 1;
        }

        private static int RenderComponentNode(RenderTreeBuilder builder, int sequenceNumber, ComponentNode n) {
            builder.OpenRegion(sequenceNumber);
            builder.OpenComponent(0, n.Type);
            AddAttributes(1, builder, n.Attributes);
            builder.CloseComponent();
            builder.CloseRegion();
            return sequenceNumber + 1;
        }

        private static int RenderFragmentNode(RenderTreeBuilder builder, int sequenceNumber, RenderFragmentNode n) {
            builder.AddContent(sequenceNumber, n.Fragment);
            return sequenceNumber + 1;
        }

        private static void AddAttributes(int sequenceNumber, RenderTreeBuilder builder, AttributeBase[] attributes) {
            
            var allAttributes = GetAttributes(attributes);

            builder.AddMultipleAttributes(
                sequenceNumber,
                from attribute in allAttributes                
                select new KeyValuePair<string, object>(attribute.Name, attribute.Value)
            );
        }

        private static IEnumerable<Attribute> GetAttributes(AttributeBase[] attributes) {
            return attributes.Select(GetAttributes).SelectMany(x => x);
        }

        private static IEnumerable<Attribute> GetAttributes(AttributeBase attribute) {
            return attribute switch {
                Attribute a => new Attribute[] { a },
                // PreventDefaultAttribute a => new Attribute[] { a },
                EmptyAttribute => new Attribute[0],
                MultipleAttributes a => a.Values.SelectMany(GetAttributes),
                _ => throw new NotImplementedException()
            };
        }

        //private static void AddBindAttribute(int sequenceNumber, RenderTreeBuilder builder) {
        //    builder.AddAttribute(16, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder()
        //}
    }
}
