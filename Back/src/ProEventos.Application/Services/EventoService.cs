using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain.Models;
using ProEventos.Presistence.Contratos;

namespace ProEventos.Application.Services
{
    public class EventoService : IEventoService
    {
        private readonly IEventosPresist _evento;
        private readonly IGeralPersist _geral;
        private readonly IMapper _mapper;
        public EventoService(IEventosPresist evento, IGeralPersist geral, IMapper mapper)
        {
            this._geral = geral;
            this._evento = evento;
            _mapper = mapper;
        }
        public async Task<EventoDto> AddEvento(int userId, EventoDto  Model)
        {
            try
            {
                var evento = _mapper.Map<Evento>(Model);
                Model.UserId = userId;
                _geral.add<Evento>(evento);
                if (await _geral.SaveChangeAsync())
                {
                    return _mapper.Map<EventoDto>( await _evento.GetEventoByIdAsync(userId,evento.Id, false));
                }
                return null;

            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<EventoDto> UpdateEvento(int userId, int EventoId, EventoDto Model)
        {
            try
            {
                var evento = await _evento.GetEventoByIdAsync(userId,EventoId, false);

                if (evento == null)
                {
                    return null;
                }
                Model.Id = evento.Id;
                Model.UserId = userId;
                _mapper.Map(Model,evento);
                _geral.Update(evento);

                if (await _geral.SaveChangeAsync())
                {
                    return _mapper.Map<EventoDto>(await _evento.GetEventoByIdAsync(userId,Model.Id, false));
                }
                return null;

            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> DeleteEvento(int userId, int eventoId)
        {
            try
            {
                var evento = await _evento.GetEventoByIdAsync(userId,eventoId, false);

                if (evento == null)
                {
                    throw new Exception("Evento para delete não encontrado.");
                }
                _geral.Delete<Evento>(evento);

                return await _geral.SaveChangeAsync();

            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]> GetAllEventosAsync(int userId, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _evento.GetAllEventosAsync(userId,includePalestrantes);
                if (eventos == null) return null;
                return _mapper.Map<EventoDto[]>(eventos);;
            }
            catch (System.Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]> GetAllEventosByTemaAsync(int userId, string Tema, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _evento.GetAllEventosByTemaAsync(userId,Tema, includePalestrantes);
                if (eventos == null) return null;
                return _mapper.Map<EventoDto[]>(eventos);;
            }
            catch (System.Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> GetEventoByIdAsync(int userId, int EventoId, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _evento.GetEventoByIdAsync(userId,EventoId, includePalestrantes);
                if (eventos == null) return null;
                return _mapper.Map<EventoDto>(eventos);
            }
            catch (System.Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}