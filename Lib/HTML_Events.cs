using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;

namespace BlazorDSL {
    public static partial class HTML {
        public static Attribute onClick(object sender, Action<MouseEventArgs> callback)
            => new Attribute(
                "onclick",
                EventCallback.Factory.Create(sender, callback)
            );
    }
}
