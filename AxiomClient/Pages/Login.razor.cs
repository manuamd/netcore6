using Axiom.Application.Interfaces.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AxiomClient.Pages
{
    public partial class Login
    {
        [Inject] private IBusinessLogic BusinessLogic { get; set; }
        [Parameter] public LoginModel loginModel { get; set; } = new();

        private async Task LoginAsync() 
        {
            var result = await BusinessLogic.Login(loginModel.UserName, loginModel.Password);
            if (!string.IsNullOrWhiteSpace(result)) {
                await JsRuntime.InvokeVoidAsync("alert", result);
            }
        }
    }
}
