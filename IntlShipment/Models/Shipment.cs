namespace IntlShipment.Models
{
    public class Shipment
    {
        public int Id { get; set; }
        public string? NumeroGuia { get; set; }
        public string? PaisOrigen { get; set; }
        public string? PaisDestino { get; set; }
        public string? CiudadOrigen { get; set; }
        public string? CiudadDestino { get; set; }
        public string? NombreRemitente { get; set; }
        public string? NombreDestinatario { get; set; }
        public string? DescripcionMercancia { get; set; }
        public decimal PesoKg { get; set; }
        public string? Estado {  get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime FechaEstimadaEntrega { get; set; } = DateTime.UtcNow;


    }
}
