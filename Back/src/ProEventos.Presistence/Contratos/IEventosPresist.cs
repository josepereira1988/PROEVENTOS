using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain.Models;

namespace ProEventos.Presistence.Contratos
{
    public interface IEventosPresist
    {
       
        //Eventos
        Task<Evento[]> GetAllEventosByTemaAsync(int userId, string Tema, bool includePalestrantes = false);
        Task<Evento[]> GetAllEventosAsync(int userId,bool includePalestrantes = false);
        Task<Evento> GetEventoByIdAsync(int userId, int EventoId, bool includePalestrantes = false);        

    }
}