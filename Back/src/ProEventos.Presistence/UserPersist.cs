using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Identity;
using ProEventos.Presistence.Contratos;
using ProEventos.Presistence.Data;

namespace ProEventos.Presistence
{
    public class UserPersist : GeralPersist ,IUserPersist
    {
        private readonly ProEventosContext _context;
        public UserPersist(ProEventosContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.Where(u => u.Id == id).SingleOrDefaultAsync();
        }

        public async Task<User> GetUserByUserNameAsync(string username)
        {
            return await _context.Users.Where(u => u.UserName.ToLower() == username.ToLower()).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        
    }
}