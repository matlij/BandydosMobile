using System;

namespace Bandydos.Dto
{
    public class AddressDto
    {
        public Guid Id { get; set; }
        public string FullAddress { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
