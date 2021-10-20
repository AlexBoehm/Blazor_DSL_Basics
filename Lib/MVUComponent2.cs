using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Diagnostics;

namespace BlazorDSL {
    public delegate TState Init<TState>();
    public delegate void Dispatch<TMessage>(TMessage message);
    public delegate TState Update<TState, TMessage>(TState state, TMessage message);
    public delegate Node View<TState, TMessage>(TState state, Dispatch<TMessage> dispatch, object @this);

    public abstract class MVUComponent2<TState, TMessage> : ComponentBase {
        TState _state;
        Dispatch<TMessage> _dispatch;

        protected override void OnInitialized() {
            _state = Init();
            _dispatch = (TMessage msg) => {
                Debug.WriteLine(msg);
                _state = Update(_state, msg);
            };
        }

        protected Init<TState> Init { get; init; }
        protected View<TState, TMessage> View { get; init; }
        protected Update<TState, TMessage> Update { get; init; }        

        protected override void BuildRenderTree(RenderTreeBuilder builder) {
            var node = View(_state, _dispatch, this);
            Renderer.Render(builder, node);
        }
    }
}
