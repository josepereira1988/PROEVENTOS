using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Application.Dtos;
using ProEventos.Domain.Models;

namespace ProEventos.Application.Contratos
{
    public interface IEventoService
    {
        Task<EventoDto> AddEvento(EventoDto Model);
        Task<EventoDto> UpdateEvento(int EventoId,EventoDto Model);
        Task<bool> DeleteEvento(int eventoId);
        Task<EventoDto[]> GetAllEventosAsync(bool includePalestrantes = false);
        Task<EventoDto[]> GetAllEventosByTemaAsync(string Tema, bool includePalestrantes = false);        
        Task<EventoDto> GetEventoByIdAsync(int EventoId, bool includePalestrantes = false);        

    }
}