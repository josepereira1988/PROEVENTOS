using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain.Models;
using ProEventos.Presistence.Models;

namespace ProEventos.Presistence.Contratos
{
    public interface IEventosPresist
    {
           //Eventos
        //Task<PageList<Evento>> GetAllEventosByTemaAsync(int userId,PageParams pageParams, string Tema, bool includePalestrantes = false);
        Task<PageList<Evento>> GetAllEventosAsync(int userId,PageParams pageParams,bool includePalestrantes = false);
        Task<Evento> GetEventoByIdAsync(int userId, int EventoId, bool includePalestrantes = false);        

    }
}