using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorDSL {
    class MySql {
        const string ConnectionString = "server=localhost;uid=admin;pwd=alex";

        public static IEnumerable<T> Query<T>(string query, Func<MySqlDataReader, T> action) {
            using (var con = new MySqlConnection(ConnectionString)) {
                con.Open();

                var cmd = new MySqlCommand(query);
                cmd.Connection = con;

                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        yield return action.Invoke(reader);
                    }
                }
            }
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(string query, Func<MySqlDataReader, T> action) {
            using (var con = new MySqlConnection(ConnectionString)) {
                con.Open();

                var cmd = new MySqlCommand(query);
                cmd.Connection = con;

                using (var reader = await cmd.ExecuteReaderAsync()) {
                    return ReadFromReader(reader, action);
                }
            }
        }

        static IEnumerable<T> ReadFromReader<T>(MySqlDataReader reader, Func<MySqlDataReader, T> action) {
            while (reader.Read()) {
                yield return action.Invoke(reader);
            }
        }

        public class ValuesBuilder {
            MySqlCommand _cmd;

            public ValuesBuilder(MySqlCommand cmd) {
                _cmd = cmd;
            }

            public void Add(string name, object value) {
                _cmd.Parameters.AddWithValue(name, value);
            }
        }

        public static int Execute(string query, Action<ValuesBuilder> values) {
            using var con = new MySqlConnection(ConnectionString);
            con.Open();

            using var cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = query;

            var valuesBuilder = new ValuesBuilder(cmd);
            values(valuesBuilder);

            var changedRows = cmd.ExecuteNonQuery();

            con.Close();
            return changedRows;
        }

        public static async Task<int> ExecuteAsync(string query, Action<ValuesBuilder> values) {
            using var con = new MySqlConnection(ConnectionString);
            con.Open();

            using var cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = query;

            var valuesBuilder = new ValuesBuilder(cmd);
            values(valuesBuilder);

            var changedRows = await cmd.ExecuteNonQueryAsync();

            con.Close();
            return changedRows;
        }
    }
}
