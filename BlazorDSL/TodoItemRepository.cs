using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BlazorDSL.Pages.TodoPage;

namespace BlazorDSL {
    public class TodoItemRepository {
        public static Task<int> InsertItem(TodoItem item) {
            return MySql.ExecuteAsync(
                "INSERT INTO todoitems (Text, Done) VALUES (@Text, @Done)",
                values => {
                    values.Add("@Text", item.Text);
                    values.Add("@Done", item.Done);
                }
            );
        }

        public static async Task<List<TodoItem>> GetItems() {
            var items = await MySql.QueryAsync(
                "SELECT * FROM todoitems",
                reader => new TodoItem(
                   (string) reader["Text"],
                   (sbyte) reader["Done"] == 1
                )
            );

            return items.ToList();
        }
    }
}
