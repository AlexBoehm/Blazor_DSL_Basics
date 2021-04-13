using Microsoft.AspNetCore.Components;

namespace BlazorDSL {
    public abstract class WebComponent : ComponentBase {
        protected abstract Node Render();
    }
}
