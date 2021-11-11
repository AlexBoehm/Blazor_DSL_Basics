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

        public static (View view, Action initialize) BuildViewMethod<TState, TMessage>(
            InitState<TState> initState,
            Command<TMessage> initCommand,
            UpdateStateBuildCommand<TState, TMessage> update,
            RenderView<TState, TMessage> view,
            Func<Func<Task>, Task> invokeAsync,
            Action stateHasChanged
        ) {
            void executeCommand(Command<TMessage> command) {
                Task.Factory.StartNew(
                    () => {
                        invokeAsync(async () => {
                            await command(dispatch);
                            stateHasChanged();
                        });
                    }
                );
            }

            var state = initState();

            void dispatch(TMessage msg) {
                Debug.WriteLine(msg);
                (var newState, var command) = update(state, msg);
                state = newState;

                // Ausführen des Commands
                executeCommand(command);                
            };

            return (
                view: sender => view(state, dispatch, sender),
                initialize: () => executeCommand(initCommand)
            );
        }
    }
}
