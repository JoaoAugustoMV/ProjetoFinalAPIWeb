using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProjetoFinalAPIWeb.Service.Entity;

namespace ProjetoFinalAPIWeb.Filtros
{
    public class ValidarPrecoFilter: Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {                                    
            CityEventEntity evento = (CityEventEntity) context.ActionArguments["cityEvent"];
            if(evento.Price < 0)
            {
                ProblemDetails problemDetails = new()
                {
                    Status = 400,
                    Title = "Preco Invalido",
                    Detail = "Não é possivel ter eventos com precos menores que zero"
                };
                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Result = new ObjectResult(problemDetails);
            }            
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {                                    
            
        }

       
    }
}
