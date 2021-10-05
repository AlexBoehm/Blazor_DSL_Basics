using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using static BlazorDSL.Html;
using System.Linq;

namespace BlazorDSL.Pages {
    [Route("/todo")]
    public partial class TodoPage : WebComponent {
        List<TodoItem> todoItems = new List<TodoItem>() {
            new TodoItem { Text = "Task 1", Done = new ValueWithSetter<bool>(false) },
            new TodoItem { Text = "Task 2", Done = new ValueWithSetter<bool>(false) },
            new TodoItem { Text = "Task 3", Done = new ValueWithSetter<bool>(false) }
        };

        protected override Node Render() =>
            ul(
                from item in todoItems
                select li(
                    input(
                        type("checkbox"),
                        item.Done.Value ? @checked("checked") : emptyAttribute(),
                        // onChange(this, e => item.Done.Set(e.Value.ToString() == "checked")),
                        bindValue(item.Done)
                    ),
                    div(item.Text)
                )
            );

        class TodoItem {
            public string Text { get; set; }
            public ValueWithSetter<bool> Done { get; set; }
        }
    }       
}
