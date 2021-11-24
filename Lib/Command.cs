using System.Threading.Tasks;

namespace BlazorDSL {
    public static class Cmd<TMessage> {
        public static Command<TMessage> None { get; } = (Dispatch<TMessage> _) => Task.CompletedTask;
    }
}
