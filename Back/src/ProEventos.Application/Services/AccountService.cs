using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain.Identity;
using ProEventos.Presistence.Contratos;

namespace ProEventos.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IUserPersist _persist;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager,
                                IMapper mapper, IUserPersist persist)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _persist = persist;
        }

        public async Task<SignInResult> CheckUserPasswordAsync(UserUpdateDto userUpdateDto, string password)
        {
            try
            {
                var user = await _userManager.Users
                                                .SingleOrDefaultAsync(u => u.UserName.ToLower() == userUpdateDto.UserName.ToLower());
                return await _signInManager.CheckPasswordSignInAsync(user, password, false);

            }
            catch (System.Exception ex)
            {

                throw new Exception($"Erro ao tentar verificar password {ex.Message}");
            }
        }

        public async Task<UserUpdateDto> CreateAccountAsync(UserDto userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                var result = await _userManager.CreateAsync(user, userDto.Password);
                if (result.Succeeded)
                {
                    return _mapper.Map<UserUpdateDto>(user);
                }
                return null;
            }
            catch (System.Exception ex)
            {

                throw new Exception($"Erro ao tentar verificar usuario {ex.Message}");
            }
        }

        public async Task<UserUpdateDto> GetUserbyUsernameAsunc(string username)
        {
            try
            {
                var user = await _persist.GetUserByUserNameAsync(username);
                if (user == null) return null;
                return _mapper.Map<UserUpdateDto>(user);
            }
            catch (System.Exception ex)
            {

                throw new Exception($"Erro ao tentar verificar usuario {ex.Message}");
            }
        }

        public async Task<UserUpdateDto> UpdateAccount(UserUpdateDto userUpdateDto)
        {
            try
            {
                var user = await _persist.GetUserByUserNameAsync(userUpdateDto.UserName);
                if (user == null) return null;
                _mapper.Map(userUpdateDto, user);
                if (userUpdateDto.Password != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    await _userManager.ResetPasswordAsync(user, token, userUpdateDto.Password);
                }
                _persist.Update<User>(user);
                if (await _persist.SaveChangeAsync())
                {
                    return _mapper.Map<UserUpdateDto>(await _persist.GetUserByUserNameAsync(user.UserName));
                }

                return null;

            }
            catch (System.Exception ex)
            {

                throw new Exception($"Erro ao tentar atualizar usuario {ex.Message}");
            }
        }

        public async Task<bool> UserExists(string username)
        {
            try
            {
                return await _userManager.Users.AnyAsync(u => u.UserName.ToLower() == username.ToLower());
            }
            catch (System.Exception ex)
            {

                throw new Exception($"Erro ao tentar verificar usuario {ex.Message}");
            }
        }
    }
}