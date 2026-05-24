using IntlShipment.Data;
using IntlShipment.Helpers;
using IntlShipment.Models;
using Microsoft.EntityFrameworkCore;

namespace IntlShipment.Services
{
    public class ShipmentService : IShipmentService
    {
        private readonly AppDbContext _db;
        public ShipmentService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Response<Shipment>> Create(Shipment shipment)
        {
            var response = new Response<Shipment>();
            try
            {
                if (await _db.Shipments.AnyAsync(s => s.NumeroGuia == shipment.NumeroGuia))
                {
                    response.Message = "El número de guía ya existe.";
                    response.Success = false;
                    return response;
                }
                _db.Shipments.Add(shipment);
                await _db.SaveChangesAsync();
                response.Data = shipment;
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = "Ocurrió un error interno.";
                response.Success = false;
                return response;
            }
        }

        public async Task<Response<PaginatedList<Shipment>>> GetAll(int pageIndex = 1, int pageSize = 10, string? estado = null)
        {
            var response = new Response<PaginatedList<Shipment>>();
            try
            {
                var query = _db.Shipments.AsQueryable();

                if (!string.IsNullOrEmpty(estado))
                    query = query.Where(s => s.Estado == estado);

                var paginatedList = await query.ToPaginatedListAsync(pageIndex, pageSize);
                response.Data = paginatedList;
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = "Ocurrió un error interno en el servidor.";
                response.Success = false;
                return response;
            }
        }

        public async Task<Response<Shipment>> GetById(int id)
        {
            var response = new Response<Shipment>();
            try
            {
                var shipment = await _db.Shipments.FirstOrDefaultAsync(s => s.Id == id);
                if (shipment is null)
                {
                    response.Message = "No se encontró el envío con el ID especificado.";
                    response.Success = false;
                    return response;
                }
                response.Data = shipment;
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = "Ocurrió un error interno.";
                response.Success = false;
                return response;
            }
        }

        public async Task<Response<Shipment>> Update(int id, Shipment shipment)
        {
            var response = new Response<Shipment>();
            try
            {
                var existing = await _db.Shipments.FindAsync(id);
                if (existing is null)
                {
                    response.Message = "No se encontró el envío con el ID especificado.";
                    response.Success = false;
                    return response;
                }


                if (existing.Estado == "Entregado" || existing.Estado == "Cancelado")
                {
                    response.Message = $"No se puede modificar un envío en estado '{existing.Estado}'.";
                    response.Success = false;
                    return response;
                }
                existing.NumeroGuia = shipment.NumeroGuia;
                existing.PaisOrigen = shipment.PaisOrigen;
                existing.PaisDestino = shipment.PaisDestino;
                existing.CiudadOrigen = shipment.CiudadOrigen;
                existing.CiudadDestino = shipment.CiudadDestino;
                existing.NombreRemitente = shipment.NombreRemitente;
                existing.NombreDestinatario = shipment.NombreDestinatario;
                existing.DescripcionMercancia = shipment.DescripcionMercancia;
                existing.PesoKg = shipment.PesoKg;
                existing.Estado = shipment.Estado;
                existing.FechaEstimadaEntrega = shipment.FechaEstimadaEntrega;

                await _db.SaveChangesAsync();
                response.Data = existing;
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = "Ocurrió un error interno.";
                response.Success = false;
                return response;
            }
        }

        public async Task<Response<Shipment>> Cancel(int id)
        {
            var response = new Response<Shipment>();
            try
            {
                var shipment = await _db.Shipments.FindAsync(id);
                if (shipment is null)
                {
                    response.Message = "No se encontró el envío con el ID especificado.";
                    response.Success = false;
                    return response;
                }
                shipment.Estado = "Cancelado";
                await _db.SaveChangesAsync();
                response.Data = shipment;
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = "Ocurrió un error interno.";
                response.Success = false;
                return response;
            }
        }

        public async Task<Response<Shipment>> Delete(int id)
        {
            var response = new Response<Shipment>();
            try
            {
                var shipment = await _db.Shipments.FindAsync(id);
                if (shipment is null)
                {
                    response.Message = "No se encontró el envío con el ID especificado.";
                    response.Success = false;
                    return response;
                }
                _db.Shipments.Remove(shipment);
                await _db.SaveChangesAsync();
                response.Data = shipment;
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = "Ocurrió un error interno.";
                response.Success = false;
                return response;
            }
        }

        public async Task<Response<Shipment>> UpdateState(int id, string estado)
        {
            var response = new Response<Shipment>();
            try
            {
                var shipment = await _db.Shipments.FindAsync(id);
                if (shipment is null)
                {
                    response.Message = "No se encontró el envío con el ID especificado.";
                    response.Success = false;
                    return response;
                }

                var estadosValidos = new[] { "Creado", "En tránsito", "Entregado", "Cancelado" };
                if (!estadosValidos.Contains(estado))
                {
                    response.Message = "Estado no válido.";
                    response.Success = false;
                    return response;
                }

                shipment.Estado = estado;
                await _db.SaveChangesAsync();
                response.Data = shipment;
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = "Ocurrió un error interno.";
                response.Success = false;
                return response;
            }
        }
    }
}
