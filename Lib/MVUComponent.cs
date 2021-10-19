using BlazorDSL;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Diagnostics;

namespace BlazorDSL {
    public abstract class MVUComponent<TState, TMessage> : ComponentBase {
        public MVUComponent() {
            _state = Init();
            _dispatch = (TMessage msg) => {
                Debug.WriteLine(msg);
                _state = Update(_state, msg);
            };
        }

        protected delegate void Dispatch(TMessage message);

        protected abstract TState Init();

        protected abstract Node View(TState state, Dispatch dispatch, object @this);
        protected abstract TState Update(TState state, TMessage message);

        TState _state;
        Dispatch _dispatch;

        protected override void BuildRenderTree(RenderTreeBuilder builder) {
            var node = View(_state, _dispatch, this);
            Renderer.Render(builder, node);
        }
    }
}
