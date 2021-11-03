using Microsoft.AspNetCore.Components;

using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorDSL {
    public delegate Node View(object sender);

    public abstract class MVUComponent3 : ComponentBase {        
        View _view;

        protected void SetView(View view) =>
            _view = view;

        protected override void BuildRenderTree(RenderTreeBuilder builder) {
            var node = _view(this);
            Renderer.Render(builder, node);
        }
    }    
}
