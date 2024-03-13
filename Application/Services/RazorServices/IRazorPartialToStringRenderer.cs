using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace Application.Services.RazorServices;

public interface IRazorPartialToStringRenderer
{
     Task<string> RenderPartialToStringAsync<TModel>(string partialName, TModel model);
     IView FindView(ActionContext actionContext, string partialName);
}