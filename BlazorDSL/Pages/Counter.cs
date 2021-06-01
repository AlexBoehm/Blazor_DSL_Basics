using Microsoft.AspNetCore.Components;
using static BlazorDSL.Html;
using System.Linq;

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
                    div(
                        from name in names
                        where name.StartsWith("J")
                        select p(name)
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

        string[] names = new[]{
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
