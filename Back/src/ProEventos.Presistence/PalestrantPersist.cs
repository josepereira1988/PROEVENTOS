using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Models;
using ProEventos.Presistence.Data;
using ProEventos.Presistence.Contratos;
using ProEventos.Presistence.Models;

namespace ProEventos.Presistence
{
    public class PalestrantPersist : GeralPersist, IPalestrantPersist
    {
        private readonly ProEventosContext _context;
        public PalestrantPersist(ProEventosContext context) : base(context)
        {
            _context = context;
        }
        public async Task<PageList<Palestrante>> GetAllPalestrantesAsync(PageParams pageParams,bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.Include(e => e.RedeSociais).Include(p => p.User);

            if (includeEventos)
            {
                query = query.Include(e => e.PalestranteEventos).ThenInclude(e => e.Evento);
            }
            query = query.AsTracking().Where(p => (p.MiniCurriculo.ToLower().Contains(pageParams.Term.ToLower()) ||
                                      p.User.PrimeiroNome.ToLower().Contains(pageParams.Term.ToLower()) ||
                                      p.User.UltimoNome.ToLower().Contains(pageParams.Term.ToLower())) &&
                                      p.User.Funcao == Domain.Enum.Funcao.Palestrante)
            .OrderBy(e => e.Id);
            return await PageList<Palestrante>.CreateAsync(query, pageParams.PageNumber, pageParams.pageSize);
        }
    
        public async Task<Palestrante> GetPalestranteByIdAsync(int userId, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.Include(e => e.RedeSociais).Include(p => p.User);

            if (includeEventos)
            {
                query = query.Include(e => e.PalestranteEventos).ThenInclude(e => e.Evento);
            }
            query = query.OrderBy(e => e.Id).Where(p => p.UserId == userId);
            return await query.FirstAsync();
        }
    }
}