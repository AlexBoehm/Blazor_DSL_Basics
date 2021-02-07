using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using static BlazorDSL.HTML;
using System.Collections.Generic;

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
                        .div(
                            attrs(), 
                            inner => inner.ForEach(
                                names,
                                (inner, name) => {
                                    inner.p(name);

                                    if (name.StartsWith("J")) {
                                        inner.button(
                                            attrs(OnClick(this, () => names.Remove(name))),
                                            "delete"
                                        );
                                    }
                                }
                            )
                        )
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

        List<string> names = new List<string>{
            "George Washington",
            "John Adams",
            "Thomas Jefferson",
            "James Madison",
            "James Monroe",
            "John Quincy Adams",
            "Andrew Jackson"
        };

        private int currentCount = 0;

        private void IncrementCount() {
            currentCount++;
        }
    }
}