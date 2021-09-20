using System.Linq;

namespace DSLMethodsGenerator {
    partial class Program {
        static void Main(string[] args) {
            // ExportBlazorEvents();
            GenerateAttributeMethods();
            GenerateTagMethods();
            GenerateEventMethods();
        }

        private static string[] FilterLines(string[] lines)
            => lines
                .Where(line => !line.Trim().StartsWith("#"))
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .ToArray();
    }
}
