using Microsoft.AspNetCore.Mvc;
using ProjetoFinalAPIWeb.Infra.Data;
using ProjetoFinalAPIWeb.Service.Interface;
using ProjetoFinalAPIWeb.Service.Entity;
using Microsoft.AspNetCore.Authorization;
using ProjetoFinalAPIWeb.Filtros;
using ProjetoFinalAPIWeb.Service.Enum;

namespace ProjetoFinalAPIWeb.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class CityEventController: ControllerBase
    {
             
        private readonly ICityEventService _service;

        public CityEventController(ICityEventService service)
        {            
            _service = service;
        }

        #region Gerenciar Eventos
        
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidarPrecoFilter]
        [ValidarDataEventoFilter]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AdicionarEvento(CityEventEntity cityEvent)
        {
                        
            if(!_service.ValidarPreco(cityEvent.Price))
            {
                return BadRequest(new
                {
                    sucess = false,
                    errors = "Preco Invalido: Apenas valores igual ou maior que 0"
                });
            }
            if(!await _service.AdicionarEvento(cityEvent))
            {
                return BadRequest();
            }

            return Created("Evento Adicionado", null);
            
        }

        [HttpPut("{idEvent}")]
        [Authorize(Roles = "admin")]
        [ValidarPrecoFilter]
        [ValidarDataEventoFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> AtualizarEvento(long idEvent, CityEventEntity cityEvent)
        {            

            if(!await _service.AtualizarEvento(idEvent, cityEvent))
            {
                return NotFound();
            }           

            return NoContent();
        }

        [HttpDelete("{idEvent}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]       
        public async Task<ActionResult> Excluir(long idEvent)
        {
            var result = await _service.ExcluirOuInativar(idEvent);
            if(result == RetornoDeleteEvent.NaoEncontrado)
            {
                return NotFound();
            }

            return Ok(result.ToString());
        }


        #endregion

        #region Filtros

        //// Parametros [FromQuery] do tipo DateTime, se não passados virão como DateTime.MinValue
        [HttpGet("filtrarLocalData")]
        [AllowAnonymous]
        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<CityEventEntity>>> FiltrarPorLocalData(string local, DateTime data)
        {

            if(data == DateTime.MinValue) // Se não passou pelo menos uma data
            {
                return BadRequest();
            }
                 
            return Ok(await _service.ObterPorLocalData(local, data));
        }

        [HttpGet("filtrarPrecoData")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<CityEventEntity>>> FiltrarPorPrecolData(decimal precoMaximo, decimal precoMinimo, DateTime data)
        {            
            if(data == DateTime.MinValue) // Se não passou pelo menos uma data
            {
                return BadRequest();
            }                 
            return Ok(await _service.ObterPorPrecoData(precoMaximo, precoMinimo, data));
        }

        [HttpGet("filtrarTitulo")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CityEventEntity>>> FiltrarPorTitulo(string termo)
        {
            Console.WriteLine("Controller " + termo);
            return Ok(await _service.ObterPorTitulo(termo));
        }     

        #endregion
    }

}

//Para o CityEvent, construa os métodos:

//Inclusão de um novo evento; ok *Autenticação e Autorização admin
//Edição de um evento existente, filtrando por id; ok *Autenticação e Autorização admin
//Remoção de um evento, caso o mesmo não possua reservas em andamento, caso possua inative-o; ok*Autenticação e Autorização admin
//Consulta por título, utilizando similaridades, por exemplo, caso pesquise Show, traga todos os eventos que possuem a palavra Show no título ok;
//Consulta por local e data ok;
//Consulta por range de preço e a data ok;