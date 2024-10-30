
namespace Infrastructure.Dtos
{
    public class ProductsDto
    {
        public string ArticleNumber { get; set; } = null!; // id
        public string ImageUrl { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Ingress { get; set; } = null!;
        public string Price { get; set; } = null!;
       
    }
}
