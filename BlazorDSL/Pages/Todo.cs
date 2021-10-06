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
                ul(
                    from item in todoItems
                    select li(
                        input(
                            type("checkbox"),
                            bind.@checked(
                                this,
                                item.Done,
                                nv => item.Done = nv
                            )
                        ),
                        div(item.Text),
                        div(item.Done ? "(Done)" : "")
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
                        button("add")
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

        class TodoItem {
            public string Text { get; set; }
            public bool Done { get; set; }
        }
    }
}
