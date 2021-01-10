using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorDSL.Pages {
    [Route("/counter")]
    public partial class Counter : ComponentBase {

        protected override void BuildRenderTree(RenderTreeBuilder __builder) {
            __builder.AddMarkupContent(0, "<h1>Counter</h1>\r\n\r\n");
            __builder.OpenElement(1, "div");
            __builder.AddAttribute(2, "class", "box");
            __builder.OpenElement(3, "p");
            __builder.AddContent(4, "Current count: ");
            __builder.AddContent(5, currentCount );
            __builder.CloseElement();
            __builder.AddMarkupContent(6, "\r\n    ");
            __builder.OpenElement(7, "button");
            __builder.AddAttribute(8, "class", "btn btn-primary");
            __builder.AddAttribute(9, "onclick", 
                EventCallback.Factory.Create<MouseEventArgs>(
                    this,
                    IncrementCount
                )
            );
            __builder.AddContent(10, "Click me");
            __builder.CloseElement();
            __builder.CloseElement();
        }

        private int currentCount = 0;

        private void IncrementCount() {
            currentCount++;
        }
    }
}
