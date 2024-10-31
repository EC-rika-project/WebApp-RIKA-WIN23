
namespace Infrastructure.Dtos
{
    public class ProductDetailsDto
    {
        public List<string> ImageUrls { get; set; } = [];
        public string Name { get; set; } = null!;
        public string? Ingress { get; set; } = null!;
        public string Description { get; set; } = null!;

        public string Color { get; set; } = null!;
        public string Price { get; set; } = null!; // ??

        public string CategoryName { get; set; } = null!; // behövs väl inte?
        public string ArticleNumber { get; set; } = null!; // id???


        public List<ProductVariation>? Variations { get; set; }
    }

    public class ProductVariation
    {
        public string? Name { get; set; }
        public int Stock { get; set; } // ?????

        // color & articlenumber ??
    }
}
