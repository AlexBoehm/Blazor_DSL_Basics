using Microsoft.AspNetCore.Components;
using static BlazorDSL.Html;

namespace BlazorDSL.Pages {
    [Route("/counter")]
    public partial class Counter : WebComponent {

        protected override Node Render() =>
            div(
                h1("counter")
            );
    }
}
