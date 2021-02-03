using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Rendering;
using static BlazorDSL.HTML;

namespace BlazorDSL.Pages {
    [Route("/counter")]
    public partial class Counter : ComponentBase {
       protected override void BuildRenderTree(RenderTreeBuilder builder) {
            builder
                .h1("Counter")
                .div(attrs(
                    Attribute("class", "box")),
                    inner =>
                        inner
                        .p("Current count: " + currentCount)
                        .button(attrs(
                            Attribute("class", "btn btn-primary"),
                            Attribute(
                                "onclick",
                                EventCallback.Factory.Create<MouseEventArgs>(
                                    this,
                                    IncrementCount
                                )
                            )),
                            "Click me"
                        )
                    );
        }

        private int currentCount = 0;

        private void IncrementCount() {
            currentCount++;
        }
    }
}