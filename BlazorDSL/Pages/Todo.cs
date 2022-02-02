using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using static BlazorDSL.Html;
using System.Linq;
using System;

namespace BlazorDSL.Pages {
    [Route("/todo")]
    public partial class TodoPage : WebComponent {
        protected override Node Render() =>
            div( attrs( className("TodoList") ),
                div(
                    input( attrs( type("text"), bind.input.@string(this, filterText, value => filterText = value)))
                ),

                ul(
                    from item in _items where item.Text.Contains(filterText, StringComparison.OrdinalIgnoreCase) select
                    li (
                        input( type("checkbox"), bind.change.@checked(this, item.Done, status => item.Done = status)),
                        input( type("text"), item.Done ? style("text-decoration: line-through") : emptyAttribute(), bind.change.@string(this, item.Text, nv => item.Text = nv)),
                        button( attrs( className("Delete"), onClick(this, _ => _items.Remove(item)) ), text("x"))
                    )
                ),

                form( attrs( onSubmit(this, _ => AddItem())),
                    input( attrs( type("text"), bind.change.@string(this, inputText, nv => inputText = nv))),
                    button(
                        span( attrs( className("oi oi-plus"), attribute("aria-hidden", "true"))),
                        text("add")
                    )
                ),

                div( attrs( className("ButtonBox")),
                    button( attrs( onClick(this, x => RemoveDoneItems()), className("RemoveDone")),
                        "remove all done"
                    )
                )
            );

        List<TodoItem> _items = new List<TodoItem>() {
            new TodoItem { Text = "Task 1", Done = true},
            new TodoItem { Text = "Task 2", Done = false },
            new TodoItem { Text = "Task 3", Done = false }
        };

        string inputText = "";
        string filterText = "";

        void AddItem() {
            _items.Add(new TodoItem {
                Done = false,
                Text = inputText
            });

            inputText = "";
        }

        void RemoveDoneItems() {
            _items.RemoveAll(x => x.Done);
        }
    }
}