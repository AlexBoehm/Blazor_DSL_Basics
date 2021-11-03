using Microsoft.AspNetCore.Components;

using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BlazorDSL {
    public delegate Node View(object sender);

    public abstract class MVUComponent3 : ComponentBase {        
        View _view;

        //public MVUComponent3(View view) {
        //    _view = view;
        //}

        protected void SetView(View view) =>
            _view = view;

        protected override void BuildRenderTree(RenderTreeBuilder builder) {
            var node = _view(this);
            Renderer.Render(builder, node);
        }
    }

    
}
