using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using static BlazorDSL.Html;
using System.Linq;
using System.Collections.Immutable;

namespace BlazorDSL.Pages {

    [Route("/todo")]
    public partial class TodoPage : MVUComponent<TodoPage.State, TodoPage.Message> {
        static List<TodoItem> todoItems => new List<TodoItem>() {
            new TodoItem("Task 1", true),
            new TodoItem("Task 2", false),
            new TodoItem ("Task 3", false)
        };

        override protected Node View(State state, Dispatch dispatch, object @this) =>
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
                            bind.change.@string(@this, item.Text, nv => dispatch(new ChangeItemText(item, nv)))
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
                                bind.change.@string(@this, state.inputText, nv => dispatch(new SetInputText(nv)))
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

        // Sollte nach Möglichkeit nicht public sein
        public record State(ImmutableList<TodoItem> todoItems, string inputText);

        // Sollte nach Möglichkeit nicht public sein
        public record TodoItem(
            string Text,
            bool Done
        );

        protected override State Init() =>
            new State(
                todoItems: ImmutableList<TodoItem>.Empty.AddRange(todoItems),
                inputText: ""
            );

        protected override State Update(State state, Message message) =>
            message switch {
                AddItem cmd => state with { 
                    todoItems = state.todoItems.Add(
                        new TodoItem(cmd.text, false)
                    )
                },

                RemoveItem cmd => state with { 
                    todoItems = state.todoItems.Remove(cmd.item)
                },

                RemoveDoneItems cmd => state with {
                    todoItems = state.todoItems.RemoveAll(
                        item => item.Done == true
                    )
                },

                SetItemStatus cmd => state with { 
                    todoItems = state.todoItems.Replace(
                        cmd.item,
                        new TodoItem(cmd.item.Text, cmd.status)
                    )
                },

                ChangeItemText cmd => state with
                {
                    todoItems = state.todoItems.Replace(
                        cmd.item,
                        new TodoItem(cmd.text, cmd.item.Done)
                    )
                },

                SetInputText cmd => state with {
                    inputText = cmd.text
                }
            };
    }
}
