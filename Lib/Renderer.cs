﻿using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

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
            builder.AddMultipleAttributes(
                sequenceNumber,
                from attribute in attributes
                where !(attribute is EmptyAttribute)
                select attribute switch {
                    Attribute a => new KeyValuePair<string, object>(a.Name, a.Value),
                    _ => throw new NotImplementedException("Unknown type " + attribute.GetType())
                }
            );
        }
    }
}
