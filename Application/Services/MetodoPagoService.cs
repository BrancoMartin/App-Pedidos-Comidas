using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Domain.Entities;
using Domain.Enum;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Application.Services
{
    public class MetodoPagoService : IMetodoPagoService
    {

        private readonly IMetodoPagoRepository _metodoPagoRepository;
        private readonly IUserRepository _userRepository;


        public MetodoPagoService(IMetodoPagoRepository pagoRepository, IUserRepository userRepository)
        {
            _metodoPagoRepository = pagoRepository;
            _userRepository = userRepository;
        }

        public async Task<MetodoPagoDto?> GetMetodoPagoById(int id)
        {
            var metodoPago = await _metodoPagoRepository.GetByIdAsync(id);
            if (metodoPago == null)
            {
                throw new NotFoundException($"Metodo pago con id:{id} no fue encontrado.");
            }

            return MetodoPagoDto.CreateMetodoPago(metodoPago);
        }

        public async Task<IEnumerable<MetodoPagoDto>> GetAllMetodoPago()
        {
            var metodoPago = await _metodoPagoRepository.GetAllAsync();
            return MetodoPagoDto.CreateList(metodoPago.ToList());
        }

        public async Task<MetodoPagoDto> CreateMetodoPago(CreationMetodoPagoDto metodoPagoDto)
        {
            var usuario = await _userRepository.GetByIdAsync(metodoPagoDto.UsuarioId);

            if (usuario == null)
            {
                throw new NotFoundException($"Usuario con id:{metodoPagoDto.UsuarioId} no fue encontrado.");
            }

            var newMetodoPago = new MetodoPago();

            if (
                    ((metodoPagoDto.TipoMetodo == TipoMetodoPago.TarjetaCredito) || (metodoPagoDto.TipoMetodo == TipoMetodoPago.TarjetaDebito)))
            {
                if (string.IsNullOrWhiteSpace(metodoPagoDto.Tarjeta.NumeroTarjeta))
                {
                    throw new Exception("Debe ingresar el numero de la tarjeta");
                }
                else if (string.IsNullOrWhiteSpace(metodoPagoDto.Tarjeta.NombreTitular))
                {
                    throw new Exception("Debe ingresar el nombre del titular");
                }
                else if (string.IsNullOrWhiteSpace(metodoPagoDto.Tarjeta.FechaExpiracion))
                {
                    throw new Exception("Debe ingresar la fecha de expiracion");
                }
                else if (string.IsNullOrWhiteSpace(metodoPagoDto.Tarjeta.CodigoSeguridad))
                {
                    throw new Exception("Debe ingresar el codigo de seguridad");
                }


                // Valida si la tarjeta esta vencida

                var partes = metodoPagoDto.Tarjeta.FechaExpiracion.Split('/');
                int mes = int.Parse(partes[0]);
                int anio = int.Parse("20" + partes[1]); // concatena el 20 con el mes para pasarlo a formato DateTime

                // daysInMonth devuelve el numero de dias que tiene el mes y DateTime crea una fecha con el año mes y dia que le pasemos

                var fechaVencimiento = new DateTime(anio, mes, DateTime.DaysInMonth(anio, mes)); // ultimo dia de vencimiento de la tarjeta

                if (fechaVencimiento < DateTime.Today)
                {
                    throw new Exception("La tarjeta está vencida");
                }

                var tarjeta = JsonSerializer.Serialize(new
                {
                    NumeroTarjeta = metodoPagoDto.Tarjeta.NumeroTarjeta,
                    NombreTitular = metodoPagoDto.Tarjeta.NombreTitular,
                    FechaExpiracion = metodoPagoDto.Tarjeta.FechaExpiracion,
                    CodigoSeguridad = metodoPagoDto.Tarjeta.CodigoSeguridad
                });

                newMetodoPago = new MetodoPago
                {
                    UsuarioId = metodoPagoDto.UsuarioId,
                    TipoMetodo = metodoPagoDto.TipoMetodo,
                    Tarjeta = tarjeta,
                    Detalles = metodoPagoDto.Detalles
                };

            }
            else
            {
                newMetodoPago = new MetodoPago
                {
                    UsuarioId = metodoPagoDto.UsuarioId,
                    TipoMetodo = metodoPagoDto.TipoMetodo,
                    Tarjeta = null,
                    Detalles = metodoPagoDto.Detalles
                };
            }

            var createMetodoPago = await _metodoPagoRepository.CreateAsync(newMetodoPago);

            return MetodoPagoDto.CreateMetodoPago(createMetodoPago);
        }

        public async Task UpdateMetodoPago(int id, CreationMetodoPagoDto metodoPagoDto)
        {
            var metodoPagoToUpdate = await _metodoPagoRepository.GetByIdAsync(id);
            if (metodoPagoToUpdate == null)
            {
                throw new NotFoundException($"Método de pago con id:{id} no fue encontrado.");
            }
            metodoPagoToUpdate.TipoMetodo = metodoPagoDto.TipoMetodo;
            metodoPagoToUpdate.Detalles = metodoPagoDto.Detalles;
            await _metodoPagoRepository.UpdateAsync(metodoPagoToUpdate);
        }

        public async Task DeleteMetodoPago(int id)
        {
            var metodoPagoToDelete = await _metodoPagoRepository.GetByIdAsync(id);
            if (metodoPagoToDelete == null)
            {
                throw new NotFoundException($"Método de pago con id:{id} no fue encontrado.");
            }
            await _metodoPagoRepository.DeleteAsync(metodoPagoToDelete);
        }

        public async Task<IEnumerable<MetodoPagoDto>> GetMetodoPagoByUserIdAsync(int usuarioId)
        {
            var metodoPagos = await _metodoPagoRepository.GetMetodoPagoByUserIdAsync(usuarioId);
            return MetodoPagoDto.CreateList(metodoPagos.ToList());
        }
    }
}
