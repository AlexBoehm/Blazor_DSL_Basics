using Microsoft.AspNetCore.Components;

using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

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

    public static class ViewBuilder {
        public static View BuildViewMethod<TState, TMessage>(
            InitState<TState> initState,
            UpdateState<TState, TMessage> update,
            RenderView<TState, TMessage> view
        ) {
            var state = initState();

            Dispatch<TMessage> dispatch = (TMessage msg) => {
                Debug.WriteLine(msg);
                state = update(state, msg);
            };

            return sender => view(state, dispatch, sender);
        }

        public static View BuildViewMethod<TState, TMessage>(
            InitState<TState> initState,
            UpdateStateBuildCommand<TState, TMessage> update,
            RenderView<TState, TMessage> view
        ) {
            var state = initState();

            Dispatch<TMessage> dispatch = null;
            
            dispatch = (TMessage msg) => {
                Debug.WriteLine(msg);
                (var newState, var command) = update(state, msg);
                state = newState;

                //Task.Run()
                //await command(dispatch);
                // Task.Run(command(dispatch))

                Task.Factory.StartNew(
                    () => {
                        command(dispatch);
                    }
                );
                // command.
            };

            return sender => view(state, dispatch, sender);
        }
    }
}
