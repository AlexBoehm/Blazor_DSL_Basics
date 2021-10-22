using Microsoft.AspNetCore.Components;

using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Diagnostics;

namespace BlazorDSL {
    public delegate Node View(object sender);

    public abstract class MVUComponent3 : ComponentBase {        
        View _view;

        public MVUComponent3(View view) {
            _view = view;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder) {
            var node = _view(this);
            Renderer.Render(builder, node);
        }
    }

    class MvuTipple<TState, TMessage> {
        TState _state;
        Dispatch<TMessage> _dispatch;
        Update<TState, TMessage> _update;
        View<TState, TMessage> _view;

        public MvuTipple(Init<TState> init, Update<TState, TMessage> update, View<TState, TMessage> view) {
            _state = init();
            _update = update;
            _view = view;

            _dispatch = (TMessage msg) => {
                Debug.WriteLine(msg);
                _state = _update(_state, msg);
            };
        }

        public Node View(object @this) => _view(_state, _dispatch, @this);
    }

    public static class ViewBuilder {
        public static View BuildViewMethod<TState, TMessage>(
            Init<TState> initState,
            Update<TState, TMessage> update,
            View<TState, TMessage> view
        ) {
            var state = initState();

            Dispatch<TMessage> dispatch = (TMessage msg) => {
                Debug.WriteLine(msg);
                state = update(state, msg);
            };

            return sender => view(state, dispatch, sender);
        }
    }
}
