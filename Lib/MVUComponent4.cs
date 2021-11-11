using Microsoft.AspNetCore.Components;

using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Threading.Tasks;

namespace BlazorDSL {
    public abstract class MVUComponent4 : ComponentBase {        
        View _view;
        Action _initialize;

        protected void SetUp<TState, TMessage>(
            InitState<TState> initState,
            Command<TMessage> initCommand,
            UpdateStateBuildCommand<TState, TMessage> update,
            RenderView<TState, TMessage> view
            ) {
            (_view, _initialize) = MVUViewBuilder.BuildViewMethod(
                initState,
                initCommand,
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

        protected override void OnInitialized() {
            _initialize();
        }
    }    
}
