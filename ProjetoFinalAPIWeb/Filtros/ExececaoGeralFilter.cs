using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MySqlConnector;

namespace ProjetoFinalAPIWeb.Filtros
{
    public class ExececaoGeralFilter: ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            ProblemDetails problem = new()
            {
                Status = 500,
                Title = "Erro Inesparado",
                Detail = "Ocorreu um erro inesperado",
                Type = context.Exception.GetType().Name
            };

            switch(context.Exception)
            {
                case MySqlException:
                    problem.Title = "Ocorreu no banco de dados";
                    break;
                default:
                    context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    
                    break;
            
            }
            context.Result = new ObjectResult(problem);
        }
    }
}
