using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Contratos;
using Microsoft.AspNetCore.Http;
using ProEventos.Application.Dtos;
using ProEventos.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using ProEventos.Presistence.Models;
using ProEventos.API.Helpers;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        private readonly IEventoService _eventoService;
         private readonly IUtil _util;
        private readonly IAccountService _accountService;
        private readonly string _destino = "Images";
        public EventoController(IEventoService eventoService, 
                                                IUtil util,
                                                IAccountService accountService)
        {
            _eventoService = eventoService;
            _util = util;
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]PageParams pageParams)
        {
            try
            {
                var eventos = await _eventoService.GetAllEventosAsync(User.GetUserId(),pageParams,true);
                
                if (eventos == null) return NoContent();
                Response.AddPagination(eventos.CurrentPage,eventos.PageSize,eventos.TotalCount,eventos.TotalPages);

                return Ok(eventos);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, bool includePalestrantes)
        {
            try
            {
                var eventos = await _eventoService.GetEventoByIdAsync(User.GetUserId(),id, includePalestrantes);
                if (eventos == null) return NoContent();
                return Ok(eventos);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model)
        {
            try
            {
                var eventos = await _eventoService.AddEvento(User.GetUserId(),model);
                if (eventos == null) return BadRequest("Erro ao tentar adicionar evento.");
                return Ok(eventos);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar adicionar eventos. Erro: {ex.Message}");
            }
        }

        [HttpPost("upload-image/{eventoId}")]
        public async Task<IActionResult> uploadimage(int eventoId)
        {
            try
            {
                var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(),eventoId);
                if (evento == null) return NoContent();

                var file = Request.Form.Files[0];
                if (file.Length > 0)
                {
                    _util.DeleteImage(evento.ImagemURL,_destino);
                    evento.ImagemURL = await _util.SaveImage(file,_destino);
                }
                var EventoRetorno = await _eventoService.UpdateEvento(User.GetUserId(),eventoId, evento);
                return Ok(evento);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar adicionar eventos. Erro: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EventoDto model)
        {
            try
            {
                var eventos = await _eventoService.UpdateEvento(User.GetUserId(),id, model);
                if (eventos == null) return BadRequest("Erro ao atualizar evento");
                return Ok(eventos);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar atualizar eventos. Erro: {ex.Message}");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> delete(int id)
        {
            try
            {
                var eventos = await _eventoService.GetEventoByIdAsync(User.GetUserId(),id, false);
                if (eventos == null) return NoContent();
                if (await _eventoService.DeleteEvento(User.GetUserId(),id))
                {
                    _util.DeleteImage(eventos.ImagemURL,_destino);
                    return Ok(new { menssagem = "Deletado" });
                }
                else
                {
                    throw new Exception("Evento não deletado");

                }

            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar deletar eventos. Erro: {ex.Message}");
            }
        }        
    }
}