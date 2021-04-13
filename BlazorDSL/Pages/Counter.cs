using Microsoft.AspNetCore.Components;

namespace BlazorDSL.Pages {
    [Route("/counter")]
    public partial class Counter : WebComponent {

        protected override void Render() =>            
            div(
                h1("Counter"),
                div(
                    attrs(
                        className("box")
                    ),
                    p("Current count: " + currentCount),
                    button(
                        attrs(
                            className("btn btn-primary"),
                            onClick(IncrementCount),
                        ),
                        "Click me"
                    )
                )
            );

        private int currentCount = 0;

        private void IncrementCount() {
            currentCount++;
        }
    }
}
