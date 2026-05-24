using FluentValidation;
using IntlShipment.Data;
using IntlShipment.Models;

namespace IntlShipment.Validators
{
    public class ShipmentValidator : AbstractValidator<Shipment>
    {
        private readonly AppDbContext _db;

        public ShipmentValidator(AppDbContext db)
        {
            _db = db;

            RuleFor(x => x.NumeroGuia)
                .NotEmpty().WithMessage("El número de guía es obligatorio.")
                .MaximumLength(50).WithMessage("El número de guía no puede superar 50 caracteres.");

            RuleFor(x => x.PaisOrigen)
                .NotEmpty().WithMessage("El país de origen es obligatorio.");

            RuleFor(x => x.PaisDestino)
                .NotEmpty().WithMessage("El país de destino es obligatorio.")
                .NotEqual(x => x.PaisOrigen).WithMessage("El país de destino no puede ser igual al país de origen.");

            RuleFor(x => x.CiudadOrigen)
                .NotEmpty().WithMessage("La ciudad de origen es obligatoria.");

            RuleFor(x => x.CiudadDestino)
                .NotEmpty().WithMessage("La ciudad de destino es obligatoria.");

            RuleFor(x => x.NombreRemitente)
                .NotEmpty().WithMessage("El nombre del remitente es obligatorio.");

            RuleFor(x => x.NombreDestinatario)
                .NotEmpty().WithMessage("El nombre del destinatario es obligatorio.");

            RuleFor(x => x.PesoKg)
                .GreaterThan(0).WithMessage("El peso debe ser mayor a cero.");

            RuleFor(x => x.FechaEstimadaEntrega)
                .GreaterThanOrEqualTo(x => x.FechaCreacion)
                .WithMessage("La fecha estimada de entrega no puede ser menor a la fecha de creación.");

            RuleFor(x => x.Estado)
                .NotEmpty().WithMessage("El estado es obligatorio.")
                .Must(e => new[] { "Pendiente", "En tránsito", "Entregado", "Cancelado" }.Contains(e))
                .WithMessage("Estado no válido. Use: Pendiente, En tránsito, Entregado o Cancelado.");
        }

    }
}
