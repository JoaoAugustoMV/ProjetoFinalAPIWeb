using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProjetoFinalAPIWeb.Service.Entity;

namespace ProjetoFinalAPIWeb.Filtros
{
    public class ValidarDataEventoFilter: Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {            
            
            CityEventEntity evento = (CityEventEntity) context.ActionArguments["cityEvent"];
            if(evento.DateHourEvent < DateTime.Now)
            {
                ProblemDetails problemDetails = new()
                {
                    Status = 400,
                    Title = "Data Invalida",
                    Detail = "Não é possivel ter eventos com datas ultrapassadas"
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
