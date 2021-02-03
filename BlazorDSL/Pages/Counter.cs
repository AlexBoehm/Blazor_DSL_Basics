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
                .div(
                    attrs(
                        className("box")
                    ),
                    inner =>
                        inner
                        .p("Current count: " + currentCount)
                        .button(
                            attrs(
                                className("btn btn-primary"),
                                OnClick(this, IncrementCount)
                            ),
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