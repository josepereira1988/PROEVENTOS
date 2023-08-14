using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Application.Dtos;
using ProEventos.Domain.Models;
using ProEventos.Presistence.Models;

namespace ProEventos.Application.Contratos
{
    public interface IEventoService
    {
        Task<EventoDto> AddEvento(int userId, EventoDto Model);
        Task<EventoDto> UpdateEvento(int userId, int EventoId,EventoDto Model);
        Task<bool> DeleteEvento(int userId, int eventoId);
        Task<PageList<EventoDto>> GetAllEventosAsync(int userId,PageParams pageParams, bool includePalestrantes = false);      
        Task<EventoDto> GetEventoByIdAsync(int userId, int EventoId, bool includePalestrantes = false);        

    }
}