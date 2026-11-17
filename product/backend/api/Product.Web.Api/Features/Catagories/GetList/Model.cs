
namespace Product.Web.Api.Features.Categories.GetList
{
    public sealed class Response
    {
        public IReadOnlyList<CategoryDTO> Categories { get; set; } = [];
    }

    public sealed class CategoryDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public int? ParentCategoryId { get; set; }
        public bool IsActive { get; set; }
    }
}

