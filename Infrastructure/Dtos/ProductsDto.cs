
namespace Infrastructure.Dtos
{
    public class ProductsDto
    {
        public string ArticleNumber { get; set; } = null!; // id
        public string CoverImageUrl { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Ingress { get; set; } = null!;
        public decimal Price { get; set; } 
    }
}
