using Microsoft.AspNetCore.Components.Rendering;
using System;

namespace BlazorDSL {
    static class Renderer {
        public static void Render(RenderTreeBuilder builder, Node node) {
            Render(builder, node, 0);
        }

        private static int Render(RenderTreeBuilder builder, Node node, int sequenceNumber) {
            return node switch {
                TagNode n => RenderTagNode(builder, sequenceNumber, n),
                TextNode n => RenderTextNode(builder, sequenceNumber, n),
                _ => throw new Exception("Unexpected node of Type " + node.GetType().FullName)
            };
        }

        private static int RenderTagNode(RenderTreeBuilder builder, int sequenceNumber, TagNode n) {
            builder.OpenRegion(sequenceNumber);
            builder.OpenElement(0, n.Tag);

            var nextSequenceInRegion = 1;

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
    }
}
