using System;
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

        public static async Task UpdateItem(TodoItem item) {
            if (item.Id == 0)
                throw new ArgumentException($"The Id of an TodoItem must be != 0 when updating an item");

            await MySql.ExecuteAsync(
                "UPDATE todoitems SET Text = @Text, Done = @Done WHERE Id = @Id",
                values => {
                    values.Add("@Id", item.Id);
                    values.Add("@Text", item.Text);
                    values.Add("@Done", item.Done);
                }
            ).ConfigureAwait(false);
        }

        public static async Task RemoveItem(int id) {
            if (id == 0)
                throw new ArgumentException($"The Id of an TodoItem must be != 0 when removing an item");

            await MySql.ExecuteAsync(
                "DELETE FROM todoitems WHERE Id = @Id",
                values => {
                    values.Add("@Id", id);                    
                }
            ).ConfigureAwait(false);
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
