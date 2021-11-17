using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BlazorDSL.Pages.TodoPage;

namespace BlazorDSL {
    public class TodoItemRepository {
        public static async Task<int> InsertItem(TodoItem item) {
            await MySql.ExecuteAsync(
                "INSERT INTO todoitems (Text, Done) VALUES (@Text, @Done)",
                values => {
                    values.Add("@Text", item.Text);
                    values.Add("@Done", item.Done);
                }
            ).ConfigureAwait(false);

            var result = 
                await MySql.QueryScalarAsync(
                    "SELECT LAST_INSERT_ID();", 
                    value => int.Parse(value.ToString())
                )
                .ConfigureAwait(false);

            return result;
        }

        public static async Task<List<TodoItem>> GetItems() {
            var items = await MySql.QueryAsync(
                "SELECT * FROM todoitems",
                reader => new TodoItem(
                    (int) reader["Id"],
                    (string) reader["Text"],
                    (sbyte) reader["Done"] == 1
                )
            );

            return items.ToList();
        }
    }
}
