using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventos.API.Extensions;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;
        private readonly ITokenService _tokenService;

        public AccountController(IAccountService service, ITokenService tokenService)
        {
            _service = service;
            _tokenService = tokenService;
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var userName = User.GetUserName();
                var user = await _service.GetUserbyUsernameAsunc(userName);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar Usuário. Erro: {ex.Message}");
            }
        }
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            try
            {
                if (await _service.UserExists(userDto.UserName))
                    return BadRequest("Usuário já existe");

                var user = await _service.CreateAccountAsync(userDto);
                if (user != null)
                    return Ok(new
                    {
                        userName = user.UserName,
                        PrimeroNome = user.PrimeiroNome,
                        token = _tokenService.CreateToken(user).Result
                    });

                return BadRequest("Usuário não criado, tente novamente mais tarde!");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar Registrar Usuário. Erro: {ex.Message}");
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userLogin)
        {
            try
            {
                if (userLogin.UserName != null && userLogin.UserName != "")
                {
                    var user = await _service.GetUserbyUsernameAsunc(userLogin.UserName);
                    if (user == null) return Unauthorized("Usuário ou Senha está errado");

                    var result = await _service.CheckUserPasswordAsync(user, userLogin.Password);
                    if (!result.Succeeded) return Unauthorized();

                    return Ok(new
                    {
                        userName = user.UserName,
                        PrimeroNome = user.PrimeiroNome,
                        token = _tokenService.CreateToken(user).Result
                    });

                }
                else
                {
                    return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Login em branco.");
                }

            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar realizar Login. Erro: {ex.Message}");
            }
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserUpdateDto userUpdateDto)
        {
            try
            {
                if (userUpdateDto.UserName != User.GetUserName())
                    return Unauthorized("Usuário Inválido");

                var user = await _service.GetUserbyUsernameAsunc(User.GetUserName());
                if (user == null) return Unauthorized("Usuário Inválido");

                userUpdateDto.Id = user.Id;
                
                var userReturn = await _service.UpdateAccount(userUpdateDto);
                if (userReturn == null) return NoContent();
                return Ok(userReturn);

                return Ok(new
                {
                    userName = userReturn.UserName,
                    PrimeroNome = userReturn.PrimeiroNome,
                    token = _tokenService.CreateToken(userReturn).Result
                });
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar Atualizar Usuário. Erro: {ex.Message}");
            }
        }

        // [HttpPost("upload-image")]
        // public async Task<IActionResult> UploadImage()
        // {
        //     try
        //     {
        //         var user = await _service.GetUserbyUsernameAsunc(User.GetUserName());
        //         if (user == null) return NoContent();

        //         var file = Request.Form.Files[0];
        //         if (file.Length > 0)
        //         {
        //             _util.DeleteImage(user.ImagemURL, _destino);
        //             user.ImagemURL = await _util.SaveImage(file, _destino);
        //         }
        //         var userRetorno = await _accountService.UpdateAccount(user);

        //         return Ok(userRetorno);
        //     }
        //     catch (Exception ex)
        //     {
        //         return this.StatusCode(StatusCodes.Status500InternalServerError,
        //             $"Erro ao tentar realizar upload de Foto do Usuário. Erro: {ex.Message}");
        //     }
        // }

    }
}