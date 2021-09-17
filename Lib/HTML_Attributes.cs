using System.Collections.Generic;
using System.Linq;

namespace BlazorDSL {
    public static partial class Html {
        public static Attribute className(string className)
            => new Attribute("class", className);

        public static Attribute href(string value)
            => new Attribute("href", value);

        public static Attribute target(string value)
            => new Attribute("target", value);
    }
}
