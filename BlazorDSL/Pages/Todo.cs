using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using static BlazorDSL.Html;
using System.Linq;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace BlazorDSL.Pages {

    [Route("/todo")]
    public partial class TodoPage : MVUComponent3{
        public TodoPage() : base(
            ViewBuilder.BuildViewMethod<State, Message>(
                Init,
                Update,
                View
            )
        ) {
        }

        static Node View(State state, Dispatch<Message> dispatch, object @this) =>
            div(
                attrs(
                    className("TodoList"),
                    onSubmit(@this, _ => dispatch(new AddItemDelayed(state.inputText)))
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

                form(
                    attrs(
                        className("form")
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
                                attribute("aria-hidden", "true"),
                                type("button")
                                //onClick(@this, e => {
                                //    dispatch(new AddItem(state.inputText));
                                //})
                            )
                        ),
                        text("add") // <span class="oi oi-plus" aria-hidden="true" b-4vrz9tvk6a=""></span>
                    ),
                    button(
                        span(
                            attrs(
                                className("oi oi-plus"),
                                attribute("aria-hidden", "true"),
                                type("button")
                                //onClick(@this, _ => {
                                //    dispatch(new AddItemDelayed(state.inputText));
                                //})
                            )
                        ),
                        text("add delayed") // <span class="oi oi-plus" aria-hidden="true" b-4vrz9tvk6a=""></span>
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

        // Sollte nach Möglichkeit nicht public sein
        public record State(ImmutableList<TodoItem> todoItems, string inputText);

        // Sollte nach Möglichkeit nicht public sein
        public record TodoItem(
            string Text,
            bool Done
        );

        static List<TodoItem> todoItems => new List<TodoItem>() {
            new TodoItem("Task 1", true),
            new TodoItem("Task 2", false),
            new TodoItem ("Task 3", false)
        };

        static State Init() =>
            new State(
                todoItems: ImmutableList<TodoItem>.Empty.AddRange(todoItems),
                inputText: ""
            );

        // Es wäre besser, wenn Update static sein könnte. Denn dann ist es wahrscheinlicher,
        // das Update auch pure ist.
        static (State, Command<Message>) Update(State state, Message message) =>
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
                    state, 
                    async (Dispatch<Message> dispatch) => {
                        await Task.Delay(1000);
                        dispatch(new AddItem(cmd.text));
                    }
                )
            };
    }
}
