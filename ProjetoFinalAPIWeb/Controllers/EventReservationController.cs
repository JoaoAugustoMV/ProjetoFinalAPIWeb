using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Logging;
using ProjetoFinalAPIWeb.Infra.Data;
using ProjetoFinalAPIWeb.Service.DTO;
using ProjetoFinalAPIWeb.Service.Entity;
using ProjetoFinalAPIWeb.Service.Interface;

namespace ProjetoFinalAPIWeb.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class EventReservationController: ControllerBase
    {        
        private readonly IEventReservationService _serviceReservation ;
        private readonly ICityEventService _serviceEvent ;
        

        public EventReservationController(IEventReservationService service, ICityEventService cityEventService)
        {            
            _serviceReservation = service;
            _serviceEvent = cityEventService;
        }

        #region Gerenciar Reservas

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]       
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]     
        public async Task<ActionResult> AdicionarReserva(EventReservationEntity eventReservation)
        {
            if(!await _serviceEvent.EventoExiste(eventReservation.IdEvent)){
                return BadRequest(new
                {
                    sucess = false,
                    errors = "Evento não existe"
                }) ;
            }
            if(!await _serviceEvent.EventoDisponivel(eventReservation.IdEvent)){
                return BadRequest(new
                {
                    sucess = false,
                    errors = "Evento não disponivel"
                }) ;
            }
            if(!_serviceReservation.ValidarQuantidade(eventReservation.Quantity)){
                return BadRequest(new
                {
                    sucess = false,
                    errors = "Deve ter pelo menos 1 reserva"
                }) ;
            }
            
            return Created("Reserva Feita", await _serviceReservation.AdicionarReserva(eventReservation));
        }

        [HttpPut("{idReserva}/{quantidade}")]
        [Authorize(Roles = "admin")]        
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> AtualizarQuantidadeReserva(long idReserva, long quantidade)
        {
            if(!await _serviceReservation.AtualizarQuantidadeReserva(idReserva, quantidade))
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpDelete("{idReserva}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ExcluirPorId(long idReserva)
        {            

            if(!await _serviceReservation.RemoverReserva(idReserva))
            {
                return NotFound();
            }
            return NoContent();
        }

        #endregion

        #region Filtros
        [HttpGet("{nome}/{termo}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        
        public async Task<ActionResult<ReservationWithEventTitle>> FiltrarPorNomeTitulo(string nome, string termo)
        {
            var resultado = await _serviceReservation.ObterPorNomeTitulo(nome, termo);
            return Ok(resultado);
        }

        #endregion

    }
}

//Para o EventReservation, construa os métodos:

//Inclusão de uma nova reserva;ok *Autenticação
//Edição da quantidade de uma reserva; ok *Autenticação e Autorização admin
//Remoção de uma reserva; ok *Autenticação e Autorização admin
//Consulta de reserva pelo PersonName e Title do evento, utilizando similaridade para o title; ok *Autenticação