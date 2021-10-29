using System.Threading.Tasks;

namespace BlazorDSL {
    public static class Cmd {
        public static Command<TMessage> None<TMessage>() => (Dispatch<TMessage> _) => Task.CompletedTask;
    }
}
