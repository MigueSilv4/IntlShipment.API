using IntlShipment.Helpers;
using IntlShipment.Models;

namespace IntlShipment.Services
{
    public interface IShipmentService
    {
        Task<Response<PaginatedList<Shipment>>> GetAll(int pageIndex = 1, int pageSize = 10, string estado = null);
        Task<Response<Shipment>> Create(Shipment shipment);
        Task<Response<Shipment>> GetById(int id);
        Task<Response<Shipment>> Update(int id, Shipment shipment);
        Task<Response<Shipment>> UpdateState(int id, string estado);
        Task<Response<Shipment>> Cancel(int id);
        Task<Response<Shipment>> Delete(int id);

    }
}
