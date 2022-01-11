using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using static BlazorDSL.Html;
using System.Linq;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace BlazorDSL.Pages {

    [Route("/todo")]
    public partial class TodoPage : MVUComponent4 {
        public TodoPage() {
            SetUp<State, Message>(
                InitState,
                LoadItemsFromDatabase,
                Update,
                View
            );
        }

        static Node View(State state, Dispatch<Message> dispatch, object @this) =>
            div(
                attrs(
                    className("TodoList")
                ),

                div(
                    input(
                        type("text"),
                        bind.input.@string(
                            @this,
                            state.filterText,
                            value => dispatch(new SetFilter(value))
                        )
                    )
                ),

                ul(
                    from item in state.todoItems
                    where state.filterText == "" || item.Text.Contains(state.filterText, System.StringComparison.OrdinalIgnoreCase)
                    select li(
                        input(
                            type("checkbox"),
                            bind.@checked(
                                @this,
                                item.Done,
                                status => dispatch(new SetItemStatus(item, status))
                            )
                        ),
                        input(
                            type("text"),
                            item.Done ? style("text-decoration: line-through") : emptyAttribute(),
                            bind.input.@string(@this, item.Text, nv => dispatch(new ChangeItemText(item, nv)))
                        ),
                        button(
                            attrs(
                                className("Delete"),
                                onClick(@this, _ => dispatch(new RemoveItem(item)))
                            ),
                            text("x")
                        )
                    )
                ),

                div(
                    form(
                        attrs(
                            onSubmit(@this, _ => dispatch(new AddItem(state.inputText)))
                        ),
                        input(
                            attrs(
                                type("text"),
                                bind.input.@string(@this, state.inputText, nv => dispatch(new SetInputText(nv)))
                            )
                        ),
                        button(
                            span(
                                attrs(
                                    className("oi oi-plus"),
                                    attribute("aria-hidden", "true")

                                )
                            ),
                            text("add") // <span class="oi oi-plus" aria-hidden="true" b-4vrz9tvk6a=""></span>
                        )
                    )
                ),

                div(
                    attrs(
                        className("ButtonBox")
                    ),
                    button(
                        attrs(
                            onClick(@this, x => dispatch(new RemoveDoneItems())),
                            className("RemoveDone")
                        ),
                        "remove all done"
                    )
                )
            );

        public record Message();

        // record AddItem(AsyncOperationStatus<string, TodoItem> state);
        record AddItem(string text) : Message;
        record ItemAdded(TodoItem item) : Message;
        record RemoveItem(TodoItem item) : Message;
        record SetItemStatus(TodoItem item, bool status) : Message;
        record ChangeItemText(TodoItem item, string text) : Message;
        record RemoveDoneItems() : Message;
        record DoneItemsRemoved(IEnumerable<int> ids) : Message;
        record SetInputText(string text) : Message;
        record ItemsFromDatabaseLoaded(IEnumerable<TodoItem> items) : Message;
        record SetFilter(string filter) : Message;

        //static class Result {
        //    Result
        //}

        //class Result<TValue, TError> {
        //    public bool Success { get; private set; }
        //    public TValue Value { get; private set; }
        //    public TError Error { get; private set; }



        //}

        record AsyncOperationStatus<TInput, TOuput>(bool finished, TInput input, TOuput output);

        //class AsyncOperationStatus<TValue> {
        //    public bool Finished { get; set; }

        //}

        // Sollte nach Möglichkeit nicht public sein
        public record State(
            ImmutableList<TodoItem> todoItems, 
            string inputText,
            string filterText
        );

        // Sollte nach Möglichkeit nicht public sein
        public record TodoItem(
            int Id,
            string Text,
            bool Done
        );

        static State InitState() =>
            new State(
                todoItems: ImmutableList<TodoItem>.Empty,
                inputText: "",
                filterText: ""
            );

        static async Task LoadItemsFromDatabase(Dispatch<Message> dispatch) {
            
            var items = await TodoItemRepository.GetItems();
            dispatch(new ItemsFromDatabaseLoaded(items));
        }

        private static Command<Message> AddNewItem(string text) {
            return async (Dispatch<Message> dispatch) => {
                var item = new TodoItem(-1, text, false);
                var id = await TodoItemRepository.InsertItem(item);
                item = item with { Id = id };
                dispatch(new ItemAdded(item));
            };
        }

        static (State state, Command<Message> command) Update(State state, Message message) =>
            message switch {
                AddItem cmd => (
                    state with
                    {
                        inputText = ""
                    },
                    AddNewItem(cmd.text)
                ),

                ItemAdded cmd => (
                    state with
                    {
                        todoItems = state.todoItems.Add(cmd.item)
                    },
                    Cmd<Message>.None
                ),

                RemoveItem cmd => (
                    state with
                    {
                        todoItems = state.todoItems.Remove(cmd.item)
                    },
                    async (Dispatch<Message> Dispatch) => {
                        await TodoItemRepository.RemoveItem(cmd.item.Id).ConfigureAwait(false);
                    }
                ),

                RemoveDoneItems cmd => (
                    state,
                    async (Dispatch<Message> dispatch) => {
                        var doneItems = state.todoItems.Where(x => x.Done);
                        var ids = doneItems.Select(x => x.Id);
                        await TodoItemRepository.RemoveItems(ids).ConfigureAwait(false);
                        dispatch(new DoneItemsRemoved(ids));
                    }
                ),

                DoneItemsRemoved cmd => (
                    state with { todoItems = state.todoItems.RemoveAll(item => cmd.ids.Contains(item.Id))},
                    Cmd<Message>.None
                ),

                SetItemStatus cmd => (
                    state with
                    {
                        todoItems = state.todoItems.Replace(
                            cmd.item,
                            cmd.item with { Done = cmd.status }
                        )
                    },
                    async (Dispatch<Message> Dispatch) => {
                        await TodoItemRepository.UpdateItem(cmd.item with { Done = cmd.status }).ConfigureAwait(false);
                    }
                ),

                ChangeItemText cmd => (
                    state with
                    {
                        todoItems = state.todoItems.Replace(
                            cmd.item,
                            cmd.item with { Text = cmd.text }
                        )
                    },
                    async (Dispatch<Message> Dispatch) => {
                        await TodoItemRepository.UpdateItem(cmd.item with { Text = cmd.text }).ConfigureAwait(false);
                    }
                ),

                SetInputText cmd => (
                    state with
                    {
                        inputText = cmd.text
                    },
                    Cmd<Message>.None
                ),

                ItemsFromDatabaseLoaded cmd => (
                    state with
                    {
                        todoItems = ImmutableList<TodoItem>.Empty.AddRange(cmd.items)
                    },
                    Cmd<Message>.None
                ),

                SetFilter cmd => (
                    state with {  filterText = cmd.filter }, Cmd<Message>.None
                )
            };
    }
}
