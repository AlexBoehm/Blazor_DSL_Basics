using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.IO;
using System.Linq;

namespace DSLMethodsGenerator
{
    class Program
    {
        static void Main(string[] args) {
            // Microsoft.AspNetCore.Components.Web
            // EventHandlers
            GenerateEventMethods();


            // ExportBlazorEvents();
        }

        private static void GenerateEventMethods() {
            var eventMehodNames = (
                from line in File.ReadLines("Blazor Events.txt")
                where !line.Trim().StartsWith("#")
                let parts = line.Trim().Split("\t")
                select (method: parts[0], attribute: parts[1])
            ).ToDictionary(x => x.attribute);

            var eventHandlerAttributes = ReadEventHandlerAttributes();

            var methods =
                from attribute in eventHandlerAttributes
                where eventMehodNames.ContainsKey(attribute.AttributeName)
                let methodNameConfiguration = eventMehodNames[attribute.AttributeName]
                select (method: methodNameConfiguration.method, attribute: attribute.AttributeName, eventType: attribute.EventArgsType)
                ;

            var source = @"
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorDSL {
    static partial class Html {" +
    string.Join("\n", methods.Select(item => BuildMethod(item.method, item.attribute, item.eventType))) +
    @"}
}
";

            File.WriteAllText("HTML_Events.cs", source);
        }

        // private static 

        private static string BuildMethod(string methodName, string attibuteName, Type eventType) =>
            $@"
            public static Attribute {methodName}(object sender, Action<{eventType.Name}> callback)
            => new Attribute(
                ""{attibuteName}"",
                EventCallback.Factory.Create(sender, callback)
            );";
        
        private static void ExportBlazorEvents() {
            System.Collections.Generic.IEnumerable<EventHandlerAttribute> attributes = ReadEventHandlerAttributes();

            File.WriteAllLines(
                "Blazor Events.txt",
                from item in attributes
                select $"{item.AttributeName}\t{item.AttributeName}\t{item.EventArgsType}"
            );

            foreach (var item in attributes) {
                Console.WriteLine($"{item.AttributeName} {item.EventArgsType}");
            }
        }

        private static System.Collections.Generic.IEnumerable<EventHandlerAttribute> ReadEventHandlerAttributes() {
            return typeof(EventHandlers).GetCustomAttributes(
                typeof(EventHandlerAttribute), true
            )
            .Cast<EventHandlerAttribute>();
        }
    }
}
