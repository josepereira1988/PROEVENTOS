using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain.Models;

namespace ProEventos.Presistence.Contratos
{
    public interface IGeralPersist
    {
        //Geral
        void add<T> (T entity) where T: class;
        void Update<T> (T entity) where T: class;
        void Delete<T> (T entity) where T: class;
        void DeleteRange<T> (T[] entityArray) where T: class;
        Task<bool> SaveChangeAsync();

    }
}