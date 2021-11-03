using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BlazorDSL {
    public static class MVUViewBuilder {
        public static View BuildViewMethod<TState, TMessage>(
            InitState<TState> initState,
            UpdateState<TState, TMessage> update,
            RenderView<TState, TMessage> view
        ) {
            var state = initState();

            void dispatch(TMessage msg) {
                Debug.WriteLine(msg);
                state = update(state, msg);
            };

            return sender => view(state, dispatch, sender);
        }

        public static View BuildViewMethod<TState, TMessage>(
            InitState<TState> initState,
            UpdateStateBuildCommand<TState, TMessage> update,
            RenderView<TState, TMessage> view,
            Func<Func<Task>, Task> invokeAsync,
            Action stateHasChanged
        ) {
            var state = initState();

            void dispatch(TMessage msg) {
                Debug.WriteLine(msg);
                (var newState, var command) = update(state, msg);
                state = newState;

                // Ausführen des Commands
                Task.Factory.StartNew(() => {
                    invokeAsync(async () => {
                        await command(dispatch);
                        stateHasChanged();
                    });
                }
                );
            };

            return sender => view(state, dispatch, sender);
        }
    }
}
