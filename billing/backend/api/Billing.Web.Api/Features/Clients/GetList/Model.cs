namespace Billing.Web.Api.Features.Clients.GetList
{
    public sealed class Response
    {
        public IReadOnlyList<ClientDTO> Clients { get; set; } = [];
    }

    public sealed class ClientDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}