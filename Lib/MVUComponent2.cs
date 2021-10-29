using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BlazorDSL {
    public delegate TState InitState<TState>();
    // public delegate (TState state, Command)
    public delegate void Dispatch<TMessage>(TMessage message);
    public delegate Task Command<TMessage>(Dispatch<TMessage> dispatch);

    public delegate TState UpdateState<TState, TMessage>(TState state, TMessage message);
    public delegate (TState state, Command<TMessage> command) UpdateStateBuildCommand<TState, TMessage>(TState state, TMessage message);

    public delegate Node RenderView<TState, TMessage>(TState state, Dispatch<TMessage> dispatch, object @this);

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

        protected InitState<TState> Init { get; init; }
        protected RenderView<TState, TMessage> View { get; init; }
        protected UpdateState<TState, TMessage> Update { get; init; }        

        protected override void BuildRenderTree(RenderTreeBuilder builder) {
            var node = View(_state, _dispatch, this);
            Renderer.Render(builder, node);
        }
    }
}
