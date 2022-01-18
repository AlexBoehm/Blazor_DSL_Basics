using Microsoft.AspNetCore.Components;
using System;

namespace BlazorDSL
{
    public static partial class Html
    {
        public static class bind
        {
            public static class change
            {
                public static Attribute @checked(object receiver, bool value, Action<bool> onChange)
                {
                    return new MultipleAttributes(
                        new Attribute[] {
                            new KeyValueAttribute("checked", Microsoft.AspNetCore.Components.BindConverter.FormatValue(value)),
                            new KeyValueAttribute("onchange", EventCallback.Factory.CreateBinder(receiver, onChange, value))
                        }
                    );
                }

                public static Attribute @string(object receiver, string value, Action<string> onChange)
                {
                    return new MultipleAttributes(
                        new Attribute[] {
                            new KeyValueAttribute("value", Microsoft.AspNetCore.Components.BindConverter.FormatValue(value)),
                            new KeyValueAttribute("onchange", EventCallback.Factory.CreateBinder(receiver, onChange, value))
                        }
                    );
                }
            }

            public static class input
            {
                public static Attribute @string(object receiver, string value, Action<string> onChange)
                {
                    return new MultipleAttributes(
                        new Attribute[] {
                            new KeyValueAttribute("value", Microsoft.AspNetCore.Components.BindConverter.FormatValue(value)),
                            new KeyValueAttribute("oninput", EventCallback.Factory.CreateBinder(receiver, onChange, value))
                        }
                    );
                }
            }
        }
    }
}
