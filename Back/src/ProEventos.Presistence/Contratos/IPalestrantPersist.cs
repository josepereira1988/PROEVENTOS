using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain.Models;
using ProEventos.Presistence.Models;

namespace ProEventos.Presistence.Contratos
{
    public interface IPalestrantPersist : IGeralPersist
    {
        //Palestrantes
        Task<PageList<Palestrante>> GetAllPalestrantesAsync(PageParams pageParams,bool includeEventos = false);
        Task<Palestrante> GetPalestranteByIdAsync(int userId, bool includeEventos = false);

    }
}