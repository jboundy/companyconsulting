namespace Billing.Domain.Models
{
    public class Address
    {
        public required string AddressOne { get; set; }
        public string? AddressTwo { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public int Zip { get; set; }
        public int? ZipExtension { get; set; }

    }
}