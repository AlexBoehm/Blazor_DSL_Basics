using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DSLMethodsGenerator
{
    class Program
    {
        static void Main(string[] args) {
            ExportBlazorEvents();
        }
        
        private static void ExportBlazorEvents() {
            IEnumerable<EventHandlerAttribute> attributes = ReadEventHandlerAttributes();

            File.WriteAllLines(
                "Blazor Events.txt",
                from item in attributes
                select $"{item.AttributeName}\t{item.AttributeName}\t{item.EventArgsType}"
            );

            foreach (var item in attributes) {
                Console.WriteLine($"{item.AttributeName} {item.EventArgsType}");
            }
        }

        private static IEnumerable<EventHandlerAttribute> ReadEventHandlerAttributes() {
            return typeof(EventHandlers).GetCustomAttributes(
                typeof(EventHandlerAttribute), true
            )
            .Cast<EventHandlerAttribute>();
        }
    }
}
