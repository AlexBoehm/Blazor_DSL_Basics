using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using static BlazorDSL.Html;

namespace BlazorDSL.Shared {
    public class MainLayout : LayoutComponentBase {
        protected override void BuildRenderTree(RenderTreeBuilder builder) {
            var content = new[] {
                div (attrs(className("page")),
                    div (
                        attrs(className("sidebar")),
                        Component<NavMenu>()
                    ),
                    div (attrs(className("main")),
                        div (attrs(className("top-row px-4")),
                            a (
                                attrs(
                                    href("https://docs.microsoft.com/aspnet/"),
                                    target("_blank")
                                ),
                                text("About")
                            )
                        ),
                        div (
                            attrs(className("context px-4")),
                            fragment(Body) // Body ist Teil von LayoutComponentBase
                        )
                    )
                )
            };

            Renderer.Render(builder, new ArrayNode(content));
        }
    }
}
