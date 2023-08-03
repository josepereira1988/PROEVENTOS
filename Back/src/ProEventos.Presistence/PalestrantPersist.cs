using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Models;
using ProEventos.Presistence.Data;
using ProEventos.Presistence.Contratos;

namespace ProEventos.Presistence
{
    public class PalestrantPersist : IPalestrantPersist
    {
        private readonly ProEventosContext _context;
        public PalestrantPersist(ProEventosContext context)
        {
            _context = context;
        }
        public async Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.Include(e => e.RedeSociais);

            if (includeEventos)
            {
                query = query.Include(e => e.PalestranteEventos).ThenInclude(e => e.Evento);
            }
            query = query.OrderBy(e => e.Id);
            return await query.ToArrayAsync();
        }
        public async Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string Nome, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.Include(e => e.RedeSociais);

            if (includeEventos)
            {
                query = query.Include(e => e.PalestranteEventos).ThenInclude(e => e.Evento);
            }
            query = query.OrderBy(e => e.Id).Where(p => p.User.PrimeiroNome == Nome && p.User.UltimoNome == Nome);
            return await query.ToArrayAsync();
        }
        public async Task<Palestrante> GetPalestranteByIdAsync(int PalestranteId, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.Include(e => e.RedeSociais);

            if (includeEventos)
            {
                query = query.Include(e => e.PalestranteEventos).ThenInclude(e => e.Evento);
            }
            query = query.OrderBy(e => e.Id).Where(p => p.Id == PalestranteId);
            return await query.FirstAsync();
        }
    }
}