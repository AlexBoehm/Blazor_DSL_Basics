using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorDSL {
    public abstract class WebComponent : ComponentBase {
        protected override void BuildRenderTree(RenderTreeBuilder builder) {
            var node = Render();
            Renderer.Render(builder, node);
        }

        protected abstract Node Render();
        

    }
}
