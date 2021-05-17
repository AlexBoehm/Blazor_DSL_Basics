using Microsoft.AspNetCore.Components;
using static BlazorDSL.Html;

namespace BlazorDSL.Pages {
    [Route("/counter")]
    public partial class Counter : WebComponent {
        int currentCount = 0;

        protected override Node Render() =>
            div(
                h1("counter"),
                div(
                    attrs(
                        className("box")
                    ),
                    p("Current Count: " + currentCount),
                    button(
                        attrs(
                            className("btn btn-primary")
                        ),
                        "Click me"
                    )
                )
            );
    }
}
