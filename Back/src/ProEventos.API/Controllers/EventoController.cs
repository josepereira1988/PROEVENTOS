using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Contratos;
using Microsoft.AspNetCore.Http;
using ProEventos.Application.Dtos;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using ProEventos.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using ProEventos.Presistence.Models;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        private readonly IEventoService _eventoService;
        private readonly IWebHostEnvironment _hostEvironment;
        private readonly IAccountService _accountService;
        public EventoController(IEventoService eventoService, 
                                                IWebHostEnvironment hostEvironment,
                                                IAccountService accountService)
        {
            _eventoService = eventoService;
            _hostEvironment = hostEvironment;
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

        // [HttpGet("{tema}/tema")]
        // public async Task<IActionResult> GetByTema(string tema, bool includePalestrantes)
        // {
        //     try
        //     {
        //         var eventos = await _eventoService.GetAllEventosByTemaAsync(User.GetUserId(),tema, includePalestrantes);
        //         if (eventos == null) return NoContent();
        //         return Ok(eventos);
        //     }
        //     catch (System.Exception ex)
        //     {
        //         return this.StatusCode(StatusCodes.Status500InternalServerError,
        //         $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
        //     }
        // }
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
                    DeleteImage(evento.ImagemURL);
                    evento.ImagemURL = await SalveImage(file);
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
                    DeleteImage(eventos.ImagemURL);
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
        [NonAction]
        public async Task<string> SalveImage(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName)
                                                        .Take(10).ToArray()).Replace(' ', '-');

            imageName = $"{imageName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(imageFile.FileName)}";
            var imagePath = Path.Combine(_hostEvironment.ContentRootPath, @"Resources/Images", imageName);

            using (var filesStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(filesStream);
            }

            return imageName;
        }

        [NonAction]
        public void DeleteImage(string imageName)
        {
            var imagePath = Path.Combine(_hostEvironment.ContentRootPath, @"Resources/images", imageName);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }
    }
}