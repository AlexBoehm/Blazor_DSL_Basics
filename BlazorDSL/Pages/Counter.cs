using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Rendering;
using static BlazorDSL.HTML;
using System.Linq;
using System;

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
                                    var styleValue = name.StartsWith("j", StringComparison.OrdinalIgnoreCase)
                                        ? "background: green"
                                        : "";

                                    inner.p(
                                        attrs(
                                            style(styleValue)
                                        ),
                                        name
                                    );
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

        string[] names = new[]{
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