namespace Project.DigitalMarket.Application.Contract.DTOs.Business.Product
{
    public sealed class CategoryTreeReqDto
    {
        public bool? IncludeDisabled { get; set; }
    }

    public sealed class CategoryNodeDto
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Slug { get; set; }
        public int Level { get; set; }
        public string? ParentId { get; set; }
        public bool IsLeaf { get; set; }
        public int? SortOrder { get; set; }
        public List<CategoryNodeDto>? Children { get; set; }
    }

    public sealed class CategoryTreeResDto
    {
        public List<CategoryNodeDto> Categories { get; set; } = new();
    }
}
