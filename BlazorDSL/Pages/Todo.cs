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

                ul(
                    from item in state.todoItems
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
                            onSubmit(@this, _ => dispatch(new AddItemDelayed(state.inputText)))
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

        record AddItem(string text) : Message;
        record RemoveItem(TodoItem item) : Message;
        record SetItemStatus(TodoItem item, bool status) : Message;
        record ChangeItemText(TodoItem item, string text) : Message;
        record RemoveDoneItems() : Message;
        record SetInputText(string text) : Message;
        record AddItemDelayed(string text) : Message;
        record LoadItemFromDatabase() : Message;
        record ItemsFromDatabaseLoaded(IEnumerable<TodoItem> items) : Message;

        // Sollte nach Möglichkeit nicht public sein
        public record State(ImmutableList<TodoItem> todoItems, string inputText);

        // Sollte nach Möglichkeit nicht public sein
        public record TodoItem(
            string Text,
            bool Done
        );

        static State InitState() =>
            new State(
                todoItems: ImmutableList<TodoItem>.Empty,
                inputText: ""
            );

        static async Task LoadItemsFromDatabase(Dispatch<Message> dispatch) {
            var items = await TodoItemRepository.GetItems();
            dispatch(new ItemsFromDatabaseLoaded(items));
        }

        // Es wäre besser, wenn Update static sein könnte. Denn dann ist es wahrscheinlicher,
        // das Update auch pure ist.
        static (State state, Command<Message> command) Update(State state, Message message) =>
            message switch {
                AddItem cmd => (
                    state with { 
                        todoItems = state.todoItems.Add(
                            new TodoItem(cmd.text, false)
                        )
                    },
                    Cmd.None<Message>()
                ),

                RemoveItem cmd => (
                    state with { 
                        todoItems = state.todoItems.Remove(cmd.item)
                    },
                    Cmd.None<Message>()
                ),

                RemoveDoneItems cmd => (
                    state with {
                        todoItems = state.todoItems.RemoveAll(
                            item => item.Done == true
                        )
                    },
                    Cmd.None<Message>()
                ),

                SetItemStatus cmd => (
                    state with { 
                        todoItems = state.todoItems.Replace(
                            cmd.item,
                            new TodoItem(cmd.item.Text, cmd.status)
                        )
                    },
                    Cmd.None<Message>()
                ),

                ChangeItemText cmd => (
                    state with
                    {
                        todoItems = state.todoItems.Replace(
                            cmd.item,
                            new TodoItem(cmd.text, cmd.item.Done)
                        )
                    },

                    Cmd.None<Message>()
                ),

                SetInputText cmd => (
                    state with {
                        inputText = cmd.text
                    },
                    Cmd.None<Message>()
                ),

                AddItemDelayed cmd => (
                    state with { 
                        inputText = ""
                    },
                    async (Dispatch<Message> dispatch) => {
                        await Task.Delay(1000);
                        dispatch(new AddItem(cmd.text));
                    }
                ),

                ItemsFromDatabaseLoaded cmd => (
                    state with
                    {
                        todoItems = ImmutableList<TodoItem>.Empty.AddRange(cmd.items)
                    },
                    Cmd.None<Message>()
                )
            };
    }
}
