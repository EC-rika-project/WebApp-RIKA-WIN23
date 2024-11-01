
namespace Infrastructure.Dtos
{
    public class ProductDetailsDto
    {
        public List<string> ImageUrls { get; set; } = [];
        public string ImageUrl { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Ingress { get; set; } = null!;
        public string Description { get; set; } = null!;

        public string Color { get; set; } = null!;
        public decimal Price { get; set; } 

        public string CategoryName { get; set; } = null!; 
        public string ArticleNumber { get; set; } = null!; 


        public List<ProductVariation>? Variations { get; set; }
    }

    public class ProductVariation
    {
        public string? Name { get; set; } // size
        public int Stock { get; set; } // ?????

        // color & articlenumber ??
    }
}
