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

        protected override Node Render() =>
            ul(
                from item in todoItems
                select li(
                    input(
                        type("checkbox"),
                        item.Done ? @checked("checked") : emptyAttribute(),
                        bind.@checked(
                            this,
                            item.Done,
                            nv => item.Done = nv
                        )
                    ),
                    div(item.Text),
                    div(item.Done ? "(Done)" : "")
                )
            );

        class TodoItem {
            public string Text { get; set; }
            public bool Done { get; set; }
        }
    }
}
