using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Dtos
{
    public class ProductResponseDto
    {
        public ProductDetailsDto? Product { get; set; }
        public List<ProductDetailsDto> Variants { get; set; } = [];
    }
}
