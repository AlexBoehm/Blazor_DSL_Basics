using Microsoft.AspNetCore.Components;

using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorDSL {
    public abstract class MVUComponent4 : ComponentBase {        
        View _view;

        protected void SetUp<TState, TMessage>(
            InitState<TState> initState,
            UpdateStateBuildCommand<TState, TMessage> update,
            RenderView<TState, TMessage> view
            ) {
            _view = MVUViewBuilder.BuildViewMethod(
                initState,
                update,
                view,
                this.InvokeAsync,
                this.StateHasChanged
            );
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder) {
            var node = _view(this);
            Renderer.Render(builder, node);
        }
    }    
}
