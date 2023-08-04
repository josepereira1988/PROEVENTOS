using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Models;
using ProEventos.Presistence.Data;
using ProEventos.Presistence.Contratos;

namespace ProEventos.Presistence
{
    public class EventosPresist : IEventosPresist
    {

        private readonly ProEventosContext _context;
        public EventosPresist(ProEventosContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        public async Task<Evento[]> GetAllEventosAsync(int userId, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos.Include(e => e.Lotes).Include(e => e.RedeSociais);

            if (includePalestrantes)
            {
                query = query.Include(e => e.PalestranteEventos).ThenInclude(e => e.Palestrante);
            }
            query = query.Where(e => e.UserId == userId).OrderBy(e => e.Id);
            return await query.ToArrayAsync();
        }
        public async Task<Evento[]> GetAllEventosByTemaAsync(int userId,string Tema, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos.Include(e => e.Lotes).Include(e => e.RedeSociais);

            if (includePalestrantes)
            {
                query = query.Include(e => e.PalestranteEventos).ThenInclude(e => e.Palestrante);
            }
            query = query.OrderBy(e => e.Id).Where(e => e.Tema.ToLower().Contains(Tema.ToLower()) && e.UserId == userId);
            return await query.ToArrayAsync();
        }
        public async Task<Evento> GetEventoByIdAsync(int userId, int EventoId, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos.Include(e => e.Lotes).Include(e => e.RedeSociais);

            if (includePalestrantes)
            {
                query = query.Include(e => e.PalestranteEventos).ThenInclude(e => e.Palestrante);
            }
            query = query.OrderBy(e => e.Id).Where(e => e.Id == EventoId && e.UserId == userId);
            return await query.FirstAsync();
        }
    }
}