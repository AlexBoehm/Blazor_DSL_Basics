using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using static BlazorDSL.Html;
using System.Linq;

namespace BlazorDSL.Pages {
    [Route("/todo")]
    public partial class TodoPage : WebComponent {
        List<TodoItem> todoItems = new List<TodoItem>() {
            new TodoItem { Text = "Task 1", Done = true},
            new TodoItem { Text = "Task 2", Done = false },
            new TodoItem { Text = "Task 3", Done = false }
        };

        string inputText = "";

        protected override Node Render() =>
            div(
                attrs(
                    className("TodoList")
                ),

                ul(
                    from item in todoItems
                    select li(
                        input(
                            type("checkbox"),
                            bind.@checked(
                                this,
                                item.Done,
                                status => SetItemStatus(item, status)
                            )
                        ),
                        input(
                            type("text"),
                            item.Done ? style("text-decoration: line-through") : emptyAttribute(),
                            bind.change.@string(this, item.Text, nv => item.Text = nv)
                        ),
                        button(
                            attrs(
                                className("Delete"),
                                onClick(this, _ => todoItems.Remove(item))
                            ),
                            text("x")
                        )
                    )
                ),

                div(
                    form(
                        attrs(
                            onSubmit(this, _ => AddToDoItem())
                        ),
                        input(
                            attrs(
                                type("text"),
                                bind.change.@string(this, inputText, nv => inputText = nv)
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
                            onClick(this, x => RemoveDoneItems()),
                            className("RemoveDone")
                        ),
                        "remove all done"
                    )
                )
            );

        void AddToDoItem() {
            todoItems.Add(new TodoItem {
                Done = false,
                Text = inputText
            });

            inputText = "";
        }

        void SetItemStatus(TodoItem item, bool status)
            => item.Done = status;

        void RemoveDoneItems() {
            todoItems.RemoveAll(x => x.Done);
        }

        class TodoItem {
            public string Text { get; set; }
            public bool Done { get; set; }
        }
    }
}
