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
            Func<Action, Task> invokeAsync,
            Action stateHasChanged
        ) {
            void executeCommand(Command<TMessage> command) {
                var hasToBeExecuted = !Equals(Cmd<TMessage>.None, command);

                if(hasToBeExecuted) {
                    Task.Factory.StartNew(
                        async () => {
                            Debug.WriteLine($"[{DateTime.Now}] Starting command");
                            var stopwatch = new Stopwatch();
                            stopwatch.Start();
                            await command(dispatch);
                            stopwatch.Stop();
                            Debug.WriteLine($"[{DateTime.Now}] Command executed; Took {stopwatch.Elapsed.TotalMilliseconds} ms");

                            await invokeAsync(() => {
                                try {
                                    stateHasChanged();
                                } catch (Exception e) {
                                    Debug.WriteLine(e.Message);
                                    Debug.WriteLine(e.StackTrace);
                                }
                            });
                        }
                    );
                }                
            }

            var state = initState();

            void dispatch(TMessage msg) {
                Debug.WriteLine($"[{DateTime.Now}] {msg}");

                // Berechnung neuer State und Command
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
