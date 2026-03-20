using Livora_Lite_Blazor.Services;
using Microsoft.AspNetCore.Components;

namespace Livora_Lite_Blazor.Components
{
    /// <summary>
    /// Componente base para proteger páginas que requerem autenticação
    /// </summary>
    public class ProtectedComponent : Microsoft.AspNetCore.Components.ComponentBase
    {
        [Microsoft.AspNetCore.Components.Inject]
        protected IAuthService AuthService { get; set; } = default!;

        [Microsoft.AspNetCore.Components.Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        protected bool IsAuthenticated { get; private set; }
        protected bool IsLoading { get; private set; } = true;

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            IsAuthenticated = AuthService.IsAuthenticated();

            if (!IsAuthenticated)
            {
                NavigationManager.NavigateTo("/login", forceLoad: true);
            }

            IsLoading = false;
            await base.OnInitializedAsync();
        }
    }
}
