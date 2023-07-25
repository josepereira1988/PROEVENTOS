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
        Task<Evento[]> GetAllEventosByTemaAsync(string Tema, bool includePalestrantes = false);
        Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false);
        Task<Evento> GetEventoByIdAsync(int EventoId, bool includePalestrantes = false);        

    }
}