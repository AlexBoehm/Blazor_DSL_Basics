using Microsoft.AspNetCore.Components;
using static BlazorDSL.Html;
using System.Linq;
using System.Collections.Generic;

namespace BlazorDSL.Pages {
    [Route("/counter")]
    public partial class Counter : WebComponent {
        int currentCount = 0;

        protected override Node Render() =>
            div(
                h1("counter"),
                Component<Greeting>(
                    parameter("Name", "Max Mustermann")
                ),
                div(
                    attrs(
                        className("box")
                    ),
                    div(
                        from name in names
                        select Tags(
                            p(name),
                            name.StartsWith("J")
                                ? button(
                                    attrs(onClick(this, () => names.Remove(name))),
                                    "delete"
                                )
                                : empty()
                        )
                    ),
                    p("Current Count: " + currentCount),
                    button(
                        attrs(
                            className("btn btn-primary"),
                            onClick(this, IncrementCount)
                        ),
                        "Click me"
                    )
                )
            );

        
        List<string> names = new List<string>{
            "George Washington",
            "John Adams",
            "Thomas Jefferson",
            "James Madison",
            "James Monroe",
            "John Quincy Adams",
            "Andrew Jackson"
        };

        private void IncrementCount() {
            currentCount++;
        }
    }
}
